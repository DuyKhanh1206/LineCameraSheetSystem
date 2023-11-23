using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using HalconDotNet;

namespace HalconCamera
{
    class HalconCameraGrabLink : HalconCameraBase, IDisposable
    {
        public HalconCameraGrabLink(int index, string name, string description, string mirror)
            : base(index, name, description, mirror)
        {
            _enableGain = false;
            _enableOffset = false;
            _enableTimeout = true;
            _enableExposureTime = true;
        }

        public override bool Open()
        {
            if (IsOpen)
                return false;

            try
            {
                // デバイス情報を設定する
                HTuple wk_htHorizontalResolution = _dicOpenParams.GetValueOrDefault("horizontalresolution", new HTuple(-1));
                HTuple wk_htVerticalResolution = _dicOpenParams.GetValueOrDefault("verticalresolution", new HTuple(-1));
                HTuple wk_htImageWidth = _dicOpenParams.GetValueOrDefault("imagewidth", new HTuple(0));
                HTuple wk_htImageHeight = _dicOpenParams.GetValueOrDefault("imageheight", new HTuple(0));
                HTuple wk_htStartRow = _dicOpenParams.GetValueOrDefault("startrow", new HTuple(0));
                HTuple wk_htStartColumn = _dicOpenParams.GetValueOrDefault("startcolumn", new HTuple(0));
                HTuple wk_htField = _dicOpenParams.GetValueOrDefault("field", new HTuple("default"));
                HTuple wk_htBitPerChannel = _dicOpenParams.GetValueOrDefault("bitperchannel", new HTuple(-1));
                HTuple wk_htColorSpace = _dicOpenParams.GetValueOrDefault("colorspace", new HTuple("rgb"));
                HTuple wk_htExternalTrigger = _dicOpenParams.GetValueOrDefault("externaltrigger", new HTuple("false"));
                HTuple wk_htGeneric = _dicOpenParams.GetValueOrDefault("generic", new HTuple(-1));
                HTuple wk_htCameraType = _dicOpenParams.GetValueOrDefault("cameratype", new HTuple("default"));
                HTuple wk_htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple wk_htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple wk_htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(0));

                HOperatorSet.OpenFramegrabber(
                    "MultiCam",                                 // 1
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

                base.Open();
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
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

        public override int GetExposureTime()
        {
            if (!IsOpen)
                return -1;

            if (!_enableExposureTime)
                return -1;

            try
            {
                HTuple value;
                HOperatorSet.GetFramegrabberParam(_htAcqHandle, "exposure_us", out value);
                return value[0].I;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return -1;
            }

        }

        public override bool SetExposureTime(int value)
        {
            if (!IsOpen)
                return false;

            if (!_enableExposureTime)
                return false;

            if (_exposureTimeMin == -1)
            {
                GetExposureTimeRange(ref _exposureTimeMin, ref _exposureTimeMax, ref _exposureTimeStep, ref _exposureTimeNow);
            }

            if (_exposureTimeMin > value || _exposureTimeMax < value)
            {
                return false;
            }

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "exposure_us", value);
                return true;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
        }

        protected override bool executeTrigger()
        {
            if (_tmNowTriggerMode != TriggerMode.Software)
                return false;

            try
            {
                HOperatorSet.SetFramegrabberParam(_htAcqHandle, "do_force_trigger", "");
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override HalconCameraBase.TriggerMode GetTriggerMode()
        {
            if (!IsOpen)
                return TriggerMode.Unknown;

            return _tmNowTriggerMode;
        }

        public override bool SetTriggerMode(TriggerMode trig, bool bCaptureThread = true)
        {
            if (!IsOpen)
                return false;

            try
            {
                switch (trig)
                {
                    case TriggerMode.FreeRun:
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", "disable");
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "external_trigger", "false");
                        break;
                    case TriggerMode.Software:
                    case TriggerMode.Hardware:
                    case TriggerMode.CC1:
                    case TriggerMode.CC2:
                    case TriggerMode.CC3:
                    case TriggerMode.CC4:
                    case TriggerMode.Line1:
                    case TriggerMode.Line2:
                    case TriggerMode.Line3:
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "start_async_after_grab_async", "enable");
                        HOperatorSet.SetFramegrabberParam(_htAcqHandle, "external_trigger", "true");
                        break;
                }
                _tmNowTriggerMode = trig;
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }

            return true;
        }

    }
}
