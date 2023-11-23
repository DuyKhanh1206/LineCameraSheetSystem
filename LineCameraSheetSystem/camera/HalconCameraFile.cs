using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using HalconDotNet;

namespace HalconCamera
{
    public class HalconCameraFile : HalconCameraBase
    {
        private string _sDirPath = "";
        private string _seqFilePath = "";

        public HalconCameraFile(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            _enableExposureTime = false;
            _enableGain = false;
            _enableOffset = false;
            _enableTimeout = false;
            _enableLineRate = false;
            _enableTriggerDelay = false;

            IsAreaSensor = true;
            IsLineSensor = false;
            IsOffLine = true;
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
                    beginCaptureThread();
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    if (bCaptureThread == true)
                        beginCaptureThread();
                }
                else if (trig == TriggerMode.Software)
                {
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
            return true;
        }

        public override bool Open()
        {
            openParamFetch("Directory", "", out _sDirPath);

            if (_sDirPath == "")
                return false;
            _sDirPath += (_sDirPath[_sDirPath.Length - 1] != '\\') ? "\\" : "";
            _seqFilePath = _sDirPath + "image.seq";

            openParamFetch("ThreadSleepTime", 1, out _threadSleepTime);
            // すでにシーケンスファイルがある場合削除する
            try
            {
                File.Delete(_seqFilePath);
                string[] wk_saDirs = Directory.GetFiles(_sDirPath);
                using (FileStream wk_fs = new FileStream(_seqFilePath, FileMode.Create))
                {
                    using (StreamWriter wk_sw = new StreamWriter(wk_fs, Encoding.ASCII))
                    {
                        foreach (string s in wk_saDirs)
                        {
                            wk_sw.WriteLine(s);
                        }
                        wk_sw.Close();
                    }
                    wk_fs.Close();
                }

                // ファイルシーケンスを開く
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
                HTuple wk_htDevice = _dicOpenParams.GetValueOrDefault("device", new HTuple("default"));
                HTuple wk_htPort = _dicOpenParams.GetValueOrDefault("port", new HTuple(0));
                HTuple wk_htLineIn = _dicOpenParams.GetValueOrDefault("linein", new HTuple(0));

                HOperatorSet.OpenFramegrabber(
                    "File",                                    // 1
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
                    _seqFilePath,                            // 12
                    wk_htDevice,                                // 14
                    wk_htPort,                                  // 15
                    wk_htLineIn,                                // 16
                    out _htAcqHandle);                          // 17

                //HOperatorSet.OpenFramegrabber(
                //    "File",
                //    1,
                //    1,
                //    0,
                //    0,
                //    0,
                //    0,
                //    "default",
                //    -1,
                //    "rgb",
                //    -1,
                //    "default",
                //    _seqFilePath,
                //    "default",
                //    -1,
                //    -1,
                //    out _htAcqHandle);

                HObject img;
                HOperatorSet.GrabImage(out img, _htAcqHandle);
                HTuple w, h;
                HOperatorSet.GetImageSize(img, out w, out h);
                _imageWidth = w.I;
                _imageHeight = h.I;

                base.Open();
            }
            catch (HOperatorException oe)
            {
                TraceError(oe.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            return true;
        }

        public override bool Close()
        {
            bool bResult = base.Close();
            if (_seqFilePath != "")
                File.Delete(_seqFilePath);
            return bResult;
        }
    }

    public class HalconCameraFileMemory : HalconCameraBase
    {
        string _sDirPath;
        HObject[] _ahoObjs = null;
        int _iCaptCnt = 0;

        public HalconCameraFileMemory(int iIndex, string name, string description, string mirror)
            : base(iIndex, name, description, mirror)
        {
            _enableExposureTime = false;
            _enableGain = false;
            _enableOffset = false;
            _enableTimeout = false;
            _enableLineRate = false;
            _enableTriggerDelay = false;

            IsAreaSensor = false;
            IsLineSensor = false;
            IsOffLine = true;
        }

        public override bool Open()
        {
            openParamFetch("Directory", "", out _sDirPath);
            if (_sDirPath == "")
                return false;
            _sDirPath += (_sDirPath[_sDirPath.Length - 1] != '\\') ? "\\" : "";

            openParamFetch("ThreadSleepTime", 1, out _threadSleepTime);

            // ファイル数をカウントする
            string[] saDirs = Directory.GetFiles(_sDirPath);
            List<string> lstFiles = new List<string>();

            Regex reg = new Regex(@"\.(bmp|tiff?|png)$", RegexOptions.IgnoreCase);
            // 画像ファイルのみを抽出
            foreach (string s in saDirs)
            {
                if (reg.IsMatch(s))
                {
                    lstFiles.Add(s);
                }
            }

            if (lstFiles.Count == 0)
                return false;

            int iCount = lstFiles.Count;
            if (iCount > 20)
                iCount = 20;
            _ahoObjs = new HObject[iCount];
            try
            {
                for (int i = 0; i < lstFiles.Count && i < iCount; i++)
                {
                    HOperatorSet.ReadImage(out _ahoObjs[i], lstFiles[i]);
                }
            }
            catch (HOperatorException)
            {
                return false;
            }

            _htAcqHandle = new HTuple(0);
            _iCaptCnt = 0;

            // 画像ｻｲｽﾞ等を入れておく
            HTuple htImgWidth, htImgHeight;
            HTuple htChannels;
            try
            {
                HOperatorSet.GetImageSize(_ahoObjs[0], out htImgWidth, out htImgHeight);
                _imageWidth = htImgWidth.I;
                _imageHeight = htImgHeight.I;
                HOperatorSet.CountChannels(_ahoObjs[0], out htChannels);
                if (htChannels.I == 1)
                    _cmColor = ColorMode.Gray;
                else
                    _cmColor = ColorMode.RGB;
            }
            catch (HOperatorException)
            {
            }

            // トリガーモードをソフトウェアにしておく
            SetTriggerMode(TriggerMode.Software);

            base.Open();

            return true;
        }

        protected override bool executeTrigger()
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
                    //endCaptureThread();
                }

                if (IsHardTrigger(trig))
                {
                    _iCaptCnt = 0;
                    //beginCaptureThread();
                }
                else if (trig == TriggerMode.FreeRun)
                {
                    _iCaptCnt = 0;
                    //beginCaptureThread();
                }
                else if (trig == TriggerMode.Software)
                {
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

        public override bool getImage(out HObject img)
        {
            img = null;
            try
            {
                HOperatorSet.CopyObj(_ahoObjs[_iCaptCnt], out img, 1, -1);
                _iCaptCnt++;
                if (_iCaptCnt >= _ahoObjs.Length)
                    _iCaptCnt = 0;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }
        public bool getImageFileMemory(out HObject img)
        {
            return getImage(out img);
        }

        protected override bool endCaptureThread()
        {
            if (_captureThread == null)
                return false;
            _stop = true;
            do
            {
                _captureThread.Join(100);
            }
            while (_captureThread.ThreadState == ThreadState.Running);
            _captureThread = null;
            return true;
        }

        public override bool Close()
        {
            if (!IsOpen)
                return false;

            for (int i = 0; i < _ahoObjs.Length; i++)
            {
                _ahoObjs[i].Dispose();
            }

            try
            {
                //トリガモードをソフトに戻す
                if (_tmNowTriggerMode != TriggerMode.Software)
                {
                    SetTriggerMode(TriggerMode.Software);
                }
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
    }
}
