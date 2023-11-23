using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LineCameraSheetSystem;
using Fujita.Misc;
namespace InspectionNameSpace
{
    /// <summary>「IniAutoInspection.ini」ファイルの設定データ</summary>
    public class IniAutoInspection
    {
        private string _filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "IniAutoInspection.ini");
        /// <summary>
        /// 情報画面を表示する・しない
        /// </summary>
        public bool InfoDispEnable { get; set; }
        /// <summary>
        /// 基準位置(mm)
        /// </summary>
        public double BasePoint { get; set; }
        /// <summary>
        /// NG集約範囲(mm)
        /// </summary>
        public double OverlapRange { get; set; }
        /// <summary>
        /// イメージ保存フォルダ最大数
        /// </summary>
        public int ImageNumDirMax { get; set; }
        /// <summary>
        /// イメージ保存ファイル最大数
        /// </summary>
        public int ImageNumDirFileMax { get; set; }

        /// <summary>
        /// イメージ同期キューMax数
        /// </summary>
        public int ImageSyncQueueCnt { get; set; }

        public AppData.InspID[] InspOrder { get; set; }

        public bool AutoKandoBrightEnabled { get; set; }
        public bool AutoKandoDarkEnabled { get; set; }
        public int AutoKandoLimit { get; set; }

        /// <summary>
        /// 最大・最小・平均算出位置（イメージ下部pix数）（自動調光用）
        /// </summary>
        public int ImageDataGetLine { get; set; }

        /// <summary>
        /// 基準カメラ
        /// </summary>
        public int CalibBaseCamera { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NGPositionMode { get; set; }
        /// <summary>
        /// 自動調光の時の画像保存先
        /// </summary>
        public string BmpSaveDir { get; set; }
        /// <summary>
        /// 自動調光時の画像保存の有効           
        /// </summary>
        public bool BmpSaveEnabled { get; set; } //21081214 moteki V1054

        public void Load()
        {
            IniFileAccessMisc ini = new IniFileAccessMisc();
            string file = this._filePath;

			this.InspOrder = new AppData.InspID[Enum.GetValues(typeof(AppData.InspID)).Length];

            string sectionName = "AutoInspection";
			this.InfoDispEnable = ini.GetIni(sectionName, "InfoDispEnable", true, file);
            this.BasePoint = ini.GetIni(sectionName, "BasePoint", 0.0, file);
            this.OverlapRange = ini.GetIni(sectionName, "OverlapRange", 1.0, file);
            this.ImageNumDirMax = ini.GetIni(sectionName, "ImageNumDirMax", 500, file);
            this.ImageNumDirFileMax = ini.GetIni(sectionName, "ImageNumDirFileMax", 1000, file);
            //this.MinMaxAveStartPos = ini.GetIni(sectionName, "MinMaxAveStartPos", 1548, file);
            //this.MinMaxAveEndPos = ini.GetIni(sectionName, "MinMaxAveEndPos", 2548, file);
			this.ImageSyncQueueCnt = ini.GetIni(sectionName, "ImageSyncQueueCnt", 5, file);
			foreach(AppData.InspID id in Enum.GetValues(typeof(AppData.InspID)))
			{
				string key = string.Format("InspOrder_{0}", (int)id);
				this.InspOrder[(int)id] = (AppData.InspID)ini.GetIniEnum(sectionName, key, typeof(AppData.InspID), file, id);
			}

			sectionName = "AutoKando";
			this.AutoKandoBrightEnabled = ini.GetIni(sectionName, "AutoKandoBrightEnabled", false, file);
			this.AutoKandoDarkEnabled = ini.GetIni(sectionName, "AutoKandoDarkEnabled", false, file);
			this.AutoKandoLimit = ini.GetIni(sectionName, "AutoKandoLimit", 30, file);

			sectionName = "ImageDatasParameter";
			this.ImageDataGetLine = ini.GetIni(sectionName, "ImageDataGetLine", 50, file);

			sectionName = "CalibBaseCamera";
			this.CalibBaseCamera = ini.GetIni(sectionName, "CalibBaseCamera", 0, file);

			sectionName = "NGPrameter";
			this.NGPositionMode = ini.GetIni(sectionName, "NGPositionMode", 0, file);

            sectionName = "BmpSave";
            this.BmpSaveDir = ini.GetIni(sectionName, "BmpSaveDir", @"c:\fujitaSheet\BmpSave", file);
            if (!Directory.Exists(this.BmpSaveDir))
                Directory.CreateDirectory(this.BmpSaveDir);
            this.BmpSaveEnabled = ini.GetIni(sectionName, "BmpSaveEnabled", false, file);       //20181214 moteki   V1054

        }
        public void Save()
        {
            IniFileAccessMisc ini = new IniFileAccessMisc();
            string file = this._filePath;

            string sectionName = "AutoInspection";
			ini.SetIni(sectionName, "InfoDispEnable", this.InfoDispEnable, file);
			ini.SetIni(sectionName, "BasePoint", this.BasePoint, file);
            ini.SetIni(sectionName, "OverlapRange", this.OverlapRange, file);
            ini.SetIni(sectionName, "ImageNumDirMax", this.ImageNumDirMax, file);
            ini.SetIni(sectionName, "ImageNumDirFileMax", this.ImageNumDirFileMax, file);
            //ini.SetIni(sectionName, "MinMaxAveStartPos", this.MinMaxAveStartPos, file);
            //ini.SetIni(sectionName, "MinMaxAveEndPos", this.MinMaxAveEndPos, file);
			ini.SetIni(sectionName, "ImageSyncQueueCnt", this.ImageSyncQueueCnt, file);
			foreach (AppData.InspID id in Enum.GetValues(typeof(AppData.InspID)))
			{
				string key = string.Format("InspOrder_{0}", (int)id);
				ini.SetIniEnum(sectionName, key, this.InspOrder[(int)id], file);
			}

			sectionName = "AutoKando";
			ini.SetIni(sectionName, "AutoKandoBrightEnabled", this.AutoKandoBrightEnabled, file);
			ini.SetIni(sectionName, "AutoKandoDarkEnabled", this.AutoKandoDarkEnabled, file);
			ini.SetIni(sectionName, "AutoKandoLimit", this.AutoKandoLimit, file);

			sectionName = "ImageDatasParameter";
			ini.SetIni(sectionName, "ImageDataGetLine", this.ImageDataGetLine, file);

			sectionName = "CalibBaseCamera";
			ini.SetIni(sectionName, "CalibBaseCamera", this.CalibBaseCamera, file);

			sectionName = "NGPrameter";
			ini.SetIni(sectionName, "NGPositionMode", this.NGPositionMode, file);

            sectionName = "BmpSave";
            ini.SetIni(sectionName, "BmpSaveDir", this.BmpSaveDir, file);
            ini.SetIni(sectionName, "BmpSaveEnabled", this.BmpSaveEnabled, file);   //20181214 moteki V1054
        }
    }
}
