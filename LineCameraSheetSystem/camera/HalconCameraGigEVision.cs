using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Text.RegularExpressions;

using HalconDotNet;
#if FUJITA_INSPECTION_SYSTEM
using Fujita.Misc;
using Fujita.InspectionSystem;
#endif

namespace HalconCamera
{
    /// <summary>
    /// Basler Pylonライブラリベース
    /// </summary>
    public class HalconCameraGigEVision : HalconCameraBase
    {
        public HalconCameraGigEVision(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _enableGain = true;
            _enableOffset = true;
            _enableTimeout = true;
            _enableExposureTime = true;
            _enableTriggerDelay = false;
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
                HTuple wk_htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple wk_htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple wk_htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(-1));

                HOperatorSet.OpenFramegrabber(
                    "GigEVision2",                                    // 1
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
                if (_dicOpenParams.Keys.Contains("GevSCPSPacketSize"))
                {
                    HTuple val;
                    HOperatorSet.GetFramegrabberParam(_htAcqHandle, new HTuple("GevSCPSPacketSize"), out val);
                    int a = _dicOpenParams["GevSCPSPacketSize"];
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, new HTuple("GevSCPSPacketSize"), new HTuple(_dicOpenParams["GevSCPSPacketSize"]));
                }
                if (_dicOpenParams.Keys.Contains("GevSCPD"))
                {
                    HTuple val;
                    HOperatorSet.GetFramegrabberParam(_htAcqHandle, new HTuple("GevSCPD"), out val);
                    int a = _dicOpenParams["GevSCPD"];
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, new HTuple("GevSCPSPacketSize"), new HTuple(_dicOpenParams["GevSCPD"]));
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "GainRaw_Range", out htGainRange);

