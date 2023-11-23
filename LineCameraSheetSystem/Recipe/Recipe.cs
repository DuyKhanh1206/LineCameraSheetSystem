using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fujita.InspectionSystem;
using Fujita.Misc;
using Fujita.LightControl;
using System.IO;
using System.Collections;

namespace LineCameraSheetSystem
{
    public class Recipe
    {
        private static Recipe _singleton = new Recipe();

        public enum InspTypes
        {
            透過,
            反射,
            透過反射
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Recipe()
        {
        }
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <returns></returns>
        public static Recipe GetInstance()
        {
            return _singleton;
        }

        /// <summary>
        /// 品種名
        /// </summary>
        public string KindName { get; set; }

        /// <summary>
        /// ﾘｽﾄの番号
        /// </summary>   
        public string SelectItem { get; set; }

        /// <summary>
        /// 表裏感度共通
        /// </summary>
        public bool UpDownSideCommon { get; set; }

        /// <summary>
        /// 分割数
        /// </summary>
        public int Partition { get; set; }
        /// <summary>
        /// 両端任意指定
        /// </summary>
        public bool IsBothEndAny { get; set; }
        /// <summary>
        /// 共通検査領域を使用する(true:共通　false:個別)　
        /// </summary>
        public bool CommonInspAreaEnable { get; set; }

        /// <summary>
        /// 感度・幅・ゾーン
        /// </summary>
        public List<InspParameter> InspParam = new List<InspParameter>();

        /// <summary>
        /// 照明コントロール
        /// </summary>
        public List<LightParameter> LightParam = new List<LightParameter>();

        /// <summary>
        /// 表面検査有効
        /// </summary>
        public bool UpSideInspEnable { get; set; }      //20181202 moteki   V1053

        /// <summary>
        /// 裏面検査有効
        /// </summary>
        public bool DownsideInspEnable { get; set; }    //20181202 moteki   V1053

        #region 外部出力
        //レシピ設定値を有効にする
        public bool ExternalEnable { get; set; }
        //外部出力1の時間
        public int ExternalResetTime1 { get; set; }
        //外部出力2の時間
        public int ExternalResetTime2 { get; set; }
        //外部出力3の時間
        public int ExternalResetTime3 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部３追加
        //外部出力4の時間
        public int ExternalResetTime4 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部４追加
        //外部出力1のディレイ時間
        public int ExternalDelayTime1 { get; set; }
        //外部出力2のディレイ時間
        public int ExternalDelayTime2 { get; set; }
        //外部出力3のディレイ時間
        public int ExternalDelayTime3 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部３追加
        //外部出力4のディレイ時間
        public int ExternalDelayTime4 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部４追加
        //外部出力1のショット数
        public int ExternalShot1 { get; set; }
        //外部出力2のショット数
        public int ExternalShot2 { get; set; }
        //外部出力3のショット数
        public int ExternalShot3 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部３追加
        //外部出力4のショット数
        public int ExternalShot4 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部４追加
        #endregion

        #region 実速度
        public bool UseCommonCamRealSpeed { get; set; }
        public double CamRealSpeedValue { get; set; }
        public double CamRealSpeedValueUra { get; set; }
        #endregion

        #region 速度
        //レシピ設定値を有効にする
        public bool CamSpeedEnable { get; set; }
        //速度(m/分)
        public double CamSpeedValue { get; set; }
        public double CamSpeedValueUra { get; set; }
        #endregion

        #region 露光
        //レシピ設定値を有効にする
        public bool CamExposureEnable { get; set; }
        //露光
        public double CamExposureValue { get; set; }
        public double CamExposureValueUra { get; set; }
        #endregion

        #region ゲイン
        public double CamGainOmote { get; set; }
        public double CamGainUra { get; set; }
        #endregion

        #region 岐阜カスタム パトライト設定 v1326
        /// <summary>パトライト有効</summary>
        public bool PatLiteEnable { get; set; }
        /// <summary>パトライトディレイ時間（秒）</summary>
        public int PatLiteDelay { get; set; }
        /// <summary>パトライトオン時間（秒）</summary>
        public int PatLiteOnTime { get; set; }

        #endregion

