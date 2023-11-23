using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Diagnostics;

using LineCameraSheetSystem;

using HalconDotNet;

namespace ResultActionDataClassNameSpace
{
    public class ResultActionDataClass : IDisposable
    {
        /// <summary>
        /// Imageフォルダ配下の有限フォルダ数
        /// </summary>
        public int ImageNumDirMax { get; set; }
        /// <summary>
        /// Image\Numberフォルダ内の最大イメージ数
        /// </summary>
        public int ImageNumDirFileMax { get; set; }

		private int _imageStartNumDirectiryNo;
		private int _imageNowNumDirectoryNo;
		private int _imageNowNumFileNo;

        /// <summary>
        /// イベント識別
        /// </summary>
        public enum EEventId
        {
            Action,
            Result,
        }
        /// <summary>
        /// イベントモード
        /// </summary>
        public enum EEventMode
        {
            Start,
            Suspend,
            Stop,
            OK,
            NG,
            AlarmSW,
            NGSW,
            NGST,
            AlarmSM,
            NGSM,
			Reset,
        }

        /// <summary>
        /// リアルタイム中に保存するWorkフォルダ
        /// </summary>
        private string _workDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "WorkResult");
        /// <summary>
        /// システムで結果テキストファイルを保存するフォルダ
        /// </summary>
        public string SystemResultDir { get; set; }
        /// <summary>
        /// システムでイメージを保存するフォルダ
        /// </summary>
        public string SystemImageDir { get; set; }
        /// <summary>
        /// 保存名称
        /// </summary>
        private string _resultName;
        /// <summary>
        /// 結果ファイルパス
        /// </summary>
        public string ResultFilePath
        {
            get;
            private set;
        }
        /// <summary>
        /// 結果パス
        /// </summary>
        public string ResultDir { get; private set; }

        const string RESULTFILE = "Result.txt";

        #region プロパティ
        /// <summary>
        /// 品種名
        /// </summary>
        public string HinsyuName { get; private set; }
        /// <summary>
        /// LotNo
        /// </summary>
        public string LotNo { get; private set; }
        /// <summary>
        /// 開始時刻
        /// </summary>
        public DateTime StTime { get; private set; }
        /// <summary>
        /// 終了時刻
        /// </summary>
        public DateTime EndTime { get; private set; }
        /// <summary>
        /// 終了時の距離
        /// </summary>
        public double EndLength { get; private set; }
        /// <summary>
        /// NG数
        /// </summary>
        public int CountNG { get; private set; }
        /// <summary>
        /// ゾーン別NG数
        /// </summary>
        public int[,] CountNGZone { get; private set; }
        /// <summary>
        /// 項目別NG数
        /// </summary>
        public int[,] CountNGItems { get; private set; }
        /// <summary>
        /// カメラ別NG数
        /// </summary>
        public int[] CountNGCamera { get; private set; }
		/// <summary>
		/// 現在のライン番号
		/// </summary>
        public int LineCount { get; private set; }
		/// <summary>
		/// 現在の結果番号
		/// </summary>
		public int ResultCount { get; private set; }
        #endregion

        /// <summary>
        /// バッファデータ
        /// </summary>
        private List<ResActionData> ResActionDatas = new List<ResActionData>();

        //private List<ResActionData> _fileDataResActionDatas;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResultActionDataClass()
        {
            //Workフォルダが存在しているか？
            if (Directory.Exists(this._workDir) == false)
            {
                //存在していなければ、作成する
                Directory.CreateDirectory(this._workDir);
            }

            CountNGZone = new int[Enum.GetValues(typeof(AppData.SideID)).Length, Enum.GetValues(typeof(AppData.ZoneID)).Length];
            CountNGItems = new int[Enum.GetValues(typeof(AppData.SideID)).Length, Enum.GetValues(typeof(AppData.InspID)).Length];
            CountNGCamera = new int[Enum.GetValues(typeof(AppData.CamID)).Length];
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }


        #region リアルタイム保存の開始・停止

        private object _fileAccess = new object();
        private FileStream _fs = null;
        private StreamWriter _sw = null;
        private StreamReader _sr = null;

        /// <summary>
        /// リアルタイムでの結果保存を開始する
        /// </summary>
        /// <param name="hinsyu"></param>
        /// <param name="lotNo"></param>
        /// <param name="startTime"></param>
        public void StartRealtime(string hinsyu, string lotNo, DateTime startTime)
        {
			//結果フォルダ
            if (Directory.Exists(this.SystemResultDir) == false)
            {
                Directory.CreateDirectory(this.SystemResultDir);
            }
			//イメージフォルダ
            if (Directory.Exists(this.SystemImageDir) == false)
            {
                Directory.CreateDirectory(this.SystemImageDir);
            }

            this.Clear();

            this.HinsyuName = hinsyu;
            this.LotNo = lotNo;
            this.StTime = startTime;
            this.EndTime = startTime;
            this.EndLength = 0.0;

			this.InitImageDirInfo();

            //結果ファイル：開始時刻 _ Hinsyu _ LotNo
            this._resultName = string.Format("{0}_{1}_{2}", startTime.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo), hinsyu, lotNo);