                min = htGainRange[0].I;
                max = htGainRange[1].I;
                step = htGainRange[2].I;
                now = htGainRange[3].I;
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
                    GetGainRange(ref _gainMin, ref _gainMax, ref _gainStep, ref _gainNow);
                }

                if (value < _gainMin || value > _gainMax)
                {
                    return false;
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "GainRaw", value);
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "GainRaw", out value);
                return value[0].I;
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "OffsetRaw_Range", out htOffsetRange);

                min = htOffsetRange[0].I;
                max = htOffsetRange[1].I;
                step = htOffsetRange[2].I;
                now = htOffsetRange[3].I;
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

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "OffsetRaw", value);
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "OffsetRaw", out value);
                return value[0].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }


        public override double GetTriggerDelay()
        {
            if (!IsOpen)
            {
                return -1.0;
            }

            HTuple htTriggerDelayTimeAbs;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "TriggerDelayAbs", out htTriggerDelayTimeAbs);
                return htTriggerDelayTimeAbs.D;
            }
            catch (HOperatorException)
            {
                return -1.0;
            }
        }

        public override bool SetTriggerDelay(double value)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerDelayAbs", value);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public override bool GetTriggerDelayRange(ref double min, ref double max, ref double step, ref double now)
        {
            if (!IsOpen)
            {
                return false;
            }

            HTuple htTriggerDelayTimeAbs;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "TriggerDelayRaw_range", out htTriggerDelayTimeAbs);
                min = htTriggerDelayTimeAbs[0].D;
                max = htTriggerDelayTimeAbs[1].D;
                step = htTriggerDelayTimeAbs[2].D;
                now = htTriggerDelayTimeAbs[3].D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public override bool SetTriggerMode_Hard_GrabImageStart()
        {
            HOperatorSet.GrabImageStart(_htAcqHandle, -1);
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
                    //FrameStart->OFF Acquisition->ON   通常
                    //FrameStart->ON Acquisition->ON   トリガバッファOFF


                    //                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionControl", "OneFrame");
                    //                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionMode", "Continuous");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "AcquisitionStart");
                    if (_dicOpenParams.Keys.Contains("acquisitionstart"))
                    {
                        string p = _dicOpenParams["acquisitionstart"];
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", p);
                    }
                    else
                    {
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    }
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    if (_dicOpenParams.Keys.Contains("framestart"))
                    {
                        string p = _dicOpenParams["framestart"];
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", p);
                    }
                    else
                    {
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    }
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", trig.ToString());

                    //                    HOperatorSet.GrabImageStart(_htAcqHandle, -1);

                    SetGrabTimeout(1000);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionMode", "SingleFrame");
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1);

                    SetGrabTimeout(-1);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.Software)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionMode", "SingleFrame");
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1);
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
                HOperatorSet.GrabImageAsync(out img, _htAcqHandle, -1);
            }
            catch (HOperatorException)
            {
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
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "true");

                HTuple htParam;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", out htParam);
                if (htParam == "enable")
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", "disable");
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
            while (_captureThread.ThreadState == System.Threading.ThreadState.Running);
            _captureThread = null;

            return true;
        }

    }

    /// <summary>
    /// Hitachi GigEカメラ
    /// </summary>
	public class HalconCameraGigEVisionHitachi : HalconCameraGigEVision
    {
        public HalconCameraGigEVisionHitachi(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _hardTrigger = TriggerMode.Line1;
            _enableTriggerDelay = true;
            IsGigE = true;
            IsCameraLink = false;
            IsAreaSensor = true;
            IsLineSensor = false;
        }

        protected override void prevGrabStartAction()
        {
            if (_dicOpenParams.Keys.Contains("configurationset"))
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSelector", _dicOpenParams["configurationset"]);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetLoad", 1);
                }
                catch (HOperatorException)
                {
                }
            }

            base.prevGrabStartAction();
        }
    }

    /// <summary>
    /// NED GigE LineSensorカメラ
    /// </summary>
    public class HalconCameraGigEVisionNEDLineSensor : HalconCameraGigEVision
    {
        public HalconCameraGigEVisionNEDLineSensor(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _hardTrigger = TriggerMode.Line1;
            _enableLineRate = true;
            IsGigE = true;
            IsCameraLink = false;
            IsAreaSensor = false;
            IsLineSensor = true;
        }

        public override bool Open()
        {
            if (!_dicOpenParams.Keys.Contains("device"))
            {
                return base.Open();
            }

            string device = _dicOpenParams["device"];

            if (!open(device.ToLower()))
                return false;

            return base.Open();
        }
        protected virtual bool open(string sIP)
        {
            HTuple htInformation, htValueList;
            try
            {
                HOperatorSet.InfoFramegrabber("GigEVision2", "device", out htInformation, out htValueList);
                for (int i = 0; i < htValueList.Length; i++)
                {
                    string sVal = htValueList[i].S.ToLower();

                    if (sVal.IndexOf(sIP) != -1)
                    {
                        _dicOpenParams["device"] = htValueList[i];
                        return true;
                    }
                }
            }
            catch (HOperatorException)
            {
                // ライブラリエラー
                return false;
            }
            // 見つからない
            return false;
        }
        protected override void prevGrabStartAction()
        {
            if (_dicOpenParams.Keys.Contains("configurationset"))
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSelector", _dicOpenParams["configurationset"]);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetLoad", 1);
                }
                catch (HOperatorException)
                {
                }
            }

            base.prevGrabStartAction();
        }

        public override bool GetGainRange(ref double min, ref double max, ref double step, ref double now)
        {
            if (!IsOpen)
                return false;
            try
            {
                HTuple htGainRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Gain_Range", out htGainRange);

                min = 10000;
                max = 20000;
                step = 2;
                now = 10000;

                //min = (int)htGainRange[0].D;
                //max = (int)htGainRange[1].D;
                //step = (int)htGainRange[2].D;
                //now = (int)htGainRange[3].D;
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
                    GetGainRange(ref _gainMin, ref _gainMax, ref _gainStep, ref _gainNow);
                }

                if (value < _gainMin || value > _gainMax)
                {
                    return false;
                }

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Gain", value / 10000.0);
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
                return (int)(value[0].D * 10000);
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTime_Range", out htExposureTimeRange);

                min = (int)htExposureTimeRange[0].D;
                max = (int)htExposureTimeRange[1].D;
                step = (int)1;
                now = (int)htExposureTimeRange[3].D;
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

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ExposureTime", value);
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
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "1");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "ExposureStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameActive");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", "LineIn1");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "0");
                    //HOperatorSet.GrabImageStart(_htAcqHandle, -1);

                    SetGrabTimeout(1000);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "1");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "ExposureStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameActive");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", "NoConnect");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "0");
                    HOperatorSet.GrabImageStart(_htAcqHandle, -1);

                    SetGrabTimeout(-1);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }

                }
                else if (trig == TriggerMode.Software)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "1");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "ExposureStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameActive");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", "Software");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "0");
                    //HOperatorSet.GrabImageStart(_htAcqHandle, -1);
                    SetGrabTimeout(5000);
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
        protected override bool executeTrigger()
        {
            if (_tmNowTriggerMode != TriggerMode.Software)
                return false;

            try
            {
                HOperatorSet.GrabImageStart(_htAcqHandle, -1);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSoftware", 1);
            }
            catch (HOperatorException oe)
            {
                setError(true, oe.Message);
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }

            return true;
        }

        #region LineRate
        public override bool GetLineRateRange(ref double dMin, ref double dMax, ref double dStep, ref double dNow)
        {
            if (!IsOpen)
            {
                return false;
            }

            HTuple htValues;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "AcquisitionLineRate_Range", out htValues);

                dMin = htValues[0].D;
                dMax = htValues[1].D;
                dStep = htValues[2].D;
                dNow = htValues[3].D;
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
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 1);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionLineRate", dLineRate);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", 0);
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
