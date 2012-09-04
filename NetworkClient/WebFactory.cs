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
			Options = new HttpDownloaderOptions("GET", Timeout);
		}

		public HttpDownloaderOptions Options { get; set; }

		public HttpWebResponse GetHttpResponse(string url)
		{
			try
			{
				return GetHttpResponse(url, Options);
			}
			catch (Exception ex)
			{
				throw new DownloaderException("Downloader exception", ex);
			}
		}

		private HttpWebResponse GetHttpResponse(string url, HttpDownloaderOptions options)
		{
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

		private HttpWebRequest CreateRequestInternal(string url)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);

			if (!string.IsNullOrEmpty(Options.ProxyUri) && Options.ProxyPort != 0)
			{
				request.Proxy = new WebProxy(Options.ProxyUri, Options.ProxyPort);
			}

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

		private HttpWebResponse GetResponseInternal(WebRequest request)
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