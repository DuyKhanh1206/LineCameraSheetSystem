
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LineCameraSheetSystem
{
    class ChangeTimeHandlerEventArgs : EventArgs
    {
        long _lAccumulateSecond;
        public long AccumulateSecond
        {
            get { return _lAccumulateSecond; }
        }

        public long AccumulateMinute
        {
            get { return _lAccumulateSecond / (long)clsMeasPeriod.ECallbackSpan.Minute; }
        }

        public long AccumulateHour
        {
            get { return _lAccumulateSecond / (long)clsMeasPeriod.ECallbackSpan.Hour; }
        }

        public object UserParam
        {
            get;
            private set;
        }

        public ChangeTimeHandlerEventArgs(long lAccuSec, object user)
        {
            _lAccumulateSecond = lAccuSec;
            UserParam = user;
        }
    }

    delegate void ChangeTimeEventHandler( object sender, ChangeTimeHandlerEventArgs e );
    class clsMeasPeriod
    {
        public enum ECallbackSpan : long
        {
            Seconds = 1,
            Minute = 60,
            Hour = 3600,
        }

        public clsMeasPeriod()
        {
            tm.Tick += new EventHandler(tm_Tick);
            tm.Interval = 1000;
        }

        public ECallbackSpan CallbackSpan
        {
            get;
            set;
        }

        private long _lAccumulateSecond = 0;
        public long AccumulateSecond
        {
            get { return _lAccumulateSecond; }
        }

        public long AccumulateMinute
        {
            get { return _lAccumulateSecond / (long)ECallbackSpan.Minute; }
        }

        public long AccumulateHour
        {
            get { return _lAccumulateSecond / (long)ECallbackSpan.Hour; }
        }

        public void Clear()
        {
            _lAccumulateSecond = 0L;
            if (_fs == null)
            {
                using (FileStream fs = new FileStream(_sFilePath, FileMode.OpenOrCreate))
                {
                    writeValue(fs);
                    fs.Close();
                }
            }
            else
            {
                writeValue(_fs);
            }
        }

        private bool writeValue(FileStream fs)
        {
            try
            {
                byte[] bytData = BitConverter.GetBytes(_lAccumulateSecond);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(bytData, 0, bytData.Length);
                fs.Flush();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        Timer tm = new Timer();
        FileStream _fs = null;
        string _sFilePath = "";
        bool _bInitialize = false;
        object _oUser = null;

        public bool Initialize( object oUser )
        {
            _bInitialize = true;
            _oUser = oUser;

            CallbackSpan = ECallbackSpan.Hour;

            return true;
        }

        public bool Load(string sPath, string sSection)
        {
            if (!_bInitialize)
                return false;

            _sFilePath = sPath;

            if (File.Exists(sPath))
            {
                byte [] bytRead = new byte[sizeof(long)];
                // 値を読み出す
                using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate))
                {
                    fs.Read(bytRead, 0, sizeof(long));
                    _lAccumulateSecond = BitConverter.ToInt64( bytRead, 0);
                    fs.Close();
                }
            }
            else
            {
                _lAccumulateSecond = 0;
            }
            return true;
        }

        public bool Terminate()
        {
            if (!_bInitialize)
                return false;

            Stop();
            _bInitialize = false;

            return true;
        }

        // 経過時間測定
        public bool Start()
        {
            if (!_bInitialize)
                return false;

            if (_fs != null)
                return false;

            // ファイルをオープンする
            try
            {
                _fs = new FileStream(_sFilePath, FileMode.OpenOrCreate);
            }
            catch (Exception)
            {
                return false;
            }

            tm.Enabled = true;
            return true;
        }

        void tm_Tick(object sender, EventArgs e)
        {
            if (_fs == null)
                return;

            _lAccumulateSecond++;
            try
            {
                writeValue(_fs);
            }
            catch (Exception)
            {
            }

            if (_lAccumulateSecond % (long)CallbackSpan == 0)
            {
                if( ChangeTime != null )
                    ChangeTime(this, new ChangeTimeHandlerEventArgs(_lAccumulateSecond, _oUser));
            }
        }

        public bool Stop()
        {
            if (!_bInitialize)
                return false;

            tm.Enabled = false;

            if (_fs == null)
                return false;

            // ファイルを閉じる
            try
            {
                _fs.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                _fs = null;
            }
            return true;

        }

        public event ChangeTimeEventHandler ChangeTime = null;

    }
}
