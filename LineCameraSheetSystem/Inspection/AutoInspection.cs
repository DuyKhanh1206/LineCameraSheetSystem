using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using LineCameraSheetSystem;
using LogingDllWrap;
using HalconDotNet;
using Fujita.Misc;
using HalconCamera;
using ResultActionDataClassNameSpace;
using KaTool;

namespace InspectionNameSpace
{
    /// <summary>自動検査クラス</summary>
    public class AutoInspection : IDisposable, IError
    {
        #region ■IError用メンバ変数
        public bool IsError { get; set; }
		public string ErrorReason { get; set; }
        #endregion

        #region ■メンバ変数（private）
        /// <summary>カメラ情報</summary>
        private HObject[] _hoImageTargets;
        private HObject[] _hoImageInspScales;

        private HObject[] _baseImageOrgBufs;
        private HObject[] _baseImageTargetBufs;
        private HObject[] _baseImageInspScaleBufs;
        private HObject[] _imageOrgs;
        private HObject[] _imageTargets;
        private HObject[] _imageInspScales;

        /// <summary>検査スレッド</summary>
        private Thread[] _inspThread;
        /// <summary>検査を行う時にシグナルになるイベント</summary>
        private AutoResetEvent[] _inspEvent;
        /// <summary>実検査クラス</summary>
        private ImageInspection[] _inspMainProc;
        /// <summary>検査完了中？</summary>
        private bool[] _isInspEnd;
        /// <summary>カメラ毎のイメージをキューイングする</summary>
        private QueuingImage[] _queueImage;

        /// <summary>同期スレッド</summary>
        private Thread _syncImageThread;
        /// <summary>オリジナルイメージロック</summary>
        private List<object> _imageOrgLockObject;

        private IniAutoInspection _iniAccess = new IniAutoInspection();
        private FormAutoInspInfo _frmAInpInfo = null;
        private HObject[] _imageOrgAInspInfo = null;
        private RefreshImageEvent _refreshImageEvent = null;
        #endregion

        #region ■メンバ変数（public）
        public List<CameraInfo> CamInfos { get; private set; }

        public int[] _connectOffsetXpix;
        public int[] _connectOffsetYpix;
        public int[] _connectCenterPix;

        /// <summary>測長監視クラス</summary>
        public LengthMeas LengthMeas { get; private set; }
        /// <summary>結果管理クラス</summary>
        public EventMonitor EventMonitor { get; private set; }

        #endregion

        #region ■プロパティ
        /// <summary>基準位置からカメラまでの距離(mm)</summary>
        public double BasePoint
        {
            get { return _iniAccess.BasePoint; }
            set { _iniAccess.BasePoint = value; }
        }
        /// <summary>NGを集約する範囲(mm)</summary>
        public double OverlapRange
        {
            get { return _iniAccess.OverlapRange; }
            set { _iniAccess.OverlapRange = value; }
        }
        /// <summary>Imageフォルダ配下の有限フォルダ数</summary>
        public int ImageNumDirMax
        {
            get { return _iniAccess.ImageNumDirMax; }
            set { _iniAccess.ImageNumDirMax = value; }
        }
        /// <summary>Image\Numberフォルダ内の最大イメージ数</summary>
        public int ImageNumDirFileMax
        {
            get { return _iniAccess.ImageNumDirFileMax; }
            set { _iniAccess.ImageNumDirFileMax = value; }
        }
        /// <summary>結果を保存するシステムフォルダ</summary>
        public string SystemResultDir { get; set; }
        /// <summary>イメージを保存するシステムフォルダ</summary>
        public string SystemImageDir { get; set; }

        /// <summary>検査時間(ms)</summary>
        public double InspTime
        {
            get { return EventMonitor.InspTime; }
        }
        public int[] RealGrabCount
        {
            get;
            private set;
        }
        public int[] QueueGrabCount
        {
            get;
            private set;
        }
        public int[] QueueCount
        {
            get;
            private set;
        }
        public int SyncGrabCount
        {
            get;
            private set;
        }
        public IniAutoInspection IniAccess
        {
            get { return this._iniAccess; }
        }
        public bool IsDebugForm
        {
            get { return (_frmAInpInfo != null); }
        }
        #endregion

        #region ■イベント
        /// <summary>各カメラの結果確定が発生したときのイベントハンドラ</summary>
        public delegate void EntryResultDataEventHandler(object sender, EntryResultDataEventArgs e);
        public event EntryResultDataEventHandler OnEventResultEntry;

        /// <summary>結果データ　パラメータ / AutoInspection -> EventMonitor</summary>
        public class EntryResultDataEventArgs : EventArgs
        {
            /// <summary>結果データ</summary>
            public List<ImageResultData> ResultDatas { get; private set; }

            public EntryResultDataEventArgs(List<ImageResultData> resultDatas)
            {
                this.ResultDatas = resultDatas;
            }
        }

        /// <summary>検査完了が発生した時のイベントハンドラ</summary>
        public delegate void ResultDataEventHandler(object sender, ActionDataEventArgs e);
        public event ResultDataEventHandler OnEventResult;

        /// <summary>開始・中断・停止操作が発生したときのイベントハンドラ</summary>
        public delegate void ActionDataEventHandler(object sender, ActionDataEventArgs e);
        public event ActionDataEventHandler OnEventAction;

        /// <summary>アクション（結果・操作）　パラメータ / AutoInspection -> EventMonitor</summary>
        public class ActionDataEventArgs : EventArgs
        {
            /// <summary>アクション識別</summary>
            public EResultActionId Id { get; private set; }
            /// <summary>カメラ情報</summary>
            public CameraInfo CamInfo { get; private set; }
            /// <summary>結果(true:OK false:NG)</summary>
            public bool Result { get; private set; }
            /// <summary>トータルのイメージ開始位置</summary>
            public double StartLength { get; private set; }
            /// <summary>トータルのイメージ終了位置</summary>
            public double EndLength { get; private set; }
            /// <summary>品種</summary>
            public Recipe Recipe { get; private set; }

            public double SheetValue { get; private set; }

            public int CamNo { get; private set; }
            public HObject ImageOrgs { get { return _imgOrgs; } }
            public HObject ImageTargets { get { return _imgTargets; } }
            public HObject ImageInspScales { get { return _imgInspScales; } }
            private HObject _imgOrgs;
            private HObject _imgTargets;
            private HObject _imgInspScales;

            public ActionDataEventArgs(EResultActionId id, CameraInfo camInfo, bool result, double st, double end, Recipe recipe, double sheetValue,
                int camNo = 0, HObject imgOrgs = null, HObject imgTargets = null, HObject imgInspScales = null)
            {
                this.Id = id;
                this.CamInfo = camInfo;
                this.Result = result;
                this.StartLength = st;
                this.EndLength = end;
                this.Recipe = recipe;
                this.SheetValue = sheetValue;

                CamNo = camNo;
                if (imgOrgs != null)
                    HOperatorSet.CopyObj(imgOrgs, out _imgOrgs, 1, -1);
                if (imgTargets != null)
                    HOperatorSet.CopyObj(imgTargets, out _imgTargets, 1, -1);
                if (imgInspScales != null)
                    HOperatorSet.CopyObj(imgInspScales, out _imgInspScales, 1, -1);
            }
            public void Dispose()
            {
                if (_imgOrgs != null)
                    _imgOrgs.Dispose();
                if (_imgTargets != null)
                    _imgTargets.Dispose();
                if (_imgInspScales != null)
                    _imgInspScales.Dispose();
            }
        }
        #endregion

        #region ■イメージ更新　通知
        /// <summary>イメージ更新イベントを登録する</summary>
        public void SetRefreshImageEvent(RefreshImageEvent.RefreshImageEventHandler handler)
        {
            if (_refreshImageEvent != null)
            {
                _refreshImageEvent.ChangeOnEvent = true;
                _refreshImageEvent.OnBuffEventRefreshImage += handler;
            }
        }
        /// <summary>イメージ更新イベントをクリアする</summary>
		public void ClearRefreshImageEvent(RefreshImageEvent.RefreshImageEventHandler handler)
        {
            if (_refreshImageEvent != null)
            {
                _refreshImageEvent.ChangeOnEvent = true;
                _refreshImageEvent.OnBuffEventRefreshImage -= handler;
            }
        }
        public void ClearAllRefreshImageEvent()
        {
            if (_refreshImageEvent != null)
            {
                _refreshImageEvent.ChangeOnEvent = true;
            }
        }
        #endregion







        #region コンストラクタ・デストラクタ
        /// <summary>コンストラクタ</summary>
        public AutoInspection(List<CameraInfo> camInfo, ResultActionDataClass resultActionDataClass)
        {
            _iniAccess.Load();

            //カメラ情報
            this.CamInfos = camInfo;

			this._refreshImageEvent = new RefreshImageEvent(camInfo);
			this._refreshImageEvent.ImageDataGetLine = _iniAccess.ImageDataGetLine;

			// 最小・最大・平均グレー値算出範囲の設定　をシステム基準範囲に戻す
			this.ResetMinMaxAveCalcRange();

            //測長監視クラス（代表のCam1の情報を使用する）
            this.LengthMeas = new LengthMeas(camInfo[0]);

            //品種
            _nowRecipe = null;
            _nowInspParamUpSide = new List<InspKandoParam>();
			_nowInspParamDownSide = new List<InspKandoParam>();

            //イベントモニタ
            this.EventMonitor = new EventMonitor(resultActionDataClass, camInfo);
            //イベントモニタへ結果通知する
            this.OnEventResultEntry += this.EventMonitor.OnEventResultEntry;
            //イベントモニタへ結果(Judge)アクションを通知する
            this.OnEventResult += this.EventMonitor.OnEventResult;
            //イベントモニタへ操作(Start,Stop,Suspend)アクションを通知する
            this.OnEventAction += this.EventMonitor.OnEventAction;

			//初期化
            this.SystemResultDir = @"C:\";
            this.SystemImageDir = @"C:\";

            //ステータス制御を初期化する
            this.InitializeStatus();

            this._imageOrgLockObject = new List<object>();
            foreach(CameraInfo cm in camInfo)
            {
                this._imageOrgLockObject.Add(new object());
            }
        }

		void EventMonitor_OnEventUpdateResultAction(object sender, EventMonitor.EventMonitorEventArgs e)
		{
			try
			{
				this.RefreshAutoInspInfo(null, EInfoRefreshPoint.OnAction);
			}
			catch (Exception exc)
			{
				string ErrStr = string.Format("AutoInspection.EventMonitor_OnEventUpdateResultAction() exc = {0}", exc.Message);
				LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);

				this.ErrorReason = ErrStr;
				this.IsError = true;
			}
		}
        /// <summary>デストラクタ</summary>
        public void Dispose()
        {
            //スレッドを終了する
            EndAutoInspection();

            //測長クラスを終了する
            this.LengthMeas.Dispose();
            //イベント監視を終了する
            this.EventMonitor.Dispose();

            _iniAccess.Save();
        }
        #endregion


        #region カメラ取込開始時の初期化処理
        /// <summary>
        /// カメラ取込開始時の初期化処理
        /// </summary>
        public void CameraStartInitialize()
        {
            //破棄するイメージ数
            for (int i = 0; i < CamInfos.Count; i++)
            {
				this._discardCnt[i] = CamInfos[i].DiscardCount;
				this.RealGrabCount[i] = 0;
				this.QueueGrabCount[i] = 0;
				this.QueueCount[i] = 0;
                if (_queueImage[i] != null)
                    _queueImage[i].Clear();
            }
			this.SyncGrabCount = 0;
        }
        #endregion


        #region 開始・終了
        /// <summary>
        /// 同期・検査スレッドを開始する
        /// </summary>
        public void BeginAutoInspection()
        {
            //インスタンス生成
            this.CreateInstanceDatas();

            //カメラ開始時の初期化処理
            this.CameraStartInitialize();

            //イベントモニタを開始する
            this.EventMonitor.BeginEventMonitor();

            //測長
            this.LengthMeas.BeginLengthMeas();

            //停止をOFFする
            _stopThread = false;

            //検査スレッドを開始する
            for (int i = 0; i < _inspThread.Length; i++)
            {
                _inspThread[i].Start(i);
            }

            //同期スレッドを開始する
            _syncImageThread.Start();
        }

		public delegate void EndAutoInspectionCompletionEventHandler(object sender, EventArgs e);
		public event EndAutoInspectionCompletionEventHandler OnEventEndAutoInspectionCompletion;
        /// <summary>
        /// 同期・検査スレッドを停止する
        /// </summary>
        public void EndAutoInspection()
        {
            //スレッドが停止中は、停止処理を行わない
            if (_stopThread == true)
            {
                return;
            }

            if (_frmAInpInfo != null)
            {
                _frmAInpInfo.FormEnd();
                _frmAInpInfo.Dispose();
            }
			_frmAInpInfo = null;
		
			//スレッド停止指示をONにする
            _stopThread = true;

			this._threadEndAutoInsp = new Thread(new ThreadStart(ThreadEndAutoInspection));
			this._threadEndAutoInsp.Name = "AutoInspction.ThreadEndAutoInspection";
			this._threadEndAutoInsp.Start();
        }
		private Thread _threadEndAutoInsp;
		private void ThreadEndAutoInspection()
		{
			//検査スレッドを停止する
			for (int i = 0; i < _inspThread.Length; i++)
			{
				_inspEvent[i].Set();
				_inspThread[i].Join();
			}

			//同期スレッドを停止する
			_syncImageThread.Join();

			//測長
			this.LengthMeas.EndLengthMeas();

			//イベントモニタを停止する
			this.EventMonitor.EndEventMonitor();

			//インスタンス解放
			this.DestroyInstanceDatas();

			if (OnEventEndAutoInspectionCompletion != null)
			{
				OnEventEndAutoInspectionCompletion(this, new EventArgs());
			}
		}
        #endregion


