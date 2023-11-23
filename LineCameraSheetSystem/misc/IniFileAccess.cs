using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Fujita.Misc
{
    //INIファイルI/Oクラス
    class IniFileAccess
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern uint GetPrivateProfileSection(string lpAppName,
           IntPtr lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault,
            char[] lpReturnedString,int nSize,string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileInt(
            string lpAppName, string lpKeyName,
            int nDefault, string lpFileName);

        // <summary> ghi thông số camera và đèn </summary>
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(
            string lpAppName,string lpKeyName,
            string lpString,string lpFileName);


        [DllImport("kernel32.dll")]
        static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
           uint nSize, string lpFileName);

        //INIファイルから文字列を読込む
        public string GetIniString(string SectionName, string KeyName, string FileName, string Defaults)
        {
            char[] buf = new char[1024];
            string Result = "";
            int ret = GetPrivateProfileString(SectionName, KeyName, Defaults, buf, buf.Length, FileName);

            Result = new string(buf,0,ret); 

            return Result;
        }


        public string[] GetIniSectionNames(string FileName)
        {
            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, FileName);
            if (bytesReturned == 0)
            {
                Marshal.FreeCoTaskMem(pReturnedString);
                return null;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            return local.Substring(0, local.Length - 1).Split('\0');
        }

        public void Flush(string FileName)
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //INIファイルに文字列を書込む
        public int SetIniString(string SectionName, string KeyName, string SetValue, string FileName)
        {
            int ret = WritePrivateProfileString(SectionName, KeyName, SetValue, FileName);
            return ret;
        }

        public int SetIni(string SectionName, string KeyName, int SetValue, string FileName)
        {
            int ret = WritePrivateProfileString(SectionName, KeyName, SetValue.ToString(), FileName);
            return ret;
        }

        public int SetIni(string SectionName, string KeyName, double SetValue, string FileName)
        {
            int ret = WritePrivateProfileString(SectionName, KeyName, SetValue.ToString(), FileName);
            return ret;
        }

        public int SetIni(string SectionName, string KeyName, string SetValue, string FileName)
        {
            int ret = WritePrivateProfileString(SectionName, KeyName, SetValue, FileName);
            return ret;
        }

        public int SetIni(string SectionName, string KeyName, bool SetValue, string FileName)
        {
            return SetIniBoolean(SectionName, KeyName, SetValue, FileName);
        }

        public int SetIni(string SectionName, string KeyName, Enum SetValue, string FileName)
        {
            return SetIniEnum(SectionName, KeyName, SetValue, FileName);
        }

        public int SetIni(string SectionName, string KeyName, System.Drawing.Point pt, string FileName)
        {
            return SetIniPoint(SectionName, KeyName, FileName, pt);
        }

        public System.Drawing.Point GetIni(string SectionName, string KeyName, System.Drawing.Point pt, string FileName)
        {
            return GetIniPoint(SectionName, KeyName, FileName, pt);
        }

        public int SetIni(string SectionName, string KeyName, System.Drawing.PointF pt, string FileName)
        {
            return SetIniPointF(SectionName, KeyName, FileName, pt);
        }

        public System.Drawing.PointF GetIni(string SectionName, string KeyName, System.Drawing.PointF pt, string FileName)
        {
            return GetIniPointF(SectionName, KeyName, FileName, pt);
        }

        public int SetIni(string SectionName, string KeyName, System.Drawing.Rectangle pt, string FileName)
        {
            return SetIniRectangle(SectionName, KeyName, FileName, pt);
        }

        public System.Drawing.Rectangle GetIni(string SectionName, string KeyName, System.Drawing.Rectangle pt, string FileName)
        {
            return GetIniRectangle(SectionName, KeyName, FileName, pt);
        }
        // kiểm tra thông tin đúng và có trong tệp init thì trả về true
        public bool GetIni(string SectionName, string KeyName, bool DefValue, string FileName)
        {
            return GetIniBoolean(SectionName, KeyName, FileName, DefValue);
        }

        public int GetIni(string SectionName, string KeyName, int DefValue, string FileName)
        {
            return GetIniInt(SectionName, KeyName, FileName, DefValue);
        }

        public double GetIni(string SectionName, string KeyName, double DefValue, string FileName)
        {
            return GetIniDouble(SectionName, KeyName, FileName, DefValue);
        }

        public string GetIni(string SectionName, string KeyName, string DefValue, string FileName)
        {
            return GetIniString(SectionName, KeyName, FileName, DefValue);
        }

        public Enum GetIni(string SectionName, string KeyName, Type EnumType, Enum DefValue, string FileName)
        {
            return (Enum)GetIniEnum(SectionName, KeyName, EnumType, FileName, DefValue);
        }

        //INIファイルから数値(Integer)を読込む      Đọc số (Số nguyên) từ tệp INI
        public int GetIniInt(string SectionName, string KeyName, string FileName, int Defaults)
        {
            //(phát đầu đọc light thì trả về số lượng light) nếu k có thì mặc đinh là 0 đọc từ file kernel32 thuộc lớp IniFileAccess
            int ret = GetPrivateProfileInt(SectionName, KeyName, Defaults, FileName);
            return ret;
        }

        public double GetIniDouble(string SectionName, string KeyName, string FileName, double Defaults)
        {
            string sValue = GetIniString(SectionName, KeyName, FileName, "");

            double dValue;
            if( sValue == "" || !double.TryParse( sValue, out dValue ))
            {
                return Defaults;
            }
            return dValue;
        }

        // INIファイルから、カンマ区切りのデータ列を読み込む
        public bool GetIniArray(string sSectionname, string sKeyName, string sFileName, out object[] oaParam)
        {
            oaParam = null;
            string sValue = GetIniString(sSectionname, sKeyName, sFileName, "");
            if (sValue == "")
                return false;

            //文字列の中から未使用コードを取得する
            string sReplChars = "@`[{;+:*]}<>/?_!#$%&'()=~^~|";
            char [] cReplaceCode = new char[2]{'0','1'};
            int iCnt = 0;
            for (int i = 0; i < sReplChars.Length; i++)
            {
                if (sValue.IndexOf(sReplChars[i]) == -1)
                {
                    cReplaceCode[iCnt] = sReplChars[i];
                    ++iCnt;
                    if (iCnt == 2)
                        break;
                }
            }

            // エスケープクオートは変換しておく
            string sBuf = sValue.Replace("\\\"", cReplaceCode[0].ToString());
            StringBuilder sb = new StringBuilder(sBuf);
            int iStart = 0;
            bool bFirst = true;
            int iFirstIndex = -1;
            // "で囲われる,では、変換を行わない為、変換する
            for (int iIndex = sBuf.IndexOf('"', iStart); iIndex != -1; )
            {
                if (bFirst)
                {
                    iFirstIndex = iIndex;
                    bFirst = false;
                }
                else
                {
                    sBuf = sb.Replace( ',', cReplaceCode[1], iFirstIndex, iIndex - iFirstIndex ).ToString();
                    iStart = iIndex + 1;
                    bFirst = true;
                }
            }

            // 変換が終わったので、文字を分ける
            string [] saVal = sBuf.Split( ',' );
            oaParam = new object[ saVal.Length];

            int iResult;
            double dResult;
            for( int i = 0; i < saVal.Length; i++ )
            {
                string sVal = saVal[i].Trim();
                if( sVal == "" )        // 空文字列
                {
                    oaParam[i] = sVal;
                }
                else if( sVal[0] == '"' ) // クオート始まり
                {
                    oaParam[i] = sVal.Trim( new char[]{'"'}).Replace( cReplaceCode[1], ',').Replace( cReplaceCode[0], '\"');
                }
                else if( int.TryParse( sVal, out iResult )) // int変換可能
                {
                    oaParam[i] = iResult;
                }
                else if( double.TryParse( sVal, out dResult )) // double 変換可能
                {
                    oaParam[i] = dResult;
                }
                else // それ以外
                {
                    oaParam[i] = sVal.Replace( cReplaceCode[1], ',' ).Replace( cReplaceCode[0], '\"' );
                }
            }
            return true;
        }


        //INIファイルにEnum型を読み込む
        public object GetIniEnum(string SectionName, string KeyName, Type typ, string FileName, System.Enum edef )
        {
            string sEnumData = GetIniString(SectionName, KeyName, FileName, edef.ToString());
            return Enum.Parse(typ, sEnumData);

        }

        //INIファイルからEnum型を書き込む
        public int SetIniEnum(string SectionName, string KeyName, System.Enum e, string FileName)
        {
            return WritePrivateProfileString(SectionName, KeyName, e.ToString(), FileName);
        }



        public bool GetIniSection(string appName, string fileName, out string[] section)
        {
            section = null;

            if (!System.IO.File.Exists(fileName))
                return false;

            uint MAX_BUFFER = 32767;

            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));

            uint bytesReturned = GetPrivateProfileSection(appName, pReturnedString, MAX_BUFFER, fileName);

            if ((bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
            {
                Marshal.FreeCoTaskMem(pReturnedString);
                return false;
            }

            //bytesReturned -1 to remove trailing \0

            // NOTE: Calling Marshal.PtrToStringAuto(pReturnedString) will
            //       result in only the first pair being returned
            string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)(bytesReturned - 1));

            section = returnedString.Split('\0');

            Marshal.FreeCoTaskMem(pReturnedString);

            return true;
        }

        public bool GetIniSection(string appName, string filename, out Dictionary<string, string> sections, bool trim_quote = true )
        {
            string[] saSection;
            sections = new Dictionary<string, string>();
            if (GetIniSection(appName, filename, out saSection))
            {
                foreach (string s in saSection)
                {
                    string[] sDiv = s.quoteStart().Split('=');
                    if (sDiv.Length == 2)
                    {
                        if (trim_quote)
                            sections.Add(sDiv[0].quoteBack().Trim(), sDiv[1].quoteBack().Trim(new char[] { ' ', '"'}));
                        else
                            sections.Add(sDiv[0].quoteBack().Trim(), sDiv[1].quoteBack().Trim(new char[] { ' '}));
                    }
                }
                return true;
            }
            return false;
        }
        // từ tệp INIT nếu = 1 thì trả về true
        public bool GetIniBoolean(string sSection, string sKeyName, string sFileName, bool bDefault)
        {
            return GetIniInt(sSection, sKeyName, sFileName, bDefault ? 1 : 0) == 1 ? true : false;
        }

        public int SetIniBoolean(string sSection, string sKeyName, bool bValue, string sFileName)
        {
            return SetIniString(sSection, sKeyName, bValue ? "1" : "0", sFileName);
        }

        public int SetIniPoint(string sSection, string sKeyName, string sFileName, System.Drawing.Point pt)
        {
            return SetIniString(sSection, sKeyName, pt.X.ToString() + "," + pt.Y.ToString(), sFileName); 
        }

        public System.Drawing.Point GetIniPoint(string sSection, string sKeyName, string sFileName, System.Drawing.Point ptDefault)
        {
            System.Drawing.Point pt = new System.Drawing.Point( int.MinValue, int.MinValue );

            string sRet = GetIniString(sSection, sKeyName, sFileName, "");
            if (sRet != "")
            {
                string[] ptParams = sRet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

                if (ptParams.Length == 2)
                {
                    int x, y;
                    if (int.TryParse(ptParams[0], out x) && int.TryParse(ptParams[1], out y))
                    {
                        pt.X = x;
                        pt.Y = y;
                    }
                }
            }

            if (pt.X == int.MinValue && pt.Y == int.MinValue)
            {
                pt.X = ptDefault.X;
                pt.Y = ptDefault.Y;
            }
            return pt;
        }

        public int SetIniPointF(string sSection, string sKeyName, string sFileName, System.Drawing.PointF pt)
        {
            return SetIniString(sSection, sKeyName, pt.X.ToString() + "," + pt.Y.ToString(), sFileName);
        }

        public System.Drawing.PointF GetIniPointF(string sSection, string sKeyName, string sFileName, System.Drawing.PointF ptDefault)
        {
            System.Drawing.PointF pt = new System.Drawing.PointF(float.MinValue, float.MinValue);

            string sRet = GetIniString(sSection, sKeyName, sFileName, "");
            if (sRet != "")
            {
                string[] ptParams = sRet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ptParams.Length == 2)
                {
                    float x, y;
                    if (float.TryParse(ptParams[0], out x) && float.TryParse(ptParams[1], out y))
                    {
                        pt.X = x;
                        pt.Y = y;
                    }
                }
            }

            if (pt.X == int.MinValue && pt.Y == int.MinValue)
            {
                pt.X = ptDefault.X;
                pt.Y = ptDefault.Y;
            }
            return pt;
        }

        public int SetIniRectangle(string sSection, string sKeyName, string sFileName, System.Drawing.Rectangle rc)
        {
            return SetIniString(sSection, sKeyName, rc.X.ToString() + "," + rc.Y.ToString() + "," + rc.Width.ToString() + "," + rc.Height.ToString(), sFileName);
        }

        public System.Drawing.Rectangle GetIniRectangle(string sSection, string sKeyName, string sFileName, System.Drawing.Rectangle rcDefault)
        {
            System.Drawing.Rectangle rc = new System.Drawing.Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

            string sRet = GetIniString(sSection, sKeyName, sFileName, "");
            if (sRet != "")
            {
                string[] ptParams = sRet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ptParams.Length == 4)
                {
                    int x, y,w,h;
                    if (int.TryParse(ptParams[0], out x)
                        && int.TryParse(ptParams[1], out y)
                        && int.TryParse(ptParams[2], out w)
                        && int.TryParse(ptParams[3], out h)
                        )
                    {
                        rc.X = x;
                        rc.Y = y;
                        rc.Width = w;
                        rc.Height = h;
                    }
                }
            }

            if (rc.X == int.MinValue && rc.Y == int.MinValue)
            {
                rc.X = rcDefault.X;
                rc.Y = rcDefault.Y;
                rc.Width = rcDefault.Width;
                rc.Height = rcDefault.Height;
            }
            return rc;
        }
    }

    internal static class StringFileAccessExt
    {
        public static string quoteStart(this string s)
        {
            int iFirst = s.IndexOf('"');
            int iLast = s.LastIndexOf('"');

            if (iFirst != -1 && iLast != -1)
            {
                string sSub = s.Substring(iFirst, iLast - iFirst);
                string sReplace = sSub.Replace("=", "[&equ]");
                return s.Replace(sSub, sReplace);
            }
            return s;
        }

        public static string quoteBack(this string s)
        {
            return s.Replace("[&equ]", "=");
        }
    }
}
