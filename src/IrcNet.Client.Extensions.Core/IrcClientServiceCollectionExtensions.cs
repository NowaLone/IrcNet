using IrcNet;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class IrcClientServiceCollectionExtensions
	{
		public static IServiceCollection AddIrcWebSocketClient<TClient, TOptions, TParser, TMessage>(this IServiceCollection services, Action<TOptions> setupAction = null)
			where TClient : class, IIrcClientWebSocket
			where TOptions : class
			where TParser : class, IIrcParser<TMessage>
			where TMessage : class, IIrcMessage
		{
			services.TryAddTransient<IIrcClientWebSocket, TClient>();
			services.TryAddTransient(s => s.GetRequiredService<IOptions<TOptions>>().Value);
			services.TryAddTransient<IIrcParser<TMessage>, TParser>();

			services.Configure(setupAction);

			return services;
		}
	}
}