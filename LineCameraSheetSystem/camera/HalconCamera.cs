using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.IO;
using LogingDllWrap;

using HalconDotNet;
using Fujita.Misc;
#if FUJITA_INSPECTION_SYSTEM 
using Fujita.Misc;
using Fujita.InspectionSystem;
#endif

namespace HalconCamera
{
    /// <summary>
    /// カメラ画像取得イベント情報
    /// </summary>
    public class GrabbedImageEventArgs : EventArgs
    {
        /// <summary>
        /// カメラのインデックス番号
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// カメラから取得した画像
        /// </summary>
        public HObject OrgImage { get { return _orgImage; } }
        private HObject _orgImage;
        public HObject ShadingImage { get { return _shadingImage; } }
        private HObject _shadingImage;
        /// <summary>
        /// カメラから画像を取得した日時
        /// </summary>
        public DateTime GrabbedTime { get; set; }

        /// <summary>
        /// オブジェクトを初期化する。
        /// </summary>
        /// <param name="orgImage">カメラから取得した画像</param>
        /// <param name="grabbedTime">カメラから画像を取得した日時</param>
        public GrabbedImageEventArgs(HObject orgImage, HObject shadingImage, DateTime grabbedTime, int index)
        {
            HOperatorSet.CopyObj(orgImage, out _orgImage, 1, -1);
            HOperatorSet.CopyObj(shadingImage, out _shadingImage, 1, -1);
            //HOperatorSet.CopyImage(orgImage, out _orgImage);
            //HOperatorSet.CopyImage(shadingImage, out _shadingImage);
            this.GrabbedTime = grabbedTime;
            this.Index = index;
        }
    }

    public delegate void GrabbedImageEventHandler(object sender, GrabbedImageEventArgs e);

    public enum CameraType
    {
        Unknown,
        PylonGigE,
        LinX,
    }

    public class HalconCameraBase : IDisposable, IError
#if FUJITA_INSPECTION_SYSTEM
        , IDispSettings, ISerialize
#endif
    {
        public enum TriggerMode
        {
            Unknown,
            Software,
            Hardware,
            Line1,
            Line2,
            Line3,
            CC1,
            CC2,
            CC3,
            CC4,
            FreeRun,
        }

        public enum ColorMode
        {
            Unknown,
            Gray,
            RGB,
            RAW,
            YUV,
        }

        public enum LineInput
        {
            InternalLineRate,
            ExternalPulse,
        }

        public event GrabbedImageEventHandler OnGrabbedImage;

#if FUJITA_INSPECTION_SYSTEM

        public void DispSetting(System.Windows.Forms.ListView view)
        {
            uclSettingData.AddSettingList(view, Name, "ｶﾒﾗｲﾝﾃﾞｯｸｽ", Index, "");
            uclSettingData.AddSettingList(view, Name, "横", ImageWidth, "[pix]");
            uclSettingData.AddSettingList(view, Name, "縦", ImageHeight, "[pix]");

            uclSettingData.AddSettingList(view, Name, "解像度横", ResolutionHorz, "[um/pix]");
            uclSettingData.AddSettingList(view, Name, "解像度縦", ResolutionVert, "[um/pix]");

            uclSettingData.AddSettingList(view, "", "ｹﾞｲﾝ有効", EnableGain, "");
            if (EnableGain)
            {
                uclSettingData.AddSettingList(view, "", "ｹﾞｲﾝValue", Gain, "");
                uclSettingData.AddSettingList(view, "", "ｹﾞｲﾝMin", GainMin, "");
                uclSettingData.AddSettingList(view, "", "ｹﾞｲﾝMax", GainMax, "");
            }
            uclSettingData.AddSettingList(view, "", "ｵﾌｾｯﾄ有効", EnableOffset, "");
            if (EnableOffset)
            {
                uclSettingData.AddSettingList(view, "", "ｵﾌｾｯﾄValue", Offset, "");
                uclSettingData.AddSettingList(view, "", "ｵﾌｾｯﾄMin", OffsetMin, "");
                uclSettingData.AddSettingList(view, "", "ｵﾌｾｯﾄMax", OffsetMax, "");
            }
            uclSettingData.AddSettingList(view, "", "露光時間有効", EnableExposureTime, "");
            if (EnableExposureTime)
            {
                uclSettingData.AddSettingList(view, "", "露光時間Value", ExposureTime, "");
                uclSettingData.AddSettingList(view, "", "露光時間Min", ExposureTimeMin, "");
                uclSettingData.AddSettingList(view, "", "露光時間Max", ExposureTimeMax, "");
            }
        }

        /// <summary>
        /// 解像度横
        /// </summary>
        public double ResolutionHorz { get; set; }
        /// <summary>
        /// 解像度縦
        /// </summary>
        public double ResolutionVert { get; set; }

        /// <summary>
        /// セーブ及びロード時の最終パス
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// セーブ及びロード時のセクション
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sSection"></param>
        /// <returns></returns>
        public bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();
            Path = sPath;
            Section = sSection;
            ResolutionHorz = ifa.GetIniDouble(sSection, "ResolutionHorz", sPath, ResolutionHorz);
            ResolutionVert = ifa.GetIniDouble(sSection, "ResolutionVert", sPath, ResolutionVert);

            return true;
        }

        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sSection"></param>
        /// <returns></returns>
        public bool Save(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();
            Path = sPath;
            Section = sSection;
            ifa.SetIniString(sSection, "ResolutionHorz", ResolutionHorz.ToString(), sPath);
            ifa.SetIniString(sSection, "ResolutionVert", ResolutionVert.ToString(), sPath);
            return true;
        }

#endif

