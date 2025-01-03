using System;
using System.Text;

namespace IrcNet.EventsArgs
{
	/// <summary>
	/// Provides data for the MessageSent event.
	/// </summary>
	public class MessageSentEventArgs : EventArgs
	{
		private readonly byte[] message;

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageSentEventArgs"/> class.
		/// </summary>
		/// <param name="message">The message that was sent.</param>
		public MessageSentEventArgs(byte[] message)
		{
			this.message = message;
		}

		/// <summary>
		/// Gets the message that was sent.
		/// </summary>
		public string Message => Encoding.UTF8.GetString(message);
	}
}