        /// <summary>
        /// 更新フラグ
        /// </summary>
        public bool Modify { get; set; }
        /// <summary>
        /// 品種保存パス
        /// </summary>        
        public string Path { get; set; }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="recpi"></param>
        public void Copy(Recipe recpi)
        {
            //名称
            recpi.KindName = this.KindName;
            //番号
            recpi.SelectItem = this.SelectItem;
            //表裏共通
            recpi.UpDownSideCommon = this.UpDownSideCommon;
            //分割数
            recpi.Partition = this.Partition;
            //両端任意指定
            recpi.IsBothEndAny = this.IsBothEndAny;
            //共通検査領域を使用する
            recpi.CommonInspAreaEnable = this.CommonInspAreaEnable;
            //感度・幅・ゾーン
            for (int i = 0; i < recpi.InspParam.Count; i++)
                this.InspParam[i].Copy(recpi.InspParam[i]);
            //照明
            for (int i = 0; i < recpi.LightParam.Count; i++)
                this.LightParam[i].Copy(recpi.LightParam[i]);
            //表面検査有効
            recpi.UpSideInspEnable = this.UpSideInspEnable; //20181202 moteki   V1053
            //裏面検査有効
            recpi.DownsideInspEnable = this.DownsideInspEnable; //20181202 moteki   V1053
            //外部出力
            recpi.ExternalEnable = this.ExternalEnable;
            recpi.ExternalDelayTime1 = this.ExternalDelayTime1;
            recpi.ExternalDelayTime2 = this.ExternalDelayTime2;
            recpi.ExternalDelayTime3 = this.ExternalDelayTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recpi.ExternalDelayTime4 = this.ExternalDelayTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            recpi.ExternalResetTime1 = this.ExternalResetTime1;
            recpi.ExternalResetTime2 = this.ExternalResetTime2;
            recpi.ExternalResetTime3 = this.ExternalResetTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recpi.ExternalResetTime4 = this.ExternalResetTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            recpi.ExternalShot1 = this.ExternalShot1;
            recpi.ExternalShot2 = this.ExternalShot2;
            recpi.ExternalShot3 = this.ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recpi.ExternalShot4 = this.ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            //実速度
            recpi.UseCommonCamRealSpeed = this.UseCommonCamRealSpeed;
            recpi.CamRealSpeedValue = this.CamRealSpeedValue;
            recpi.CamRealSpeedValueUra = this.CamRealSpeedValueUra;
            //速度
            recpi.CamSpeedEnable = this.CamSpeedEnable;
            recpi.CamSpeedValue = this.CamSpeedValue;
            recpi.CamSpeedValueUra = this.CamSpeedValueUra;
            //露光
            recpi.CamExposureEnable = this.CamExposureEnable;
            recpi.CamExposureValue = this.CamExposureValue;
            recpi.CamExposureValueUra = this.CamExposureValueUra;
            //ゲイン
            recpi.CamGainOmote = this.CamGainOmote;
            recpi.CamGainUra = this.CamGainUra;
            //岐阜カスタム パトライト設定 v1326
            recpi.PatLiteEnable = this.PatLiteEnable;
            recpi.PatLiteDelay = this.PatLiteDelay;
            recpi.PatLiteOnTime = this.PatLiteOnTime;
            //更新フラグ
            recpi.Modify = this.Modify;
            //保存パス
            recpi.Path = this.Path;

            return;
        }
        /// <summary>
        /// オブジェクト生成してコピーする
        /// </summary>
        /// <returns></returns>
        public Recipe Copy()
        {
            //生成
            Recipe recpi = new Recipe();
            recpi = (Recipe)this.MemberwiseClone();
            recpi.InspParam = new List<InspParameter>();
            recpi.LightParam = new List<LightParameter>();
            for (int i = 0; i < this.InspParam.Count; i++)
                recpi.InspParam.Add(new InspParameter());
            for (int i = 0; i < this.LightParam.Count; i++)
                recpi.LightParam.Add(new LightParameter());
            //コピーする
            Copy(recpi);

            return recpi;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="eModeID"></param>
        /// <returns></returns>
        public bool Load(string sName, AppData.ModeID? eModeID = null)
        {
            if (sName != "")
            {
                IniFileAccess ini = new IniFileAccess();
                string sec;

                if (eModeID == null)
                {
                    this.Path = SystemParam.GetInstance().RecipeFoldr + sName + "\\" + AppData.RCP_FILE;
                }
                else if (eModeID == AppData.ModeID.Old)
                {
                    this.Path = sName +"\\"+ AppData.RCP_FILE;
                }

                //名称
                this.KindName = LoadKindName(this.Path);

                sec = "recipe";
                //番号
                this.SelectItem = ini.GetIni(sec, GetNameClass.GetName(() => SelectItem), "000", Path);
                //表裏設定共通
                this.UpDownSideCommon = ini.GetIni(sec, GetNameClass.GetName(() => UpDownSideCommon), true, Path);
                //分割数
                this.Partition = ini.GetIni(sec, GetNameClass.GetName(() => Partition), 8, Path);
                //両端任意指定
                this.IsBothEndAny = ini.GetIni(sec, GetNameClass.GetName(() => IsBothEndAny), false, Path);
                //共通検査領域を使用する
                this.CommonInspAreaEnable = ini.GetIni(sec, GetNameClass.GetName(() => CommonInspAreaEnable), SystemParam.GetInstance().InspArea_DefaultMode, Path);

                //感度・幅・ゾーン
                this.InspParam.Clear();
                for(int i=0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
                {
                    InspParameter inp = new InspParameter();

                    sec = "insp." + (AppData.SideID)i;
                    inp.Width = ini.GetIni(sec, GetNameClass.GetName(() => inp.Width), inp.Width, Path);
                    inp.MaskWidth = ini.GetIni(sec, GetNameClass.GetName(() => inp.MaskWidth), inp.MaskWidth, Path);
                    inp.MaskShift = ini.GetIni(sec, GetNameClass.GetName(() => inp.MaskShift), inp.MaskShift, Path);
                    for (int j = 0; j < AppData.MAX_PARTITION; j++)
                    {
                        inp.Zone[j] = ini.GetIni(sec, j + "_" + GetNameClass.GetName(() => inp.Zone),(int)(inp.Width/AppData.MAX_PARTITION), Path);
                    }
                    for (int j = 0; j < Enum.GetNames(typeof(AppData.InspID)).Length; j++)
                    {
                        inp.Kando[j].inspID = (AppData.InspID)Enum.Parse(typeof(AppData.InspID), ini.GetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => inp.Kando[j].inspID), ((AppData.InspID)j).ToString(), Path));
                        inp.Kando[j].Threshold = ini.GetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => inp.Kando[j].Threshold), 128, Path);
                        inp.Kando[j].LengthH = ini.GetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => inp.Kando[j].LengthH), 0.0, Path);
                        inp.Kando[j].LengthV = ini.GetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => inp.Kando[j].LengthV), 0.0, Path);
                        inp.Kando[j].Area = ini.GetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => inp.Kando[j].Area), 0.0, Path);
                    }
                    this.InspParam.Add(inp);
                }

                //照明
                this.LightParam.Clear();
                LightControlManager ltCtrl = LightControlManager.getInstance();
                for (int i = 0; i < ltCtrl.LightCount; i++)
                {
                    LightParameter lp = new LightParameter();
                    sec = "LightControl";
                    lp.LightValue = ini.GetIni(sec, i + "_" + GetNameClass.GetName(() => lp.LightValue), 0, Path);
                    lp.LightEnable = ini.GetIni(sec, i + "_" + GetNameClass.GetName(() => lp.LightEnable), true, Path);
                    lp.LightEnable = true;
                    this.LightParam.Add(lp);
                }
                //表面検査有効        //20181202 moteki   V1053
                sec = "InspectionEnable";
                this.UpSideInspEnable = ini.GetIni(sec, GetNameClass.GetName(() => UpSideInspEnable), true, Path);
                //裏面検査有効        //20181202 moteki   V1053
                sec = "InspectionEnable";
                this.DownsideInspEnable = ini.GetIni(sec, GetNameClass.GetName(() => DownsideInspEnable), true, Path);
                //外部出力
                sec = "ExternalOutput";
                this.ExternalEnable = ini.GetIni(sec, GetNameClass.GetName(() => ExternalEnable), false, Path);
                this.ExternalDelayTime1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime1), 0, Path);
                this.ExternalDelayTime2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime2), 0, Path);
                this.ExternalDelayTime3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime3), 0, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                this.ExternalDelayTime4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime4), 0, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                this.ExternalResetTime1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime1), 1000, Path);
                this.ExternalResetTime2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime2), 1000, Path);
                this.ExternalResetTime3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime3), 1000, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                this.ExternalResetTime4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime4), 1000, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                this.ExternalShot1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot1), 0, Path);
                this.ExternalShot2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot2), 0, Path);
                this.ExternalShot3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot3), 0, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                this.ExternalShot4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot4), 0, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加

                //実速度
                sec = "CamRealSpeed";
                this.UseCommonCamRealSpeed = ini.GetIni(sec, GetNameClass.GetName(() => UseCommonCamRealSpeed), true, Path);
                this.CamRealSpeedValue = ini.GetIni(sec, GetNameClass.GetName(() => CamRealSpeedValue), 15.0, Path);
                this.CamRealSpeedValueUra = ini.GetIni(sec, GetNameClass.GetName(() => CamRealSpeedValueUra), this.CamRealSpeedValue, Path);

                //速度
                sec = "CamSpeed";
                this.CamSpeedEnable = ini.GetIni(sec, GetNameClass.GetName(() => CamSpeedEnable), false, Path);
                this.CamSpeedValue = ini.GetIni(sec, GetNameClass.GetName(() => CamSpeedValue), 20.0, Path);
                this.CamSpeedValueUra = ini.GetIni(sec, GetNameClass.GetName(() => CamSpeedValueUra), this.CamSpeedValue, Path);
                //SetCamSpeed();

                //露光
                sec = "CamExposure";
                this.CamExposureEnable = ini.GetIni(sec, GetNameClass.GetName(() => CamExposureEnable), false, Path);
                this.CamExposureValue = ini.GetIni(sec, GetNameClass.GetName(() => CamExposureValue), 200.0, Path);
                this.CamExposureValueUra = ini.GetIni(sec, GetNameClass.GetName(() => CamExposureValueUra), this.CamExposureValue, Path);
                //SetCamExposure();

                //ゲイン
                sec = "CamGain";
                this.CamGainOmote = ini.GetIni(sec, GetNameClass.GetName(() => CamGainOmote), 1.0, Path);
                this.CamGainUra = ini.GetIni(sec, GetNameClass.GetName(() => CamGainUra), 1.0, Path);

                //岐阜カスタム パトライト設定 v1326
                sec = "GCustom";
                this.PatLiteEnable = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteEnable), false, Path);
                this.PatLiteDelay = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteDelay), 0, Path);
                this.PatLiteOnTime = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteOnTime), 0, Path);

                return true;    
            }
            else
            {
                return false; 
            }
        }


        private string GetFileNameWOExtension(string s)
        {
            int iLastIndex = s.LastIndexOf('\\');
            int iPeriodIndex = s.LastIndexOf('.');
            if (iLastIndex != -1 && (iPeriodIndex == -1 || iPeriodIndex < iLastIndex))
            {
                return s.Substring(iLastIndex + 1);
            }
            else if (iLastIndex != -1 && iPeriodIndex > iLastIndex)
            {
                return s.Substring(iLastIndex + 1, iPeriodIndex - iLastIndex);
            }
            else if (iLastIndex == -1 && iPeriodIndex > 0)
            {
                return s.Substring(0, iPeriodIndex);
            }
            return "";
        }

        private void BackupRecipeFile()
        {
            string RECIPE_BACKUP_EXT = "rcphst";

            //バックアップ品種名
            string sDir = System.IO.Path.GetDirectoryName(this.Path);
            string sRepFile = System.IO.Path.GetFileNameWithoutExtension(AppData.RCP_FILE);
            string sBackupFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + sRepFile + "." + RECIPE_BACKUP_EXT;
            string sBackupPath = System.IO.Path.Combine(sDir, sBackupFile);
            //バックアップ実施（Copy）
            if (File.Exists(this.Path) == false)
                return;
            File.Copy(this.Path, sBackupPath);
            int rcpBackCnt = 0;
            //バックアップファイル数
            foreach (string sFile in Directory.GetFiles(sDir))
            {
                if (sFile.GetFileExtension().ToLower() == RECIPE_BACKUP_EXT)
                    rcpBackCnt++;
            }
            if (rcpBackCnt > SystemParam.GetInstance().RecipeBackupCount)
            {
                foreach (string sFile in Directory.GetFiles(sDir))
                {
                    if (sFile.GetFileExtension().ToLower() == RECIPE_BACKUP_EXT)
                    {
                        string dateStr = System.IO.Path.GetFileNameWithoutExtension(sFile).Left(14);
                        DateTime rcpBackupDate = DateTime.ParseExact(dateStr, "yyyyMMddHHmmss", null);
                        DateTime nowDate = DateTime.Now;
                        TimeSpan diffDays = nowDate - rcpBackupDate;
                        if (diffDays.Days > SystemParam.GetInstance().RecipeBackupDays)
                        {
                            File.Delete(sFile);
                            rcpBackCnt--;
                        }
                        if (rcpBackCnt <= SystemParam.GetInstance().RecipeBackupCount)
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Save
        /// </summary>
        public void Save(bool backFlag=true)
        {
            if (backFlag == true)
                BackupRecipeFile();

            IniFileAccess ini = new IniFileAccess();
            string sec;

            //名称
            SaveKindName();

            sec = "recipe";
            //番号
            ini.SetIni(sec, GetNameClass.GetName(() => SelectItem), SelectItem, Path);
            //表裏感度共通
            ini.SetIni(sec, GetNameClass.GetName(() => UpDownSideCommon), UpDownSideCommon, Path);
            //分割数
            ini.SetIni(sec, GetNameClass.GetName(() => Partition), Partition, Path);
            //両端任意指定
            ini.SetIni(sec, GetNameClass.GetName(() => IsBothEndAny), IsBothEndAny, Path);
            //共通検査領域を使用する
            ini.SetIni(sec, GetNameClass.GetName(() => CommonInspAreaEnable), CommonInspAreaEnable, Path);

            //感度・幅・ゾーン
            for (int i = 0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
            {
                sec = "insp." + (AppData.SideID)i;
                ini.SetIni(sec, GetNameClass.GetName(() => InspParam[i].Width), InspParam[i].Width, Path);
                ini.SetIni(sec, GetNameClass.GetName(() => InspParam[i].MaskWidth), InspParam[i].MaskWidth, Path);
                ini.SetIni(sec, GetNameClass.GetName(() => InspParam[i].MaskShift), InspParam[i].MaskShift, Path);
                for (int j = 0; j < AppData.MAX_PARTITION; j++)
                {
                    ini.SetIni(sec, j + "_" + GetNameClass.GetName(() => InspParam[i].Zone), InspParam[i].Zone[j], Path);
                }
                for (int j = 0; j < Enum.GetNames(typeof(AppData.InspID)).Length; j++)
                {
                    ini.SetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => InspParam[i].Kando[j].inspID), InspParam[i].Kando[j].inspID, Path);
                    ini.SetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => InspParam[i].Kando[j].Threshold), InspParam[i].Kando[j].Threshold, Path);
                    ini.SetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => InspParam[i].Kando[j].LengthH), InspParam[i].Kando[j].LengthH, Path);
                    ini.SetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => InspParam[i].Kando[j].LengthV), InspParam[i].Kando[j].LengthV, Path);
                    ini.SetIni(sec, (AppData.InspID)j + "_" + GetNameClass.GetName(() => InspParam[i].Kando[j].Area), InspParam[i].Kando[j].Area, Path);
                }
            }

            //照明
            LightControlManager ltCtrl = LightControlManager.getInstance();
            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                sec = "LightControl";
                ini.SetIni(sec, i + "_" + GetNameClass.GetName(() => LightParam[i].LightValue), LightParam[i].LightValue, Path);
                ini.SetIni(sec, i + "_" + GetNameClass.GetName(() => LightParam[i].LightEnable), LightParam[i].LightEnable, Path);
            }
            //表面検査有効        //20181202 moteki   V1053
            sec = "InspectionEnable";
            ini.SetIni(sec, GetNameClass.GetName(() => UpSideInspEnable), UpSideInspEnable, Path);
            //裏面検査有効        //20181202 moteki   V1053
            sec = "InspectionEnable";
            ini.SetIni(sec, GetNameClass.GetName(() => DownsideInspEnable), DownsideInspEnable, Path);
            //外部出力
            sec = "ExternalOutput";
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalEnable), ExternalEnable, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime1), ExternalDelayTime1, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime2), ExternalDelayTime2, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime3), ExternalDelayTime3, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime4), ExternalDelayTime4, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime1), ExternalResetTime1, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime2), ExternalResetTime2, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime3), ExternalResetTime3, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime4), ExternalResetTime4, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot1), ExternalShot1, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot2), ExternalShot2, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot3), ExternalShot3, Path); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot4), ExternalShot4, Path); //V1057 NG表裏修正 yuasa 20190118：外部４追加

            //実速度
            sec = "CamRealSpeed";
            ini.SetIni(sec, GetNameClass.GetName(() => UseCommonCamRealSpeed), UseCommonCamRealSpeed, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamRealSpeedValue), CamRealSpeedValue, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamRealSpeedValueUra), CamRealSpeedValueUra, Path);

            //速度
            sec = "CamSpeed";
            ini.SetIni(sec, GetNameClass.GetName(() => CamSpeedEnable), CamSpeedEnable, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamSpeedValue), CamSpeedValue, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamSpeedValueUra), CamSpeedValueUra, Path);
            //SetCamSpeed();

            //露光
            sec = "CamExposure";
            ini.SetIni(sec, GetNameClass.GetName(() => CamExposureEnable), CamExposureEnable, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamExposureValue), CamExposureValue, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamExposureValueUra), CamExposureValueUra, Path);
            //SetCamExposure();

            //ゲイン
            sec = "CamGain";
            ini.SetIni(sec, GetNameClass.GetName(() => CamGainOmote), CamGainOmote, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => CamGainUra), CamGainUra, Path);

            //岐阜カスタム パトライト設定 v1326
            sec = "GCustom";
            ini.SetIni(sec, GetNameClass.GetName(() => PatLiteEnable), PatLiteEnable, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => PatLiteDelay), PatLiteDelay, Path);
            ini.SetIni(sec, GetNameClass.GetName(() => PatLiteOnTime), PatLiteOnTime, Path);

            ini.Flush(this.Path);
        }

        public void SetCamSpeed()
        {
            double speed, speedUra;
            if (this.CamSpeedEnable == true)
            {
                speed = this.CamSpeedValue;
                speedUra = this.CamSpeedValueUra;
            }
            else
            {
                speed = SystemParam.GetInstance().CamSpeed;
                speedUra = SystemParam.GetInstance().CamSpeedUra;
            }

            for (int i = 0; i < HalconCamera.APCameraManager.getInstance().CameraNum; i++)
            {
                AppData.SideID side;
                if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
                {
                    double bufHz = (side == AppData.SideID.表) ? (double)speed : (double)speedUra;
                    double hz = SystemParam.GetInstance().Speed2Hz(bufHz);

                    HalconCamera.HalconCameraBase cam = HalconCamera.APCameraManager.getInstance().GetCamera(i);
                    cam.SetLineRate(hz);
                }
            }
        }
        public void SetCamExposure()
        {
            int exposure, exposureUra;
            if (this.CamExposureEnable == true)
            {
                exposure = (int)this.CamExposureValue;
                exposureUra = (int)this.CamExposureValueUra;
            }
            else
            {
                exposure = SystemParam.GetInstance().CamExposure;
                exposureUra = SystemParam.GetInstance().CamExposureUra;
            }

            for (int i = 0; i < HalconCamera.APCameraManager.getInstance().CameraNum; i++)
            {
                AppData.SideID side;
                if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
                {
                    int exp = (side == AppData.SideID.表) ? (int)exposure : (int)exposureUra;

                    HalconCamera.HalconCameraBase camBase = HalconCamera.APCameraManager.getInstance().GetCamera(i);
                    camBase.SetExposureTime(exp);
                }
            }
        }
        public void SetCamGain()
        {
            for (int i = 0; i < HalconCamera.APCameraManager.getInstance().CameraNum; i++)
            {
                AppData.SideID side;
                if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
                {
                    double gain = (side == AppData.SideID.表) ? this.CamGainOmote : this.CamGainUra;

                    HalconCamera.HalconCameraBase camBase = HalconCamera.APCameraManager.getInstance().GetCamera(i);
                    camBase.SetGain(gain);
                }
            }
        }

        string _kindNameSec = "RecipeName";
        /// <summary>
        /// 
        /// </summary>
        public void SaveKindName()
        {
            IniFileAccess ini = new IniFileAccess();
            ini.SetIni(_kindNameSec, GetNameClass.GetName(() => KindName), KindName, Path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string LoadKindName(string sPath)
        {
            IniFileAccess ini = new IniFileAccess();
            return ini.GetIni(_kindNameSec, GetNameClass.GetName(() => KindName), "未登録", sPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="subFolders"></param>
        public static void GetSubfolders( string folderName, ref ArrayList subFolders)
        {
            //folderNameにあるサブフォルダを取得
            foreach (string folder in
                System.IO.Directory.GetDirectories(folderName))
            {
                //取得したサブフォルダに"\Recipe.rcp"を足す。
                string stPath = folder + @"\" + AppData.RCP_FILE;

                //リストに追加
                subFolders.Add(stPath);
                //再帰的にサブフォルダを取得する
                GetSubfolders(folder, ref subFolders);
            }
        }
    }
    
}
