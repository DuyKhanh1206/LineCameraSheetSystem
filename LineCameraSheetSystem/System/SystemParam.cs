using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

using Fujita.InspectionSystem;
using Fujita.Misc;
using Adjustment;
using HalconDotNet;
using HalconCamera;

namespace LineCameraSheetSystem
{
    public class SystemParam                // thông số hệ thống
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        private static SystemParam _singleton = new SystemParam();
        public static SystemParam GetInstance()
        {
            return _singleton;
        }

        const string DEFAULT_DEVELOPER_PASSWORD_HASH = "C64C4DF7676E33A98806D8069A7D3";			//921663

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private SystemParam()
        {
            ExposureDefault = new int[AppData.CAM_COUNT, 2];
        }

        #region カメラ設定
        //カメラ設定List
        public List<CameraParam> camParam = new List<CameraParam>();
        //カメラの基準露光値
        public int[,] ExposureDefault { get; set; }
        #endregion

        /// <summary>
        /// 裏検査有効
        /// </summary>
        public bool DownSideEnable { get; set; }

        #region システム
        /// <summary>
        /// 画面センター固定モード
        /// </summary>
        public bool ForceCenteringMode { get; set; }
        #endregion

        #region 表示有効
        /// <summary>
        /// 中断ボタン表示有効
        /// </summary>
        public bool SuspendButtonEnable { get; set; }
        /// <summary>
        /// ロットNo.表示有効
        /// </summary>
        public bool LotNoEnable { get; set; }
        #endregion

        #region 照明警告時間
        /// <summary>
        /// 透過照明警告時間
        /// </summary>
        public int LightWarningTime { get; set; }
        #endregion

        #region 保存データ削除
        /// <summary>
        /// 保存データ削除の月
        /// </summary>
        public int DeleteMonth { get; set; }
        #endregion

        #region 両端マスク
        /// <summary>
        /// 両端マスク実施
        /// </summary>
        public bool SideMaskEnable { get; set; }
        /// <summary>
        /// 両端マスク時のワーク検出しきい値
        /// </summary>
        public int SideMaskThreshold { get; set; }
        /// <summary>
        /// 両端マスクの膨張値
        /// </summary>
        public int SideMaskDilation { get; set; }
        #endregion

        #region ソフトシェーディング
        /// <summary>
        /// ソフトシェーディング実施
        /// </summary>
        public bool SoftShadingEnable { get; set; }
        /// <summary>
        /// SoftShadingのターゲットGlay値
        /// </summary>
        public int SoftShadingTargetGrayLevel { get; set; }
        /// <summary>
        /// SoftShading実施可能範囲（[value - TargetGray値] ～ [TargetGray値 + value]）
        /// </summary>
        public int SoftShadingLimit { get; set; }
        /// <summary>
        /// 係数を算出する画像枚数
        /// </summary>
        public int SoftShadingCalcImageCount { get; set; }
        #endregion

        #region 明暗検査実施
        //明るい方向検査有効
        public bool InspBrightEnable { get; set; }
        //暗い方向の検査有効
        public bool InspDarkEnable { get; set; }
        #endregion

        #region カラーカメラ時に検査を実施する画像
        public List<bool> ColorCamInspImage { get; set; }
        #endregion

        #region 除外（白・黒）
        public bool OutofWhiteEnabled { get; set; }
        public bool OutofBlackEnabled { get; set; }
        public int OutofWhiteLimit { get; set; }
        public int OutofBlackLimit { get; set; }
        #endregion

        #region カメラ設定
        //カメラ露光値
        public int CamExposure { get; set; }
        //カメラ取り込み速度(m/分)
        public double CamSpeed { get; set; }
        public int CamExposureUra { get; set; }
        public double CamSpeedUra { get; set; }
        /// <summary>
        /// 検査開始時にカメラを再オープンする
        /// </summary>
        public bool CamCloseOpenEnable { get; set; }
        /// <summary>
        /// 自動調光時にカメラを再オープンする
        /// </summary>
        public bool CamCloseOpenAutoLightingEnable { get; set; }
        #endregion

        #region イメージクロップ
        //イメージの切り抜きサイズの指定
        public int CropWidth { get; private set; }
        public int CropHeight { get; private set; }
        public int ScaleWidth { get; private set; }
        public int ScaleHeight { get; private set; }
        #endregion

        #region システムパス                  // đường dẫn hệ thống
        //システムiniファイルのパス
        public const string SysPath = AppData.EXE_FOLDER + AppData.SYSTEM_FILE;
        public string CommunicatioPath { get; private set; }
        public string CameraPath { get; private set; }
        public string LightCtrlPath { get; private set; }
        public string ImageFolder { get; private set; }
        public string ProductFolder { get; private set; }
        public string RecipeFoldr { get; private set; }
        public string ProductTimeFolder { get; private set; }
        public string BackupFolder { get; private set; }
        public string ImageSaveFolder { get; private set; }
        public const string LogFolder = AppData.LOG_FOLDER;
        #endregion

        #region 自動シャットダウン    // tự động tắt máy
        /// <summary>
        /// 自動シャットダウン実施
        /// </summary>
        public bool AutoShutdownEnable { get; set; }
        /// <summary>
        /// 自動シャットダウン待ち時間(秒)
        /// </summary>
        public int AutoShutdownWaitSec { get; set; }
        #endregion

        #region 小数点桁数
        /// <summary>
        /// 測長の少数桁
        /// </summary>
        public string LengthDecimal { get; private set; }
        /// <summary>
        /// アドレスの少数桁
        /// </summary>
        public string AddressDecimal { get; private set; }
        /// <summary>
        /// 欠点サイズの少数桁
        /// </summary>
        public string NgDataDecimal { get; private set; }
        /// <summary>
        /// シート幅の少数桁
        /// </summary>
        public string SWNgDataDecimal { get; private set; }
        /// <summary>
        /// シートズレの少数桁
        /// </summary>
        public string SMNgDataDecimal { get; private set; }
        /// <summary>
        /// シート厚みの少数桁
        /// </summary>
        public string STNgDataDecimal { get; private set; }
        /// <summary>
        /// 速度の少数桁
        /// </summary>
        public string SpeedMainDecimal { get; private set; }
        #endregion

        #region 欠点種別の色
        /// <summary>
        /// 欠点種色
        /// </summary>
        public List<MarkColor> markColorUpSide = new List<MarkColor>();
        public List<MarkColor> markColorDownSide = new List<MarkColor>();
        #endregion

        #region 測長監視(測長がUPしているか)
        /// <summary>
        /// 測長監視の実施
        /// </summary>
        public bool EnableLengthMeasMonitor { get; private set; }
        /// <summary>
        /// 測長監視の間隔時間
        /// </summary>
        public int LengthMeasMonitorLimitSec { get; private set; }
        #endregion

        #region IOポート番号
        /// <summary>
        /// Red IO番号
        /// </summary>
        public int OutPointRed { get; set; }
        /// <summary>
        /// Green IO番号
        /// </summary>
        public int OutPointGreen { get; set; }
        /// <summary>
        /// Yellow IO番号
        /// </summary>
        public int OutPointYellow { get; set; }
        /// <summary>
        /// Blue IO番号
        /// </summary>
        public int OutPointBlue { get; set; }
        /// <summary>
        /// Buzzer IO番号（中央）
        /// </summary>
        public int OutPointBuzzer { get; set; }
        /// <summary>
        /// 外部出力1 IO番号
        /// </summary>
        public int OutPointExternal1 { get; set; }
        /// <summary>
        /// 外部出力2 IO番号
        /// </summary>
        public int OutPointExternal2 { get; set; }
        /// <summary>
        /// 外部出力3 IO番号
        /// </summary>
        public int OutPointExternal3 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部３追加
        /// <summary>
        /// 外部出力4 IO番号
        /// </summary>
        public int OutPointExternal4 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部４追加
        /// <summary>
        /// 岐阜カスタムBuzzer IO番号
        /// </summary>
        public int OutPointGCustomBuzzer { get; set; }
        /// <summary>
        /// Ngリセット入力
        /// </summary>
        public int InNgReset { get; set; }  //V1293 moteki
        /// <summary>
        /// エラーリセット入力
        /// </summary>
        public int InErorrReset { get; set; }   //V1293 moteki
        /// <summary>
        /// 検査開始入力
        /// </summary>
        public int InInspectionStart { get; set; }  //V1293 moteki
        /// <summary>
        /// Buzzer IO番号
        /// </summary>
        public int OutPointBuzzerBothSide { get; set; } //V1333
        /// <summary>
        /// PC電源ボタン接続用リレー
        /// </summary>
        public int PowerOffRelay { get; set; } //v1338

        #endregion

        #region 外部出力
        //外部出力1の有無
        public bool ExternalFrontReverseDivide { get; set; } //V1057 NG表裏修正 yuasa 20190118：表裏分割有無
        //外部出力1の有無
        public bool ExternalEnable1 { get; set; }
        //外部出力2の有無
        public bool ExternalEnable2 { get; set; }
        //外部出力3の有無
        public bool ExternalEnable3 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部３追加
        //外部出力4の有無
        public bool ExternalEnable4 { get; set; } //V1057 NG表裏修正 yuasa 20190118：外部４追加
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
        public List<bool> Extarnal1Zone { get; set; }
        public List<bool> Extarnal2Zone { get; set; }
        public List<bool> Extarnal3Zone { get; set; }
        public List<bool> Extarnal4Zone { get; set; }

        //ブザー音時間
        //public int BuzzerTimer { get; set; }//V1333 削除
        public int PatLiteResetTimeBothSide { get; set; }//V1333
        public int PatLiteResetTimeCenter { get; set; }//V1333
        public int PatLiteDelayTimeBothSide { get; set; }//V1333
        public int PatLiteDelayTimeCenter { get; set; }//V1333
        public int PatLiteResetTimeBothSideUra { get; set; }//V1333
        public int PatLiteResetTimeCenterUra { get; set; }//V1333
        public int PatLiteDelayTimeBothSideUra { get; set; }//V1333
        public int PatLiteDelayTimeCenterUra { get; set; }//V1333

        /// <summary>ゾーン１～８の出力設定</summary>
        public bool SystemSettingFormZoneDisp { get; set; }//V1333 名称変更：ZoneSettingEnable→SystemSettingFormZoneDisp
        #endregion

