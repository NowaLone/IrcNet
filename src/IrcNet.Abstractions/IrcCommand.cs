namespace IrcNet
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	/// <summary>
	/// Enum of Irc commands and replies.
	/// </summary>
	public enum IrcCommand
	{
		#region Commands

		/// <summary>
		/// Unknown command.
		/// </summary>
		UNKNOWN = 0,

		/// <summary>
		/// 4.1.1 Password message.
		/// </summary>
		PASS = 111,

		/// <summary>
		/// 4.1.2 Nick message.
		/// </summary>
		NICK = 112,

		/// <summary>
		/// 4.1.3 User message.
		/// </summary>
		USER = 113,

		/// <summary>
		/// 4.1.4 Server message.
		/// </summary>
		SERVER = 114,

		/// <summary>
		/// 4.1.5 Oper.
		/// </summary>
		OPER = 115,

		/// <summary>
		/// 4.1.6 Quit.
		/// </summary>
		QUIT = 116,

		/// <summary>
		/// 4.1.7 Server quit message.
		/// </summary>
		SQUIT = 117,

		/// <summary>
		/// 4.2.1 Join message.
		/// </summary>
		JOIN = 121,

		/// <summary>
		/// 4.2.2 Part message.
		/// </summary>
		PART = 122,

		/// <summary>
		/// 4.2.3 Mode message.
		/// </summary>
		MODE = 123,

		/// <summary>
		/// 4.2.4 Topic message.
		/// </summary>
		TOPIC = 124,

		/// <summary>
		/// 4.2.5 Names message.
		/// </summary>
		NAMES = 125,

		/// <summary>
		/// 4.2.6 List message.
		/// </summary>
		LIST = 126,

		/// <summary>
		/// 4.2.7 Invite message.
		/// </summary>
		INVITE = 127,

		/// <summary>
		/// 4.2.8 Kick command.
		/// </summary>
		KICK = 128,

		/// <summary>
		/// 4.3.1 Version message.
		/// </summary>
		VERSION = 131,

		/// <summary>
		/// 4.3.2 Stats message.
		/// </summary>
		STATS = 132,

		/// <summary>
		/// 4.3.3 Links message.
		/// </summary>
		LINKS = 133,

		/// <summary>
		/// 4.3.4 Time message.
		/// </summary>
		TIME = 134,

		/// <summary>
		/// 4.3.5 Connect message.
		/// </summary>
		CONNECT = 135,

		/// <summary>
		/// 4.3.6 Trace message.
		/// </summary>
		TRACE = 136,

		/// <summary>
		/// 4.3.7 Admin command.
		/// </summary>
		ADMIN = 137,

		/// <summary>
		/// 4.3.8 Info command.
		/// </summary>
		INFO = 138,

		/// <summary>
		/// 3.4.1 Motd message.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		MOTD = 139,

		/// <summary>
		/// 3.4.2 Lusers message
		/// </summary>
		/// <remarks>RFC2812</remarks>
		LUSERS = 140,

		/// <summary>
		/// 4.4.1 Private messages.
		/// </summary>
		PRIVMSG = 141,

		/// <summary>
		/// 4.4.2 Notice.
		/// </summary>
		NOTICE = 142,

		/// <summary>
		/// 4.5.1 Who query.
		/// </summary>
		WHO = 151,

		/// <summary>
		/// 4.5.2 Whois query.
		/// </summary>
		WHOIS = 152,

		/// <summary>
		/// 4.5.3 Whowas.
		/// </summary>
		WHOWAS = 153,

		/// <summary>
		/// 4.6.1 Kill message.
		/// </summary>
		KILL = 161,

		/// <summary>
		/// 4.6.2 Ping message.
		/// </summary>
		PING = 162,

		/// <summary>
		/// 4.6.3 Pong message.
		/// </summary>
		PONG = 163,

		/// <summary>
		/// 4.6.4 Error.
		/// </summary>
		ERROR = 164,

		/// <summary>
		/// 5.1 Away.
		/// </summary>
		AWAY = 171,

		/// <summary>
		/// 5.2 Rehash message.
		/// </summary>
		REHASH = 172,

		/// <summary>
		/// 5.3 Restart message.
		/// </summary>
		RESTART = 173,

		/// <summary>
		/// 5.4 Summon message.
		/// </summary>
		SUMMON = 174,

		/// <summary>
		/// 5.5 Users.
		/// </summary>
		USERS = 175,

		/// <summary>
		/// 5.6 Operwall message.
		/// </summary>
		WALLOPS = 176,

		/// <summary>
		/// 5.7 Userhost message.
		/// </summary>
		USERHOST = 177,

		/// <summary>
		/// 5.8 Ison message.
		/// </summary>
		ISON = 178,

		/// <summary>
		/// HELP message.
		/// </summary>
		/// <remarks>Modern IRC</remarks>
		HELP = 181,

		#region V3 Commands Extension

		/// <summary>
		/// CAP message.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		CAP = 191,

		/// <summary>
		/// AUTHENTICATE message.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		AUTHENTICATE = 192,

		#endregion V3 Commands Extension

		#endregion Commands

		#region Command Responses

		/// <summary>
		/// The first message sent after client registration. The text used varies widely.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_WELCOME = 001,

		/// <summary>
		/// Part of the post-registration greeting. Text varies widely.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_YOURHOST = 002,

		/// <summary>
		/// Part of the post-registration greeting. Text varies widely.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_CREATED = 003,

		/// <summary>
		/// Part of the post-registration greeting.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_MYINFO = 004,

		/// <summary>
		/// Sent by the server to a user to suggest an alternative server, sometimes used when the connection is refused because the server is already full.
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_BOUNCE = 005,

		RPL_TRACELINK = 200,
		RPL_TRACECONNECTING = 201,
		RPL_TRACEHANDSHAKE = 202,
		RPL_TRACEUNKNOWN = 203,
		RPL_TRACEOPERATOR = 204,
		RPL_TRACEUSER = 205,
		RPL_TRACESERVER = 206,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_TRACESERVICE = 207,

		RPL_TRACENEWTYPE = 208,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		/// <remarks>RFC2812</remarks>
		RPL_TRACECLASS = 209,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_TRACERECONNECT = 210,

		RPL_STATSLINKINFO = 211,
		RPL_STATSCOMMANDS = 212,
		RPL_STATSCLINE = 213,
		RPL_STATSNLINE = 214,
		RPL_STATSILINE = 215,
		RPL_STATSKLINE = 216,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_STATSQLINE = 217,

		RPL_STATSYLINE = 218,
		RPL_ENDOFSTATS = 219,
		RPL_UMODEIS = 221,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_SERVICEINFO = 231,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_ENDOFSERVICES = 232,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_SERVICE = 233,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		/// <remarks>RFC2812</remarks>
		RPL_SERVLIST = 234,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		/// <remarks>RFC2812</remarks>
		RPL_SERVLISTEND = 235,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_STATSVLINE = 240,

		RPL_STATSLLINE = 241,
		RPL_STATSUPTIME = 242,
		RPL_STATSOLINE = 243,
		RPL_STATSHLINE = 244,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_STATSPING = 246,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_STATSBLINE = 247,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_STATSDLINE = 250,

		RPL_LUSERCLIENT = 251,
		RPL_LUSEROP = 252,
		RPL_LUSERUNKNOWN = 253,
		RPL_LUSERCHANNELS = 254,
		RPL_LUSERME = 255,
		RPL_ADMINME = 256,
		RPL_ADMINLOC1 = 257,
		RPL_ADMINLOC2 = 258,
		RPL_ADMINEMAIL = 259,
		RPL_TRACELOG = 261,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_TRACEEND = 262,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_TRYAGAIN = 263,

		RPL_NONE = 300,
		RPL_AWAY = 301,
		RPL_USERHOST = 302,
		RPL_ISON = 303,
		RPL_UNAWAY = 305,
		RPL_NOWAWAY = 306,
		RPL_WHOISUSER = 311,
		RPL_WHOISSERVER = 312,
		RPL_WHOISOPERATOR = 313,
		RPL_WHOWASUSER = 314,
		RPL_ENDOFWHO = 315,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_WHOISCHANOP = 316,

		RPL_WHOISIDLE = 317,
		RPL_ENDOFWHOIS = 318,
		RPL_WHOISCHANNELS = 319,
		RPL_LISTSTART = 321,
		RPL_LIST = 322,
		RPL_LISTEND = 323,
		RPL_CHANNELMODEIS = 324,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_UNIQOPIS = 325,

		RPL_NOTOPIC = 331,
		RPL_TOPIC = 332,
		RPL_INVITING = 341,
		RPL_SUMMONING = 342,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_INVITELIST = 346,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_ENDOFINVITELIST = 347,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_EXCEPTLIST = 348,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_ENDOFEXCEPTLIST = 349,

		RPL_VERSION = 351,
		RPL_WHOREPLY = 352,
		RPL_NAMREPLY = 353,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_KILLDONE = 361,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_CLOSING = 362,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_CLOSEEND = 363,

		RPL_LINKS = 364,
		RPL_ENDOFLINKS = 365,
		RPL_ENDOFNAMES = 366,
		RPL_BANLIST = 367,
		RPL_ENDOFBANLIST = 368,
		RPL_ENDOFWHOWAS = 369,
		RPL_INFO = 371,
		RPL_MOTD = 372,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_INFOSTART = 373,

		RPL_ENDOFINFO = 374,
		RPL_ENDOFMOTD = 376,
		RPL_YOUREOPER = 381,
		RPL_REHASHING = 382,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		RPL_YOURESERVICE = 383,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		RPL_MYPORTIS = 384,

		RPL_TIME = 391,
		RPL_USERSSTART = 392,
		RPL_USERS = 393,
		RPL_ENDOFUSERS = 394,
		RPL_NOUSERS = 395,
		RPL_MOTDSTART = 375,

		#endregion Command Responses

		#region Error Replies

		/// <summary>
		/// Used to indicate the nickname parameter supplied to a command is currently unused.
		/// </summary>
		/// <example>"&lt;nickname&gt; :No such nick/channel"</example>
		ERR_NOSUCHNICK = 401,

		/// <summary>
		/// Used to indicate the server name given currently doesn't exist.
		/// </summary>
		/// <example>"&lt;server name&gt; :No such server"</example>
		ERR_NOSUCHSERVER = 402,

		ERR_NOSUCHCHANNEL = 403,
		ERR_CANNOTSENDTOCHAN = 404,
		ERR_TOOMANYCHANNELS = 405,
		ERR_WASNOSUCHNICK = 406,
		ERR_TOOMANYTARGETS = 407,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_NOSUCHSERVICE = 408,

		ERR_NOORIGIN = 409,
		ERR_NORECIPIENT = 411,
		ERR_NOTEXTTOSEND = 412,
		ERR_NOTOPLEVEL = 413,
		ERR_WILDTOPLEVEL = 414,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_BADMASK = 415,

		ERR_UNKNOWNCOMMAND = 421,
		ERR_NOMOTD = 422,
		ERR_NOADMININFO = 423,
		ERR_FILEERROR = 424,
		ERR_NONICKNAMEGIVEN = 431,
		ERR_ERRONEUSNICKNAME = 432,
		ERR_NICKNAMEINUSE = 433,
		ERR_NICKCOLLISION = 436,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_UNAVAILRESOURCE = 437,

		ERR_USERNOTINCHANNEL = 441,
		ERR_NOTONCHANNEL = 442,
		ERR_USERONCHANNEL = 443,
		ERR_NOLOGIN = 444,
		ERR_SUMMONDISABLED = 445,
		ERR_USERSDISABLED = 446,
		ERR_NOTREGISTERED = 451,
		ERR_NEEDMOREPARAMS = 461,
		ERR_ALREADYREGISTRED = 462,
		ERR_NOPERMFORHOST = 463,
		ERR_PASSWDMISMATCH = 464,
		ERR_YOUREBANNEDCREEP = 465,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		ERR_YOUWILLBEBANNED = 466,

		ERR_KEYSET = 467,
		ERR_CHANNELISFULL = 471,
		ERR_UNKNOWNMODE = 472,
		ERR_INVITEONLYCHAN = 473,
		ERR_BANNEDFROMCHAN = 474,
		ERR_BADCHANNELKEY = 475,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		/// <remarks>RFC2812</remarks>
		ERR_BADCHANMASK = 476,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_NOCHANMODES = 477,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_BANLISTFULL = 478,

		ERR_NOPRIVILEGES = 481,
		ERR_CHANOPRIVSNEEDED = 482,
		ERR_CANTKILLSERVER = 483,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_RESTRICTED = 484,

		/// <summary>
		///
		/// </summary>
		/// <remarks>RFC2812</remarks>
		ERR_UNIQOPRIVSNEEDED = 485,

		ERR_NOOPERHOST = 491,

		/// <summary>
		///
		/// </summary>
		/// <remarks>Reserved numeric.</remarks>
		ERR_NOSERVICEHOST = 492,

		ERR_UMODEUNKNOWNFLAG = 501,
		ERR_USERSDONTMATCH = 502,

		/// <summary>
		/// Indicates that a <see cref="HELP"/> command requested help on a subject the server does not know about.
		/// </summary>
		/// <remarks>Modern IRC</remarks>
		/// <example>"&lt;client&gt; &lt;subject&gt; :No help available on this topic"</example>
		ERR_HELPNOTFOUND = 524,

		/// <summary>
		/// Indicates the start of a reply to a <see cref="HELP"/> command.
		/// </summary>
		/// <remarks>Modern IRC</remarks>
		/// <example>"&lt;client&gt; &lt;subject&gt; :&lt;first line of help section&gt;"</example>
		RPL_HELPSTART = 704,

		/// <summary>
		/// Returns a line of <see cref="HELP"/> text to the client.
		/// </summary>
		/// <remarks>Modern IRC</remarks>
		/// <example>"&lt;client&gt; &lt;subject&gt; :&lt;line of help text&gt;"</example>
		RPL_HELPTXT = 705,

		/// <summary>
		/// Returns the final <see cref="HELP"/> line to the client.
		/// </summary>
		/// <remarks>Modern IRC</remarks>
		/// <example>"&lt;client&gt; &lt;subject&gt; :&lt;last line of help text&gt;"</example>
		RPL_ENDOFHELP = 706,

		#region V3 Replies Extension

		/// <summary>
		/// Sent when the user’s account name is set (whether by SASL or otherwise).
		/// </summary>
		/// <remarks>IRCv3</remarks>
		RPL_LOGGEDIN = 900,

		/// <summary>
		/// Sent when the user’s account name is unset (whether by SASL or otherwise).
		/// </summary>
		/// <remarks>IRCv3</remarks>
		RPL_LOGGEDOUT = 901,

		/// <summary>
		/// Sent when the SASL authentication fails because the account is currently locked out, held, or otherwise administratively made unavailable.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		ERR_NICKLOCKED = 902,

		/// <summary>
		/// Sent when the SASL authentication finishes successfully. It usually goes along with 900.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		RPL_SASLSUCCESS = 903,

		/// <summary>
		/// Sent when the SASL authentication fails because of invalid credentials or other errors not explicitly mentioned by other numerics.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		ERR_SASLFAIL = 904,

		/// <summary>
		/// Sent when credentials are valid, but the SASL authentication fails because the client-sent <see cref="AUTHENTICATE"/> command was too long (i.e. the parameter longer than 400 bytes).
		/// </summary>
		/// <remarks>IRCv3</remarks>
		ERR_SASLTOOLONG = 905,

		/// <summary>
		/// Sent when the SASL authentication is aborted because the client sent an <see cref="AUTHENTICATE"/> command with * as the parameter.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		ERR_SASLABORTED = 906,

		/// <summary>
		/// Sent when the client attempts to initiate SASL authentication after it has already finished successfully for that connection.
		/// </summary>
		/// <remarks>IRCv3</remarks>
		ERR_SASLALREADY = 907,

		/// <summary>
		/// Sent in reply to an <see cref="AUTHENTICATE"/> command which requests an unsupported mechanism. The numeric contains a comma-separated list of mechanisms supported by the server (or network, services).
		/// </summary>
		/// <remarks>IRCv3</remarks>
		RPL_SASLMECHS = 908,

		#endregion V3 Replies Extension

		#endregion Error Replies
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}