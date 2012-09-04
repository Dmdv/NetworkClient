using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GuiDownloader.Properties;
using NetworkClient;

namespace GuiDownloader
{
	public class Downloader : IDownloader
	{
		private readonly IView _view;
		private readonly HttpDownloaderOptions _options;

		public Downloader(IView view)
		{
			_view = view;
			_options = new HttpDownloaderOptions("GET",
			                                     Settings.Default.Timeout,
			                                     Settings.Default.RangeFrom,
			                                     Settings.Default.RangeTo,
			                                     Settings.Default.ProxyUri,
			                                     Settings.Default.ProxyPort);
		}

		public void StartDownload(string uri)
		{
			ThreadPool.QueueUserWorkItem(WorkingUnit, uri);
		}

		public void StartDownload(string uri, Action<double> action)
		{
			ThreadPool.QueueUserWorkItem(WorkingUnitWithAction, new object[] {uri, action});
		}

		public void StartDownload(string uri, object argument)
		{
			ThreadPool.QueueUserWorkItem(WorkingUnitWithArguments, new[] { uri, argument });
		}

		private void WorkingUnit(object uri)
		{
			var downloader = new HttpDownloader((string)uri, _options);
			downloader.DownloadProgress += (o, args) => _view.UpdateProgress(args.PercentComplete);
			downloader.DownloadToFile(Path.GetTempFileName());
		}

		// Вместо массива можно создать анонимный тип и применить dynamic.
	    private void WorkingUnitWithArguments(object state)
	    {
	        var array = (object[])state;
	        if (array.Length == 0)
	            throw new ArgumentException(Resources.WorkingUnitWithAction_state_must_be_an_array, "state");

	        var uri = array[0];
	        var arg = array[1];

			var downloader = new HttpDownloader((string)uri, _options);
			downloader.DownloadProgress += (o, args) => _view.UpdateProgress(args.PercentComplete, (ProgressBar)arg);
			downloader.DownloadToFile(Path.GetTempFileName());
	    }

	    private void WorkingUnitWithAction(object state)
		{
			var array = (object[]) state;
			if (array.Length == 0)
				throw new ArgumentException(Resources.WorkingUnitWithAction_state_must_be_an_array, "state");

			var uri = array[0];
			var action = (Action<double>)array[1];

			var downloader = new HttpDownloader((string)uri, _options);
			downloader.DownloadProgress += (o, args) => action(args.PercentComplete);
			downloader.DownloadToFile(Path.GetTempFileName());
		}
	}
}