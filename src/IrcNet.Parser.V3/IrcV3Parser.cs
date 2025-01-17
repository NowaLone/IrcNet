using IrcNet.Parser.Rfc1459;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IrcNet.Parser.V3
{
	/// <summary>
	/// Parses IRCv3 messages, including tags, prefix, command, and parameters.
	/// </summary>
	public class IrcV3Parser : IrcParser, IIrcParser<IrcV3Message>
	{
		/// <summary>
		/// Builds an IRCv3 message string from the given <see cref="IrcV3Message"/> object.
		/// </summary>
		/// <param name="message">The <see cref="IrcV3Message"/> object to build the message from.</param>
		/// <param name="useNumeric">A boolean indicating whether to use numeric values for commands.</param>
		/// <returns>A string representing the built IRCv3 message.</returns>
		/// <inheritdoc cref="IrcParser.BuildMessage(IrcMessage, bool)"/>
		public virtual string BuildMessage(IrcV3Message message, bool useNumeric = false)
		{
			var result = base.BuildMessage(message, useNumeric);

			return !string.IsNullOrEmpty(result) && message.Tags?.Count > 0 ? $"@{string.Join(";", message.Tags.Select(x => x.Key + "=" + x.Value))} {result}" : result;
		}

		/// <summary>
		/// Parses an IRC message, including tags if present.
		/// </summary>
		/// <param name="message">The raw IRC message.</param>
		/// <returns>An <see cref="IrcV3Message"/> object representing the parsed message.</returns>
		/// <inheritdoc cref="IrcParser.ParseMessage(string)"/>
		public new IrcV3Message ParseMessage(string message)
		{
			return message != null && message.StartsWith("@") ? ParseWithTags(message) : new IrcV3Message(base.ParseMessage(message));
		}

		/// <summary>
		/// Parses an IRCv3 message with tags.
		/// </summary>
		/// <param name="message">The raw IRC message with tags.</param>
		/// <returns>An <see cref="IrcV3Message"/> object representing the parsed message.</returns>
		protected virtual IrcV3Message ParseWithTags(string message)
		{
			var parts = SplitMessageParts(message);

			var tags = parts.ElementAt(0)
				.Remove(0, 1)
				.Split(';')
				.Select(item => item.Split(new char[] { '=' }, 2, StringSplitOptions.None))
				.Where(s => s.Length == 2)
				.ToDictionary(s => s[0], s => s[1]);

			return new IrcV3Message
			{
				Raw = message,
				Tags = tags,
				Prefix = ParsePrefix(parts.ElementAt(1)),
				Command = ParseCommand(parts.ElementAt(2)),
				Parameters = parts.Skip(3).ToList()
			};
		}

		/// <summary>
		/// Splits the IRC message into its constituent parts where first is tags.
		/// </summary>
		/// <param name="message">The raw IRC message to split.</param>
		/// <returns>An enumerable collection of message parts.</returns>
		/// <inheritdoc cref="IrcParser.SplitMessageParts(string)"/>
		protected override IEnumerable<string> SplitMessageParts(string message)
		{
			var parts = message.Split(new char[] { ' ' }, 2);

			yield return parts[0];

			foreach (string part in base.SplitMessageParts(parts[1]))
			{
				yield return part;
			}
		}
	}
}