        #region インスタンスの生成・解放
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        private void CreateInstanceDatas()
        {
            int sideCount = Enum.GetValues(typeof(AppData.SideID)).Length;
            int camCount = CamInfos.Count;

			//イメージ破棄カウンタ
			_discardCnt = new int[camCount];
			//
			RealGrabCount = new int[camCount];
			QueueGrabCount = new int[camCount];
			QueueCount = new int[camCount];
			SyncGrabCount = 0;

            //カメラ毎イメージ
            _queueImage = new QueuingImage[camCount];

            _hoImageTargets = new HObject[camCount];
            _baseImageTargetBufs = new HObject[camCount];
            _baseImageOrgBufs = new HObject[camCount];
            _hoImageInspScales = new HObject[camCount];
            _baseImageInspScaleBufs = new HObject[camCount];


            _imageOrgs = new HObject[camCount];
            _imageTargets = new HObject[camCount];
            _imageInspScales = new HObject[camCount];

            _connectOffsetXpix = new int[sideCount];
			_connectOffsetYpix = new int[sideCount];
			_connectCenterPix = new int[sideCount];

			//連結のオフセット
			this.SetConnectOffset();

            //検査
            _inspThread = new Thread[camCount];
            _inspEvent = new AutoResetEvent[camCount];
            _inspMainProc = new ImageInspection[camCount];
            _isInspEnd = new bool[camCount];

            //同期スレッド
            _syncImageThread = new Thread(new ThreadStart(ThreadSyncImages));
            _syncImageThread.Name = "AutoInspction.ThreadSyncImages";

            _summaryData = new SummaryData[camCount][,];
            int index = 0;

            _resetEvent = new List<AutoResetEvent[]>();
            _hoBeforeShadingImage = new List<HObject[]>();
            _hoAfterShadingImage = new List<HObject[]>();
            _hoAfterScaleShadingImage = new List<HObject[]>();
            _iStartPos = new List<int>();
            _iEndPos = new List<int>();
            _dAdCoeff = new List<double[][]>();
            _iTopHeight = new List<int>();
            _iScaleShift = new List<int[]>();

            foreach (CameraInfo ci in this.CamInfos)
            {
                _resetEvent.Add(new AutoResetEvent[] { });
                _hoBeforeShadingImage.Add(new HObject[] { });
                _hoAfterShadingImage.Add(new HObject[] { });
                _hoAfterScaleShadingImage.Add(new HObject[] { });
                _iStartPos.Add(0);
                _iEndPos.Add(0);
                _dAdCoeff.Add(new double[][] { });
                _iTopHeight.Add(0);
                _iScaleShift.Add(new int[] { });

                _summaryData[index] = new SummaryData[1,1];
                index++;
                //カメラ番号
                int camNo = (int)ci.CamNo;

                //イメージキュー
                _queueImage[camNo] = new QueuingImage(camNo, 0);


                if (APCameraManager.getInstance().GetCamera(0).IsColor == false)
                {
                    SystemParam.GetInstance().ColorCamInspImage[0] = true;
                    SystemParam.GetInstance().ColorCamInspImage[1] = false;
                    SystemParam.GetInstance().ColorCamInspImage[2] = false;
                    SystemParam.GetInstance().ColorCamInspImage[3] = false;
                    //オリジナルイメージ[4000]
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out _hoImageTargets[camNo], 255);
                    //バッファイメージ[5000]
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight, out _baseImageTargetBufs[camNo], 255);
                    //ターゲットイメージ[6000]
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight + ci.OverLapLines, out _imageTargets[camNo], 255);

                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight, out _baseImageOrgBufs[camNo], 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight + ci.OverLapLines, out _imageOrgs[camNo], 255);

                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out _hoImageInspScales[camNo], 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight, out _baseImageInspScaleBufs[camNo], 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageTileHeight + ci.OverLapLines, out _imageInspScales[camNo], 255);
                }
                else
                {
                    ImageTool.InitializeImage3(ci.ImageWidth, ci.ImageHeight, out _hoImageTargets[camNo], 255);
                    ImageTool.InitializeImage3(ci.ImageWidth, ci.ImageTileHeight, out _baseImageTargetBufs[camNo], 255);
                    ImageTool.InitializeImage3(ci.ImageWidth, ci.ImageTileHeight + ci.OverLapLines, out _imageTargets[camNo], 255);

                    ImageTool.InitializeImage3(ci.ImageWidth, ci.ImageTileHeight, out _baseImageOrgBufs[camNo], 255);
                    ImageTool.InitializeImage3(ci.ImageWidth, ci.ImageTileHeight + ci.OverLapLines, out _imageOrgs[camNo], 255);

                    HObject img11, img12, img13, img14;
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img11, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img12, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img13, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img14, 255);
                    HObject img21, img22, img23, img24;
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img21, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img22, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img23, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img24, 255);
                    UtilityImage.ConcatObject4(img21, img22, img23, img24, out _hoImageInspScales[camNo]);
                    HObject img31, img32, img33, img34;
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img31, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img32, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img33, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img34, 255);
                    UtilityImage.ConcatObject4(img31, img32, img33, img34, out _baseImageInspScaleBufs[camNo]);
                    HObject img41, img42, img43, img44;
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img41, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img42, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img43, 255);
                    ImageTool.InitializeImage(ci.ImageWidth, ci.ImageHeight, out img44, 255);
                    UtilityImage.ConcatObject4(img41, img42, img43, img44, out _imageInspScales[camNo]);
                }

                //検査シグナルイベント
                _inspEvent[camNo] = new AutoResetEvent(false);
                //検査スレッド
                _inspThread[camNo] = new Thread(new ParameterizedThreadStart(ThreadImageInspect));
                _inspThread[camNo].Name = "AutoInspction.ThreadImageInspect_" + ci.CamNo.ToString();
                //実検査クラス
                _inspMainProc[camNo] = new ImageInspection(ci, ci.ResolutionX, CamInfos[0].ResolutionY, this.LengthMeas);
				_inspMainProc[camNo].MinMaxAveStartPos = _refreshImageEvent.MinMaxAveStartPos[camNo];
				_inspMainProc[camNo].MinMaxAveEndPos = _refreshImageEvent.MinMaxAveEndPos[camNo];
                _inspMainProc[camNo].LeftMaskWidthPix = _refreshImageEvent.LeftMaskWidthPix[camNo];
                _inspMainProc[camNo].RightMaskWidthPix = _refreshImageEvent.RightMaskWidthPix[camNo];
                //検査完了
                this._isInspEnd[camNo] = true;
            }

			if (_iniAccess.InfoDispEnable == true)
			{
				this._imageOrgAInspInfo = new HObject[_hoImageTargets.Length];
				_frmAInpInfo = new FormAutoInspInfo(this);
                Fujita.Communication.CommunicationManager.getInstance().getCommunicationDIO().SetFormAutoInspInfo(_frmAInpInfo);
                _frmAInpInfo.Show();
				this.EventMonitor.OnEventUpdateResultAction += new InspectionNameSpace.EventMonitor.UpdateResultActionEventHandler(EventMonitor_OnEventUpdateResultAction);
			}
		}
        /// <summary>
        /// インスタンスを解放する
        /// </summary>
        private void DestroyInstanceDatas()
        {
            if (_frmAInpInfo != null)
            {
                _frmAInpInfo.FormEnd();
                _frmAInpInfo.Dispose();
            }

			//同期スレッド
            _syncImageThread = null;

            //検査
            for (int i = 0; i < _inspThread.Length; i++)
            {
                _inspEvent[i].Dispose();
                _inspEvent[i] = null;
                _inspThread[i] = null;
                _inspMainProc[i] = null;
                _isInspEnd[i] = true;
            }

            //イメージ
            for (int i = 0; i < CamInfos.Count(); i++)
            {
                UtilityImage.ClearHalconObject(ref _hoImageTargets[i]);
                UtilityImage.ClearHalconObject(ref _hoImageInspScales[i]);

                UtilityImage.ClearHalconObject(ref _baseImageOrgBufs[i]);
                UtilityImage.ClearHalconObject(ref _baseImageTargetBufs[i]);
                UtilityImage.ClearHalconObject(ref _baseImageInspScaleBufs[i]);

                UtilityImage.ClearHalconObject(ref _imageOrgs[i]);
                UtilityImage.ClearHalconObject(ref _imageTargets[i]);
                UtilityImage.ClearHalconObject(ref _imageInspScales[i]);
            }

            //イメージキュー
            for (int i = 0; i < Enum.GetValues(typeof(AppData.SideID)).Length; i++)
            {
                _queueImage[i].Dispose();
                _queueImage[i] = null;
            }
        }
        #endregion


        #region 

        public class AutoInspectGrabbedEventArgs
        {
            public int CamIndex { get; private set; }
            public AutoInspectGrabbedEventArgs(int iCamIndex)
            {
                CamIndex = iCamIndex;
            }
        }
        public delegate void AutoInspectGrabbedEventHandler(object sender, AutoInspectGrabbedEventArgs e);
        public event AutoInspectGrabbedEventHandler OnAutoInspectGrabbed;


        public bool ShadingStart { get; set; }
        private int[] _discardCnt;   //破棄するイメージ数
        /// <summary>
        /// 画像取得イベント
        /// </summary>
        public void OnGrabbedEventHander(object sender, GrabbedImageEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            HObject hoShadingColorImage;
            HObject hoChangeColorImage;
            HObject sideMaskRegion;
            HObject targetGetImage;
            HObject hoImage1, hoImage2, hoImage3;
            HObject hoGrayImage;
            HObject hoSelObj1;
            HObject hoSelObj2;
            HObject hoSelScaleObject1;
            HObject hoSelScaleObject2;
            HObject convColorImage;
            HObject inspScaleGetImage;
            HObject dmy1, dmy2;
            HObject buf;
            HOperatorSet.GenEmptyObj(out hoShadingColorImage);
            HOperatorSet.GenEmptyObj(out hoChangeColorImage);
            HOperatorSet.GenEmptyObj(out sideMaskRegion);
            HOperatorSet.GenEmptyObj(out targetGetImage);
            HOperatorSet.GenEmptyObj(out hoImage1);
            HOperatorSet.GenEmptyObj(out hoImage2);
            HOperatorSet.GenEmptyObj(out hoImage3);
            HOperatorSet.GenEmptyObj(out hoGrayImage);
            HOperatorSet.GenEmptyObj(out hoSelObj1);
            HOperatorSet.GenEmptyObj(out hoSelObj2);
            HOperatorSet.GenEmptyObj(out hoSelScaleObject1);
            HOperatorSet.GenEmptyObj(out hoSelScaleObject2);
            HOperatorSet.GenEmptyObj(out convColorImage);
            HOperatorSet.GenEmptyObj(out inspScaleGetImage);
            HOperatorSet.GenEmptyObj(out dmy1);
            HOperatorSet.GenEmptyObj(out dmy2);
            HOperatorSet.GenEmptyObj(out buf);

            HTuple htNumber;
            int cnt;

            try
            {

                //イメージ取得の時間
                DateTime getTime = e.GrabbedTime;

                //カメラ番号(0-1)
                int camNo = e.Index;

                //カメラが有効か？
                if (CamInfos[camNo].Enabled == true)
                {
                    sw.Reset();
                    sw.Start();

                    lock (this._imageOrgLockObject[camNo])
                    {
                        UtilityImage.ClearHalconObject(ref _baseImageOrgBufs[camNo]);
                        HOperatorSet.CopyObj(e.OrgImage, out _baseImageOrgBufs[camNo], 1, -1);
                        //HOperatorSet.CopyImage(e.OrgImage, out _baseImageOrgBufs[camNo]);

                        hoShadingColorImage.Dispose();
                        HOperatorSet.CopyObj(e.ShadingImage, out hoShadingColorImage, 1, -1);
                        //HOperatorSet.CopyImage(e.ShadingImage, out hoShadingColorImage);

                        int inspImgCnt = 0;
                        foreach (bool b in SystemParam.GetInstance().ColorCamInspImage)
                        {
                            if (b)
                                inspImgCnt++;
                        }
                        bool isPainting = (inspImgCnt == 1);

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()01======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();

                        //明るい・暗い補正
                        hoChangeColorImage.Dispose();
                        HOperatorSet.CopyObj(hoShadingColorImage, out hoChangeColorImage, 1, -1);
                        if (this.ShadingStart == true || this._statusInsp != EInspectionStatus.Stop)
                        {
                            if (APCameraManager.getInstance().GetCamera(0).IsColor == false)
                            {
                                hoChangeColorImage.Dispose();
                                overLapCompetion(hoShadingColorImage, out hoChangeColorImage, camNo);
                                if (this._statusInsp != EInspectionStatus.Stop)
                                    this.ShadingStart = false;
                                isPainting = false;
                            }
                            else
                            {
                                hoImage1.Dispose();
                                hoImage2.Dispose();
                                hoImage3.Dispose();
                                HOperatorSet.Decompose3(hoShadingColorImage, out hoImage1, out hoImage2, out hoImage3);
                                overLapWhiteBlack(ref hoImage1, ref hoImage2, ref hoImage3, camNo);
                                hoChangeColorImage.Dispose();
                                HOperatorSet.Compose3(hoImage1, hoImage2, hoImage3, out hoChangeColorImage);
                            }
                        }

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()02======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();

                        //両端マスク
                        bool isCreateSideMask = false;
                        if (this.ShadingStart == true || this._statusInsp != EInspectionStatus.Stop)
                        {
                            sideMaskRegion.Dispose();
                            SideMaskFunc(hoChangeColorImage, out sideMaskRegion, camNo);
                            if (sideMaskRegion != null)
                            {
                                isCreateSideMask = true;
                            }
                            else
                            {
                                HOperatorSet.GenEmptyObj(out sideMaskRegion);
                            }
                            if (this._statusInsp != EInspectionStatus.Stop)
                                this.ShadingStart = false;
                        }

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()03======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();

                        //ソフトシェーディング
                        targetGetImage.Dispose();
                        inspScaleGetImage.Dispose();
                        if (this.ShadingStart == true || this._statusInsp != EInspectionStatus.Stop)
                        {
                            HObject targetDmy;
                            HObject inspScaleDmy;
                            softshading(hoChangeColorImage, isPainting, out targetDmy, out inspScaleDmy, camNo);
                            if (isCreateSideMask == true)
                            {
                                HOperatorSet.ReduceDomain(targetDmy, sideMaskRegion, out targetGetImage);
                                HOperatorSet.ReduceDomain(inspScaleDmy, sideMaskRegion, out inspScaleGetImage);
                            }
                            else
                            {
                                HOperatorSet.CopyObj(targetDmy, out targetGetImage, 1, -1);
                                HOperatorSet.CopyObj(inspScaleDmy, out inspScaleGetImage, 1, -1);
                            }
                            targetDmy.Dispose();
                            inspScaleDmy.Dispose();
                            if (this._statusInsp != EInspectionStatus.Stop)
                                this.ShadingStart = false;
                        }
                        else
                        {
                            if (APCameraManager.getInstance().GetCamera(0).IsColor == false)
                            {
                                HOperatorSet.CopyObj(hoChangeColorImage, out targetGetImage, 1, -1);
                                HOperatorSet.CopyObj(hoChangeColorImage, out inspScaleGetImage, 1, -1);
                            }
                            else
                            {
                                HOperatorSet.CopyObj(hoChangeColorImage, out targetGetImage, 1, -1);
                                UtilityImage.ConcatColor2ConcatObject4(hoChangeColorImage, out inspScaleGetImage);
                            }
                        }

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()04-1======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();

                        //オリジナルイメージをそのまま登録する
                        UtilityImage.ClearHalconObject(ref _hoImageTargets[camNo]);
                        _hoImageTargets[camNo] = UtilityImage.CopyHalconImage(targetGetImage);

                        UtilityImage.ClearHalconObject(ref _hoImageInspScales[camNo]);
                        _hoImageInspScales[camNo] = UtilityImage.CopyHalconImage(inspScaleGetImage);

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()04-2======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();


                        //
                        UtilityImage.ClearHalconObject(ref _baseImageTargetBufs[camNo]);
                        HOperatorSet.CopyObj(_hoImageTargets[camNo], out _baseImageTargetBufs[camNo], 1, -1);
                        //////////////////////////////////////////////////////////////////////////////
                        HOperatorSet.CountObj(inspScaleGetImage, out htNumber);
                        cnt = htNumber.I;
                        HOperatorSet.GenEmptyObj(out dmy2);
                        HOperatorSet.CopyObj(_baseImageInspScaleBufs[camNo], out buf, 1, -1);
                        for (int inspObj = 0; inspObj < cnt; inspObj++)
                        {
                            hoSelObj2.Dispose();
                            HOperatorSet.SelectObj(_hoImageInspScales[camNo], out hoSelObj2, inspObj + 1);

                            convColorImage.Dispose();
                            HOperatorSet.CopyObj(hoSelObj2, out convColorImage, 1, -1);


                            dmy1.Dispose();
                            HOperatorSet.CopyObj(dmy2, out dmy1, 1, -1);
                            dmy2.Dispose();
                            HOperatorSet.ConcatObj(dmy1, convColorImage, out dmy2);
                        }
                        UtilityImage.ClearHalconObject(ref _baseImageInspScaleBufs[camNo]);
                        _baseImageInspScaleBufs[camNo] = UtilityImage.CopyHalconImage(dmy2);
                        //////////////////////////////////////////////////////////////////////////////

                        sw.Stop();
                        //Console.WriteLine("OnGrabbedEventHander()05======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                    }
                }

                sw.Reset();
                sw.Start();

                if (_discardCnt[camNo] == 0)
                {
                    if (CamInfos[camNo].Enabled == true)
                    {
                        if (this.QueueCount[camNo] < this._iniAccess.ImageSyncQueueCnt)
                        {
                            //イメージをキューに登録する
                            _queueImage[camNo].Enqueue(
                                _baseImageOrgBufs[camNo],
                                _baseImageTargetBufs[camNo],
                                _baseImageInspScaleBufs[camNo]);
                            this.QueueGrabCount[camNo] = _queueImage[camNo].Counter;
                            this.QueueCount[camNo] = _queueImage[camNo].Count;
                        }
                        else
                        {
                            if (this.IsError == false)
                            {
                                this.ErrorReason = "メモリバッファ領域を超えました。";
                                LogingDll.Loging_SetLogString(string.Format("AutoInspection.OnGrabbedEventHander() ") + this.ErrorReason);
                                this.IsError = true;
                            }
                        }
                    }
                }
                else
                {
                    _discardCnt[camNo] -= 1;
                }
				this.RealGrabCount[camNo]++;

				this.RefreshAutoInspInfo(camNo, EInfoRefreshPoint.OnGrabbed);

                sw.Stop();
                //Console.WriteLine("OnGrabbedEventHander()06======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));

                if (OnAutoInspectGrabbed != null)
                    OnAutoInspectGrabbed(this, new AutoInspectGrabbedEventArgs(camNo));
            }
            catch (Exception exc)
            {
                string ErrStr = string.Format("AutoInspection.OnGrabbedEventHander() exc = {0}", exc.Message);
                LogingDll.Loging_SetLogString(ErrStr);
                Debug.WriteLine(ErrStr);
				//throw exc;

				this.ErrorReason = ErrStr;
				this.IsError = true;
            }
            finally
            {
                hoShadingColorImage.Dispose();
                hoChangeColorImage.Dispose();
                sideMaskRegion.Dispose();
                targetGetImage.Dispose();
                hoImage1.Dispose();
                hoImage2.Dispose();
                hoImage3.Dispose();
                hoGrayImage.Dispose();
                hoSelObj1.Dispose();
                hoSelObj2.Dispose();
                hoSelScaleObject1.Dispose();
                hoSelScaleObject2.Dispose();
                convColorImage.Dispose();
                inspScaleGetImage.Dispose();
                dmy1.Dispose();
                dmy2.Dispose();
                buf.Dispose();
            }
        }

        #endregion


        enum EInfoRefreshPoint
		{
			OnGrabbed,
			OnAction,
			OnImageRefresh,
		}
		private void RefreshAutoInspInfo(int? camno, EInfoRefreshPoint point)
		{
			if (_frmAInpInfo == null)
				return;

			bool refresh=false;
			if (point == EInfoRefreshPoint.OnGrabbed)
			{
				if (_statusInspBefore == EInspectionStatus.Stop)
					refresh = true;
			}
			else if (point == EInfoRefreshPoint.OnAction)
			{
				if (_statusInspBefore != EInspectionStatus.Stop)
					refresh = true;
			}
			else
			{
			}

			try
			{
				if (refresh == true)
				{
					lock (_imageOrgAInspInfo)
					{
						int length;
						lock (this._hoImageTargets)
						{
							length = this._hoImageTargets.Length;
							//orgImg = new HObject[length];
							for (int i = 0; i < length; i++)
							{
								if (this._hoImageTargets[i] != null)
								{
									UtilityImage.ClearHalconObject(ref _imageOrgAInspInfo[i]);
									_imageOrgAInspInfo[i] = UtilityImage.CopyHalconImage(this._hoImageTargets[i]);
									//HOperatorSet.CopyObj(this._imageOriginals[i], out _imageOrgAInspInfo[i], 1, -1);
								}
							}
						}
						_frmAInpInfo.RefreshDatas(camno, _imageOrgAInspInfo, _imageOrgAInspInfo);
						//for (int i = 0; i < _imageOrgAInspInfo.Length; i++)
						//    UtilityImage.ClearHalconObject(ref _imageOrgAInspInfo[i]);
						//for (int i = 0; i < _imageConAInspInfo.Length; i++)
						//    UtilityImage.ClearHalconObject(ref _imageConAInspInfo[i]);
					}
				}
			}
			catch(Exception exc)
			{
				string ErrStr = string.Format("AutoInspection.RefreshAutoInspInfo() exc = {0}", exc.Message);
				LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				throw exc;
			}
		}
        /// <summary>
        /// 全検査が完了しているか？
        /// </summary>
        /// <returns>true:完了 false:検査中</returns>
        private bool IsInspectionEnded()
        {
            bool ret = true;
            lock (this._isInspEnd)
            {
                for (int i = 0; i < _isInspEnd.Length; i++)
                {
                    ret &= _isInspEnd[i];
                }
            }
            return ret;
        }
        /// <summary>
        /// 全検査が完了するまで待つ
        /// </summary>
        private void WaitInspectionEnd()
        {
            while (true)
            {
                if (IsInspectionEnded() == true)
                    break;
                Thread.Sleep(1);
            }
        }
        /// <summary>
        /// 全イメージが揃っているか？
        /// </summary>
        /// <returns>true:OK false:NG</returns>
        private bool IsExistImages(out bool[] updown, out bool[] enableCam)
        {
            updown = new bool[Enum.GetNames(typeof(AppData.SideID)).Length];
            enableCam = new bool[CamInfos.Count];

            bool ret = false;
            for (int i=0; i<CamInfos.Count; i++)
            {
                if (CamInfos[i].Enabled == true)
                {
                    if (_queueImage[i].IsExist() == true)
                    {
                        updown[(int)CamInfos[i].CamSide] = true;
                        enableCam[i] = true;
                        ret = true;
                    }
                    else
                    {
                        //ret = false;
                    }
                }
            }
            if (ret == true)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("updown:[{0}][{1}]   cam:[{2}][{3}][{4}][{5}]", updown[0], updown[1], enableCam[0], enableCam[1], enableCam[2], enableCam[3]));
                ret = true;
            }
            else
            {
                ret = false;
            }

            //bool ret = true;
            //for (int i = 0; i < _queueImage.Length; i++)
            //{
            //    ret &= _queueImage[i].IsExist();
            //}
            return ret;
        }
        
        /// <summary>
        /// 同期・検査スレッド停止指示
        /// </summary>
        private bool _stopThread = true;

        #region 同期スレッド制御

        /// <summary>
        /// イメージ同期スレッド
        /// </summary>
        private void ThreadSyncImages()
        {
            QueuingImage.QueueData[] queueImageBuffer = new QueuingImage.QueueData[_queueImage.Length];

            HObject hoSelObj1;
            HObject hoSelObj2;
            HObject dmy;
            HObject dmy1, dmy2;
            HObject buf;
            HOperatorSet.GenEmptyObj(out hoSelObj1);
            HOperatorSet.GenEmptyObj(out hoSelObj2);
            HOperatorSet.GenEmptyObj(out dmy);
            HOperatorSet.GenEmptyObj(out dmy1);
            HOperatorSet.GenEmptyObj(out dmy2);
            HOperatorSet.GenEmptyObj(out buf);

            HTuple htNumber;
            int cnt;

            Stopwatch sw = new Stopwatch();
            while (!_stopThread)
            {
                try
                {
                    //全カメラの検査が完了するまで待つ
                    WaitInspectionEnd();

                    //操作アクションを処理する
                    EInspectionStatus nowStatus = ActionDataEvent();

                    bool[] updown;
                    bool[] enableCamera;
                    //全カメラのイメージが取得されているか？
                    if (IsExistImages(out updown, out enableCamera) == true)
                    {
                        sw.Reset();
                        sw.Start();

						this.SyncGrabCount++;

                        EventMonitor._____sw.Restart();

                        //全カメラのイメージを内部へ格納する
                        for (int i = 0; i < _queueImage.Length; i++)
                        {
                            int camNo = _queueImage[i].CamNo;

                            if (enableCamera[camNo] == false)
                                continue;

                            //イメージキューを保持する
                            queueImageBuffer[camNo] = _queueImage[i].Dequeue();

#if true

                            if (CamInfos[camNo].Enabled == true)
                            {
                                //イメージをターゲットに保存し、検査で使用する
                                //
                                dmy.Dispose();
                                HOperatorSet.CopyObj(queueImageBuffer[camNo].ImageTarget, out dmy, 1, -1);
                                UtilityImage.ClearHalconObject(ref _imageTargets[camNo]);
                                SideMaskFunc2(dmy, out _imageTargets[camNo]);
                                //
                                UtilityImage.ClearHalconObject(ref _imageOrgs[camNo]);
                                HOperatorSet.CopyObj(queueImageBuffer[camNo].ImageOrg, out _imageOrgs[camNo], 1, -1);
                                //////////////////////////////////////////////////////////////////////////////
                                HOperatorSet.CountObj(_baseImageInspScaleBufs[camNo], out htNumber);
                                cnt = htNumber.I;
                                dmy2.Dispose();
                                HOperatorSet.GenEmptyObj(out dmy2);
                                for (int inspObj = 0; inspObj < cnt; inspObj++)
                                {
                                    hoSelObj1.Dispose();
                                    HOperatorSet.SelectObj(queueImageBuffer[camNo].ImageInspScale, out hoSelObj1, inspObj + 1);

                                    dmy.Dispose();
                                    HOperatorSet.CopyObj(hoSelObj1, out dmy, 1, -1);
                                    buf.Dispose();
                                    SideMaskFunc2(dmy, out buf);

                                    dmy1.Dispose();
                                    HOperatorSet.CopyObj(dmy2, out dmy1, 1, -1);
                                    dmy2.Dispose();
                                    HOperatorSet.ConcatObj(dmy1, buf, out dmy2);
                                }
                                UtilityImage.ClearHalconObject(ref _imageInspScales[camNo]);
                                _imageInspScales[camNo] = UtilityImage.CopyHalconImage(dmy2);
                                //////////////////////////////////////////////////////////////////////////////
                            }
#endif
                            //イメージキューを開放する
                            if (queueImageBuffer[i] != null)
                                queueImageBuffer[i].Dispose();
                        }

                        sw.Stop();
                        //Console.WriteLine("ThreadSyncImages()01======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));
                        sw.Reset();
                        sw.Start();


#if true
                        List<int> brightKando = new List<int>();
						List<int> darkKando = new List<int>();

						if (nowStatus != EInspectionStatus.Stop)
						{
							//画像のプロットをログに出力する
							//foreach (CameraInfo cam in CamInfos)
							//{
							//	bool first = (this.LengthMeas.ImageCount == 0) ? true : false;
							//	if ((this.LengthMeas.ImageCount % 10) == 0)
							//		projectionLogging(first, (int)cam.CamNo, imageOriginals[(int)cam.CamNo]);
							//}

							//イメージを取得したので測長を更新する
                            if (updown[0]==true)
    							this.LengthMeas.Updates();

							//検査実施を指定する
							if (nowStatus == EInspectionStatus.Start || nowStatus == EInspectionStatus.Restart)
								this._runInsp = true;
							else
								this._runInsp = false;

							//検査を実施する
							for (int i = 0; i < _inspEvent.Length; i++)
							{
                                if (enableCamera[i] == false)
                                    continue;

								//検査中(false)にする
								SetInspectionEnded(i, false);
								_inspEvent[i].Set();
							}

                            foreach (ImageInspection inp in _inspMainProc)
                            {
                                brightKando.Add(inp.GetBrightBaseValue());
                                darkKando.Add(inp.GetDarkBaseValue());
                            }
						}
						else
						{
							foreach (ImageInspection inp in _inspMainProc)
							{
								brightKando.Add(128);
								darkKando.Add(128);
							}
						}

                        //_imageOriginals;
                        //imageBuffers,
                        //_imageTargets,

                        //イメージが更新されたことを通知する
                        //Debug.WriteLine(string.Format("{0} , {1} , {2}, {3} | {4} , {5} , {6}, {7}", brightKando[0], brightKando[1], brightKando[2], brightKando[3], darkKando[0], darkKando[1], darkKando[2], darkKando[3]));
                        _refreshImageEvent.Event(updown, enableCamera, CamInfos,
                            _imageOrgs,
                            _imageTargets,
                            _imageInspScales,
                            brightKando, darkKando);
#endif
                    }
                }
                catch (Exception exc)
                {
                    string ErrStr = string.Format("AutoInspection.ThreadSyncImages() exc = {0}", exc.Message);
                    LogingDll.Loging_SetLogString(ErrStr);
                    Debug.WriteLine(ErrStr);

					this.ErrorReason = ErrStr;
					this.IsError = true;
                }
                finally
                {
                    hoSelObj1.Dispose();
                    hoSelObj2.Dispose();
                    dmy.Dispose();
                    dmy1.Dispose();
                    dmy2.Dispose();
                    buf.Dispose();
                }

                Thread.Sleep(1);
            }
        }
