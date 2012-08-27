using System;
using System.Windows.Forms;

namespace GuiDownloader
{
	public partial class MainForm : Form, IView
	{
		// http://www.gradsch.ohio-state.edu/Depo/ETD_Tutorial/lesson2.pdf

		private readonly IDownloader _downloader;

		public MainForm()
		{
			InitializeComponent();
			_downloader = new Downloader(this);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			progressBar1.Value = 0;
			_downloader.StartDownload(textBox1.Text);
		}

		/// <summary>
		/// This parameter should be a percentage.
		/// </summary>
		/// <param name="step">Step.</param>
		private void UpdateProgressInternal(double step)
		{
			progressBar1.Value = (int) step;
		}

		/// <summary>
		/// This parameter should be a percentage.
		/// </summary>
		/// <param name="step">Step.</param>
		public void UpdateProgress(double step)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => UpdateProgress(step)), step);
				return;
			}

			UpdateProgressInternal(step);
		}
	}
}
