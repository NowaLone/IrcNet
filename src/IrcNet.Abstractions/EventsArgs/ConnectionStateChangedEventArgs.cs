using System;
using System.Net.WebSockets;

namespace IrcNet.EventsArgs
{
	/// <summary>
	/// Provides data for the connection state changed event.
	/// </summary>
	public class ConnectionStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionStateChangedEventArgs"/> class.
		/// </summary>
		/// <param name="currentState">The current state of the WebSocket connection.</param>
		/// <param name="lastState">The previous state of the WebSocket connection.</param>
		public ConnectionStateChangedEventArgs(WebSocketState currentState, WebSocketState lastState)
		{
			CurrentState = currentState;
			LastState = lastState;
		}

		/// <summary>
		/// Gets the current state of the WebSocket connection.
		/// </summary>
		public WebSocketState CurrentState { get; }

		/// <summary>
		/// Gets the previous state of the WebSocket connection.
		/// </summary>
		public WebSocketState LastState { get; }
	}
}