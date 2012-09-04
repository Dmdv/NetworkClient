namespace NetworkClient
{
	public class HttpDownloaderOptions
	{
		private const string DefaultMethod = "GET";
		private const int DefaultTimeout = 30000;

		public HttpDownloaderOptions(string method = DefaultMethod,
		                             int timeout = DefaultTimeout,
		                             long rangeFrom = 0L,
		                             long rangeTo = 0L,
		                             string proxyUri = null,
		                             int proxyPort = 0)
		{
			Method = method;
			Timeout = timeout;
			RangeFrom = rangeFrom;
			RangeTo = rangeTo;
			ProxyUri = proxyUri;
			ProxyPort = proxyPort;
		}

		public string Method { get; private set; }
		public long RangeTo { get; private set; }
		public long RangeFrom { get; private set; }
		public int Timeout { get; private set; }

		public string ProxyUri { get; set; }
		public int ProxyPort { get; set; }
		public DownloadMode DownloadMode { get; set; }
	}
}