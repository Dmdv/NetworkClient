using System.Windows.Forms;

namespace GuiDownloader
{
	public interface IView
	{
		void UpdateProgress(double step);
		void UpdateProgress(double step, ProgressBar bar);
	}
}
