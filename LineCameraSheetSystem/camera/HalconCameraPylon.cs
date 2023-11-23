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
    public class HalconCameraPylon : HalconCameraBase
    {
        public HalconCameraPylon(int index, string name, string description, string mirror)
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
                HTuple wk_htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(0));

                HOperatorSet.OpenFramegrabber(
                    "pylon",                                    // 1
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

        public override bool StartGrab()
        {
            StartAsyncGrab();
            return base.StartGrab();
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

        public bool StopWhiteBAsyncGrab()
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_abort_grab", "true");
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_unlock_parameters", "true");
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                throw oe;
            }
            return true;
        }


        public override bool ExecuteUsrSetLoad()
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetLoad", 1);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }
        public override bool ExecuteUsrSetSave()
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSave", 1);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override bool SetWhiteBImageWidth(int width)
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Width", width);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                throw oe;
            }
            return true;
        }
        public override bool SetWhiteBImageHeight(int height)
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Height", height);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                throw oe;
            }
            return true;
        }
        public override bool SetWhiteBImageOffsetX(int offsetx)
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "OffsetX", offsetx);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                throw oe;
            }
            return true;
        }
        public override bool SetWhiteBImageOffsetY(int offsety)
        {
            if (!IsOpen)
                return false;
            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "OffsetY", offsety);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                throw oe;
            }
            return true;
        }
        public override bool UserSetSave()
        {
            if (_dicOpenParams.Keys.Contains("configurationset"))
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSelector", _dicOpenParams["configurationset"]);
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "UserSetSave", 1);
                }
                catch (HOperatorException)
                {
                    return false;
                }
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
                    double gainNow = 0;
                    GetGainRange(ref _gainMin, ref _gainMax, ref _gainStep, ref gainNow);
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
                HTuple htOffsetRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTimeRaw_Range", out htOffsetRange);

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

            if (!_enableOffset)
                return false;
            try
            {
                HTuple htOffsetRange;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "DigitalShift_Range", out htOffsetRange);

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

                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "DigitalShift", value);
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// オフセット値取得
        /// </summary>
        /// <returns>0>オフセット値 -1 エラー</returns>
        public override int GetOffset()
        {
            if (!IsOpen)
                return -1;

            if (!_enableOffset)
                return -1;
            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "DigitalShift", out value);
                return value[0].I;
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
                if (IsHardTrigger() || _tmNowTriggerMode == TriggerMode.FreeRun)
                {
                    endCaptureThread();
                }

                if (IsHardTrigger(trig))
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionMode", "Continuous");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "AcquisitionStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", trig.ToString());
                    SetGrabTimeout(100000000);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "AcquisitionStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    SetGrabTimeout(-1);
                    if (bCaptureThread)
                    {
                        beginCaptureThread();
                    }
                }
                else if (trig == TriggerMode.Software)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionMode", "Continuous");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "AcquisitionStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "FrameStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", trig.ToString());
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
    }

    /// <summary>
    /// Basler GigEカメラ
    /// </summary>
    public class HalconCameraPylonGigE : HalconCameraPylon
    {
        public HalconCameraPylonGigE(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _hardTrigger = TriggerMode.Line1;
            _enableTriggerDelay = true;
            IsGigE = true;
            IsCameraLink = false;
            IsAreaSensor = true;
            IsLineSensor = false;
        }

        public string IpAddress { get; private set; }

        public override bool Open()
        {
            if (!_dicOpenParams.Keys.Contains("device"))
            {
                return base.Open();
            }

            string device = _dicOpenParams["device"];

            // IPアドレス指定の場合
            Regex reg = new Regex(@"^(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])$");
            // IPアドレス指定の場合
            if (reg.IsMatch(device))
            {
                Match mtc = reg.Match(device);
                string sRealIP = int.Parse(mtc.Groups[1].Value).ToString() + "."
                    + int.Parse(mtc.Groups[2].Value).ToString() + "."
                    + int.Parse(mtc.Groups[3].Value).ToString() + "."
                    + int.Parse(mtc.Groups[4].Value).ToString();

                IpAddress = sRealIP;
                return open(sRealIP);
            }
            return base.Open();
        }

        public bool open(string sIP)
        {
            HTuple htInformation, htValueList;
            try
            {
                HOperatorSet.InfoFramegrabber("pylon", "device", out htInformation, out htValueList);
                for (int i = 0; i < htValueList.Length; i++)
                {
                    string sVal = htValueList[i].S;

                    if (sVal.IndexOf(sIP) != -1)
                    {
                        _dicOpenParams["device"] = htValueList[i];
                        return base.Open();
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

    }

    public class HalconCameraPylonGigELineSensor : HalconCameraPylonGigE
    {
        public HalconCameraPylonGigELineSensor(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _hardTrigger = TriggerMode.Line1;
            _enableLineRate = true;
            IsLineSensor = true;
            IsAreaSensor = false;
        }

        public override bool Open()
        {
            bool bResult = base.Open();

            if (bResult)
            {
                HTuple htWidth, htHeight;
                try
                {
                    HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Width", out htWidth);
                    _imageWidth = (int)htWidth.D;
                    HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Height", out htHeight);
                    _imageHeight = (int)htHeight.D;
                }
                catch (HOperatorException)
                {
                    return true;
                }
            }
            return bResult;
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

            int iHeight;
            if (_dicOpenParams.Keys.Contains("height") && int.TryParse(_dicOpenParams["height"].I.ToString(), out iHeight))
            {
                SetHeight(iHeight);
            }

            // ﾗｲﾝﾄﾘｶﾞ設定
            int iValue;
            string sInputSource;

            openParamFetch("UseEncoder", 0, out iValue);
            openParamFetch("FrequencyConverterInputSource", "", out sInputSource);
            if (iValue != 0 && sInputSource != "")
            {
                int iPreDivider;
                int iMultiplier;
                int iPostDvider;

                openParamFetch("FrequencyConverterPreDivider", 1, out iPreDivider);
                openParamFetch("FrequencyConverterMultiplier", 1, out iMultiplier);
                openParamFetch("FrequencyConverterPostDivider", 1, out iPostDvider);

                SetFrequencyConverterParam(iPreDivider, iMultiplier, iPostDvider);
                SetLineInputMode(LineInput.ExternalPulse);
                IsUseEncoder = true;
            }
            else
            {
                SetLineInputMode(LineInput.InternalLineRate);
                IsUseEncoder = false;
            }


            string sShadingSetSelector;
            openParamFetch("UseShading", 0, out iValue);
            openParamFetch("ShadingSetSelector", "", out sShadingSetSelector);

            if (iValue != 0 && sShadingSetSelector != "")
            {
                if (iValue == 1)
                {
                    DoShading(true, sShadingSetSelector);
                }
                else
                {
                    DoShading(false, "");
                }
            }

        }

        public bool SetFrequencyConverterParam(int iPreDiv, int iMulti, int iPostDiv)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "FrequencyConverterPreDivider", iPreDiv);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "FrequencyConverterMultiplier", iMulti);
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "FrequencyConverterPostDivider", iPostDiv);
            }
            catch (HOperatorException)
            {
            }

            return true;
        }
        public override bool GetExposureTimeRange(ref int min, ref int max, ref int step, ref int now)
        {
            if (!IsOpen)
            {
                return false;
            }

            HTuple htExposureTimeAbs;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTimeAbs_range", out htExposureTimeAbs);
                min = (int)htExposureTimeAbs[0].D;
                max = (int)htExposureTimeAbs[1].D;
                step = (int)htExposureTimeAbs[2].D;
                now = (int)htExposureTimeAbs[3].D;
            }
            catch (HOperatorException)
            {
                return false;
            }

            return true;
        }

        public override int GetExposureTime()
        {
            if (!IsOpen)
            {
                return -1;
            }

            HTuple htExposureTimeAbs;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ExposureTimeAbs", out htExposureTimeAbs);
                return (int)htExposureTimeAbs.D;
            }
            catch (HOperatorException)
            {
                return -1;
            }
        }

        public override bool SetExposureTime(int value)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ExposureTimeAbs", value);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "TriggerDelayAbs_range", out htTriggerDelayTimeAbs);
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

        public override double GetResultingLineRate()
        {
            if (!IsOpen)
            {
                return 0.0;
            }

            HTuple htResultingLineRateAbs;

            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ResultingLineRateAbs", out htResultingLineRateAbs);
                return htResultingLineRateAbs.D;
            }
            catch (HOperatorException)
            {
                return 0.0;
            }
        }

        public override double GetResultingFrameRate()
        {
            if (!IsOpen)
            {
                return 0.0;
            }

            HTuple htResultingFrameRate;

            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ResultingFrameRateAbs", out htResultingFrameRate);
                return htResultingFrameRate.D;
            }
            catch (HOperatorException)
            {
                return 0.0;
            }
        }

        public int GetHeight()
        {
            if (!IsOpen)
            {
                return 0;
            }

            HTuple htHeight;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Height", out htHeight);
                return (int)htHeight.D;
            }
            catch (HOperatorException)
            {
                return 0;
            }
        }

        public bool SetHeight(int iValue)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "Height", iValue);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public bool GetHeightRange(ref int iMin, ref int iMax, ref int iStep, ref int iNow)
        {
            if (!IsOpen)
            {
                return false;
            }

            HTuple htHeightRange;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "Height_range", out htHeightRange);

                iMin = (int)htHeightRange[0].D;
                iMax = (int)htHeightRange[1].D;
                iStep = (int)htHeightRange[2].D;
                iNow = (int)htHeightRange[3].D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public bool DoShading(bool bEnable, string sUserSetSelector)
        {
            if (!IsOpen)
            {
                return false;
            }

            if (sUserSetSelector != "DefaultShadingSet"
                || sUserSetSelector != "UserShadingSet1"
                || sUserSetSelector != "UserShadingSet2"
                )
            {
                return false;
            }

            try
            {
                if (bEnable)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ShadingSetSelector", sUserSetSelector);  //DefaultShadingSet, UserShadingSet1, UserShadingSet2
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ShadingSetActivate", "execute");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ShadingEnable", 1);
                }
                else
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "ShadingEnable", 0);
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

        public bool GetShadingEnable(ref bool bEnable)
        {
            HTuple htValue;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "ShadingEnable", out htValue);
                bEnable = htValue.I == 1 ? true : false;
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
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
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "AcquisitionLineRateAbs", out htValue);
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
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "AcquisitionLineRateAbs", dLineRate);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public override bool SetLineInputMode(LineInput eLineInput)
        {
            if (!IsLineSensor)
            {
                return false;
            }

            if (!IsOpen)
            {
                return false;
            }

            try
            {
                if (eLineInput == LineInput.InternalLineRate)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "LineStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "Off");

                }
                else if (eLineInput == LineInput.ExternalPulse)
                {
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSelector", "LineStart");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerMode", "On");
                    HOperatorSet.SetFramegrabberParam(_htAcqHandle, "TriggerSource", "FrequencyConverter");
                }
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
            return true;
        }

        public override bool GetLineRateRange(ref double dMin, ref double dMax, ref double dStep, ref double dNow)
        {
            if (!IsOpen)
            {
                return false;
            }

            HTuple htValues;
            try
            {
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "AcquisitionLineRateAbs_range", out htValues);

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
    }

}
