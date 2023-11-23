using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;
using Fujita.LightControl;
using HalconCamera;

#if !STANDALONE
using InspectionNameSpace;
#endif

namespace LineCameraSheetSystem
{

    public class clsAutoLightValueCalibration : IThreadSafeFinish
    {
        public delegate void LightValueProgressEventHandler(object sender, LightValueProgressEventArgs e);
        public delegate void LightValueEventHandler(object sender, LightValueEventArgs e);
        public delegate void ThreadEndEventHandler(object sender, EventArgs e);
        public delegate void DebugMessageEventHandler(object sender, DebugMessageEventArgs str);

#if DEBUG
        public class DebugInfoEventArgs : EventArgs
        {
            public enum ActionType
            {
                LightChange,
                ExposureCahnge,
                Average,
            }

            public ActionType ActType { get; private set; }
            public object EventData { get; private set; }

            public DebugInfoEventArgs(ActionType type, object data)
            {
                ActType = type;
                EventData = data;
            }
        }
        public delegate void DebugInfoEventHandle(object sender, DebugInfoEventArgs e);
        public event DebugInfoEventHandle DebugInfo;
#endif

        class Value
        {
            public Value(int iLightVal, double dMax)
            {
                LightValue = iLightVal;
                Maximum = dMax;
            }
            public int LightValue { get; private set; }
            public double Maximum { get; private set; }
//            List<CameraExposure> _lstCameraExposure = new List<CameraExposure>();
        }

        class ExpValue
        {
            public ExpValue(int iExposure, double dMax)
            {
                ExposureValue = iExposure;
                Maximum = dMax;
            }

            public int ExposureValue { get; private set; }
            public double Maximum { get; private set; }
        }

        class CameraExposure
        {
            int iVal1;
            public int Val1
            {
                get { return iVal1; }
                set
                {
                    iVal1 = value;
                }
            }
            int iVal2;
            public int Val2
            {
                get { return iVal2; }
                set
                {
                    iVal2 = value;
                }
            }
        }

        public enum ECameraSide
        {
            UpSide = 0,
            DownSide = 1,
        }

        public enum EEventType
        {
            Start,
            Finish,
            Cancel,
            Error,
            Message,
        }

        public class LightValueEventArgs : EventArgs
        {
            public EEventType EventType { get; private set; }
            public int LightValue { get; private set; }
            public string Message { get; private set; }
            public List<int> Exposure = new List<int>();

            public LightValueEventArgs(EEventType eventtype, string sMesssage = "", int iLightValue = 0 )
            {
                EventType = eventtype;
                Message = sMesssage;
                LightValue = iLightValue;
            }

            public LightValueEventArgs(EEventType eventtype, string sMesssage, int iLightValue, params int[] exposure )
            {
                EventType = eventtype;
                LightValue = iLightValue;
                Message = sMesssage;

                for (int i = 0; i < exposure.Length; i++)
                    Exposure.Add(exposure[i]);
            }
        }

        public class DebugMessageEventArgs : EventArgs
        {
            public ECameraSide eCameraSide { get; private set; }
            public string strMessage { get; private set; }
            public DebugMessageEventArgs(ECameraSide eCamSide, string strMsg)
            {
                eCameraSide = eCamSide;
                strMessage = strMsg;
            }
        }

        public enum EProgressType
        {
            Coarse_64,
            Coarse_32,
            Coarse_16,
            Coarse_8,
            Fine
        }

        public class LightValueProgressEventArgs : EventArgs
        {
            public int Upto { get; private set; }
            public LightValueProgressEventArgs(int iupto)
            {
                Upto = iupto;
            }
        }

        public event LightValueProgressEventHandler OnProgress;
        public event LightValueEventHandler OnEvent;
        public event ThreadEndEventHandler OnThreadEnd;
        public event DebugMessageEventHandler OnDebugMessage;
        public ECameraSide CameraSide { get; set; }
        public bool AutoLightFull { get; set; }

        public clsAutoLightCalibration.ELightingType LightingType;

        public int ResultLightValue{get; private set;}
        public double ResultCameraGain { get; private set; }
        List<int> _lstResultCameraExp = new List<int>();
        public List<int> ResultCameraExposures { get { return _lstResultCameraExp; } }

