using IrcNet.Client;
using IrcNet.Parser.Rfc1459;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for setting up IRC WebSocket client services in an <see cref="IServiceCollection" />.
	/// </summary>
	public static class IrcWebSocketClientServiceCollectionExtensions
	{
		/// <summary>
		/// Adds an IRC WebSocket client to the service collection.
		/// </summary>
		/// <param name="services">The service collection to add the client to.</param>
		/// <param name="setupAction">An optional action to configure the client's options.</param>
		/// <returns>The updated service collection.</returns>
		public static IServiceCollection AddIrcWebSocketClient(this IServiceCollection services, Action<IrcClientWebSocket.Options> setupAction = null)
		{
			return services.AddIrcWebSocketClient<IrcClientWebSocket, IrcClientWebSocket.Options, IrcParser, IrcMessage>(setupAction);
		}
	}
}