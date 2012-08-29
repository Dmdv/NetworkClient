using System;
using System.IO;
using System.Net;

namespace NetworkClient
{
	public sealed class HttpDownloader
	{
		public delegate void DownloadProgressEventHandler(object sender, DownloadProgressArgs e);

		private const int BufferSize = 4096;
		private readonly bool _connectionEstablished;

		private readonly DownloadMode _downloadMode;
		private readonly HttpWebResponse _response;

		private readonly long _rangeFrom;
		private long _bytesRead;
		private long _bytesWritten;
		private bool _cancellationPending;

		public event DownloadProgressEventHandler DownloadProgress;

		public HttpDownloader(string url)
		{
			WebFactory = new WebFactory();
			_response = WebFactory.GetHttpResponse(url);
			_connectionEstablished = IsResponseValid(_response);
			_rangeFrom = WebFactory.Options.RangeFrom;
			_downloadMode = WebFactory.Options.DownloadMode;
		}

		public HttpDownloader(string url, HttpDownloaderOptions options)
		{
			Options = options;
			_response = WebFactory.GetHttpResponse(url, options);
			_connectionEstablished = IsResponseValid(_response);
			_rangeFrom = WebFactory.Options.RangeFrom;
			_downloadMode = WebFactory.Options.DownloadMode;
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

		private WebFactory WebFactory { get; set; }

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

		public string DownloadToString()
		{
			if (!_connectionEstablished)
			{
				throw new NotConnectedException();
			}

			using (var stream = DownloadToStream() as MemoryStream)
			{
				if (stream == null) return null;
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
			return ReadToStream(stream) ? stream : null;
		}

		private bool ReadToStream(Stream stream)
		{
			var e = new DownloadProgressArgs {Size = Length};

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
					e.PercentComplete = (_bytesWritten/(float) Length - _rangeFrom)*100f;
					OnDownloadProgress(e);
				} while (_bytesRead > 0L);
			}
			return true;
		}

		private static bool IsResponseValid(HttpWebResponse response)
		{
			return 
				response.StatusCode == HttpStatusCode.OK || 
				response.StatusCode == HttpStatusCode.PartialContent;
		}
	}
}