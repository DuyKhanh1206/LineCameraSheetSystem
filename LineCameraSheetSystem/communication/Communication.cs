using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;


namespace Fujita.Communication
{
    public enum ECommunicationType
    {
        DIO,
        SIO,
        Net,
        Error,
    }

    public static class CommunicationTypeExt
    {
        public static string ToStringExt(this ECommunicationType e)
        {
            switch (e)
            {
                case ECommunicationType.DIO: return "ﾃﾞｼﾞﾀﾙIO";
                case ECommunicationType.SIO: return "ｼﾘｱﾙIO";
                case ECommunicationType.Net: return "ﾈｯﾄﾜｰｸ";
            }
            return "";
        }
    }

    public class CommunicationBase : IError
    {
        protected CommunicationBase(string name, string jpnName, ECommunicationType type)
        {
            Name = name;
            JpnName = jpnName;
            Type = type;
        }

        public string Name { get; private set; }
        public string JpnName { get; private set; }
        public ECommunicationType Type { get; private set; }

        public bool IsControl { get; private set; }
        public bool IsLightControl { get; private set; }

        /// <summary>
        /// エラープロパティ
        /// </summary>
        public bool IsError { get; set; }
        public string ErrorReason { get; set; }
        protected void setError(bool bError, string sReason = "")
        {
            IsError = bError;
            ErrorReason = sReason;
        }

        internal void setControls(bool control, bool lightcontrol)
        {
            IsControl = control;
            IsLightControl = lightcontrol;
        }

        public virtual bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();
            bool bControl = ifa.GetIni(sSection, "IsControl", false, sPath);
            bool bLightControl = ifa.GetIni(sSection, "IsLightControl", false, sPath);
            setControls(bControl, bLightControl);
            return true;
        }

        public virtual bool Open()
        {
            return false;
        }

        public virtual bool Close()
        {
            return false;
        }
#if !FUJITA_INSPECTION_SYSTEM
        protected void TraceError(string sMes, string sMethod)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("{0}-{1}", sMes, sMethod));
        }
#endif
    }

    public class CommunicationError : CommunicationBase
    {
        public CommunicationError(string name, string jpnName, string reason)
            : base(name, jpnName, ECommunicationType.Error)
        {
            setError(true, reason);
            setControls(false, false);
        }
    }
}
