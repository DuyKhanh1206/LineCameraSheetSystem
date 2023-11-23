using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using HalconDotNet;

namespace HalconCamera
{
    public class HalconCameraLinX : HalconCameraBase
    {
        public HalconCameraLinX(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            NewLine = "\r";
            _enableTimeout = true;
        }

        public override bool Open()
        {
            try
            {
                // デバイス情報を設定する
                HTuple htHorizontalResolution = _dicOpenParams.GetValueOrDefault("horizontalresolution", new HTuple(1));
                HTuple htVerticalResolution = _dicOpenParams.GetValueOrDefault("verticalresolution", new HTuple(1));
                HTuple htImageWidth = _dicOpenParams.GetValueOrDefault("imagewidth", new HTuple(0));
                HTuple htImageHeight = _dicOpenParams.GetValueOrDefault("imageheight", new HTuple(0));
                HTuple htStartRow = _dicOpenParams.GetValueOrDefault("startrow", new HTuple(0));
                HTuple htStartColumn = _dicOpenParams.GetValueOrDefault("startcolumn", new HTuple(0));
                HTuple htField = _dicOpenParams.GetValueOrDefault("field", new HTuple("default"));
                HTuple htBitPerChannel = _dicOpenParams.GetValueOrDefault("bitperchannel", new HTuple(-1));
                HTuple htColorSpace = _dicOpenParams.GetValueOrDefault("colorspace", new HTuple("gray"));
                HTuple htGeneric = _dicOpenParams.GetValueOrDefault("generic", new HTuple(0));
                HTuple htExternalTrigger = _dicOpenParams.GetValueOrDefault("externaltrigger", new HTuple("false"));
                HTuple htCameraType = _dicOpenParams.GetValueOrDefault("cameratype", new HTuple("default"));
                HTuple htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(0));

                HOperatorSet.OpenFramegrabber("LinX",
                    htHorizontalResolution,
                    htVerticalResolution,
                    htImageWidth,
                    htImageHeight,
                    htStartRow,
                    htStartColumn,
                    htField,
                    htBitPerChannel,
                    htColorSpace,
                    htGeneric,
                    htExternalTrigger,
                    htCameraType,
                    htDevice,
                    htPort,
                    htLineIn,
                    out _htAcqHandle);

            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return base.Open();
        }

        protected override bool executeTrigger()
        {
            if (_tmNowTriggerMode != TriggerMode.Software)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_force_trigger", 1);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        protected override bool endCaptureThread()
        {
            if (_captureThread == null)
                return false;
            _stop = true;
            try
            {
                //                if (_tmNowTriggerMode == TriggerMode.Hardware)
                //                {
                // 取り込みをアボートする
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_capture", "true");
                //                }
            }
            catch (HOperatorException)
            {
            }

            do
            {
                _captureThread.Join(100);
            }
            while (_captureThread.ThreadState == System.Threading.ThreadState.Running);
            _captureThread = null;
            return true;
        }

        public override int GetGrabTimeout()
        {
            if (!IsOpen)
                return -1;

            if (!_enableTimeout)
                return -1;

            try
            {
                HTuple htGrabTimeout;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "grab_timeout", out htGrabTimeout);
                return htGrabTimeout.I;
            }
            catch (HOperatorException)
            {
                return -1;
            }
        }

