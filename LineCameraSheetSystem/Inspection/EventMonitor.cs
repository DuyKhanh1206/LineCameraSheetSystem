using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using LogingDllWrap;
using LineCameraSheetSystem;
using ResultActionDataClassNameSpace;
using Fujita.Misc;
using HalconDotNet;
using System.IO;

namespace InspectionNameSpace
{
    public class EventMonitor : IDisposable, IError
    {
		public bool IsError { get; set; }
		public string ErrorReason { get; set; }

        #region イベント　パラメータ
        public class EventMonitorEventArgs : EventArgs
        {
            /// <summary>
            /// 先頭の通し番号(1～)
            /// </summary>
            public int TopLineNo { get; private set; }
			/// <summary>
			/// 先頭の結果通し番号(1～)
			/// </summary>
			public int TopResultNo { get; private set; }
            /// <summary>
            /// アクション識別
            /// </summary>
            public EResultActionId Id { get; private set; }
            /// <summary>
            /// イベント識別
            /// </summary>
            public ResultActionDataClass.EEventId EventId { get; private set; }
            /// <summary>
            /// イベントモード
            /// </summary>
            public ResultActionDataClass.EEventMode EventMode { get; private set; }
            /// <summary>
            /// 発生時刻
            /// </summary>
            public DateTime Time { get; private set; }
            /// <summary>
            /// 項目数
            /// </summary>
            public int ItemCount { get; private set; }

            /// <summary>
            /// トータルのイメージ開始位置
            /// </summary>
            public double StartLength { get; private set; }
            /// <summary>
            /// トータルのイメージ終了位置
            /// </summary>
            public double EndLength { get; private set; }

            /// <summary>
            /// シート幅or厚み　値
            /// </summary>
            public double SheetValue { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="cInfo"></param>
            /// <param name="time"></param>
            /// <param name="startLength"></param>
            /// <param name="endLength"></param>
            /// <param name="resDatas"></param>
            public EventMonitorEventArgs(int topLineNo, int topResultNo, EResultActionId id, DateTime time, double st, double end, int count, ResultActionDataClass.EEventId eId, ResultActionDataClass.EEventMode eMode, double sheetValue)
            {
                this.TopLineNo = topLineNo;
				this.TopResultNo = TopResultNo;
                this.Id = id;
                this.Time = time;
                this.StartLength = st;
                this.EndLength = end;
                this.ItemCount = count;
                this.EventId = eId;
                this.EventMode = eMode;
                this.SheetValue = sheetValue;
            }
        }
        #endregion

        #region イベント　ハンドラ
        public delegate void UpdateResultActionEventHandler(object sender, EventMonitorEventArgs e);
        public event UpdateResultActionEventHandler OnEventUpdateResultAction;
        #endregion

        #region 内部プロパティ
        /// <summary>
        /// カメラ情報
        /// </summary>
        //public List<CameraInfo> CamInfos { get; private set; }
        /// <summary>
        /// 結果・アクション管理クラス
        /// </summary>
        private ResultActionDataClass _resultActionDataClass;

        //結果データ同期
        private SyncResultEvent _syncResultEvent;
        #endregion

        #region プロパティ
        /// <summary>
        /// 品種名
        /// </summary>
        public string Hinsyu { get; set; }
        /// <summary>
        /// LotNo
        /// </summary>
        public string LotNo { get; set; }
        /// <summary>
        /// 結果を保存するシステムフォルダ
        /// </summary>
        public string SystemResultDir { get; set; }
        /// <summary>
        /// イメージを保存するシステムフォルダ
        /// </summary>
        public string SystemImageDir { get; set; }
        /// <summary>
        /// NGを集約する範囲(mm)
        /// </summary>
        public double OverlapRange { get; set; }
        /// <summary>
        /// Imageフォルダ配下の有限フォルダ数
        /// </summary>
        public int ImageNumDirMax { get; set; }
        /// <summary>
        /// Image\Numberフォルダ内の最大イメージ数
        /// </summary>
        public int ImageNumDirFileMax { get; set; }
        #endregion


        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resultDataClass"></param>
        public EventMonitor(ResultActionDataClass resultDataClass, List<CameraInfo> camInfos)
        {
            //結果管理クラス
            this._resultActionDataClass = resultDataClass;
            //全カメラ結果同期
            this._syncResultEvent = new SyncResultEvent(SystemParam.GetInstance().camParam);

            //同期スレッド
            _qactThread = new Thread(new ThreadStart(ThreadActionEvent));
            _qactThread.Name = "EventMonitor.ActionThreadProc";

			//
			_____sw = new Stopwatch();
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            EndEventMonitor();
        }
        #endregion