        public clsConnectImage _clsConnectImage = null;

        public bool ContinuousGrabbingMode { get; set; }
        private int _iBufferCount = 3;

        /// <summary>前回GrabのDateTime</summary>
        protected DateTime _GrabLastUpdate;//v1328
        /// <summary>前回GrabのDateTime</summary>
        public DateTime GrabLastUpDate//v1328
        {
            get
            {
                return _GrabLastUpdate;
            }
            set
            {
                _GrabLastUpdate = value;
            }
        }

        public int BufferCount
        {
            get { return _iBufferCount; }
            set
            {
                if (value < 0)
                    return;
                _iBufferCount = value;
            }
        }

        protected bool bInternalError = false;
        public bool IsCameraLink { get; protected set; }
        public bool IsGigE { get; protected set; }
        public bool IsAreaSensor { get; protected set; }
        public bool IsLineSensor { get; protected set; }
        public bool IsUseEncoder { get; protected set; }
        public bool IsOffLine { get; protected set; }

        /// <summary>
        /// 縦方向Image連結
        /// </summary>
        public bool IsConnectVerticalImage { get; protected set; }

        /// <summary>
        /// カメラに異常が発生した場合に呼び出される
        /// </summary>
        public bool IsError { get; set; }
        public string ErrorReason { get; set; }

        protected void setError(bool bError, string sReason = "")
        {
            IsError = bError;
            ErrorReason = sReason;
        }

