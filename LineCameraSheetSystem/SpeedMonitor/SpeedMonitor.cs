using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using HalconCamera;

namespace LineCameraSheetSystem
{
    /// <summary>
    /// 速度監視クラス
    /// </summary>
    class SpeedMonitor : IDisposable
    {

        #region イベントパラメータ
        /// <summary>
        /// 速度通知イベントパラメータ
        /// </summary>
        public class SpeedEventArgs : EventArgs
        {
            /// <summary>
            /// 開始時刻
            /// </summary>
            public DateTime StartTime { get; private set; }
            /// <summary>
            /// 終了時刻
            /// </summary>
            public DateTime EndTime { get; private set; }
            /// <summary>
            /// 速度(m/s)
            /// </summary>
            public double Speed { get; private set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="st">開始時刻</param>
            /// <param name="ed">終了時刻</param>
            /// <param name="speed">速度</param>
            public SpeedEventArgs(DateTime st, DateTime ed, double speed)
            {
                this.StartTime = st;
                this.EndTime = ed;
                this.Speed = speed;
            }
        }
        #endregion


        #region イベント定義
        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public delegate void UpdateSpeedEventHandler(object sender, SpeedEventArgs speedEventArgs);
        /// <summary>
        /// 速度計測更新通知イベント
        /// </summary>
        public event UpdateSpeedEventHandler OnEventUpdateSpeedEvent;
        #endregion


        #region プロパティ
        /// <summary>
        /// イベント発生間隔時間(ms)
        /// </summary>
        public int EventTimer { get; set; }
        #endregion


        #region 内部プロパティ
        /// <summary>
        /// 縦分解能(mm)
        /// </summary>
        private double _resolutionY;
        /// <summary>
        /// イメージ縦サイズ(pix)
        /// </summary>
        private int _imageHeight;
        /// <summary>
        /// イメージ縦サイズ(mm)
        /// </summary>
        private double _imageLength;
        #endregion


        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SpeedMonitor()
        {
            this.EventTimer = 1000;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            this.EndSpeedMonitor();
        }
        #endregion


        #region 監視の開始・終了
        /// <summary>
        /// 監視を開始する
        /// </summary>
        /// <param name="resolutionY">縦分解能(mm)</param>
        /// <param name="imageHeight">イメージ縦サイズ(pix)</param>
        public void BeginSpeedMonitor(double resolutionY, int imageHeight)
        {
            this._startTime = new DateTime();

            this._resolutionY = resolutionY;
            this._imageHeight = imageHeight;
            this._imageLength = imageHeight * resolutionY;

            //監視スレッドを開始する
            this.BeginThread();
        }
        public void SetResolution(double resolutionY)
        {
            this._resolutionY = resolutionY;
            this._imageLength = this._imageHeight * resolutionY;
        }
        /// <summary>
        /// 監視を停止する
        /// </summary>
        public void EndSpeedMonitor()
        {
            //監視スレッドを停止する
            this.EndThread();
        }
        #endregion


        #region イメージ取込イベントを受信する

        /// <summary>
        /// 前回イメージ取込時刻
        /// </summary>
        private DateTime _startTime = new DateTime();

        /// <summary>
        /// イメージ取込を受信する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnGrabedEventHandler(object sender, GrabbedImageEventArgs e)
        {
            DateTime endTime = DateTime.Now;

            if (_startTime != new DateTime())
            {
                if (_queueThread != null)
                {
                    //速度を算出する
                    TimeSpan tspan = endTime - _startTime;
                    double speed = (this._imageLength / tspan.TotalMilliseconds) * 60.0;
                    //速度データをキューへ登録する
                    SpeedEventArgs spd = new SpeedEventArgs(_startTime, endTime, speed);
                    EnqueueData(spd);
                }
            }
            //今回のイメージ取込時刻を、前回イメージ取込時刻に保持する
            _startTime = endTime;
        }

        #endregion

        
        #region 監視スレッド

        /// <summary>
        /// イベント通知指示
        /// </summary>
        private bool _enableEvent;
        /// <summary>
        /// スレッド
        /// </summary>
        private Thread _queueThread;
        /// <summary>
        /// スレッド停止指示
        /// </summary>
        private bool _stopThread = false;
        /// <summary>
        /// 速度データキュー
        /// </summary>
        private Queue<SpeedEventArgs> _queueData = new Queue<SpeedEventArgs>();

        /// <summary>
        /// 速度データをキューに登録する
        /// </summary>
        /// <param name="spd"></param>
        private void EnqueueData(SpeedEventArgs spd)
        {
            lock (_queueData)
            {
                _queueData.Enqueue(spd);
            }
        }
        /// <summary>
        /// 最新の速度データをキューから取得して削除する
        /// </summary>
        /// <returns></returns>
        private SpeedEventArgs DequeueData()
        {
            SpeedEventArgs spd = null;
            lock (_queueData)
            {
                while (_queueData.Count > 0)
                {
                    spd = _queueData.Dequeue();
                }
            }
            return spd;
        }
        /// <summary>
        /// 速度監視スレッド
        /// </summary>
        private double _beforeSpeed = 0.0;
        private void QueueThreadProc()
        {
            int count = 0;
            while (!_stopThread)
            {
                int _waitTime = this.EventTimer;

                //最新の速度データを取得する
                SpeedEventArgs spd;
                spd = DequeueData();

                if (_enableEvent == true)
                {
                    if (spd == null)
                    {
                        if (count == 2)
                        {
                            spd = new SpeedEventArgs(DateTime.Now, DateTime.Now, 0.0);
                        }
                        count++;
                    }
                    else
                    {
                        count = 0;
                        _waitTime = (int)(spd.EndTime - spd.StartTime).TotalMilliseconds;
                        if (_waitTime < this.EventTimer)
                        {
                            _waitTime = this.EventTimer;
                        }
                    }
                    if (spd != null)
                    {
                        if (_beforeSpeed != spd.Speed)
                        {
                            this.OnEventUpdateSpeedEvent(this, spd);
                            _beforeSpeed = spd.Speed;
                        }
                    }
                }
                if (_waitTime > this.EventTimer)
                    _waitTime = this.EventTimer;
                Thread.Sleep(_waitTime);
            }
        }
        #endregion


        #region スレッドの開始・終了
        /// <summary>
        /// 速度監視スレッドを開始する
        /// </summary>
        private void BeginThread()
        {
            if (_queueThread != null)
            {
                return;
            }
            _queueData.Clear();
            _stopThread = false;
            _queueThread = new Thread(new ThreadStart(QueueThreadProc));
            _queueThread.Name = "SpeedManager.QueueThreadProc";
            _queueThread.Start();
        }

        /// <summary>
        /// 速度監視スレッドを停止する
        /// </summary>
        private void EndThread()
        {
            if (_queueThread == null)
            {
                return;
            }
            _stopThread = true;
            do
            {
                _queueThread.Join(10);
                System.Windows.Forms.Application.DoEvents();
            }
            while (_queueThread.IsAlive == true);
            _queueData.Clear();
            _queueThread = null;
        }
        #endregion


        #region イベント通知の開始・停止
        /// <summary>
        /// 速度監視イベントの受け取りを開始する
        /// </summary>
        public void Start()
        {
            _enableEvent = true;
        }
        /// <summary>
        /// 速度監視イベントの受け取りを停止する
        /// </summary>
        public void Stop()
        {
            _enableEvent = false;
        }

        #endregion

    }
}
