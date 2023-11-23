using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fujita.Misc;

namespace Fujita.Misc
{
    public interface IError
    {
        bool IsError { get; set; }
        string ErrorReason { get; set; }
    }

    public class ErrorOccuredEventArgs : EventArgs
    {
        public bool Error
        {
            get;
            private set;
        }

        public string ErrorReason
        {
            get;
            private set;
        }

        public int Index
        {
            get;
            private set;
        }

        public IError Object
        {
            get;
            private set;
        }

        public ErrorOccuredEventArgs(bool bError, string sErrorReason, int iIndex, IError iError)
        {
            Index = iIndex;
            Error = bError;
            ErrorReason = sErrorReason;
            Object = iError;
        }
    }

    public class ErrorOccuredAllEventArgs : EventArgs
    {
        bool [] _bErros = null;
        public bool[] Error
        {
            get { return _bErros; }
        }

        string [] _sReasons = null;
        public string [] Reasons
        {
            get{return _sReasons;}

        }

        public ErrorOccuredAllEventArgs(bool [] bErros, string [] sReasons )
        {
            _bErros = bErros;
            _sReasons = sReasons;
        }
    }

    public delegate void ErrorOccuredEventHandler(object sender, ErrorOccuredEventArgs e);
    public delegate void ErrorOccuredAllEventHandler(object sender, ErrorOccuredAllEventArgs e);

    class clsErrorMonitor: IThreadSafeFinish
    {
        class ErrorInfo
        {
            bool _bFirst = true;
            bool _bPrev = false;
            public bool IsError
            {
                get { return _bPrev; }
            }
            bool _bEventOccured = false;
            public bool EventOccured
            {
                get { return _bEventOccured; }
            }
            public void SetStat( bool bNow)
            {
                if (!_bFirst)
                {
                    if (_bPrev != bNow)
                    {
                        _bEventOccured = true;
                    }
                    else
                    {
                        _bEventOccured = false;
                    }
                }
                else
                {
                    _bFirst = false;
                    if (bNow)
                    {
                        _bEventOccured = true;
                    }
                }
                _bPrev = bNow;
            }
        }

        public event ErrorOccuredEventHandler OnError;
        public event ErrorOccuredAllEventHandler OnErrorAll;

        List<IError> _lstErrorTarget = null;
        List<ErrorInfo> _lstInfo = null;

        public bool Initialize()
        {
            _lstErrorTarget = new List<IError>();
            _lstInfo = new List<ErrorInfo>();
            return true;
        }

        public bool Add(IError errContainer)
        {
            if (_lstErrorTarget == null)
                return false;

            lock (_lstErrorTarget)
            {
                if (_lstErrorTarget.Contains(errContainer))
                    return true;

                _lstErrorTarget.Add(errContainer);
                _lstInfo.Add(new ErrorInfo());
            }

            return true;
        }

        System.Threading.Thread _tThread = null;
        clsThreadSafeFinish _safeFinish = null;
        public bool Start()
        {
            if (_lstErrorTarget == null)
                return false;

            if (_safeFinish != null)
                return false;

            _tThread = new System.Threading.Thread(errorMonitor);
            _tThread.Name = "ｴﾗｰﾓﾆﾀｰ";
            _tThread.Start();

            return true;
        }

        public bool Stop()
        {
            if (_lstErrorTarget == null)
                return false;

            if (_tThread == null)
                return false;

            if (_safeFinish != null)
                return false;

            _safeFinish = new clsThreadSafeFinish(this);
            _safeFinish.SafeFinish(true);
            _safeFinish = null;
            return true;
        }

        public void ThreadSafeEnd()
        {
            _bStop = true;
            do
            {
                _tThread.Join(100);
            } while (_tThread.IsAlive);
        }

        bool _bStop = false;
        void errorMonitor()
        {
            bool bOccured = false;
            while (!_bStop)
            {
                bOccured = false;
                bool[] abErros;
                string[] asReasons;
                lock (_lstErrorTarget)
                {
                    abErros = _lstErrorTarget.Select(x => x != null ? x.IsError : false).ToArray();
                    asReasons = _lstErrorTarget.Select(x => x != null ? x.ErrorReason : "null object").ToArray();
                }

                for (int i = 0; i < abErros.Length; i++)
                {
//                    if (_lstErrorTarget[i] == null)
//                        continue;

                    _lstInfo[i].SetStat(abErros[i]);
                    if (_lstInfo[i].EventOccured )
                    {
                        bOccured = true;
                        if (OnError != null)
                        {
                            OnError(this, new ErrorOccuredEventArgs(abErros[i], asReasons[i], i, _lstErrorTarget[i]));
                        }
                    }
                }
                if ( bOccured && OnErrorAll != null )
                {
                    OnErrorAll( this, new ErrorOccuredAllEventArgs( abErros, asReasons));
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        public bool Terminate()
        {
            if (_lstErrorTarget == null)
                return true;

            Stop();

            while (_safeFinish != null) ;

            _lstErrorTarget.Clear();
            _lstErrorTarget = null;
            _lstInfo.Clear();
            _lstInfo = null;

            return true;
        }

    }
}
