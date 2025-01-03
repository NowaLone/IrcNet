using System;

namespace IrcNet.EventsArgs
{
	/// <summary>
	/// Provides data for the Connected event.
	/// </summary>
	public class ConnectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectedEventArgs"/> class.
		/// </summary>
		/// <param name="connectionUrl">The URL of the connection.</param>
		public ConnectedEventArgs(Uri connectionUrl)
		{
			ConnectionUrl = connectionUrl;
		}

		/// <summary>
		/// Gets the URL of the connection.
		/// </summary>
		public Uri ConnectionUrl { get; }
	}
}