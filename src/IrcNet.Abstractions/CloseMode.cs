namespace IrcNet
{
	/// <summary>
	/// Specifies the mode in which a connection can be closed.
	/// </summary>
	public enum CloseMode
	{
		/// <summary>
		/// Close the connection in the IRC way using the QUIT command.
		/// </summary>
		Irc,

		/// <summary>
		/// Close the connection in the WebSocket way.
		/// </summary>
		Websocket,

		/// <summary>
		/// Forcefully close the connection.
		/// </summary>
		Force
	}
}