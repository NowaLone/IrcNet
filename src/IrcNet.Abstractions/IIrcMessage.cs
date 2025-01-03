using System.Collections.Generic;

namespace IrcNet
{
	/// <summary>
	/// Represents an IRC message with command, parameters, prefix, and raw message.
	/// </summary>
	public interface IIrcMessage
	{
		/// <summary>
		/// Gets or sets the IRC command.
		/// </summary>
		IrcCommand Command { get; set; }

		/// <summary>
		/// Gets or sets the parameters of the IRC message.
		/// </summary>
		ICollection<string> Parameters { get; set; }

		/// <summary>
		/// Gets or sets the prefix of the IRC message.
		/// </summary>
		IrcPrefix Prefix { get; set; }

		/// <summary>
		/// Gets or sets the raw IRC message.
		/// </summary>
		string Raw { get; set; }
	}
}