        #region 欠点画像表示画面の波形OnOff
        /// <summary>
        /// [OneNGImage]波形OnOff
        /// </summary>
        public bool OneNGImageGraphEnable { get; set; }
        /// <summary>
        /// [OneNGImage]取込画像表示
        /// </summary>
        public bool OneNGImageBaseImageEnable { get; set; }
        #endregion

        #region 時刻補正
        /// <summary>
        /// 時刻補正を促すﾒｯｾｰｼﾞを表示する時刻(何時)
        /// </summary>
        public int TimeWarningPopupHour { get; set; }
        #endregion

        #region ポップアップ画面背景色
        /// <summary>
        /// ポップアップ画面背景色（欠点ＮＧ）
        /// </summary>
        public string PopupColorNG { get; set; }
        #endregion

        #region 速度表示
        public bool SpeedControlDispEnable { get; set; }
        #endregion

        #region 照明値自動検出ボタン
        public bool LightAjustEnable { get; set; }
        #endregion

        #region 外部出力１のキャンセル機能
        //外部出力１信号のキャンセル確認機能
        public bool ExtOut1CancelEnable { get; set; }
        #endregion

        #region 入力監視
        public bool Input1MonitorEnable { get; set; }
        public int Input1MonitorDInNumber { get; set; }
        public string Input1MonitorMessage1 { get; set; }
        #endregion

        #region 外部リセットボタン
        public bool OutsideResetButtonEnable { get; set; }
        #endregion


        //public List<MarkColor> markColorUpSide = new List<MarkColor>();
        public List<int> MainteLightOffset = new List<int>(); //V1058 メンテナンス追加 yuasa 20190125：追加

        #region 外部検査開始終了ボタン
        public bool OutsideManualExtButtonEnable { get; set; } //V1057 手動外部修正 yuasa 20190122：手動検査開始終了にini追加
        #endregion

        #region NG発生時に表示する画面モード
        /// <summary>
        /// NG発生時に表示する画面モード(false:文字列だけ表示する画面　true:NG画像付き画面)
        /// </summary>
        public bool NGPopupWindowMode { get; set; }
        /// <summary>
        /// 自動検査時のNG履歴数
        /// </summary>
        public int NGHistoryCount { get; set; }
        #endregion

        #region メイン(画像)表示画面パラメータ
        /// <summary>
        /// メイン画面を表示するボタンのEnable/Disable
        /// </summary>
        public bool IM_Enabled { get; set; }
        /// <summary>
        /// 縦並びで表示（true:縦　false:横
        /// </summary>
        public bool IM_VerDisplayMode { get; set; }
        public bool IM_DispInspHeight { get; set; }
        public bool IM_DispInspWidth { get; set; }
        public bool IM_DispMaskWidth { get; set; }
        public bool IM_DispGraphLight { get; set; }
        public bool IM_DispGraphDark { get; set; }
        public bool IM_DispGraphAvg { get; set; }
        public bool IM_DispKandoLine { get; set; }
        public int IM_ImageBufferCount { get; set; }
        public bool IM_AutoSaveEnable { get; set; }
        public int IM_AutoSaveCount { get; set; }
        /// <summary>
        /// NG発生時の元画像保存枚数
        /// </summary>
        public int IM_AutoSaveOneNGsaveCount { get; set; }
        /// <summary>
        /// NgCrop保存数(ColorShading,ColorOrg,Gray,Red,Green,Blue)
        /// </summary>
        public int IM_NgCropSaveCount { get; set; }
        public int IM_DispGraphWidth1ch { get; set; }
        public int IM_DispGraphWidth3ch { get; set; }
        public bool IM_GraphCalcAreaAll { get; set; }
        /// <summary>
        /// 取込画像の連結数（true:連結画像枚数と同じ　false:下から検査するところまで）
        /// </summary>
        public bool IM_OrgImageConnectMode { get; set; }
        #endregion

        #region Password
        /// <summary>開発者パスワードのハッシュコード</summary>
        public string DeveloperPasswordHash { get; set; }
        #endregion

        #region 検査領域(幅...)[共通情報]
        /// <summary>
        /// レシピDefaultモード(true:個別　false:共通)
        /// </summary>
        public bool InspArea_DefaultMode { get; set; }
        /// <summary>
        /// 共通：検査幅
        /// </summary>
        public double[] InspArea_CmnSheetWidth { get; set; }
        /// <summary>
        /// 共通：監視幅
        /// </summary>
        public double[] InspArea_CmnMaskWidth { get; set; }
        /// <summary>
        /// 共通：幅シフト
        /// </summary>
        public double[] InspArea_CmnMaskShift { get; set; }
        /// <summary>
        /// イメージ連結の場合、下から何番目で検査するか？（2～）
        /// </summary>
        public int InspArea_ConnectMode_ImagePoint { get; set; }
        /// <summary>
        /// イメージ連結の場合、１枚画像＋上下の検査領域
        /// </summary>
        public int InspArea_ConnectMode_BufferArea { get; set; }
        #endregion

        #region 検査パラメータ設定
        /// <summary>
        /// １カメラ有効NG数
        /// </summary>
        public int InspFunc_CountNgMax { get; set; }
        /// <summary>
        /// 結合
        /// </summary>
        public double InspFunc_BlobClosingCircle { get; set; }
        /// <summary>
        /// 除去
        /// </summary>
        public double InspFunc_BlobOpeningCircle { get; set; }
        /// <summary>
        /// ブロブ抽出有効Pix数
        /// </summary>
        public int InspFunc_BlobSelectArea { get; set; }
        #endregion

        #region レシピ
        /// <summary>品種バックアップ日数(30なら過去30日まで保持)</summary>
        public int RecipeBackupDays { get; set; }
        /// <summary>品種バックアップ最小数(10なら日数に関係なく10保持)</summary>
        public int RecipeBackupCount { get; set; }
        public double DefaultInspWidth { get; set; }
        public double DefaultMaskWidth { get; set; }
        public double DefaultMaskShift { get; set; }
        /// <summary>レシピのゾーン設定可否</summary>
        public bool RecipeZoneSetteingEnable { get; set; }//V1333
        #endregion

        #region 自動調光
        /// <summary>
        /// 自動調光時のチェックイメージ数
        /// </summary>
        public int AutoLightCheckImageCount { get; set; }
        /// <summary>
        /// 閾値（１２８）を超えたと判断するための枚数
        /// </summary>
        public int AutoLightOkImageCount { get; set; }
        /// <summary>
        /// 自動調光時で、輝度不足のときにUpするGain刻み値
        /// </summary>
        public double AutoLightGainUpStep { get; set; }
        /// <summary>
        /// 自動調光時で、GainUpする回数
        /// </summary>
        public int AutoLightGainMaxCount { get; set; }
        /// <summary>
        /// JustOKとする基準128　ー範囲値
        /// </summary>
        public int AutoLightOkLowLimit { get; set; }
        /// <summary>
        /// JustOKとする基準128　+範囲値
        /// </summary>
        public int AutoLightOkHighLimit { get; set; }
        /// <summary>
        /// 自動調光の詳細サーチ時のUP輝度値
        /// </summary>
        public int AutoLightDetailUpLevel { get; set; }
        #endregion

        #region 表示線の色・太さ
        /// <summary>
        /// 感度グラフ（Default:Magenta）
        /// </summary>
        public string LineKandoGrapthColor { get; set; }
        /// <summary>
        /// 基準感度位置[感度0位置]の色（Default:Yellow）
        /// </summary>
        public string LineBaseKandoColor { get; set; }
        /// <summary>
        /// 検査領域[縦方向]の色（Default:Red）
        /// </summary>
        public string LineInspHeightColor { get; set; }
        /// <summary>
        /// 検査幅の色（Default:Green）
        /// </summary>
        public string LineInspWidthColor { get; set; }
        /// <summary>
        /// 状態監視の色（Default:Red）
        /// </summary>
        public string LineStateMonitorColor { get; set; }

        /// <summary>
        /// 感度グラフの線の太さ
        /// </summary>
        public int LineKandoGrapthThick { get; set; }
        /// <summary>
        /// 基準感度位置[感度0位置]の線の太さ
        /// </summary>
        public int LineBaseKandoThick { get; set; }
        /// <summary>
        /// 検査領域[縦方向]の線の太さ
        /// </summary>
        public int LineInspHeightThick { get; set; }
        /// <summary>
        /// 検査幅の線の太さ
        /// </summary>
        public int LineInspWidthThick { get; set; }
        /// <summary>
        /// 状態監視の線の太さ
        /// </summary>
        public int LineStateMonitorThick { get; set; }
        /// <summary>
        /// 設定感度
        /// </summary>
        public int[] LineKandoThick { get; set; }
        #endregion

        #region 連続NG発生時制御
        public bool ContinuNGEnable { get; set; }
        /// <summary>
        /// 連続NGと判断する枚数
        /// </summary>
        public int ContinuNGJudgeCount { get; set; }
        /// <summary>
        /// 連続NG後、NGキャンセルする枚数
        /// </summary>
        public int ContinuNGAfterCancelCount { get; set; }
        #endregion

        #region 出力先ドライブ
        /// <summary>
        /// 出力先ドライブ
        /// </summary>
        public string OutDrive { get; set; }
        #endregion

        #region Serial速度制御
        public bool SerialEnable1 { get; set; }
        public int SerialCycleWaitTime1 { get; set; }
        /// <summary>ポート番号(COM1,COM2)</summary>
        public string SerialComPort1 { get; set; }
        /// <summary>ボーレート(2400 , 4800 , 9600 , 14400 , 19200 , 38400)</summary>
        public int SerialBaudRate1 { get; set; }
        /// <summary>パリティビット(None , Even , Odd , Mark , Space)</summary>
        public string SerialParity1 { get; set; }
        /// <summary>データビット(7 , 8)</summary>
        public int SerialDataBits1 { get; set; }
        /// <summary>ストップビット(None , One , OnePointFive , Two)</summary>
        public string SerialStopBits1 { get; set; }
        /// <summary>フロー制御(None , RequestToSend , RequestToSendXOnXOff , XOnXOff)</summary>
        public string SerialHandshake1 { get; set; }
        /// <summary>エンコード(us-ascii)</summary>
        public string SerialEnCoding1 { get; set; }
        /// <summary>リターンコード(CR , LF , CRLF)</summary>
        public string SerialRtnCode1 { get; set; }