        private int _iExposureDecrease = 10;
        public int ExposureDecrease 
        {
            get { return _iExposureDecrease; }
            set
            {
                _iExposureDecrease = value;
            }
        }

        List<HalconCameraBase> _lstCamera = new List<HalconCameraBase>();
        public List<HalconCameraBase> ListCameras { get { return _lstCamera; } }
        // 現在の露光をとっておく
        List<Tuple<int, int, int>> _lstExposure = new List<Tuple<int, int, int>>();
        public bool AddCamera(HalconCameraBase cam)
        {
            if (cam == null)
                return false;

            if (_lstCamera.Contains(cam))
                return false;

            _lstCamera.Add(cam);

            // 現在の露光値を取っておく
            //int min = 0, max = 0, step = 0, now = 0;
            //cam.GetExposureTimeRange(ref min, ref max, ref step, ref now);
            //_lstExposure.Add( new Tuple<int,int,int>(min,max,now ));

            return true;
        }
        // カメラの露光値を設定する
        public bool ClearCamera()
        {
            for (int i = 0; i < _lstCamera.Count; i++)
            {
                _lstCamera[i].SetExposureTime(_lstExposure[i].Item3);
            }
            _lstCamera.Clear();
            _lstExposure.Clear();
            return true;
        }

        public int CameraNum
        {
            get { return _lstCamera.Count; }
        }


        int _iBaseLightValue = 0;
        public int BaseLightValue
        {
            get { return _iBaseLightValue; }
            set
            {
                _iBaseLightValue = value;
            }
        }
        List<ExpValue> _lstExposureValus = new List<ExpValue>();


        // 輝度値の結果リスト
        List<Value> _lstResultValue = new List<Value>();
        List<LightType> _lstLight = new List<LightType>();
        public bool AddLight(LightType light)
        {
            if (_lstLight.Contains(light))
                return false;
            _lstLight.Add(light);
            return true;
        }

        public void ClearLight()
        {
            for (int i = 0; i < _lstLight.Count; i++)
            {
                if (_lstLight[i] == null)
                    return;
            }

            // 後処理として照明をオフにする
            for (int i = 0; i < _lstLight.Count; i++)
            {
                _lstLight[i].LightOff();
            }

            _lstLight.Clear();
        }

        clsThreadSafeFinish _threadSafeFinish = null;

        private System.Threading.Thread _tThread = null;

        public int _iThreshold = 128;

        public int _iCaptureWait = 2;
        public int WaitCaptureCount
        {
            get { return _iCaptureWait; }
            set
            {
                if (value < 0)
                    value = 0;
                _iCaptureWait = value;
            }
        }
        public int OkCaptureCount { get; set; }
        public double GainUpStep { get; set; }
        public int GainMaxCount { get; set; }
        public int OkLowLimit { get; set; }
        public int OkHightLimit { get; set; }
        public int DetailupLevel { get; set; }

