using System.Collections.Generic;

namespace IrcNet.Parser.Rfc1459
{
	/// <summary>
	/// Represents an IRC message.
	/// </summary>
	public class IrcMessage : IIrcMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IrcMessage"/> class.
		/// </summary>
		public IrcMessage()
		{
			Parameters = new List<string>();
		}

		/// <inheritdoc/>
		public string Raw { get; set; }

		/// <inheritdoc/>
		public IrcPrefix Prefix { get; set; }

		/// <inheritdoc/>
		public IrcCommand Command { get; set; }

		/// <inheritdoc/>
		public ICollection<string> Parameters { get; set; }

		/// <summary>
		/// Gets a value indicating whether the message contains a trailing parameter.
		/// </summary>
		public bool WithTrailing => Raw.IndexOf(" :") > 0;
	}
}