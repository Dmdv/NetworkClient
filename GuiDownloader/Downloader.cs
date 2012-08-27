using System.IO;
using NetworkClient;

namespace GuiDownloader
{
	public class Downloader : IDownloader
	{
		private readonly IView _view;

		public Downloader(IView view)
		{
			_view = view;
		}

		public void StartDownload(string uri)
		{
			var downloader = new HttpDownloader(uri);
			downloader.DownloadProgress += DownloaderProgress;
			var tempFileName = Path.GetTempFileName();
			downloader.DownloadToFile(tempFileName);
		}

		private void DownloaderProgress(object sender, DownloadProgressArgs e)
		{
			_view.UpdateProgress(e.PercentComplete);
		}
	}
}