        #region 監視の開始・終了
        /// <summary>
        /// 開始する
        /// </summary>
        public void BeginEventMonitor()
        {
            _stopQueueThread = false;
            _qactThread.Start();
        }
        /// <summary>
        /// 終了する
        /// </summary>
        public void EndEventMonitor()
        {
            //スレッドを停止する
            _stopQueueThread = true;
            _qactThread.Join();

            Clear();
        }
        #endregion


        /// <summary>
        /// 結果をクリアする
        /// </summary>
        public void Clear()
        {
            //
            this._syncResultEvent.Reset();
            //
            foreach (ImageResultData res in this._imgResDatas)
            {
                res.Dispose();
            }
            this._imgResDatas.Clear();
        }



        #region アクション通知をメインへ通知する制御

        private QueuingAction _queueAction = new QueuingAction();
        private Thread _qactThread;
        private bool _stopQueueThread = true;
        /// <summary>
        /// アクション通知スレッド
        /// </summary>
        private void ThreadActionEvent()
        {
            HObject hoSaveImages;
            HOperatorSet.GenEmptyObj(out hoSaveImages);
			while (true)
			{
                QueuingAction.QueueData qData;
                //キューを取得する
                lock (_queueAction)
                {
                    qData = _queueAction.Dequeue();
                }
				if (_stopQueueThread == true && qData == null)
				{
					break;
				}

				if (qData != null)
				{
					//開始通知ならば、結果集計を開始する
					if (qData.ResultActionId == EResultActionId.Start)
					{
						this.ErrorReason = "";
						this.IsError = false;
						this.StartRealTimeDataClass(qData.Time);                        
					}

					ResultActionDataClass.EEventMode eventMode;
                    double sheetValue = 0.0;
					int listCount = 0;
					int TopLineNo = this._resultActionDataClass.LineCount + 1;
					int TopResultNo = -1;
					if (qData.ResultActionId == EResultActionId.Judge)
					{
						TopResultNo = this._resultActionDataClass.ResultCount + 1;
                        
						//結果データを格納する
						lock (_imgResDatas)
						{
                            //次画像のTop部領域で、この場所でNGした場合NGとするために、EndLengthにプラスしている
                            SystemParam sysparam = SystemParam.GetInstance();
                            double dUnderBufferArea = sysparam.InspArea_ConnectMode_BufferArea * sysparam.camParam[qData.CamNo].ResoV;

                            //対象データ（検索して削除する）
                            Predicate<ImageResultData> ddd = new Predicate<ImageResultData>((m) => m.PositionY <= (qData.EndLength + dUnderBufferArea));
                            //対象のデータを取得する
                            List<ImageResultData> imageDatas = _imgResDatas.FindAll(ddd);
							//位置の早い順番にソートする
							imageDatas.Sort(Compare);
                            bool firstFlag = true;
                            //
                            foreach (ImageResultData dt in imageDatas)
							{
								//結果データを登録する
								int lineNo = this._resultActionDataClass.LineCount + 1;
								int resultNo = this._resultActionDataClass.ResultCount + 1;
								ResActionData data = new ResActionData(lineNo, resultNo, qData.EventId, qData.EventMode, qData.StartLength, qData.EndLength, qData.Time);
								//NGデータ
								data.ImageNumDirFileMax = this.ImageNumDirFileMax;
								string imageFileName = this._resultActionDataClass.NextImageDirInfo();
                                hoSaveImages.Dispose();
								data.CreateDetail(dt.CamId, dt.SideId, dt.InspId, dt.PositionX, dt.PositionY, dt.Width, dt.Height, dt.Area, dt.ZoneId,
                                    dt.ImageNG,
                                    imageFileName, this._resultActionDataClass.SystemImageDir,
                                    out hoSaveImages);
                                this.SaveImageThreadStart(this._resultActionDataClass.SystemImageDir, imageFileName, hoSaveImages);
                                this._resultActionDataClass.SetItemData(data);

                                if (firstFlag)
                                {
                                    System.Diagnostics.Debug.WriteLine("CALL[AddNgHistoryData()] CamId=" + dt.CamId.ToString() + "   SideId=" + dt.SideId.ToString());
                                    AddNgHistoryData((int)dt.CamId, dt.TargetImage, dt.TargetImage, dt.TargetImage);
                                    //AddNgHistoryData((int)dt.CamId, qData.ImageOrgs, qData.ImageTargets, qData.ImageInspScales);
                                    firstFlag = false;
                                }

                                dt.Dispose();
							}
							//NG数
							listCount = imageDatas.Count;
                            lock(_imgResDatas)
    							_imgResDatas.RemoveAll(ddd);
						}
						if (listCount > 0)
						{
							eventMode = ResultActionDataClass.EEventMode.NG;
                        }
                        else
						{
							TopLineNo = -1;
							eventMode = ResultActionDataClass.EEventMode.OK;
						}
					}
                    else
                    {
                        this.Clear();
                        //Action数
                        listCount = 1;
                        eventMode = qData.EventMode;
                        //操作データを登録する
                        int lineNo = this._resultActionDataClass.LineCount + 1;
                        int resultNo = -1;
                        //イベントモードが”Reset”の場合、”Stop”を変換して書き込む
                        ResultActionDataClass.EEventMode emode = (qData.EventMode == ResultActionDataClass.EEventMode.Reset) ? ResultActionDataClass.EEventMode.Stop : qData.EventMode;
                        ResActionData data = new ResActionData(lineNo, resultNo, qData.EventId, emode, qData.StartLength, qData.EndLength, qData.Time);
                        //削除する
                        this._resultActionDataClass.SetItemData(data);
                    }

					//this._resultActionDataClass.RemoveItemData(qData.StartLength);

					//停止通知ならば結果集計を終了する
					if (qData.ResultActionId == EResultActionId.Stop || qData.ResultActionId == EResultActionId.Reset)
					{
						try
						{
							_resultActionDataClass.EndRealtime(qData.Recipe);
						}
						catch (Exception exc)
						{
							string ErrStr = string.Format("EventMonitor.ThreadActionEvent() exc = {0}", exc.Message);
							LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
							Debug.WriteLine(ErrStr);

							this.ErrorReason = ErrStr;
							this.IsError = true;
						}
					}

                    //if (qData.CamNo == 0)
                    {
                        //アクションを通知する
                        EResultActionId id = (qData.ResultActionId == EResultActionId.Restart) ? EResultActionId.Start : qData.ResultActionId;
                        EventMonitorEventArgs args = new EventMonitorEventArgs(TopLineNo, TopResultNo, id, qData.Time, qData.StartLength, qData.EndLength, listCount, qData.EventId, eventMode, sheetValue);
                        if (OnEventUpdateResultAction != null)
                            OnEventUpdateResultAction(this, args);

                        if (eventMode != ResultActionDataClass.EEventMode.OK)
                        {
                            LogingDll.Loging_SetLogString(string.Format("EventMonitor.ThreadActionEvent() : id={0} listCount={1}", id.ToString(), listCount));
                        }
                    }

					//キュー解放
					qData.Dispose();
				}
				Thread.Sleep(1);
			}
            hoSaveImages.Dispose();
        }

