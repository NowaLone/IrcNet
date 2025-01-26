using IrcNet.Client.Extensions;
using IrcNet.Parser.V3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IrcNet.Client.V3.Extensions.Tests
{
	[TestClass]
	[TestCategory(nameof(IrcWebSocketClientServiceCollectionExtensions))]
	public sealed class IrcWebSocketClientServiceCollectionExtensionsTests
	{
		private ServiceProvider provider;

		[TestCleanup]
		public void Cleanup()
		{
			provider.Dispose();
		}

		#region AddIrcWebSocketClient

		[TestMethod]
		[TestCategory(nameof(IrcWebSocketClientServiceCollectionExtensions.AddIrcWebSocketClient))]
		public void AddIrcWebSocketClient_WithSetupAction_ShouldAddServicesAndSetup()
		{
			// Arrange
			var serviceDescriptors = new ServiceCollection();
			var pingDelay = TimeSpan.FromSeconds(new Random().Next(50));
			var action = new Action<IrcClientWebSocket.Options>((o) => o.PingDelay = pingDelay);

			// Act
			provider = serviceDescriptors.AddIrcWebSocketClient(action).BuildServiceProvider();

			// Assert
			Assert.IsNotNull(provider.GetService<IIrcClientWebSocket>());
			Assert.IsNotNull(provider.GetServices<IIrcParser<IrcV3Message>>());
			Assert.IsNotNull(provider.GetService<IOptionsMonitor<IrcClientWebSocket.Options>>());
			Assert.AreEqual(pingDelay, provider.GetService<IOptionsMonitor<IrcClientWebSocket.Options>>().CurrentValue.PingDelay);
		}

		[TestMethod]
		[TestCategory(nameof(IrcWebSocketClientServiceCollectionExtensions.AddIrcWebSocketClient))]
		public void AddIrcWebSocketClient_WithoutSetupAction_ShouldAddServicesWithoutSetup()
		{
			// Arrange
			var serviceDescriptors = new ServiceCollection();

			// Act
			provider = serviceDescriptors.AddIrcWebSocketClient().BuildServiceProvider();

			// Assert
			Assert.IsNotNull(provider.GetService<IIrcClientWebSocket>());
			Assert.IsNotNull(provider.GetServices<IIrcParser<IrcV3Message>>());
			Assert.IsNotNull(provider.GetService<IOptionsMonitor<IrcClientWebSocket.Options>>());
			Assert.AreEqual(new IrcClientWebSocket.Options().PingDelay, provider.GetService<IOptionsMonitor<IrcClientWebSocket.Options>>().CurrentValue.PingDelay);
		}

		#endregion AddIrcWebSocketClient
	}
}