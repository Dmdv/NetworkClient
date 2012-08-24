namespace NetworkClient
{
	public class HttpDownloaderOptions
	{
		public string Method = "GET";
		public long RangeFrom;
		public long RangeTo;
		public int Timeout = 0x7530;
		public string Url;

		public HttpDownloaderOptions(string method, string url, long? rangeFrom, long? rangeTo, int? timeout)
		{
			if (method != null)
			{
				Method = method;
			}
			Url = url;
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
	}
}