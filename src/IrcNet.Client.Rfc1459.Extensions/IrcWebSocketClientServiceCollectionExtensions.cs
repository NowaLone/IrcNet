﻿using IrcNet.Parser.Rfc1459;
using Microsoft.Extensions.DependencyInjection;
using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace IrcNet.Client.Extensions
#pragma warning restore IDE0130 // Namespace does not match folder structure
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