#endregion


#region アクションをEventMonitorへ通知する
        /// <summary>
        /// アクションを通知する
        /// </summary>
        /// <returns></returns>
        private EInspectionStatus ActionDataEvent()
        {
            EInspectionStatus nowStatus;

            lock (this._lockStatus)
            {
                nowStatus = this._statusInsp;

                //状態が変更になったか？
                if (this._statusInsp != this._statusInspBefore)
                {
                    //Startアクションをイベントモニタへ通知する
                    EResultActionId id;
                    switch (this._statusInsp)
                    {
                        case EInspectionStatus.Stop:
                            //停止
                            id = EResultActionId.Stop;
                            break;
                        case EInspectionStatus.Suspend:
                            //中断
                            id = EResultActionId.Suspend;
                            break;
                        case EInspectionStatus.Start:
                            //開始
                            id = EResultActionId.Start;
                            break;
                        case EInspectionStatus.Restart:
                            //再開始
                            id = EResultActionId.Restart;
                            break;
						default:
							id = EResultActionId.Reset;
							break;
                    }
                    //操作アクションを通知する
                    //AutoInspection => EventMonitor
                    Recipe recipe = null;
                    List<InspKandoParam> kandoUpSide = new List<InspKandoParam>();
					List<InspKandoParam> kandoDownSide = new List<InspKandoParam>();
                    lock (_lockRecipe)
                    {
                        this.CopyRecipe(this._nowRecipe, this._nowInspParamUpSide, this._nowInspParamDownSide, ref recipe, ref kandoUpSide, ref kandoDownSide);
                    }

                    for (int i=0; i<Enum.GetNames(typeof(AppData.SideID)).Length; i++)
                    {
                        List<InspKandoParam> ka = (i == 0) ? kandoUpSide : kandoDownSide;
                        int j = 0;
                        foreach (InspKandoParam ip in ka)
                            recipe.InspParam[i].Kando[j] = ip;
                    }

                    ActionDataEventArgs act = new ActionDataEventArgs(
                        id,
                        this.CamInfos[0],
                        true,
                        this.LengthMeas.TotalStartLength, this.LengthMeas.TotalLength,
                        recipe, 0.0);

                    //操作アクションを通知したので、状態を設定する
                    this._statusInspBefore = this._statusInsp;

                    if (this.OnEventAction != null)
                    {
                        this.OnEventAction(this, act);

						//Resetの場合、
						if (this._statusInsp == EInspectionStatus.Reset)
						{
							//新規Startアクションを通知する。
							this._statusInspBefore = this._statusInsp = EInspectionStatus.Start;
							//測長監視をクリアする
							this.LengthMeas.Clear();
							act = new ActionDataEventArgs(
								EResultActionId.Start,
								this.CamInfos[0],
								true,
								this.LengthMeas.TotalStartLength, this.LengthMeas.TotalLength,
								recipe, 0.0);
							this.OnEventAction(this, act);
						}
                    }
                }
            }
            return nowStatus;
        }
