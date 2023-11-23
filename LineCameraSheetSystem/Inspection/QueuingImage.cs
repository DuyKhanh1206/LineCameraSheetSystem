using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using HalconDotNet;

namespace InspectionNameSpace
{
    class QueuingImage : IDisposable
    {
        #region キューデータ
        /// <summary>
        /// キューデータ
        /// </summary>
        public class QueueData : IDisposable
        {
            /// <summary>
            /// 連番
            /// </summary>
            public int Count
            {
                get;
                private set;
            }

            public HObject ImageOrg
            {
                get { return _imageOrg; }
            }
            private HObject _imageOrg;
            public HObject ImageTarget
            {
                get { return _imageTarget; }
            }
            private HObject _imageTarget;
            public HObject ImageInspScale
            {
                get { return _imageInspScale; }
            }
            private HObject _imageInspScale;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="camNo">カメラ番号</param>
            /// <param name="count">連番</param>
            /// <param name="imageTarget">イメージ</param>
            public QueueData(int count, HObject imageOrg, HObject imageTarget, HObject imageInspScale)
            {
                this.Count = count;
                HOperatorSet.CopyImage(imageOrg, out _imageOrg);
                //_imageOrg = UtilityImage.CopyHalconImage(imageOrg);
                _imageTarget = UtilityImage.CopyHalconImage(imageTarget);
                _imageInspScale = UtilityImage.CopyHalconImage(imageInspScale);
            }
            public void Dispose()
            {
                UtilityImage.ClearHalconObject(ref _imageTarget);
                UtilityImage.ClearHalconObject(ref _imageOrg);
                UtilityImage.ClearHalconObject(ref _imageInspScale);
            }
        }
        #endregion


        /// <summary>
        /// キューリスト
        /// </summary>
        Queue<QueueData> _queueDatas = new Queue<QueueData>();

        /// <summary>
        /// キューに入れたTotal数
        /// </summary>
		public int Counter { get; private set; }
        /// <summary>
        /// キューに溜まっている数
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
        /// カメラ番号
        /// </summary>
        public int CamNo
        {
            get;
            private set;
        }

        int _ibuffer = 0;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QueuingImage(int camNo, int iBuffer)
        {
            this.CamNo = camNo;
            Counter = 0;
            _ibuffer = iBuffer;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
        
        /// <summary>
        /// 登録する
        /// </summary>
        /// <param name="data"></param>
        public void Enqueue(HObject imageOrg, HObject imageTarget, HObject imageInspScale)
        {
            lock(_queueDatas)
            {
                Counter++;
                if (_queueDatas.Count < 4)
                {
                    QueueData data = new QueueData(Counter, imageOrg, imageTarget, imageInspScale);
                    _queueDatas.Enqueue(data);
                }
            }
        }
        /// <summary>
        /// 取得する
        /// </summary>
        /// <returns></returns>
        public QueueData Dequeue()
        {
            QueueData data = null;
            lock (_queueDatas)
            {
                if (_queueDatas.Count > _ibuffer)
                {
                    data = _queueDatas.Dequeue();
                }
            }
            return data;
        }

        /// <summary>
        /// 次のキューを取得する
        /// </summary>
        /// <returns></returns>
        public QueueData GetNextQueue()
        {
            QueueData data = null;
            lock (_queueDatas)
            {
                if (_queueDatas.Count > 0)
                {
                    QueueData dt = _queueDatas.Peek();
                    data = new QueueData(dt.Count, dt.ImageOrg, dt.ImageTarget, dt.ImageInspScale);
                }
            }
            return data;
        }

        /// <summary>
        /// キューが存在するか？
        /// </summary>
        /// <returns>true:する false:しない</returns>
        public bool IsExist()
        {
            bool ret;
            lock (_queueDatas)
            {
                //LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("QueuingImage.IsExist() cam = {0} count = {1}", CamNo.ToString(), _queueDatas.Count));
                ret = (_queueDatas.Count > _ibuffer) ? true : false;
            }
            return ret;
        }

        /// <summary>
        /// クリアする
        /// </summary>
        public void Clear()
        {
            while (true)
            {
				lock (_queueDatas)
				{
					if (_queueDatas.Count==0)
						break;

					QueueData data = _queueDatas.Dequeue();
					data.Dispose();
				}
            }
            Counter = 0;
        }
    }
}
