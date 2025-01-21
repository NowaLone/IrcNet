using IrcNet.Client;
using IrcNet.Parser.V3;
using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	/// <summary>
	/// Extension methods for setting up IRC WebSocket client services in an <see cref="IServiceCollection" />.
	/// </summary>
	public static class IrcWebSocketClientServiceCollectionExtensions
	{
		/// <summary>
		/// Adds an IRC WebSocket client to the <see cref="IServiceCollection" />.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add the client to.</param>
		/// <param name="setupAction">An optional action to configure the client's options.</param>
		/// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
		public static IServiceCollection AddIrcWebSocketClient(this IServiceCollection services, Action<IrcClientWebSocket.Options> setupAction = null)
		{
			return services.AddIrcWebSocketClient<IrcClientWebSocket, IrcClientWebSocket.Options, IrcV3Parser, IrcV3Message>(setupAction);
		}
	}
}