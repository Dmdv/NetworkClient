using System;
using System.IO;
using System.Net;

namespace NetworkClient
{
	public sealed class HttpDownloader
	{
		public delegate void DownloadProgressEventHandler(object sender, DownloadProgressArgs e);
		public event DownloadProgressEventHandler DownloadProgress;

		private const int BufferSize = 4096;
		// private int _timeout = 15000;

		private DownloadMode _downloadMode;
		private readonly HttpWebResponse _response;
		private HttpWebRequest _request;

		private readonly bool _connectionEstablished;
		private long _bytesRead;
		private long _bytesWritten;
		private bool _cancellationPending;
		// private string _method;
		//private long _rangeFrom;
		//private long _rangeTo;
		// private string _requestUrl;

		public HttpDownloader(string url)
		{
			_response = WebFactory.GetHttpResponse(url);
			_connectionEstablished = IsResponseValid(_response);
		}

		public HttpDownloader(string url, HttpDownloaderOptions options)
		{
			Options = options;
			_response = WebFactory.GetHttpResponse(url, options);
			_connectionEstablished = IsResponseValid(_response);
		}

		public HttpDownloaderOptions Options { get; private set; }

		public string Encoding
		{
			get { return _response.ContentEncoding; }
		}

		public int Length
		{
			get { return Convert.ToInt32(_response.ContentLength); }
		}

		public string Type
		{
			get { return _response.ContentType; }
		}

		public long DownloadToFile(string destinationFile)
		{
			if (!_connectionEstablished) { throw new NotConnectedException(); }

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

		public string DownloadToString()
		{
			if (!_connectionEstablished)
			{
				throw new NotConnectedException();
			}

			using (var stream = (MemoryStream) DownloadToStream())
			{
				stream.Position = 0L;
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		public void Stop()
		{
			_cancellationPending = true;
		}

		private void OnDownloadProgress(DownloadProgressArgs e)
		{
			if (DownloadProgress != null)
			{
				DownloadProgress(this, e);
			}
		}

		private Stream DownloadToStream()
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

		private bool ReadToStream(Stream stream)
		{
			var e = new DownloadProgressArgs { Size = Length };
			
			var buffer = new byte[BufferSize];
			using (var responseStream = _response.GetResponseStream())
			{
				if (responseStream == null) return false;

				do
				{
					if (_cancellationPending)
						break;

					_bytesRead = responseStream.Read(buffer, 0, buffer.Length);
					stream.Write(buffer, 0, (int) _bytesRead);
					_bytesWritten += _bytesRead;
					e.BytesRead = _bytesRead;
					e.BytesWritten = _bytesWritten;
					e.PercentComplete = (_bytesWritten / (float) Length - _rangeFrom) * 100f;
					OnDownloadProgress(e);
				} 
				while (_bytesRead > 0L);
			}
			return true;
		}

		private static bool IsResponseValid(HttpWebResponse response)
		{
			return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.PartialContent;
		}
	}
}