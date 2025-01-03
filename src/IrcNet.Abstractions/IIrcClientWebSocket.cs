using IrcNet.EventsArgs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IrcNet
{
	/// <summary>
	/// Interface for IRC client WebSocket.
	/// </summary>
	public interface IIrcClientWebSocket
	{
		#region Events

		/// <summary>
		/// Occurs when the client is connected.
		/// </summary>
		event EventHandler<ConnectedEventArgs> OnConnected;

		/// <summary>
		/// Occurs when the connection state changes.
		/// </summary>
		event EventHandler<ConnectionStateChangedEventArgs> OnConnectionStateChanged;

		/// <summary>
		/// Occurs when the client is disconnected.
		/// </summary>
		event EventHandler<DisconnectedEventArgs> OnDisconnected;

		/// <summary>
		/// Occurs when a message is received.
		/// </summary>
		event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

		/// <summary>
		/// Occurs when a message is sent.
		/// </summary>
		event EventHandler<MessageSentEventArgs> OnMessageSent;

		#endregion Events

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the client is connected.
		/// </summary>
		bool IsConnected { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Closes the WebSocket connection asynchronously.
		/// </summary>
		/// <param name="closeMode">The mode to use when closing the connection.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous close operation.</returns>
		Task CloseAsync(CloseMode closeMode = CloseMode.Irc, CancellationToken cancellationToken = default);

		/// <summary>
		/// Opens the WebSocket connection asynchronously.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous open operation.</returns>
		Task OpenAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Sends a message asynchronously.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous send operation.</returns>
		Task SendAsync(byte[] message, CancellationToken cancellationToken = default);

		/// <summary>
		/// Sends a message asynchronously.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous send operation.</returns>
		Task SendAsync(string message, CancellationToken cancellationToken = default);

		#endregion Methods
	}
}