#endregion


#region 検査スレッド制御

        /// <summary>
        /// 検査を実施してよいか
        /// </summary>
        private bool _runInsp = false;
        private void SetInspectionEnded(int camNo, bool end)
        {
            lock(_isInspEnd)
            {
                this._isInspEnd[camNo] = end;
            }
        }

        /// <summary>
        /// 検査スレッド
        /// </summary>
        /// <param name="obj">識別ID</param>
        private void ThreadImageInspect(object obj)
        {
            //識別ID
            int camNo = (int)obj;

			Stopwatch sw = new Stopwatch();

			while (true)
            {
                //検査完了(true)にする
                SetInspectionEnded(camNo, true);

				//sw.Stop();
				//Debug.WriteLine(string.Format("InspTime cam={0}  time = {1}", camNo, sw.Elapsed));

				//検査開始待ち
                _inspEvent[camNo].WaitOne();

                sw.Reset();
				sw.Start();

                //終了指示？
                if (_stopThread)
                {
                    SetInspectionEnded(camNo, true);
                    break;
                }

                try
                {
                    //結果データ
                    List<ImageResultData> imgResDatas = new List<ImageResultData>();

                    if (this.CamInfos[camNo].Enabled == true && this._runInsp == true)
                    {
                        //現在の品種データをコピーして、検査に渡す
                        Recipe recipe = null;
                        List<InspKandoParam> kandoUpSide = new List<InspKandoParam>();
                        List<InspKandoParam> kandoDownSide = new List<InspKandoParam>();
                        lock (_lockRecipe)
                        {
                            this.CopyRecipe(this._nowRecipe, this._nowInspParamUpSide, this._nowInspParamDownSide, ref recipe, ref kandoUpSide, ref kandoDownSide);
                        }

                        //表面の画像は表面検査有効の時に検査する、裏面の画像は裏面検査有効の時に検査する。      //20181202 moteki V1053
                        if ((this.CamInfos[camNo].CamSide == AppData.SideID.表 && recipe.UpSideInspEnable) || (this.CamInfos[camNo].CamSide == AppData.SideID.裏 && recipe.DownsideInspEnable))
                        {
                            this._inspMainProc[camNo].ImageCon = _imageTargets[camNo];
                            this._inspMainProc[camNo].Recipe = recipe;
                            if (this.CamInfos[camNo].CamSide == AppData.SideID.表)
                            {
                                this._inspMainProc[camNo].InspParam = kandoUpSide;
                            }
                            else
                            {
                                if (recipe.UpDownSideCommon == true)
                                {
                                    this._inspMainProc[camNo].InspParam = kandoUpSide;
                                }
                                else
                                {
                                    this._inspMainProc[camNo].InspParam = kandoDownSide;
                                }
                            }

                            lock (_imageOrgs[camNo])
                            {
                                if (true == this._inspMainProc[camNo].Run(_imageOrgs[camNo], _imageTargets[camNo], _imageInspScales[camNo], out imgResDatas))
                                {
                                    lock (this._imageOrgLockObject)
                                    {
                                        double ave = this._inspMainProc[camNo].GetAverageValue(_imageTargets[camNo]);
                                        this._inspMainProc[camNo].AddAverageData(ave);
                                    }
                                }
                            }
                        }

                    }

                    //結果データを通知する
                    //AutoInspection => EventMonitor(EntryResultData)
                    if (this.OnEventResultEntry != null)
                    {
                        EntryResultDataEventArgs args = new EntryResultDataEventArgs(imgResDatas);
                        this.OnEventResultEntry(this, args);
                        //結果を通知したので、破棄する
                        foreach (ImageResultData res in imgResDatas)
                        {
                            res.Dispose();
                        }
                    }
                    //Judgeアクションを通知する
                    //AutoInspection => EventMonitor(ActionData[Judge])
                    if (this.OnEventResult != null)
                    {
                        ActionDataEventArgs act = new ActionDataEventArgs(
                            EResultActionId.Judge,
                            this.CamInfos[camNo],
                            (imgResDatas.Count == 0) ? true : false,
                            this.LengthMeas.TotalStartLength,
                            this.LengthMeas.TotalLength, null, 0.0, camNo, _imageOrgs[camNo], _imageTargets[camNo], _imageInspScales[camNo]);
                        this.OnEventResult(this, act);
                    }

                    sw.Stop();
                    //if (camNo == 0)
                    //    Console.WriteLine(string.Format("ThreadImageInspect() camNo={0} 検査Time:{1}", camNo, sw.ElapsedMilliseconds.ToString("F1")));
                }
                catch (Exception exc)
                {
                    string ErrStr = string.Format("AutoInspection.ThreadImageInspect() exc = {0}", exc.Message);
                    LogingDll.Loging_SetLogString(ErrStr);
                    Debug.WriteLine(ErrStr);

					this.ErrorReason = ErrStr;
					this.IsError = true;
					//throw exc;
                }
            }
        }
#endregion


#region 品種の割り付けを制御する
        private object _lockRecipe = new object();
        /// <summary>
        /// 品種全体パラメータ
        /// </summary>
        private Recipe _nowRecipe;
        /// <summary>
        /// 品種判定パラメータ
        /// </summary>
        private List<InspKandoParam> _nowInspParamUpSide;
		private List<InspKandoParam> _nowInspParamDownSide;

        public InspKandoParam[,] GetKandoData()
        {
            if (_nowInspParamUpSide.Count == 0 || _nowInspParamDownSide.Count == 0)
                return null;
            InspKandoParam[,] inspK = new InspKandoParam[Enum.GetNames(typeof(AppData.SideID)).Length, Enum.GetNames(typeof(AppData.InspID)).Length];
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int inspId = 0; inspId < Enum.GetNames(typeof(AppData.InspID)).Length; inspId++)
                {
                    if (side == 0)
                        inspK[side, inspId] = _nowInspParamUpSide.ToArray()[inspId];
                    else
                        inspK[side, inspId] = _nowInspParamDownSide.ToArray()[inspId];
                }
            }
            return inspK;
        }

        /// <summary>
        /// 品種パラメータをバインドする（割り付ける）
        /// </summary>
        /// <param name="newRecipe"></param>
        public void BindRecipe(Recipe newRecipe)
        {
            lock (_lockRecipe)
            {
                this.CopyRecipe(newRecipe, newRecipe.InspParam[0].Kando, newRecipe.InspParam[1].Kando, ref this._nowRecipe, ref this._nowInspParamUpSide, ref this._nowInspParamDownSide);
            }
        }
        /// <summary>
        /// 品種をコピーする
        /// </summary>
        /// <param name="newRecipe"></param>
        /// <param name="newKandoUpSide"></param>
        /// <param name="toRecipe"></param>
        /// <param name="toKandoUpSide"></param>
        private void CopyRecipe(Recipe newRecipe, List<InspKandoParam> newKandoUpSide, List<InspKandoParam> newKandoDownSide, ref Recipe toRecipe, ref List<InspKandoParam> toKandoUpSide, ref List<InspKandoParam> toKandoDownSide)
        {
            toRecipe = newRecipe.Copy();
            toKandoUpSide.Clear();
			toKandoDownSide.Clear();
            foreach (InspKandoParam inp in newKandoUpSide)
            {
                InspKandoParam ip = new InspKandoParam();
                inp.Copy(ip);
                toKandoUpSide.Add(ip);
            }
			foreach (InspKandoParam inp in newKandoDownSide)
			{
				InspKandoParam ip = new InspKandoParam();
				inp.Copy(ip);
				toKandoDownSide.Add(ip);
			}
        }
        /// <summary>
        /// 品種が割り付けられているか？
        /// </summary>
        /// <returns>true:OK false:NG</returns>
        public bool IsRecipe()
        {
            return (this._nowRecipe != null);
        }
#endregion


