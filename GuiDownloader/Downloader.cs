using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
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
			var downloader = new HttpDownloader((string)uri);
			downloader.DownloadProgress += (o, args) => _view.UpdateProgress(args.PercentComplete);
			downloader.DownloadToFile(Path.GetTempFileName());
		}

		// Вместо массива можно создать анонимный тип и применить dynamic.
		private void WorkingUnitWithAction(object state)
		{
			var array = (object[]) state;
			if (array.Length == 0)
				throw new ArgumentException(@"state must be an array", "state");

			var uri = array[0];
			var action = (Action<double>)array[1];

			var downloader = new HttpDownloader((string)uri);
			downloader.DownloadProgress += (o, args) => action(args.PercentComplete);
			downloader.DownloadToFile(Path.GetTempFileName());
		}

		private void WorkingUnitWithArguments(object state)
		{
			var array = (object[])state;
			if (array.Length == 0)
				throw new ArgumentException(@"state must be an array", "state");

			var uri = array[0];
			var arg = array[1];

			var downloader = new HttpDownloader((string)uri);
			downloader.DownloadProgress += (o, args) => _view.UpdateProgress(args.PercentComplete, (ProgressBar) arg);
			downloader.DownloadToFile(Path.GetTempFileName());
		}

	}
}