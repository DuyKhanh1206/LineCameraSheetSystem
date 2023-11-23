using System;
using System.Runtime.InteropServices;

namespace LogingDllWrap
{
	public class LogingDll
	{
		// -----------------------------------------------------------------------
		//	DllImport API
		// -----------------------------------------------------------------------
#if !x64
		[DllImport("LogingDLL.dll")]
		public static extern	bool	Loging_Init(string lpcStr, string lpcHeader);
		[DllImport("LogingDLL.dll")]
		public static extern	void	Loging_End();
		[DllImport("LogingDLL.dll")]
		public static extern	bool	Loging_SetLogString(string stData);
        [DllImport("LogingDLL.dll")]
        public static extern bool Loging_SetLogFormatString(string cFormat, __arglist);
#else
        [DllImport("LogingDLL64.dll")]
        public static extern bool Loging_Init(string lpcStr, string lpcHeader);
        [DllImport("LogingDLL64.dll")]
        public static extern void Loging_End();
        [DllImport("LogingDLL64.dll")]
        public static extern bool Loging_SetLogString(string stData);
        [DllImport("LogingDLL64.dll")]
        public static extern bool Loging_SetLogFormatString(string cFormat, __arglist);
#endif
	}
}
