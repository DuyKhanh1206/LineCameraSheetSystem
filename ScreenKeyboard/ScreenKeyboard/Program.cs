using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Fujita.ScreenKeyboard
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new testForm());


            //frmMain _frmmain = new frmMain();
            //_frmmain._strInputText = "test";

            //DialogResult dl;
            //dl =  _frmmain.ShowDialog();
            //string str = _frmmain._strOutputText;
        }
    }
}
