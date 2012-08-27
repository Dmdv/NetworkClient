using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiDownloader
{
	public interface IView
	{
		void UpdateProgress(double step);
	}
}
