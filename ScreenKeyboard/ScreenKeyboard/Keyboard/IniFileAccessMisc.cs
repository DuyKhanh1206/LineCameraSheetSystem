using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.Misc
{
    class IniFileAccessMisc : IniFileAccess
    {
        public int SetIni(string SectionName, string KeyName, CRectangle1 rcRect1, string FileName)
        {
            return SetIniRectangle1(SectionName, KeyName, rcRect1, FileName);
        }

        public int SetIni(string SectionName, string KeyName, CRectangle2 rcRect2, string FileName)
        {
            return SetIniRectangle2(SectionName, KeyName, rcRect2, FileName);
        }

        public int SetIniRectangle1(string sSectionName, string sKeyName, CRectangle1 rc, string sFileName)
        {
            return SetIniString(sSectionName, sKeyName, rc.ToString(), sFileName);
        }

        public bool GetIni(string SectionName, string KeyName, ref CRectangle1 rcRect1, CRectangle1 DefValue, string FileName)
        {
            return GetIniRectangle1(SectionName, KeyName, ref rcRect1, FileName, DefValue);
        }

        public bool GetIni(string SectionName, string KeyName, ref CRectangle2 rcRect2, CRectangle2 DefValue, string FileName)
        {
            return GetIniRectangle2(SectionName, KeyName, ref rcRect2, FileName, DefValue);
        }


        public bool GetIniRectangle1(string sSectionName, string sKeyName, ref CRectangle1 rc, string sFileName, CRectangle1 rcDef)
        {
            string sRet = GetIniString(sSectionName, sKeyName, sFileName, "");

            rc.X = rcDef.X;
            rc.Y = rcDef.Y;
            rc.Width = rcDef.Width;
            rc.Height = rcDef.Height;

            if (sRet == "")
                return false;

            string[] saDiv = sRet.Split(new string[] { "," }, StringSplitOptions.None);

            if (saDiv.Length != 4)
                return false;

            double dBuf;
            if (!double.TryParse(saDiv[0], out dBuf))
                return false;
            rc.X = dBuf;
            if (!double.TryParse(saDiv[1], out dBuf))
                return false;
            rc.Y = dBuf;
            if (!double.TryParse(saDiv[2], out dBuf))
                return false;
            rc.Width = dBuf;
            if (!double.TryParse(saDiv[3], out dBuf))
                return false;
            rc.Height = dBuf;
            return true;
        }

        public int SetIniRectangle2(string sSectionName, string sKeyName, CRectangle2 rc, string sFileName)
        {
            return SetIniString(sSectionName, sKeyName, rc.ToString(), sFileName);
        }

        public bool GetIniRectangle2(string sSectionName, string sKeyName, ref CRectangle2 rc, string sFileName, CRectangle2 rcDef)
        {
            string sRet = GetIniString(sSectionName, sKeyName, sFileName, "");

            rc.Row = rcDef.Row;
            rc.Col = rcDef.Col;
            rc.Phi = rcDef.Phi;
            rc.Length1 = rcDef.Length1;
            rc.Length2 = rcDef.Length2;

            if (sRet == "")
                return false;

            string[] saDiv = sRet.Split(new string[] { "," }, StringSplitOptions.None);

            if (saDiv.Length != 5)
                return false;

            double dBuf;
            if (!double.TryParse(saDiv[0], out dBuf))
                return false;
            rc.Row = dBuf;
            if (!double.TryParse(saDiv[1], out dBuf))
                return false;
            rc.Col = dBuf;
            if (!double.TryParse(saDiv[2], out dBuf))
                return false;
            rc.Phi = dBuf;
            if (!double.TryParse(saDiv[3], out dBuf))
                return false;
            rc.Length1 = dBuf;
            if (!double.TryParse(saDiv[4], out dBuf))
                return false;
            rc.Length2 = dBuf;
            return true;
        }
    }
}
