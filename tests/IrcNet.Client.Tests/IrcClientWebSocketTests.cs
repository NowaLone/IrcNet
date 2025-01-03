using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;
using static IrcNet.Client.IrcClientWebSocket;

namespace IrcNet.Client.Tests
{
	[TestClass]
	[TestCategory(nameof(IrcClientWebSocket))]
	public class IrcClientWebSocketTests
	{
		private ILoggerFactory loggerFactory;

		public TestContext TestContext { get; set; }

		[TestInitialize]
		public void Initialize()
		{
			TestContext.CancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
			loggerFactory = LoggerFactory.Create(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
		}

		[TestCleanup]
		public void Cleanup()
		{
			loggerFactory.Dispose();
		}

		#region SendAsync

		[TestMethod]
		[TestCategory(nameof(IrcClientWebSocket.SendAsync))]
		public async Task SendAsync()
		{
			// Arrange
			var options = new Options() { Uri = new Uri("wss://irc-ws.chat.twitch.tv:443"), PingDelay = TimeSpan.FromSeconds(1) };
			var client = new IrcClientWebSocket(options, loggerFactory.CreateLogger<IrcClientWebSocket>());

			await client.OpenAsync();

			// Act & Assert
			await client.SendAsync(Encoding.UTF8.GetBytes($"CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"), TestContext.CancellationTokenSource.Token);
			await client.SendAsync(Encoding.UTF8.GetBytes($"PASS oauth:yfvzjqb705z12hrhy1zkwa9xt7v662"), TestContext.CancellationTokenSource.Token);
			await client.SendAsync(Encoding.UTF8.GetBytes($"NICK justinfan123"), TestContext.CancellationTokenSource.Token);

			await Task.Delay(3000);
			await client.CloseAsync(CloseMode.Irc, TestContext.CancellationTokenSource.Token);
		}

		#endregion SendAsync

		#region CloseAsync

		[TestMethod]
		[TestCategory(nameof(IrcClientWebSocket.CloseAsync))]
		public async Task CloseAsync_NotForce_ShouldCloseNormaly()
		{
			// Arrange
			var options = new Options() { Uri = new Uri("wss://irc-ws.chat.twitch.tv:443"), PingDelay = TimeSpan.FromSeconds(1) };
			var client = new IrcClientWebSocket(options, loggerFactory.CreateLogger<IrcClientWebSocket>());

			await client.OpenAsync();

			// Act & Assert
			await Task.Delay(3000);
			await client.CloseAsync(CloseMode.Irc, TestContext.CancellationTokenSource.Token);
		}

		[TestMethod]
		[TestCategory(nameof(IrcClientWebSocket.CloseAsync))]
		public async Task CloseAsync_Force_ShouldCloseNormaly()
		{
			// Arrange
			var options = new Options() { Uri = new Uri("wss://irc-ws.chat.twitch.tv:443"), PingDelay = TimeSpan.FromSeconds(1) };
			var client = new IrcClientWebSocket(options, loggerFactory.CreateLogger<IrcClientWebSocket>());

			await client.OpenAsync();

			// Act & Assert
			await Task.Delay(3000);
			await client.CloseAsync(CloseMode.Force, TestContext.CancellationTokenSource.Token);
		}

		#endregion CloseAsync
	}
}