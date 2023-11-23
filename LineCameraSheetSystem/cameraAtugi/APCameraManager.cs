using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;
using Fujita.Communication;
using Fujita.Misc;
using LineCameraSheetSystem;
using System.Threading;
using System.Collections;
using InspectionNameSpace;

namespace HalconCamera
{
    class APCameraManager : IThreadSafeFinish
    {
        private static APCameraManager _instance = new APCameraManager();
        public static APCameraManager getInstance()
        {
            return _instance;
        }

        /// <summary>
        /// カメラの初期化
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool Initialize()
        {
            return CameraManager.getInstance().Initialize();

        }

        /// <summary>
        /// カメラの終了処理
        /// </summary>
        /// <returns></returns>
        public bool Terminate()
        {
            return CameraManager.getInstance().Terminate();
        }

        public string LastErrorMessage { get { return CameraManager.getInstance().LastErrorMessage; } }

        /// <summary>
        /// 同期動作用コマンド
        /// </summary>
        SyncLiveCommand _syncLiveCmd = null;
        /// <summary>
        /// 設定のロード
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sSection"></param>
        /// <returns></returns>
        public bool Load(string sPath, string sSection)
        {

            IniFileAccess ifa = new IniFileAccess();

            bool bResult = CameraManager.getInstance().Load(sPath, "");

            bool bLiveCmd = ifa.GetIni("LiveTrigger", "Enable", false, sPath);
            if (bLiveCmd)
            {
                string sDeviceName = ifa.GetIni("LiveTrigger", "CommunicationDioName", "", sPath);
                CommunicationDIO dio = CommunicationManager.getInstance().getCommunicationDIO(sDeviceName);
                int iDioNumber = ifa.GetIni("LiveTrigger", "DioNumber", -1, sPath);

                if (bResult && dio != null && iDioNumber != -1)
                {
                    _syncLiveCmd = new SyncLiveCommand(dio, iDioNumber);
                    CameraManager.getInstance().SetLiveCommand(_syncLiveCmd);
                }
                else
                {
                    bResult = false;
                }
            }
            return bResult;
        }

        public bool StartLive(int[] aiCams = null)
        {
            return CameraManager.getInstance().Live(true, aiCams);
        }

        public bool StopLive(int[] aiCams = null)
        {
            return CameraManager.getInstance().Live(false, aiCams);
        }

        public bool StartHardTrig(int[] aiCams = null)
        {
            return CameraManager.getInstance().HardTirgger(true, HalconCameraBase.TriggerMode.CC1, aiCams);
        }

        public bool StopHardTrig(int[] aiCams = null)
        {
            return CameraManager.getInstance().HardTirgger(false, HalconCameraBase.TriggerMode.CC1, aiCams);
        }

        public HalconCameraBase GetCamera(string sName)
        {
            return CameraManager.getInstance().GetCamera(sName);
        }

        public HalconCameraBase GetCamera(int iIndex)
        {
            return CameraManager.getInstance().GetCamera(iIndex);
        }

        public int CameraNum
        {
            get
            {
                return CameraManager.getInstance().CameraNum;
            }
        }

        public bool IsLive()
        {
            if (_tCaptureThread1 != null || _tCaptureThread2 != null)
                return true;
            if (_safeFinish != null)
                return true;
            return false;
        }

        bool _bStop = false;
        System.Threading.Thread _tCaptureThread1 = null;
        System.Threading.Thread _tCaptureThread2 = null;
        List<HalconCameraBase> _lstTargetCams = new List<HalconCameraBase>();

        System.Threading.Thread _tGetImageThread1 = null;
        System.Threading.Thread _tGetImageThread2 = null;