        public bool SerialEnable2 { get; set; }
        public int SerialCycleWaitTime2 { get; set; }
        /// <summary>ポート番号(COM1,COM2)</summary>
        public string SerialComPort2 { get; set; }
        /// <summary>ボーレート(2400 , 4800 , 9600 , 14400 , 19200 , 38400)</summary>
        public int SerialBaudRate2 { get; set; }
        /// <summary>パリティビット(None , Even , Odd , Mark , Space)</summary>
        public string SerialParity2 { get; set; }
        /// <summary>データビット(7 , 8)</summary>
        public int SerialDataBits2 { get; set; }
        /// <summary>ストップビット(None , One , OnePointFive , Two)</summary>
        public string SerialStopBits2 { get; set; }
        /// <summary>フロー制御(None , RequestToSend , RequestToSendXOnXOff , XOnXOff)</summary>
        public string SerialHandshake2 { get; set; }
        /// <summary>エンコード(us-ascii)</summary>
        public string SerialEnCoding2 { get; set; }
        /// <summary>リターンコード(CR , LF , CRLF)</summary>
        public string SerialRtnCode2 { get; set; }
        #endregion

        #region NetConnectMonitor
        /// <summary>
        /// Net接続Monitor
        /// </summary>
        /// <summary>接続タイムアウト時間[秒](0:モニタしないとき 0!=モニタするとき)</summary>
        public int NetConnectMonitor_ConnectTimeOut { get; set; }
        /// <summary>Net接続モニタする(true:する false:しない)</summary>
        public bool NetConnectMonitor_Enable { get; set; }
        #endregion

        #region 共通
        /// <summary>
        /// 品種項目である実速度の共通値
        /// </summary>
        /// <summary>品種項目である実速度の共通値</summary>
        public double Common_RecipeRealSpeedOmote { get; set; }
        public double Common_RecipeRealSpeedUra { get; set; }
        #endregion

        #region 照明連続点灯
        /// <summary>照明連続点灯</summary>
        public bool LightRepeatON { get; set; } //v1325 LightRepeatON追加
        #endregion

        #region 岐阜カスタム v1326
        /// <summary>岐阜カスタム有効</summary>
        public bool GCustomEnable { get; set; } 
        /// <summary>１Ｆパトライトディレイ時間（秒）</summary>
        public int CommonPatLiteDelaySecond { get; set; }
        /// <summary>１Ｆパトライトオン時間（秒）</summary>
        public int CommonPatLiteOnTimeSecond { get; set; }
        /// <summary>フリッパ監視CH</summary>
        public int InFlipperoOserveCH { get; set; }
        /// <summary>パトライト停止スイッチCH</summary>
        public int InPatLiteStopSWCH { get; set; }
        /// <summary>岐阜カスタムDIOサイクルタイム</summary>
        public int GCustomDIOCycleTime { get; set; }
        #endregion

        #region PC電源ボタン押下対応 v1338 yuasa
        /// <summary>PC電源ボタン押下機能有効</summary>
        public bool PowerOffButtonEnable { get; set; }
        /// <summary>DINチャンネル番号</summary>
        public int PowerOffButtonDioIndex { get; set; }
        /// <summary>DINのHigh/Low</summary>
        public bool PowerOffButtonHighLow { get; set; }
        /// <summary>ボタン長押し時間</summary>
        public int PowerOffButtonOnTime { get; set; }
        #endregion