        //AutoInspection _autoInsp = null;
        //public void SetAutoInspection(AutoInspection autoInsp)
        //{
        //    _autoInsp = autoInsp;
        //}
#if !STANDALONE
        public void OnRefreshImage(object sender, RefreshImageEvent.RefreshImageEventArgs e)
        {
            _iCaptureCnt++;
            lock (_lstResultValue)
            {
//                _lstResultValue.Add(new Value(_iLightValue, e.SideDatas[(int)CameraCategory].Maximum));
                _lstResultValue.Add(new Value(_iLightValue, e.SideDatas[(int)CameraSide].Average));
                //if (OnDebugMessage != null)
                //    OnDebugMessage(this, new DebugMessageEventArgs(CameraSide,
                //        string.Format("OnRefreshImage() _iCaptureCnt={0} _iLightValue={1} Average={2}", _iCaptureCnt, _iLightValue, e.SideDatas[(int)CameraSide].Average)));
#if DEBUG
                if (DebugInfo != null)
                {
                    DebugInfo(this, new DebugInfoEventArgs(DebugInfoEventArgs.ActionType.Average, e.SideDatas[(int)CameraSide].Average));
                }
#endif
                if (_iStatus == 4 || _iStatus == 5)
                {
                    _lstExposureValus.Add(new ExpValue(_iExposureTime, e.SideDatas[(int)CameraSide].Average));
                }
            }
        }
#else
        public void OnRefreshImage(object sender, double dMax)
        {
            if (_iStatus == -1)
                return;

            _iCaptureCnt++;
            lock (_lstResultValue)
            {
#if DEBUG
                if ( DebugInfo != null)
                {
                    DebugInfo(this, new DebugInfoEventArgs(DebugInfoEventArgs.ActionType.Average, dMax));
                }
#endif
                _lstResultValue.Add(new Value(_iLightValue, dMax));

                if (_iStatus == 4 || _iStatus == 5)
                {
                    _lstExposureValus.Add(new ExpValue(_iExposureTime, dMax)); 
                }
#if DEBUG
                string sDebug = CameraCategory.ToString();
                if (_iStatus == 4 || _iStatus == 5)
                {
                    sDebug += "***";
                    if (_lstExposureValus.Count > 10)
                    {
                        for (int i = _lstExposureValus.Count - 10; i < _lstExposureValus.Count; i++)
                        {
                            sDebug += "[" + _lstExposureValus[i].ExposureValue.ToString() + "," + _lstResultValue[i].Maximum.ToString() + "]";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _lstExposureValus.Count; i++)
                        {
                            sDebug += "[" + _lstExposureValus[i].ExposureValue.ToString() + "," + _lstResultValue[i].Maximum.ToString() + "]";
                        }
                    }

                }
                else
                {
                    sDebug += "@@@";
                    if (_lstResultValue.Count > 10)
                    {
                        for (int i = _lstResultValue.Count - 10; i < _lstResultValue.Count; i++)
                        {
                            sDebug += "[" + _lstResultValue[i].LightValue.ToString() + "," + _lstResultValue[i].Maximum.ToString() + "]";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _lstResultValue.Count; i++)
                        {
                            sDebug += "[" + _lstResultValue[i].LightValue.ToString() + "," + _lstResultValue[i].Maximum.ToString() + "]";
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine(sDebug);
#endif
            }
        }
#endif


        public void ThreadSafeEnd()
        {
            _bStop = true;

            do
            {
                _tThread.Join(100);
            } while (_tThread.IsAlive);
        }

        public bool Start()
        {
            if (_tThread != null)
                return false;

            //if (_autoInsp == null)
            //    return false;

            // 結果のクリア
            _lstResultValue.Clear();

            // イベントセット
//            _autoInsp.ClearRefreshImageEvent();
//            _autoInsp.SetRefreshImageEvent(OnRefreshImage);

            this.OnThreadEnd += new ThreadEndEventHandler(clsAutoLightValueCalibration_OnThreadEnd);

            if (!AutoLightFull)
            {
#if OFFLINE_AUTOLIGHT
                _tThread = new System.Threading.Thread(autoLightValueDummy);
#else
                _tThread = new System.Threading.Thread(autoLightValue);
#endif
                _tThread.Name = "AutoLightValue:" + CameraSide.ToString();

            }
            else
            {
            }

            _tThread.Start();

            return true;
        }

        void clsAutoLightValueCalibration_OnThreadEnd(object sender, EventArgs e)
        {
//            _autoInsp.ClearRefreshImageEvent();
            // 検査終了かエラーによるスレッド終了
            _tThread = null;
        }

        bool _bStop = false;
        public bool Stop()
        {
            if (_tThread == null)
                return false;

            // リフレッシュイメージイベント終了
//            _autoInsp.ClearRefreshImageEvent();

            _threadSafeFinish = new clsThreadSafeFinish(this);
            _threadSafeFinish.OnThreadEnded += new ThreadEndedEventHandler(_threadSafeFinish_OnThreadEnded);
            _threadSafeFinish.SafeFinish();

            return true;
        }

        void _threadSafeFinish_OnThreadEnded(object sender, ThreadEndedEventArgs e)
        {
            _tThread = null;
        }

        private void lightOn(int iValue)
        {
            for (int i = 0; i < _lstLight.Count; i++)
            {
                if (_lstLight[i] == null)
                    return;
            }

            _iLightValue = iValue;

            for (int i = 0; i < _lstLight.Count; i++)
            {
                _lstLight[i].LightOn255(iValue+ SystemParam.GetInstance().MainteLightOffset[i], true); //V1058 メンテナンス追加 yuasa 20190128：オフセット追加
            }

#if DEBUG
            if( DebugInfo != null )
            {
                DebugInfo( this, new DebugInfoEventArgs( DebugInfoEventArgs.ActionType.LightChange, iValue ));
            }
#endif
        }

        // 内容を0-255にマッピングする
        private int lightValueMapping255(int iMin, int iMax, int iValue)
        {
            if (iValue < 0)
                iValue = 0;
            if (iValue > 255)
                iValue = 255;
            if (iValue == 255)
                return iMax;

            // 256階調ならばその値をそのまま返す
            if (iMin == 0 && iMax == 255)
                return iValue;

            // 256階調->実階調変換
            int iRange = iMax - iMin;
            double dStep = iRange / 255.0;
            int iRetValue = (int)(iMin + dStep * iValue);
            return iRetValue;
        }


        int _iStatus = 0;
        int _iCaptureCnt = 0;
        private void resetCapture()
        {
            _iCaptureCnt = 0;
        }

        private int _cnt = 0;
        private int _iCapNum = 0;
        private bool waitCapture(int iCaptStep, out bool bJust)
        {
            bool ret = false;
            ret = valueCheck(out bJust);
            if (_iCapNum != _iCaptureCnt)
            {
                _cnt++;
                _iCapNum = _iCaptureCnt;
            }
            if (ret == true)
            {
                if (_cnt >= OkCaptureCount)
                {
                    _cnt = 0;
                    _iCapNum = 0;
                    _iCaptureCnt = 0;
                }
                else
                {
                    ret = false;
                }
            }
            else
            {
                //if (_cnt >= OkCaptureCount)
                //{
                //    _cnt = 0;
                //    _iCapNum = 0;
                //    _iCaptureCnt = 0;
                //    ret = true;
                //}
            }
            if (_iCaptureCnt >= iCaptStep)
            {
                _cnt = 0;
                ret = true;
            }
            return ret;
        }
        private bool waitCapture(int iCaptStep)
        {
            bool ret = false;
            ret = valueCheck();
            if (_iCapNum != _iCaptureCnt)
            {
                _cnt++;
                _iCapNum = _iCaptureCnt;
            }
            if (ret == true)
            {
                if (_cnt >= OkCaptureCount)
                {
                    _cnt = 0;
                    _iCapNum = 0;
                    _iCaptureCnt = 0;
                }
                else
                {
                    ret = false;
                }
            }
            else
            {
                //if (_cnt >= OkCaptureCount)
                //{
                //    _cnt = 0;
                //    _iCapNum = 0;
                //    _iCaptureCnt = 0;
                //    ret = true;
                //}
            }
            if (_iCaptureCnt >= iCaptStep)
            {
                _cnt = 0;
                ret = true;
            }
            return ret;
        }

        //int _baseCheckLimit = 5;

        // 現在の最新値とその前の最終値を比べ、128をまたいでいるかどうかを判断する]
        private bool valueCheck(out bool bJust)
        {
            bJust = false;
            lock (_lstResultValue)
            {
                // カウントが1未満の場合
                if (_lstResultValue.Count <= 1)
                    return false;

                int iTargetval = _lstResultValue[_lstResultValue.Count - 1].LightValue;
                double dMaximum = _lstResultValue[_lstResultValue.Count - 1].Maximum;
                if (dMaximum >= (_iThreshold + OkLowLimit) && dMaximum <= (_iThreshold + OkHightLimit))
                {
                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "**自動調光 valueCheck-just[" + iTargetval.ToString() + "," + dMaximum.ToString("F1") + "]");
                    System.Diagnostics.Debug.WriteLine(CameraSide.ToString() + "**自動調光 valueCheck-just[" + iTargetval.ToString() + "," + dMaximum.ToString("F1") + "]");
                    bJust = true;
                    return true;
                }

                if (dMaximum > _iThreshold)
                    return true;

                // しきい値未満の場合
                if (dMaximum < _iThreshold)
                    return false;

                for (int i = _lstResultValue.Count - 1; i >= 0; i--)
                {
                    // 最初に値が変化したところでの輝度値が
                    if (_lstResultValue[i].LightValue != iTargetval)
                    {
                        if (_lstResultValue[i].Maximum < 128)
                        {
                            int iPrevLightValue = _lstResultValue[i].LightValue;
                            double dPrevMaximum = _lstResultValue[i].Maximum;
                            LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "**自動調光 valueCheck-[" + iTargetval.ToString() + "," + dMaximum.ToString("F1") + "]->[" + iPrevLightValue.ToString() + "," + dPrevMaximum.ToString("F1") + "]");
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        private bool valueCheck()
        {
            lock (_lstResultValue)
            {
                // カウントが1未満の場合
                if (_lstResultValue.Count <= 1)
                    return false;

                int iTargetval = _lstResultValue[_lstResultValue.Count - 1].LightValue;
                double dMaximum = _lstResultValue[_lstResultValue.Count - 1].Maximum;

                // しきい値未満の場合
                if (dMaximum < _iThreshold)
                    return false;

                for (int i = _lstResultValue.Count - 1; i >= 0; i--)
                {
                    // 最初に値が変化したところでの輝度値が
                    if (_lstResultValue[i].LightValue != iTargetval)
                    {
                        if (_lstResultValue[i].Maximum < 128)
                            return true;
                    }
                }
                return false;
            }
        }


        // 現在の最新値とその前の最終値を比べ、128をまたいでいるかどうかを判断する]
        private bool valueCheckExposure(double dThreshold)
        {
            lock (_lstExposureValus)
            {
                // カウントが1未満の場合
                if (_lstExposureValus.Count <= 1)
                    return false;

                // 最新の値
                int iTargetval = _lstExposureValus[_lstExposureValus.Count - 1].ExposureValue;
                double dMaximum = _lstExposureValus[_lstExposureValus.Count - 1].Maximum;

                // しきい値未満の場合
                if (dMaximum > dThreshold)
                    return false;

                // 一つ前の値
                for (int i = _lstExposureValus.Count - 1; i >= 0; i--)
                {
                    // 最初に値が変化したところでの輝度値が
                    if (_lstExposureValus[i].ExposureValue != iTargetval)
                    {
                        if (_lstExposureValus[i].Maximum > 128)
                        {
                            LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "**自動調光 valueCheckExposure-[" + iTargetval.ToString() + "," + dMaximum.ToString("F1") + "]");
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        int _iLightValue = 0;
        private void autoLightValue()
        {
            // 照明のステップを取得する
            //int iLightRange;
            //if (_lstLight[0] != null)
            //    iLightRange = _lstLight[0].ValueMax - _lstLight[0].ValueMin + 1;
            //else
            //    iLightRange = 255;

            int[] iCoarseStep = new int[]{
            64,
            32,
            16,
            8,
            };
            _cnt = 0;
            _iCapNum = 0;

            //int icLightMinVal = 0;
            //int icLightMaxVal = 0;

            int iDivIndex = 0;
            int iStepValue = 0;
            int iFineStartValue = 0;
            int iFineEndValue = 0;
            int iStartValue;

            int iLightVal;

            if (_lstLight[0] != null)
            {
                iStartValue = _lstLight[0].ValueMin;
            }
            else
            {
                iStartValue = 0;
            }
            int iEndValue = 256;

            double dDefaultGain = _lstCamera[0].GetDefaultGain();
            ResultCameraGain = dDefaultGain;
            _lstCamera[0].SetGain(dDefaultGain);
            int iGainUpCnt = 0;

            // スレッド開始イベント
            if (OnEvent != null)
                OnEvent(this, new LightValueEventArgs(EEventType.Start, "照明調光調整中"));
            //// スレッド開始イベント
            //if (OnEvent != null)
            //    OnEvent(this, new LightValueEventArgs(EEventType.Start));
            bool bJust;
            try
            {
                while (!_bStop)
                {
                    // 粗サーチ
                    switch (_iStatus)
                    {
                        case 0:
                            LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "_iStatus : " + _iStatus.ToString());
                            iLightVal = iStartValue + iCoarseStep[iDivIndex] * iStepValue;
                            iLightVal = (iLightVal > 0) ? iLightVal - 1 : 0;
                            lightOn(iLightVal);
                            resetCapture();
                            _iStatus = 1;
                            if (OnDebugMessage != null)
                                OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("粗サーチ 照明値：{0}", iLightVal)));
                            break;

                        case 1:
                            if (waitCapture(WaitCaptureCount, out bJust))
                            {
                                LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "_iStatus : " + _iStatus.ToString());

                                // しきい値を超えた場合
                                if (valueCheck(out bJust ))
                                {
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "しきい値を超えた" + ":bJust=" + bJust.ToString());
                                    if (bJust)
                                    {
                                        iFineStartValue = iStartValue + iCoarseStep[iDivIndex] * iStepValue - 4;
                                        iFineEndValue = iFineStartValue + 16;
                                        iStepValue = 0;
                                        _iStatus = 2;
                                        if (OnProgress != null)
                                            OnProgress(this, new LightValueProgressEventArgs(20));
                                        if (OnDebugMessage != null)
                                            OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("粗サーチ完了１")));
                                    }
                                    else
                                    {
                                        iStartValue = iStartValue + iCoarseStep[iDivIndex] * ( iStepValue - 1 );
                                        iEndValue = iStartValue + iCoarseStep[iDivIndex] * ( iStepValue );

                                        // 粗サーチが終わったので、次のステップへ
                                        if (iCoarseStep.Length <= iDivIndex + 1 || iStartValue == iEndValue)
                                        {
                                            iFineStartValue = iStartValue;
                                            iFineEndValue = iStartValue + iCoarseStep[iDivIndex] * ((iStepValue == 0) ? 1 : iStepValue);
                                            iStepValue = 0;
                                            _iStatus = 2;
                                            if (OnProgress != null)
                                                OnProgress(this, new LightValueProgressEventArgs(20));
                                            if (OnDebugMessage != null)
                                                OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("粗サーチ完了２")));
                                        }
                                        else
                                        {
                                            iDivIndex++;
                                            iStepValue = 0;
                                            _iStatus = 0;
                                            if (OnProgress != null)
                                                OnProgress(this, new LightValueProgressEventArgs(20));
                                        }
                                    }
                                }
                                else
                                {
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "しきい値を超えてない");
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "iStartValue:" + iStartValue.ToString());
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "iDivIndex:" + iDivIndex.ToString());
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "iStepValue:" + iStepValue.ToString());
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "iEndValue:" + iEndValue.ToString());
                                    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "iCoarseStep[iDivIndex]:" + iCoarseStep[iDivIndex].ToString());
                                    _iStatus = 0;
                                    iStepValue++;
                                    // 輝度値がマックスを超えた場合エラーを返して抜ける
                                    if (iStartValue + iCoarseStep[iDivIndex] * iStepValue > iEndValue )
                                    {
                                        if (iGainUpCnt < GainMaxCount)
                                        {
                                            iGainUpCnt++;
                                            ResultCameraGain = dDefaultGain + (GainUpStep * iGainUpCnt);
                                            _lstCamera[0].SetGain(ResultCameraGain);
                                            _iStatus = 0;
                                            iStepValue = 0;
                                            iDivIndex = 0;
                                            break;
                                        }

                                        if (OnEvent != null)
                                        {
                                            string sideName;
                                            sideName = (CameraSide == ECameraSide.UpSide) ? "(おもて　おもて　おもて)" : "(うら　うら　うら)";

                                            if (iDivIndex == 0)
                                            {
                                                OnEvent(this, new LightValueEventArgs(EEventType.Error, "輝度が足りません" + sideName));
                                                LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "輝度が足りません");
                                            }
                                            else
                                            {
                                                OnEvent(this, new LightValueEventArgs(EEventType.Error, "粗サーチの輝度調整に失敗しました" + sideName));
                                                LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "粗サーチの輝度調整に失敗しました[001]");
                                            }
                                        }
                                        return;
                                    }
                                }
                            }
                            break;
                        case 2: // 精サーチを行う
                            LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "_iStatus : " + _iStatus.ToString());

                            System.Diagnostics.Debug.WriteLine(iFineStartValue + iStepValue);
                            iLightVal = iFineStartValue + iStepValue;
                            iLightVal = (iLightVal > 0) ? iLightVal - 1 : 0;
                            lightOn(iLightVal);
                            resetCapture();
                            _iStatus = 3;
                            if (OnDebugMessage != null)
                                OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("詳細サーチ 照明値：{0}", iLightVal)));
                            break;
                        case 3:
                            if (waitCapture(WaitCaptureCount))
                            {
                                LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "_iStatus : " + _iStatus.ToString());

                                if (valueCheck() || ((iFineStartValue + iStepValue) > iFineEndValue))
                                {
                                    ResultLightValue = iFineStartValue + iStepValue - 1;

                                    if (OnEvent != null)
                                        OnEvent(this, new LightValueEventArgs(EEventType.Finish, "", iFineStartValue + iStepValue - 1));
                                    if (OnDebugMessage != null)
                                        OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("詳細サーチ完了")));
                                    return;
                                }
                                else
                                {
                                    _iStatus = 2;
                                    iStepValue += DetailupLevel;
                                    if (OnProgress != null)
                                        OnProgress(this, new LightValueProgressEventArgs(1));
                                    //if (iFineStartValue + iStepValue > iFineEndValue)
                                    //{
                                    //    if (OnEvent != null)
                                    //        OnEvent(this, new LightValueEventArgs(EEventType.Error, "詳細サーチの輝度調整に失敗しました"));
                                    //    LogingDllWrap.LogingDll.Loging_SetLogString(CameraSide.ToString() + "詳細サーチの輝度調整に失敗しました[002]");
                                    //    if (OnDebugMessage != null)
                                    //        OnDebugMessage(this, new DebugMessageEventArgs(CameraSide, string.Format("詳細サーチ失敗")));
                                    //    return;
                                    //}
                                }
                            }
                            break;
                    }
                    System.Threading.Thread.Sleep(10);
                }

                if (OnEvent != null)
                    OnEvent(this, new LightValueEventArgs(EEventType.Cancel, ""));
            }
            catch (Exception e)
            {
                OnEvent(this, new LightValueEventArgs(EEventType.Error, e.Message));
            }
            finally
            {
                // ユーザーキャンセルによるストップじゃない場合はOnThreadEndイベントを発生させる
                if (OnThreadEnd != null && !_bStop)
                    OnThreadEnd(this, new EventArgs());
            }
        }

        int _iExposureTime = 0;
        private bool cameraExposureChange(int iOffset)
        {
            for (int i = 0; i < _lstCamera.Count; i++)
            {
                if (_lstExposure[i].Item3 - iOffset < _lstExposure[i].Item1)
                {
                    return false;
                }
            }

            for (int i = 0; i < _lstCamera.Count; i++)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("camera exposuure change" + (_lstExposure[i].Item3 - iOffset).ToString());
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
#endif
                _lstCamera[i].SetExposureTime(_lstExposure[i].Item3 - iOffset);
#if DEBUG
                System.Diagnostics.Debug.WriteLine("exposure change time = " + sw.Elapsed.ToString());
#endif
                _iExposureTime = _lstExposure[i].Item3 - iOffset;
#if DEBUG
                if (DebugInfo != null)
                {
                    DebugInfo(this, new DebugInfoEventArgs(DebugInfoEventArgs.ActionType.ExposureCahnge, new int[]{i,(_lstExposure[i].Item3 - iOffset)}));
                }
#endif
            }

            return true;
        }

        private void autoLightValueDummy()
        {
            Random rnd = new Random((int)DateTime.Now.ToFileTime());
            System.Threading.Thread.Sleep(3000 + rnd.Next(2000));
            ResultLightValue = rnd.Next(255);

            if (OnEvent != null)
            {
                OnEvent(this, new LightValueEventArgs(EEventType.Finish, "", ResultLightValue ));
            }

        }

        private void autoLightValueFullDummy()
        {
            Random rnd = new Random((int)DateTime.Now.ToFileTime());
            System.Threading.Thread.Sleep(3000 + rnd.Next(2000));
            ResultLightValue = rnd.Next(256);

            for (int i = 0; i < _lstExposure.Count; i++)
                _lstResultCameraExp.Add(rnd.Next(50) + 50);

            if (OnEvent != null)
            {
                OnEvent(this, new LightValueEventArgs(EEventType.Finish, "", ResultLightValue, _lstResultCameraExp.ToArray()));
            }
        }
    }
}
