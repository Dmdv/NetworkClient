using System;
using System.Windows.Forms;

namespace GuiDownloader
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = (Exception)e.ExceptionObject;
			MessageBox.Show(string.Format("{0}: \r\n{1}", ex.Message, ex.InnerException.InnerException));
			Application.Exit();
		}
	}
}
