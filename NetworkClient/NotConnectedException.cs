using System;

namespace NetworkClient
{
	public class NotConnectedException : Exception
	{
		public NotConnectedException()
		{
		}

		public NotConnectedException(string message) 
			: base(message)
		{
		}

		public NotConnectedException(string message, Exception inner) 
			: base(message, inner)
		{
		}
	}
}