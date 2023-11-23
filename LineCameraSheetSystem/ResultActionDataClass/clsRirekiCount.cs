using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ResultActionDataClassNameSpace;

namespace LineCameraSheetSystem
{
    public class clsRirekiCount
    {
        private static clsRirekiCount _instance = new clsRirekiCount();
        public static clsRirekiCount getInstance()
        {
            return _instance;
        }

        private const string FILENAME = "result.cnt";

        public void Initialize()
        {
            Zone = new int[Enum.GetNames(typeof(AppData.SideID)).Length, Enum.GetNames(typeof(AppData.ZoneID)).Length];
            Items = new int[Enum.GetNames(typeof(AppData.SideID)).Length, Enum.GetNames(typeof(AppData.InspID)).Length];
            Cam = new int[Enum.GetNames(typeof(AppData.CamID)).Length];
        }


        public int[,] Zone { get; private set; }
        public int[,] Items { get; private set; }
        public int[] Cam { get; private set; }

        public void Load(string dir)
        {
            if (dir == null || dir == "")
                return;

            Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
            string path = Path.Combine(dir, FILENAME);
            string sec;
            string key;

            //カメラ部位別
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                //ゾーン別
                foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
                {
                    sec = "Zone";
                    key = string.Format("zone_{0}_{1}", side, zone);
                    Zone[(int)side, (int)zone] = ini.GetIni(sec, key, 0, path);
                }
                //項目別
                foreach (AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
                {
                    sec = "Items";
                    key = string.Format("items_{0}_{1}", side, inspId);
                    Items[(int)side, (int)inspId] = ini.GetIni(sec, key, 0, path);
                }
            }
            //カメラ別
            foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
            {
                sec = "Camera";
                key = string.Format("cam_{0}", cam);
                Cam[(int)cam] = ini.GetIni(sec, key, 0, path);
            }
        }

        public void Save(string dir, ResultActionDataClass resData)
        {
            if (dir == null || dir == "")
                return;
            if (Directory.Exists(dir) == false)
                return;

            Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
            string path = Path.Combine(dir, FILENAME);
            string sec;
            string key;

            //カメラ部位別
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                //ゾーン別
                foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
                {
                    sec = "Zone";
                    key = string.Format("zone_{0}_{1}", side, zone);
                    ini.SetIni(sec, key, resData.CountNGZone[(int)side, (int)zone], path);
                }
                //項目別
                foreach (AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
                {
                    sec = "Items";
                    key = string.Format("items_{0}_{1}", side, inspId);
                    ini.SetIni(sec, key, resData.CountNGItems[(int)side, (int)inspId], path);
                }
            }
            //カメラ別
            foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
            {
                sec = "Camera";
                key = string.Format("cam_{0}", cam);
                ini.SetIni(sec, key, resData.CountNGCamera[(int)cam], path);
            }
        }
    }
}
