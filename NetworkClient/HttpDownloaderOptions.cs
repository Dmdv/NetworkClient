namespace NetworkClient
{
	public class HttpDownloaderOptions
	{
		public readonly string Method = "GET";
		public readonly long RangeFrom;
		public readonly long RangeTo;
		public readonly int Timeout = 30000;

		public HttpDownloaderOptions(string method, long? rangeFrom, long? rangeTo, int? timeout)
		{
			if (method != null)
			{
				Method = method;
			}

			if (rangeFrom.HasValue)
			{
				RangeFrom = rangeFrom.Value;
			}
			if (rangeTo.HasValue)
			{
				RangeTo = rangeTo.Value;
			}
			if (timeout.HasValue)
			{
				Timeout = timeout.Value;
			}
		}

		public DownloadMode DownloadMode { get; set; }
	}
}