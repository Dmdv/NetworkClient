using System;

namespace NetworkClient
{
	public class DownloaderException : Exception
	{
		public DownloaderException()
		{
		}

		public DownloaderException(string message)
			: base(message)
		{
		}

		public DownloaderException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}