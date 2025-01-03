using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IrcNet.Parser.V3.Tests
{
	[TestClass]
	[TestCategory(nameof(IrcV3Parser))]
	public class IrcV3ParserTests
	{
		#region ParseMessage

		[DataTestMethod]
		[DynamicData(nameof(ParseTestData))]
		[TestCategory(nameof(IrcV3Parser.ParseMessage))]
		public void ParseMessage_WithCorrectData_ShouldParse(string message, IrcV3Message expected, bool useNumeric)
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.ParseMessage(message);

			// Assert
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Command, result.Command);
			CollectionAssert.AreEqual((ICollection)expected.Parameters, (ICollection)result.Parameters);
			Assert.AreEqual(expected.WithTrailing, result.WithTrailing);
			CollectionAssert.AreEqual((ICollection)expected.Tags, (ICollection)result.Tags);
			Assert.AreEqual(expected.Prefix?.Host, result.Prefix?.Host);
			Assert.AreEqual(expected.Prefix?.Nick, result.Prefix?.Nick);
			Assert.AreEqual(expected.Prefix?.Raw, result.Prefix?.Raw);
			Assert.AreEqual(expected.Prefix?.Server, result.Prefix?.Server);
			Assert.AreEqual(expected.Prefix?.User, result.Prefix?.User);
		}

		[DataTestMethod]
		[DynamicData(nameof(ParseTestDataWithTags))]
		[TestCategory(nameof(IrcV3Parser.ParseMessage))]
		public void ParseMessage_WithCorrectDataAndTags_ShouldParse(string message, IrcV3Message expected, bool useNumeric)
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.ParseMessage(message);

			// Assert
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Command, result.Command);
			CollectionAssert.AreEqual((ICollection)expected.Parameters, (ICollection)result.Parameters);
			Assert.AreEqual(expected.WithTrailing, result.WithTrailing);
			CollectionAssert.AreEqual((ICollection)expected.Tags, (ICollection)result.Tags);
			Assert.AreEqual(expected.Prefix.Host, result.Prefix.Host);
			Assert.AreEqual(expected.Prefix.Nick, result.Prefix.Nick);
			Assert.AreEqual(expected.Prefix.Raw, result.Prefix.Raw);
			Assert.AreEqual(expected.Prefix.Server, result.Prefix.Server);
			Assert.AreEqual(expected.Prefix.User, result.Prefix.User);
		}

		[TestMethod]
		[TestCategory(nameof(IrcV3Parser.ParseMessage))]
		public void ParseMessage_WithUnknownCommand_ShouldParse()
		{
			// Arrange
			var parser = new IrcV3Parser();

			var expected = new IrcV3Message
			{
				Raw = "UNKWN :tmi.twitch.tv\r\n",
				Command = IrcCommand.UNKNOWN,
				Prefix = null,
				Parameters = new List<string>
				{
					":tmi.twitch.tv"
				},
				Tags = new Dictionary<string, string>()
			};

			// Act
			var result = parser.ParseMessage(expected.Raw);

			// Assert
			Assert.AreEqual(expected.Raw, result.Raw);
			Assert.AreEqual(expected.Command, result.Command);
			CollectionAssert.AreEqual((ICollection)expected.Parameters, (ICollection)result.Parameters);
			Assert.AreEqual(expected.WithTrailing, result.WithTrailing);
			CollectionAssert.AreEqual((ICollection)expected.Tags, (ICollection)result.Tags);
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
		[TestCategory(nameof(IrcV3Parser.ParseMessage))]
		public void ParseMessage_EmptyValues_ShouldThrow(string message)
		{
			// Arrange
			var parser = new IrcV3Parser();

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
			var parser = new IrcV3Parser();

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
			var parser = new IrcV3Parser();

			// Act
			Action action = new Action(() => parser.ParsePrefix(prefix));

			// Assert
			Assert.ThrowsException<ArgumentException>(action, "Message must start with colon ':' character!");
		}

		#endregion ParsePrefix

		#region BuildMessage

		[DataTestMethod]
		[DynamicData(nameof(ParseTestData))]
		[TestCategory(nameof(IrcV3Parser.BuildMessage))]
		public void BuildMessage_WithCorrectData_ShouldParseAndBuildBack(string expected, IrcV3Message message, bool useNumeric)
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.BuildMessage(message, useNumeric);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[DataTestMethod]
		[DynamicData(nameof(ParseTestDataWithTags))]
		[TestCategory(nameof(IrcV3Parser.BuildMessage))]
		public void BuildMessage_WithCorrectDataAndTags_ShouldParseAndBuildBack(string expected, IrcV3Message message, bool useNumeric)
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.BuildMessage(message, useNumeric);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		[TestCategory(nameof(IrcV3Parser.BuildMessage))]
		public void BuildMessage_WithNullValue_ShouldReturnEmpty()
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.BuildMessage(null);

			// Assert
			Assert.AreEqual(string.Empty, result);
		}

		[TestMethod]
		[TestCategory(nameof(IrcV3Parser.BuildMessage))]
		public void BuildMessage_WithNullTags_ShouldReturnWithoutTags()
		{
			// Arrange
			var parser = new IrcV3Parser();

			var message = new IrcV3Message()
			{
				Command = IrcCommand.UNKNOWN,
				Tags = null,
			};

			// Act
			var result = parser.BuildMessage(message);

			// Assert
			Assert.AreEqual("UNKNOWN\r\n", result);
		}

		#endregion BuildMessage

		#region BuildPrefix

		[DataTestMethod]
		[DynamicData(nameof(ParsePrefixTestData))]
		[TestCategory(nameof(IrcV3Parser.BuildPrefix))]
		public void BuildPrefix_WithCorrectData_ShouldParseAndBuildBack(string expected, IrcPrefix prefix)
		{
			// Arrange
			var parser = new IrcV3Parser();

			// Act
			var result = parser.BuildPrefix(prefix);

			// Assert
			Assert.AreEqual(expected.Substring(0, expected.IndexOf(' ')), result);
		}

		[TestMethod]
		[TestCategory(nameof(IrcV3Parser.BuildPrefix))]
		public void BuildPrefix_WithNullValue_ShouldReturnEmpty()
		{
			// Arrange
			var parser = new IrcV3Parser();

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
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						":ronni!ronni@ronni.tmi.twitch.tv PART #dallas\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :go dbd?\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						":twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #lovingt3s :Kappa https://example.com/\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						"PING :tmi.twitch.tv\r\n",
						new IrcV3Message
						{
							Raw = "PING :tmi.twitch.tv\r\n",
							Command = IrcCommand.PING,
							Prefix = null,
							Parameters = new List<string>
							{
								":tmi.twitch.tv"
							},
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						":tmi.twitch.tv 002 justinfan123 :Your host is tmi.twitch.tv\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						true
					},
					new object[]
					{
						":tmi.twitch.tv 004 justinfan123 :-\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						true
					},
					new object[]
					{
						":justinfan123.tmi.twitch.tv 353 justinfan123 = #xemdo :streamelements xemdo lovingt3s lovingt3s2 twitchdev twitch foo bar\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						true
					},
					new object[]
					{
						":tmi.twitch.tv CAP * ACK :twitch.tv/tags\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>()
						},
						false
					},
					new object[]
					{
						"983 :info\r\n",
						new IrcV3Message
						{
							Raw = "983 :info\r\n",
							Command = (IrcCommand)983,
							Prefix = null,
							Parameters = new List<string>
							{
								":info"
							},
							Tags = new Dictionary<string, string>()
						},
						true
					}
				};
			}
		}

		private static IEnumerable<object[]> ParseTestDataWithTags
		{
			get
			{
				return new[]
				{
					new object[]
					{
						"@badge-info=;badges=broadcaster/1;client-nonce=459e3142897c7a22b7d275178f2259e0;color=#0000FF;display-name=lovingt3s;emote-only=1;emotes=62835:0-10;first-msg=0;flags=;id=885196de-cb67-427a-baa8-82f9b0fcd05f;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1643904084794;turbo=0;user-id=713936733;user-type= :lovingt3s!lovingt3s@lovingt3s.tmi.twitch.tv PRIVMSG #lovingt3s :bleedPurple\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=broadcaster/1;client-nonce=459e3142897c7a22b7d275178f2259e0;color=#0000FF;display-name=lovingt3s;emote-only=1;emotes=62835:0-10;first-msg=0;flags=;id=885196de-cb67-427a-baa8-82f9b0fcd05f;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1643904084794;turbo=0;user-id=713936733;user-type= :lovingt3s!lovingt3s@lovingt3s.tmi.twitch.tv PRIVMSG #lovingt3s :bleedPurple\r\n",
							Command = IrcCommand.PRIVMSG,
							Prefix = new IrcPrefix
							{
								Raw = "lovingt3s!lovingt3s@lovingt3s.tmi.twitch.tv",
								Host = "lovingt3s.tmi.twitch.tv",
								User = "lovingt3s",
								Nick = "lovingt3s"
							},
							Parameters = new List<string>
							{
								"#lovingt3s",
								":bleedPurple"
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "broadcaster/1" },
								{ "client-nonce", "459e3142897c7a22b7d275178f2259e0" },
								{ "color", "#0000FF" },
								{ "display-name", "lovingt3s" },
								{ "emote-only", "1" },
								{ "emotes", "62835:0-10" },
								{ "first-msg", "0" },
								{ "flags", string.Empty },
								{ "id", "885196de-cb67-427a-baa8-82f9b0fcd05f" },
								{ "mod", "0" },
								{ "room-id", "713936733" },
								{ "subscriber", "0" },
								{ "tmi-sent-ts", "1643904084794" },
								{ "turbo", "0" },
								{ "user-id", "713936733" },
								{ "user-type", string.Empty },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=broadcaster/1;client-nonce=28e05b1c83f1e916ca1710c44b014515;color=#0000FF;display-name=foofoo;emotes=62835:0-10;first-msg=0;flags=;id=f80a19d6-e35a-4273-82d0-cd87f614e767;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1642696567751;turbo=0;user-id=713936733;user-type= :foofoo!foofoo@foofoo.tmi.twitch.tv PRIVMSG #bar :bleedPurple\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=broadcaster/1;client-nonce=28e05b1c83f1e916ca1710c44b014515;color=#0000FF;display-name=foofoo;emotes=62835:0-10;first-msg=0;flags=;id=f80a19d6-e35a-4273-82d0-cd87f614e767;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1642696567751;turbo=0;user-id=713936733;user-type= :foofoo!foofoo@foofoo.tmi.twitch.tv PRIVMSG #bar :bleedPurple\r\n",
							Command = IrcCommand.PRIVMSG,
							Prefix = new IrcPrefix
							{
								Raw = "foofoo!foofoo@foofoo.tmi.twitch.tv",
								Host = "foofoo.tmi.twitch.tv",
								User = "foofoo",
								Nick = "foofoo"
							},
							Parameters = new List<string>
							{
								"#bar",
								":bleedPurple"
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "broadcaster/1" },
								{ "client-nonce", "28e05b1c83f1e916ca1710c44b014515" },
								{ "color", "#0000FF" },
								{ "display-name", "foofoo" },
								{ "emotes", "62835:0-10" },
								{ "first-msg", "0" },
								{ "flags", string.Empty },
								{ "id", "f80a19d6-e35a-4273-82d0-cd87f614e767" },
								{ "mod", "0" },
								{ "room-id", "713936733" },
								{ "subscriber", "0" },
								{ "tmi-sent-ts", "1642696567751" },
								{ "turbo", "0" },
								{ "user-id", "713936733" },
								{ "user-type", string.Empty },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=staff/1,twitchcon-2024---san-diego/1;client-nonce=99a343c9cf2fcf4e96e0abc358f7b59b;color=#FF4500;display-name=TwitchDev;emotes=;first-msg=1;flags=;id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;mod=0;returning-chatter=0;room-id=12826;source-badge-info=;source-badges=staff/1,twitchcon-2024---san-diego/1;source-id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;source-room-id=12826;subscriber=0;tmi-sent-ts=1725918561648;turbo=0;user-id=141981764;user-type=staff :twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #twitch :Howdy!\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=staff/1,twitchcon-2024---san-diego/1;client-nonce=99a343c9cf2fcf4e96e0abc358f7b59b;color=#FF4500;display-name=TwitchDev;emotes=;first-msg=1;flags=;id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;mod=0;returning-chatter=0;room-id=12826;source-badge-info=;source-badges=staff/1,twitchcon-2024---san-diego/1;source-id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;source-room-id=12826;subscriber=0;tmi-sent-ts=1725918561648;turbo=0;user-id=141981764;user-type=staff :twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #twitch :Howdy!\r\n",
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
								"#twitch",
								":Howdy!"
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "staff/1,twitchcon-2024---san-diego/1" },
								{ "client-nonce", "99a343c9cf2fcf4e96e0abc358f7b59b" },
								{ "color", "#FF4500" },
								{ "display-name", "TwitchDev" },
								{ "emotes", string.Empty },
								{ "first-msg", "1" },
								{ "flags", string.Empty },
								{ "id", "4dcec0e7-7f79-4a82-8aed-91aac9d0640c" },
								{ "mod", "0" },
								{ "returning-chatter", "0" },
								{ "room-id", "12826" },
								{ "source-badge-info", string.Empty },
								{ "source-badges", "staff/1,twitchcon-2024---san-diego/1" },
								{ "source-id", "4dcec0e7-7f79-4a82-8aed-91aac9d0640c" },
								{ "source-room-id", "12826" },
								{ "subscriber", "0" },
								{ "tmi-sent-ts", "1725918561648" },
								{ "turbo", "0" },
								{ "user-id", "141981764" },
								{ "user-type", "staff" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=staff/1,twitchcon-2024---san-diego/1;client-nonce=99a343c9cf2fcf4e96e0abc358f7b59b;color=#FF4500;display-name=TwitchDev;emotes=;flags=;id=17152d83-1fc8-4869-9d44-5157ee212ff1;mod=0;room-id=197886470;source-badge-info=;source-badges=staff/1,twitchcon-2024---san-diego/1;source-id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;source-room-id=12826;subscriber=0;tmi-sent-ts=1725918561648;turbo=0;user-id=141981764;user-type=staff :twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #twitchrivals :Howdy!\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=staff/1,twitchcon-2024---san-diego/1;client-nonce=99a343c9cf2fcf4e96e0abc358f7b59b;color=#FF4500;display-name=TwitchDev;emotes=;flags=;id=17152d83-1fc8-4869-9d44-5157ee212ff1;mod=0;room-id=197886470;source-badge-info=;source-badges=staff/1,twitchcon-2024---san-diego/1;source-id=4dcec0e7-7f79-4a82-8aed-91aac9d0640c;source-room-id=12826;subscriber=0;tmi-sent-ts=1725918561648;turbo=0;user-id=141981764;user-type=staff :twitchdev!twitchdev@twitchdev.tmi.twitch.tv PRIVMSG #twitchrivals :Howdy!\r\n",
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
								"#twitchrivals",
								":Howdy!"
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "staff/1,twitchcon-2024---san-diego/1" },
								{ "client-nonce", "99a343c9cf2fcf4e96e0abc358f7b59b" },
								{ "color", "#FF4500" },
								{ "display-name", "TwitchDev" },
								{ "emotes", string.Empty },
								{ "flags", string.Empty },
								{ "id", "17152d83-1fc8-4869-9d44-5157ee212ff1" },
								{ "mod", "0" },
								{ "room-id", "197886470" },
								{ "source-badge-info", string.Empty },
								{ "source-badges", "staff/1,twitchcon-2024---san-diego/1" },
								{ "source-id", "4dcec0e7-7f79-4a82-8aed-91aac9d0640c" },
								{ "source-room-id", "12826" },
								{ "subscriber", "0" },
								{ "tmi-sent-ts", "1725918561648" },
								{ "turbo", "0" },
								{ "user-id", "141981764" },
								{ "user-type", "staff" },
							}
						},
						false
					},
					new object[]
					{
						"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv UNKNOWN #dallas :ronni\r\n",
						new IrcV3Message
						{
							Raw = "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv UNKNOWN #dallas :ronni\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#dallas",
								":ronni"
							},
							Tags = new Dictionary<string, string>
							{
								{ "room-id", "12345678" },
								{ "target-user-id", "87654321" },
								{ "tmi-sent-ts", "1642715756806" },
							}
						},
						false
					},
					new object[]
					{
						"@login=foo;room-id=;target-msg-id=94e6c7ff-bf98-4faa-af5d-7ad633a158a9;tmi-sent-ts=1642720582342 :tmi.twitch.tv UNKNOWN #bar :what a great day\r\n",
						new IrcV3Message
						{
							Raw = "@login=foo;room-id=;target-msg-id=94e6c7ff-bf98-4faa-af5d-7ad633a158a9;tmi-sent-ts=1642720582342 :tmi.twitch.tv UNKNOWN #bar :what a great day\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#bar",
								":what a great day"
							},
							Tags = new Dictionary<string, string>
							{
								{ "login", "foo" },
								{ "room-id", string.Empty },
								{ "target-msg-id", "94e6c7ff-bf98-4faa-af5d-7ad633a158a9" },
								{ "tmi-sent-ts", "1642720582342" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv UNKNOWN\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv UNKNOWN\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>(),
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", "subscriber/8" },
								{ "badges", "subscriber/6" },
								{ "color", "#0D4200" },
								{ "display-name", "dallas" },
								{ "emote-sets", "0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239" },
								{ "turbo", "0" },
								{ "user-id", "12345678" },
								{ "user-type", "admin" },
							}
						},
						false
					},
					new object[]
					{
						"@msg-id=delete_message_success :tmi.twitch.tv NOTICE #bar :The message from foo is now deleted.\r\n",
						new IrcV3Message
						{
							Raw = "@msg-id=delete_message_success :tmi.twitch.tv NOTICE #bar :The message from foo is now deleted.\r\n",
							Command = IrcCommand.NOTICE,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#bar",
								":The message from foo is now deleted."
							},
							Tags = new Dictionary<string, string>
							{
								{ "msg-id", "delete_message_success" },
							}
						},
						false
					},
					new object[]
					{
						"@emote-only=0;followers-only=-1;r9k=0;room-id=12345678;slow=0;subs-only=0 :tmi.twitch.tv UNKNOWN #bar\r\n",
						new IrcV3Message
						{
							Raw = "@emote-only=0;followers-only=-1;r9k=0;room-id=12345678;slow=0;subs-only=0 :tmi.twitch.tv UNKNOWN #bar\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#bar",
							},
							Tags = new Dictionary<string, string>
							{
								{ "emote-only", "0" },
								{ "followers-only", "-1" },
								{ "r9k", "0" },
								{ "room-id", "12345678" },
								{ "slow", "0" },
								{ "subs-only", "0" },
							}
						},
						false
					},
					new object[]
					{
						"@r9k=1;room-id=713936733 :tmi.twitch.tv UNKNOWN #bar\r\n",
						new IrcV3Message
						{
							Raw = "@r9k=1;room-id=713936733 :tmi.twitch.tv UNKNOWN #bar\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#bar",
							},
							Tags = new Dictionary<string, string>
							{
								{ "r9k", "1" },
								{ "room-id", "713936733" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=staff/1,broadcaster/1,turbo/1;color=#008000;display-name=ronni;emotes=;id=db25007f-7a18-43eb-9379-80131e44d633;login=ronni;mod=0;msg-id=resub;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!;tmi-sent-ts=1507246572675;turbo=1;user-id=87654321;user-type=staff :tmi.twitch.tv UNKNOWN #dallas :Great stream -- keep it up!\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=staff/1,broadcaster/1,turbo/1;color=#008000;display-name=ronni;emotes=;id=db25007f-7a18-43eb-9379-80131e44d633;login=ronni;mod=0;msg-id=resub;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!;tmi-sent-ts=1507246572675;turbo=1;user-id=87654321;user-type=staff :tmi.twitch.tv UNKNOWN #dallas :Great stream -- keep it up!\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#dallas",
								":Great stream -- keep it up!"
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "staff/1,broadcaster/1,turbo/1" },
								{ "color", "#008000" },
								{ "display-name", "ronni" },
								{ "emotes", string.Empty },
								{ "id", "db25007f-7a18-43eb-9379-80131e44d633" },
								{ "login", "ronni" },
								{ "mod", "0" },
								{ "msg-id", "resub" },
								{ "msg-param-cumulative-months", "6" },
								{ "msg-param-streak-months", "2" },
								{ "msg-param-should-share-streak", "1" },
								{ "msg-param-sub-plan", "Prime" },
								{ "msg-param-sub-plan-name", "Prime" },
								{ "room-id", "12345678" },
								{ "subscriber", "1" },
								{ "system-msg", "ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!" },
								{ "tmi-sent-ts", "1507246572675" },
								{ "turbo", "1" },
								{ "user-id", "87654321" },
								{ "user-type", "staff" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=staff/1,premium/1;color=#0000FF;display-name=TWW2;emotes=;id=e9176cd8-5e22-4684-ad40-ce53c2561c5e;login=tww2;mod=0;msg-id=subgift;msg-param-months=1;msg-param-recipient-display-name=Mr_Woodchuck;msg-param-recipient-id=55554444;msg-param-recipient-name=mr_woodchuck;msg-param-sub-plan-name=House\\sof\\sNyoro~n;msg-param-sub-plan=1000;room-id=12345678;subscriber=0;system-msg=TWW2\\sgifted\\sa\\sTier\\s1\\ssub\\sto\\sMr_Woodchuck!;tmi-sent-ts=1521159445153;turbo=0;user-id=87654321;user-type=staff :tmi.twitch.tv UNKNOWN #forstycup\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=staff/1,premium/1;color=#0000FF;display-name=TWW2;emotes=;id=e9176cd8-5e22-4684-ad40-ce53c2561c5e;login=tww2;mod=0;msg-id=subgift;msg-param-months=1;msg-param-recipient-display-name=Mr_Woodchuck;msg-param-recipient-id=55554444;msg-param-recipient-name=mr_woodchuck;msg-param-sub-plan-name=House\\sof\\sNyoro~n;msg-param-sub-plan=1000;room-id=12345678;subscriber=0;system-msg=TWW2\\sgifted\\sa\\sTier\\s1\\ssub\\sto\\sMr_Woodchuck!;tmi-sent-ts=1521159445153;turbo=0;user-id=87654321;user-type=staff :tmi.twitch.tv UNKNOWN #forstycup\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#forstycup",
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "staff/1,premium/1" },
								{ "color", "#0000FF" },
								{ "display-name", "TWW2" },
								{ "emotes", string.Empty },
								{ "id", "e9176cd8-5e22-4684-ad40-ce53c2561c5e" },
								{ "login", "tww2" },
								{ "mod", "0" },
								{ "msg-id", "subgift" },
								{ "msg-param-months", "1" },
								{ "msg-param-recipient-display-name", "Mr_Woodchuck" },
								{ "msg-param-recipient-id", "55554444" },
								{ "msg-param-recipient-name", "mr_woodchuck" },
								{ "msg-param-sub-plan-name", "House\\sof\\sNyoro~n" },
								{ "msg-param-sub-plan", "1000" },
								{ "room-id", "12345678" },
								{ "subscriber", "0" },
								{ "system-msg", "TWW2\\sgifted\\sa\\sTier\\s1\\ssub\\sto\\sMr_Woodchuck!" },
								{ "tmi-sent-ts", "1521159445153" },
								{ "turbo", "0" },
								{ "user-id", "87654321" },
								{ "user-type", "staff" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv UNKNOWN #dallas\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv UNKNOWN #dallas\r\n",
							Command = IrcCommand.UNKNOWN,
							Prefix = new IrcPrefix
							{
								Raw = "tmi.twitch.tv",
								Server = "tmi.twitch.tv"
							},
							Parameters = new List<string>
							{
								"#dallas",
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "staff/1" },
								{ "color", "#0D4200" },
								{ "display-name", "ronni" },
								{ "emote-sets", "0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239" },
								{ "mod", "1" },
								{ "subscriber", "1" },
								{ "turbo", "1" },
								{ "user-type", "staff" },
							}
						},
						false
					},
					new object[]
					{
						"@badge-info=;badges=broadcaster/1;client-nonce=997dcf443c31e258c1d32a8da47b6936;color=#0000FF;display-name=abc;emotes=;first-msg=0;flags=0-6:S.7;id=eb24e920-8065-492a-8aea-266a00fc5126;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1642786203573;turbo=0;user-id=713936733;user-type= :abc!abc@abc.tmi.twitch.tv PRIVMSG #xyz :HeyGuys\r\n",
						new IrcV3Message
						{
							Raw = "@badge-info=;badges=broadcaster/1;client-nonce=997dcf443c31e258c1d32a8da47b6936;color=#0000FF;display-name=abc;emotes=;first-msg=0;flags=0-6:S.7;id=eb24e920-8065-492a-8aea-266a00fc5126;mod=0;room-id=713936733;subscriber=0;tmi-sent-ts=1642786203573;turbo=0;user-id=713936733;user-type= :abc!abc@abc.tmi.twitch.tv PRIVMSG #xyz :HeyGuys\r\n",
							Command = IrcCommand.PRIVMSG,
							Prefix = new IrcPrefix
							{
								Raw = "abc!abc@abc.tmi.twitch.tv",
								Host = "abc.tmi.twitch.tv",
								User = "abc",
								Nick = "abc"
							},
							Parameters = new List<string>
							{
								"#xyz",
								":HeyGuys",
							},
							Tags = new Dictionary<string, string>
							{
								{ "badge-info", string.Empty },
								{ "badges", "broadcaster/1" },
								{ "client-nonce", "997dcf443c31e258c1d32a8da47b6936" },
								{ "color", "#0000FF" },
								{ "display-name", "abc" },
								{ "emotes", string.Empty },
								{ "first-msg", "0" },
								{ "flags", "0-6:S.7" },
								{ "id", "eb24e920-8065-492a-8aea-266a00fc5126" },
								{ "mod", "0" },
								{ "room-id", "713936733" },
								{ "subscriber", "0" },
								{ "tmi-sent-ts", "1642786203573" },
								{ "turbo", "0" },
								{ "user-id", "713936733" },
								{ "user-type", string.Empty },
							}
						},
						false
					},
					new object[]
					{
						":ronni!ronni@ronni.tmi.twitch.tv JOIN #dallas\r\n",
						new IrcV3Message
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
							Tags = new Dictionary<string, string>(),
						},
						false
					},
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