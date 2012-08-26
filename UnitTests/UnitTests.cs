using System;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void TestMethod()
		{
			var webRequest = WebRequest.Create("http://www.yandex.ru");
			webRequest.Method = "GET";
			webRequest.BeginGetResponse(OnRequestCompleted, webRequest);
			Thread.Sleep(1200000);
		}

		private void OnRequestCompleted(IAsyncResult ar)
		{
			var request = (HttpWebRequest) ar.AsyncState;
			var response = request.EndGetResponse(ar);

			var responseStream = response.GetResponseStream();
			if (responseStream != null)
				using (var stream = new StreamReader(responseStream))
				{
					var readToEnd = stream.ReadToEnd();
				}
		}
	}
}