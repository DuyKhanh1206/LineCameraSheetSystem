using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fujita.Misc;
using System.Reflection;
using System.IO;

namespace Fujita.InspectionSystem
{
    public class clsMainteFunc
    {
        public const string DEFALUT_MAINTE_DIR = "Mainte";
        public const string DEFAULT_MAINTE_FILE = "MainteFile";
        public const string DEFAULT_MAINTE_FILE_EXIST = ".ini";

        private static clsMainteFunc _singleton = new clsMainteFunc();
        public static clsMainteFunc getInstance()
        {
            return _singleton;
        }

        /// <summary>
        /// カメラパラメータClass
        /// </summary>
        public class CameraParameter
        {
            /// <summary>
            /// Gain値
            /// </summary>
            public int GainValue { get; set; }
            /// <summary>
            /// Offset値
            /// </summary>
            public int OffsetValue { get; set; }
            /// <summary>
            /// 露光値
            /// </summary>
            public int ExposureValue { get; set; }
            /// <summary>
            /// ラインレート
            /// </summary>
            public double LineRate { get; set; }

            /// <summary>
            /// Focus基準値
            /// </summary>
            public double FCBaseValue { get; set; }
            /// <summary>
            /// Focus枠
            /// </summary>
            public double FCRow1 { get; set; }
            public double FCColumn1 { get; set; }
            public double FCRow2 { get; set; }
            public double FCColumn2 { get; set; }

            /// <summary>
            /// ホワイトバランス枠
            /// </summary>
            public double WhiteBRow1 { get; set; }
            public double WhiteBColumn1 { get; set; }
            public double WhiteBRow2 { get; set; }
            public double WhiteBColumn2 { get; set; }

            /// <summary>
            /// ホワイトバランスを画像全体で行う
            /// </summary>
            public bool WhiteBImageAll { get; set; }

            public List<bool> FcLightEnabled { get; set; }
            public List<int> FcLightValue { get; set; }

            public List<bool> WhiteBLightEnabled { get; set; }
            public List<int> WhiteBLightValue { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="gain"></param>
            /// <param name="offset"></param>
            /// <param name="exposure"></param>
            public CameraParameter(int gain, int offset, int exposure, double lineRate, int lightNum)
            {
                GainValue = gain;
                OffsetValue = offset;
                ExposureValue = exposure;
                LineRate = lineRate;

                FCBaseValue = 0.0;
                FCRow1 = 100;
                FCColumn1 = 100;
                FCRow2 = 200;
                FCColumn2 = 300;

                WhiteBRow1 = 100;
                WhiteBColumn1 = 100;
                WhiteBRow2 = 200;
                WhiteBColumn2 = 300;

                WhiteBImageAll = true;

                FcLightEnabled = new List<bool>();
                FcLightValue = new List<int>();

                WhiteBLightEnabled = new List<bool>();
                WhiteBLightValue = new List<int>();

                for (int i=0; i<lightNum;i++)
                {
                    FcLightEnabled.Add(false);
                    FcLightValue.Add(0);
                    WhiteBLightEnabled.Add(false);
                    WhiteBLightValue.Add(0);
                }
            }
        }

        /// <summary>
        /// 照明パラメータClass
        /// </summary>
        public class LightParameter
        {
            /// <summary>
            /// カメラ№（0～
            /// </summary>
            public int CameraNumber { get; set; }
            /// <summary>
            /// 基準Gray値
            /// </summary>
            public int BaseGrayValue { get; set; }
            /// <summary>
            /// 基準照明(輝度)値
            /// </summary>
            public int BaseLightValue { get; set; }
            /// <summary>
            /// 基準との差（照明点灯時にこの差を加味して数値設定）
            /// 但し、CameraTest画面時は、加味しない
            /// </summary>
            public int DiffLightValue { get; set; }
            /// <summary>
            /// 差を算出した時の照明値
            /// </summary>
            public int CalcLightValue { get; set; }

            public double Row1 { get; set; }
            public double Column1 { get; set; }
            public double Row2 { get; set; }
            public double Column2 { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LightParameter()
            {
                CameraNumber = 0;
                BaseGrayValue = 128;
                BaseLightValue = 128;
                DiffLightValue = 0;
                CalcLightValue = 128;
                Row1 = 100;
                Column1 = 100;
                Row2 = 200;
                Column2 = 300;
            }
        }