        /// <summary>
        /// １つのスレッド動作によるキャプチャの開始
        /// </summary>
        /// <param name="cams"></param>
        /// <returns></returns>
        public bool SyncStartLive(int[] cams = null)
        {
            // すでに起動済み
            if (_tCaptureThread1 != null || _tCaptureThread2 != null)
                return false;

            // 非同期ライブ停止処理が終わっていない
            if (_safeFinish != null)
                return false;

            if (_syncLiveCmd != null)
            {
                _syncLiveCmd.PrevCommand(true);
            }

            // 使用するカメラを生成
            for (int i = 0; i < AppData.CAM_COUNT; i++)
            {
                if (cams == null || Array.IndexOf(cams, i) != -1)
                {
                    _lstTargetCams.Add(APCameraManager.getInstance().GetCamera(SystemParam.GetInstance().camParam[i].CamNo));
                }
            }

            List<bool> abResult = new List<bool>(new bool[_lstTargetCams.Count]);
            abResult.ForEach(x => x = true);

            // トリガモードを変更してオープンする
            for (int i = 0; i < CameraNum; i++)
            {
                HalconCameraBase fileCam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFile;
                if (fileCam != null)
                {
                    fileCam.SetTriggerMode(HalconCameraBase.TriggerMode.FreeRun, false);
                }
                else
                {
                    HalconCameraBase fileMemoryCam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFileMemory;
                    if (fileMemoryCam != null)
                    {
                    }
                    else
                    {
                        HalconCameraBase cam = APCameraManager.getInstance().GetCamera(i);
                        HOperatorSet.SetFramegrabberParam(cam.AcqHandle, "do_abort_grab", 0);
                        try
                        {
                            HOperatorSet.SetFramegrabberParam(cam.AcqHandle, "continuous_grabbing", "enable");
                        }
                        catch
                        {
                        }
                        cam.SetTriggerMode(HalconCameraBase.TriggerMode.FreeRun, false);
                    }
                }
            }

            List<CameraParam> camp = SystemParam.GetInstance().camParam;
            bool[] updown = new bool[CameraNum];
            int[][] enableCam = new int[CameraNum][];
            int[] cntNo = new int[CameraNum];
            for (int camNo = 0; camNo < CameraNum; camNo++)
            {
                enableCam[camNo] = new int[] { -1, -1, -1, -1 };
                //Loop=4
                for (int i = 0; i < camp.Count; i++)
                {
                    if (camp[i].OnOff == true)
                    {
                        if ((int)camp[i].CamParts == camNo)
                        {
                            enableCam[camNo][cntNo[camNo]] = camNo;
                            cntNo[camNo]++;
                            updown[camNo] = true;
                        }
                    }
                }
            }

            QueueImage = new QueuingImage[updown.Length];

            // キャプチャ用スレッドを立ち上げる
            _bStop = false;
            for (int i = 0; i < updown.Length; i++)
            {
                if (updown[i] == true)
                {
                    QueueImage[i] = new QueuingImage(i, 0);
                    if (i == 0)
                    {
                        //イメージ取得後
                        _tGetImageThread1 = new Thread(new ParameterizedThreadStart(getImageThread));
                        _tGetImageThread1.Name = "Getｲﾒｰｼﾞ" + (i + 1).ToString();
                        //イメージCapture
                        _tCaptureThread1 = new Thread(new ParameterizedThreadStart(captureThread));
                        _tCaptureThread1.Name = "ﾏﾙﾁｷｬﾌﾟﾁｬ-" + (i + 1).ToString();
                    }
                    else
                    {
                        //イメージ取得後
                        _tGetImageThread2 = new Thread(new ParameterizedThreadStart(getImageThread));
                        _tGetImageThread2.Name = "Getｲﾒｰｼﾞ" + (i + 1).ToString();
                        //イメージCapture
                        _tCaptureThread2 = new Thread(new ParameterizedThreadStart(captureThread));
                        _tCaptureThread2.Name = "ﾏﾙﾁｷｬﾌﾟﾁｬ-" + (i + 1).ToString();
                    }
                }
            }
            for (int i = 0; i < updown.Length; i++)
            {
                if (updown[i] == true)
                {
                    if (i == 0)
                        _tGetImageThread1.Start(new int[] { 0, 0 });
                    else
                        _tGetImageThread2.Start(new int[] { 1, 1 });
                }
            }
            for (int i = 0; i < updown.Length; i++)
            {
                if (updown[i] == true)
                {
                    if (i == 0)
                        _tCaptureThread1.Start(new int[] { 0, 0 });
                    else
                        _tCaptureThread2.Start(new int[] { 1, 1 });
                }
            }

            System.Threading.Thread.Sleep(2000);
            if (_syncLiveCmd != null)
            {
                _syncLiveCmd.AfterCommand(true);
            }

            return true;
        }

        clsThreadSafeFinish _safeFinish = null;