            //WORKフォルダによる結果ファイル名
            this.ResultDir = Path.Combine(this._workDir, this._resultName);
            if (Directory.Exists(this.ResultDir) == false)
            {
                Directory.CreateDirectory(this.ResultDir);
            }
            this.ResultFilePath = Path.Combine(this.ResultDir, RESULTFILE);
            lock (_fileAccess)
            {
                _fs = new FileStream(this.ResultFilePath, FileMode.Create, FileAccess.ReadWrite);
                _sr = new StreamReader(_fs, Encoding.GetEncoding("Shift-JIS"));
                _sw = new StreamWriter(_fs, Encoding.GetEncoding("Shift-JIS"));
                _sw.AutoFlush = true;
            }
            //ヘッダ情報を保存する
            this.SaveFileHeader();

            this.LineCount = 0;
			this.ResultCount = 0;
        }

		public string NextImageDirInfo()
		{
			string numdir;
			string numPath;
			string numFile;

			//ファイル番号をカウントアップ
			this._imageNowNumFileNo += 1;

			//ファイル数が最大？
			if (this._imageNowNumFileNo > this.ImageNumDirFileMax)
			{
				//ファイル番号をリセットする
				this._imageNowNumFileNo = 1;
				//ディレクトリ番号をカウントアップする
				this._imageNowNumDirectoryNo += 1;

				//ディレクトリ数が最大？
				if ((this._imageNowNumDirectoryNo - this._imageStartNumDirectiryNo) >= this.ImageNumDirMax)
				{
					//古いディレクトリを削除
					numdir = string.Format("{0}", this._imageStartNumDirectiryNo.ToString("D010"));
					numPath = Path.Combine(this.SystemImageDir, numdir);
					//削除
					if (Directory.Exists(numPath) == true)
						Directory.Delete(numPath, true);

					//古いディレクトリ番号をカウントアップする
					this._imageStartNumDirectiryNo += 1;
				}
			}
			numdir = string.Format("{0}", this._imageNowNumDirectoryNo.ToString("D010"));
			numFile = this._imageNowNumFileNo.ToString("D04") + ".bmp";

			numPath = Path.Combine(this.SystemImageDir, numdir);
			//最新のディレクトリを作成する
			if (Directory.Exists(numPath) == false)
				Directory.CreateDirectory(numPath);

			return Path.Combine(numdir, numFile);
		}

		private void InitImageDirInfo()
		{
			//Imageフォルダ内の全ディレクトリ名を取得する
			DirectoryInfo di = new DirectoryInfo(this.SystemImageDir);
			DirectoryInfo[] dirs = di.GetDirectories();

			if (dirs.Length == 0)
			{
				this._imageStartNumDirectiryNo = 0;
				this._imageNowNumDirectoryNo = 0;
				this._imageNowNumFileNo = 0;
			}
			else
			{
				//Imageフォルダ内のディレクトリ名を降順にソートする
				//System.Comparison<DirectoryInfo> sortCond = new System.Comparison<DirectoryInfo>(delegate(DirectoryInfo d1, DirectoryInfo d2) { return d2.Name.CompareTo(d1.Name); });
				Array.Sort(dirs, delegate(DirectoryInfo d1, DirectoryInfo d2) { return d2.Name.CompareTo(d1.Name); });

				this._imageStartNumDirectiryNo = int.Parse(dirs[dirs.Length - 1].Name);
				this._imageNowNumDirectoryNo = int.Parse(dirs[0].Name);
				this._imageNowNumFileNo = Directory.GetFiles(Path.Combine(this.SystemImageDir, dirs[0].Name)).Length;
			}
		}

        /// <summary>
        /// リアルタイムでの結果保存を終了する
        /// </summary>
        /// <param name="endTime"></param>
        /// <param name="endLength"></param>
        public void EndRealtime(Recipe recipe)
        {
            lock (_fileAccess)
            {
                if (_sw != null)
                {
                    _sw.Close();
                    _sw = null;
                }
                if (_sr != null)
                {
                    _sr.Close();
                    _sr = null;
                }
                if (_fs != null)
                {
                    _fs.Close();
                    _fs = null;
                }
            }

            clsRirekiCount.getInstance().Save(Path.GetDirectoryName(recipe.Path), this);
            
            recipe.Path = Path.Combine(this.ResultDir, AppData.RCP_FILE);
            recipe.Save(false);

            LogingDllWrap.LogingDll.Loging_SetLogString("(ActionCheck):結果ファイルの保存");


            //イメージフォルダ内のフォルダ数をチェックして500より大きかったら　古いを削除する
            //Imageフォルダ内の全ディレクトリ名を取得する
            DirectoryInfo di = new DirectoryInfo(this.SystemImageDir);
            DirectoryInfo[] dirs = di.GetDirectories();
            int cnt = dirs.Length;
            if (cnt > this.ImageNumDirMax)
            {
                //昇順
                Array.Sort(dirs, delegate(DirectoryInfo d1, DirectoryInfo d2)
                {
                    return d1.Name.CompareTo(d2.Name);
                });
                int delcnt = cnt - this.ImageNumDirMax;
                for (int i = 0; i < delcnt; i++)
                {
                    Directory.Delete(dirs[i].FullName, true);
                }
            }

            string nowDir = this.ResultDir;
            // 開始時刻 から　YYYYMM フォルダを作成する
            string dir = Path.Combine(this.SystemResultDir, this._resultName.Substring(0, 6));
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            // 開始時刻 _ Hinsyu _ LotNo _ 終了時刻 _ 終了距離
            this._resultName += string.Format("_{0}_{1}_{2}", this.EndTime.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo), this.EndLength.ToString("F2"), this.CountNG.ToString());
            string newDir = Path.Combine(dir, this._resultName);
            try
            {
                CopyDirectory(nowDir, newDir, true);
                Directory.Delete(nowDir, true);
				//Directory.Move(nowDir, newDir);
			}
            catch (Exception exc)
            {
				string ErrStr = string.Format("ResultActionDataClass.EndRealtime() exc = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				throw exc;
			}
            this.ResultDir = newDir;
            this.ResultFilePath = Path.Combine(this.ResultDir, RESULTFILE);

            this.Clear();
        }

        #region ディレクトリコピー
        public static void CopyDirectory(string stSourcePath, string stDestPath, bool bOverwrite)
        {
            // コピー先のディレクトリがなければ作成する
            if (!System.IO.Directory.Exists(stDestPath))
            {
                System.IO.Directory.CreateDirectory(stDestPath);
                System.IO.File.SetAttributes(stDestPath, System.IO.File.GetAttributes(stSourcePath));
                bOverwrite = true;
            }

            // コピー元のディレクトリにあるすべてのファイルをコピーする
            if (bOverwrite)
            {
                foreach (string stCopyFrom in System.IO.Directory.GetFiles(stSourcePath))
                {
                    string stCopyTo = System.IO.Path.Combine(stDestPath, System.IO.Path.GetFileName(stCopyFrom));
                    System.IO.File.Copy(stCopyFrom, stCopyTo, true);
                }

                // 上書き不可能な場合は存在しない時のみコピーする
            }
            else
            {
                foreach (string stCopyFrom in System.IO.Directory.GetFiles(stSourcePath))
                {
                    string stCopyTo = System.IO.Path.Combine(stDestPath, System.IO.Path.GetFileName(stCopyFrom));

                    if (!System.IO.File.Exists(stCopyTo))
                    {
                        System.IO.File.Copy(stCopyFrom, stCopyTo, false);
                    }
                }
            }

            // コピー元のディレクトリをすべてコピーする (再帰)
            foreach (string stCopyFrom in System.IO.Directory.GetDirectories(stSourcePath))
            {
                string stCopyTo = System.IO.Path.Combine(stDestPath, System.IO.Path.GetFileName(stCopyFrom));
                CopyDirectory(stCopyFrom, stCopyTo, bOverwrite);
            }
        }
        #endregion

        #endregion


        /// <summary>
        /// クリアする
        /// </summary>
        public void Clear()
        {
			this.LineCount = 0;
			this.ResultCount = 0;
            this.HinsyuName = null;
            this.LotNo = null;
            this.StTime = new DateTime();
            this.EndTime = new DateTime();
            this.EndLength = 0.0;
            this.CountNG = 0;
            ////カメラ部位別
            //foreach(AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            //{
            //    //ゾーン別
            //    foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
            //    {
            //        this.CountNGZone[(int)side, (int)zone] = 0;
            //    }
            //    //項目別
            //    foreach(AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
            //    {
            //        this.CountNGItems[(int)side, (int)inspId] = 0;
            //    }
            //}
            ////カメラ別
            //foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
            //{
            //    this.CountNGCamera[(int)cam] = 0;
            //}
            //データ
            foreach (ResActionData res in ResActionDatas)
            {
                res.Dispose();
            }
            ResActionDatas.Clear();
        }

        public void ClearRirekiCount()
        {
            //カメラ部位別
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                //ゾーン別
                foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
                {
                    this.CountNGZone[(int)side, (int)zone] = 0;
                }
                //項目別
                foreach (AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
                {
                    this.CountNGItems[(int)side, (int)inspId] = 0;
                }
            }
            //カメラ別
            foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
            {
                this.CountNGCamera[(int)cam] = 0;
            }
        }

        /// <summary>
        /// 二重登録チェック
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<ResActionData> IsOverlapData(Predicate<ResActionData> match)
        {
			List<ResActionData> datas;
			lock (this.ResActionDatas)
            {
                //Debug.WriteLine(string.Format("IsOverlapData() cnt={0}", this.ResActionDatas.Count));
                datas = this.ResActionDatas.FindAll(match);
            }
            return datas;
        }

		#region 項目データを設定する
		/// <summary>
		/// 項目データを設定する
		/// </summary>
		/// <param name="res"></param>
		public void SetItemData(ResActionData res)
		{
			this.EndTime = res.Time;
			this.EndLength = res.EndPosition;

			//セットする項目データが結果の場合
			if (res.EventId == EEventId.Result)
			{
				//NG数をカウントアップする
				this.ResultCount += 1;

				//NG合計
				this.CountNG += 1;

				int sideNo = (int)res.SideId;
				//
				int zoneNo = (int)res.ZoneId;
				this.CountNGZone[sideNo, zoneNo] += 1;

				//項目別
				int itemNo = (int)res.InspId;
				this.CountNGItems[sideNo, itemNo] += 1;
				//カメラ別
				int camNo = (int)res.CamId;
				this.CountNGCamera[camNo] += 1;
			}

			lock (_fileAccess)
			{
				//結果を保存する
				this.SaveItemData(res);

				this.SaveFileEndTime(this._posEndTime);
				this.SaveFileEndLength(this._posEndLength);
				if (res.EventId == EEventId.Result)
				{
					this.SaveFileCountNG(this._posCountNG);
					this.SaveFileZoneCountNG(this._posZoneCountNG);
					this.SaveFileItemCountNG(this._posItemCountNG);
					this.SaveFileCameraCountNG(this._posCameraCountNG);
				}
			}

			lock (this.ResActionDatas)
			{
				this.ResActionDatas.Add(res);
			}

			this.LineCount++;
		}
		#endregion


        #region 項目データを取得する
        //private List<ResActionData> _bufferResActionDatas = new List<ResActionData>();
        /// <summary>
        /// 条件で項目データを取得する
        /// </summary>
        /// <param name="getStart">取得開始位置(mm)</param>
        /// <param name="getEnd">取得終了位置(mm)</param>
        /// <param name="getEventId">取得イベントID null=全て</param>
        /// <param name="getEventMode">取得モードID null=全て</param>
        /// <param name="getZoneId">取得ゾーン[true or false] null=全て</param>
        /// <param name="getSideId">取得サイド[true or false] null=全て</param>
        /// <param name="getInspId">取得検査項目[true or false] null=全て</param>
        /// <returns>項目データリスト</returns>
        public List<ResActionData> GetItemDatas(double getStart, double getEnd, EEventId? getEventId, EEventMode? getEventMode, bool[] getZoneId, bool[] getSideId, bool[] getInspId)
        {
            List<ResActionData> datas = new List<ResActionData>();
            lock (this.ResActionDatas)
            {
                //取得開始位置が0の場合、0を取得するために負数にする
                double startPos = (getStart == 0.0) ? -1.0 : getStart;

                foreach (ResActionData dt in this.ResActionDatas)
                {
                    //v1332 2021/11/29 yuasa 画像一覧に反映されないバグ修正
                    SystemParam sysparam = SystemParam.GetInstance();
                    double dUnderBufferArea = sysparam.InspArea_ConnectMode_BufferArea * sysparam.camParam[(int)dt.CamId].ResoV;

                    bool get = true;
                    if (dt.PositionY <= startPos)
                        get = false;
                    if (getEnd + dUnderBufferArea < dt.PositionY)//v1332 2021/11/29 yuasa 画像一覧に反映されないバグ修正
                        get = false;
                    if (getEventId != null && getEventId != dt.EventId)
                        get = false;
                    if (getEventMode != null && getEventMode != dt.EventMode)
                        get = false;
                    if (dt.EventId == ResultActionDataClass.EEventId.Result)
                    {
                        if (getZoneId != null && getZoneId[(int)dt.ZoneId] == false)
                            get = false;
                        if (getSideId != null && getSideId[(int)dt.SideId] == false)
                            get = false;
                        if (getInspId != null && getInspId[(int)dt.InspId] == false)
                            get = false;
                    }
                    if (get == true)
                    {
                        ResActionData resData = dt.Copy();
                        datas.Add(resData);
                    }
                }
            }
            return datas;
        }
        /// <summary>
        /// Line番号で項目データを取得する
        /// </summary>
        /// <param name="lineNo">Line番号(1～)</param>
        /// <returns></returns>
        public List<ResActionData> GetItemDataIndex(int lineNo)
        {
            //this._fileDataResActionDatas = ReadItemDatas(EFilterMode.Index, LineNo, 0.0, 0.0, null, null, null, null, null);
            List<ResActionData> datas;
			lock (this.ResActionDatas)
            {
#if false
				datas = ResActionDatas.FindAll(x => x.LineNo == lineNo);
#else
				datas = new List<ResActionData>();
				foreach (ResActionData dt in this.ResActionDatas)
                {
                    if (dt.LineNo == lineNo)
                    {
                        ResActionData resData = dt.Copy();
                        datas.Add(resData);
                        break;
                    }
                }
#endif
			}
            return datas;
        }
		/// <summary>
		/// Result番号で項目データを取得する
		/// </summary>
		/// <param name="resultNo"></param>
		/// <returns></returns>
		public List<ResActionData> GetItemDataResultIndex(int resultNo)
		{
			List<ResActionData> datas;
			lock (this.ResActionDatas)
			{
#if false
				datas = ResActionDatas.FindAll(x => x.ResultNo == resultNo);
#else
				datas = new List<ResActionData>();
				foreach (ResActionData dt in this.ResActionDatas)
				{
					if (dt.ResultNo == resultNo)
					{
						ResActionData resData = dt.Copy();
						datas.Add(resData);
						break;
					}
				}
#endif
			}
			return datas;
		}
        public ResActionData GetItemLastResult()
        {
            ResActionData data = null;
            lock(this.ResActionDatas)
            {
                for (int i = this.ResActionDatas.Count - 1; i >= 0; i--)
                {
                    if (this.ResActionDatas[i].EventId == EEventId.Result)
                    {
                        data = this.ResActionDatas[this.ResActionDatas.Count - 1].Copy();
                        break;
                    }
                }
            }
            return data;
        }

        public ResActionData LastNgData = null; //V1057 NG表裏修正 yuasa 20190118：前回データを保持するため追加
        public void LastNgDataInit() //V1057 NG表裏修正 yuasa 20190121：外部から初期化するように追加
        {
            LastNgData = null;
        }
        //V1333 lstZonesを追加。同時刻に発生したゾーン毎のリストとしてoutで値を返すように修正
        public void GetItemSideResult(out ResActionData data , out int frontReverseSide,out List<AppData.ZoneID> lstZones, out bool isBothSide, out bool isCenter) //V1057 NG表裏修正 yuasa 20190118：関数追加
        {
            data = null;
            isBothSide = false;
            isCenter = false;
            frontReverseSide = -1;
            lstZones = new List<AppData.ZoneID>();//V1333 追加

            lock (this.ResActionDatas) //V1333 最終データと最終データと同タイムスタンプのデータを集めてlstZonesにまとめるように修正
            {
                bool lastDataPickupAlready = false;
                for (int i = this.ResActionDatas.Count - 1; i >= 0; i--)
                {
                    if (this.ResActionDatas[i].EventId == EEventId.Result)
                    {
                        //最後の結果データを見つける
                        if (lastDataPickupAlready == false)
                        {
                            int frontSide = -1;
                            int reverseSide = -1;
                            if (LastNgData == null) //初回はnullなので時間比較しない
                            {
                                frontSide = this.ResActionDatas.FindIndex(n => (n.SideId == AppData.SideID.表) && (n.ResultNo >= 0)); //ResultNo「-1」がマップのヘッダとして存在しているので無視する
                                reverseSide = this.ResActionDatas.FindIndex(n => (n.SideId == AppData.SideID.裏) && (n.ResultNo >= 0));
                            }
                            else
                            {
                                frontSide = this.ResActionDatas.FindIndex(n => (n.SideId == AppData.SideID.表) && (n.ResultNo > LastNgData.ResultNo) && (n.ResultNo >= 0));
                                reverseSide = this.ResActionDatas.FindIndex(n => (n.SideId == AppData.SideID.裏) && (n.ResultNo > LastNgData.ResultNo) && (n.ResultNo >= 0));
                            }

                            if (frontSide >= 0 && reverseSide >= 0)
                            {
                                frontReverseSide = 2; //表裏
                            }
                            else if (frontSide >= 0)
                            {
                                frontReverseSide = 0; //表
                            }
                            else if (reverseSide >= 0)
                            {
                                frontReverseSide = 1; //裏
                            }
                            data = this.ResActionDatas[this.ResActionDatas.Count - 1].Copy();
                            LastNgData = data;
                            lstZones.Add(data.ZoneId);//V1333 ゾーンも合わせて追加

                            //端か中央かのチェック
                            bool bCheckRes = isBothSideCheck(data.PositionX, data.SideId, Recipe.GetInstance());
                            isBothSide = isBothSide | bCheckRes;
                            isCenter = isCenter | !bCheckRes;

                            lastDataPickupAlready = true;
                        }
                        else //V1333 最後から２番目以降は時間が同じ、ゾーンIDが異なるものをリストに追加していく
                        {
                            //時間が同じかどうか
                            if (LastNgData.Time == this.ResActionDatas[i].Time)
                            {
                                //ゾーンが未登録なら追加する。
                                if (lstZones.Contains(this.ResActionDatas[i].ZoneId) == false)
                                {
                                    lstZones.Add(this.ResActionDatas[i].ZoneId);
                                }
                                //端か中央かのチェック
                                bool bCheckRes = isBothSideCheck(ResActionDatas[i].PositionX, ResActionDatas[i].SideId, Recipe.GetInstance());
                                isBothSide = isBothSide | bCheckRes;
                                isCenter = isCenter | !bCheckRes;
                            }
                            else//違う時間になったらbreakで抜ける
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public List<ResActionData> GetNgDatas()
        {
            return this.ResActionDatas.FindAll(x => x.EventId == EEventId.Result);
        }
        #endregion


        #region 結果データをセーブする

        long _posEndTime;
        long _posEndLength;
        long _posCountNG;
        long _posZoneCountNG;
        long _posItemCountNG;
        long _posCameraCountNG;
        /// <summary>
        /// ヘッダ部
        /// </summary>
        private void SaveFileHeader()
        {
            List<string> lines = new List<string>();
            List<string> cols = new List<string>();

            lock (_fileAccess)
            {
                //品種名
                cols.Clear();
                cols.Add("010");
                cols.Add("品種名");
                cols.Add(this.HinsyuName);
                lines.Add(string.Join(SEPARATOR, cols.ToArray()));
                //LotNo
                cols.Clear();
                cols.Add("020");
                cols.Add("ロット");
                cols.Add(this.LotNo);
                lines.Add(string.Join(SEPARATOR, cols.ToArray()));
                //開始時刻
                cols.Clear();
                cols.Add("030");
                cols.Add("開始時刻");
                cols.Add(this.StTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
                lines.Add(string.Join(SEPARATOR, cols.ToArray()));
                //
                foreach (string s in lines)
                {
                    _sw.WriteLine(s);
                }

                //終了時刻
                this._posEndTime = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileEndTime(this._posEndTime);
                //終了距離
                this._posEndLength = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileEndLength(this._posEndLength);
                //NG数
                this._posCountNG = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileCountNG(this._posCountNG);
                //ゾーン別NG数
                this._posZoneCountNG = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileZoneCountNG(this._posZoneCountNG);
                //項目別NG数
                this._posItemCountNG = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileItemCountNG(this._posItemCountNG);
                //カメラ別NG数
                this._posCameraCountNG = _fs.Seek(0, SeekOrigin.Current);
                this.SaveFileCameraCountNG(this._posCameraCountNG);
                //項目ヘッダ
                this.SaveFileItemHeader();
            }
        }
        /// <summary>
        /// 終了時刻を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileEndTime(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            cols.Add("040");
            cols.Add("終了時刻");
            cols.Add(this.EndTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));       //茂木　修正
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        /// <summary>
        /// 終了距離を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileEndLength(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            cols.Add("050");
            cols.Add("終了距離");
            cols.Add(string.Format("{0,30}", this.EndLength.ToString("F6")));
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        /// <summary>
        /// NG数を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileCountNG(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            cols.Add("060");
            cols.Add("NG数");
            cols.Add(this.CountNG.ToString("D010"));
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        /// <summary>
        /// ゾーン別NG数を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileZoneCountNG(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            int head = 70;
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                cols.Clear();
                head += (int)side;
                cols.Add(string.Format("{0}", head.ToString("D03")));
                cols.Add(string.Format("ゾーン別NG数-{0}", side.ToString()));
                foreach (AppData.ZoneID zoneId in Enum.GetValues(typeof(AppData.ZoneID)))
                {
                    cols.Add(zoneId.ToString());
                    cols.Add(this.CountNGZone[(int)side, (int)zoneId].ToString("D010"));
                }
                string str = string.Join(SEPARATOR, cols.ToArray());
                _sw.WriteLine(str);
            }
        }
        /// <summary>
        /// 項目別NG数を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileItemCountNG(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            int head = 80;
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                cols.Clear();
                head += (int)side;
                cols.Add(string.Format("{0}", head.ToString("D03")));
                cols.Add(string.Format("項目別NG数-{0}", side.ToString()));
                foreach (AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
                {
                    cols.Add(inspId.ToString());
                    cols.Add(this.CountNGItems[(int)side, (int)inspId].ToString("D010"));
                }
                string str = string.Join(SEPARATOR, cols.ToArray());
                _sw.WriteLine(str);
            }
        }
        /// <summary>
        /// カメラ別NG数を保存する
        /// </summary>
        /// <param name="pos"></param>
        private void SaveFileCameraCountNG(long pos)
        {
            _fs.Seek(pos, SeekOrigin.Begin);
            List<string> cols = new List<string>();
            cols.Add("090");
            cols.Add("カメラ別NG数");
            foreach (AppData.CamID id in Enum.GetValues(typeof(AppData.CamID)))
            {
                cols.Add(id.ToString());
                cols.Add(this.CountNGCamera[(int)id].ToString("D010"));
            }
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        /// <summary>
        /// 項目ヘッダを保存する
        /// </summary>
        private void SaveFileItemHeader()
        {
            List<string> cols = new List<string>();
            cols.Add("100");
			cols.Add("番号");
			cols.Add("結果番号");
            cols.Add("イベント識別ID");
            cols.Add("イベントモード");
            cols.Add("開始位置");
            cols.Add("終了位置");
            cols.Add("発生時刻");
            cols.Add("カメラ部位");
            cols.Add("カメラ番号");
            cols.Add("NG項目");
            cols.Add("縦位置");
            cols.Add("横位置");
            cols.Add("縦サイズ");
            cols.Add("横サイズ");
            cols.Add("面積");
            cols.Add("ゾーン番号");
            cols.Add("NGイメージファイル名");
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        /// <summary>
        /// 項目データを保存する
        /// </summary>
        /// <param name="data"></param>
        private void SaveItemData(ResActionData data)
        {
            _fs.Seek(0, SeekOrigin.End);
            List<string> cols = new List<string>();

            cols.Clear();
            cols.Add("200");
			cols.Add(data.LineNo.ToString());
			cols.Add(data.ResultNo.ToString());
            cols.Add(data.EventId.ToString());
            cols.Add(data.EventMode.ToString());
            cols.Add(data.StartPosition.ToString("F2"));
            cols.Add(data.EndPosition.ToString("F2"));
            cols.Add(data.Time.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
            cols.Add(data.SideId.ToString());
            cols.Add(data.CamId.ToString());
            cols.Add(data.InspId.ToString().Replace("暗", ""));
            cols.Add(data.PositionY.ToString("F2"));
            cols.Add(data.PositionX.ToString("F2"));
            cols.Add(data.Height.ToString("F2"));
            cols.Add(data.Width.ToString("F2"));
            cols.Add(data.Area.ToString("F2"));
            cols.Add(data.ZoneId.ToString());
            cols.Add(data.ImageFileName);
            string str = string.Join(SEPARATOR, cols.ToArray());
            _sw.WriteLine(str);
        }
        #endregion


        #region 結果データをロードする
        /// <summary>
        /// 結果データをロードする
        /// </summary>
        /// <param name="resultDir">ロードする結果フォルダ名</param>
        /// <returns>true:OK false:NG</returns>
        public bool Load(string resultDir)
        {
            //ファイルが存在しているか？
            if (Directory.Exists(resultDir) == false)
            {
                return false;
            }

            this._resultName = Path.GetFileName(resultDir);
            this.ResultDir = resultDir;
            this.ResultFilePath = Path.Combine(this.ResultDir, RESULTFILE);

            try
            {
                lock (this.ResActionDatas)
                {
                    this.ResActionDatas.Clear();

//					using (FileStream fs = new FileStream(this.ResultFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, FileOptions.SequentialScan))
					using (FileStream fs = new FileStream(this.ResultFilePath, FileMode.Open, FileAccess.Read))
					using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("Shift-JIS")))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] strs = line.Split(SEPARATOR[0]);
                            if (strs[0] == "010")
                                this.HinsyuName = strs[2];								//品種名
                            else if (strs[0] == "020")
                                this.LotNo = strs[2];										//LotNo
                            else if (strs[0] == "030")
                                this.StTime = DateTime.Parse(strs[2]);			//開始時刻
							else if (strs[0] == "040")
                                this.EndTime = DateTime.Parse(strs[2]);		//終了時刻
							else if (strs[0] == "050")
                                this.EndLength = double.Parse(strs[2]);		//終了距離
							else if (strs[0] == "060")
                                this.CountNG = int.Parse(strs[2]);					//NG数
							else if (strs[0] == "070")
                                LoadZoneCount(AppData.SideID.表, strs);		//ゾーン別-表
							else if (strs[0] == "071")
                                LoadZoneCount(AppData.SideID.裏, strs);		//ゾーン別-裏
							else if (strs[0] == "080")
                                LoadItemCount(AppData.SideID.表, strs);		//項目別NG数-表
							else if (strs[0] == "081")
                                LoadItemCount(AppData.SideID.裏, strs);		//項目別NG数-裏
							else if (strs[0] == "090")
                            {
                                //カメラ別NG数
                                foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
                                {
                                    int pos = 3 + (int)cam * 2;
                                    this.CountNGCamera[(int)cam] = int.Parse(strs[pos]);     //茂木　修正
                                }
                            }
                            else if (strs[0] == "200")
                            {
                                //ファイルデータを取得する
                                int pos = 1;
								int lineNo = int.Parse(strs[pos++]);
								int resultNo = int.Parse(strs[pos++]);
                                EEventId eventId = (EEventId)Enum.Parse(typeof(EEventId), strs[pos++]);
                                EEventMode eventMode = (EEventMode)Enum.Parse(typeof(EEventMode), strs[pos++]);
                                double stpos = double.Parse(strs[pos++]);
                                double edpos = double.Parse(strs[pos++]);
                                DateTime time = DateTime.Parse(strs[pos++]);
                                AppData.SideID sideId = (AppData.SideID)Enum.Parse(typeof(AppData.SideID), strs[pos++]);
                                AppData.CamID camId = (AppData.CamID)Enum.Parse(typeof(AppData.CamID), strs[pos++]);

                                if (strs[pos].IndexOf("明") < 0)
                                {
                                    string s1 = strs[pos].Replace("暗", "");
                                    string s2 = "暗" + s1;
                                    strs[pos] = s2;
                                }
                                AppData.InspID inspId = (AppData.InspID)Enum.Parse(typeof(AppData.InspID), strs[pos++]);

                                double y = double.Parse(strs[pos++]);
                                double x = double.Parse(strs[pos++]);
                                double height = double.Parse(strs[pos++]);
                                double width = double.Parse(strs[pos++]);
                                double area = double.Parse(strs[pos++]);
                                AppData.ZoneID zoneId = (AppData.ZoneID)Enum.Parse(typeof(AppData.ZoneID), strs[pos++]);
                                string ngFName = strs[pos++];

                                //データを取得する
                                ResActionData data = new ResActionData(lineNo, resultNo, eventId, eventMode, stpos, edpos, time);
                                data.ImageNumDirFileMax = this.ImageNumDirFileMax;
                                HObject hoDmy;
                                data.CreateDetail(camId, sideId, inspId, x, y, width, height, area, zoneId, null, ngFName, this.SystemImageDir, out hoDmy);
                                this.ResActionDatas.Add(data);
                            }
                        }
                    }
				}
            }
            catch(Exception exc)
            {
				string ErrStr = string.Format("ResultActionDataClass.Load() exc = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				return false;
            }
            return true;
        }

        /// <summary>
        /// カメラ部位別－検査項目別
        /// </summary>
        /// <param name="side"></param>
        /// <param name="strs"></param>
        private void LoadItemCount(AppData.SideID side, string[] strs)
        {
            foreach(AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
            {
                int pos = 3 + (int)inspId * 2;
                this.CountNGItems[(int)side, (int)inspId] = int.Parse(strs[pos]);      //茂木　修正
            }
        }
        /// <summary>
        /// カメラ部位別－ゾーン別
        /// </summary>
        /// <param name="side"></param>
        /// <param name="strs"></param>
        private void LoadZoneCount(AppData.SideID side, string[] strs)
        {
            foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
            {
                int pos = 3 + (int)zone * 2;
                this.CountNGZone[(int)side, (int)zone] = int.Parse(strs[pos]);
            }
        }

        #endregion

		/// <summary>
		/// カラムの区切り文字。
		/// </summary>
		private const string SEPARATOR = "\t";

        /// <summary>両サイドかどうか判定する</summary>//V1333 追加
        private bool isBothSideCheck(double PosX, AppData.SideID SideId, Recipe Recipe)
        {
            bool bRes = false;

            int OmoteUra;
            double MaskWidth;
            double Width;

            //表裏判定
            if (SideId == AppData.SideID.表)
            {
                OmoteUra = 0;
            }
            else
            {
                OmoteUra = 1;
            }

            //基本設定値取得
            if (Recipe.CommonInspAreaEnable == true)
            {
                MaskWidth = SystemParam.GetInstance().InspArea_CmnMaskWidth[OmoteUra];
                Width = SystemParam.GetInstance().InspArea_CmnSheetWidth[OmoteUra];
            }
            else
            {
                MaskWidth = Recipe.InspParam[OmoteUra].MaskWidth;
                Width = Recipe.InspParam[OmoteUra].Width;
            }

            //MaskShift = Recipe.InspParam[OmoteUra].MaskShift;
            //MaskShift = 0;//PosXはMask考慮していないよう
            //leftZone = Recipe.InspParam[OmoteUra].Zone[0];
            //for (int i = AppData.MAX_PARTITION -1; i >= 0; i--)
            //{
            //    if(Recipe.InspParam[OmoteUra].Zone[i] != 0)
            //    {
            //        rightZone = Recipe.InspParam[OmoteUra].Zone[i];
            //        break;
            //    }
            //}

            //判定
            //if ((0 <= PosX) && (PosX <= MaskWidth))//左端？
            if (PosX <= MaskWidth)//左端？
            {
                bRes = true;
            }
            //else if ((Width - MaskWidth <= PosX) && (PosX <= Width))//右端？
            else if (Width - MaskWidth <= PosX)//右端？
            {
                bRes = true;
            }

            return bRes;
        }


    }
}
