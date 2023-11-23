using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Fujita.Misc;

namespace Fujita.ScreenKeyboard
{
    public class SystemSetting
    {
         //システム設定ファイル
        private const string SYSTEM_FILE = "scrnkeysystem.ini";
    
        private string GetSystemFilePath()
        {
            //実行ファイルパス
            string strAppPath = strAppPath = System.AppDomain.CurrentDomain.BaseDirectory;

            //システムファイルのパス
            string strSetting = Path.Combine(strAppPath, SYSTEM_FILE);

            return strSetting;
        }


        public void LoadKeyEng(Button btnSp1,Button btnSp2,Button btnSp3,Button btnSp4,Button btnSp5,Button btnSp6,Button btnSp7,Button btnSp8)
        {
            Button[] arrayBtn = new Button []{ btnSp1, btnSp2, btnSp3, btnSp4, btnSp5, btnSp6, btnSp7, btnSp8 };

            IniFileAccess ini = new IniFileAccess();
            String strPath = GetSystemFilePath();

            for (int i = 0; arrayBtn.Length > i; i++)
            {
                arrayBtn[i].Text = ini.GetIniString("KeyEng" + i.ToString(), "text", strPath, "");
                arrayBtn[i].Tag =  ini.GetIniString("KeyEng" + i.ToString(), "tag", strPath, "");
                if (arrayBtn[i].Tag.ToString() != "")
                {
                    arrayBtn[i].Visible = true;
                }
            }

        }

        public void LoadKeyJpn(Button btnSp1, Button btnSp2, Button btnSp3, Button btnSp4, Button btnSp5, Button btnSp6, Button btnSp7, Button btnSp8)
        {
            Button[] arrayBtn = new Button[] { btnSp1, btnSp2, btnSp3, btnSp4, btnSp5, btnSp6, btnSp7, btnSp8 };

            IniFileAccess ini = new IniFileAccess();
            String strPath = GetSystemFilePath();

            for (int i = 0; arrayBtn.Length > i; i++)
            {
                arrayBtn[i].Text = ini.GetIniString("KeyJpn" + i.ToString(), "text", strPath, "");
                arrayBtn[i].Tag = ini.GetIniString("KeyJpn" + i.ToString(), "tag", strPath, "");
                if (arrayBtn[i].Tag.ToString() != "")
                {
                    arrayBtn[i].Visible = true;
                }
            }

        }

        //ひらがなならtrue
        public bool LoadStatLanguage()
        {
            IniFileAccess ini = new IniFileAccess();
            string strPath = GetSystemFilePath();

            return ini.GetIniBoolean("global", "Start", strPath, true);
        }

        public bool KeyReturn()
        {
            IniFileAccess ini = new IniFileAccess();
            string strPath = GetSystemFilePath();

            return ini.GetIniBoolean("global", "KeyReturn", strPath, true);
        }

 	public List<char> LoadNgKey()
        {
            
            IniFileAccess ini = new IniFileAccess();
            string strPath = GetSystemFilePath();

            List<char> listChar= new List<char>();
            string str = ini.GetIniString("global","NgKey",strPath,"");

            for (int i = 0 ; str.Length>i;i++)
            {
                listChar.Add(str[i]);
            }


            return listChar;
        }

    }
}
