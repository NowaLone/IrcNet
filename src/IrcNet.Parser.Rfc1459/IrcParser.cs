using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrcNet.Parser.Rfc1459
{
	/// <summary>
	/// Parses IRC messages according to RFC 1459.
	/// </summary>
	public class IrcParser : IIrcParser<IrcMessage>
	{
		// The BNF representation for this is:
		// <message>  ::= [':' <prefix> <SPACE> ] <command> <params> <crlf>
		// <prefix>   ::= <servername> | <nick> [ '!' <user> ] [ '@' <host> ]
		// <command>  ::= <letter> { <letter> } | <number> <number> <number>
		// <SPACE>    ::= ' ' { ' ' }
		// <params>   ::= <SPACE> [ ':' <trailing> | <middle> <params> ]
		// <middle>   ::= <Any *non-empty* sequence of octets not including SPACE or NUL or CR or LF, the first of which may not be ':'>
		// <trailing> ::= <Any, possibly *empty*, sequence of octets not including NUL or CR or LF>
		// <crlf>     ::= CR LF

		/// <summary>
		/// Parses an IRC message from a string.
		/// </summary>
		/// <param name="message">The raw IRC message string.</param>
		/// <returns>An <see cref="IrcMessage"/> object representing the parsed message.</returns>
		/// <exception cref="ArgumentException">Thrown when the message is null or whitespace.</exception>
		public virtual IrcMessage ParseMessage(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
			}

			return message.StartsWith(":") ? ParseWithPrefix(message) : ParseOnlyCommand(message);
		}

		/// <inheritdoc/>
		public virtual IrcPrefix ParsePrefix(string message)
		{
			if (string.IsNullOrWhiteSpace(message) || !message.StartsWith(":"))
			{
				throw new ArgumentException("Message must start with colon ':' character!", nameof(message));
			}

			int userIndex = message.IndexOf('!'), hostIndex = message.IndexOf('@'), commandPrefix = message.IndexOf(' ');
			commandPrefix = commandPrefix == -1 ? message.Length : commandPrefix;

			var prefix = new IrcPrefix
			{
				Raw = message.Substring(1, commandPrefix - 1),
			};

			if (userIndex > 0 && hostIndex > 0)
			{
				prefix.Nick = message.Substring(1, userIndex - 1);
				prefix.User = message.Substring(userIndex + 1, hostIndex - userIndex - 1);
				prefix.Host = message.Substring(hostIndex + 1, commandPrefix - hostIndex - 1);
			}
			else if (userIndex > 0)
			{
				prefix.Nick = message.Substring(1, userIndex - 1);
				prefix.User = message.Substring(userIndex + 1, commandPrefix - userIndex - 1);
			}
			else if (hostIndex > 0)
			{
				prefix.Nick = message.Substring(1, hostIndex - 1);
				prefix.Host = message.Substring(hostIndex + 1, commandPrefix - hostIndex - 1);
			}
			else
			{
				prefix.Server = message.Substring(1, commandPrefix - 1);
			}

			return prefix;
		}

		/// <summary>
		/// Builds an IRC message string from an <see cref="IrcMessage"/> object.
		/// </summary>
		/// <param name="message">The <see cref="IrcMessage"/> object to build the string from.</param>
		/// <param name="useNumeric">Indicates whether to use numeric command representation.</param>
		/// <returns>The raw IRC message string.</returns>
		public virtual string BuildMessage(IrcMessage message, bool useNumeric = false)
		{
			if (message is null)
			{
				return string.Empty;
			}

			var stringBuilder = new StringBuilder();

			// <prefix> ::= <servername> | <nick> [ ’!’ <user> ] [ ’@’ <host> ]
			if (message.Prefix != null)
			{
				stringBuilder.Append(BuildPrefix(message.Prefix));

				stringBuilder.Append(' ');
			}

			// <command> ::= <letter> { <letter> } | <number> <number> <number>
			stringBuilder.Append(useNumeric ? ((int)message.Command).ToString("d3") : Enum.GetName(typeof(IrcCommand), message.Command));

			// <params> ::= <SPACE> [ ’:’ <trailing> | <middle> <params> ]
			if (message.Parameters.Any())
			{
				for (int i = 0; i < message.Parameters.Count; i++)
				{
					//if (message.WithTrailing && i == message.Parameters.Count - 1)
					//{
					//	stringBuilder.Append($" :{message.Parameters.ElementAt(i)}");
					//}
					//else
					{
						stringBuilder.Append($" {message.Parameters.ElementAt(i)}");
					}
				}
			}

			// <crlf> ::= CR LF
			stringBuilder.Append("\r\n");

			return stringBuilder.ToString();
		}

		/// <inheritdoc/>
		public virtual string BuildPrefix(IrcPrefix ircPrefix)
		{
			var stringBuilder = new StringBuilder();

			// <prefix> ::= <servername> | <nick> [ ’!’ <user> ] [ ’@’ <host> ]
			if (ircPrefix != null)
			{
				stringBuilder.Append(':');
				stringBuilder.Append(ircPrefix.Server ?? ircPrefix.Nick);

				if (!string.IsNullOrEmpty(ircPrefix.User))
				{
					stringBuilder.Append($"!{ircPrefix.User}");
				}

				if (!string.IsNullOrEmpty(ircPrefix.Host))
				{
					stringBuilder.Append($"@{ircPrefix.Host}");
				}
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Parses an IRC message with a prefix.
		/// </summary>
		/// <param name="message">The raw IRC message string.</param>
		/// <returns>An <see cref="IrcMessage"/> object representing the parsed message.</returns>
		protected virtual IrcMessage ParseWithPrefix(string message)
		{
			var parts = SplitMessageParts(message);

			return new IrcMessage
			{
				Raw = message,
				Command = ParseCommand(parts.ElementAt(1)),
				Prefix = ParsePrefix(parts.ElementAt(0)),
				Parameters = parts.Skip(2).ToList(),
			};
		}

		/// <summary>
		/// Parses an IRC message without a prefix.
		/// </summary>
		/// <param name="message">The raw IRC message string.</param>
		/// <returns>An <see cref="IrcMessage"/> object representing the parsed message.</returns>
		protected virtual IrcMessage ParseOnlyCommand(string message)
		{
			var parts = SplitMessageParts(message);

			return new IrcMessage
			{
				Raw = message,
				Command = ParseCommand(parts.ElementAt(0)),
				Parameters = parts.Skip(1).ToList(),
			};
		}

		/// <summary>
		/// Parses the command part of an IRC message.
		/// </summary>
		/// <param name="command">The command string.</param>
		/// <returns>An <see cref="IrcCommand"/> enumeration value representing the command.</returns>
		protected virtual IrcCommand ParseCommand(string command)
		{
			if (char.IsDigit(command[0]) && int.TryParse(command, out int replyEnum))
			{
				return (IrcCommand)replyEnum;
			}
			else
			{
				return Enum.TryParse(command, out IrcCommand commandEnum) ? commandEnum : IrcCommand.UNKNOWN;
			}
		}

		/// <summary>
		/// Splits an IRC message string into its component parts.
		/// </summary>
		/// <param name="message">The raw IRC message string.</param>
		/// <returns>An enumerable collection of strings representing the parts of the message.</returns>
		protected virtual IEnumerable<string> SplitMessageParts(string message)
		{
			var colonIndex = message.IndexOf(" :");
			if (colonIndex > 0)
			{
				var lastSpaceIndex = message.Substring(0, colonIndex + 1).Split(new char[] { ' ' }).Length;
				return message.TrimEnd().Split(new char[] { ' ' }, lastSpaceIndex);
			}
			else
			{
				// [RFC 1459 2.3] ...The prefix, command, and all parameters are separated by one(or more) ASCII space character(s)(0x20).
				return message.TrimEnd().Split(new char[] { ' ' }, 5);
			}
		}
	}
}