        //システムパラメーターのロード                        Đọc thông số hệ thống
        public void SystemLoad()
        {
            //Listのクリア
            camParam.Clear();               // list danh sách CAM
            markColorUpSide.Clear();
            markColorDownSide.Clear();

            IniFileAccess ini = new IniFileAccess();
            string sec;

            DownSideEnable = false;
            //cam
            for (int i = 0; AppData.CAM_COUNT > i; i++)    // khai báo khởi đông CAM
            {
                CameraParam camp = new CameraParam();       // khai báo sử dụng lớp thông số CAM
                this.camParam.Add(camp);

                sec = "cam" + i.ToString();
                //CamID
                camParam[i].CamID = (AppData.CamID)Enum.Parse(typeof(AppData.CamID), ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].CamID), Enum.GetNames(typeof(AppData.CamID))[i].ToString(), SysPath));
                //CamNo
                camParam[i].CamNo = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].CamNo), i, SysPath);
                //OnOff
                camParam[i].OnOff = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].OnOff), true, SysPath);
                //
                camParam[i].PixV = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].PixV), 1000, SysPath);
                camParam[i].PixH = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].PixH), 2048, SysPath);
                camParam[i].ResoV = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].ResoV), 0.1, SysPath);
                camParam[i].ResoH = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].ResoH), 0.1, SysPath);
                camParam[i].ShiftV = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].ShiftV), 0.0, SysPath);
                camParam[i].ShiftH = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].ShiftH), 0.0, SysPath);
                camParam[i].CamParts = (AppData.SideID)Enum.Parse(typeof(AppData.SideID), ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].CamParts), AppData.SideID.表.ToString(), SysPath));
                //破棄する枚数
                camParam[i].DiscardCount = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].DiscardCount), 2, SysPath);
                //UseLightNo
                camParam[i].UseLightNo = ini.GetIni(sec, GetNameClass.GetName(() => camParam[i].UseLightNo), "-1", SysPath).Split(new char[] { ',' }).Select<string, int>(x => int.Parse(x)).ToList();


                //基準露光値
                ExposureDefault[i, 0] = ini.GetIni(sec, GetNameClass.GetName(()=> ExposureDefault) + "1", 0, SysPath);
                ExposureDefault[i, 1] = ini.GetIni(sec, GetNameClass.GetName(() => ExposureDefault) + "2", 0, SysPath);

                //裏カメラあり？
                if (camParam[i].OnOff == true && camParam[i].CamParts == AppData.SideID.裏)
                    DownSideEnable = true;
            }

            //System
            sec = "System";
            ForceCenteringMode = ini.GetIni(sec, GetNameClass.GetName(() => ForceCenteringMode), true, SysPath);

            //LotNo
            sec = "DisplayEnable";
            SuspendButtonEnable = ini.GetIni(sec, GetNameClass.GetName(() => SuspendButtonEnable), false, SysPath);
            LotNoEnable = ini.GetIni(sec, GetNameClass.GetName(() => LotNoEnable), false, SysPath);

            //LightTime
            sec = "LightTime";
            LightWarningTime = ini.GetIni(sec, GetNameClass.GetName(() => LightWarningTime), 0, SysPath);

            //DeleteMonth
            sec = "DeleteMonth";
            DeleteMonth = ini.GetIni(sec, GetNameClass.GetName(() => DeleteMonth), 3, SysPath);

            //SideMask
            sec = "SideMask";
            SideMaskEnable = ini.GetIni(sec, GetNameClass.GetName(() => SideMaskEnable), true, SysPath);
            SideMaskThreshold = ini.GetIni(sec, GetNameClass.GetName(() => SideMaskThreshold), 100, SysPath);
            SideMaskDilation = ini.GetIni(sec, GetNameClass.GetName(() => SideMaskDilation), 10, SysPath);

            //SoftShading
            sec = "SoftShading";
            SoftShadingEnable = ini.GetIni(sec, GetNameClass.GetName(() => SoftShadingEnable), true, SysPath);
            SoftShadingTargetGrayLevel = ini.GetIni(sec, GetNameClass.GetName(() => SoftShadingTargetGrayLevel), 128, SysPath);
            SoftShadingLimit = ini.GetIni(sec, GetNameClass.GetName(() => SoftShadingLimit), 50, SysPath);
            SoftShadingCalcImageCount= ini.GetIni(sec, GetNameClass.GetName(() => SoftShadingCalcImageCount), 10, SysPath);

            //BrightDarkInspection
            sec = "BrightDarkInspection";
            InspBrightEnable = ini.GetIni(sec, GetNameClass.GetName(() => InspBrightEnable), true, SysPath);
            InspDarkEnable = ini.GetIni(sec, GetNameClass.GetName(() => InspDarkEnable), true, SysPath);

            //ColorCamera
            sec = "ColorCamera";
            ColorCamInspImage = ini.GetIni("ColorCamInspImage", GetNameClass.GetName(() => ColorCamInspImage), "true,true,true,true", SysPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();

            //OutofWhiteBlack
            sec = "OutofWhiteBlack";
            OutofWhiteEnabled = ini.GetIni(sec, GetNameClass.GetName(() => OutofWhiteEnabled), false, SysPath);
            OutofBlackEnabled = ini.GetIni(sec, GetNameClass.GetName(() => OutofBlackEnabled), false, SysPath);
            OutofWhiteLimit = ini.GetIni(sec, GetNameClass.GetName(() => OutofWhiteLimit), 230, SysPath);
            OutofBlackLimit = ini.GetIni(sec, GetNameClass.GetName(() => OutofBlackLimit), 25, SysPath);

            //CameraSetting
            sec = "CameraSetting";
            CamExposure = ini.GetIni(sec, GetNameClass.GetName(() => CamExposure), 100, SysPath);
            CamSpeed = ini.GetIni(sec, GetNameClass.GetName(() => CamSpeed), 50.0, SysPath);
            CamExposureUra = ini.GetIni(sec, GetNameClass.GetName(() => CamExposureUra), CamExposure, SysPath);
            CamSpeedUra = ini.GetIni(sec, GetNameClass.GetName(() => CamSpeedUra), CamSpeed, SysPath);
            CamCloseOpenEnable = ini.GetIni(sec, GetNameClass.GetName(() => CamCloseOpenEnable), true, SysPath);
            CamCloseOpenAutoLightingEnable = ini.GetIni(sec, GetNameClass.GetName(() => CamCloseOpenAutoLightingEnable), true, SysPath);

            //ImageCliping
            sec = "ImageCliping";
            CropWidth = ini.GetIni(sec, GetNameClass.GetName(() => CropWidth), 300, SysPath);
            CropHeight = ini.GetIni(sec, GetNameClass.GetName(() => CropHeight), 1200, SysPath);
            ScaleWidth = ini.GetIni(sec, GetNameClass.GetName(() => ScaleWidth), 300, SysPath);
            ScaleHeight = ini.GetIni(sec, GetNameClass.GetName(() => ScaleHeight), 300, SysPath);

            //Path
            sec = "Path";
            CommunicatioPath = ini.GetIni(sec, GetNameClass.GetName(() => CommunicatioPath), AppData.EXE_FOLDER, SysPath);
            CameraPath = ini.GetIni(sec, GetNameClass.GetName(() => CameraPath), AppData.EXE_FOLDER, SysPath);
            LightCtrlPath = ini.GetIni(sec, GetNameClass.GetName(() => LightCtrlPath), AppData.EXE_FOLDER, SysPath);
            ImageFolder = ini.GetIni(sec, GetNameClass.GetName(() => ImageFolder), AppData.IMAGE_FOLDER, SysPath);
            ProductTimeFolder = ini.GetIni(sec, GetNameClass.GetName(() => ProductTimeFolder), AppData.PURODUCT_TIME_FOLDER, SysPath);
            ProductFolder = ini.GetIni(sec, GetNameClass.GetName(() => ProductFolder), AppData.PURODUCT_FOLDER, SysPath);
            RecipeFoldr = ini.GetIni(sec, GetNameClass.GetName(() => RecipeFoldr), AppData.RCP_FOLDER, SysPath);
            BackupFolder = ini.GetIni(sec, GetNameClass.GetName(() => BackupFolder), AppData.BACKUP_FOLDER, SysPath);
            ImageSaveFolder = ini.GetIni(sec, GetNameClass.GetName(() => ImageSaveFolder), AppData.EXE_FOLDER, SysPath);

            //AutoShutdown
            sec = "AutoShutdown";
            AutoShutdownEnable = ini.GetIni(sec, GetNameClass.GetName(() => AutoShutdownEnable), true, SysPath);
            AutoShutdownWaitSec = ini.GetIni(sec, GetNameClass.GetName(() => AutoShutdownWaitSec), 30, SysPath);

            //Decimal
            sec = "Decimal";
            LengthDecimal = ini.GetIni(sec, GetNameClass.GetName(() => LengthDecimal), "F1", SysPath);
            AddressDecimal = ini.GetIni(sec, GetNameClass.GetName(() => AddressDecimal), "F0", SysPath);
            NgDataDecimal = ini.GetIni(sec, GetNameClass.GetName(() => NgDataDecimal), "F1", SysPath);
            SWNgDataDecimal = ini.GetIni(sec, GetNameClass.GetName(() => SWNgDataDecimal), "F2", SysPath);
            SMNgDataDecimal = ini.GetIni(sec, GetNameClass.GetName(() => SMNgDataDecimal), "F2", SysPath);
            STNgDataDecimal = ini.GetIni(sec, GetNameClass.GetName(() => STNgDataDecimal), "F2", SysPath);
            SpeedMainDecimal = ini.GetIni(sec, GetNameClass.GetName(() => SpeedMainDecimal), "F1", SysPath);

            //Color
            string[] InspUpIDColor = new string[] { "White", "White", "White", "#80FF80", "#FFFF80", "#FF8080" };
            string[] InspDownIDColor = new string[] { "White", "White", "White", "Lime", "Yellow", "Red" };
            sec = "Color";
            for (int i = 0; i < Enum.GetNames(typeof(AppData.InspID)).Length; i++)
            {
                MarkColor mcUpSide = new MarkColor();
                markColorUpSide.Add(mcUpSide);
                markColorUpSide[i].colorARGB = ini.GetIni(sec + "UpSide", ((AppData.InspID)i).ToString(), InspUpIDColor[i], SysPath);
                MarkColor mcDownSide = new MarkColor();
                markColorDownSide.Add(mcDownSide);
                markColorDownSide[i].colorARGB = ini.GetIni(sec + "DownSide", ((AppData.InspID)i).ToString(), InspDownIDColor[i], SysPath);
            }

            //LengthMeas
            sec = "LengthMeas";
            EnableLengthMeasMonitor = ini.GetIni(sec, GetNameClass.GetName(() => EnableLengthMeasMonitor), true, SysPath);
            LengthMeasMonitorLimitSec = ini.GetIni(sec, GetNameClass.GetName(() => LengthMeasMonitorLimitSec), 30, SysPath);

            //IOポート番号
            sec = "SignalControl_DioAssign";
            OutPointRed = ini.GetIni(sec, GetNameClass.GetName(() => OutPointRed), -1, SysPath);
            OutPointGreen = ini.GetIni(sec, GetNameClass.GetName(() => OutPointGreen), -1, SysPath);
            OutPointYellow = ini.GetIni(sec, GetNameClass.GetName(() => OutPointYellow), -1, SysPath);
            OutPointBlue = ini.GetIni(sec, GetNameClass.GetName(() => OutPointBlue), -1, SysPath);
            OutPointBuzzer = ini.GetIni(sec, GetNameClass.GetName(() => OutPointBuzzer), -1, SysPath);
            OutPointExternal1 = ini.GetIni(sec, GetNameClass.GetName(() => OutPointExternal1), -1, SysPath);
            OutPointExternal2 = ini.GetIni(sec, GetNameClass.GetName(() => OutPointExternal2), -1, SysPath);
            OutPointExternal3 = ini.GetIni(sec, GetNameClass.GetName(() => OutPointExternal3), -1, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            OutPointExternal4 = ini.GetIni(sec, GetNameClass.GetName(() => OutPointExternal4), -1, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            OutPointGCustomBuzzer = ini.GetIni(sec, GetNameClass.GetName(() => OutPointGCustomBuzzer), -1, SysPath); //v1326
            InNgReset = ini.GetIni(sec, GetNameClass.GetName(() => InNgReset), -1, SysPath);    //V1293 moteki
            InErorrReset = ini.GetIni(sec, GetNameClass.GetName(() => InErorrReset), -1, SysPath);  //V1293 moteki
            InInspectionStart = ini.GetIni(sec, GetNameClass.GetName(() => InInspectionStart), -1, SysPath);    //V1293 moteki

            //V1333 追加の「OutPointBuzzerBothSide」データが無い場合　OutPointBuzzerを設定する。
            OutPointBuzzerBothSide = ini.GetIni(sec, GetNameClass.GetName(() => OutPointBuzzerBothSide), OutPointBuzzer, SysPath);

            //v1338
            PowerOffRelay = ini.GetIni(sec, GetNameClass.GetName(() => PowerOffRelay), -1, SysPath);

            //ExternalOut
            sec = "ExternalOut";
            ExternalFrontReverseDivide = ini.GetIni(sec, GetNameClass.GetName(() => ExternalFrontReverseDivide), false, SysPath); //V1057 NG表裏修正 yuasa 20190118：表裏分割有効／無効
            ExternalEnable1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalEnable1), false, SysPath);
            ExternalEnable2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalEnable2), false, SysPath);
            ExternalEnable3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalEnable3), false, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ExternalEnable4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalEnable4), false, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加

            //Signalcontrol_Param
            sec = "Signalcontrol_Param";
            ExternalResetTime1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime1), 50, SysPath);
            ExternalResetTime2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime2), 50, SysPath);
            ExternalResetTime3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime3), 50, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ExternalResetTime4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalResetTime4), 50, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            ExternalDelayTime1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime1), 0, SysPath);
            ExternalDelayTime2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime2), 0, SysPath);
            ExternalDelayTime3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime3), 0, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ExternalDelayTime4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalDelayTime4), 0, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            ExternalShot1 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot1), 0, SysPath);
            ExternalShot2 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot2), 0, SysPath);
            ExternalShot3 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot3), 0, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
            ExternalShot4 = ini.GetIni(sec, GetNameClass.GetName(() => ExternalShot4), 0, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
            Extarnal1Zone = ini.GetIni(sec, GetNameClass.GetName(() => Extarnal1Zone), "True,True,True,True,True,True,True,True", SysPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
            Extarnal2Zone = ini.GetIni(sec, GetNameClass.GetName(() => Extarnal2Zone), "True,True,True,True,True,True,True,True", SysPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
            Extarnal3Zone = ini.GetIni(sec, GetNameClass.GetName(() => Extarnal3Zone), "True,True,True,True,True,True,True,True", SysPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
            Extarnal4Zone = ini.GetIni(sec, GetNameClass.GetName(() => Extarnal4Zone), "True,True,True,True,True,True,True,True", SysPath).Split(new char[] { ',' }).Select<string, bool>(x => bool.Parse(x)).ToList();
            SystemSettingFormZoneDisp = ini.GetIni(sec, GetNameClass.GetName(() => SystemSettingFormZoneDisp), false, SysPath);

            //V1333
            //旧データが残っている場合、BuzzerTimerを表示灯（両端）と（中央）にセット。
            //ディレイは、元々機能自体無かったので、0とする。
            //なお、BuzzerTimerのSaveは存在しないので、2回目以降はelseに入り、PatLiteResetTimeBothSideやPatLiteResetTimeCenterを
            //取得する仕組み
            int BuzzerTimer = 0;
            BuzzerTimer = ini.GetIni(sec, GetNameClass.GetName(() => BuzzerTimer), 0, SysPath);

            //表示灯（両端）
            PatLiteResetTimeBothSide = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeBothSide), BuzzerTimer, SysPath);//V1333
            PatLiteDelayTimeBothSide = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeBothSide), 0, SysPath);//V1333

            //表示灯（中央）
            PatLiteResetTimeCenter = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeCenter), BuzzerTimer, SysPath);//V1333
            PatLiteDelayTimeCenter = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeCenter), 0, SysPath);//V1333

            //表示灯（両端）裏
            PatLiteResetTimeBothSideUra = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeBothSideUra), BuzzerTimer, SysPath);//V1333
            PatLiteDelayTimeBothSideUra = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeBothSideUra), 0, SysPath);//V1333

            //表示灯（中央）裏
            PatLiteResetTimeCenterUra = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeCenterUra), BuzzerTimer, SysPath);//V1333
            PatLiteDelayTimeCenterUra = ini.GetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeCenterUra), 0, SysPath);//V1333

            //OneNGImage
            sec = "OneNGImage";
            OneNGImageGraphEnable = ini.GetIni(sec, GetNameClass.GetName(() => OneNGImageGraphEnable), false, SysPath);
            OneNGImageBaseImageEnable = ini.GetIni(sec, GetNameClass.GetName(() => OneNGImageBaseImageEnable), false, SysPath);

            //WarningPopup
            sec = "WarningPopup";
            TimeWarningPopupHour = ini.GetIni(sec, GetNameClass.GetName(() => TimeWarningPopupHour), -1, SysPath);

            //PopupBackColor
            sec = "PopupBackColor";
            PopupColorNG = ini.GetIni(sec, GetNameClass.GetName(() => PopupColorNG), AppData.COLOR_YELLOW, SysPath);

            //SpeedControl
            sec = "SpeedControl";
            SpeedControlDispEnable = ini.GetIni(sec, GetNameClass.GetName(() => SpeedControlDispEnable), false, SysPath);

            //LightAjust
            sec = "LightAjust";
            LightAjustEnable = ini.GetIni(sec, GetNameClass.GetName(() => LightAjustEnable), false, SysPath);

            //ExtOutCancel
            sec = "ExtOutCancel";
            ExtOut1CancelEnable = ini.GetIni(sec, GetNameClass.GetName(() => ExtOut1CancelEnable), false, SysPath);

            //Input1Monitor
            sec = "Input1Monitor";
            Input1MonitorEnable = ini.GetIni(sec, GetNameClass.GetName(() => Input1MonitorEnable), false, SysPath);
            Input1MonitorDInNumber = ini.GetIni(sec, GetNameClass.GetName(() => Input1MonitorDInNumber), 1, SysPath);
            Input1MonitorMessage1 = ini.GetIni(sec, GetNameClass.GetName(() => Input1MonitorMessage1), "連続でエアー排出が行われていないため、\n検査を開始できません。", SysPath);

            //OutsideResetButton
            sec = "OutsideResetButton";
            OutsideResetButtonEnable = ini.GetIni(sec, GetNameClass.GetName(() => OutsideResetButtonEnable), false, SysPath);

            //OutsideManualExtButton
            sec = "OutsideManualExtButton";
            OutsideManualExtButtonEnable = ini.GetIni(sec, GetNameClass.GetName(() => OutsideManualExtButtonEnable), false, SysPath); //V1057 手動外部修正 yuasa 20190122：手動検査開始終了にini追加



            sec = "MainteLightOffset"; //V1058 メンテナンス追加 yuasa 20190125：読み取り
            //int num = ini.GetIni(sec, "MainteLightOffsetNum", 0, SysPath);
            for (int i = 0; i < 5; i++)
            {
                MainteLightOffset.Add(0);
                MainteLightOffset[i] = ini.GetIni(sec, "MainteLightOffsetNum"+i, 0, SysPath);
            }

            sec = "NGPopupWindow";
            NGPopupWindowMode = ini.GetIni(sec, GetNameClass.GetName(() => NGPopupWindowMode), false, SysPath);
            NGHistoryCount = ini.GetIni(sec, GetNameClass.GetName(() => NGHistoryCount), 10, SysPath);

            sec = "ImageMain";
            IM_Enabled = ini.GetIni(sec, GetNameClass.GetName(() => IM_Enabled), false, SysPath);
            IM_VerDisplayMode = ini.GetIni(sec, GetNameClass.GetName(() => IM_VerDisplayMode), true, SysPath);
            IM_DispInspHeight = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispInspHeight), true, SysPath);
            IM_DispInspWidth = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispInspWidth), true, SysPath);
            IM_DispMaskWidth = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispMaskWidth), true, SysPath);
            IM_DispGraphLight = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispGraphLight), false, SysPath);
            IM_DispGraphDark = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispGraphDark), true, SysPath);
            IM_DispGraphAvg = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispGraphAvg), false, SysPath);
            IM_DispKandoLine = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispKandoLine), true, SysPath);
            IM_ImageBufferCount = ini.GetIni(sec, GetNameClass.GetName(() => IM_ImageBufferCount), 5, SysPath);
            IM_AutoSaveEnable= ini.GetIni(sec, GetNameClass.GetName(() => IM_AutoSaveEnable), false, SysPath);
            IM_AutoSaveCount = ini.GetIni(sec, GetNameClass.GetName(() => IM_AutoSaveCount), 1000, SysPath);
            IM_AutoSaveOneNGsaveCount = ini.GetIni(sec, GetNameClass.GetName(() => IM_AutoSaveOneNGsaveCount), 2, SysPath);
            IM_NgCropSaveCount = ini.GetIni(sec, GetNameClass.GetName(() => IM_NgCropSaveCount), 1, SysPath);
            IM_DispGraphWidth1ch = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispGraphWidth1ch), 8, SysPath);
            IM_DispGraphWidth3ch = ini.GetIni(sec, GetNameClass.GetName(() => IM_DispGraphWidth3ch), 16, SysPath);
            IM_GraphCalcAreaAll = ini.GetIni(sec, GetNameClass.GetName(() => IM_GraphCalcAreaAll), false, SysPath);
            IM_OrgImageConnectMode = ini.GetIni(sec, GetNameClass.GetName(() => IM_OrgImageConnectMode), false, SysPath);

            sec = "Password";
            DeveloperPasswordHash = ini.GetIni(sec, GetNameClass.GetName(() => DeveloperPasswordHash), DEFAULT_DEVELOPER_PASSWORD_HASH, SysPath);

            sec = "CommonInspArea";
            int cnt = Enum.GetNames(typeof(AppData.SideID)).Length;
            InspArea_DefaultMode = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_DefaultMode), true, SysPath);
            InspArea_CmnSheetWidth = new double[cnt];
            InspArea_CmnMaskWidth = new double[cnt];
            InspArea_CmnMaskShift = new double[cnt];
            for (int i = 0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
            {
                InspArea_CmnSheetWidth[i] = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_CmnSheetWidth) + i.ToString(), 400, SysPath);
                InspArea_CmnMaskWidth[i] = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_CmnMaskWidth) + i.ToString(), 30, SysPath);
                InspArea_CmnMaskShift[i] = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_CmnMaskShift) + i.ToString(), 10, SysPath);
            }
            InspArea_ConnectMode_ImagePoint = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_ConnectMode_ImagePoint), 2, SysPath);
            InspArea_ConnectMode_BufferArea = ini.GetIni(sec, GetNameClass.GetName(() => InspArea_ConnectMode_BufferArea), 50, SysPath);

            sec = "InspectionParameter";
            InspFunc_CountNgMax = ini.GetIni(sec, GetNameClass.GetName(() => InspFunc_CountNgMax), 10, SysPath);
            InspFunc_BlobClosingCircle = ini.GetIni(sec, GetNameClass.GetName(() => InspFunc_BlobClosingCircle), 2.5, SysPath);
            InspFunc_BlobOpeningCircle = ini.GetIni(sec, GetNameClass.GetName(() => InspFunc_BlobOpeningCircle), 2.5, SysPath);
            InspFunc_BlobSelectArea = ini.GetIni(sec, GetNameClass.GetName(() => InspFunc_BlobSelectArea), 2, SysPath);

            sec = "RecipeData";
            RecipeBackupDays = ini.GetIni(sec, GetNameClass.GetName(() => RecipeBackupDays), 30, SysPath);
            RecipeBackupCount = ini.GetIni(sec, GetNameClass.GetName(() => RecipeBackupCount), 10, SysPath);
            DefaultInspWidth = ini.GetIni(sec, GetNameClass.GetName(() => DefaultInspWidth), 400, SysPath);
            DefaultMaskWidth = ini.GetIni(sec, GetNameClass.GetName(() => DefaultMaskWidth), 20, SysPath);
            DefaultMaskShift = ini.GetIni(sec, GetNameClass.GetName(() => DefaultMaskShift), 10, SysPath);
            RecipeZoneSetteingEnable = ini.GetIni(sec, GetNameClass.GetName(() => RecipeZoneSetteingEnable), false, SysPath);

            sec = "AutoLight";
            AutoLightCheckImageCount = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightCheckImageCount), 15, SysPath);
            AutoLightOkImageCount = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightOkImageCount), 5, SysPath);
            AutoLightGainUpStep = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightGainUpStep), 0, SysPath);
            AutoLightGainMaxCount = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightGainMaxCount), 0, SysPath);
            AutoLightOkLowLimit = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightOkLowLimit), -5, SysPath);
            AutoLightOkHighLimit = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightOkHighLimit), +5, SysPath);
            AutoLightDetailUpLevel = ini.GetIni(sec, GetNameClass.GetName(() => AutoLightDetailUpLevel), 2, SysPath);

            sec = "LinesSetting";
            LineKandoGrapthColor = ini.GetIni(sec, GetNameClass.GetName(() => LineKandoGrapthColor), "Magenta", SysPath);
            LineBaseKandoColor = ini.GetIni(sec, GetNameClass.GetName(() => LineBaseKandoColor), "Yellow", SysPath);
            LineInspHeightColor = ini.GetIni(sec, GetNameClass.GetName(() => LineInspHeightColor), "Red", SysPath);
            LineInspWidthColor = ini.GetIni(sec, GetNameClass.GetName(() => LineInspWidthColor), "Green", SysPath);
            LineStateMonitorColor = ini.GetIni(sec, GetNameClass.GetName(() => LineStateMonitorColor), "Red", SysPath);
            LineKandoGrapthThick = ini.GetIni(sec, GetNameClass.GetName(() => LineKandoGrapthThick), 1, SysPath);
            LineBaseKandoThick = ini.GetIni(sec, GetNameClass.GetName(() => LineBaseKandoThick), 1, SysPath);
            LineInspHeightThick = ini.GetIni(sec, GetNameClass.GetName(() => LineInspHeightThick), 2, SysPath);
            LineInspWidthThick = ini.GetIni(sec, GetNameClass.GetName(() => LineInspWidthThick), 1, SysPath);
            LineStateMonitorThick = ini.GetIni(sec, GetNameClass.GetName(() => LineStateMonitorThick), 1, SysPath);
            LineKandoThick = new int[Enum.GetNames(typeof(AppData.InspID)).Length];
            for (int i=0; i<LineKandoThick.Length; i++)
                LineKandoThick[i] = ini.GetIni(sec, GetNameClass.GetName(() => LineKandoThick) + i.ToString(), 1, SysPath);

            sec = "ContinuNG";
            ContinuNGEnable = ini.GetIni(sec, GetNameClass.GetName(() => ContinuNGEnable), false, SysPath);
            ContinuNGJudgeCount = ini.GetIni(sec, GetNameClass.GetName(() => ContinuNGJudgeCount), 3, SysPath);
            ContinuNGAfterCancelCount = ini.GetIni(sec, GetNameClass.GetName(() => ContinuNGAfterCancelCount), 10, SysPath);

            sec = "OutDrive";
            OutDrive = ini.GetIni(sec, GetNameClass.GetName(() => OutDrive), "", SysPath);

            sec = "SerialSpeedControl1";
            SerialEnable1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialEnable1), false, SysPath);
            SerialCycleWaitTime1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialCycleWaitTime1), 1000, SysPath);
            SerialComPort1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialComPort1), "COM3",  SysPath);
            SerialBaudRate1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialBaudRate1), 9600, SysPath);
            SerialParity1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialParity1), "None", SysPath);
            SerialDataBits1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialDataBits1), 8, SysPath);
            SerialStopBits1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialStopBits1), "One", SysPath);
            SerialHandshake1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialHandshake1), "None", SysPath);
            SerialEnCoding1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialEnCoding1), "ascii", SysPath);
            SerialRtnCode1 = ini.GetIni(sec, GetNameClass.GetName(() => SerialRtnCode1),"EXT", SysPath);
            sec = "SerialSpeedControl2";
            SerialEnable2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialEnable2), false, SysPath);
            SerialCycleWaitTime2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialCycleWaitTime2), 1000, SysPath);
            SerialComPort2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialComPort2), "COM5", SysPath);
            SerialBaudRate2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialBaudRate2), 9600, SysPath);
            SerialParity2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialParity2), "None", SysPath);
            SerialDataBits2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialDataBits2), 8, SysPath);
            SerialStopBits2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialStopBits2), "One", SysPath);
            SerialHandshake2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialHandshake2), "None", SysPath);
            SerialEnCoding2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialEnCoding2), "ascii", SysPath);
            SerialRtnCode2 = ini.GetIni(sec, GetNameClass.GetName(() => SerialRtnCode2), "EXT", SysPath);

            sec = "NetConnectMonitor";
            NetConnectMonitor_ConnectTimeOut = ini.GetIni(sec, GetNameClass.GetName(() => NetConnectMonitor_ConnectTimeOut), 0, SysPath);
            NetConnectMonitor_Enable = ini.GetIni(sec, GetNameClass.GetName(() => NetConnectMonitor_Enable), false, SysPath);

            sec = "CommonItems";
            Common_RecipeRealSpeedOmote = ini.GetIni(sec, GetNameClass.GetName(() => Common_RecipeRealSpeedOmote), 30.0, SysPath);
            Common_RecipeRealSpeedUra = ini.GetIni(sec, GetNameClass.GetName(() => Common_RecipeRealSpeedUra), 30.0, SysPath);

            sec = "LightRepeat";
            LightRepeatON = ini.GetIni(sec, GetNameClass.GetName(() => LightRepeatON), false, SysPath);//v1325 LightRepeatON追加

            sec = "GCustom";//v1326
            GCustomEnable = ini.GetIni(sec, GetNameClass.GetName(() => GCustomEnable), false, SysPath);
            CommonPatLiteDelaySecond = ini.GetIni(sec, GetNameClass.GetName(() => CommonPatLiteDelaySecond), 0, SysPath);
            CommonPatLiteOnTimeSecond = ini.GetIni(sec, GetNameClass.GetName(() => CommonPatLiteOnTimeSecond), 0, SysPath);
            InFlipperoOserveCH = ini.GetIni(sec, GetNameClass.GetName(() => InFlipperoOserveCH), -1, SysPath);
            InPatLiteStopSWCH = ini.GetIni(sec, GetNameClass.GetName(() => InPatLiteStopSWCH), -1, SysPath);
            GCustomDIOCycleTime = ini.GetIni(sec, GetNameClass.GetName(() => GCustomDIOCycleTime), 5, SysPath);

            sec = "PowerOffButton";//v1338
            PowerOffButtonEnable = ini.GetIni(sec, GetNameClass.GetName(() => PowerOffButtonEnable), false, SysPath);
            PowerOffButtonDioIndex = ini.GetIni(sec, GetNameClass.GetName(() => PowerOffButtonDioIndex), -1, SysPath);
            PowerOffButtonHighLow = ini.GetIni(sec, GetNameClass.GetName(() => PowerOffButtonHighLow), true, SysPath);
            PowerOffButtonOnTime = ini.GetIni(sec, GetNameClass.GetName(() => PowerOffButtonOnTime), 3000, SysPath);

            CheckDirectory();
        }

        public void SystemSaveBackup()                      // sao lưu hệ thống copy file thành 1 file lưu lại
        {
            if (System.IO.File.Exists(SysPath))
                System.IO.File.Copy(SysPath, SysPath + "_SystemBackup", true);
        }
        public bool RestoreBackupFile()                     // khôi phục tệp sao lưu. copy file dự phòng bên trên vào file cũ SysPath
        {
            string sBackupPath = SysPath + "_SystemBackup";
            try
            {
                if (System.IO.File.Exists(sBackupPath))     //kiểm tra xem một tệp có tồn tại hay không tại đường dẫn sBackupPath.  true nếu tệp tồn tại, và false nếu tệp không tồn tại.
                    System.IO.File.Copy(sBackupPath, SysPath, true); // sao chép từ đường dẫn sBackupPath sang đường dẫn SysPath. nếu là true thì ghi đè nếu là false thì hay xảy ra lỗi
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //システムパラメーターのセーブ                    lưu thông số hệ thống vào file
        public void SystemSave()
        {
            if (SystemStatus.GetInstance().RestoreShutdown == true)
                return;
            string sMoveFile = SysPath + DateTime.Now.ToString("_yyyyMMdd_HHmmss");
            System.IO.File.Move(SysPath, sMoveFile);

            try
            {
                IniFileAccess ini = new IniFileAccess();
                string sec;

                //cam
                for (int i = 0; AppData.CAM_COUNT > i; i++)
                {
                    sec = "cam" + i.ToString();
                    //CamID
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].CamID), camParam[i].CamID, SysPath);
                    //CamNo
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].CamNo), camParam[i].CamNo, SysPath);
                    //OnOff
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].OnOff), camParam[i].OnOff, SysPath);
                    //
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].PixV), camParam[i].PixV, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].PixH), camParam[i].PixH, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].ResoV), camParam[i].ResoV, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].ResoH), camParam[i].ResoH, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].ShiftV), camParam[i].ShiftV, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].ShiftH), camParam[i].ShiftH, SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].CamParts), camParam[i].CamParts, SysPath);
                    //破棄する枚数
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].DiscardCount), camParam[i].DiscardCount, SysPath);
                    //UseLightNo
                    ini.SetIni(sec, GetNameClass.GetName(() => camParam[i].UseLightNo), string.Join(",", camParam[i].UseLightNo.Select(x => x.ToString()).ToArray()), SysPath);

                    //基準露光値
                    ini.SetIni(sec, GetNameClass.GetName(() => ExposureDefault) + "1", ExposureDefault[i, 0], SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => ExposureDefault) + "2", ExposureDefault[i, 1], SysPath);
                }

                //System
                sec = "System";
                ini.SetIni(sec, GetNameClass.GetName(() => ForceCenteringMode), ForceCenteringMode, SysPath);

                //LotNo
                sec = "DisplayEnable";
                ini.SetIni(sec, GetNameClass.GetName(() => SuspendButtonEnable), SuspendButtonEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LotNoEnable), LotNoEnable, SysPath);

                //LightTime
                sec = "LightTime";
                ini.SetIni(sec, GetNameClass.GetName(() => LightWarningTime), LightWarningTime, SysPath);

                //DeleteMonth
                sec = "DeleteMonth";
                ini.SetIni(sec, GetNameClass.GetName(() => DeleteMonth), DeleteMonth, SysPath);

                //SideMask
                sec = "SideMask";
                ini.SetIni(sec, GetNameClass.GetName(() => SideMaskEnable), SideMaskEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SideMaskThreshold), SideMaskThreshold, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SideMaskDilation), SideMaskDilation, SysPath);

                //SoftShading
                sec = "SoftShading";
                ini.SetIni(sec, GetNameClass.GetName(() => SoftShadingEnable), SoftShadingEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SoftShadingTargetGrayLevel), SoftShadingTargetGrayLevel, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SoftShadingLimit), SoftShadingLimit, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SoftShadingCalcImageCount), SoftShadingCalcImageCount, SysPath);

                //BrightDarkInspection
                sec = "BrightDarkInspection";
                ini.SetIni(sec, GetNameClass.GetName(() => InspBrightEnable), InspBrightEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InspDarkEnable), InspDarkEnable, SysPath);

                //ColorCamera
                sec = "ColorCamera";
                ini.SetIni("ColorCamInspImage", GetNameClass.GetName(() => ColorCamInspImage), string.Join(",", ColorCamInspImage.Select(x => x.ToString()).ToArray()), SysPath);

                //OutofWhiteBlack
                sec = "OutofWhiteBlack";
                ini.SetIni(sec, GetNameClass.GetName(() => OutofWhiteEnabled), OutofWhiteEnabled, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutofBlackEnabled), OutofBlackEnabled, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutofWhiteLimit), OutofWhiteLimit, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutofBlackLimit), OutofBlackLimit, SysPath);

                //CameraSetting
                sec = "CameraSetting";
                ini.SetIni(sec, GetNameClass.GetName(() => CamExposure), CamExposure, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CamSpeed), CamSpeed, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CamExposureUra), CamExposureUra, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CamSpeedUra), CamSpeedUra, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CamCloseOpenEnable), CamCloseOpenEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CamCloseOpenAutoLightingEnable), CamCloseOpenAutoLightingEnable, SysPath);

                //ImageCliping
                sec = "ImageCliping";
                ini.SetIni(sec, GetNameClass.GetName(() => CropWidth), CropWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CropHeight), CropHeight, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ScaleWidth), ScaleWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ScaleHeight), ScaleHeight, SysPath);

                //Path
                sec = "Path";
                ini.SetIni(sec, GetNameClass.GetName(() => CommunicatioPath), CommunicatioPath, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CameraPath), CameraPath, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LightCtrlPath), LightCtrlPath, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ImageFolder), ImageFolder, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ProductTimeFolder), ProductTimeFolder, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ProductFolder), ProductFolder, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => RecipeFoldr), RecipeFoldr, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => BackupFolder), BackupFolder, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ImageSaveFolder), ImageSaveFolder, SysPath);

                //AutoShutdown
                sec = "AutoShutdown";
                ini.SetIni(sec, GetNameClass.GetName(() => AutoShutdownEnable), AutoShutdownEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoShutdownWaitSec), AutoShutdownWaitSec, SysPath);

                //Decimal
                sec = "Decimal";
                ini.SetIni(sec, GetNameClass.GetName(() => LengthDecimal), LengthDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AddressDecimal), AddressDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => NgDataDecimal), NgDataDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SWNgDataDecimal), SWNgDataDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SMNgDataDecimal), SMNgDataDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => STNgDataDecimal), STNgDataDecimal, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SpeedMainDecimal), SpeedMainDecimal, SysPath);


                //Color
                sec = "Color";
                for (int i = 0; i < Enum.GetNames(typeof(AppData.InspID)).Length; i++)
                {
                    ini.SetIni(sec + "UpSide", ((AppData.InspID)i).ToString(), markColorUpSide[i].colorARGB, SysPath);
                    ini.SetIni(sec + "DownSide", ((AppData.InspID)i).ToString(), markColorDownSide[i].colorARGB, SysPath);
                }

                //LengthMeas
                sec = "LengthMeas";
                ini.SetIni(sec, GetNameClass.GetName(() => EnableLengthMeasMonitor), EnableLengthMeasMonitor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LengthMeasMonitorLimitSec), LengthMeasMonitorLimitSec, SysPath);

                //IOポート番号
                sec = "SignalControl_DioAssign";
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointRed), OutPointRed, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointGreen), OutPointGreen, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointYellow), OutPointYellow, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointBlue), OutPointBlue, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointBuzzer), OutPointBuzzer, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointExternal1), OutPointExternal1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointExternal2), OutPointExternal2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointExternal3), OutPointExternal3, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointExternal4), OutPointExternal4, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointGCustomBuzzer), OutPointGCustomBuzzer, SysPath); //v1326
                ini.SetIni(sec, GetNameClass.GetName(() => InNgReset), InNgReset, SysPath);    //V1293 moteki
                ini.SetIni(sec, GetNameClass.GetName(() => InErorrReset), InErorrReset, SysPath);   //V1293 moteki
                ini.SetIni(sec, GetNameClass.GetName(() => InInspectionStart), InInspectionStart, SysPath); //V1293 moteki
                ini.SetIni(sec, GetNameClass.GetName(() => OutPointBuzzerBothSide), OutPointBuzzerBothSide, SysPath); //V1333
                ini.SetIni(sec, GetNameClass.GetName(() => PowerOffRelay), PowerOffRelay, SysPath); //v1338

                //ExternalOut
                sec = "ExternalOut";
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalFrontReverseDivide), ExternalFrontReverseDivide, SysPath); //V1057 NG表裏修正 yuasa 20190118：表裏分割有効／無効
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalEnable1), ExternalEnable1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalEnable2), ExternalEnable2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalEnable3), ExternalEnable3, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalEnable4), ExternalEnable4, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加

                //Signalcontrol_Param
                sec = "Signalcontrol_Param";
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime1), ExternalResetTime1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime2), ExternalResetTime2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime3), ExternalResetTime3, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalResetTime4), ExternalResetTime4, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime1), ExternalDelayTime1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime2), ExternalDelayTime2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime3), ExternalDelayTime3, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalDelayTime4), ExternalDelayTime4, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot1), ExternalShot1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot2), ExternalShot2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot3), ExternalShot3, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部３追加
                ini.SetIni(sec, GetNameClass.GetName(() => ExternalShot4), ExternalShot4, SysPath); //V1057 NG表裏修正 yuasa 20190118：外部４追加
                ini.SetIni(sec, GetNameClass.GetName(() => Extarnal1Zone), string.Join(",", Extarnal1Zone.Select(x => x.ToString()).ToArray()), SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Extarnal2Zone), string.Join(",", Extarnal2Zone.Select(x => x.ToString()).ToArray()), SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Extarnal3Zone), string.Join(",", Extarnal3Zone.Select(x => x.ToString()).ToArray()), SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Extarnal4Zone), string.Join(",", Extarnal4Zone.Select(x => x.ToString()).ToArray()), SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SystemSettingFormZoneDisp), SystemSettingFormZoneDisp, SysPath);
                //ini.SetIni(sec, GetNameClass.GetName(() => BuzzerTimer), BuzzerTimer, SysPath);

                //表示灯（両端）
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeBothSide), PatLiteResetTimeBothSide, SysPath);//V1333
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeBothSide), PatLiteDelayTimeBothSide, SysPath);//V1333

                //表示灯（中央）
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeCenter), PatLiteResetTimeCenter, SysPath);//V1333
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeCenter), PatLiteDelayTimeCenter, SysPath);//V1333

                //表示灯（両端）裏
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeBothSideUra), PatLiteResetTimeBothSideUra, SysPath);//V1333
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeBothSideUra), PatLiteDelayTimeBothSideUra, SysPath);//V1333

                //表示灯（中央）裏
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteResetTimeCenterUra), PatLiteResetTimeCenterUra, SysPath);//V1333
                ini.SetIni(sec, GetNameClass.GetName(() => PatLiteDelayTimeCenterUra), PatLiteDelayTimeCenterUra, SysPath);//V1333

                //OneNGImage
                sec = "OneNGImage";
                ini.SetIni(sec, GetNameClass.GetName(() => OneNGImageGraphEnable), OneNGImageGraphEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => OneNGImageBaseImageEnable), OneNGImageBaseImageEnable, SysPath);

                //WarningPopup
                sec = "WarningPopup";
                ini.SetIni(sec, GetNameClass.GetName(() => TimeWarningPopupHour), TimeWarningPopupHour, SysPath);

                //PopupBackColor
                sec = "PopupBackColor";
                ini.SetIni(sec, GetNameClass.GetName(() => PopupColorNG), PopupColorNG, SysPath);

                //SpeedControl
                sec = "SpeedControl";
                ini.SetIni(sec, GetNameClass.GetName(() => SpeedControlDispEnable), SpeedControlDispEnable, SysPath);

                //LightAjust
                sec = "LightAjust";
                ini.SetIni(sec, GetNameClass.GetName(() => LightAjustEnable), LightAjustEnable, SysPath);

                //ExtOutCancel
                sec = "ExtOutCancel";
                ini.SetIni(sec, GetNameClass.GetName(() => ExtOut1CancelEnable), ExtOut1CancelEnable, SysPath);

                //Input1Monitor
                sec = "Input1Monitor";
                ini.SetIni(sec, GetNameClass.GetName(() => Input1MonitorEnable), Input1MonitorEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Input1MonitorDInNumber), Input1MonitorDInNumber, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Input1MonitorMessage1), Input1MonitorMessage1, SysPath);

                //OutsideResetButton
                sec = "OutsideResetButton";
                ini.SetIni(sec, GetNameClass.GetName(() => OutsideResetButtonEnable), OutsideResetButtonEnable, SysPath);

                sec = "MainteLightOffset"; //V1058 メンテナンス追加 yuasa 20190125：書き込み追加
                                           //ini.SetIni(sec, "MainteLightOffsetNum", MainteLightOffset.Count, SysPath);
                for (int i = 0; i < 5; i++)
                {
                    ini.SetIni(sec, "MainteLightOffsetNum" + i.ToString(), MainteLightOffset[i], SysPath);
                }

                //OutsideManualExtButton
                sec = "OutsideManualExtButton";
                ini.SetIni(sec, GetNameClass.GetName(() => OutsideManualExtButtonEnable), OutsideManualExtButtonEnable, SysPath); //V1057 手動外部修正 yuasa 20190122：手動検査開始終了にini追加

                sec = "NGPopupWindow";
                ini.SetIni(sec, GetNameClass.GetName(() => NGPopupWindowMode), NGPopupWindowMode, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => NGHistoryCount), NGHistoryCount, SysPath);

                sec = "ImageMain";
                ini.SetIni(sec, GetNameClass.GetName(() => IM_Enabled), IM_Enabled, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_VerDisplayMode), IM_VerDisplayMode, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispInspHeight), IM_DispInspHeight, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispInspWidth), IM_DispInspWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispMaskWidth), IM_DispMaskWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispGraphLight), IM_DispGraphLight, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispGraphDark), IM_DispGraphDark, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispGraphAvg), IM_DispGraphAvg, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispKandoLine), IM_DispKandoLine, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_ImageBufferCount), IM_ImageBufferCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_AutoSaveEnable), IM_AutoSaveEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_AutoSaveCount), IM_AutoSaveCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_AutoSaveOneNGsaveCount), IM_AutoSaveOneNGsaveCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_NgCropSaveCount), IM_NgCropSaveCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispGraphWidth1ch), IM_DispGraphWidth1ch, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_DispGraphWidth3ch), IM_DispGraphWidth3ch, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_GraphCalcAreaAll), IM_GraphCalcAreaAll, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => IM_OrgImageConnectMode), IM_OrgImageConnectMode, SysPath);

                sec = "Password";
                ini.SetIni(sec, GetNameClass.GetName(() => DeveloperPasswordHash), DeveloperPasswordHash, SysPath);

                sec = "CommonInspArea";
                int cnt = Enum.GetNames(typeof(AppData.SideID)).Length;
                ini.SetIni(sec, GetNameClass.GetName(() => InspArea_DefaultMode), InspArea_DefaultMode, SysPath);
                for (int i = 0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
                {
                    ini.SetIni(sec, GetNameClass.GetName(() => InspArea_CmnSheetWidth) + i.ToString(), InspArea_CmnSheetWidth[i], SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => InspArea_CmnMaskWidth) + i.ToString(), InspArea_CmnMaskWidth[i], SysPath);
                    ini.SetIni(sec, GetNameClass.GetName(() => InspArea_CmnMaskShift) + i.ToString(), InspArea_CmnMaskShift[i], SysPath);
                }
                ini.SetIni(sec, GetNameClass.GetName(() => InspArea_ConnectMode_ImagePoint), InspArea_ConnectMode_ImagePoint, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InspArea_ConnectMode_BufferArea), InspArea_ConnectMode_BufferArea, SysPath);

                sec = "InspectionParameter";
                ini.SetIni(sec, GetNameClass.GetName(() => InspFunc_CountNgMax), InspFunc_CountNgMax, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InspFunc_BlobClosingCircle), InspFunc_BlobClosingCircle, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InspFunc_BlobOpeningCircle), InspFunc_BlobOpeningCircle, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InspFunc_BlobSelectArea), InspFunc_BlobSelectArea, SysPath);

                sec = "RecipeData";
                ini.SetIni(sec, GetNameClass.GetName(() => RecipeBackupDays), RecipeBackupDays, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => RecipeBackupCount), RecipeBackupCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => DefaultInspWidth), DefaultInspWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => DefaultMaskWidth), DefaultMaskWidth, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => DefaultMaskShift), DefaultMaskShift, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => RecipeZoneSetteingEnable), RecipeZoneSetteingEnable, SysPath);

                sec = "AutoLight";
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightCheckImageCount), AutoLightCheckImageCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightOkImageCount), AutoLightOkImageCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightGainUpStep), AutoLightGainUpStep, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightGainMaxCount), AutoLightGainMaxCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightOkLowLimit), AutoLightOkLowLimit, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightOkHighLimit), AutoLightOkHighLimit, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => AutoLightDetailUpLevel), AutoLightDetailUpLevel, SysPath);

                sec = "LinesSetting";
                ini.SetIni(sec, GetNameClass.GetName(() => LineKandoGrapthColor), LineKandoGrapthColor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineBaseKandoColor), LineBaseKandoColor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineInspHeightColor), LineInspHeightColor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineInspWidthColor), LineInspWidthColor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineStateMonitorColor), LineStateMonitorColor, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineKandoGrapthThick), LineKandoGrapthThick, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineBaseKandoThick), LineBaseKandoThick, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineInspHeightThick), LineInspHeightThick, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineInspWidthThick), LineInspWidthThick, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => LineStateMonitorThick), LineStateMonitorThick, SysPath);
                for (int i = 0; i < LineKandoThick.Length; i++)
                    ini.SetIni(sec, GetNameClass.GetName(() => LineKandoThick) + i.ToString(), LineKandoThick[i], SysPath);

                sec = "ContinuNG";
                ini.SetIni(sec, GetNameClass.GetName(() => ContinuNGEnable), ContinuNGEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ContinuNGJudgeCount), ContinuNGJudgeCount, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => ContinuNGAfterCancelCount), ContinuNGAfterCancelCount, SysPath);

                sec = "OutDrive";
                ini.SetIni(sec, GetNameClass.GetName(() => OutDrive), OutDrive, SysPath);

                sec = "SerialSpeedControl1";
                ini.SetIni(sec, GetNameClass.GetName(() => SerialEnable1), SerialEnable1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialCycleWaitTime1), SerialCycleWaitTime1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialComPort1), SerialComPort1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialBaudRate1), SerialBaudRate1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialParity1), SerialParity1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialDataBits1), SerialDataBits1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialStopBits1), SerialStopBits1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialHandshake1), SerialHandshake1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialEnCoding1), SerialEnCoding1, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialRtnCode1), SerialRtnCode1, SysPath);
                sec = "SerialSpeedControl2";
                ini.SetIni(sec, GetNameClass.GetName(() => SerialEnable2), SerialEnable2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialCycleWaitTime2), SerialCycleWaitTime2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialComPort2), SerialComPort2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialBaudRate2), SerialBaudRate2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialParity2), SerialParity2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialDataBits2), SerialDataBits2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialStopBits2), SerialStopBits2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialHandshake2), SerialHandshake2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialEnCoding2), SerialEnCoding2, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => SerialRtnCode2), SerialRtnCode2, SysPath);

                sec = "NetConnectMonitor";
                ini.SetIni(sec, GetNameClass.GetName(() => NetConnectMonitor_ConnectTimeOut), NetConnectMonitor_ConnectTimeOut, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => NetConnectMonitor_Enable), NetConnectMonitor_Enable, SysPath);

                sec = "CommonItems";
                ini.SetIni(sec, GetNameClass.GetName(() => Common_RecipeRealSpeedOmote), Common_RecipeRealSpeedOmote, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => Common_RecipeRealSpeedUra), Common_RecipeRealSpeedUra, SysPath);

                sec = "LightRepeat";
                ini.SetIni(sec, GetNameClass.GetName(() => LightRepeatON), LightRepeatON, SysPath);//v1325 LightRepeatON追加

                sec = "GCustom";//v1326
                ini.SetIni(sec, GetNameClass.GetName(() => GCustomEnable), GCustomEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CommonPatLiteDelaySecond), CommonPatLiteDelaySecond, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => CommonPatLiteOnTimeSecond), CommonPatLiteOnTimeSecond, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InFlipperoOserveCH), InFlipperoOserveCH, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => InPatLiteStopSWCH), InPatLiteStopSWCH, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => GCustomDIOCycleTime), GCustomDIOCycleTime, SysPath);

                sec = "PowerOffButton";//v1338
                ini.SetIni(sec, GetNameClass.GetName(() => PowerOffButtonEnable), PowerOffButtonEnable, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => PowerOffButtonDioIndex), PowerOffButtonDioIndex, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => PowerOffButtonHighLow), PowerOffButtonHighLow, SysPath);
                ini.SetIni(sec, GetNameClass.GetName(() => PowerOffButtonOnTime), PowerOffButtonOnTime, SysPath);

                //v1329 Flushを追加して保存を促進
                ini.Flush(SysPath);
            }
            catch (Exception)
            {
                System.IO.File.Copy(sMoveFile, SysPath, true);
            }
            finally
            {
                System.IO.File.Delete(sMoveFile);
            }
        }

        public void SaveExposureDefault()                   // lưu mặc định
        {
            IniFileAccess ini = new IniFileAccess();
            //基準露光値
            for (int k = 0; this.ExposureDefault.GetLength(0) > k; k++)
            {
                ini.SetIniString("Cam" + k.ToString(), "ExposeDefVal1",  this.ExposureDefault[k, 0].ToString() , SysPath);
                ini.SetIniString("Cam" + k.ToString(), "ExposeDefVal2", this.ExposureDefault[k, 1].ToString(), SysPath);
            }
        }

        private void CheckDirectory()
        {
           //各フォルダの作成
            if(!Directory.Exists(this.RecipeFoldr))
            {
                Directory.CreateDirectory(this.RecipeFoldr);  
            }

            if(!Directory.Exists(this.ProductFolder))
            {
                Directory.CreateDirectory(this.ProductFolder);
            }

            if (!Directory.Exists(this.ProductTimeFolder))
            {
                Directory.CreateDirectory(this.ProductTimeFolder);
            }

            if (!Directory.Exists(this.ImageFolder))
            {
                Directory.CreateDirectory(this.ImageFolder);
            }
        }

        /// <summary>
        /// 速度　→　周波数
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public double Speed2Hz(double speed)
        {
            double hz;
            hz = (speed / 60.0) * 1000.0;
            hz = hz / SystemParam.GetInstance().camParam[0].ResoV;
            return hz;
        }
        /// <summary>
        /// 周波数　→　速度
        /// </summary>
        /// <param name="hz"></param>
        /// <returns></returns>
        public double Hz2Speed(double hz)
        {
            double speed;
            speed = (hz * SystemParam.GetInstance().camParam[0].ResoV) / 1000.0 * 60.0;
            return speed;
        }

        /// <summary>
        /// 速度表示
        /// </summary>
        /// <param name="hz"></param>
        public void DispHz(double hz, System.Windows.Forms.Label lHz, System.Windows.Forms.Label lExp)
        {
            lHz.Text = hz.ToString("F1");
            lExp.Text = ((1.0 / hz) * 1000.0 * 1000.0).ToString("F1");//(us)
        }


        public bool CheckCameraIndex(int camIndex, out AppData.SideID side)
        {
            side = AppData.SideID.表;

            bool enable = false;
            for (int i = 0; i < SystemParam.GetInstance().camParam.Count; i++)
            {
                if (SystemParam.GetInstance().camParam[i].CamNo == camIndex &&
                    SystemParam.GetInstance().camParam[i].OnOff == true)
                {
                    side = SystemParam.GetInstance().camParam[i].CamParts;
                    enable = true;
                    break;
                }
            }
            return enable;
        }

        public bool GetImageHeightArea(int imgHeight, out int startHeight, out int endHeight, out int iUnderHeight)
        {
            startHeight = 0;
            endHeight = 0;
            bool IsConnectImage = false;

            try
            {
                HalconCameraBase cam = APCameraManager.getInstance().GetCamera(0);
                IsConnectImage = cam.IsConnectVerticalImage;
                int oneHeight = cam.ImageHeight;

                startHeight = imgHeight - (oneHeight * InspArea_ConnectMode_ImagePoint);
                endHeight = imgHeight - ((oneHeight * InspArea_ConnectMode_ImagePoint) - oneHeight - InspArea_ConnectMode_BufferArea);

                if (IsConnectImage == true)
                    iUnderHeight = imgHeight - oneHeight;
                else
                    iUnderHeight = imgHeight - SystemParam.GetInstance().camParam[0].PixV;
            }
            catch(Exception exc)
            {
                throw exc;
            }
            return IsConnectImage;
        }

        public void GetDirAndFile(string dirName, string fName, out string dir, out string withoutFileName, out string[] fooder)
        {
            string path = Path.Combine(dirName, fName);   // kết hợp thành 1 đường dẫn
            dir = Path.GetDirectoryName(path);              // thông tin thư mục
            withoutFileName = Path.GetFileNameWithoutExtension(path);
            fooder = new string[] { "", "a", "b", "c" };
            return;
        }

    }

    public class MarkColor
    {
        //色
        public string colorARGB;
    }

    /// <summary>
    ///　照明番号
    /// </summary>
    public enum LedNum
    {
        NoLed = -1,
        Led1,
        Led2,
        Led3,
        Led4,
        Led5,
        Led6,
    }

    /// <summary>
    ///　固有(ｼｽﾃﾑ)か個別(ﾚｼﾋﾟ)
    /// </summary>
    public enum EncSetup
    {
        System,
        Recipe
    }

    /// <summary>
    ///　最大速度
    /// </summary>
    public enum Speed
    {
        Max10,
        Max20
    }
}
