using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetworkClient
{
	public class HttpDownloaderFactory
	{
		// Fields
		public delegate void DownloadProgressHandler(object sender, DownloadProgressArgs e);

		private long _bytesWritten;
		private long _bytesRead;
		private readonly bool _connectionEstablished;
		private string _requestUrl;
		private int _timeout;
		private bool _cancellationPending;
		private DownloadMode _downloadMode;
		private string _method;
		private long _rangeFrom;
		private long _rangeTo;
		private HttpWebRequest _request;
		private HttpWebResponse _response;

		// Methods
		public HttpDownloaderFactory(HttpDownloaderOptions options)
		{
			_timeout = 0x3a98;
			_method = "GET";
			Options = options;
			_connectionEstablished = InitInstances(_requestUrl);
		}

		public HttpDownloaderFactory(string url)
		{
			_timeout = 0x3a98;
			_method = "GET";
			_connectionEstablished = InitInstances(url);
		}

		public string Encoding
		{
			get { return _response.ContentEncoding; }
		}

		public int Length
		{
			get { return Convert.ToInt32(_response.ContentLength); }
		}

		public HttpDownloaderOptions Options
		{
			set
			{
				_requestUrl = value.Url;
				_rangeFrom = value.RangeFrom;
				_rangeTo = value.RangeTo;
				_timeout = value.Timeout;
				_method = value.Method;
			}
		}

		public string Type
		{
			get { return _response.ContentType; }
		}

		public event DownloadProgressHandler DownloadProgress;

		public long DownloadToFile(string destinationFile)
		{
			if (!_connectionEstablished)
			{
				throw new NotConnectedException();
			}
			FileStream stream = null;
			switch (_downloadMode)
			{
				case DownloadMode.Download:
					new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.Read).Seek(0L, SeekOrigin.Begin);
					break;

				case DownloadMode.Append:
					stream = new FileStream(destinationFile, FileMode.Append, FileAccess.Write, FileShare.Read);
					stream.Seek(_rangeFrom, SeekOrigin.Begin);
					stream.Position = _rangeFrom;
					break;
			}
			ReadToStream(stream);
			if (stream != null)
				stream.Close();
			return _bytesWritten;
		}

		public long DownloadToFile(string destinationFile, bool deleteIfExist)
		{
			if (deleteIfExist && File.Exists(destinationFile))
			{
				File.Delete(destinationFile);
			}
			return DownloadToFile(destinationFile);
		}

		public Stream DownloadToStream()
		{
			if (!_connectionEstablished)
			{
				throw new NotConnectedException();
			}
			Stream stream = new MemoryStream();
			stream.Position = 0L;
			ReadToStream(stream);
			return stream;
		}

		public string DownloadToString()
		{
			if (!_connectionEstablished)
			{
				throw new NotConnectedException();
			}
			var stream = (MemoryStream) DownloadToStream();
			stream.Position = 0L;
			var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		public string GetUrlHost(string url)
		{
			return new Regex(@"(?<host>https?\:\/\/(?<domain>[^\?]*))").Match(url).Groups["host"].Value;
		}

		public string GetUrlParams(string url)
		{
			return new Regex(@"(\?(?<params>[^\?]*))").Match(url).Groups["params"].Value;
		}

		public void Stop()
		{
			_cancellationPending = true;
		}

		protected virtual void OnDownloadProgress(DownloadProgressArgs e)
		{
			if (DownloadProgress != null)
			{
				DownloadProgress(this, e);
			}
		}

		private bool ReadToStream(Stream stream)
		{
			var e = new DownloadProgressArgs
			{
				Size = Length
			};
			var buffer = new byte[0x800];
			var responseStream = _response.GetResponseStream();
			if (responseStream == null)
			{
				return false;
			}
			do
			{
				if (!_cancellationPending)
				{
					_bytesRead = responseStream.Read(buffer, 0, buffer.Length);
					stream.Write(buffer, 0, (int) _bytesRead);
					_bytesWritten += _bytesRead;
					e.BytesRead = _bytesRead;
					e.BytesWritten = _bytesWritten;
					e.PercentComplete = (((_bytesWritten) / ((float) Length)) - _rangeFrom) * 100f;
					OnDownloadProgress(e);
				}
				else
				{
					responseStream.Close();
					break;
				}
			}
			while (_bytesRead > 0L);
			responseStream.Close();
			return true;
		}

		private bool InitInstances(string url)
		{
			try
			{
				_requestUrl = url;
				_request = (HttpWebRequest) WebRequest.Create(url);
				_request.UserAgent = "Common downloader/0.1";
				_request.Timeout = _timeout;
				_request.Method = _method;
				if (_request.Method.ToLower().Equals("post"))
				{
					_request.ContentType = "application/x-www-form-urlencoded";
					var bytes = new ASCIIEncoding().GetBytes(GetUrlParams(url));
					_request.ContentLength = bytes.Length;
					_request.AllowAutoRedirect = true;
					var requestStream = _request.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				if ((_rangeFrom != 0L) && (_rangeTo != 0L))
				{
					_downloadMode = DownloadMode.DownloadRange;
					_request.AddRange((int) _rangeFrom, (int) _rangeTo);
				}
				else
				{
					_downloadMode = DownloadMode.Append;
					_request.AddRange((int) _rangeFrom);
				}
				_response = (HttpWebResponse) _request.GetResponse();
				return ((_response.StatusCode == HttpStatusCode.OK) || (_response.StatusCode == HttpStatusCode.PartialContent));
			}
			catch
			{
				return false;
			}
		}

		// Properties
	}
}