using System;

namespace NetworkClient
{
	internal class DownloaderException : Exception
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