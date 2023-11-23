using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UpsRemote;

namespace LineCameraSheetSystem
{
    /// <summary>
    /// UPSコントローラ。
    /// </summary>
    public class UpsController : IDisposable
    {
        /// <summary>
        /// UPSリモートサービス
        /// </summary>
        private UpsRemoteService _service;

        /// <summary>
        /// UPSイベントを受信したときのイベント
        /// </summary>
        public event UpsRemoteEventHandller OnUpsRemoteEvent;
        /// <summary>
        /// UPSイベントの最新ステータスコード。
        /// </summary>
        public UpsEventCode LastUpsEventCode { get; private set; }
        /// <summary>
        /// 停電中か否か。
        /// </summary>
        public bool IsBreakDown { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public UpsController()
        {
            this.LastUpsEventCode = UpsEventCode.None;
            this.IsBreakDown = false;
        }

        /// <summary>
        /// 初期化処理を行う。
        /// </summary>
        public void Initialize()
        {
            this._service = new UpsRemoteService();
            this._service.OnUpsRemoteEvent += this.Service_OnUpsRemoteEvent;
            this._service.Open();
        }

        /// <summary>
        /// オブジェクトを破棄する。
        /// </summary>
        public void Dispose()
        {
            this._service.OnUpsRemoteEvent -= this.Service_OnUpsRemoteEvent;
            this._service.Dispose();
        }

        /// <summary>
        /// UPSにシャットダウンコマンドを送信する。
        /// シャットダウンコマンドを送信したら電源が切れるので、
        /// 直ちにアプリを終了させること。
        /// </summary>
        //public void ShutDown()
        //{
        //    string cmd = Program.Context.Settings.ShutdownCommand;
        //    string arg = Program.Context.Settings.ShutdownArgument;
        //    if (File.Exists(cmd))
        //    {
        //        try
        //        {
        //            System.Diagnostics.Process.Start(cmd, arg);
        //            Log.Write(this, "UPSシャットダウンコマンド実行");
        //        }
        //        catch
        //        {
        //            Log.Write(this, string.Format("UPSシャットダウンコマンド失敗 コマンド:{0} 引数:{1}", cmd, arg));
        //        }
        //    }
        //    else
        //    {
        //        Log.Write(this, string.Format("UPSシャットダウンコマンドがみつかりません: {0}", cmd));
        //    }
        //}

        /// <summary>
        /// UPSリモートサービスがUPSイベント受信イベントのハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Service_OnUpsRemoteEvent(object sender, UpsRemoteEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(">>>OnUpsRemoteEvent code=" + e.Code.ToString());

            this.LastUpsEventCode = (UpsEventCode)e.Code;

            switch (this.LastUpsEventCode)
            {
                case UpsEventCode.BreakDown:
                    this.IsBreakDown = true;
                    break;
                case UpsEventCode.PowerFailRecovery:
                    this.IsBreakDown = false;
                    break;
            }

//            Log.Write(this, "Service_OnUpsRemoteEvent:" + this.LastUpsEventCode.ToString());

            Action action = () =>
            {
                UpsRemoteEventHandller handler = this.OnUpsRemoteEvent;
                if (null != handler)
                {
                    handler(this, e);
                }
            };
            action.InvokeAsync();
        }
    }
}
