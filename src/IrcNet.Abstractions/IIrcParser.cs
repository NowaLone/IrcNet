namespace IrcNet
{
	/// <summary>
	/// Interface for parsing and building IRC messages and prefixes.
	/// </summary>
	/// <typeparam name="T">Type of the IRC message.</typeparam>
	public interface IIrcParser<T> where T : IIrcMessage
	{
		/// <summary>
		/// Parse raw IRC message.
		/// </summary>
		/// <param name="message">The raw IRC message to parse.</param>
		/// <returns>The parsed IRC message.</returns>
		T ParseMessage(string message);

		/// <summary>
		/// Parse raw IRC prefix.
		/// </summary>
		/// <param name="message">The raw IRC message with prefix to parse.</param>
		/// <returns>The parsed IRC prefix.</returns>
		IrcPrefix ParsePrefix(string message);

		/// <summary>
		/// Build raw IRC message from <see cref="IIrcMessage"/> instance.
		/// </summary>
		/// <param name="message">The IRC message instance to build from.</param>
		/// <param name="useNumeric">Indicates whether to use numeric representation.</param>
		/// <returns>The built raw IRC message.</returns>
		string BuildMessage(T message, bool useNumeric = false);

		/// <summary>
		/// Build raw IRC prefix from <see cref="IrcPrefix"/> instance.
		/// </summary>
		/// <param name="ircPrefix">The IRC prefix instance to build from.</param>
		/// <returns>The built raw IRC prefix.</returns>
		string BuildPrefix(IrcPrefix ircPrefix);
	}
}