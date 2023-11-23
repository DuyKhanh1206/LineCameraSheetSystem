using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using Fujita.Misc;

namespace Fujita.FormMisc
{
    public static class clsControlSerialize
    {
        const string FILE_NAME = "control.ini";
        public enum ESerializeType
        {
            Position = 0x1,
            Size = 0x2,
            PositionSize = 0x3,
        }

        private static string getFormPath(Control frm)
        {
            if (frm.Parent != null)
                return getFormPath(frm.Parent) + ":" + frm.Name;
            return frm.Name;
        }

        private static string getExePath()
        {
            Assembly myAsm = Assembly.GetEntryAssembly();
            string path = myAsm.Location;
            return path.Substring(0, path.LastIndexOf("\\") + 1);
        }


        public static bool Store(Form form, ESerializeType type = ESerializeType.Position, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();

            string sName;
            if (bSingleton)
                sName = form.Name;
            else
                sName = getFormPath(form);

            string sExePath = getExePath();

            if ((type & ESerializeType.Position) != 0)
            {
                ifa.SetIni(sName, "Position", form.Location, sExePath + FILE_NAME);
            }
            
            if ((type & ESerializeType.Size) != 0)
            {
                ifa.SetIni(sName, "Bounds", form.Bounds, sExePath + FILE_NAME);
            }

            return true;
        }

        public static bool Store(CheckBox cb, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;
            if (bSingleton)
                sName = cb.Name;
            else
                sName = getFormPath(cb);
            
            string sExePath = getExePath();

            ifa.SetIni(sName, "Checked", cb.Checked, sExePath + FILE_NAME);
            return true;
        }


        public static bool Restore(CheckBox cb, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;
            if (bSingleton)
                sName = cb.Name;
            else
                sName = getFormPath(cb);
            string sExePath = getExePath();
            cb.Checked = ifa.GetIni(sName, "Checked", cb.Checked, sExePath + FILE_NAME);

            return true;
        }

        public static bool Store(RadioButton rb, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;
            if (bSingleton)
                sName = rb.Name;
            else
                sName = getFormPath(rb);

            string sExePath = getExePath();

            ifa.SetIni(sName, "Checked", rb.Checked, sExePath + FILE_NAME);
            return true;
        }

        public static bool Restore(RadioButton rb, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;
            if (bSingleton)
                sName = rb.Name;
            else
                sName = getFormPath(rb);
            string sExePath = getExePath();
            rb.Checked = ifa.GetIni(sName, "Checked", rb.Checked, sExePath + FILE_NAME);

            return true;
        }

        public static bool Restore(Form form, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;
            if( bSingleton )
                sName = form.Name;
            else
                sName = getFormPath(form);

            string sExePath = getExePath();

            Point pt = ifa.GetIni(sName, "Position", new Point(), sExePath + FILE_NAME);
            Rectangle rc = ifa.GetIni(sName, "Bounds", new Rectangle(), sExePath + FILE_NAME);

            if( pt != new System.Drawing.Point())
                form.Location = pt;
            if( rc != new System.Drawing.Rectangle())
                form.Bounds = rc;

            return true;
        }

        public static bool Store(ListView lv, bool bSingleton = false)
        {
            if (lv.View != View.Details)
                return true;

            IniFileAccess ifa = new IniFileAccess();
            string sName;

            if (bSingleton)
                sName = lv.Name;
            else
                sName = getFormPath(lv);
            string sExePath = getExePath();

            for (int i = 0; i < lv.Columns.Count; i++)
                ifa.SetIni(sName, "ColumnWidth" + (i + 1).ToString(), lv.Columns[i].Width, sExePath + FILE_NAME);
            return true;
        }

        public static bool Restore(ListView lv, bool bSingleton = false)
        {
            if (lv.View != View.Details)
                return true;

            IniFileAccess ifa = new IniFileAccess();
            string sName;

            if (bSingleton)
                sName = lv.Name;
            else
                sName = getFormPath(lv);

            string sExePath = getExePath();

            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns[i].Width = ifa.GetIni(sName, "ColumnWidth" + (i + 1).ToString(), lv.Columns[i].Width, sExePath + FILE_NAME);
            }
            return true;
        }

        public static bool Store(DataGridView dgv, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;

            if (bSingleton)
                sName = dgv.Name;
            else
                sName = getFormPath(dgv);
            string sExePath = getExePath();

            for (int i = 0; i < dgv.Columns.Count; i++)
                ifa.SetIni(sName, "DgvColumnWidth" + (i + 1).ToString(), dgv.Columns[i].Width, sExePath + FILE_NAME);
            return true;
        }

        public static bool Restore(DataGridView dgv, bool bSingleton = false)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sName;

            if (bSingleton)
                sName = dgv.Name;
            else
                sName = getFormPath(dgv);

            string sExePath = getExePath();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Width = ifa.GetIni(sName, "DgvColumnWidth" + (i + 1).ToString(), dgv.Columns[i].Width, sExePath + FILE_NAME);
            }
            return true;
        }

    }
}
