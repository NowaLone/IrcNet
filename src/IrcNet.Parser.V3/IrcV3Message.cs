using IrcNet.Parser.Rfc1459;
using System.Collections.Generic;

namespace IrcNet.Parser.V3
{
	/// <summary>
	/// Represents an IRCv3 message with support for message tags.
	/// </summary>
	public class IrcV3Message : IrcMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IrcV3Message"/> class.
		/// </summary>
		public IrcV3Message()
		{
			Tags = new Dictionary<string, string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IrcV3Message"/> class with the specified message.
		/// </summary>
		/// <param name="message">The IRC message to initialize from.</param>
		public IrcV3Message(IIrcMessage message) : this()
		{
			Command = message.Command;
			Parameters = message.Parameters;
			Prefix = message.Prefix;
			Raw = message.Raw;
		}

		/// <summary>
		/// Gets or sets the tags associated with the IRCv3 message.
		/// </summary>
		public IDictionary<string, string> Tags { get; set; }
	}
}