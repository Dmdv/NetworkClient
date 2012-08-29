using System;
using System.Linq;
using System.Windows.Forms;

namespace GuiDownloader
{
	public partial class MainForm : Form, IView
	{
		private const string Uri = @"http://www.gradsch.ohio-state.edu/Depo/ETD_Tutorial/lesson2.pdf";

		private readonly IDownloader _downloader;

		public MainForm()
		{
			InitializeComponent();
			foreach (var textBox in Controls.OfType<TextBox>())
			{
				textBox.Text = Uri;
			}
			_downloader = new Downloader(this);
		}

		/// <summary>
		/// This parameter should be a percentage.
		/// </summary>
		/// <param name="step">Step.</param>
		public void UpdateProgress(double step)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => progressBar1.Value = (int) step), null);
				return;
			}

			progressBar1.Value = (int) step;
		}

		public void UpdateProgress(double step, ProgressBar bar)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => bar.Value = (int) step), null);
				return;
			}

			bar.Value = (int) step;
		}

		private void DownloadSingleFile(object sender, EventArgs e)
		{
			progressBar1.Value = 0;
			_downloader.StartDownload(textBox1.Text);
		}

		// Для примера просто предположим, что у нас 4 textbox и 4 progressbar,
		// и что они имеют соответствие по порядковому номеру.
		private void DownloadAllFiles(object sender, EventArgs e)
		{
			var uris = Controls.OfType<TextBox>().Select(x => x.Text).ToList();
			var bars = Controls.OfType<ProgressBar>().ToList();

			var zip = uris.Zip(bars,
				(uri, bar) => new
				              	{
				              		Uri = uri,
									Action = new Action<double>(val => UpdateProgress(val, bar))
				              	});

			foreach (var item in zip)
			{
				_downloader.StartDownload(item.Uri, item.Action);
			}
		}
	}
}
