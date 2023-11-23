using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.InspectionSystem
{
    static class FormMisc
    {
        public static string GetWindowTitle( string sTitle )
        {
            string sText;
            if (AppData.getInstance().param.EnableAuthenticationMode)
            {
                sText = AppData.getInstance().param.ApplicationName
                    + " " + AppData.getInstance().status.VersionFull
                    + " - " + sTitle
                    + " [" + AppData.getInstance().status.UserJpn + "]";
            }
            else
            {
                sText = AppData.getInstance().param.ApplicationName
                    + " " + AppData.getInstance().status.VersionFull
                    + " - " + sTitle;
            }
            return sText;
        }
    }
}
