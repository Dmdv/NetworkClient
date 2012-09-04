using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkClient;

namespace UnitTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UnitTests
	{
		const string Uri = @"http://www.gradsch.ohio-state.edu/Depo/ETD_Tutorial/lesson2.pdf";

		[TestMethod]
		public void TestDownloadToString()
		{
			const string Target =
				@"http://maps.googleapis.com/maps/api/geocode/xml?address=" + 
				@"1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=false";

			var client = new HttpDownloader(Target);
			var str = client.DownloadToString();
			Assert.IsTrue(str.Length == 1756);
			Assert.IsTrue(str.Contains("1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA"));
		}

		[TestMethod]
		public void TestDownloadString()
		{
			var webRequest = WebRequest.Create("http://www.yandex.ru");
			webRequest.Method = "GET";
			webRequest.BeginGetResponse(OnRequestCompleted, webRequest);
			Thread.Sleep(1200000);
		}

		[TestMethod]
		public void TestDownloadFile()
		{
			var downloader = new HttpDownloader(Uri);
			downloader.DownloadProgress += DownloaderProgress;
			var tempFileName = Path.GetTempFileName();
			downloader.DownloadToFile(tempFileName);
			Assert.IsTrue(File.Exists(tempFileName));
			try
			{
				File.Delete(tempFileName);
			}
			catch
			{
			}
		}

		private void DownloaderProgress(object sender, DownloadProgressArgs e)
		{
			Trace.WriteLine(e.PercentComplete);
		}

		private void OnRequestCompleted(IAsyncResult ar)
		{
			var request = (HttpWebRequest) ar.AsyncState;
			var response = request.EndGetResponse(ar);

			var responseStream = response.GetResponseStream();
			if (responseStream == null) return;
			using (var stream = new StreamReader(responseStream))
			{
				var readToEnd = stream.ReadToEnd();
				Assert.IsTrue(!string.IsNullOrEmpty(readToEnd));
			}
		}
	}
}