        public override bool SetGrabTimeout(int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableTimeout)
                return false;

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "grab_timeout", now);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override bool GetGrabTimeoutRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableTimeout)
                return false;

            try
            {
                HTuple htGrabTimeout;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "grab_timeout", out htGrabTimeout);
                min = 0;
                max = 10000;
                step = 1;
                now = htGrabTimeout.I;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public override bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;

            try
            {
                min = 10;
                max = 1000000;
                step = 1;
                now = GetExposureTime();
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// コマンドラインのデリミタ
        /// </summary>
        public string NewLine { get; set; }
        protected bool _bCommKeepOpen = true;
        public bool CommKeepOpen
        {
            get
            {
                return _bCommKeepOpen;
            }
            set
            {
                if (!value && CommOpened)
                {
                    CommClose();
                }
                _bCommKeepOpen = value;
            }
        }
        public bool CommOpened { get; set; }

        public virtual bool CommOpen()
        {
            if (CommOpened)
                return true;

            try
            {
                HTuple htNewLine = NewLine;
                HTuple htOrds = htNewLine.TupleOrds();
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_comm_open", new HTuple());
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "serial_terminate_code", htOrds);
                CommOpened = true;
            }
            catch (HOperatorException e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            return true;
        }

        public virtual bool CommClose()
        {
            if (!CommOpened)
                return true;

            CommOpened = false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_comm_close", new HTuple());
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public virtual bool SetCommTimeout(int iTimeoutMS)
        {
            if (!IsOpen)
                return false;

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "serial_timeout", iTimeoutMS);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public virtual bool GetCommTimeout(ref int iTimeoutMS)
        {
            if (!IsOpen)
                return false;

            HTuple htTimeout;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "serial_timout", out htTimeout);
                iTimeoutMS = htTimeout.I;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        protected virtual bool sendCommandAndreadResponse(string sCmd, ref string sRet, int iTimeoutMS = -1)
        {
            if (!IsOpen)
                return false;

            if (sCmd.Length < NewLine.Length)
                return false;

            if (sCmd.Substring(sCmd.Length - NewLine.Length, NewLine.Length) != NewLine)
                sCmd += NewLine;

            try
            {
                if (!CommOpened)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_comm_open", new HTuple());
                }
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "serial_message", new HTuple(sCmd));

                // コマンド応答待ち
                System.Threading.Thread.Sleep(500);

                try
                {
                    do
                    {
                        HTuple htReceive;
                        HOperatorSet.GetFramegrabberParam(_htAcqHandle, "serial_message", out htReceive);
                        sRet += htReceive.S + NewLine;
                    } while (true);
                }
                catch (HOperatorException)
                {
                    // タイムアウトで戻ってきても何もしない
                }

                if (!CommKeepOpen)
                {
                    CommClose();
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }
    }

    public class HalconCameraLinX_NED : HalconCameraLinX
    {
        public HalconCameraLinX_NED(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
        }
    }

    public class HalconCameraLinX_NEDLineCamera : HalconCameraLinX_NED
    {
        public HalconCameraLinX_NEDLineCamera(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            IsAreaSensor = false;
            IsLineSensor = true;
            _enableExposureTime = false;
        }
    }

    public class HalconCameraLinX_NEDLineCameraXCM4040SAT2 : HalconCameraLinX_NEDLineCamera
    {
        const string TRIG_CAMFILE = "NED-XCM4040SAT2-E2-OneShot-TTL.gdg";
        const string FREERUN_CAMFILE = "NED-XCM4040SAT2-E2-FreeRun.gdg";

        const string ENC_TRIG_CAMFILE = "NED-XCM4040SAT2-E2-ENC-TTL-OneShot-TTL.gdg";
        const string ENC_FREERUN_CAMFILE = "NED-XCM4040SAT2-E2-ENC-TTL.gdg";

        string _sTrigCamFile = TRIG_CAMFILE;
        string _sFreerunCamFile = FREERUN_CAMFILE;
        string _sEncTrigCamFile = ENC_TRIG_CAMFILE;
        string _sEncFreerunCamFile = ENC_FREERUN_CAMFILE;

        string _sTargetTrigCamFile = "";
        string _sTargetFreerunCamFile = "";

        bool _bFreeRunTrig = false;

        const int MIN_PROGRAMABLE_EXPOSURE_VAL1 = 0;
        const int MAX_PROGRAMABLE_EXPOSURE_VAL1 = 11;
        const int MIN_PROGRAMABLE_EXPOSURE_VAL2 = 61;
        const int MAX_PROGRAMABLE_EXPOSURE_VAL2 = 1023;


        public HalconCameraLinX_NEDLineCameraXCM4040SAT2(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            // ターミネートコードはEOT(0x4)
            NewLine = new string(new char[] { (char)0x4 });

            _gainMin = 0;
            _gainMax = 20;
            _gainStep = 1;

            _offsetMin = -15;
            _offsetMax = 15;
            _offsetStep = 1;

            _exposureTimeMin = MIN_PROGRAMABLE_EXPOSURE_VAL2;
            _exposureTimeMax = MAX_PROGRAMABLE_EXPOSURE_VAL2;
            _exposureTimeStep = 1;

        }

        public bool EnableGainDigital { get; set; }
        // デジタルゲイン
        int _gainDigtalMin = 0;
        int _gainDigtalMax = 511;
        int _gainDigtalStep = 1;
        int _gainDigtalNow;

        // プログラマブル露光時間Divider
        int _exposureDivMin = MIN_PROGRAMABLE_EXPOSURE_VAL1;
        int _exposureDivMax = MAX_PROGRAMABLE_EXPOSURE_VAL1;
        int _exposureDivStep = 1;
        int _exposureDivNow;

        // 露光時間-読出時間
        public bool EnableExposureReadTime { get; set; }
        int _expReadTimeMin = 0;
        int _expReadTimeMax = 50;
        int _expReadTimeStep = 1;
        int _expReadTimeNow;

        protected override bool sendCommandAndreadResponse(string sCmd, ref string sRet, int iTimeout = -1)
        {
            if (!IsOpen)
                return false;

            // コマンド終端文字が入っていない場合付加
            if (sCmd[sCmd.Length - 1] != '\r')
                sCmd += '\r';

            try
            {
                if (!CommOpened)
                {
                    CommOpen();
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "serial_message", new HTuple(sCmd));

                int iTimeoutOld = 1000;
                if (iTimeout != -1)
                {
                    GetCommTimeout(ref iTimeoutOld);
                    SetCommTimeout(iTimeout);
                }

                // コマンド応答待ち
                System.Threading.Thread.Sleep(500);

                HTuple htReceive;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "serial_message", out htReceive);
                sRet = htReceive.S;

                if (iTimeout != -1)
                {
                    SetCommTimeout(iTimeoutOld);
                }

                if (!CommKeepOpen)
                {
                    CommClose();
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        protected enum EReturnCode
        {
            OK,
            Error,
            ErrorCommand,
            ErrorOverflow,
            ErrorValue,
            ErrorMemory,
            InvalidArgument,
        }


        protected EReturnCode analyseResponse(string sRes, bool bErrorSpec = false)
        {

            if (sRes == null || sRes == "")
                return EReturnCode.InvalidArgument;

            string[] sSplit = sRes.Split(new char[] { '\r' });
            foreach (string s in sSplit)
            {
                if (s.Length < 3)
                    continue;

                if (s.IndexOf("OK") != -1)
                {
                    return EReturnCode.OK;
                }
                else if (s.IndexOf("ERR") != -1)
                {
                    if (bErrorSpec)
                    {
                        if (s.IndexOf("OVR") != -1)
                            return EReturnCode.ErrorOverflow;
                        else if (s.IndexOf("VAL") != -1)
                            return EReturnCode.ErrorValue;
                        else if (s.IndexOf("MEM") != -1)
                            return EReturnCode.ErrorValue;
                        else
                            return EReturnCode.ErrorCommand;
                    }
                    else
                    {
                        return EReturnCode.Error;
                    }
                }
            }
            return EReturnCode.InvalidArgument;
        }

        protected bool analyseStatusCommandResponse(string sRes, out Dictionary<string, string> data)
        {
            data = null;

            if (sRes == null || sRes == "")
                return false;

            if (analyseResponse(sRes) != EReturnCode.OK)
                return false;

            data = new Dictionary<string, string>();
            string[] sSplit = sRes.Split(new char[] { '\r' });

            foreach (string s in sSplit)
            {
                // 応答コード,最後のスペースをとる
                string sTrim = s.Trim(new char[] { '>', ' ' });

                // いらないものを削除
                switch (sTrim)
                {
                    case "OK":
                    case "sta":
                    case "":
                        continue;
                }

                if (sTrim.IndexOf('=') != -1)
                {
                    string[] sSp2 = sTrim.Split(new char[] { '=' });
                    data.Add(sSp2[0].Trim(), sSp2[1].Trim());
                }
                else
                {
                    string[] sSp2 = sTrim.Split(new char[] { ' ' });
                    data.Add(sSp2[0].Trim(), sSp2[1].Trim());
                }
            }
            return true;
        }

        public bool RefreshStatus()
        {
            if (!IsOpen)
                return false;

            string sRet = "";
            sendCommandAndreadResponse("sta", ref sRet);

            Dictionary<string, string> dicStat;
            if (analyseStatusCommandResponse(sRet, out dicStat))
            {

                if (dicStat.ContainsKey("gax"))
                {
                    _enableGain = true;
                    _gainNow = int.Parse(dicStat["gax"]);
                }

                if (dicStat.ContainsKey("gdx"))
                {
                    _gainDigtalNow = int.Parse(dicStat["gdx"]);
                }

                if (dicStat.ContainsKey("odx"))
                {
                    _enableOffset = true;
                    _offsetNow = int.Parse(dicStat["odx"]);
                }

                if (dicStat.ContainsKey("int"))
                {
                    _enableExposureTime = true;

                    string[] sSpli = dicStat["int"].Split(new char[] { ',' });
                    _exposureTimeNow = int.Parse(sSpli[1].Trim());
                    _exposureDivNow = int.Parse(sSpli[0].Trim());
                }

                if (dicStat.ContainsKey("pad"))
                {
                    _expReadTimeNow = int.Parse(dicStat["pad"]);
                }
            }

            return true;
        }

        public override bool Open()
        {
            // 標準コマンドがしようできないため、関数を使用する
            _enableExposureTime = false;
            _enableGain = false;
            _enableOffset = false;
            _enableTriggerDelay = false;

            openParamFetch("FreerunCamFile", FREERUN_CAMFILE, out _sFreerunCamFile);
            openParamFetch("TrigCamFile", TRIG_CAMFILE, out _sTrigCamFile);
            openParamFetch("EncFreerunCamFile", ENC_FREERUN_CAMFILE, out _sEncFreerunCamFile);
            openParamFetch("EncTrigCamFile", ENC_TRIG_CAMFILE, out _sEncTrigCamFile);

            string sDevice;
            int iPort;
            openParamFetch("Device", "default", out sDevice);
            openParamFetch("Port", 0, out iPort);
            _htDevice = sDevice;
            _htPort = iPort;

            int iFreeRunTrig;
            openParamFetch("FreeRunTrig", 0, out iFreeRunTrig);
            _bFreeRunTrig = (iFreeRunTrig != 0) ? true : false;

            bool bResult = base.Open();

            //ステータスを読み出し値をセットする
            if (bResult)
            {
                RefreshStatus();
            }

            // 露光リストを生成する
            initExposureList();

            return bResult;
        }

        static bool _bInitExposureList = false;
        static void initExposureList()
        {
            if (!_bInitExposureList)
            {
                ListProExp = new List<ProgrammableExposure> { };
                genProgrammableExposure();
                _bInitExposureList = true;
            }
        }

        static List<ProgrammableExposure> ListProExp { get; set; }
        struct ProgrammableExposure
        {
            //露光時間(μs)
            public double dExpsure { get; set; }
            //カメラ用パラメータ VAl1　(0-11)
            public int iVal1 { get; set; }
            //カメラ用パラメータ VAl2　(61-1023)
            public int iVal2 { get; set; }
        }

        //プログラマブル露光値の全パターンの計算
        static private void genProgrammableExposure()
        {
            //Val1の最小値
            int iVal1Min = 0;
            int iVal1Max = 11;
            int iVal2Min = 61;
            int iVal2Max = 1023;

            for (int i = 0; (iVal1Max - iVal1Min + 1) > i; i++)
            {
                for (int j = 0; (iVal2Max - iVal2Min + 1) > j; j++)
                {
                    ProgrammableExposure ProgExpo = new ProgrammableExposure();
                    ProgExpo.dExpsure = (((iVal2Min + j) / (20000000 / (16 * Math.Pow(2, (iVal1Min + i))))) * 1000000);
                    ProgExpo.iVal1 = i;
                    ProgExpo.iVal2 = iVal2Min + j;
                    ListProExp.Add(ProgExpo);
                }
            }
            //露光値の小さい順に並び替え
            // ListProExp.Sort((X, Y) => { return (int)(X.dExpsure - Y.dExpsure); });    
            ListProExp.Sort((X, Y) => { return sortHikaku(X.dExpsure, Y.dExpsure); });
        }
        static private int sortHikaku(double X, double Y)
        {
            if (X > Y)
            {
                return 1;
            }
            else if (X < Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        //露光時間(μｓ)を入れてVAL1,VAL2を返す。
        static public void FindExposure(double dExpTme, out int iVal1, out int iVal2)
        {
            if (ListProExp == null)
            {
                iVal1 = 0;
                iVal2 = 61;
                return;
            }

            if (dExpTme < 48.8)
            {
                iVal1 = 0;
                iVal2 = 61;

                return;
            }
            else if (dExpTme > 167683.2)
            {
                iVal1 = 11;
                iVal2 = 1023;
                return;
            }

            int index = -1;
            index = ListProExp.FindIndex(x => x.dExpsure >= dExpTme);

            iVal1 = ListProExp[index].iVal1;
            iVal2 = ListProExp[index].iVal2;
        }

        public override bool Close()
        {
            if (CommKeepOpen && CommOpened)
            {
                CommClose();
            }
            return base.Close();
        }

        protected string makeCommand(string sCmd, params int[] p)
        {
            string sRet = sCmd + " ";
            if (p != null)
            {
                foreach (int i in p)
                {
                    sRet += " " + i.ToString();
                }
            }
            return sRet;
        }

        // 時間計算
        static public double CalcExposureTime(int iExpDiv, int iExpVal)
        {
            return iExpVal / (20000000.0 / (double)(16 * Math.Pow(2.0, (double)iExpDiv)));
        }

        static public double CalcPaddingTime(int iPadVal, int iExpDiv)
        {
            return iPadVal / (20000000.0 / (double)(16 * Math.Pow(2.0, (double)iExpDiv)));
        }

        static public double CalcFixedBlankingTime(int iExpDiv)
        {
            return 6 / (20000000.0 / (double)(16 * Math.Pow(2.0, (double)iExpDiv)));
        }

        static public double CalcScanTime(int iExpDiv, int iExpVal, int iPadVal)
        {
            return CalcExposureTime(iExpDiv, iExpVal) + CalcPaddingTime(iPadVal, iExpDiv) + CalcFixedBlankingTime(iExpDiv);
        }

        static public double CalcScanRate(int iExpDiv, int iExpVal, int iPadVal)
        {
            return 1 / CalcScanTime(iExpDiv, iExpVal, iPadVal);
        }

        public override bool SetCameraStatus(string sStat)
        {
            string sRet = "";
            bool bResult = true;
            bResult = sendCommandAndreadResponse(makeCommand(sStat, null), ref sRet);
            if (!bResult || analyseResponse(sRet) != EReturnCode.OK)
                return false;
            return true;
        }

        public override bool SetCameraStatus(string sStat, params int[] p)
        {
            //            if (p != null)
            //                return false;

            string sRet = "";
            bool bResult = true;

            switch (sStat)
            {
                case "rst":
                case "rfd":
                case "sav":
                    bResult = SetCameraStatus(sStat);
                    break;
                case "wht": // 時間がかかるのでタイムアウト設定を長くする
                    bResult = sendCommandAndreadResponse(makeCommand(sStat, p), ref sRet, 10000);
                    if (!bResult || analyseResponse(sRet) != EReturnCode.OK)
                        bResult = false;
                    break;

                case "gax":
                case "gdx":
                case "odx":
                case "inm":
                case "pad":
                case "rev":
                case "voc":
                case "tpn":
                    if (p.Length != 1)
                        return false;
                    bResult = sendCommandAndreadResponse(makeCommand(sStat, p), ref sRet);
                    if (!bResult || analyseResponse(sRet) != EReturnCode.OK)
                        bResult = false;
                    break;

                case "int":
                case "shc":
                case "voa":
                    if (p.Length != 2)
                        return false;
                    bResult = sendCommandAndreadResponse(makeCommand(sStat, p), ref sRet);
                    if (!bResult || analyseResponse(sRet) != EReturnCode.OK)
                        bResult = false;
                    break;

                default:
                    // それ以外のコマンドは未対応
                    return false;

            }
            return bResult;
        }

        public override bool GetCameraStatus(string sStat, out int[] p)
        {
            p = null;
            switch (sStat)
            {
                case "gax":
                    p = new int[] { (int)_gainNow };
                    break;
                case "gax_range":
                    p = new int[] { (int)_gainNow, (int)_gainMin, (int)_gainMax, (int)_gainStep };
                    break;
                case "gdx":
                    p = new int[] { _gainDigtalNow };
                    break;
                case "gdx_range":
                    p = new int[] { _gainDigtalNow, _gainDigtalMin, _gainDigtalMax, _gainDigtalStep };
                    break;
                case "odx":
                    p = new int[] { _offsetNow };
                    break;
                case "odx_range":
                    p = new int[] { _offsetNow, _offsetMin, _offsetMax, _offsetStep };
                    break;
                case "int":
                    p = new int[] { _exposureDivNow, _exposureTimeNow };
                    break;
                case "int_range":
                    p = new int[] { _exposureDivNow, _exposureDivMin, _exposureDivMax, _exposureDivStep,
                                    _exposureTimeNow, _exposureTimeMin, _exposureTimeMax, _exposureTimeStep };
                    break;
                default:
                    return false;
            }
            return true;
        }

        public override bool GetCameraStatus(string sStat, out string[] p)
        {
            p = null;

            bool bResult = false;
            string sRet = "";
            switch (sStat)
            {
                case "sta":
                    bResult = sendCommandAndreadResponse("sta", ref sRet);
                    Dictionary<string, string> dicStat;
                    if (bResult && analyseStatusCommandResponse(sRet, out dicStat))
                    {
                        p = new string[dicStat.Count];

                        int i = 0;
                        foreach (string sKey in dicStat.Keys)
                        {
                            p[i] = sKey + " " + dicStat[sKey];
                            i++;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return bResult;
        }
        public override double GetGain()
        {
            return _gainNow;
        }

        public override bool GetGainRange(ref double min, ref double max, ref double step, ref double now)
        {
            if (!IsOpen)
                return false;

            if (!_enableGain)
                return false;

            min = _gainMin;
            max = _gainMax;
            step = _gainStep;
            now = _gainNow;
            return true;
        }

        public override bool SetGain(double value)
        {
            if (!IsOpen)
                return false;

            if (!_enableGain)
                return false;

            if (value < _gainMin || value > _gainMax)
                return false;
            bool bResult = SetCameraStatus("gax", new int[] { (int)value });
            if (bResult)
            {
                _gainNow = value;
            }
            return true;
        }

        public override int GetOffset()
        {
            if (!IsOpen)
                return 0;
            if (_enableOffset)
                return 0;

            return _offsetNow;
        }

        public int GetGainDigital()
        {
            if (!IsOpen)
                return -1;

            if (!_enableGain)
                return -1;

            return _gainDigtalNow;
        }

        public bool SetGainDigital(int iVal)
        {
            if (!IsOpen)
                return false;

            if (!_enableGain)
                return false;

            bool bResult = SetCameraStatus("gdx", new int[] { iVal });
            if (bResult)
            {
                _gainDigtalNow = iVal;
            }

            return true;
        }

        public bool GetGainDigitalRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;
            if (!_enableGain)
                return false;

            min = _gainDigtalMin;
            max = _gainDigtalMax;
            step = _gainDigtalStep;
            now = _gainDigtalNow;

            return true;
        }

        public override bool GetOffsetRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableOffset)
                return false;

            min = _offsetMin;
            max = _offsetMax;
            step = _offsetStep;
            now = _offsetNow;

            return true;
        }

        public override bool SetOffset(int value)
        {
            if (!IsOpen)
                return false;

            if (!_enableOffset)
                return false;

            if (value < _offsetMin || value > _offsetMax)
                return false;

            bool bResult = SetCameraStatus("odx", new int[] { value });
            if (bResult)
            {
                _offsetNow = value;
            }
            return true;
        }


        public bool SetProgramableExposureTime(int value1, int value2)
        {
            if (!IsOpen)
                return false;

            if (value1 < _exposureDivMin || value1 > _exposureDivMax)
                return false;

            if (value2 < _exposureTimeMin || value2 > _exposureTimeMax)
                return false;

            bool bResult = SetCameraStatus("int", new int[] { value1, value2 });
            if (bResult)
            {
                _exposureDivNow = value1;
                _exposureTimeNow = value2;
            }

            return true;
        }

        public bool GetProgramableExposureTime(ref int value1, ref int value2)
        {
            if (!IsOpen)
                return false;

            value1 = _exposureDivNow;
            value2 = _exposureTimeNow;

            return true;
        }

        public bool GetProgramableExposureTimeRange(ref int v1min, ref int v1max, ref int v1step, ref int v1now,
                                                     ref int v2min, ref int v2max, ref int v2step, ref int v2now)
        {
            if (!IsOpen)
                return false;

            v1min = _exposureDivMin;
            v1max = _exposureDivMax;
            v1step = _exposureDivStep;
            v1now = _exposureDivNow;

            v2min = _exposureTimeMin;
            v2max = _exposureTimeMax;
            v2step = _exposureTimeStep;
            v2now = _exposureTimeNow;

            return true;
        }

        public int GetExposureReadTime()
        {
            if (!IsOpen)
                return -1;

            return _expReadTimeNow;

        }

        public bool SetExposureReadTime(int iVal)
        {
            if (!IsOpen)
                return false;


            bool bResult = SetCameraStatus("pad", new int[] { iVal });
            if (bResult)
            {
                _expReadTimeNow = iVal;
            }
            return bResult;
        }

        public bool GetExposureReadTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            min = _expReadTimeMin;
            max = _expReadTimeMax;
            step = _expReadTimeStep;
            now = _expReadTimeNow;

            return true;
        }

        public override int GetExposureTime()
        {
            if (!IsOpen)
                return -1;

            int iVal1 = 0, iVal2 = 0;
            GetProgramableExposureTime(ref iVal1, ref iVal2);

            return (int)(CalcExposureTime(iVal1, iVal2) * 1000000);
        }

        public override bool SetExposureTime(int value)
        {
            if (!IsOpen)
                return false;

            int iVal1, iVal2;
            FindExposure(value, out iVal1, out iVal2);
            return SetProgramableExposureTime(iVal1, iVal2);
        }

        public override bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            min = (int)(CalcExposureTime(_exposureDivMin, _exposureTimeMin) * 1000000);
            max = (int)(CalcExposureTime(_exposureDivMax, _exposureTimeMax) * 1000000);
            step = 1;
            now = GetExposureTime();

            return true;
        }

        public override bool SetLineInputMode(LineInput eLineInput)
        {
            if (!IsOpen)
                return false;

            if (eLineInput == LineInput.InternalLineRate)
            {
                SetCameraStatus("inm", new int[] { 0 });
            }
            else
            {
                SetCameraStatus("inm", new int[] { 1 });
            }

            return true;
        }

        public bool SetHeight(int iHeight)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "image_height", iHeight);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
            }
            return true;
        }

        public bool SetEncoderScanStep(int iStep)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "encoder_scan_step", iStep);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
            }
            return true;
        }

        HTuple _htDevice;
        HTuple _htPort;

        public override bool SetTriggerMode(TriggerMode trig, bool bCaptureThread = true)
        {
            if (!IsOpen)
            {
                return false;
            }
            try
            {
                if (_tmNowTriggerMode == trig)
                    return true;

                // ハードトリガモードの場合、スレッド停止
                if (IsHardTrigger() || IsLive)
                {
                    endCaptureThread();
                }

                if (IsHardTrigger(trig))
                {
                    CommClose();
                    HOperatorSet.CloseFramegrabber(_htAcqHandle);
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", "default", "true", _sTargetTrigCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    SetGrabTimeout(100000000);
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1.0);
                    if (CommKeepOpen)
                        CommOpen();
                    beginCaptureThread();
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    CommClose();
                    HOperatorSet.CloseFramegrabber(_htAcqHandle);
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", "default", _bFreeRunTrig.ToString().ToLower(), _sTargetFreerunCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    SetGrabTimeout(-1);
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1.0);
                    if (CommKeepOpen)
                        CommOpen();
                    beginCaptureThread();
                }
                else if (trig == TriggerMode.Software)
                {
                    CommClose();
                    HOperatorSet.CloseFramegrabber(_htAcqHandle);
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", "default", "false", _sTargetTrigCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    SetGrabTimeout(1000000000);
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1.0);
                    if (CommKeepOpen)
                        CommOpen();
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }

            _tmNowTriggerMode = trig;
            return true;
        }

        public override bool TriggerModeOpen(TriggerMode trig)
        {
            try
            {
                if (_sTargetFreerunCamFile == "" || _sTargetTrigCamFile == "")
                    return false;

                HTuple htHorizontalResolution = _dicOpenParams.GetValueOrDefault("horizontalresolution", new HTuple(1));
                HTuple htVerticalResolution = _dicOpenParams.GetValueOrDefault("verticalresolution", new HTuple(1));
                HTuple htImageWidth = _dicOpenParams.GetValueOrDefault("imagewidth", new HTuple(0));
                HTuple htImageHeight = _dicOpenParams.GetValueOrDefault("imageheight", new HTuple(0));
                HTuple htStartRow = _dicOpenParams.GetValueOrDefault("startrow", new HTuple(0));
                HTuple htStartColumn = _dicOpenParams.GetValueOrDefault("startcolumn", new HTuple(0));
                HTuple htField = _dicOpenParams.GetValueOrDefault("field", new HTuple("default"));
                HTuple htBitPerChannel = _dicOpenParams.GetValueOrDefault("bitperchannel", new HTuple(-1));
                HTuple htColorSpace = _dicOpenParams.GetValueOrDefault("colorspace", new HTuple("gray"));
                HTuple htGeneric = _dicOpenParams.GetValueOrDefault("generic", new HTuple(0));
                HTuple htExternalTrigger = _dicOpenParams.GetValueOrDefault("externaltrigger", new HTuple("false"));
                HTuple htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(0));

                if (IsHardTrigger(trig))
                {
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", htGeneric, "true", _sTargetTrigCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    if (CommKeepOpen)
                        CommOpen();
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", htGeneric, _bFreeRunTrig.ToString().ToLower(), _sTargetFreerunCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    if (CommKeepOpen)
                        CommOpen();
                }
                else if (trig == TriggerMode.Software)
                {
                    HOperatorSet.OpenFramegrabber("LinX", 1, 1, 0, 0, 0, 0, "default", 8, "gray", htGeneric, "false", _sTargetTrigCamFile, _htDevice, _htPort, -1, out _htAcqHandle);
                    if (CommKeepOpen)
                        CommOpen();
                }

                prevGrabStartAction();

                SetGrabTimeout(100000000);
                HOperatorSet.GrabImageStart(_htAcqHandle, -1.0);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            _tmNowTriggerMode = trig;
            return true;
        }

        public override bool StartGrab()
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                if (IsHardTrigger(_tmNowTriggerMode) || _tmNowTriggerMode == TriggerMode.FreeRun)
                {
                    beginCaptureThread();
                }
                else if (_tmNowTriggerMode == TriggerMode.Software)
                {
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            return true;
        }

        public override bool StopGrab()
        {
            if (!IsOpen)
            {
                return false;
            }

            if (IsHardTrigger() || IsLive)
            {
                endCaptureThread();
            }

            return base.StopGrab();
        }

        protected override void prevGrabStartAction()
        {
            if (!IsOpen)
                return;

            int iHeight;
            if (_dicOpenParams.Keys.Contains("height") && int.TryParse(_dicOpenParams["height"].I.ToString(), out iHeight))
            {
                SetHeight(iHeight);
            }

            int iUseEncoder, iEncoderScanStep;
            openParamFetch("UseEncoder", 0, out iUseEncoder);
            if (iUseEncoder != 0)
            {
                // カメラの設定を変える
                SetLineInputMode(LineInput.ExternalPulse);
                _sTargetFreerunCamFile = _sEncFreerunCamFile;
                _sTargetTrigCamFile = _sEncTrigCamFile;

                if (openParamFetch("EncoderScanStep", 0, out iEncoderScanStep))
                {
                    SetEncoderScanStep(iEncoderScanStep);
                }
                SetEncoderDivision(_iDivision);
            }
            else
            {
                SetLineInputMode(LineInput.InternalLineRate);
                _sTargetFreerunCamFile = _sFreerunCamFile;
                _sTargetTrigCamFile = _sTrigCamFile;
            }

        }

        int _iDivision = 1;
        public override bool SetEncoderDivision(int iDivision)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "register_poke:ENC_DIV_M", iDivision);
                _iDivision = iDivision;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
            }

            return true;
        }

        public override bool GetEncoderDivision(out int iDivision)
        {
            iDivision = 1;
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HTuple htDivision;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "register_peek:ENC_DIV_M", out htDivision);
                iDivision = htDivision.I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
            }
            return true;
        }
    }
}
