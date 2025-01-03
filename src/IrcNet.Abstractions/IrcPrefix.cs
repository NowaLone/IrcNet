namespace IrcNet
{
	/// <summary>
	/// Represents the prefix of an IRC message, which can include server, nick, user, and host information.
	/// </summary>
	public class IrcPrefix
	{
		/// <summary>
		/// Gets or sets the raw prefix string.
		/// </summary>
		public string Raw { get; set; }

		/// <summary>
		/// Gets or sets the server name.
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Gets or sets the nickname.
		/// </summary>
		public string Nick { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// Gets or sets the host name.
		/// </summary>
		public string Host { get; set; }
	}
}