        public static void CloseAllFrameGrabber()
        {
            try
            {
                HOperatorSet.CloseAllFramegrabbers();
            }
            catch (HOperatorException oe)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} - {1}", oe.Message, MethodBase.GetCurrentMethod().ToString()));
            }
        }

        protected TriggerMode _hardTrigger = TriggerMode.CC1;
        public TriggerMode HardTrigger
        {
            get
            {
                return _hardTrigger;
            }
            set
            {
                if (!IsHardTrigger(value))
                    throw new ArgumentOutOfRangeException();
                _hardTrigger = value;
            }
        }

        public bool EnableHardTrigger { get; set; }
        public bool EnableSoftTrigger { get; set; }

        /// <summary>
        /// トリガーモード
        /// </summary>
        protected TriggerMode _tmNowTriggerMode = TriggerMode.Unknown;
        /// <summary>
        /// キャプチャハンドル
        /// </summary>
        protected HTuple _htAcqHandle;
        internal HTuple AcqHandle
        {
            get
            {
                return _htAcqHandle;
            }
        }
        /// <summary>
        /// 最大ディレイ
        /// </summary>
        protected HTuple _htMaxDelay = new HTuple(-1);
        /// <summary>
        /// カラーモード
        /// </summary>
        protected ColorMode _cmColor = ColorMode.Unknown;
        public ColorMode Color
        {
            get
            {
                return _cmColor;
            }
        }

        public bool IsColor
        {
            get
            {
                return (_cmColor == ColorMode.RGB || _cmColor == ColorMode.YUV);
            }
        }


        /// <summary>
        /// カメラの番号
        /// </summary>
        int _iIndex;
        public int Index
        {
            get
            {
                return _iIndex;
            }
        }

        string _sMirrorType;
        public string Mirror
        {
            get { return _sMirrorType; }
        }

        /// <summary>
        /// カメラの名前
        /// </summary>
        string _sCamName;
        public string Name
        {
            get
            {
                return _sCamName;
            }
        }

        /// <summary>
        /// カメラの説明
        /// </summary>
        string _sDescription;
        public string Description
        {
            get
            {
                return _sDescription;
            }
        }

        protected Dictionary<string, string> _dicDeviceInfo = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        public string GetPropertyValue(string PropName)
        {
            if (_dicDeviceInfo.Keys.Contains(PropName))
                return _dicDeviceInfo[PropName];
            return "";
        }

        public Dictionary<string, HTuple> DicOpenParams { get { return _dicOpenParams; } }
        /// <summary>
        /// Openを実行するときのパラメーター
        /// </summary>
        protected Dictionary<string, HTuple> _dicOpenParams = new Dictionary<string, HTuple>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>
        /// Open時のパラメーターをセットする
        /// </summary>
        /// <param name="sKey">キー名</param>
        /// <param name="value">値</param>
        public void SetOpenParam(string sKey, HTuple value)
        {
            _dicOpenParams.Add(sKey, value);
        }

        public void SetOpenParam(Dictionary<string, HTuple> param)
        {
            foreach (KeyValuePair<string, HTuple> k in param)
            {
                _dicOpenParams.Add(k.Key, k.Value);
            }
        }

        /// <summary>
        /// トリガがハードかどうか
        /// </summary>
        /// <param name="e">検査するトリガモード</param>
        /// <returns>true ハード false それ以外</returns>
        public static bool IsHardTrigger(TriggerMode e)
        {
            return (e == TriggerMode.CC1
                || e == TriggerMode.CC2
                || e == TriggerMode.CC3
                || e == TriggerMode.CC4
                || e == TriggerMode.Line1
                || e == TriggerMode.Line2
                || e == TriggerMode.Line3);
        }

        /// <summary>
        /// トリガがハードかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsHardTrigger()
        {
            return HalconCameraBase.IsHardTrigger(_tmNowTriggerMode);
        }

        public bool IsFreeRun()
        {
            return (_tmNowTriggerMode == TriggerMode.FreeRun);
        }
        /// <summary>
        /// 現在デバイスが開いているかどうか
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return (_htAcqHandle != null);
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">カメラの名前</param>
        /// <param name="description">カメラの説明</param>
        public HalconCameraBase(int iIndex, string name, string description, string mirrorType)
        {
            _iIndex = iIndex;
            _sCamName = name;
            _sDescription = description;
            _sMirrorType = mirrorType;
        }

        /// <summary>
        /// カメラデバイスをクロースする
        /// </summary>
        /// <returns> true 正常終了 false 異常終了</returns>
        public virtual bool Close()
        {
            if (!IsOpen)
                return true;
            try
            {
                //トリガモードをソフトに戻す
                if (_tmNowTriggerMode != TriggerMode.Software)
                {
                    SetTriggerMode(TriggerMode.Software);
                }

                if (!bInternalError)
                {
                    HOperatorSet.CloseFramegrabber(_htAcqHandle);
                }

                if (IsConnectVerticalImage && _clsConnectImage != null)
                    _clsConnectImage.Dispose();
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            finally
            {
                _htAcqHandle = null;
            }
            return true;
        }
        /// <summary>
        /// オープンのベース処理
        /// </summary>
        /// <returns></returns>
        public virtual bool Open()
        {
            if (IsOpen)
            {
                // スタート前にしておくことがある場合こちらが呼ばれる
                prevGrabStartAction();

                //タイムアウトレンジを取得する
                if (_enableTimeout)
                {
                    GetGrabTimeoutRange(ref _timeoutMin, ref _timeoutMax, ref _timeoutStep, ref _timeoutNow);
                }

                //ゲインレンジを取得する
                if (_enableGain)
                {
                    if (!GetGainRange(ref _gainMin, ref _gainMax, ref _gainStep, ref _gainNow))
                    {
                        _enableGain = false;
                    }
                    if (_gainStep == 0)
                        _gainStep = 1;
                }

                if (_enableExposureTime)
                {
                    if (!GetExposureTimeRange(ref _exposureTimeMin, ref _exposureTimeMax, ref _exposureTimeStep, ref _exposureTimeNow))
                    {
                        _enableExposureTime = false;
                    }
                    if (_exposureTimeStep == 0)
                        _exposureTimeStep = 1;
                }

                if (_enableOffset)
                {
                    if (!GetOffsetRange(ref _offsetMin, ref _offsetMax, ref _offsetStep, ref _offsetNow))
                    {
                        _enableOffset = false;
                    }
                    if (_offsetStep == 0)
                        _offsetStep = 1;
                }

                if (_enableTriggerDelay)
                {
                    if (!GetTriggerDelayRange(ref _triggerDelayMin, ref _triggerDelayMax, ref _triggerDelayStep, ref _triggerDelayNow))
                    {
                        _enableTriggerDelay = false;
                    }

                    if (_triggerDelayStep == 0)
                        _triggerDelayStep = 1;
                }

                if (_enableLineRate)
                {
                    if (!GetLineRateRange(ref _lineRateMin, ref _lineRateMax, ref _lineRateStep, ref _lineRateNow))
                    {
                        _enableLineRate = false;
                    }

                    if (_lineRateStep == 0.0)
                        _lineRateStep = 1.0;
                }

                try
                {
                    if (this.Description.ToLower() != "filememory")
                    {
                        // イメージのサイズを取得する
                        HTuple value;
                        HOperatorSet.GetFramegrabberParam(_htAcqHandle, "image_width", out value);
                        _imageWidth = value.I;

                        if (_dicOpenParams.Keys.Contains("imgh"))
                        {
                            HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Height", _dicOpenParams["imgh"]);
                        }
                        HOperatorSet.GetFramegrabberParam(_htAcqHandle, "image_height", out value);
                        _imageHeight = value.I;

                        HOperatorSet.GetFramegrabberParam(_htAcqHandle, "color_space", out value);
                        switch (value[0].S.ToLower())
                        {
                            case "default": _cmColor = ColorMode.Gray; break;
                            case "gray": _cmColor = ColorMode.Gray; break;
                            case "rgb": _cmColor = ColorMode.RGB; break;
                            case "raw": _cmColor = ColorMode.RAW; break;
                            case "yuv": _cmColor = ColorMode.YUV; break;
                            case "multichannel": _cmColor = ColorMode.RGB; break;
                        }
                        HOperatorSet.GrabImageStart(_htAcqHandle, _htMaxDelay);
                    }

                    //連結イメージ数
                    HTuple htConnectCnt = _dicOpenParams.GetValueOrDefault("connectimagecount", new HTuple(0));
                    _imageConnectCnt = htConnectCnt.I;
                    if (_imageConnectCnt > 1)
                    {
                        IsConnectVerticalImage = true;
                        _clsConnectImage = new clsConnectImage(1, ImageWidth, ImageHeight);
                        _clsConnectImage.InitSetImageParameters((_cmColor == ColorMode.RGB) ? 3 : 1, _imageConnectCnt, new List<int>() { 0, 0, 0, 0 }, new List<int>() { 0, 0, 0, 0 });
                    }
                }
                catch (HOperatorException oe)
                {
                    bInternalError = true;
                    setError(true, oe.Message);
                    TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                    return false;
                }

                // トリガーモードをソフトウェアにしておく
                SetTriggerMode(TriggerMode.Software);
            }
            setError(false);
            return true;
        }

        #region ■ポーリング処理追加 v1328

        protected bool _bPollingThread;//v1328
        protected Thread _thPolling;//v1328

        /// <summary>ポーリング処理開始（public）</summary>//v1328
        public bool InitPolling()
        {
            _bPollingThread = false;
            _thPolling = null;
            return StartPollingThread();
        }

        /// <summary>ポーリング処理終了（public）</summary>//v1328
        public bool TerminatePolling()
        {
            return StopPollingThread();
        }

        protected bool StartPollingThread()//v1328
        {
            bool bRes = true;

            if (true == bRes)// 開いてるかチェック
                bRes &= IsOpen;

            if (true == bRes)// すでにスレッド起動中かチェック
                bRes &= (false == _bPollingThread);

            if (true == bRes)
            {
                _thPolling = new Thread(new ThreadStart(Polling));
                _thPolling.Name = "Cam"+ _iIndex + ".PollingMonitor";
                _thPolling.IsBackground = true;
                _bPollingThread = true;
                _thPolling.Start();
            }
            return bRes;
        }
        protected bool StopPollingThread()//v1328
        {
            if (_thPolling == null)// スレッドが起動していない
                return true;

            _bPollingThread = false;

            do
            {
                _thPolling.Join(100);//強制終了の場合：Abort、Whileフラグありの場合：Join(100)
            } while (_thPolling.IsAlive);

            _thPolling = null;

            return true;
        }
        protected void Polling()//v1328
        {
            int pollingInterval = 3000;
            int falseCounter = 0;

            //次回実行時間
            DateTime NextExcTime = DateTime.Now.AddMilliseconds(pollingInterval);

            //エラー発生済 v1330
            bool bIsErrorOccurrerd = false;

            while (_bPollingThread)
            {
                if(DateTime.Now >= NextExcTime)//現在時刻が次回実行時間より大きい場合のみ実行
                {
                    //実行時に次回実行時間を更新
                    NextExcTime = DateTime.Now.AddMilliseconds(pollingInterval);
                    if (false == CheckCommunication())
                        falseCounter++;
                    else
                        falseCounter = 0;

                    if (falseCounter >= 3 && bIsErrorOccurrerd == false)//3回連続エラーだった場合 v1330 bIsErrorOccurrerd追加
                    {
                        bIsErrorOccurrerd = true;//v1330
                        string msgStr = _thPolling.Name + "通信エラー発生";
                        System.Diagnostics.Debug.WriteLine(msgStr);
                        LogingDllWrap.LogingDll.Loging_SetLogString(msgStr);
                        setError(true, "通信エラー発生");
                        //エラーカウンタインクリ v1330
                        LineCameraSheetSystem.SystemCounter.GetInstance().CamCommunicationError++;

                        //this.Close();
                        //this.Open();

                        //v1330 コメントアウト
                        //falseCounter = 0;
                    }
                }
                //スレッド基本Sleep時間
                System.Threading.Thread.Sleep(50);
            }
        }

        protected virtual bool CheckCommunication()//v1328
        {
            return true;
        }

        #endregion

        public virtual bool TriggerModeOpen(TriggerMode trig)
        {
            return true;
        }

        public virtual bool StartGrab()
        {
            return true;
        }

        public virtual bool StopGrab()
        {
            return true;
        }

        public virtual bool ExecuteUsrSetLoad()
        {
            return true;
        }
        public virtual bool ExecuteUsrSetSave()
        {
            return true;
        }

        protected bool openParamFetch(string sValue, int iDefault, out int iValue)
        {
            int iTemp;
            iValue = iDefault;

            if (!_dicOpenParams.Keys.Contains(sValue))
            {
                return true;
            }

            try
            {
                if (int.TryParse(_dicOpenParams[sValue].I.ToString(), out iTemp))
                {
                    iValue = iTemp;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool openParamFetch(string sKey, string sDefault, out string sValue)
        {
            sValue = sDefault;

            if (!_dicOpenParams.Keys.Contains(sKey))
            {
                return true;
            }

            try
            {
                sValue = _dicOpenParams[sKey].S;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected virtual void prevGrabStartAction()
        {

        }

        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// 画像のイメージサイズ幅
        /// </summary>
        protected int _imageWidth = -1;
        public int ImageWidth
        {
            get
            {
                return _imageWidth;
            }
        }

        /// <summary>
        /// 画像のイメージサイズ高さ
        /// </summary>
        protected int _imageHeight = -1;
        public int ImageHeight
        {
            get
            {
                return _imageHeight;
            }
        }

        protected int _imageConnectCnt = 0;
        public int ImageConnectCnt
        {
            get
            {
                return _imageConnectCnt;
            }
        }

        protected int _imageOffsetX = 0;
        public int ImageOffsetX
        {
            get
            {
                return _imageOffsetX;
            }
        }
        protected int _imageOffsetY = 0;
        public int ImageOffsetY
        {
            get
            {
                return _imageOffsetY;
            }
        }

        public bool GrabSync(out HObject img, int timeout)
        {
            img = null;
            if (IsOpen && !IsHardTrigger() && !IsLive)
            {
                SetGrabTimeout(timeout);
                try
                {
                    if (!executeTrigger())
                    {
                        throw new Exception("ソフトトリガに失敗");
                    }
                    getImage(out img);
                    //                    HOperatorSet.GrabImageAsync(out img, _htAcqHandle, _htMaxDelay);
                }
                catch (HOperatorException oe)
                {
                    bInternalError = true;
                    TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                    return false;
                }
                catch (Exception e)
                {
                    TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                    return false;
                }
            }
            return true;
        }

        private bool _bGrabAsync = false;

        /// <summary>
        /// 非同期取り込み実行
        /// </summary>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns>true 正常終了 false 異常終了</returns>
        public bool GrabAsync(int timeout)
        {
            if (!IsOpen || IsHardTrigger() || IsLive)
                return false;

            // ダミー取り込み
            if (_bGrabAsync)
            {
                HObject img;
                WaitGrabAsync(out img);
            }

            try
            {
                SetGrabTimeout(timeout);
                if (!executeTrigger())
                {
                    throw new Exception("ソフトトリガに失敗");
                }
                _bGrabAsync = true;
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 非同期取り込みを待つ
        /// </summary>
        /// <param name="img">撮像されたイメージ</param>
        /// <returns>true 正常終了 false 異常終了</returns>
        public bool WaitGrabAsync(out HObject img)
        {
            img = null;
            if (!IsOpen)
                return false;
            if (IsHardTrigger() || IsLive)
                return false;
            if (!_bGrabAsync)
                return false;

            _bGrabAsync = false;
            try
            {
                getImage(out img);
                //                HOperatorSet.GrabImageAsync(out img, _htAcqHandle, _htMaxDelay);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return true;
            }
            return true;
        }

        public virtual bool SetWhiteBImageWidth(int width)
        {
            return true;
        }
        public virtual bool SetWhiteBImageHeight(int height)
        {
            return true;
        }
        public virtual bool SetWhiteBImageOffsetX(int offsetx)
        {
            return true;
        }
        public virtual bool SetWhiteBImageOffsetY(int offsety)
        {
            return true;
        }
        public virtual bool UserSetSave()
        {
            return true;
        }

        #region WhiteBalanceColor
        public virtual bool WhiteBalanceColor()
        {
            return false;
        }
        public virtual bool IsWhiteBalanceStop()
        {
            return true;
        }
        public virtual bool WhiteBalanceReset()
        {
            return true;
        }
        #endregion

        protected bool _enableGain = true;
        protected double _gainMin = -1;
        protected double _gainMax = -1;
        protected double _gainStep = -1;
        protected double _gainNow = -1;
        public double GetDefaultGain()
        {
            return _gainNow;
        }
        public bool EnableGain
        {
            get
            {
                return _enableGain;
            }
        }
        public double GainMin
        {
            get
            {
                return _gainMin;
            }
        }
        public double GainMax
        {
            get
            {
                return _gainMax;
            }
        }
        public double GainStep
        {
            get
            {
                return _gainStep;
            }
        }
        public double Gain
        {
            get
            {
                return GetGain();
            }
            set
            {
                SetGain(value);
            }
        }
        public virtual bool GetGainRange(ref double min, ref double max, ref double step, ref double now)
        {
            return false;
        }

        public virtual bool SetGain(double value)
        {
            return false;
        }

        public virtual double GetGain()
        {
            return -1;
        }

        protected bool _enableOffset;
        protected int _offsetMin = -1;
        protected int _offsetMax = -1;
        protected int _offsetStep = -1;
        protected int _offsetNow = -1;
        public bool EnableOffset
        {
            get
            {
                return _enableOffset;
            }
        }
        public int OffsetMin
        {
            get
            {
                return _offsetMin;
            }
        }
        public int OffsetMax
        {
            get
            {
                return _offsetMax;
            }
        }

        public int OffsetStep
        {
            get
            {
                return _offsetStep;
            }
        }
        public int Offset
        {
            get
            {
                return GetOffset();
            }
            set
            {
                SetOffset(value);
            }
        }
        public virtual bool GetOffsetRange(ref int min, ref int max, ref int step, ref int now)
        {
            return false;
        }
        public virtual bool SetOffset(int value)
        {
            return false;
        }
        public virtual int GetOffset()
        {
            return -1;
        }

        protected bool _enableExposureTime = false;
        protected int _exposureTimeMin = -1;
        protected int _exposureTimeMax = -1;
        protected int _exposureTimeStep = -1;
        protected int _exposureTimeNow = -1;

        public bool EnableExposureTime
        {
            get
            {
                return _enableExposureTime;
            }
        }

        public int ExposureTimeMin
        {
            get
            {
                return _exposureTimeMin;
            }
        }
        public int ExposureTimeMax
        {
            get
            {
                return _exposureTimeMax;
            }
            set
            {
                _exposureTimeMax = value;
            }
        }
        public int ExposureTimeStep
        {
            get
            {
                return _exposureTimeStep;
            }
        }
        public int ExposureTime
        {
            get
            {
                return GetExposureTime();
            }
            set
            {
                SetExposureTime(value);
            }
        }

        public virtual int GetExposureTime()
        {
            return -1;
        }
        public virtual bool SetExposureTime(int value)
        {
            return false;
        }
        public virtual bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            return false;
        }

        protected bool _enableTriggerDelay = false;
        protected double _triggerDelayMin = 0.0;
        protected double _triggerDelayMax = 0.0;
        protected double _triggerDelayStep = 0.0;
        protected double _triggerDelayNow = 0.0;

        public bool EnableTriggerDelay
        {
            get
            {
                return _enableTriggerDelay;
            }
        }

        public double TriggerDelayMin
        {
            get
            {
                return _triggerDelayMin;
            }
        }

        public double TriggerDelayMax
        {
            get
            {
                return _triggerDelayMax;
            }
        }

        public double TriggerDelayStep
        {
            get
            {
                return _triggerDelayStep;
            }
        }

        public double TriggerDelay
        {
            get
            {
                return GetTriggerDelay();
            }

            set
            {
                SetTriggerDelay(value);
            }
        }

        public virtual double GetTriggerDelay()
        {
            return 0.0;
        }

        public virtual bool SetTriggerDelay(double value)
        {
            return false;
        }

        public virtual bool GetTriggerDelayRange(ref double min, ref double max, ref double step, ref double now)
        {
            return false;
        }



        protected bool _enableTimeout;
        protected int _timeoutMin = -1;
        protected int _timeoutMax = -1;
        protected int _timeoutStep = -1;
        protected int _timeoutNow = -1;
        public int TimeoutMin
        {
            get
            {
                return _timeoutMin;
            }
        }

        public int TimeoutMax
        {
            get
            {
                return _timeoutMax;
            }
        }
        public int TimeoutStep
        {
            get
            {
                return _timeoutStep;
            }
        }
        public int TimeoutNow
        {
            get
            {
                return _timeoutNow;
            }
        }

        public virtual bool GetGrabTimeoutRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableTimeout)
                return false;

            try
            {
                HTuple htGrabTimeoutRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, new HTuple("grab_timeout"), out htGrabTimeoutRange);

                min = 0;
                max = 10000;
                step = 1;
                now = htGrabTimeoutRange[0].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public bool IsLive
        {
            get
            {
                return (GetTriggerMode() == TriggerMode.FreeRun);
            }
        }

        public virtual double GetResultingFrameRate()
        {
            return 0.0;
        }

        public virtual double GetResultingLineRate()
        {
            return 0.0;
        }

        protected bool _enableLineRate = false;
        protected double _lineRateMin = -1.0;
        protected double _lineRateMax = -1.0;
        protected double _lineRateStep = -1.0;
        protected double _lineRateNow = -1.0;

        public bool EnableLineRate
        {
            get { return _enableLineRate; }
        }

        public double LineRateMin
        {
            get { return _lineRateMin; }
        }

        public double LineRateMax
        {
            get { return _lineRateMax; }
            set { _lineRateMax = value; }
        }

        public double LineRateStep
        {
            get { return _lineRateStep; }
        }

        public double LineRate
        {
            get { return GetLineRate(); }

            set
            {
                SetLineRate(value);
            }
        }

        public virtual double GetLineRate()
        {
            return 0.0;
        }

        public virtual bool SetLineRate(double value)
        {
            return false;
        }

        public virtual bool GetLineRateRange(ref double dMin, ref double dMax, ref double dStep, ref double dNow)
        {
            return false;
        }


        TriggerMode _tmOldTriggerMode = TriggerMode.Software;
        public bool Live(bool bLive, bool bBaseCaptureThread = true)
        {
            if (!IsOpen)
                return false;

            if (IsLive)
            {
                if (!bLive)
                {
                    SetTriggerMode(_tmOldTriggerMode);
                }
            }
            else
            {
                _tmOldTriggerMode = GetTriggerMode();
                if (bLive)
                {
                    SetTriggerMode(TriggerMode.FreeRun, bBaseCaptureThread);
                }
            }

            return true;
        }

        public virtual bool SetHardTrigger(bool trig, bool grabImageStart)
        {
            if (!IsOpen)
                return false;
            //            if (IsHardTrigger())
            //                return true;
            if (trig)
            {
                SetTriggerMode(_hardTrigger);
                if (grabImageStart == true)
                    SetTriggerMode_Hard_GrabImageStart();
            }
            else
            {
                SetTriggerMode(TriggerMode.Software);
            }
            return true;
        }

        public virtual bool SetGrabTimeout(int now)
        {
            if (!IsOpen)
                return false;

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, new HTuple("grab_timeout"), now);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public virtual int GetGrabTimeout()
        {
            if (!IsOpen)
                return -1;

            try
            {
                HTuple htValue;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, new HTuple("grab_timeout"), out htValue);

                return htValue[0].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }

        protected int _threadSleepTime = 1;
        public int ThreadSleepTime
        {
            get
            {
                return _threadSleepTime;
            }
            set
            {
                if (value < 1)
                    return;
                _threadSleepTime = value;
            }
        }

        /// <summary>
        /// トリガーモード取得
        /// </summary>
        /// <returns>トリガーモード値</returns>
        public virtual TriggerMode GetTriggerMode()
        {
            if (!IsOpen)
                return TriggerMode.Unknown;

            return _tmNowTriggerMode;
        }
        /// <summary>
        /// トリガーモードを設定する
        /// </summary>
        /// <param name="trig">設定するトリガーモード</param>
        /// <returns>true 正常終了 false 異常終了</returns>
        public virtual bool SetTriggerMode(TriggerMode trig, bool bCaptureThread = true)
        {
            return false;
        }
        public virtual bool SetTriggerMode_Hard_GrabImageStart()
        {
            return false;
        }

        public virtual bool SetLineInputMode(LineInput eLineInput)
        {
            return false;
        }

        public virtual bool SetCameraStatus(string sStat)
        {
            return false;
        }

        public virtual bool SetCameraStatus(string sStat, params int[] p)
        {
            return false;
        }

        public virtual bool SetCameraStatus(string sStat, params string[] p)
        {
            return false;
        }

        public virtual bool GetCameraStatus(string sStat, out int[] p)
        {
            p = null;
            return false;
        }

        public virtual bool GetCameraStatus(string sStat, out string[] p)
        {
            p = null;
            return false;
        }

        public virtual bool SetEncoderDivision(int iDivision)
        {
            return false;
        }

        public virtual bool GetEncoderDivision(out int iDivision)
        {
            iDivision = 1;
            return false;
        }

        /// <summary>
        /// ソフトウェアトリガを実行する
        /// </summary>
        /// <returns>true 正常終了 false 異常終了</returns>
        protected virtual bool executeTrigger()
        {
            if (_tmNowTriggerMode != TriggerMode.Software)
                return false;

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSoftware", "Execute");
            }
            catch (HOperatorException oe)
            {
                setError(true, oe.Message);
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーメッセージトレイサー
        /// </summary>
        /// <param name="sMessage">エラーメッセージ</param>
        /// <param name="sMethod">エラーが発生したメソッド</param>
        protected void TraceError(string sMessage, string sMethod)
        {
            LogingDll.Loging_SetLogString(string.Format("{0} - {1}", sMessage, sMethod));
            //            System.Diagnostics.Trace.WriteLine( string.Format( "{0} - {1}", sMessage, sMethod));

        }

        protected bool _stop = false;
        protected Thread _captureThread = null;
        /// <summary>
        /// 非同期通信を開始する
        /// </summary>
        /// <returns></returns>
        protected virtual bool beginCaptureThread()
        {
            if (_captureThread != null)
                return false;

            try
            {
                if (ContinuousGrabbingMode)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "continuous_grab_timeout", 0x7FFFFFFF);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "num_buffers", _iBufferCount);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "continuous_grabbing", "enable");
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
            }

            _stop = false;
            _captureThread = new Thread(captureThread);
            _captureThread.Name = string.Format("{0}-ｷｬﾌﾟﾁｬｽﾚｯﾄﾞ", Name);
            _captureThread.Start();

            return true;
        }

        /// <summary>
        /// 非同期通信を終了する
        /// </summary>
        /// <returns>true 正常終了　false 異常終了</returns>
        protected virtual bool endCaptureThread()
        {
            if (_captureThread == null)
                return false;
            _stop = true;
            try
            {
                // 取り込みをアボートする
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "true");

                HTuple htParam;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "continuous_grabbing", out htParam);
                if (htParam == "enable")
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "continuous_grabbing", "disable");
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
            }

            do
            {
                _captureThread.Join(100);
            }
            while (_captureThread.ThreadState == ThreadState.Running);
            _captureThread = null;

            return true;
        }

        public virtual bool getImage(out HObject img)
        {
            img = null;
            try
            {
                HOperatorSet.GrabImageAsync(out img, _htAcqHandle, _htMaxDelay);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 非同期取り込みのキャプチャスレッド
        /// </summary>
        private void captureThread()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            while (!_stop)
            {
                sw.Restart();
                try
                {
                    HObject img;
                    getImage(out img);
                    if (img != null)
                    {
                        if (OnGrabbedImage != null && !_stop)
                        {
                            OnGrabbedImage(this, new GrabbedImageEventArgs(img, img, DateTime.Now, Index));
                        }
                        img.Dispose();
                        img = null;
                    }
                }
                catch (HOperatorException oe)
                {
                    TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                }
                sw.Stop();
                int iSleepTime = ThreadSleepTime - (int)sw.ElapsedMilliseconds;
                if (iSleepTime < 1)
                    iSleepTime = 1;
                Thread.Sleep(iSleepTime);
            }
        }

        public void ClearOnGrabbedImage()
        {
            OnGrabbedImage = null;
        }
    }

    public class HalconCameraDummy : HalconCameraBase
    {
        public HalconCameraDummy(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            _enableExposureTime = false;
            _enableGain = false;
            _enableLineRate = false;
            _enableOffset = false;
            _enableTimeout = false;
            _enableTriggerDelay = false;

            IsAreaSensor = true;
            IsLineSensor = false;
            IsGigE = false;
        }

        public override bool Open()
        {
            _htAcqHandle = new HTuple(0);
            return true;
        }

        public override bool Close()
        {
            if (!IsOpen)
                return false;

            _htAcqHandle = null;
            return true;
        }
    }

    public static class DictionaryExt
    {
        public static HTuple GetValueOrDefault(this Dictionary<string, HTuple> dic, string s, HTuple def)
        {
            return dic.ContainsKey(s) ? dic[s] : def;
        }
    }
}
