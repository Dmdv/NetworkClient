using System;

namespace GuiDownloader
{
	internal interface IDownloader
	{
		void StartDownload(string uri);
		void StartDownload(string uri, Action<double> action);
		void StartDownload(string uri, object argument);
	}
}