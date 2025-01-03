using System;

namespace IrcNet.EventsArgs
{
	/// <summary>
	/// Provides data for the Disconnected event.
	/// </summary>
	public class DisconnectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisconnectedEventArgs"/> class.
		/// </summary>
		/// <param name="connectionUrl">The URL of the connection that was disconnected.</param>
		public DisconnectedEventArgs(Uri connectionUrl)
		{
			ConnectionUrl = connectionUrl;
		}

		/// <summary>
		/// Gets the URL of the connection that was disconnected.
		/// </summary>
		public Uri ConnectionUrl { get; }
	}
}