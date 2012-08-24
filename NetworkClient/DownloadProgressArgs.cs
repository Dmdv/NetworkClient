using System;

namespace NetworkClient
{
	public class DownloadProgressArgs : EventArgs
	{
		public long BytesRead;
		public long BytesWritten;
		public float PercentComplete;
		public long Size;
	}
}