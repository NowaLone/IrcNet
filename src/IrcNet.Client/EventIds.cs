using Microsoft.Extensions.Logging;

namespace IrcNet.Client
{
	internal static class EventIds
	{
		public static EventId ConnectedEvent = new EventId(0, "Connected");
		public static EventId ConnectingEvent = new EventId(1, "Connecting");
		public static EventId DisconnectedEvent = new EventId(2, "Disconnected");
		public static EventId StateChangedEvent = new EventId(3, "State Changed");
		public static EventId MessageReceivedEvent = new EventId(4, "Message Received");
		public static EventId MessageSentEvent = new EventId(5, "Message Sent");
		public static EventId ReconnectingEvent = new EventId(1, "Reconnecting");
	}
}