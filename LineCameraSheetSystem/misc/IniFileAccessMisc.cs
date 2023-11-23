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

            double dX, dY, dW, dH;
            if (!double.TryParse(saDiv[0], out dX))
                return false;
            if (!double.TryParse(saDiv[1], out dY))
                return false;
            if (!double.TryParse(saDiv[2], out dW))
                return false;
            if (!double.TryParse(saDiv[3], out dH))
                return false;
            rc.X = dX;
            rc.Y = dY;
            rc.Width = dW;
            rc.Height = dH;
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

            double dR, dC, dPhi, dLen1, dLen2;
            if (!double.TryParse(saDiv[0], out dR))
                return false;
            if (!double.TryParse(saDiv[1], out dC))
                return false;
            if (!double.TryParse(saDiv[2], out dPhi))
                return false;
            if (!double.TryParse(saDiv[3], out dLen1))
                return false;
            if (!double.TryParse(saDiv[4], out dLen2))
                return false;
            rc.Row = dR;
            rc.Col = dC;
            rc.Phi = dPhi;
            rc.Length1 = dLen1;
            rc.Length2 = dLen2;
            return true;
        }
    }
}
