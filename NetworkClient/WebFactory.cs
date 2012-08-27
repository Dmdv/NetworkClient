using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetworkClient
{
	internal sealed class WebFactory
	{
		private const int Timeout = 15000;

		public WebFactory()
		{
			Options = new HttpDownloaderOptions("GET", 0L, 0L, Timeout);
		}

		public HttpDownloaderOptions Options { get; private set; }

		public HttpWebResponse GetHttpResponse(string url)
		{
			try
			{
				return GetHttpResponse(url, Options);
			}
			catch (Exception ex)
			{
				throw new DownloaderException("Downloader unknown exception", ex);
			}
		}

		public HttpWebResponse GetHttpResponse(string url, HttpDownloaderOptions options)
		{
			Options = options;

			var request = CreateRequestInternal(url);

			if (options.RangeFrom != 0L && options.RangeTo != 0L)
			{
				options.DownloadMode = DownloadMode.DownloadRange;
				request.AddRange((int) options.RangeFrom, (int) options.RangeTo);
			}
			else
			{
				options.DownloadMode = DownloadMode.Append;
				request.AddRange((int) options.RangeFrom);
			}

			return GetResponseInternal(request);
		}

		private static HttpWebRequest CreateRequestInternal(string url)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.UserAgent = "Common downloader/1.0";
			request.Timeout = Timeout;
			request.Method = "GET";

			if (request.Method.ToLower().Equals("post"))
			{
				var bytes = new ASCIIEncoding().GetBytes(GetUrlParams(url));

				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = bytes.Length;
				request.AllowAutoRedirect = true;

				using (var requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
			}
			return request;
		}

		private static HttpWebResponse GetResponseInternal(HttpWebRequest request)
		{
			try
			{
				return (HttpWebResponse)request.GetResponse();
			}
			catch (Exception ex)
			{
				throw new DownloaderException("Failed to get request", ex);
			}
		}

		private static string GetUrlHost(string url)
		{
			return new Regex(@"(?<host>https?\:\/\/(?<domain>[^\?]*))").Match(url).Groups["host"].Value;
		}

		private static string GetUrlParams(string url)
		{
			return new Regex(@"(\?(?<params>[^\?]*))").Match(url).Groups["params"].Value;
		}

	}
}