        public int GetCropImageQueueCount
        {
            get
            {
                int iCnt;
                lock (_thCropImage)
                    iCnt = _thCropImage.Count;
                return iCnt;
            }
        }

        List<Thread> _thCropImage = new List<Thread>();
        
        public class clsSaveCropImage
        {
            public string DirectoryName { get; private set; }
            public string FileName { get; private set; }
            public HObject hoImage { get { return _hoImage; } }
            private HObject _hoImage;
            public clsSaveCropImage(string dirName, string fName, HObject img)
            {
                DirectoryName = dirName;
                FileName = fName;
                HOperatorSet.CopyImage(img, out _hoImage);
            }
            public void Clear()
            {
                if (hoImage!=null)
                    hoImage.Dispose();
            }
        }
        private Queue<clsSaveCropImage> _queueSaveCropImage = new Queue<clsSaveCropImage>();
        private void SaveImageThreadStart(string dirName, string fName, HObject image)
        {
            lock (_queueSaveCropImage)
            {
                _queueSaveCropImage.Enqueue(new clsSaveCropImage(dirName, fName, image));
            }
            Thread th;
            th = new Thread(new ThreadStart(SaveImage));
            lock (_thCropImage)
                _thCropImage.Add(th);
            th.Name = "SaveImageｽﾚｯﾄﾞ";
            th.Start();
        }
        /// <summary>
        /// イメージを保存する
        /// </summary>
        private void SaveImage()
        {
            clsSaveCropImage clsSaveCrop;
            lock (_queueSaveCropImage)
            {
                clsSaveCrop = _queueSaveCropImage.Dequeue();
            }
            HObject images = clsSaveCrop.hoImage;
            string dirName = clsSaveCrop.DirectoryName;
            string fName = clsSaveCrop.FileName;

            HObject targetImage;
            HOperatorSet.GenEmptyObj(out targetImage);

            HTuple htCount;
            int iCnt;
            try
            {
                if (images != null)
                {
                    string dir;
                    string file;
                    string[] fooder;
                    SystemParam.GetInstance().GetDirAndFile(dirName, fName, out dir, out file, out fooder);
                    HOperatorSet.CountObj(images, out htCount);
                    iCnt = fooder.Length;
                    for (int i = 0; i < iCnt && i < SystemParam.GetInstance().IM_NgCropSaveCount; i++)
                    {
                        targetImage.Dispose();
                        //
                        HOperatorSet.SelectObj(images, out targetImage, i + 1);
                        HOperatorSet.WriteImage(targetImage, "bmp", 0, Path.Combine(dir, file) + fooder[i] + ".bmp");
                    }
                }
                lock (_thCropImage)
                {
                    if (_thCropImage.Count > 0)
                        _thCropImage.RemoveAt(0);
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                targetImage.Dispose();
                clsSaveCrop.Clear();
            }
        }


        /// <summary>
        /// Y位置ソート
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        private static int Compare(ImageResultData data1, ImageResultData data2)
        {
            if (data1.PositionY < data2.PositionY)
            {
                return -1;
            }
            else if (data1.PositionY > data2.PositionY)
            {
                return 1;
            }
            else
            {
                if (data1.PositionX < data2.PositionX)
                {
                    return -1;
                }
                else if (data1.PositionX > data2.PositionX)
                {
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// データ管理を開始する
        /// </summary>
        private void StartRealTimeDataClass(DateTime stTime)
        {
            _resultActionDataClass.ImageNumDirMax = this.ImageNumDirMax;
            _resultActionDataClass.ImageNumDirFileMax = this.ImageNumDirFileMax;
            _resultActionDataClass.SystemResultDir = this.SystemResultDir;
            _resultActionDataClass.SystemImageDir = this.SystemImageDir;
            _resultActionDataClass.StartRealtime(this.Hinsyu, this.LotNo, stTime);
        }
        #endregion


        /// <summary>
        /// 結果のバッファ領域
        /// </summary>
        private List<ImageResultData> _imgResDatas = new List<ImageResultData>();


        #region NG履歴データ
        public class NgHistoryData
        {
            public int CamNo { get; private set; }
            public HObject ImageOrgs { get { return _imgOrgs; } }
            public HObject ImageTargets { get { return _imgTargets; } }
            public HObject ImageInspScales { get { return _imgInspScales; } }
            private HObject _imgOrgs;
            private HObject _imgTargets;
            private HObject _imgInspScales;
            public NgHistoryData(int camNo, HObject imgOrgs, HObject imgTargets, HObject imgInspScales)
            {
                CamNo = camNo;
                if (imgOrgs != null)
                    HOperatorSet.CopyImage(imgOrgs, out _imgOrgs);
                if (imgTargets != null)
                    HOperatorSet.CopyImage(imgTargets, out _imgTargets);
                if (imgInspScales != null)
                    HOperatorSet.CopyImage(imgInspScales, out _imgInspScales);
            }
            public NgHistoryData Copy()
            {
                NgHistoryData res = new NgHistoryData(this.CamNo, this._imgOrgs, this._imgTargets, this._imgInspScales);
                return res;
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
        private List<NgHistoryData> _lstNgHistoryData;
        /// <summary>
        /// NG履歴　データ取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NgHistoryData GetNgHistoryData(int index)
        {
            NgHistoryData res = null;
            if (_lstNgHistoryData != null)
            {
                lock (_lstNgHistoryData)
                {
                    if (index < _lstNgHistoryData.Count)
                        res = _lstNgHistoryData[index].Copy();
                }
            }
            return res;
        }
        public string GetNgHistoryDataTime(int index)
        {
            string strTime = "-";
            if (_lstNgHistoryData != null)
            {
                lock (_lstNgHistoryData)
                {
                    if (index < _lstNgHistoryData.Count)
                    {
                        try
                        {
                            HTuple htMSecond, htSecond, htMinute, htHour, htDay, htYDay, htMonth, htYear;
                            HOperatorSet.GetImageTime(_lstNgHistoryData[index].ImageOrgs, out htMSecond, out htSecond, out htMinute, out htHour, out htDay, out htYDay, out htMonth, out htYear);
                            strTime = htHour.I.ToString("D02") + ":" + htMinute.I.ToString("D02") + ":" + htSecond.I.ToString("D02") + ":" + htMSecond.I.ToString("D03");
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return strTime;
        }
        /// <summary>
        /// NG履歴　初期処理
        /// </summary>
        private void StartNgHistoryDatas()
        {
            if (_lstNgHistoryData != null)
            {
                ClearNgHistoryDatas();
            }
            else
            {
                _lstNgHistoryData = new List<NgHistoryData>();
            }
        }
        /// <summary>
        /// NG履歴　追加
        /// </summary>
        /// <param name="camNo"></param>
        /// <param name="imgOrgs"></param>
        /// <param name="imgTargets"></param>
        /// <param name="imgInspScales"></param>
        private void AddNgHistoryData(int camNo, HObject imgOrgs, HObject imgTargets, HObject imgInspScales)
        {
            NgHistoryData res = new NgHistoryData(camNo, imgOrgs, imgTargets, imgInspScales);
            lock (_lstNgHistoryData)
            {
                _lstNgHistoryData.Add(res);
                if (_lstNgHistoryData.Count > SystemParam.GetInstance().NGHistoryCount)
                {
                    int delIndex = _lstNgHistoryData.Count - 1;
                    _lstNgHistoryData[delIndex].Dispose();
                    _lstNgHistoryData.RemoveAt(delIndex);
                }
            }
        }
        /// <summary>
        /// NG履歴　クリア
        /// </summary>
        private void ClearNgHistoryDatas()
        {
            if (_lstNgHistoryData != null)
            {
                lock (_lstNgHistoryData)
                {
                    for (int i = 0; i < _lstNgHistoryData.Count; i++)
                        _lstNgHistoryData[i].Dispose();
                    _lstNgHistoryData.Clear();
                }
            }
        }
        #endregion


        #region 同期スレッドからアクション(Start,Stop,Suspend)受信
        /// <summary>
        /// 同期スレッドから開始・停止・中断(Start,Stop,Suspend)　操作アクションイベントを受信する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEventAction(object sender, AutoInspection.ActionDataEventArgs e)
        {
            //結果確定時刻
            DateTime nowTime = DateTime.Now;
            //
            Recipe recipe = e.Recipe;

            //
            ResultActionDataClass.EEventId eventId;
            ResultActionDataClass.EEventMode eventMode;

            eventId = ResultActionDataClass.EEventId.Action;
            switch (e.Id)
            {
                case EResultActionId.Stop:
					//停止
                    eventMode = ResultActionDataClass.EEventMode.Stop;
                    ClearNgHistoryDatas();
                    break;
				case EResultActionId.Reset:
					//リセット
					eventMode = ResultActionDataClass.EEventMode.Reset;
					break;
				case EResultActionId.Suspend:
                    //中断
                    eventMode = ResultActionDataClass.EEventMode.Suspend;
                    break;
                default:
                    //開始
                    eventMode = ResultActionDataClass.EEventMode.Start;
                    StartNgHistoryDatas();
                    break;
            }
            //
            lock (_queueAction)
            {
                QueuingAction.QueueData qData = new QueuingAction.QueueData(e.Id, eventId, eventMode, e.StartLength, e.EndLength, nowTime, recipe, 0.0);
                _queueAction.Enqueue(qData);
            }
        }
        #endregion

		public double InspTime { get; set; }
		public Stopwatch _____sw { get; private set; }

        #region 検査スレッドからアクション(Result)受信
        /// <summary>
        /// 検査スレッドのアクションをキューに登録する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEventResult(object sender, AutoInspection.ActionDataEventArgs e)
        {
            //カメラ番号
            int camNo = (int)Enum.Parse(typeof(AppData.CamID), e.CamInfo.CamNo.ToString());

            //カメラからの結果確定を受信した
            this._syncResultEvent.SetEnabled(camNo);

            //全カメラの結果が確定したか？
            if (this._syncResultEvent.IsAllEnabled() == false)
            {
                return;
            }

			//検査時間
            _____sw.Stop();
            //System.Diagnostics.Debug.WriteLine(string.Format("OnEventResult() camNo:{0}     time:{1}", camNo, _____sw.ElapsedMilliseconds.ToString("F2")));
			this.InspTime = _____sw.ElapsedMilliseconds;
            //Debug.WriteLine(string.Format("Insp Time = {0}", _____sw.ElapsedMilliseconds.ToString("F2")));

            //結果確定時刻
            DateTime nowTime = DateTime.Now;

            //if (camNo == 0)
            lock(_queueAction)
            {
                //全カメラの結果が揃ったので、キューに登録する
                QueuingAction.QueueData qData = new QueuingAction.QueueData(
                    e.Id,
                    ResultActionDataClass.EEventId.Result,
                    ResultActionDataClass.EEventMode.NG,
                    e.StartLength, e.EndLength, nowTime, null, 0.0, e.CamNo, e.ImageOrgs, e.ImageTargets, e.ImageInspScales);
                _queueAction.Enqueue(qData);
            }

            //全カメラの結果が揃ったのでクリアする
            this._syncResultEvent.Reset();
        }
        #endregion

        #region 検査スレッドから結果データ(datas)受信
        /// <summary>
        /// 検査スレッドから結果データを受信する
        /// 　結果データを内部リストにバッファしておく
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEventResultEntry(object sender, AutoInspection.EntryResultDataEventArgs e)
        {
            lock (_imgResDatas)
            {
                foreach (ImageResultData res in e.ResultDatas)
                {
                    double mx = res.PositionX - this.OverlapRange;
                    double px = res.PositionX + this.OverlapRange;
                    double my = res.PositionY - this.OverlapRange;
                    double py = res.PositionY + this.OverlapRange;

					//LogingDll.Loging_SetLogString(string.Format("EventMonitor.OnEventResultEntry() : _resultActionDataClass.Count={0} _imgResDatas.Count={1}", this._resultActionDataClass.CountNG, this._imgResDatas.Count));

                    //重複データが存在していないか？
                    //１つ前の画像のヘッダ部分の重複チェック
                    List<ResActionData> resDatas = this._resultActionDataClass.IsOverlapData((m) => (res.SideId == m.SideId && mx <= m.PositionX && m.PositionX <= px && my <= m.PositionY && m.PositionY <= py));
                    //隣り通しにカメラの重複チェック
					bool imgEntry = false;
                    int index = this._imgResDatas.FindIndex((m) => (res.SideId == m.SideId && mx <= m.PositionX && m.PositionX <= px && my <= m.PositionY && m.PositionY <= py));
					if (index >= 0)
					{
						bool chg = false;
						//既存データより、新規データのほうが　面積が大きい
						if (res.Area > this._imgResDatas[index].Area)
							chg = true;
						else if (res.Area == this._imgResDatas[index].Area)
						{
							////既存データCamNoが、大きかったら　新規データに置き換える
							//if (res.CamId < this._imgResDatas[index].CamId)
								chg = true;
						}
						if (chg == true)
						{
							//既存を消す
                            lock(_imgResDatas)
    							this._imgResDatas.RemoveAt(index);
							//エントリーの対象
							imgEntry = true;
						}
					}
					else
					{
						//既存にないので、エントリー対象
						imgEntry = true;
					}

                    //重複がなければ、登録する
					if (resDatas.Count == 0 && imgEntry == true)
                    {
                        ImageResultData data = res.Copy();
                        lock (_imgResDatas)
                        {
                            if (_imgResDatas.Count < SystemParam.GetInstance().InspFunc_CountNgMax)
                                this._imgResDatas.Add(data);
                        }
                    }
                }
            }
        }
        #endregion



        #region 全カメラの結果が全て完了したかを管理する
        /// <summary>
        /// 全カメラの結果が全て完了したかを管理する
        /// </summary>
        class SyncResultEvent
        {
            /// <summary>
            /// 結果完了フラグ
            /// </summary>
            private bool[] _enabled;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="camInfos"></param>
            public SyncResultEvent(List<CameraParam> camp)
            {
                int iCamNum = HalconCamera.APCameraManager.getInstance().CameraNum;
                int iEnableCnt = 0;
                foreach(CameraParam c in camp)
                {
                    if (c.OnOff == true)
                        iEnableCnt++;
                }
                this._enabled = new bool[iEnableCnt];
            }

            /// <summary>
            /// 完了を確定する
            /// </summary>
            /// <param name="camNo"></param>
            /// <param name="result"></param>
            public void SetEnabled(int camNo)
            {
                lock (_enabled)
                {
                    this._enabled[camNo] = true;
                }
            }

            /// <summary>
            /// 全カメラの結果が揃ったか？
            /// </summary>
            /// <returns>true:揃った false:揃ってない</returns>
            public bool IsAllEnabled()
            {
                bool flg = false;
                lock (_enabled)
                {
                    foreach (bool f in this._enabled)
                    {
                        flg |= f;
                    }
                }
                return flg;
            }

            /// <summary>
            /// リセット
            /// </summary>
            public void Reset()
            {
                for (int i = 0; i < this._enabled.Length; i++)
                {
                    this._enabled[i] = false;
                }
            }
        }
        #endregion
    }
}
