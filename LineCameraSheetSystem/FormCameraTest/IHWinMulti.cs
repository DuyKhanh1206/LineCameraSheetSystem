using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewROI;
using System.Drawing;

namespace Fujita.InspectionSystem
{
	public delegate void WindowClickEventHander(object sender, WindowClickEventArgs e);

	interface IHWinMulti
	{
		event WindowClickEventHander WindowClick;

		int WindowNum { get; set; }
		void SetWindowIndex(List<int> lstIndex);
		void LayoutDefault();
		HWndCtrl GetWindowControl(int i);
		bool LayoutOne(int iWindowNo);
		bool SetWindowName(int i, string sName);
        Size GetSizeLayoutOne();
        Size GetSizeLayoutMulti();
	}

	public class WindowClickEventArgs : EventArgs
	{
		public HWndCtrl Window { get; private set; }
		public int WindowIndex { get; private set; }

		public WindowClickEventArgs(HWndCtrl window, int windowindex)
		{
			Window = window;
			WindowIndex = windowindex;
		}
	}
}