        /// <summary>
        /// カメラパラメータ
        /// </summary>
        public List<CameraParameter> CamParam = new List<CameraParameter>();
        /// <summary>
        /// 照明パラメータ
        /// </summary>
        public List<LightParameter> LightParam = new List<LightParameter>();

        public string MaintePath { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public clsMainteFunc()
        {
            Assembly myAsm = Assembly.GetEntryAssembly();
            string path = myAsm.Location;
            string exePath = path.Substring(0, path.LastIndexOf("\\") + 1);
            //_exeName = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
            MaintePath = Path.Combine(exePath, DEFALUT_MAINTE_DIR);
            if (!Directory.Exists(this.MaintePath))
                Directory.CreateDirectory(this.MaintePath);
        }
        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="camNum"></param>
        /// <param name="lightNum"></param>
        public void Initialize(int camNum, int lightNum)
        {
            for (int i = 0; i < camNum; i++)
                CamParam.Add(new CameraParameter(0, 0, 0, 0.0, lightNum));
            for (int i = 0; i < lightNum; i++)
                LightParam.Add(new clsMainteFunc.LightParameter());
        }
        /// <summary>
        /// ロード                     trọng tải cài đặt thông số cho đèn và camera
        /// </summary>
        /// <param name="sPath"></param>
        public void Load(string sPath)
        {
            IniFileAccess ini = new IniFileAccess();
            string sSection;

            //グローバル
            sSection = "Global";
            int camCnt = ini.GetIni(sSection, "CameraCount", CamParam.Count, sPath);
            int lightCnt = ini.GetIni(sSection, "LightCount", LightParam.Count, sPath);

            int camMax = (camCnt > CamParam.Count) ? camCnt : CamParam.Count;
            int lightMax = (lightCnt > LightParam.Count) ? lightCnt : LightParam.Count;

            //カメラ           list camera.thông tin
            int i;
            for (i = 0; i < camMax; i++)
            {
                sSection = "Camera" + i.ToString();
                if (i >= CamParam.Count)
                {
                    CameraParameter camp = new CameraParameter(CamParam[0].GainValue, CamParam[0].OffsetValue, CamParam[0].ExposureValue, CamParam[0].LineRate, lightMax);
                    CamParam.Add(camp);
                }
                else
                {
                    CamParam[i].GainValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].GainValue), CamParam[i].GainValue, sPath);
                    CamParam[i].OffsetValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].OffsetValue), CamParam[i].OffsetValue, sPath);
                    CamParam[i].ExposureValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].ExposureValue), CamParam[i].ExposureValue, sPath);
                    CamParam[i].LineRate = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].LineRate), CamParam[i].LineRate, sPath);
                    CamParam[i].FCBaseValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCBaseValue), CamParam[i].FCBaseValue, sPath);
                    CamParam[i].FCRow1 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCRow1), CamParam[i].FCRow1, sPath);
                    CamParam[i].FCColumn1 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCColumn1), CamParam[i].FCColumn1, sPath);
                    CamParam[i].FCRow2 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCRow2), CamParam[i].FCRow2, sPath);
                    CamParam[i].FCColumn2 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCColumn2), CamParam[i].FCColumn2, sPath);
                    CamParam[i].WhiteBRow1 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBRow1), CamParam[i].WhiteBRow1, sPath);
                    CamParam[i].WhiteBColumn1 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBColumn1), CamParam[i].WhiteBColumn1, sPath);
                    CamParam[i].WhiteBRow2 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBRow2), CamParam[i].WhiteBRow2, sPath);
                    CamParam[i].WhiteBColumn2 = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBColumn2), CamParam[i].WhiteBColumn2, sPath);
                    CamParam[i].WhiteBImageAll = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBImageAll), CamParam[i].WhiteBImageAll, sPath);
                    CamParam[i].FcLightEnabled = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FcLightEnabled), "0,0,0,0,0,0,0,0", sPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
                    CamParam[i].FcLightValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].FcLightValue), "0,0,0,0,0,0,0,0", sPath).Split(new char[] { ',' }).Select<string, int>(x => int.Parse(x)).ToList();
                    CamParam[i].WhiteBLightEnabled = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBLightEnabled), "0,0,0,0,0,0,0,0", sPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
                    CamParam[i].WhiteBLightValue = ini.GetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBLightValue), "0,0,0,0,0,0,0,0", sPath).Split(new char[] { ',' }).Select<string, int>(x => int.Parse(x)).ToList();
                }
            }
            for (i = CamParam.Count - 1; i >= camCnt; i--)
            {
                CamParam.RemoveAt(i);
            }

            //照明 l          List đè.thông tin
            for (i = 0; i < lightCnt; i++)
            {
                sSection = "Light" + i.ToString();
                if (i >= LightParam.Count)
                {
                    LightParameter lghp = new LightParameter();
                    LightParam.Add(lghp);
                }
                else
                {
                    LightParam[i].CameraNumber = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].CameraNumber), LightParam[i].CameraNumber, sPath);
                    LightParam[i].BaseGrayValue = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].BaseGrayValue), LightParam[i].BaseGrayValue, sPath);
                    LightParam[i].BaseLightValue = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].BaseLightValue), LightParam[i].BaseLightValue, sPath);
                    LightParam[i].DiffLightValue = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].DiffLightValue), LightParam[i].DiffLightValue, sPath);
                    LightParam[i].CalcLightValue = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].CalcLightValue), LightParam[i].CalcLightValue, sPath);
                    LightParam[i].Row1 = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].Row1), LightParam[i].Row1, sPath);
                    LightParam[i].Column1 = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].Column1), LightParam[i].Column1, sPath);
                    LightParam[i].Row2 = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].Row2), LightParam[i].Row2, sPath);
                    LightParam[i].Column2 = ini.GetIni(sSection, GetNameClass.GetName(() => LightParam[i].Column2), LightParam[i].Column2, sPath);
                }
            }
            for (i = LightParam.Count - 1; i >= lightCnt; i--)
            {
                LightParam.RemoveAt(i);
            }
        }
        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="camCnt"></param>
        /// <param name="lightCnt"></param>
        public void Save(string sPath, int camCnt, int lightCnt)
        {
            IniFileAccess ini = new IniFileAccess();
            string sSection;

            //グローバル                 Lưu toàn cục
            sSection = "Global";
            ini.SetIni(sSection, "CameraCount", camCnt, sPath);
            ini.SetIni(sSection, "LightCount", lightCnt, sPath);

            //カメラ           lưu cục bộ từng camera   vào file  [DllImport("kernel32", CharSet = CharSet.Auto)]
            for (int i = 0; i < camCnt; i++)
            {
                sSection = "Camera" + i.ToString();
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].GainValue), CamParam[i].GainValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].OffsetValue), CamParam[i].OffsetValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].ExposureValue), CamParam[i].ExposureValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].LineRate), CamParam[i].LineRate, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCBaseValue), CamParam[i].FCBaseValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCRow1), CamParam[i].FCRow1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCColumn1), CamParam[i].FCColumn1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCRow2), CamParam[i].FCRow2, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FCColumn2), CamParam[i].FCColumn2, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBRow1), CamParam[i].WhiteBRow1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBColumn1), CamParam[i].WhiteBColumn1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBRow2), CamParam[i].WhiteBRow2, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBColumn2), CamParam[i].WhiteBColumn2, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBImageAll), CamParam[i].WhiteBImageAll, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FcLightEnabled), string.Join(",", CamParam[i].FcLightEnabled.Select(x => x.ToString()).ToArray()), sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].FcLightValue), string.Join(",", CamParam[i].FcLightValue.Select(x => x.ToString()).ToArray()), sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBLightEnabled), string.Join(",", CamParam[i].WhiteBLightEnabled.Select(x => x.ToString()).ToArray()), sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => CamParam[i].WhiteBLightValue), string.Join(",", CamParam[i].WhiteBLightValue.Select(x => x.ToString()).ToArray()), sPath);
            }

            //照明   Lưu cho từng đèn vào file  [DllImport("kernel32", CharSet = CharSet.Auto)]
            for (int i = 0; i < lightCnt; i++)
            {
                sSection = "Light" + i.ToString();
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].CameraNumber), LightParam[i].CameraNumber, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].BaseGrayValue), LightParam[i].BaseGrayValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].BaseLightValue), LightParam[i].BaseLightValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].DiffLightValue), LightParam[i].DiffLightValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].CalcLightValue), LightParam[i].CalcLightValue, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].Row1), LightParam[i].Row1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].Column1), LightParam[i].Column1, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].Row2), LightParam[i].Row2, sPath);
                ini.SetIni(sSection, GetNameClass.GetName(() => LightParam[i].Column2), LightParam[i].Column2, sPath);
            }
        }
    }
}
