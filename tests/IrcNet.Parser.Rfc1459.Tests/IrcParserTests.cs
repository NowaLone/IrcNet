using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IrcNet.Parser.Rfc1459.Tests
{
	[TestClass]
	[TestCategory(nameof(IrcParser))]
	public class IrcParserTests
	{
		#region ParseMessage

		[DataTestMethod]
		[DynamicData(nameof(ParseTestData))]
		[TestCategory(nameof(IrcParser.ParseMessage))]
		public void ParseMessage_WithCorrectData_ShouldParse(string message, IrcMessage expected, bool useNumeric)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.ParseMessage(message);

			// Assert
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Command, result.Command);
			CollectionAssert.AreEqual((ICollection)expected.Parameters, (ICollection)result.Parameters);
			Assert.AreEqual(expected.WithTrailing, result.WithTrailing);
			Assert.AreEqual(expected.Prefix?.Host, result.Prefix?.Host);
			Assert.AreEqual(expected.Prefix?.Nick, result.Prefix?.Nick);
			Assert.AreEqual(expected.Prefix?.Raw, result.Prefix?.Raw);
			Assert.AreEqual(expected.Prefix?.Server, result.Prefix?.Server);
			Assert.AreEqual(expected.Prefix?.User, result.Prefix?.User);
		}

		[TestMethod]
		[TestCategory(nameof(IrcParser.ParseMessage))]
		public void ParseMessage_WithUnknownCommand_ShouldParse()
		{
			// Arrange
			var parser = new IrcParser();

			var expected = new IrcMessage
			{
				Raw = "UNKWN :tmi.twitch.tv\r\n",
				Command = IrcCommand.UNKNOWN,
				Prefix = null,
				Parameters = new List<string>
				{
					":tmi.twitch.tv"
				},
			};

			// Act
			var result = parser.ParseMessage(expected.Raw);

			// Assert
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Command, result.Command);
			CollectionAssert.AreEqual((ICollection)expected.Parameters, (ICollection)result.Parameters);
			Assert.AreEqual(expected.WithTrailing, result.WithTrailing);
			Assert.AreEqual(expected.Prefix, result.Prefix);
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("  ")]
		[DataRow("\t")]
		[DataRow("\r")]
		[DataRow("\n")]
		[DataRow("\r\n")]
		[DataRow(null)]
		[TestCategory(nameof(IrcParser.ParseMessage))]
		public void ParseMessage_EmptyValues_ShouldThrow(string message)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			Action action = new Action(() => parser.ParseMessage(message));

			// Assert
			Assert.ThrowsException<ArgumentException>(action, "'message' cannot be null or whitespace.");
		}

		#endregion ParseMessage

		#region ParsePrefix

		[DataTestMethod]
		[DynamicData(nameof(ParsePrefixTestData))]
		public void ParsePrefix_WithCorrectData_ShouldParse(string prefix, IrcPrefix expected)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.ParsePrefix(prefix);

			// Assert
			Assert.AreEqual(expected.Host, result.Host);
			Assert.AreEqual(expected.Nick, result.Nick);
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Server, result.Server);
			Assert.AreEqual(expected.User, result.User);
		}

		[DataTestMethod]
		[DataRow("PING :tmi.twitch.tv\r\n")]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("  ")]
		[DataRow("\t")]
		[DataRow("\r")]
		[DataRow("\n")]
		[DataRow("\r\n")]
		[DataRow(null)]
		public void ParsePrefix_EmptyValues_ShouldThrow(string prefix)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			Action action = new Action(() => parser.ParsePrefix(prefix));

			// Assert
			Assert.ThrowsException<ArgumentException>(action, "Message must start with colon ':' character!");
		}

		#endregion ParsePrefix

		#region BuildMessage

		[DataTestMethod]
		[DynamicData(nameof(ParseTestData))]
		[TestCategory(nameof(IrcParser.BuildMessage))]
		public void BuildMessage_WithCorrectData_ShouldParseAndBuildBack(string expected, IrcMessage message, bool useNumeric)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.BuildMessage(message, useNumeric);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		[TestCategory(nameof(IrcParser.BuildMessage))]
		public void BuildMessage_WithNullValue_ShouldReturnEmpty()
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.BuildMessage(null);

			// Assert
			Assert.AreEqual(string.Empty, result);
		}

		#endregion BuildMessage

		#region BuildPrefix

		[DataTestMethod]
		[DynamicData(nameof(ParsePrefixTestData))]
		[TestCategory(nameof(IrcParser.BuildPrefix))]
		public void BuildPrefix_WithCorrectData_ShouldParseAndBuildBack(string expected, IrcPrefix prefix)
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.BuildPrefix(prefix);

			// Assert
			Assert.AreEqual(expected.Substring(0, expected.IndexOf(' ')), result);
		}

		[TestMethod]
		[TestCategory(nameof(IrcParser.BuildPrefix))]
		public void BuildPrefix_WithNullValue_ShouldReturnEmpty()
		{
			// Arrange
			var parser = new IrcParser();

			// Act
			var result = parser.BuildPrefix(null);

			// Assert
			Assert.AreEqual(string.Empty, result);
		}

		#endregion BuildPrefix

		private static IEnumerable<object[]> ParseTestData
		{
			get
			{
				return new[]
				{
					new object[]
					{
						":ronni!ronni@ronni.tmi.twitch.tv JOIN #dallas\r\n",
						new IrcMessage
						{
							Raw=":ronni!ronni@ronni.tmi.twitch.tv JOIN #dallas\r\n",
							Command = IrcCommand.JOIN,
							Prefix = new IrcPrefix
							{
								Raw="ronni!ronni@ronni.tmi.twitch.tv",
								Host = "ronni.tmi.twitch.tv",
								User = "ronni",
								Nick = "ronni"
							},
							Parameters = new List<string>
							{
								"#dallas"
							},
						},
						false
					},
					new object[]
					{
						":ronni!ronni@ronni.tmi.twitch.tv PART #dallas\r\n",
						new IrcMessage
						{
							Raw=":ronni!ronni@ronni.tmi.twitch.tv PART #dallas\r\n",
							Command = IrcCommand.PART,
							Prefix = new IrcPrefix
							{
								Raw="ronni!ronni@ronni.tmi.twitch.tv",
								Host = "ronni.tmi.twitch.tv",
								User = "ronni",
								Nick = "ronni"
							},
							Parameters = new List<string>
							{
								"#dallas"
							},
						},
						false
					},
					new object[]
					{
						":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :go dbd?\r\n",
						new IrcMessage
						{
							Raw = ":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :go dbd?\r\n",
							Command = IrcCommand.PRIVMSG,
							Prefix = new IrcPrefix
							{
								Raw = "twitchdev!twitchdev@twitchdev.tmi.twitch.tv",
								Host = "twitchdev.tmi.twitch.tv",
								User = "twitchdev",
								Nick = "twitchdev"
							},
							Parameters = new List<string>
							{
								"#lovingt3s",
								":go dbd?"
							},
						},
						false
					},
					new object[]
					{
						":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :Kappa https://example.com/\r\n",
						new IrcMessage
						{
							Raw = ":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :Kappa https://example.com/\r\n",
							Command = IrcCommand.PRIVMSG,
							Prefix = new IrcPrefix
							{
								Raw = "twitchdev!twitchdev@twitchdev.tmi.twitch.tv",
								Host = "twitchdev.tmi.twitch.tv",
								User = "twitchdev",
								Nick = "twitchdev"
							},
							Parameters = new List<string>
							{
								"#lovingt3s",
								":Kappa https://example.com/"
							},
						},
						false
					},
					new object[]
					{
						"PING :tmi.twitch.tv\r\n",
						new IrcMessage
						{
							Raw = "PING :tmi.twitch.tv\r\n",
							Command = IrcCommand.PING,
							Prefix = null,
							Parameters = new List<string>
							{
								":tmi.twitch.tv"
							},
						},
						false
					},
					new object[]
					{
						":tmi.twitch.tv 002 justinfan123 :Your host is tmi.twitch.tv\r\n",
						new IrcMessage
						{
							Raw = ":tmi.twitch.tv 002 justinfan123 :Your host is tmi.twitch.tv\r\n",
							Command = (IrcCommand)2,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"justinfan123",
								":Your host is tmi.twitch.tv"
							},
						},
						true
					},
					new object[]
					{
						":tmi.twitch.tv 004 justinfan123 :-\r\n",
						new IrcMessage
						{
							Raw = ":tmi.twitch.tv 004 justinfan123 :-\r\n",
							Command = (IrcCommand)4,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"justinfan123",
								":-"
							},
						},
						true
					},
					new object[]
					{
						":justinfan123.tmi.twitch.tv 353 justinfan123 = #xemdo :streamelements xemdo lovingt3s lovingt3s2 twitchdev twitch foo bar\r\n",
						new IrcMessage
						{
							Raw = ":justinfan123.tmi.twitch.tv 353 justinfan123 = #xemdo :streamelements xemdo lovingt3s lovingt3s2 twitchdev twitch foo bar\r\n",
							Command = IrcCommand.RPL_NAMREPLY,
							Prefix = new IrcPrefix
							{
								Raw = "justinfan123.tmi.twitch.tv",
								Server = "justinfan123.tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"justinfan123",
								"=",
								"#xemdo",
								":streamelements xemdo lovingt3s lovingt3s2 twitchdev twitch foo bar"
							},
						},
						true
					},
					new object[]
					{
						":tmi.twitch.tv CAP * ACK :twitch.tv/tags\r\n",
						new IrcMessage
						{
							Raw = ":tmi.twitch.tv CAP * ACK :twitch.tv/tags\r\n",
							Command = IrcCommand.CAP,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"*",
								"ACK",
								":twitch.tv/tags"
							},
						},
						false
					},
					new object[]
					{
						"983 :info\r\n",
						new IrcMessage
						{
							Raw = "983 :info\r\n",
							Command = (IrcCommand)983,
							Prefix = null,
							Parameters = new List<string>
							{
								":info"
							},
						},
						true
					}
				};
			}
		}

		private static IEnumerable<object[]> ParsePrefixTestData
		{
			get
			{
				return new[]
				{
					new object[]
					{
						":justinfan123!justinfan1234@justinfan12345.tmi.twitch.tv JOIN #lovingt3s\r\n",
						new IrcPrefix
						{
							Host = "justinfan12345.tmi.twitch.tv",
							Nick = "justinfan123",
							Raw = "justinfan123!justinfan1234@justinfan12345.tmi.twitch.tv",
							User = "justinfan1234"
						}
					},
					new object[]
					{
						":justinfan123!justinfan1234 JOIN #lovingt3s\r\n",
						new IrcPrefix
						{
							Nick = "justinfan123",
							Raw = "justinfan123!justinfan1234",
							User = "justinfan1234"
						}
					},
					new object[]
					{
						":justinfan123@justinfan12345.tmi.twitch.tv JOIN #lovingt3s\r\n",
						new IrcPrefix
						{
							Host = "justinfan12345.tmi.twitch.tv",
							Nick = "justinfan123",
							Raw = "justinfan123@justinfan12345.tmi.twitch.tv",
						}
					},
					new object[]
					{
						":justinfan123.tmi.twitch.tv 366 justinfan123 #lovingt3s :End of /NAMES list\r\n",
						new IrcPrefix
						{
							Raw = "justinfan123.tmi.twitch.tv",
							Server = "justinfan123.tmi.twitch.tv"
						}
					}
				};
			}
		}
	}
}