#region 切り抜きイメージ設定
        /// <summary>
        /// 切り抜く範囲(mm)
        /// </summary>
        private int _cropWidth = 300;
        private int _cropHeight = 1200;
        /// <summary>
        /// イメージサイズ(pix)
        /// </summary>
        private int _scaleWidth = 300;
        private int _scaleHeight = 300;

        private int _cropHeightOmote_fromSpeed = 300;
        private int _cropHeightUra_fromSpeed = 300;
        /// <summary>
        /// NGイメージの切り取りサイズを指定する
        /// </summary>
        /// <param name="cropWidth">切り抜く横サイズ(pix)</param>
        /// <param name="cropHeight">切り抜く縦サイズ(pix)</param>
        /// <param name="scaleWidth">イメージ横サイズ(pix)</param>
        /// <param name="scaleHeight">イメージ縦サイズ(pix)</param>
        public void CropNgImageSize(int cropWidth, int cropHeight, int scaleWidth, int scaleHeight)
        {
            this._cropWidth = cropWidth;
            this._cropHeight = cropHeight;
            this._scaleWidth = scaleWidth;
            this._scaleHeight = scaleHeight;
        }
        public void CropNgImageSize_Speed(double speed, double speedUra)
        {
            double sp;
            sp = speed / SystemParam.GetInstance().CamSpeed;
            this._cropHeightOmote_fromSpeed = (int)(this._scaleHeight / sp);

            sp = speedUra / SystemParam.GetInstance().CamSpeedUra;
            this._cropHeightUra_fromSpeed = (int)(this._scaleHeight / sp);
        }
        #endregion


        #region 検査ステータス制御
        private object _lockStatus;
        /// <summary>
        /// 検査ステータス状態
        /// </summary>
        private EInspectionStatus _statusInsp;
        private EInspectionStatus _statusInspBefore;
        /// <summary>
        /// ステータスの初期処理
        /// </summary>
        private void InitializeStatus()
        {
            this._lockStatus = new object();
            this._statusInsp = EInspectionStatus.Stop;
            this._statusInspBefore = EInspectionStatus.Stop;
        }

        /// <summary>
        /// ステータスを変更可能か？
        /// </summary>
        /// <param name="newStatus">変更するステータス</param>
        /// <returns>true:OK false:NG</returns>
        private bool IsChangeStatus(EInspectionStatus newStatus)
        {
            //既にNew状態の場合、変更しない
            if (this._statusInsp == newStatus)
            {
                return false;
            }
            //前回状態が更新されてない（メインへの通知を行っていないため）、変更できない
            if (this._statusInsp != this._statusInspBefore)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// ステータスを変更する
        /// </summary>
        /// <param name="newStatus"></param>
        private void SetStatus(EInspectionStatus newStatus)
        {
            lock (this._lockStatus)
            {
                this._statusInsp = newStatus;
            }
        }
#endregion


		

#region 開始・停止・中断
        /// <summary>
        /// 検査を開始する
        /// </summary>
        /// <returns>true:OK false:NG</returns>
        public bool Start(string hinsyuName, string lotNo)
        {
			this.ErrorReason = "";
			this.IsError = false;
            LogingDll.Loging_SetLogString(string.Format("(ActionCheck):開始({0},{1})", hinsyuName.ToString(), lotNo.ToString()));

            EInspectionStatus newStatus = EInspectionStatus.Start;
            //Start可能か？
            if (false == this.IsChangeStatus(newStatus))
            {
                LogingDll.Loging_SetLogString(string.Format("Start可能か？ = false"));
                return false;
            }
            //品種が割り当てられているか？
            if (false == this.IsRecipe())
            {
                LogingDll.Loging_SetLogString(string.Format("品種が割り当てられているか？ = false"));
                return false;
            }

            //停止状態なので、新規Startとなる
            if (this._statusInsp == EInspectionStatus.Stop)
            {
                //
                ClearSoftShadingSummary();

                //測長監視をクリアする
                this.LengthMeas.Clear();
                //イベントモニタをクリアする
                this.EventMonitor.Clear();

                //品種名・LotNo
                this.EventMonitor.Hinsyu = hinsyuName;
                this.EventMonitor.LotNo = lotNo;
                this.EventMonitor.OverlapRange = this.OverlapRange;
                this.EventMonitor.ImageNumDirMax = this.ImageNumDirMax;
                this.EventMonitor.ImageNumDirFileMax = this.ImageNumDirFileMax;
                this.EventMonitor.SystemResultDir = this.SystemResultDir;
                this.EventMonitor.SystemImageDir = this.SystemImageDir;

				//自動感度調整のパラメータを検査スレッドに設定する
				this.SetAutoKandoDatas();

                for (int i = 0; i < _inspMainProc.Length; i++)
                {
                    _inspMainProc[i].Start();

					//NGイメージサイズ
					_inspMainProc[i].CropImageWidth = this._cropWidth;

                    if (i==0)
                        _inspMainProc[i].CropImageHeight = this._cropHeightOmote_fromSpeed;
                    else
                        _inspMainProc[i].CropImageHeight = this._cropHeightUra_fromSpeed;

                    _inspMainProc[i].ScaleImageWidth = this._scaleWidth;
                    _inspMainProc[i].ScaleImageHeight = this._scaleHeight;
					//基準
                    _inspMainProc[i].BasePoint = this.BasePoint;
					//検査の順番
					_inspMainProc[i].InspOrder = this._iniAccess.InspOrder;
					//平均輝度リストデータをリセットする
					_inspMainProc[i].ResetAverageDatas();
					//
					_inspMainProc[i].NGPositionMode = this._iniAccess.NGPositionMode;
                }
				int cam1 = 0, cam2 = 1;
				//NG切り抜き用オフセット
				_inspMainProc[cam1].PareOffsetXpix = CamInfos[cam2].ImageWidth - _connectOffsetXpix[(int)AppData.SideID.表];
				_inspMainProc[cam2].PareOffsetXpix = _connectOffsetXpix[(int)AppData.SideID.表];
				_inspMainProc[cam1].PareOffsetYpix = _connectOffsetYpix[(int)AppData.SideID.表];
				_inspMainProc[cam2].PareOffsetYpix = -(_connectOffsetYpix[(int)AppData.SideID.表]);
				
				int overlapPix;
				int overlapHalfPix;
				int range;
				//重なり部除外範囲(cam1,cam2)
				overlapPix = CamInfos[cam2].ImageWidth - _connectOffsetXpix[(int)AppData.SideID.表];
				overlapHalfPix = overlapPix / 2;
				range = (int)(_iniAccess.OverlapRange / CamInfos[cam1].ResolutionX);
				_inspMainProc[cam1].HOverlopExceptMin = _connectCenterPix[(int)AppData.SideID.表] + range;
				_inspMainProc[cam1].HOverlopExceptMax = CamInfos[cam1].ImageWidth;
				_inspMainProc[cam2].HOverlopExceptMin = 0;
				_inspMainProc[cam2].HOverlopExceptMax = overlapHalfPix - range;
                //重なり部除外範囲(cam3,cam4)

                //平均、最小、最大　算出範囲
                if (this._nowRecipe.CommonInspAreaEnable == false)
                {
                    this.SetMinMaxAveCalcRange(
                        new double[] { this._nowRecipe.InspParam[0].MaskWidth, this._nowRecipe.InspParam[1].MaskWidth },
                        new double[] { this._nowRecipe.InspParam[0].MaskShift, this._nowRecipe.InspParam[1].MaskShift },
                        new double[] { this._nowRecipe.InspParam[0].Width, this._nowRecipe.InspParam[1].Width });
                }
                else
                {
                    this.SetMinMaxAveCalcRange(
                        new double[] { SystemParam.GetInstance().InspArea_CmnMaskWidth[0], SystemParam.GetInstance().InspArea_CmnMaskWidth[1] },
                        new double[] { SystemParam.GetInstance().InspArea_CmnMaskShift[0], SystemParam.GetInstance().InspArea_CmnMaskShift[1] },
                        new double[] { SystemParam.GetInstance().InspArea_CmnSheetWidth[0], SystemParam.GetInstance().InspArea_CmnSheetWidth[1] });
                }

                //
                int no = 0;
				foreach (ImageInspection inp in _inspMainProc)
				{
					inp.MinMaxAveStartPos = _refreshImageEvent.MinMaxAveStartPos[no];
					inp.MinMaxAveEndPos = _refreshImageEvent.MinMaxAveEndPos[no];
                    inp.LeftMaskWidthPix = _refreshImageEvent.LeftMaskWidthPix[no];
                    inp.RightMaskWidthPix = _refreshImageEvent.RightMaskWidthPix[no];
					no = no + 1;
				}
			}
            else
            {
                //中断なので、再開になる
                newStatus = EInspectionStatus.Restart;
            }

            //ステータスを変更する
            this.SetStatus(newStatus);
            return true;
        }
        /// <summary>
        /// 検査を停止する
        /// </summary>
        public bool Stop()
        {
			LogingDll.Loging_SetLogString("(ActionCheck):停止");

            EInspectionStatus newStatus = EInspectionStatus.Stop;
            //Stop可能か？
            if (false == this.IsChangeStatus(newStatus))
            {
                LogingDll.Loging_SetLogString(string.Format("Stop可能か？ = false"));
                return false;
            }
            //ステータスを変更する
            this.SetStatus(newStatus);

			// 最小・最大・平均グレー値算出範囲の設定　をシステム基準範囲に戻す
			this.ResetMinMaxAveCalcRange();

			return true;
        }
        /// <summary>
        /// 検査を中断する
        /// </summary>
        public bool Suspend()
        {
            LogingDll.Loging_SetLogString("AutoInspection:中断");

            EInspectionStatus newStatus = EInspectionStatus.Suspend;

            //Suspend可能か？
            if (false == this.IsChangeStatus(newStatus))
            {
                LogingDll.Loging_SetLogString(string.Format("Suspend可能か？ = false"));
                return false;
            }
            //ステータスを変更する
            this.SetStatus(newStatus);
            return true;
        }
		public bool Reset()
		{
			LogingDll.Loging_SetLogString("AutoInspection:リセット");

			EInspectionStatus newStatus = EInspectionStatus.Reset;

			if (false == this.IsChangeStatus(newStatus))
			{
				LogingDll.Loging_SetLogString(string.Format("Reset可能か？ = false"));
				return false;
			}
			if (this._statusInsp == EInspectionStatus.Stop || this._statusInsp == EInspectionStatus.Suspend)
			{
				return false;
			}
			this.SetStatus(newStatus);
			return true;
		}
#endregion

		public void GetMinMaxAveCalcRangePix(AppData.CamID camNo, out int left, out int right)
		{
			int offxPix = (int)(CamInfos[(int)camNo].OffsetX / CamInfos[(int)camNo].ResolutionX);
			left = _refreshImageEvent.MinMaxAveStartPos[(int)camNo];
			right = _refreshImageEvent.MinMaxAveEndPos[(int)camNo];
		}
		public void GetMinMaxAveCalcRangePix(AppData.SideID sideId, out int left, out int right)
		{
			int leftCam;
			int rightCam;
			if (sideId == AppData.SideID.表)
			{
				leftCam = (int)AppData.CamID.cam1;
				rightCam = (int)AppData.CamID.cam1;
			}
			else
			{
				leftCam = (int)AppData.CamID.cam2;
				rightCam = (int)AppData.CamID.cam2;
			}
			left = _refreshImageEvent.MinMaxAveStartPos[leftCam];
			right = _refreshImageEvent.MinMaxAveEndPos[rightCam] + this._connectOffsetXpix[(int)sideId];
		}
		/// <summary>
		/// 最小・最大・平均グレー値算出範囲の設定
		/// </summary>
		/// <param name="maskShift">シフト</param>
		/// <param name="inspWidth">検査幅</param>
		public void SetMinMaxAveCalcRange(double[] maskWidth, double[] maskShift, double[] inspWidth)
		{
			int cam1 = 0, cam2 = 1;
			double startReal;
			int startPix;
			double endReal;
			int endPix;

            int maskWidthPix;

			int leftNo, rightNo;
			for (int i = 0; i < Enum.GetValues(typeof(AppData.SideID)).Length; i++)
			{
				leftNo = (i == 0) ? cam1 : cam2;
				rightNo = (i == 0) ? cam1 : cam2;

                maskWidthPix = (int)(maskWidth[i] / CamInfos[leftNo].ResolutionX);

                //開始位置の算出
                startReal = maskShift[i] - this.BasePoint;
				startPix = (int)(startReal / CamInfos[leftNo].ResolutionX);
				//終了位置の算出
				endReal = startReal + inspWidth[i];
				endPix = (int)(endReal / CamInfos[leftNo].ResolutionX);
				//開始・終了Pix位置の設定
				int imgw = CamInfos[leftNo].ImageWidth - 1;
				int offxPix = (int)(CamInfos[rightNo].OffsetX / CamInfos[rightNo].ResolutionX);

				int baseSideNo;
				if (this._iniAccess.CalibBaseCamera == 0)
				{
					baseSideNo = 0;	//sideNo 基準Cam1,2 表
				}
				else
				{
					baseSideNo = 1;	//sideNo 基準Cam3,4 裏
				}
				if (i == baseSideNo)
				{
					_refreshImageEvent.MinMaxAveStartPos[leftNo] = startPix;
					_refreshImageEvent.MinMaxAveEndPos[leftNo] = endPix;
					_refreshImageEvent.MinMaxAveStartPos[rightNo] = startPix - offxPix;
					_refreshImageEvent.MinMaxAveEndPos[rightNo] = endPix - offxPix;
                    //
                    _refreshImageEvent.LeftMaskWidthPix[leftNo] = maskWidthPix;
                    _refreshImageEvent.RightMaskWidthPix[leftNo] = maskWidthPix;
                    _refreshImageEvent.LeftMaskWidthPix[rightNo] = maskWidthPix;
                    _refreshImageEvent.RightMaskWidthPix[rightNo] = maskWidthPix;
                    if (this._statusInsp == EInspectionStatus.Stop)
					{
                        _refreshImageEvent.MinMaxAveStartPosNowRecipe[leftNo] = startPix;
						_refreshImageEvent.MinMaxAveEndPosNowRecipe[leftNo] = endPix;
						_refreshImageEvent.MinMaxAveStartPosNowRecipe[rightNo] = startPix - offxPix;
						_refreshImageEvent.MinMaxAveEndPosNowRecipe[rightNo] = endPix - offxPix;
                        //
                        _refreshImageEvent.LeftMaskWidthPixNowRecipe[leftNo] = maskWidthPix;
                        _refreshImageEvent.RightMaskWidthPixNowRecipe[leftNo] = maskWidthPix;
                        _refreshImageEvent.LeftMaskWidthPixNowRecipe[rightNo] = maskWidthPix;
                        _refreshImageEvent.RightMaskWidthPixNowRecipe[rightNo] = maskWidthPix;
                    }
                }
				else
				{
					int offset =0;
					_refreshImageEvent.MinMaxAveStartPos[leftNo] = startPix - offset;
					_refreshImageEvent.MinMaxAveEndPos[leftNo] = endPix - offset;
					_refreshImageEvent.MinMaxAveStartPos[rightNo] = startPix - offxPix;
					_refreshImageEvent.MinMaxAveEndPos[rightNo] = endPix - offxPix;
                    //
                    _refreshImageEvent.LeftMaskWidthPix[leftNo] = maskWidthPix;
                    _refreshImageEvent.RightMaskWidthPix[leftNo] = maskWidthPix;
                    _refreshImageEvent.LeftMaskWidthPix[rightNo] = maskWidthPix;
                    _refreshImageEvent.RightMaskWidthPix[rightNo] = maskWidthPix;
                    if (this._statusInsp == EInspectionStatus.Stop)
					{
						_refreshImageEvent.MinMaxAveStartPosNowRecipe[leftNo] = startPix - offset;
						_refreshImageEvent.MinMaxAveEndPosNowRecipe[leftNo] = endPix - offset;
						_refreshImageEvent.MinMaxAveStartPosNowRecipe[rightNo] = startPix - offxPix;
						_refreshImageEvent.MinMaxAveEndPosNowRecipe[rightNo] = endPix - offxPix;
                        //
                        _refreshImageEvent.LeftMaskWidthPixNowRecipe[leftNo] = maskWidthPix;
                        _refreshImageEvent.RightMaskWidthPixNowRecipe[leftNo] = maskWidthPix;
                        _refreshImageEvent.LeftMaskWidthPixNowRecipe[rightNo] = maskWidthPix;
                        _refreshImageEvent.RightMaskWidthPixNowRecipe[rightNo] = maskWidthPix;
                    }
                }

                if (_inspMainProc != null && _inspMainProc[0]!=null)//v1338 yuasa ポップアップを閉じる順番でエラーとなるため、「_inspMainProc[0]!=null」のチェックを追加。
                {
                    for (int no = 0; no < _refreshImageEvent.MinMaxAveStartPos.Length; no++)
                        if (_refreshImageEvent.MinMaxAveStartPos[no] < 0)
                            _refreshImageEvent.MinMaxAveStartPos[no] = 0;
                    for (int no = 0; no < _refreshImageEvent.MinMaxAveEndPos.Length; no++)
                        if (_refreshImageEvent.MinMaxAveEndPos[no] >= _inspMainProc[no].CamInfo.ImageWidth)
                            _refreshImageEvent.MinMaxAveEndPos[no] = _inspMainProc[no].CamInfo.ImageWidth - 1;
                    for (int no = 0; no < _refreshImageEvent.MinMaxAveStartPosNowRecipe.Length; no++)
                        if (_refreshImageEvent.MinMaxAveStartPosNowRecipe[no] < 0)
                            _refreshImageEvent.MinMaxAveStartPosNowRecipe[no] = 0;
                    for (int no = 0; no < _refreshImageEvent.MinMaxAveEndPosNowRecipe.Length; no++)
                        if (_refreshImageEvent.MinMaxAveEndPosNowRecipe[no] >= _inspMainProc[no].CamInfo.ImageWidth)
                            _refreshImageEvent.MinMaxAveEndPosNowRecipe[no] = _inspMainProc[no].CamInfo.ImageWidth - 1;
                }

                if (_inspMainProc != null && _inspMainProc[0] != null)//v1338 yuasa ポップアップを閉じる順番でエラーとなるため、「_inspMainProc[0]!=null」のチェックを追加。
                {
                    int pos;
                    pos = _refreshImageEvent.MinMaxAveStartPos[leftNo];
                    _inspMainProc[leftNo].InspWidthStartPix = (pos < 0) ? 0 : pos;
                    pos = _refreshImageEvent.MinMaxAveEndPos[leftNo];
                    _inspMainProc[leftNo].InspWidthEndPix = (pos > _inspMainProc[leftNo].CamInfo.ImageWidth) ? _inspMainProc[leftNo].CamInfo.ImageWidth - 1 : pos;
                    pos = _refreshImageEvent.MinMaxAveStartPos[rightNo];
                    _inspMainProc[rightNo].InspWidthStartPix = (pos < 0) ? 0 : pos;
                    pos = _refreshImageEvent.MinMaxAveEndPos[rightNo];
                    _inspMainProc[rightNo].InspWidthEndPix = (pos > _inspMainProc[rightNo].CamInfo.ImageWidth) ? _inspMainProc[rightNo].CamInfo.ImageWidth - 1 : pos;
                }
			}
		}
		/// <summary>
		/// 最小・最大・平均グレー値算出範囲の設定　をシステム基準範囲に戻す
		/// </summary>
		public void ResetMinMaxAveCalcRange()
		{
            if (this._statusInsp == EInspectionStatus.Stop)
            {
                this.SetMinMaxAveCalcRange(
                    new double[] { SystemParam.GetInstance().DefaultMaskWidth, SystemParam.GetInstance().DefaultMaskWidth },
                    new double[] { SystemParam.GetInstance().DefaultMaskShift, SystemParam.GetInstance().DefaultMaskShift },
                    new double[] { SystemParam.GetInstance().DefaultInspWidth, SystemParam.GetInstance().DefaultInspWidth });
            }
            else
            {
                if (this._nowRecipe.CommonInspAreaEnable == false)
                {
                    this.SetMinMaxAveCalcRange(
                        new double[] { this._nowRecipe.InspParam[0].MaskWidth, this._nowRecipe.InspParam[1].MaskWidth },
                        new double[] { this._nowRecipe.InspParam[0].MaskShift, this._nowRecipe.InspParam[1].MaskShift },
                        new double[] { this._nowRecipe.InspParam[0].Width, this._nowRecipe.InspParam[1].Width });
                }
                else
                {
                    this.SetMinMaxAveCalcRange(
                        new double[] { SystemParam.GetInstance().InspArea_CmnMaskWidth[0], SystemParam.GetInstance().InspArea_CmnMaskWidth[1] },
                        new double[] { SystemParam.GetInstance().InspArea_CmnMaskShift[0], SystemParam.GetInstance().InspArea_CmnMaskShift[1] },
                        new double[] { SystemParam.GetInstance().InspArea_CmnSheetWidth[0], SystemParam.GetInstance().InspArea_CmnSheetWidth[1] });
                }
            }
		}

		/// <summary>
		/// 自動感度の設定を各検査クラスに設定する
		/// </summary>
		public void SetAutoKandoDatas()
		{
			foreach (ImageInspection ins in _inspMainProc)
			{
				ins.AutoKandoBrightEnable = this._iniAccess.AutoKandoBrightEnabled;
				ins.AutoKandoDarkEnable = this._iniAccess.AutoKandoDarkEnabled;
				ins.AutoKandoLimit = this._iniAccess.AutoKandoLimit;
			}
		}

		/// <summary>
		/// 画像のプロットをログに出力する
		/// V1020-002
		/// </summary>
		/// <param name="bFirst"></param>
		/// <param name="iCamIndex"></param>
		/// <param name="hoImg"></param>
		/// <returns></returns>
		private bool projectionLogging(bool bFirst, int iCamIndex, HObject hoImg)
		{
			HObject hoRegion = null;

			if (iCamIndex < 0 || iCamIndex > 4)
				return false;

			if (hoImg == null)
				return false;

			try
			{
				HTuple htWidth, htHeight;
				HTuple htHorzProj, htVertProj;

				HOperatorSet.GetImageSize(hoImg, out htWidth, out htHeight);
				int iWidth = htWidth.I;
				HOperatorSet.GenRectangle1(out hoRegion, 0, 0, 49, iWidth - 1);
				HOperatorSet.GrayProjections(hoRegion, hoImg, "simple", out htHorzProj, out htVertProj);

				double dStep = iWidth / 99.0;

				int[] iaVal = new int[99];
				double[] daData = htVertProj.DArr;
				double dIndex = 0;
				int iLen = iaVal.Length;
				for (int i = 0; i < iLen; i++)
				{
					iaVal[i] = (int)daData[(int)dIndex];
					dIndex += dStep;
				}

                //StringBuilder sb = new StringBuilder(200);
                //for (int i = 0; i < iLen; i++)
                //{
                //    sb.Append(iaVal[i].ToString("X2"));
                //}
                //if (bFirst)
                //    LogingDll.Loging_SetLogString("$" + iCamIndex.ToString() + sb.ToString());
                //else
                //    LogingDll.Loging_SetLogString("@" + iCamIndex.ToString() + sb.ToString());
			}
			catch (HOperatorException oe)
			{
				// ログデータに書き込む
				LogingDllWrap.LogingDll.Loging_SetLogString("projectionLogging:" + oe.Message);
			}
			finally
			{
				if (hoRegion != null)
				{
					hoRegion.Dispose();
					hoRegion = null;
				}
			}
			return true;
		}

		/// <summary>
		/// 画像を連結するときのオフセット値を算出する
		/// </summary>
		private void SetConnectOffset()
		{
			//int cam1 = 0, cam2 = 1, cam3 = 2, cam4 = 3;
            //int cam1, cam2, cam3, cam4;
            //switch (_iniAccess.CalibBaseCamera)
            //{
            //    case 0:
            //        cam1 = 0; cam2 = 1; cam3 = 2; cam4 = 3;
            //        break;
            //    case 1:
            //        cam1 = 1; cam2 = 0; cam3 = 3; cam4 = 2;
            //        break;
            //    case 2:
            //        cam1 = 2; cam2 = 3; cam3 = 0; cam4 = 1;
            //        break;
            //    default:
            //        cam1 = 3; cam2 = 2; cam3 = 1; cam4 = 0;
            //        break;
            //}
            //_connectOffsetXpix[0] = (int)(CamInfos[cam2].OffsetX / CamInfos[cam1].ResolutionX);	//Cam1の分解能で算出する
            //_connectOffsetYpix[0] = (int)(CamInfos[cam2].OffsetY / CamInfos[cam1].ResolutionY);	//Cam1の分解能で算出する
            //_connectOffsetXpix[1] = (int)((CamInfos[cam4].OffsetX - CamInfos[cam3].OffsetX) / CamInfos[cam1].ResolutionX);	//Cam1の分解能で算出する
            //_connectOffsetYpix[1] = (int)((CamInfos[cam4].OffsetY - CamInfos[cam3].OffsetY) / CamInfos[cam1].ResolutionY);	//Cam1の分解能で算出する
            //_connectCenterPix[0] = (CamInfos[cam1].ImageWidth + _connectOffsetXpix[0]) / 2;
            //_connectCenterPix[1] = (CamInfos[cam3].ImageWidth + _connectOffsetXpix[1]) / 2;

            int cam1 = 0, cam2 = 1;
            switch (_iniAccess.CalibBaseCamera)
            {
                case 0:
			        _connectOffsetXpix[0] = (int)(CamInfos[cam2].OffsetX / CamInfos[cam1].ResolutionX);	//Cam1の分解能で算出する
			        _connectOffsetYpix[0] = (int)(CamInfos[cam2].OffsetY / CamInfos[cam1].ResolutionY);	//Cam1の分解能で算出する
			        _connectCenterPix[0] = (CamInfos[cam1].ImageWidth + _connectOffsetXpix[0]) / 2;
                    break;
                default:
			        _connectOffsetXpix[0] = (int)((CamInfos[cam2].OffsetX - CamInfos[cam1].OffsetX) / CamInfos[cam1].ResolutionX);	//Cam1の分解能で算出する
			        _connectOffsetYpix[0] = (int)((CamInfos[cam2].OffsetY - CamInfos[cam1].OffsetY) / CamInfos[cam1].ResolutionY);	//Cam1の分解能で算出する
			        _connectCenterPix[0] = (CamInfos[cam1].ImageWidth + _connectOffsetXpix[0]) / 2;
                    break;
            }
		}

        private void overLapCompetion(HObject img, out HObject compImage, int camNo)
        {
            HTuple htWidth, htHeight;
            HObject hoRegion = null;
            HObject hoThresRegion = null;
            HObject hoReduceDomain = null;
            int startPos = _refreshImageEvent.MinMaxAveStartPos[camNo];
            int endPos = _refreshImageEvent.MinMaxAveEndPos[camNo];

            try
            {
                HOperatorSet.CopyObj(img, out compImage, 1, -1);

                if (SystemParam.GetInstance().InspBrightEnable == true && SystemParam.GetInstance().InspDarkEnable == true)
                    return;

                HOperatorSet.GetImageSize(compImage, out htWidth, out htHeight);

                HOperatorSet.GenRectangle1(out hoRegion, 0, startPos, htHeight.I, endPos);
                HOperatorSet.ReduceDomain(compImage, hoRegion, out hoReduceDomain);
                if (!SystemParam.GetInstance().InspBrightEnable)
                {
                    HOperatorSet.Threshold(hoReduceDomain, out hoThresRegion, 129, 255);
                    HOperatorSet.OverpaintRegion(compImage, hoThresRegion, 128, "fill");
                }
                if (!SystemParam.GetInstance().InspDarkEnable)
                {
                    HOperatorSet.Threshold(hoReduceDomain, out hoThresRegion, 0, 127);
                    HOperatorSet.OverpaintRegion(compImage, hoThresRegion, 128, "fill");
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                if (hoRegion != null)
                    hoRegion.Dispose();
                if (hoThresRegion != null)
                    hoThresRegion.Dispose();
                if (hoReduceDomain != null)
                    hoReduceDomain.Dispose();
            }
        }


        /// <summary>
        /// 白い部分は検知しない（例：RGB全てが200以上のところ）
        /// </summary>
        /// <param name="img"></param>
        /// <param name="compImage"></param>
        /// <param name="camNo"></param>
        private void overLapWhiteBlack(ref HObject hoImage1, ref HObject hoImage2, ref HObject hoImage3, int camNo)
        {
            bool bWhite = SystemParam.GetInstance().OutofWhiteEnabled;
            bool bBlack = SystemParam.GetInstance().OutofBlackEnabled;
            int limitWhite = SystemParam.GetInstance().OutofWhiteLimit;
            int limitBlack = SystemParam.GetInstance().OutofBlackLimit;

            if (!bWhite && !bBlack)
                return;

            HObject hoMask;
            HObject hoReduceDomain1, hoReduceDomain2, hoReduceDomain3;
            HObject hoRegion1, hoRegion2, hoRegion3;
            HObject hoIntersection1, hoIntersection2;
            HOperatorSet.GenEmptyObj(out hoMask);
            HOperatorSet.GenEmptyObj(out hoReduceDomain1);
            HOperatorSet.GenEmptyObj(out hoReduceDomain2);
            HOperatorSet.GenEmptyObj(out hoReduceDomain3);
            HOperatorSet.GenEmptyObj(out hoRegion1);
            HOperatorSet.GenEmptyObj(out hoRegion2);
            HOperatorSet.GenEmptyObj(out hoRegion3);
            HOperatorSet.GenEmptyObj(out hoIntersection1);
            HOperatorSet.GenEmptyObj(out hoIntersection2);

            HTuple htWidth, htHeight;

            int startPos = _refreshImageEvent.MinMaxAveStartPos[camNo];
            int endPos = _refreshImageEvent.MinMaxAveEndPos[camNo];

            try
            {
                HOperatorSet.GetImageSize(hoImage1, out htWidth, out htHeight);
                HOperatorSet.GenRectangle1(out hoMask, 0, startPos, htHeight.I, endPos);
                HOperatorSet.ReduceDomain(hoImage1, hoMask, out hoReduceDomain1);
                HOperatorSet.ReduceDomain(hoImage2, hoMask, out hoReduceDomain2);
                HOperatorSet.ReduceDomain(hoImage3, hoMask, out hoReduceDomain3);

                if (bWhite)
                {
                    hoRegion1.Dispose();
                    hoRegion2.Dispose();
                    hoRegion3.Dispose();
                    HOperatorSet.Threshold(hoReduceDomain1, out hoRegion1, limitWhite, 255);
                    HOperatorSet.Threshold(hoReduceDomain2, out hoRegion2, limitWhite, 255);
                    HOperatorSet.Threshold(hoReduceDomain3, out hoRegion3, limitWhite, 255);
                    hoIntersection1.Dispose();
                    hoIntersection2.Dispose();
                    HOperatorSet.Intersection(hoRegion1, hoRegion2, out hoIntersection1);
                    HOperatorSet.Intersection(hoIntersection1, hoRegion3, out hoIntersection2);
                    HOperatorSet.OverpaintRegion(hoImage1, hoIntersection2, 128, "fill");
                    HOperatorSet.OverpaintRegion(hoImage2, hoIntersection2, 128, "fill");
                    HOperatorSet.OverpaintRegion(hoImage3, hoIntersection2, 128, "fill");
                }
                if (bBlack)
                {
                    hoRegion1.Dispose();
                    hoRegion2.Dispose();
                    hoRegion3.Dispose();
                    HOperatorSet.Threshold(hoReduceDomain1, out hoRegion1, 0, limitBlack);
                    HOperatorSet.Threshold(hoReduceDomain2, out hoRegion2, 0, limitBlack);
                    HOperatorSet.Threshold(hoReduceDomain3, out hoRegion3, 0, limitBlack);
                    hoIntersection1.Dispose();
                    hoIntersection2.Dispose();
                    HOperatorSet.Intersection(hoRegion1, hoRegion2, out hoIntersection1);
                    HOperatorSet.Intersection(hoIntersection1, hoRegion3, out hoIntersection2);
                    HOperatorSet.OverpaintRegion(hoImage1, hoIntersection2, 128, "fill");
                    HOperatorSet.OverpaintRegion(hoImage2, hoIntersection2, 128, "fill");
                    HOperatorSet.OverpaintRegion(hoImage3, hoIntersection2, 128, "fill");
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoMask.Dispose();
                hoReduceDomain1.Dispose();
                hoReduceDomain2.Dispose();
                hoReduceDomain3.Dispose();
                hoRegion1.Dispose();
                hoRegion2.Dispose();
                hoRegion3.Dispose();
                hoIntersection1.Dispose();
                hoIntersection2.Dispose();
            }
        }

        private void SideMaskFunc(HObject img, out HObject sideMaskRegion, int camNo)
        {
            const int maxTemp = 30;
            HObject[] hoTemp = new HObject[maxTemp];
            int tmpNo = 0;
            HObject hoImgRegion = null;
            HObject hoLeftRegion = null;
            HObject hoRightRegion = null;
            HObject hoSideRegion = null;
            HObject hoThresRegion = null;
            HObject hoSelObj = null;
            HObject hoIntersec = null;
            HObject hoSelShape = null;
            HObject hoMaskBefore = null;
            HObject hoMask = null;
            HObject hoNonMask = null;
            HObject hoSmallestRectMask = null;
            HTuple width, height;
            HTuple count;
            HTuple isSide;
            HTuple row1, col1, row2, col2;
            HTuple htCntChannel;

            int startPos = _refreshImageEvent.MinMaxAveStartPos[camNo];
            int endPos = _refreshImageEvent.MinMaxAveEndPos[camNo];
            int leftPos = _refreshImageEvent.LeftMaskWidthPix[camNo];
            int rightPos = _refreshImageEvent.RightMaskWidthPix[camNo];
            int left = startPos + leftPos;
            int right = endPos - rightPos;

            HOperatorSet.GenEmptyObj(out hoImgRegion);
            HOperatorSet.GenEmptyObj(out hoLeftRegion);
            HOperatorSet.GenEmptyObj(out hoRightRegion);
            HOperatorSet.GenEmptyObj(out hoSideRegion);
            HOperatorSet.GenEmptyObj(out hoThresRegion);
            HOperatorSet.GenEmptyObj(out hoSelObj);
            HOperatorSet.GenEmptyObj(out hoIntersec);
            HOperatorSet.GenEmptyObj(out hoSelShape);
            HOperatorSet.GenEmptyObj(out hoMaskBefore);
            HOperatorSet.GenEmptyObj(out hoMask);
            HOperatorSet.GenEmptyObj(out hoNonMask);
            HOperatorSet.GenEmptyObj(out hoSmallestRectMask);

            HObject hoReduceDomain;
            HOperatorSet.GenEmptyObj(out hoReduceDomain);
            try
            {
                sideMaskRegion = null;
                if (SystemParam.GetInstance().SideMaskEnable == false)
                    return;

                HOperatorSet.GetImageSize(img, out width, out height);
                int w = width.I - 1;
                int h = height.I - 1;

                //System.Diagnostics.Debug.WriteLine("=============================");
                //System.Diagnostics.Debug.WriteLine("camNo=" + camNo);
                //System.Diagnostics.Debug.WriteLine("img size=" + w + " , " + h);
                //System.Diagnostics.Debug.WriteLine("left=" + left);
                //System.Diagnostics.Debug.WriteLine("right=" + right);
                //System.Diagnostics.Debug.WriteLine("=============================");

                HOperatorSet.GenRectangle1(out hoLeftRegion, 0, startPos, h, startPos);
                HOperatorSet.GenRectangle1(out hoRightRegion, 0, endPos, h, endPos);
                HOperatorSet.Union2(hoLeftRegion, hoRightRegion, out hoSideRegion);

                HOperatorSet.GenRectangle1(out hoImgRegion, 0, startPos, h, endPos);

                HOperatorSet.ReduceDomain(img, hoImgRegion, out hoReduceDomain);

                HOperatorSet.CountChannels(hoReduceDomain, out htCntChannel);
                if (htCntChannel.I == 3)
                    HOperatorSet.Rgb1ToGray(hoReduceDomain, out hoTemp[++tmpNo]);
                else
                    HOperatorSet.CopyObj(hoReduceDomain, out hoTemp[++tmpNo], 1, -1);
                HOperatorSet.Threshold(hoTemp[tmpNo], out hoTemp[++tmpNo], SystemParam.GetInstance().SideMaskThreshold, 255);
                HOperatorSet.FillUp(hoTemp[tmpNo], out hoTemp[++tmpNo]);
                HOperatorSet.Complement(hoTemp[tmpNo], out hoTemp[++tmpNo]);
                HOperatorSet.Intersection(hoImgRegion, hoTemp[tmpNo], out hoTemp[++tmpNo]);
                HOperatorSet.DilationRectangle1(hoTemp[tmpNo], out hoTemp[++tmpNo], SystemParam.GetInstance().SideMaskDilation, 0.5);
                HOperatorSet.Intersection(hoImgRegion, hoTemp[tmpNo], out hoTemp[++tmpNo]);
                HOperatorSet.Connection(hoTemp[tmpNo], out hoTemp[++tmpNo]);
                HOperatorSet.SelectShape(hoTemp[tmpNo], out hoThresRegion, "area", "and", 1, "max");
                HOperatorSet.CountObj(hoThresRegion, out count);
                int cnt = count.I;
                for (int i = 0; i < cnt; i++)
                {
                    hoSelObj.Dispose();
                    hoIntersec.Dispose();
                    hoSelShape.Dispose();
                    HOperatorSet.SelectObj(hoThresRegion, out hoSelObj, i + 1);
                    HOperatorSet.Intersection(hoSelObj, hoSideRegion, out hoIntersec);
                    HOperatorSet.SelectShape(hoIntersec, out hoSelShape, "area", "and", 1, "max");
                    HOperatorSet.CountObj(hoSelShape, out isSide);
                    if (isSide.I > 0)
                    {
                        hoMaskBefore.Dispose();
                        HOperatorSet.CopyObj(hoMask, out hoMaskBefore, 1, -1);

                        //２値化によるマスクリージョン(ごつごつ)をSmallestRect1して、きれいなリージョンにする
                        HOperatorSet.SmallestRectangle1(hoSelObj, out row1, out col1, out row2, out col2);
                        hoSmallestRectMask.Dispose();
                        HOperatorSet.GenRectangle1(out hoSmallestRectMask, row1, col1, row2, col2);
                        hoSelObj.Dispose();
                        HOperatorSet.CopyObj(hoSmallestRectMask, out hoSelObj, 1, -1);

                        hoMask.Dispose();
                        HOperatorSet.ConcatObj(hoMaskBefore, hoSelObj, out hoMask);
                    }
                }
                HOperatorSet.Union1(hoMask, out hoTemp[++tmpNo]);
                HOperatorSet.Complement(hoTemp[tmpNo], out hoTemp[++tmpNo]);

                HOperatorSet.GenRectangle1(out hoNonMask, 0, (left < 0) ? 0 : left, h, (w < right) ? w : right);
                HOperatorSet.CountObj(hoTemp[tmpNo], out count);
                if (count.I == 0)
                    HOperatorSet.CopyObj(hoImgRegion, out hoTemp[++tmpNo], 1, -1);
                HOperatorSet.Union2(hoTemp[tmpNo], hoNonMask, out hoTemp[++tmpNo]);
                HOperatorSet.Intersection(hoTemp[tmpNo], hoImgRegion, out sideMaskRegion);
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                for (int i = 0; i < maxTemp; i++)
                {
                    if (hoTemp[i] != null)
                        hoTemp[i].Dispose();
                }
                hoImgRegion.Dispose();
                hoLeftRegion.Dispose();
                hoRightRegion.Dispose();
                hoSideRegion.Dispose();
                hoThresRegion.Dispose();
                hoSelObj.Dispose();
                hoIntersec.Dispose();
                hoSelShape.Dispose();
                hoMaskBefore.Dispose();
                hoMask.Dispose();
                hoNonMask.Dispose();
                hoSmallestRectMask.Dispose();

                hoReduceDomain.Dispose();
            }
            return;
        }

        /// <summary>
        /// 連結されているイメージで、両サイドのマスクを直線的にする
        /// </summary>
        /// <param name="img">連結イメージ</param>
        /// <param name="imaAlign">マスクを直線にしたイメージ</param>
        private void SideMaskFunc2(HObject img, out HObject imgAlign)
        {
            HOperatorSet.GenEmptyObj(out imgAlign);

            HObject hoEnableRectangle;
            HObject hoIntersection;
            HObject hoDomain;
            HObject hoDilation;
            HObject hoConnectedRegions;
            HObject hoSelectShapeObj;
            HObject hoSelectedObj;
            HObject hoRectangle;
            HObject hoConcatObj;
            HObject hoRegionUnion;
            HObject hoComplement;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoEnableRectangle);
            HOperatorSet.GenEmptyObj(out hoIntersection);
            HOperatorSet.GenEmptyObj(out hoDomain);
            HOperatorSet.GenEmptyObj(out hoDilation);
            HOperatorSet.GenEmptyObj(out hoConnectedRegions);
            HOperatorSet.GenEmptyObj(out hoSelectShapeObj);
            HOperatorSet.GenEmptyObj(out hoSelectedObj);
            HOperatorSet.GenEmptyObj(out hoRectangle);
            HOperatorSet.GenEmptyObj(out hoConcatObj);
            HOperatorSet.GenEmptyObj(out hoRegionUnion);
            HOperatorSet.GenEmptyObj(out hoComplement);
            HOperatorSet.GenEmptyObj(out dmy);

            HTuple htImgWidth, htImgHeight;
            HTuple htCount;
            HTuple row1, col1, row2, col2;
            int width, height;
            int count;

            try
            {
                HOperatorSet.GetImageSize(img, out htImgWidth, out htImgHeight);
                width = htImgWidth.I;
                height = htImgHeight.I;

                hoEnableRectangle.Dispose();
                HOperatorSet.GenRectangle1(out hoEnableRectangle, 0, 0, htImgHeight.I - 1, htImgWidth.I - 1);

                hoDomain.Dispose();
                HOperatorSet.GetDomain(img, out hoDomain);
                hoComplement.Dispose();
                HOperatorSet.Complement(hoDomain, out hoComplement);
                hoIntersection.Dispose();
                HOperatorSet.Intersection(hoEnableRectangle, hoComplement, out hoIntersection);
                hoConnectedRegions.Dispose();
                HOperatorSet.Connection(hoIntersection, out hoConnectedRegions);
                hoSelectShapeObj.Dispose();
                HOperatorSet.SelectShape(hoConnectedRegions, out hoSelectShapeObj, "area", "and", 1, "max");
                HOperatorSet.CountObj(hoSelectShapeObj, out htCount);
                count = htCount.I;

                for (int index = 0; index < count; index++)
                {
                    hoSelectedObj.Dispose();
                    HOperatorSet.SelectObj(hoSelectShapeObj, out hoSelectedObj, index + 1);
                    HOperatorSet.SmallestRectangle1(hoSelectedObj, out row1, out col1, out row2, out col2);
                    if (row1.D == 0 || row2.I == (height - 1))
                    {
                        hoRectangle.Dispose();
                        HOperatorSet.GenRectangle1(out hoRectangle, row1, col1, row2, col2);
                        //hoDilation.Dispose();
                        //HOperatorSet.DilationRectangle1(hoRectangle, out hoDilation, 2.5, 0.5);
                        dmy.Dispose();
                        HOperatorSet.CopyObj(hoConcatObj, out dmy, 1, -1);
                        hoConcatObj.Dispose();
                        HOperatorSet.ConcatObj(dmy, hoRectangle, out hoConcatObj);
                    }
                }
                hoSelectedObj.Dispose();
                HOperatorSet.SelectShape(hoConcatObj, out hoSelectedObj, "area", "and", 1, "max");
                HOperatorSet.CountObj(hoSelectedObj, out htCount);
                if (htCount.I !=0)
                {
                    hoRegionUnion.Dispose();
                    HOperatorSet.Union1(hoSelectedObj, out hoRegionUnion);
                    hoComplement.Dispose();
                    HOperatorSet.Complement(hoRegionUnion, out hoComplement);
                    hoIntersection.Dispose();
                    HOperatorSet.Intersection(hoEnableRectangle, hoComplement, out hoIntersection);
                    HOperatorSet.ReduceDomain(img, hoIntersection, out imgAlign);
                }
                else
                {
                    HOperatorSet.CopyObj(img, out imgAlign, 1, -1);
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoEnableRectangle.Dispose();
                hoIntersection.Dispose();
                hoDomain.Dispose();
                hoDilation.Dispose();
                hoConnectedRegions.Dispose();
                hoSelectShapeObj.Dispose();
                hoSelectedObj.Dispose();
                hoRectangle.Dispose();
                hoConcatObj.Dispose();
                hoRegionUnion.Dispose();
                hoComplement.Dispose();
                dmy.Dispose();
            }
        }


        private SummaryData[][,] _summaryData;

        private void ClearSoftShadingSummary()
        {
            for (int camNo = 0; camNo < CamInfos.Count; camNo++)
            for (int chNo = 0; chNo < _summaryData[camNo].GetLength(0); chNo++)
                for (int i = 0; i < _summaryData[camNo].GetLength(1); i++)
                    _summaryData[camNo][chNo, i] = new SummaryData(SystemParam.GetInstance().SoftShadingCalcImageCount);
        }

        private void softshading(HObject img, bool isPainting, out HObject shadingImage, out HObject inspScaleShadingImage, int camNo)
        {
            HOperatorSet.GenEmptyObj(out shadingImage);
            HOperatorSet.GenEmptyObj(out inspScaleShadingImage);

            HTuple htWidth, htHeight;
            HTuple htIntensityMean, htIntensityDeviation;
            HTuple htProjectionHorz, htProjectionVert;
            HObject hoRegion= null;
            int startPos = _refreshImageEvent.MinMaxAveStartPos[camNo];
            int endPos = _refreshImageEvent.MinMaxAveEndPos[camNo];

            HObject hoObjectSelected;
            HObject hoShadingObject;
            HObject hoConcatObject;
            HObject hoConcatScaleObject;
            HObject hoPaintImage;
            HObject hoScaledObj;
            HObject hoUnionObject;
            HObject hoImage1, hoImage2, hoImage3;
            HObject hoGrayImage;
            HObject hoAllObject;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoObjectSelected);
            HOperatorSet.GenEmptyObj(out hoShadingObject);
            HOperatorSet.GenEmptyObj(out hoConcatObject);
            HOperatorSet.GenEmptyObj(out hoConcatScaleObject);
            HOperatorSet.GenEmptyObj(out hoPaintImage);
            HOperatorSet.GenEmptyObj(out hoScaledObj);
            HOperatorSet.GenEmptyObj(out hoUnionObject);
            HOperatorSet.GenEmptyObj(out hoImage1);
            HOperatorSet.GenEmptyObj(out hoImage2);
            HOperatorSet.GenEmptyObj(out hoImage3);
            HOperatorSet.GenEmptyObj(out hoGrayImage);
            HOperatorSet.GenEmptyObj(out hoAllObject);
            HOperatorSet.GenEmptyObj(out dmy);
            HTuple htCntChannel;
            HTuple htNumber;
            int chCnt = 0;
            int number;

            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Restart();
                if (SystemParam.GetInstance().SoftShadingEnable == false)
                {
                    if (APCameraManager.getInstance().GetCamera(0).IsColor == false)
                    {
                        HOperatorSet.CopyObj(img, out shadingImage, 1, -1);
                        HOperatorSet.CopyObj(img, out inspScaleShadingImage, 1, -1);
                    }
                    else
                    {
                        HOperatorSet.CopyObj(img, out shadingImage, 1, -1);
                        UtilityImage.ConcatColor2ConcatObject4(img, out inspScaleShadingImage);
                    }
                    return;
                }

                HOperatorSet.CountChannels(img, out htCntChannel);
                chCnt = htCntChannel.I;
                if (chCnt== 1)
                {
                    HOperatorSet.CopyObj(img, out hoAllObject, 1, -1);
                }
                else
                {
                    UtilityImage.ConcatColor2ConcatObject4(img, out hoAllObject);
                }

                //描画用に高さをとっておく
                HOperatorSet.GetImageSize(img, out htWidth, out htHeight);
                int h = htHeight.I;

                int topHeight = 0;
                if (SystemParam.GetInstance().IM_GraphCalcAreaAll == false)
                {
                    int startHeight, endHeight, underHeight;
                    SystemParam.GetInstance().GetImageHeightArea(htHeight.I, out startHeight, out endHeight, out underHeight);
                    topHeight = underHeight;
                }
                HOperatorSet.GenRectangle1(out hoRegion, topHeight, startPos, h - 1, endPos);


                HOperatorSet.CountObj(hoAllObject, out htNumber);
                number = htNumber.I;

                sw.Stop();
                //Console.WriteLine("01_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

                int newWidthLength = endPos - startPos + 1;
                if (_summaryData[camNo].GetLength(1) != newWidthLength)
                {
                    _summaryData[camNo] = new SummaryData[number, newWidthLength];
                    for (int chNo = 0; chNo < _summaryData[camNo].GetLength(0); chNo++)
                        for (int i = 0; i < _summaryData[camNo].GetLength(1); i++)
                            _summaryData[camNo][chNo, i] = new SummaryData(SystemParam.GetInstance().SoftShadingCalcImageCount);
                }

                sw.Stop();
                //Console.WriteLine("02_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

                _resetEvent[camNo] = new AutoResetEvent[number];
                _hoBeforeShadingImage[camNo] = new HObject[number];
                _hoAfterShadingImage[camNo] = new HObject[number];
                _hoAfterScaleShadingImage[camNo] = new HObject[number];
                _iStartPos[camNo] = startPos;
                _iEndPos[camNo] = endPos;
                _dAdCoeff[camNo] = new double[number][];
                _iTopHeight[camNo] = topHeight;
                _iScaleShift[camNo] = new int[number];

                double[] dIntensityMean = new double[number];
                Thread[] thShading = new Thread[number];

                sw.Stop();
                //Console.WriteLine("03_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

                for (int chNo = 0; chNo < number; chNo++)
                {
                    hoObjectSelected.Dispose();
                    HOperatorSet.SelectObj(hoAllObject, out hoObjectSelected, chNo + 1);

                    hoPaintImage.Dispose();
                    if (isPainting == true && SystemParam.GetInstance().ColorCamInspImage[chNo] == true)
                        overLapCompetion(hoObjectSelected, out hoPaintImage, camNo);
                    else
                        HOperatorSet.CopyObj(hoObjectSelected, out hoPaintImage, 1, -1);

                    // シェーディングテストエリアの平均明度取得
                    HOperatorSet.Intensity(hoRegion, hoPaintImage, out htIntensityMean, out htIntensityDeviation);

                    // 垂直方向の各要素における平均値明度取得
                    HOperatorSet.GrayProjections(hoRegion, hoPaintImage, "simple", out htProjectionHorz, out htProjectionVert);

                    // 係数を算出
                    double[] adCoeff = new double[htProjectionVert.Length];
                    double[] dAves = htProjectionVert.DArr;
                    double low, high;
                    double targetGrayLevel;
                    int limit = SystemParam.GetInstance().SoftShadingLimit;

                    dIntensityMean[chNo] = htIntensityMean.D;
                    targetGrayLevel = dIntensityMean[chNo];

                    _iScaleShift[camNo][chNo] = SystemParam.GetInstance().SoftShadingTargetGrayLevel - (int)dIntensityMean[chNo];

                    for (int i = 0; i < adCoeff.Length; i++)
                    {
                        low = targetGrayLevel - limit;
                        high = targetGrayLevel + limit;
                        adCoeff[i] = targetGrayLevel / ((low <= dAves[i] && dAves[i] <= high) ? dAves[i] : targetGrayLevel);

                        _summaryData[camNo][chNo, i].Update(adCoeff[i], true);
                        adCoeff[i] = _summaryData[camNo][chNo, i].dataMean;
                    }


                    hoShadingObject.Dispose();
                    if (SystemParam.GetInstance().ColorCamInspImage[chNo] == true)
                    {
#if true
                        _resetEvent[camNo][chNo] = new AutoResetEvent(false);
                        HOperatorSet.CopyObj(hoObjectSelected, out _hoBeforeShadingImage[camNo][chNo], 1, 1);
                        _dAdCoeff[camNo][chNo] = adCoeff;
                        thShading[chNo] = new Thread(new ParameterizedThreadStart(shadingThread));
                        thShading[chNo].Name = "shading cam" + camNo.ToString() + "-" + (chNo + 1).ToString();
#else
                        if (!shading_fix_unsafe(hoObjectSelected, out _hoAfterShadingImage[camNo][chNo], startPos, endPos, adCoeff, topHeight))
                            return;
                        HOperatorSet.ScaleImage(_hoAfterShadingImage[camNo][chNo], out _hoAfterScaleShadingImage[camNo][chNo], 1, _iScaleShift[camNo][chNo]);
#endif
                        //if (!shading_fix_unsafe(hoObjectSelected, out hoShadingObject, startPos, endPos, adCoeff, topHeight))
                        //    return;
                    }
                    else
                    {
                        HOperatorSet.CopyObj(hoObjectSelected, out _hoAfterShadingImage[camNo][chNo], 1, 1);
                        HOperatorSet.CopyObj(hoObjectSelected, out _hoAfterScaleShadingImage[camNo][chNo], 1, 1);
                        //HOperatorSet.CopyObj(hoObjectSelected, out hoShadingObject, 1, -1);
                    }
                }

                sw.Stop();
                //Console.WriteLine("04_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

#if true
                for (int chNo = 0; chNo < number; chNo++)
                {
                    if (thShading[chNo] != null)
                        thShading[chNo].Start(new int[] { camNo, chNo });
                }
#endif
                sw.Stop();
                //Console.WriteLine("Wait START  time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();
#if true
                bool bWait = true;
                while (bWait)
                {
                    for (int i = 0; i < _resetEvent[camNo].Length; i++)
                    {
                        if (_resetEvent[camNo][i] != null)
                            _resetEvent[camNo][i].WaitOne();
                    }
                    bWait = false;
                }
#endif
                sw.Stop();
                //Console.WriteLine("Wait END  time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

                for (int chNo = 0; chNo < number; chNo++)
                { 
                    dmy.Dispose();
                    HOperatorSet.CopyObj(hoConcatObject, out dmy, 1, -1);
                    hoConcatObject.Dispose();
                    HOperatorSet.ConcatObj(dmy, _hoAfterShadingImage[camNo][chNo], out hoConcatObject);

                    //int shift = SystemParam.GetInstance().SoftShadingTargetGrayLevel - (int)dIntensityMean[chNo];
                    //hoScaledObj.Dispose();
                    //HOperatorSet.ScaleImage(_hoAfterShadingImage[camNo][chNo], out hoScaledObj, 1, shift);
                    HOperatorSet.CopyObj(_hoAfterScaleShadingImage[camNo][chNo], out hoScaledObj, 1, 1);

                    hoPaintImage.Dispose();
                    if (isPainting == true && SystemParam.GetInstance().ColorCamInspImage[chNo] == true)
                        overLapCompetion(hoScaledObj, out hoPaintImage, camNo);
                    else
                        HOperatorSet.CopyObj(hoScaledObj, out hoPaintImage, 1, -1);

                    dmy.Dispose();
                    HOperatorSet.CopyObj(hoConcatScaleObject, out dmy, 1, -1);
                    hoConcatScaleObject.Dispose();
                    HOperatorSet.ConcatObj(dmy, hoPaintImage, out hoConcatScaleObject);

                    if (_hoAfterScaleShadingImage[camNo][chNo] != null)
                        _hoAfterScaleShadingImage[camNo][chNo].Dispose();
                    if (_hoAfterShadingImage[camNo][chNo] != null)
                        _hoAfterShadingImage[camNo][chNo].Dispose();
                    if (_hoBeforeShadingImage[camNo][chNo] != null)
                        _hoBeforeShadingImage[camNo][chNo].Dispose();
                }

                sw.Stop();
                //Console.WriteLine("05_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();

                if (chCnt == 1)
                {
                    HOperatorSet.CopyObj(hoConcatScaleObject, out shadingImage, 1, -1);
                    HOperatorSet.CopyObj(hoConcatScaleObject, out inspScaleShadingImage, 1, -1);
                }
                else
                {
                    hoImage1.Dispose();
                    hoImage2.Dispose();
                    hoImage3.Dispose();
                    HOperatorSet.SelectObj(hoConcatObject, out hoImage1, 2);
                    HOperatorSet.SelectObj(hoConcatObject, out hoImage2, 3);
                    HOperatorSet.SelectObj(hoConcatObject, out hoImage3, 4);
                    HOperatorSet.Compose3(hoImage1, hoImage2, hoImage3, out shadingImage);
                    HOperatorSet.CopyObj(hoConcatScaleObject, out inspScaleShadingImage, 1, -1);
                }

                sw.Stop();
                //Console.WriteLine("06_time=" + sw.ElapsedMilliseconds.ToString());
                sw.Restart();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (hoRegion != null)
                    hoRegion.Dispose();
                hoObjectSelected.Dispose();
                hoShadingObject.Dispose();
                hoConcatObject.Dispose();
                hoConcatScaleObject.Dispose();
                hoPaintImage.Dispose();
                hoScaledObj.Dispose();
                hoUnionObject.Dispose();
                hoImage1.Dispose();
                hoImage2.Dispose();
                hoImage3.Dispose();
                hoGrayImage.Dispose();
                hoAllObject.Dispose();
                dmy.Dispose();
            }
            return;
        }

        List<AutoResetEvent[]> _resetEvent;
        List<HObject[]> _hoBeforeShadingImage;
        List<HObject[]> _hoAfterShadingImage;
        List<HObject[]> _hoAfterScaleShadingImage;
        List<int> _iStartPos;
        List<int> _iEndPos;
        List<double[][]> _dAdCoeff;
        List<int> _iTopHeight;
        List<int[]> _iScaleShift;

        private void shadingThread(object obj)
        {
            int[] datas = (int[])obj;
            int iCamNo = datas[0];
            int iIndex = datas[1];

            shading_fix_unsafe(_hoBeforeShadingImage[iCamNo][iIndex], out _hoAfterShadingImage[iCamNo][iIndex], _iStartPos[iCamNo], _iEndPos[iCamNo], _dAdCoeff[iCamNo][iIndex], _iTopHeight[iCamNo]);
            HOperatorSet.ScaleImage(_hoAfterShadingImage[iCamNo][iIndex], out _hoAfterScaleShadingImage[iCamNo][iIndex], 1, _iScaleShift[iCamNo][iIndex]);
            //HOperatorSet.CopyObj(_hoAfterShadingImage[iCamNo][iIndex], out _hoAfterScaleShadingImage[iCamNo][iIndex], 1, 1);
            _resetEvent[iCamNo][iIndex].Set();
        }

        unsafe private bool shading_unsafe(HObject src, out HObject dest, int iStart, int iEnd, double[] adCoeff, int topHeight)
        {
            HTuple htPointer, htType, htWidth, htHeight;
            HTuple htVerticalPitch, htHorizontalBitPitch, htBitsPerPixel;

            HOperatorSet.CopyObj(src, out dest, 1, -1);
            HOperatorSet.GetImagePointer1(dest, out htPointer, out htType, out htWidth, out htHeight);
            HOperatorSet.GetImagePointer1Rect(dest, out htPointer, out htWidth, out htHeight, out htVerticalPitch, out htHorizontalBitPitch, out htBitsPerPixel);

            int iVerticalPitch = htVerticalPitch.I;
            int iHeight = htHeight.I;

            // Imageのタイプがbyteじゃない場合、NG
            if (htType != "byte")
                return false;

            // 配列の大きさが、iEnd - iStart - 1じゃない場合NG
            if (adCoeff.Length != iEnd - iStart + 1)
                return false;

            // ポインタの位置を取得する
            byte* pPixOrg = (byte*)htPointer.L;
            byte* pPix;
            int iSize = iEnd - iStart + 1;
            for (int y = iHeight; y < iHeight; y++)
            {
                pPix = pPixOrg + iVerticalPitch * y + iStart;
                for (int x = 0; x < iSize; x++, pPix++)
                {
                    double dValue = *pPix * adCoeff[x];
                    dValue = dValue > 255.0 ? 255.0 : dValue;
                    *pPix = (byte)Math.Round(dValue);
                }
            }
            return true;
        }

        /// <summary>
        /// シェーディングの実態（補正計算に固定小数点を使う）
        /// 浮動小数点よりも2倍以上早い為こっちを採用する
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="iStart"></param>
        /// <param name="iEnd"></param>
        /// <param name="adCoeff"></param>
        /// <param name="iShift"></param>
        /// <returns></returns>
        unsafe private bool shading_fix_unsafe(HObject src, out HObject dest, int iStart, int iEnd, double[] adCoeff, int topHeight, int iShift = 24)
        {
            HTuple htPointer, htType, htWidth, htHeight;
            HTuple htVerticalPitch, htHorizontalBitPitch, htBitsPerPixel;

            HOperatorSet.CopyObj(src, out dest, 1, -1);
            HOperatorSet.GetImagePointer1(dest, out htPointer, out htType, out htWidth, out htHeight);
            HOperatorSet.GetImagePointer1Rect(dest, out htPointer, out htWidth, out htHeight, out htVerticalPitch, out htHorizontalBitPitch, out htBitsPerPixel);

            int iVerticalPitch = htVerticalPitch.I;
            int iHeight = htHeight.I;

            // Imageのタイプがbyteじゃない場合、NG
            if (htType != "byte")
                return false;

            // 配列の大きさが、iEnd - iStart - 1じゃない場合NG
            if (adCoeff.Length != iEnd - iStart + 1)
                return false;

            if (iShift < 1 || iShift > 48)
                return false;

            // 係数算出(浮動小数点→8_24固定小数点)
            long[] alCoeffFix = new long[adCoeff.Length];
            long lShiftFix = (long)1 << iShift;
            // 四捨五入用
            long lRoundFix = (long)1 << (iShift - 1);
            // 固定小数点による係数を計算する
            for (int i = 0; i < adCoeff.Length; i++)
                alCoeffFix[i] = (long)Math.Round(adCoeff[i] * lShiftFix);

            // 画像イメージのポインタを取得する
            byte* pPixOrg = (byte*)htPointer.L;
            byte* pPix;
            int iSize = alCoeffFix.Length;
            for (int y = topHeight; y < iHeight; y++)
            {
                pPix = pPixOrg + iVerticalPitch * y + iStart;
                for (int x = 0; x < iSize; x++, pPix++)
                {
                    long lVal = (*pPix * alCoeffFix[x] + lRoundFix) >> iShift;
                    *pPix = (byte)(lVal > byte.MaxValue ? byte.MaxValue : lVal);
                }
            }
            return true;
        }
    }






    /// <summary>
    /// 検査結果集計を格納するクラス。
    /// </summary>
    public class SummaryData
    {
        /// <summary>
        /// 平均。
        /// </summary>
        public double dataMean { private set; get; }

        /// <summary>
        /// インスタンスを初期化する。
        /// </summary>
        public SummaryData(int meanMax)
        {
            //平均値を算出するValue数（0は無制限）
            this._meanMax = meanMax;
            _lstValues = new List<double>();
            this.Clear();
        }
        private void DataClear()
        {
            this.dataMean = 0.0;
            _s1 = 0.0;
            _s2 = 0.0;
            _n = 0;
        }
        public void Clear()
        {
            this.DataClear();
            _lstValues.Clear();
        }

        private int _meanMax;
        private List<double> _lstValues;

        /// <summary>仮平均</summary>
        private double _s1 = 0.0;
        /// <summary>平方和</summary>
        private double _s2 = 0.0;
        /// <summary>カウンタ</summary>
        private int _n = 0;

        /// <summary>
        /// 検査結果集計を更新する。
        /// </summary>
        /// <param name="value">結果値。</param>
        /// <param name="ignoreZero">結果値が0の場合は更新しない。</param>
        public void Update(double value, bool ignoreZero)
        {
            if (ignoreZero && value == 0.0)
            {
                return;
            }
            // 平均と標準偏差を計算する
            try
            {
                if (_meanMax != 0)
                {
                    _lstValues.Add(value);
                    if (_lstValues.Count > _meanMax)
                        _lstValues.RemoveAt(0);
                    this.DataClear();
                    for (int i = 0; i < _lstValues.Count; i++)
                    {
                        CalcData(_lstValues[i]);
                    }
                }
                else
                {
                    CalcData(value);
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.WriteLine(exc.Message);
            }
            // 平均値を更新する
            this.dataMean = _s1;
        }
        private void CalcData(double value)
        {
            double x = value;
            _n++;
            x -= _s1;
            _s1 += x / _n;
            _s2 += (_n - 1) * x * x / _n;
        }
    }

    //アクション識別
    public enum EResultActionId
    {
        Stop,
        Suspend,
        Start,
        Restart,
        Judge,
        Reset,
    }
    //ステータス
    enum EInspectionStatus
    {
        Stop,
        Suspend,
        Start,
        Restart,
        Reset,
    }

}
