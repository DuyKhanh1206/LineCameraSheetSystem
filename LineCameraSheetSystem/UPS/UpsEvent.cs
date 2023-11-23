using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace UpsRemote
{
    /// <summary>
    /// UPSで発生したイベントをサーバーに通知するためのサービス
    /// </summary>
    [ServiceContract]
    public interface IUpsRemoteService
    {
        /// <summary>
        /// UPSのイベントを検査アプリに通知する。
        /// </summary>
        /// <param name="code">UPSで発生したイベントのコード (1=停電発生、2=停電復旧)</param>
        [OperationContract]
        void PublishEvent(int code);
    }

    /// <summary>
    /// UPSで発生したイベントのコード
    /// </summary>
    public enum UpsEventCode : int
    {
        /// <summary>0:イベントなし</summary>
        None = 0,
        /// <summary>1:停電発生</summary>
        BreakDown = 1,
        /// <summary>2:停電復旧</summary>
        PowerFailRecovery = 2,
    }
}
