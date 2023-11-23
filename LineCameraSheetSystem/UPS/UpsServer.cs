using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

using UpsRemote;

namespace UpsRemote
{
    /// <summary>
    /// UPSで発生したイベントの通知を受信するサービス
    /// </summary>
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class UpsRemoteService : IUpsRemoteService, IDisposable
    {
        /// <summary>
        /// UPSイベントを受信したときに発生するイベント
        /// </summary>
        public event UpsRemoteEventHandller OnUpsRemoteEvent;

        /// <summary>
        /// サービスホスト
        /// </summary>
        private ServiceHost _svc = null;

        /// <summary>
        /// インスタンスを初期化する。
        /// </summary>
        public UpsRemoteService()
        {
        }
        
        /// <summary>
        /// リソースを破棄する。
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// サービスを開始する。
        /// </summary>
        public void Open()
        {
            _svc = new ServiceHost(this);
            NetNamedPipeBinding binding = new NetNamedPipeBinding();
            _svc.AddServiceEndpoint(typeof(IUpsRemoteService), binding, "net.pipe://localhost/ups-remote-service");
            _svc.Open();
        }

        /// <summary>
        /// サービスを終了する。
        /// </summary>
        public void Close()
        {
            _svc.Close();
        }

        /// <summary>
        /// UPSイベントをサーバーに通知する。
        /// </summary>
        /// <param name="code"></param>
        public void PublishEvent(int code)
        {
            if (this.OnUpsRemoteEvent != null)
            {
                this.OnUpsRemoteEvent(this, new UpsRemoteEventArgs(code));
            }
        }
    }

    /// <summary>
    /// UPSイベントの情報。
    /// </summary>
    public class UpsRemoteEventArgs : EventArgs
    {
        /// <summary>
        /// イベントコード 1=停電、2=復旧
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// UPSイベントコード。
        /// </summary>
        public UpsEventCode EventCode { get; private set; }

        /// <summary>
        /// インスタンスを初期化する。
        /// </summary>
        /// <param name="code">イベントコード。</param>
        public UpsRemoteEventArgs(int code)
            : base()
        {
            this.Code = code;
            this.EventCode = (UpsEventCode)this.Code;
        }
    }
    public delegate void UpsRemoteEventHandller(object sender, UpsRemoteEventArgs e);
}
