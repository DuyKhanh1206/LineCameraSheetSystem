using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Text.RegularExpressions;

using HalconDotNet;
using DALSA.SaperaLT.SapClassBasic;
#if FUJITA_INSPECTION_SYSTEM
using Fujita.Misc;
using Fujita.InspectionSystem;
#endif

namespace HalconCamera
{
    /// <summary>
    /// Basler Pylonライブラリベース
    /// </summary>
    public class HalconCameraSaperaLT : HalconCameraBase
    {
        public HalconCameraSaperaLT(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _enableGain = true;
            _enableOffset = true;
            _enableTimeout = true;
            _enableExposureTime = true;
            _enableTriggerDelay = true;
            EnableHardTrigger = true;
            EnableSoftTrigger = true;
        }

        public override bool Open()
        {
            if (IsOpen)
                return false;
            try
            {                
                // デバイス情報を設定する
                HTuple wk_htHorizontalResolution = _dicOpenParams.GetValueOrDefault("horizontalresolution", new HTuple(1));
                HTuple wk_htVerticalResolution = _dicOpenParams.GetValueOrDefault("verticalresolution", new HTuple(1));
                HTuple wk_htImageWidth = _dicOpenParams.GetValueOrDefault("imagewidth", new HTuple(0));
                HTuple wk_htImageHeight = _dicOpenParams.GetValueOrDefault("imageheight", new HTuple(0));
                HTuple wk_htStartRow = _dicOpenParams.GetValueOrDefault("startrow", new HTuple(0));
                HTuple wk_htStartColumn = _dicOpenParams.GetValueOrDefault("startcolumn", new HTuple(0));
                HTuple wk_htField = _dicOpenParams.GetValueOrDefault("field", new HTuple("default"));
                HTuple wk_htBitPerChannel = _dicOpenParams.GetValueOrDefault("bitperchannel", new HTuple(-1));
                HTuple wk_htColorSpace = _dicOpenParams.GetValueOrDefault("colorspace", new HTuple("default"));
                HTuple wk_htExternalTrigger = _dicOpenParams.GetValueOrDefault("externaltrigger", new HTuple("false"));
                HTuple wk_htGeneric = _dicOpenParams.GetValueOrDefault("generic", new HTuple(-1));
                HTuple wk_htCameraType = _dicOpenParams.GetValueOrDefault("cameratype", new HTuple("default"));
                //HTuple wk_htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple wk_htDevice = DeviceCheck();             //1332b 処理変更
                HTuple wk_htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple wk_htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(-1));

                HOperatorSet.OpenFramegrabber(
                    "SaperaLT",                                    // 1
                    wk_htHorizontalResolution,                  // 2
                    wk_htVerticalResolution,                    // 3
                    wk_htImageWidth,                            // 4
                    wk_htImageHeight,                           // 5
                    wk_htStartRow,                              // 6
                    wk_htStartColumn,                           // 7
                    wk_htField,                                 // 8
                    wk_htBitPerChannel,                         // 9
                    wk_htColorSpace,                            // 10
                    wk_htGeneric,                               // 13
                    wk_htExternalTrigger,                       // 11
                    wk_htCameraType,                            // 12
                    wk_htDevice,                                // 14
                    wk_htPort,                                  // 15
                    wk_htLineIn,                                // 16
                    out _htAcqHandle);                          // 17

                // トリガの種別を受ける
                if (_dicOpenParams.Keys.Contains("hardtriggersource"))
                {
                    TriggerMode eTriggerHard;
                    if (Enum.TryParse<TriggerMode>(_dicOpenParams["hardtriggersource"], out eTriggerHard))
                    {
                        HardTrigger = eTriggerHard;
                    }
                }


                base.Open();
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public bool StartAsyncGrab()
        {
            if (!IsOpen)
                return false;

            try
            {
                HOperatorSet.GrabImageStart(_htAcqHandle, -1.0);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public bool StopAsyncGrab()
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_unlock_parameters", "true");
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override bool GetGainRange(ref double min, ref double max, ref double step, ref double now)
        {
            if (!IsOpen)
                return false;
            try
            {
                HTuple htGainRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Gain_range", out htGainRange);

                min = htGainRange[0].D;
                max = htGainRange[1].D;
                step = 0.1;
                now = htGainRange[3].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override bool SetGain(double value)
        {
            if (!IsOpen)
                return false;

            if (!_enableGain)
                return false;

            try
            {
                if (_gainMin == -1)
                {
                    double gainNow = 0;
                    GetGainRange(ref _gainMin, ref _gainMax, ref _gainStep, ref gainNow);
                }

                if (value < _gainMin || value > _gainMax)
                {
                    return false;
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Gain", value);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override double GetGain()
        {
            if (!IsOpen)
                return -1;

            if (!_enableGain)
                return -1;

            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Gain", out value);
                return value[0].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }
        public override bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;
            try
            {
                HTuple htExposureTimeRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTimeRaw_Range", out htExposureTimeRange);

                min = htExposureTimeRange[0].I;
                max = htExposureTimeRange[1].I;
                step = htExposureTimeRange[2].I;
                now = htExposureTimeRange[3].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override bool SetExposureTime(int value)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;

            try
            {
                if (_exposureTimeMin == -1)
                {
                    GetExposureTimeRange(ref _exposureTimeMin, ref _exposureTimeMax, ref _exposureTimeStep, ref _exposureTimeNow);
                }

                if (value < _exposureTimeMin || value > _exposureTimeMax)
                {
                    return false;
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ExposureTimeRaw", value);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override int GetExposureTime()
        {
            if (!IsOpen)
                return -1;

            if (!_enableExposureTime)
                return -1;
            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTimeRaw", out value);
                return value[0].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }

        public override bool GetOffsetRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;
            try
            {
                HTuple htOffsetRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "BlackLevel_range", out htOffsetRange);

                min = (int)htOffsetRange[0].D;
                max = (int)htOffsetRange[1].D;
                step = (int)htOffsetRange[2].D;
                now = (int)htOffsetRange[3].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override bool SetOffset(int value)
        {
            if (!IsOpen)
                return false;

            if (!_enableOffset)
                return false;

            try
            {
                if (_offsetMin == -1)
                {
                    GetOffsetRange(ref _offsetMin, ref _offsetMax, ref _offsetStep, ref _offsetNow);
                }

                if (value < _offsetMin || value > _offsetMax)
                {
                    return false;
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "BlackLevel", value);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override int GetOffset()
        {
            if (!IsOpen)
                return -1;

            if (!_enableOffset)
                return -1;

            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "BlackLevel", out value);
                return (int)value[0].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }

        /// <summary>
        /// ソフトウェアトリガを実行する
        /// </summary>
        /// <returns>true 正常終了 false 異常終了</returns>
        protected override bool executeTrigger()
        {
            if (_tmNowTriggerMode != TriggerMode.Software)
                return false;

            try
            {
                SetTriggerMode(TriggerMode.Software);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSoftware", "Enable");
            }
            catch (HOperatorException oe)
            {
                setError(true, oe.Message);
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }

            return true;
        }

        public override bool SetTriggerMode_Hard_GrabImageStart()
        {
            return true;
        }
        public override bool SetTriggerMode(TriggerMode trig, bool bCaptureThread = true)
        {
            if (!IsOpen)
                return false;

            try
            {
                // ハードトリガモードの場合、スレッド停止
                if (IsHardTrigger()
                    || _tmNowTriggerMode == TriggerMode.FreeRun)
                {
                    endCaptureThread();
                }

                if (IsHardTrigger(trig))
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "grab_timeout", -1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", trig.ToString());
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);

                    SetGrabTimeout(1000);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1);
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "grab_timeout", 1000);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
                    //HOperatorSet.GrabImageStart(_htAcqHandle, _htMaxDelay);

                    SetGrabTimeout(-1);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.Software)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "grab_timeout", 1000);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", trig.ToString());
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
                    HOperatorSet.GrabImageStart(_htAcqHandle, _htMaxDelay);
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                bInternalError = true;
                return false;
            }
            _tmNowTriggerMode = trig;
            return true;
        }

        // キャプチャ時データを取得する方法
        public override bool getImage(out HObject img)
        {
            img = null;
            try
            {
                System.Diagnostics.Debug.WriteLine("getImage() START");
                HOperatorSet.GrabImageAsync(out img, _htAcqHandle, -1);
                System.Diagnostics.Debug.WriteLine("getImage() END");
            }
            catch (HOperatorException)
            {
                System.Diagnostics.Debug.WriteLine("getImage() HOperatorException");
                img = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 非同期通信を終了する
        /// </summary>
        /// <returns>true 正常終了　false 異常終了</returns>
        protected override bool endCaptureThread()
        {
            if (_captureThread == null)
                return false;
            _stop = true;
            try
            {
                // 取り込みをアボートする
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_clear_buffers", 1);

                HTuple htParam;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", out htParam);
                if (htParam == "enable")
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", "disable");
                }
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
            }

            do
            {
                _captureThread.Join(100);
            }
            while (_captureThread.ThreadState == System.Threading.ThreadState.Running);
            _captureThread = null;

            return true;
        }

        //protected override int CheckStatus(int iRepeat = 5)//v1328
        //{
        //    return 0;
        //}

        /// <summary>
        /// オープンするためのデバイス名の確認 
        /// </summary>
        /// <returns>デバイス名</returns>
        private string DeviceCheck()
        {
            //参照とusingに　DALSA.SaperaLT.SapClassBasic　を追加する必要あり
            int serverCount = SapManager.GetServerCount();
            List<string[]> listServerNames = new List<string[]>();

            for (int serverIndex = 0; serverIndex < serverCount; serverIndex++)
            {
                if (SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.AcqDevice) != 0)
                {
                    string serverName = SapManager.GetServerName(serverIndex);
                    string deviceName = SapManager.GetResourceName(serverName, SapManager.ResourceType.AcqDevice, 0);


                    listServerNames.Add(new string[] { serverName, deviceName });
                }
            }
            if (listServerNames.Count == 0)
                return "";

            HTuple wk_htSerialNumber = _dicOpenParams.GetValueOrDefault("serialnumber", new HTuple("default"));
            string stSerial = (wk_htSerialNumber.Type == HTupleType.STRING) ? wk_htSerialNumber.S : wk_htSerialNumber.ToString();
            string[] stName = listServerNames.Find(x => x[1] == stSerial);
            return stName[0];
        }
    }

    /// <summary>
    /// DALSA GigE LineSensorカメラ
    /// </summary>
    public class HalconCameraSaperaLTDALSALineSensor : HalconCameraSaperaLT
    {
        public HalconCameraSaperaLTDALSALineSensor(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _hardTrigger = TriggerMode.Line1;
            _enableTriggerDelay = false;
            _enableLineRate = true;
            IsGigE = true;
            IsCameraLink = false;
            IsAreaSensor = false;
            IsLineSensor = true;
        }
        /// <summary>
        /// Open
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            bool bResult = base.Open();
            if (bResult)
            {
                try
                {
                }
                catch (HOperatorException oe)
                {
                    TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                    return true;
                }
            }

            // トリガーモードをソフトウェアにしておく
            SetTriggerMode(TriggerMode.Software);

            return bResult;
        }

        public void AbortGrab()
        {
            try
            {
                // 取り込みをアボートする
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_clear_buffers", 1);

                HTuple htParam;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", out htParam);
                if (htParam == "enable")
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", "disable");
                }
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
            }
        }

        protected override void prevGrabStartAction()
        {
            try
            {
                if (_dicOpenParams.Keys.Contains("configurationset"))
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSelector", _dicOpenParams["configurationset"]);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetLoad", 1);
                }
                if (_dicOpenParams.Keys.Contains("isencoder"))
                {
                    HTuple val = _dicOpenParams["isencoder"];
                    bool isEncoder = false;
                    if ((val.Type == HTupleType.INTEGER && val.I == 1) || (val.Type == HTupleType.STRING && (val.S.ToLower() == "on" || val.S.ToLower() == "true")))
                        isEncoder = true;

                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "LineStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", (isEncoder == true) ? "On" : "Off");
                }
            }
            catch (HOperatorException)
            {
            }
            base.prevGrabStartAction();
        }

        public override bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;
            try
            {
                min = 4;
                max = 2000;
                step = 1;
                now = GetExposureTime();
                //HTuple htExposureTimeRange;
                //HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTime_Range", out htExposureTimeRange);
                //min = (int)htExposureTimeRange[0].D;
                //max = (int)htExposureTimeRange[1].D;
                //step = (int)htExposureTimeRange[2].D;
                //now = (int)htExposureTimeRange[3].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override bool SetExposureTime(int value)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;

            try
            {
                if (_exposureTimeMin == -1)
                {
                    GetExposureTimeRange(ref _exposureTimeMin, ref _exposureTimeMax, ref _exposureTimeStep, ref _exposureTimeNow);
                }

                if (value < _exposureTimeMin || value > _exposureTimeMax)
                {
                    return false;
                }
                System.Threading.Thread.Sleep(50);
                //HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ExposureTime", value);
                //HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
                System.Threading.Thread.Sleep(100);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override int GetExposureTime()
        {
            if (!IsOpen)
                return -1;

            if (!_enableExposureTime)
                return -1;
            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTime", out value);
                return (int)value[0].D;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }

        public override bool GetLineRateRange(ref double dMin, ref double dMax, ref double dStep, ref double dNow)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                dMin = 100.9;
                dMax = 29000;
                dStep = 1;
                dNow = GetLineRate();
                //HTuple htValues;
                //HOperatorSet.GetFramegrabberParam(_htAcqHandle, "AcquisitionLineRate_Range", out htValues);
                //dMin = htValues[0].D;
                //dMax = htValues[1].D;
                //dStep = htValues[2].D;
                //dNow = htValues[3].D;
            }
            catch (HOperatorException)
            {
                return false;
            }

            return true;
        }
        public override double GetLineRate()
        {
            if (!IsOpen)
            {
                return 0.0;
            }

            HTuple htValue;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "AcquisitionLineRate", out htValue);
                return htValue.D;
            }
            catch (HOperatorException)
            {
                return 0.0;
            }
        }
        public override bool SetLineRate(double dLineRate)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                lock (_htAcqHandle)
                {
                    //HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionLineRate", dLineRate);
                    //HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }
        /// <summary>通信確認、HalconCameraSaperaLTDALSALineSensorは、前回Grabとの時間差が1000ms以下なら通信OK</summary>
        protected override bool CheckCommunication()//v1328
        {
            //bool bRes = true;
            int SpanValue = 1000;//1000ミリ秒
            TimeSpan Span = DateTime.Now - this._GrabLastUpdate;
            System.Diagnostics.Debug.WriteLine("前回Grab：" + this._GrabLastUpdate);
            System.Diagnostics.Debug.WriteLine("差分：" + Span.TotalMilliseconds);
            return Span.TotalMilliseconds <= SpanValue;//1000ミリ秒より小さければtrue
        }


    }
}
