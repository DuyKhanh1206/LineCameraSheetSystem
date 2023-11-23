using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.InspectionSystem
{
    /// <summary>
    /// オペレーターの情報。
    /// </summary>
    public class User
    {
        /// <summary>
        /// 権限。
        /// </summary>
        public EAuthenticationType Authentication { get; set; }

        public string AuthenticationJpn
        {
            get
            {
                switch (Authentication)
                {
                    case EAuthenticationType.Operator:
                        return "オペレーター";
                    case EAuthenticationType.Administrator:
                        return "管理者";
                    case EAuthenticationType.Developer:
                        return "開発者";
                }
                return "";
            }
        }

        /// <summary>
        /// インスタンスを初期化する。
        /// </summary>
        /// <param name="authentication">権限。</param>
        public User(EAuthenticationType authentication)
        {
            this.Authentication = authentication;
        }
    }

    /// <summary>
    /// 権限種別
    /// </summary>
    public enum EAuthenticationType
    {
        /// <summary>オペレータ</summary>
        Operator,
        /// <summary>管理者</summary>
        Administrator,
        /// <summary>開発者</summary>
        Developer,
    }
}
