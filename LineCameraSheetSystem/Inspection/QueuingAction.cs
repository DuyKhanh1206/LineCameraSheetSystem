using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LineCameraSheetSystem;
using ResultActionDataClassNameSpace;
using HalconDotNet;

namespace InspectionNameSpace
{
    class QueuingAction
    {
        #region キューデータ
        /// <summary>
        /// キューデータ
        /// </summary>
        public class QueueData : IDisposable
        {
            /// <summary>
            /// アクション識別
            /// </summary>
            public EResultActionId ResultActionId { get; private set; }
            /// <summary>
            /// イベント識別
            /// </summary>
            public ResultActionDataClass.EEventId EventId { get; private set; }
            /// <summary>
            /// イベントモード
            /// </summary>
            public ResultActionDataClass.EEventMode EventMode { get; private set; }
            /// <summary>
            /// 開始位置
            /// </summary>
            public double StartLength { get; private set; }
            /// <summary>
            /// 終了位置
            /// </summary>
            public double EndLength { get; private set; }
            /// <summary>
            /// 品種
            /// </summary>
            public Recipe Recipe { get; private set; }
            /// <summary>
            /// 発生時刻
            /// </summary>
            public DateTime Time { get; private set; }

            public double SheetValue { get; private set; }

            public int CamNo { get; private set; }
            public HObject ImageOrgs { get { return _imgOrgs; } }
            public HObject ImageTargets { get { return _imgTargets; } }
            public HObject ImageInspScales { get { return _imgInspScales; } }
            private HObject _imgOrgs;
            private HObject _imgTargets;
            private HObject _imgInspScales;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="resultActionId"></param>
            /// <param name="eventId"></param>
            /// <param name="eventMode"></param>
            /// <param name="st"></param>
            /// <param name="end"></param>
            /// <param name="time"></param>
            /// <param name="recipe"></param>
            public QueueData(EResultActionId resultActionId, ResultActionDataClass.EEventId eventId, ResultActionDataClass.EEventMode eventMode, double st, double end, DateTime time, Recipe recipe, double sheetValue,
                int camNo=0, HObject imgOrgs = null, HObject imgTargets = null, HObject imgInspScales = null)
            {
                this.ResultActionId = resultActionId;
                this.EventId = eventId;
                this.EventMode = eventMode;
                this.StartLength = st;
                this.EndLength = end;
                this.Time = time;
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

        /// <summary>
        /// キューリスト
        /// </summary>
        private Queue<QueueData> _queueDatas = new Queue<QueueData>();

		/// <summary>
		/// キューカウント
		/// </summary>
		public int Count
		{
			get
			{
				int cnt;
				lock (_queueDatas)
				{
					cnt = _queueDatas.Count;
				}
				return cnt;
			}
		}
        /// <summary>
        /// アクションをキューに登録する
        /// </summary>
        /// <param name="spd"></param>
        public void Enqueue(QueueData data)
        {
            lock (_queueDatas)
            {
                _queueDatas.Enqueue(data);
            }
        }
        /// <summary>
        /// アクションをキューから取得して削除する
        /// </summary>
        /// <returns></returns>
        public QueueData Dequeue()
        {
            QueueData spd = null;
            lock (_queueDatas)
            {
                if (_queueDatas.Count > 0)
                {
                    spd = _queueDatas.Dequeue();
                }
            }
            return spd;
        }
    }
}