        /// <summary>
        /// １つのスレッド動作によるキャプチャ停止
        /// </summary>
        /// <returns></returns>
        public bool SyncStopLive(bool bWaitStop = true)
        {
            if (_tCaptureThread1 == null && _tCaptureThread2 == null)
                return false;

            _safeFinish = new clsThreadSafeFinish(this);
            _safeFinish.OnThreadEnded += _safeFinish_OnThreadEnded;
            _safeFinish.SafeFinish(bWaitStop);
            return true;
        }

        public delegate void LiveFinishCompleteEventHandler(object sender, EventArgs e);
        public event LiveFinishCompleteEventHandler LiveFinishComplete = null;

        void _safeFinish_OnThreadEnded(object sender, ThreadEndedEventArgs e)
        {
            _safeFinish.OnThreadEnded -= _safeFinish_OnThreadEnded;
            _safeFinish = null;

            if (LiveFinishComplete != null)
            {
                LiveFinishComplete(this, new EventArgs());
            }
        }

        public void ThreadSafeEnd()
        {
            if (_syncLiveCmd != null)
            {
                _syncLiveCmd.PrevCommand(false);
            }

            _bStop = true;

            for (int i = 0; i < CameraNum; i++)
            {
                HalconCameraBase fileCam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFile;
                if (fileCam != null)
                {
                    fileCam.SetTriggerMode(HalconCameraBase.TriggerMode.FreeRun, false);
                }
                else
                {
                    HalconCameraBase fileMemoryCam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFileMemory;
                    if (fileMemoryCam != null)
                    {
                    }
                    else
                    {
                        try
                        {
                            HalconCameraSaperaLTDALSALineSensor dalsaCam = APCameraManager.getInstance().GetCamera(i) as HalconCameraSaperaLTDALSALineSensor;
                            if (dalsaCam != null)
                            {
                                dalsaCam.AbortGrab();
                            }
                            else
                            {
                                HalconCameraBase cam = APCameraManager.getInstance().GetCamera(i);
                                HOperatorSet.SetFramegrabberParam(cam.AcqHandle, "do_abort_grab", 1);
                                HOperatorSet.SetFramegrabberParam(cam.AcqHandle, "continuous_grabbing", "disable");
                                cam.SetTriggerMode(HalconCameraBase.TriggerMode.FreeRun, false);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            System.Threading.Thread.Sleep(2000);

            // スレッドを終了させる
            if (_tCaptureThread1 != null)
            {
                do
                {
                    _tGetImageThread1.Join(100);
                } while (_tGetImageThread1.IsAlive);
                _tGetImageThread1 = null;
                do
                {
                    _tCaptureThread1.Join(100);
                } while (_tCaptureThread1.IsAlive);
                _tCaptureThread1 = null;

                QueueImage[0].Dispose();
            }

            if (_tCaptureThread2 != null)
            {
                do
                {
                    _tGetImageThread2.Join(100);
                } while (_tGetImageThread2.IsAlive);
                _tGetImageThread2 = null;
                do
                {
                    _tCaptureThread2.Join(100);
                } while (_tCaptureThread2.IsAlive);
                _tCaptureThread2 = null;

                QueueImage[1].Dispose();
            }

            List<bool> abResult = new List<bool>(new bool[_lstTargetCams.Count]);
            abResult.ForEach(x => x = true);

            // すべてをクローズ状態にする
            //for (int i = 0; i < _lstTargetCams.Count; i++)
            //{
            //    abResult[i] |= _lstTargetCams[i].Close();
            //}

            // トリガモードを変更してオープンする
            for (int i = 0; i < CameraNum; i++)
            {
                HalconCameraBase cam = APCameraManager.getInstance().GetCamera(i);
                cam.SetTriggerMode(HalconCameraBase.TriggerMode.Software);
            }

            if (_syncLiveCmd != null)
            {
                _syncLiveCmd.AfterCommand(false);
            }

            _lstTargetCams.Clear();

        }

        public bool ResetSyncTrigger()
        {
            if (_syncLiveCmd == null)
                return false;

            if ((_tCaptureThread1 == null && _tCaptureThread2 == null) || _safeFinish != null)
                return false;

            _syncLiveCmd.PrevCommand(false);

            return true;
        }

        public bool SetSyncTrigger()
        {
            if (_syncLiveCmd == null)
                return false;

            if ((_tCaptureThread1 == null && _tCaptureThread2 == null) || _safeFinish != null)
                return false;

            _syncLiveCmd.AfterCommand(true);

            return true;
        }

        private int _iBufferCount = 10;
        public int BufferCount
        {
            get { return _iBufferCount; }
            set
            {
                if (value < 1)
                    return;
                _iBufferCount = value;
            }
        }


        public class CaptureImageEventArgs
        {
            public int CamIndex { get; private set; }
            public int CaptureCount { get; private set; }
            public int FailCount { get; private set; }
            public CaptureImageEventArgs(int iCamIndex, int iCaptureCount, int iFailCnt)
            {
                CamIndex = iCamIndex;
                CaptureCount = iCaptureCount;
                FailCount = iFailCnt;
            }
        }
        public delegate void CaptureImageEventHandler(object sender, CaptureImageEventArgs e);
        public event CaptureImageEventHandler OnCaptureThread = null;

        public QueuingImage[] QueueImage { get; private set; }

        void captureThread(object obj)
        {
            int[] CameraNumbers = (int[])obj;
            System.Threading.Thread.Sleep(3000);

            int iCamIndex = CameraNumbers[0];
            int iCamInfoIndex = CameraNumbers[1];

            HObject hoMirrorImage;
            HOperatorSet.GenEmptyObj(out hoMirrorImage);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            HTuple htTotalCount;
            HTuple htFailCount;

            int totalCnt = 0;
            int failCnt = 0;
            int[] bufFailCount = new int[CameraNum];
            while (!_bStop)
            {
                try
                {
                    HObject hoImage;

                    HalconCameraBase cam = APCameraManager.getInstance().GetCamera(iCamIndex) as HalconCameraFileMemory;
                    if (cam != null)
                    {
                        cam.getImage(out hoImage);
                    }
                    else
                    {
                        cam = APCameraManager.getInstance().GetCamera(iCamIndex);
                        lock (cam.AcqHandle)
                        {
                            System.Diagnostics.Debug.WriteLine("APCameraManager[] getImage() START");
                            HOperatorSet.GrabImageAsync(out hoImage, cam.AcqHandle, -1);
                            //グラッブした時間を保持
                            cam.GrabLastUpDate = DateTime.Now;//v1328
                            System.Diagnostics.Debug.WriteLine("now"+DateTime.Now);
                            System.Diagnostics.Debug.WriteLine("APCameraManager[] getImage() END");
                        }
                        if (cam.Description.ToLower() != "file")
                        {
                            try
                            {
                                string sDesc = cam.Description.ToLower();
                                if (sDesc.IndexOf("dalsa") < 0)
                                {
                                    HOperatorSet.GetFramegrabberParam(cam.AcqHandle, "Statistic_Total_Buffer_Count", out htTotalCount);
                                    HOperatorSet.GetFramegrabberParam(cam.AcqHandle, "Statistic_Failed_Buffer_Count", out htFailCount);
                                    totalCnt = htTotalCount.I;
                                    failCnt = htFailCount.I;
                                }
                                else
                                {
                                    totalCnt++;
                                    failCnt = 0;
                                }
                                string str = string.Format("cam:{0} Statistic_Total_Buffer_Count={1}  Statistic_Failed_Buffer_Count={2}", iCamIndex, totalCnt, failCnt);
                                //Console.WriteLine(str);
                                if (failCnt != bufFailCount[iCamIndex])
                                {
                                    LogingDllWrap.LogingDll.Loging_SetLogString(str);
                                    bufFailCount[iCamIndex] = failCnt;
                                }
                            }
                            catch (HOperatorException)
                            {
                                System.Diagnostics.Debug.WriteLine("APCameraManager[] getImage() HOperatorException");
                            }
                        }
                    }
                    sw.Restart();

                    if (hoImage == null)
                    {
                        LogingDllWrap.LogingDll.Loging_SetLogString("capture thread exception [hoImage = null]");
                    }
                    else
                    {
                        if (cam.Mirror.ToLower() == "row" || cam.Mirror.ToLower() == "column")
                        {
                            hoMirrorImage.Dispose();
                            HOperatorSet.MirrorImage(hoImage, out hoMirrorImage, cam.Mirror.ToLower());
                            hoImage.Dispose();
                            HOperatorSet.CopyObj(hoMirrorImage, out hoImage, 1, -1);
                        }

                        QueueImage[iCamIndex].Enqueue(hoImage, null, null);
                        if (OnCaptureThread != null)
                            OnCaptureThread(this, new CaptureImageEventArgs(iCamIndex, totalCnt, failCnt));
                    }

                    if (hoImage != null)
                        hoImage.Dispose();
                }
                catch (HOperatorException oe)
                {
                    if (oe.Message.IndexOf("#5336") < 0)
                    {
                        if (sw.ElapsedMilliseconds > 5000)
                        {
                            _lstTargetCams[iCamIndex].IsError = true;
                            _lstTargetCams[iCamIndex].ErrorReason = oe.Message;
                        }
                        // 例外発生
                        string msg = string.Format("captureThread() CamName={0} Msg={1}", APCameraManager.getInstance().GetCamera(iCamIndex).Name, oe.Message);
                        CaptureErrorLog(iCamIndex, msg);
                        System.Diagnostics.Debug.WriteLine(msg);
                        LogingDllWrap.LogingDll.Loging_SetLogString(msg);
                    }
                }
                finally
                {
                    hoMirrorImage.Dispose();
                }
                if (APCameraManager.getInstance().GetCamera(0) != null)
                    System.Threading.Thread.Sleep(APCameraManager.getInstance().GetCamera(0).ThreadSleepTime);
            }
        }


        public class InspectTimeEventArgs
        {
            public int CamIndex { get; private set; }
            public long ConnectImageTime { get; private set; }
            public long OnGrabbedTime { get; private set; }
            public long GetImageTotalTime { get; private set; }
            public int CaptureBufferCount { get; private set; }
            public InspectTimeEventArgs(int iCamIndex, long connectImage, long onGrabbed, long getImageTotal, int iCaptureBuffCount)
            {
                CamIndex = iCamIndex;
                ConnectImageTime = connectImage;
                OnGrabbedTime = onGrabbed;
                GetImageTotalTime = getImageTotal;
                CaptureBufferCount = iCaptureBuffCount;
            }
        }
        public delegate void InspectTimeEventHandler(object sender, InspectTimeEventArgs e);
        public event InspectTimeEventHandler OnGetImageThread = null;


        void getImageThread(object obj)
        {
            int[] CameraNumbers = (int[])obj;
            System.Threading.Thread.Sleep(3000);

            int iCamIndex = CameraNumbers[0];
            int iCamInfoIndex = CameraNumbers[1];

            System.Diagnostics.Stopwatch swGetImageTotal = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swConnectImage = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swOnGrabbed = new System.Diagnostics.Stopwatch();

            bool bSetFlag = false;
            int iThrowCount = 0;

            int[] bufFailCount = new int[CameraNum];

            HObject hoOrgImage;
            HObject hoConnectImage;
            HObject hoShadingImage;
            HObject hoMirrorImage;
            HOperatorSet.GenEmptyObj(out hoOrgImage);
            HOperatorSet.GenEmptyObj(out hoConnectImage);
            HOperatorSet.GenEmptyObj(out hoShadingImage);
            HOperatorSet.GenEmptyObj(out hoMirrorImage);

            while (!_bStop)
            {
                try
                {
                    if (QueueImage[iCamIndex].IsExist() == false)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    swGetImageTotal.Restart();
                    QueuingImage.QueueData qdata = QueueImage[iCamIndex].Dequeue();

                    hoOrgImage.Dispose();
                    hoShadingImage.Dispose();
                    HOperatorSet.CopyObj(qdata.ImageOrg, out hoOrgImage, 1, -1);
                    HOperatorSet.CopyObj(qdata.ImageOrg, out hoShadingImage, 1, -1);

                    HalconCameraBase cam = APCameraManager.getInstance().GetCamera(iCamIndex);
                    if (bSetFlag == false && cam.IsConnectVerticalImage && cam._clsConnectImage != null)
                    {
                        iThrowCount = cam._clsConnectImage.ConnectCount;
                        bSetFlag = true;
                    }

                    if (cam.IsConnectVerticalImage)
                    {
                        swConnectImage.Restart();

                        hoConnectImage.Dispose();
                        HOperatorSet.CopyObj(hoOrgImage, out hoConnectImage, 1, -1);
                        hoOrgImage.Dispose();
                        hoShadingImage.Dispose();
                        cam._clsConnectImage.ConnectImage(new List<HObject>() { hoConnectImage }, out hoOrgImage, out hoShadingImage);
                        if (iThrowCount > 0)
                            iThrowCount--;

                        swConnectImage.Stop();
                        //Console.WriteLine("Cam:" + iCamIndex + " Connect Time = " + swDebug.ElapsedMilliseconds.ToString());
                    }

                    if (iThrowCount == 0)
                    {
                        HTuple htMSecond, htSecond, htMinute, htHour, htDay, htYDay, htMonth, htYear;
                        HOperatorSet.GetImageTime(hoOrgImage, out htMSecond, out htSecond, out htMinute, out htHour, out htDay, out htYDay, out htMonth, out htYear);

                        swOnGrabbed.Restart();

                        _lstGrabbedImageEventHandler[iCamIndex](this, new GrabbedImageEventArgs(hoOrgImage, hoShadingImage, new DateTime(htYear.I, htMonth.I, htDay.I, htHour.I, htMinute.I, htSecond.I, htMSecond.I), iCamInfoIndex));

                        swOnGrabbed.Stop();
                        //Console.WriteLine("Cam:" + iCamIndex + " OnGrabbed Time = " + swOnGrabbed.ElapsedMilliseconds.ToString());
                    }
                    if (hoOrgImage != null)
                        hoOrgImage.Dispose();
                    if (hoShadingImage != null)
                        hoShadingImage.Dispose();

                    qdata.Dispose();

                    swGetImageTotal.Stop();
                    //Console.WriteLine("Cam:" + iNowCamera + " Time = " + sWatch.ElapsedMilliseconds.ToString());
                    if (OnGetImageThread != null)
                        OnGetImageThread(this, new InspectTimeEventArgs(
                            iCamIndex,
                            swConnectImage.ElapsedMilliseconds,
                            swOnGrabbed.ElapsedMilliseconds,
                            swGetImageTotal.ElapsedMilliseconds,
                            QueueImage[iCamIndex].Count));
                }
                catch (HOperatorException oe)
                {
                    // 例外発生
                    string msg = string.Format("getImageThread() CamName={0} Msg={1}", APCameraManager.getInstance().GetCamera(iCamIndex).Name, oe.Message);
                    System.Diagnostics.Debug.WriteLine(msg);
                    LogingDllWrap.LogingDll.Loging_SetLogString(msg);
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        private void CaptureErrorLog(int iCamNo, string msg)
        {
            string dir = SystemParam.LogFolder;
            string camDir = iCamNo.ToString();
            string dir3 = System.IO.Path.Combine(dir, camDir);
            string filename = string.Format("CaptureException-{0}.log", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff", System.Globalization.DateTimeFormatInfo.InvariantInfo));

            string path = System.IO.Path.Combine(dir3, filename);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (!System.IO.Directory.Exists(dir3))
            {
                System.IO.Directory.CreateDirectory(dir3);
            }
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, Encoding.GetEncoding("Shift-JIS")))
            {
                sw.Write(msg);
            }
        }

        /// <summary>
        /// イベントハンドラーリスト
        /// </summary>
        private List<GrabbedImageEventHandler> _lstGrabbedImageEventHandler = new List<GrabbedImageEventHandler>();
        /// <summary>
        /// キャプチャイベントを登録する
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool SetGrabbedImageEvent(int iIndex, GrabbedImageEventHandler handler)
        {
            _lstGrabbedImageEventHandler.Add(null);
            _lstGrabbedImageEventHandler[_lstGrabbedImageEventHandler.Count - 1] += handler;
            return true;
        }

        /// <summary>
        /// キャプチャイベントの登録を解除する
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool ResetGrabbedImageEvent(int iIndex, GrabbedImageEventHandler handler)
        {
            if (_lstGrabbedImageEventHandler.Count <= iIndex)
                return false;

            _lstGrabbedImageEventHandler[iIndex] -= handler;
            return true;
        }

        /// <summary>
        /// すべてのキャプチャイベントの登録を解除する
        /// </summary>
        public void ResetGrabbedImageEventAll()
        {
            for (int i = 0; i < _lstGrabbedImageEventHandler.Count; i++)
                _lstGrabbedImageEventHandler[i] = null;
            _lstGrabbedImageEventHandler.Clear();
        }
    }
}
