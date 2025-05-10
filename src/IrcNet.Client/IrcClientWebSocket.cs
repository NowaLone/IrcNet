using IrcNet.EventsArgs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IrcNet.Client
{
	/// <summary>
	/// Represents a WebSocket client for IRC communication.
	/// </summary>
	public class IrcClientWebSocket : IIrcClientWebSocket
	{
		#region Fields

		private readonly Options options;
		private readonly ILogger logger;

		private ClientWebSocket client;
		private CancellationTokenSource tasksCancellationTokenSource;
		private SemaphoreSlim semaphoreSlim;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="IrcClientWebSocket"/> class.
		/// </summary>
		/// <param name="options">The options for the WebSocket client.</param>
		/// <param name="logger">The logger instance.</param>
		public IrcClientWebSocket(Options options, ILogger<IrcClientWebSocket> logger = default)
		{
			this.options = options;
			this.logger = logger ?? NullLogger<IrcClientWebSocket>.Instance;
		}

		#endregion Constructors

		#region Events

		/// <inheritdoc/>
		public event EventHandler<ConnectedEventArgs> OnConnected;

		/// <inheritdoc/>
		public event EventHandler<DisconnectedEventArgs> OnDisconnected;

		/// <inheritdoc/>
		public event EventHandler<ConnectionStateChangedEventArgs> OnConnectionStateChanged;

		/// <inheritdoc/>
		public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

		/// <inheritdoc/>
		public event EventHandler<MessageSentEventArgs> OnMessageSent;

		#endregion Events

		#region Properties

		/// <inheritdoc/>
		public bool IsConnected => client?.State == WebSocketState.Open;

		#endregion Properties

		#region Methods

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException">Thrown when the URI is not specified.</exception>
		public async Task OpenAsync(CancellationToken cancellationToken = default)
		{
			if (options.Uri is null)
			{
				throw new ArgumentNullException($"{nameof(options.Uri)} must be specified.");
			}

			if (IsConnected)
			{
				return;
			}

			tasksCancellationTokenSource = new CancellationTokenSource();
			semaphoreSlim = new SemaphoreSlim(1, 1);

			client = new ClientWebSocket();

			ConnectionMonitorTask(tasksCancellationTokenSource.Token);
			await client.ConnectAsync(options.Uri, cancellationToken).ConfigureAwait(false);
			ListenerTask(tasksCancellationTokenSource.Token);
			ReconnectTask(tasksCancellationTokenSource.Token);
			PingTask(tasksCancellationTokenSource.Token);
		}

		/// <inheritdoc/>
		public async Task CloseAsync(CloseMode closeMode = CloseMode.Irc, CancellationToken cancellationToken = default)
		{
			if (!IsConnected)
			{
				return;
			}

			// TODO: maybe move cancel operations in semaphore wait block
			// Cancel all tasks
			tasksCancellationTokenSource.Cancel();
			tasksCancellationTokenSource.Dispose();

			if (closeMode == CloseMode.Irc)
			{
				// Send QUIT irc command
				await SendAsync(new byte[4] { 81, 85, 73, 84 }, cancellationToken).ConfigureAwait(false);

				// Block send operations and waiting for closure
				await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

				while (client.State != WebSocketState.Closed && client.State != WebSocketState.Aborted)
				{
					await Task.Delay(200, cancellationToken).ConfigureAwait(false);
				}

				semaphoreSlim.Release();
			}
			else if (closeMode == CloseMode.Websocket)
			{
				// Block send operations and waiting for closure
				await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
				await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken).ConfigureAwait(false);
				semaphoreSlim.Release();
			}
			else if (closeMode == CloseMode.Force)
			{
				// Block send operations and waiting for closure
				await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

				if (IsConnected)
				{
					client.Abort();
				}

				semaphoreSlim.Release();
			}

			semaphoreSlim.Dispose();
		}

		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException">Thrown when the client is not connected.</exception>
		public async Task SendAsync(byte[] message, CancellationToken cancellationToken = default)
		{
			if (!IsConnected)
			{
				throw new InvalidOperationException("Client should be connected first!");
			}

			try
			{
				await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

				for (int i = 0; i < message.Length; i += options.MessageSize)
				{
					// Don't pass token because it aborts connection, see https://github.com/dotnet/runtime/issues/31566
					await client.SendAsync(new ArraySegment<byte>(message, i * options.MessageSize, i + options.MessageSize >= message.Length ? message.Length - i : options.MessageSize),
							   WebSocketMessageType.Text,
							   i + options.MessageSize >= message.Length,
							   CancellationToken.None).ConfigureAwait(false);
				}

				semaphoreSlim.Release();
			}
			catch (OperationCanceledException)
			{
				logger.LogInformation("A send command was canceled.");
			}

			if (logger.IsEnabled(LogLevel.Trace))
			{
				logger.LogTrace(EventIds.MessageSentEvent, "Message sent: \"{Message}\"", Encoding.UTF8.GetString(message));
			}
			else
			{
				logger.LogDebug(EventIds.MessageSentEvent, "Message sent.");
			}

			OnMessageSent?.Invoke(this, new MessageSentEventArgs(message));
		}

		/// <inheritdoc/>
		public Task SendAsync(string message, CancellationToken cancellationToken = default)
		{
			return SendAsync(Encoding.UTF8.GetBytes(message), cancellationToken);
		}

		/// <summary>
		/// Listens for incoming messages asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task ListenerTask(CancellationToken cancellationToken)
		{
			var message = Enumerable.Empty<byte>();

			while (!cancellationToken.IsCancellationRequested)
			{
				if (!IsConnected)
				{
					await Task.Delay(200, cancellationToken).ConfigureAwait(false);
					continue;
				}

				var buffer = new byte[options.MessageSize];

				try
				{
					// Don't pass token because it aborts connection, see https://github.com/dotnet/runtime/issues/31566
					var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);

					switch (result.MessageType)
					{
						case WebSocketMessageType.Close:
							logger.LogDebug(EventIds.MessageReceivedEvent, "Received end of message.");
							await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken).ConfigureAwait(false);
							break;

						case WebSocketMessageType.Text when !result.EndOfMessage:
							{
								message = message.Concat(buffer);

								if (logger.IsEnabled(LogLevel.Trace))
								{
									logger.LogTrace(EventIds.MessageReceivedEvent, "Received a message part: \"{Message}\".", Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
								}
								else
								{
									logger.LogDebug(EventIds.MessageReceivedEvent, "Received a message part.");
								}
							}
							continue;

						case WebSocketMessageType.Text:
							{
								message = message.Concat(buffer);

								if (logger.IsEnabled(LogLevel.Trace))
								{
									logger.LogTrace(EventIds.MessageReceivedEvent, "Received a full message: \"{Message}\".", Encoding.UTF8.GetString(message.ToArray()).TrimEnd('\0'));
								}
								else
								{
									logger.LogDebug(EventIds.MessageReceivedEvent, "Received a full message.");
								}

								foreach (var line in Encoding.UTF8.GetString(message.ToArray()).TrimEnd('\0').Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
								{
									OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(line));
								}

								message = Enumerable.Empty<byte>();
							}
							break;

						case WebSocketMessageType.Binary:
							{
								if (logger.IsEnabled(LogLevel.Trace))
								{
									logger.LogTrace(EventIds.MessageReceivedEvent, "Received a binary message \"{Message}\".", string.Concat(buffer));
								}
								else
								{
									logger.LogDebug(EventIds.MessageReceivedEvent, "Received a binary message.");
								}
							}
							break;

						default:
							logger.LogCritical("Unknown WebSocketMessageType: {WebSocketMessageType}", result.MessageType);
							break;
					}
				}
				catch (OperationCanceledException)
				{
					logger.LogInformation("A {taskName} was canceled.", nameof(ListenerTask));
				}
				catch (WebSocketException ex)
				{
					logger.LogError(ex, "Web Socket Error.");
				}
				catch (SocketException ex)
				{
					logger.LogError(ex, "Socket Error.");
				}
				catch (ObjectDisposedException ex)
				{
					logger.LogError(ex, "Dispose Error.");
				}
			}
		}

		/// <summary>
		/// Monitors the connection state asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task ConnectionMonitorTask(CancellationToken cancellationToken)
		{
			var lastStatus = IsConnected;
			var lastState = client.State;

			logger.LogInformation(EventIds.ConnectingEvent, "Connecting to {Url}", options.Uri.AbsoluteUri);
			OnConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(client.State, lastState));

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (lastStatus == IsConnected)
					{
						await Task.Delay(200, cancellationToken).ConfigureAwait(false);
						continue;
					}

					logger.LogInformation(EventIds.StateChangedEvent, "Connection state changed from {LastState} to {CurrentState}", lastState, client.State);
					OnConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(client.State, lastState));

					lastStatus = IsConnected;
					lastState = client.State;

					if (IsConnected)
					{
						logger.LogInformation(EventIds.ConnectedEvent, "Connected to {Url}", options.Uri.AbsoluteUri);
						OnConnected?.Invoke(this, new ConnectedEventArgs(options.Uri));
						continue;
					}

					if (!IsConnected)
					{
						logger.LogInformation(EventIds.DisconnectedEvent, "Disconnected from {Url}", options.Uri.AbsoluteUri);
						OnDisconnected?.Invoke(this, new DisconnectedEventArgs(options.Uri));
						continue;
					}
				}
				catch (OperationCanceledException)
				{
					logger.LogInformation("A {taskName} was canceled.", nameof(ConnectionMonitorTask));
				}
			}
		}

		/// <summary>
		/// Attempts to reconnect asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task ReconnectTask(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (options.AutoReconnect && !IsConnected)
					{
						client.Dispose();
						client = new ClientWebSocket();
						await client.ConnectAsync(options.Uri, cancellationToken).ConfigureAwait(false);

						if (!IsConnected)
						{
							await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
							logger.LogInformation(EventIds.ReconnectingEvent, "Unable to reconnect, retry.");
						}
					}
					else
					{
						await Task.Delay(200, cancellationToken).ConfigureAwait(false);
					}
				}
				catch (OperationCanceledException)
				{
					logger.LogInformation("A {taskName} was canceled.", nameof(ReconnectTask));
				}
				catch (WebSocketException ex)
				{
					logger.LogError(ex, "Web Socket Error.");
				}
			}
		}

		/// <summary>
		/// Sends ping messages asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task PingTask(CancellationToken cancellationToken)
		{
			var pingMessage = new byte[4] { 80, 73, 78, 71 };

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (options.PingDelay != TimeSpan.Zero)
					{
						if (IsConnected)
						{
							await SendAsync(pingMessage, cancellationToken).ConfigureAwait(false);
						}

						await Task.Delay(options.PingDelay, cancellationToken).ConfigureAwait(false);
					}
					else
					{
						await Task.Delay(200, cancellationToken).ConfigureAwait(false);
					}
				}
				catch (OperationCanceledException)
				{
					logger.LogInformation("A {taskName} was canceled.", nameof(PingTask));
				}
				catch (WebSocketException ex)
				{
					logger.LogError(ex, "Web Socket Error.");
				}
			}
		}

		#endregion Methods

		/// <summary>
		/// Represents options managed by the <see cref="IrcClientWebSocket"/>.
		/// </summary>
		public class Options
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Options"/> class with default options.
			/// </summary>
			public Options()
			{
				AutoReconnect = true;
				MessageSize = 512;
				PingDelay = TimeSpan.Zero;
			}

			/// <summary>
			/// Delay between ping messages, <see cref="TimeSpan.Zero"/> by defalut (ping messages will not be send).
			/// </summary>
			public TimeSpan PingDelay { get; set; }

			/// <summary>
			/// Specifies that the connection should automatically reconnect when the connection is lost.
			/// </summary>
			public bool AutoReconnect { get; set; }

			/// <summary>
			/// Irc message size in bytes, 512 by default.
			/// </summary>
			public int MessageSize { get; set; }

			/// <summary>
			/// Irc server address.
			/// </summary>
			public Uri Uri { get; set; }
		}
	}
}