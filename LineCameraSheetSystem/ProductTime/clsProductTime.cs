using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace LineCameraSheetSystem

{
    class clsProductTime
    {   
        string _sDirPath;
        public string DirPath
        {
            get { return _sDirPath; }
            set
            {
                _sDirPath = value;
            }
        }

        long _lSeekPos = 0;
        string _sRecipeName;
        string _sLotNo;
        DateTime _dtStart;

        public bool Initialize(string sDirPath)
        {
            _sDirPath = pathChar(sDirPath);
            if (!Directory.Exists(_sDirPath))
                Directory.CreateDirectory(_sDirPath);

            _sRecipeName = "";
            _sLotNo = "";

            return true;
        }

        public bool WriteProductTimeStart(string sRecipeName, string sLotNo, DateTime dtStart)
        {
            if (sRecipeName == "")
                return false;

            if (_sRecipeName != "")
                return false;

            _sRecipeName = sRecipeName;
            _sLotNo = sLotNo;
            _dtStart = dtStart;

            string sFilePath = _sDirPath + _dtStart.ToString("yyyyMM") + ".csv";

            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("Shift_JIS")))
                    {
                        fs.Seek(0, SeekOrigin.End);
                        _lSeekPos = fs.Position;
                        sw.WriteLine(_sRecipeName + "," + _sLotNo + "," + _dtStart.ToString("yyyy/MM/dd hh:mm:ss") + "," + "----/--/-- --:--:--" + "," + "00000000.000" + "," + "00000000");
                    }
                }
            }
            catch (Exception e)
            {
                _sRecipeName = "";
                _sLotNo = "";
                TraceLogger(e.Message);
                return false;
            }

            return true;
        }

        public bool WriteProductTimeEnd( DateTime dtEnd, double dMeasMeter, int iTotalNGCnt)
        {
            if (_sRecipeName == "")
                return false;

            string sFilePath = _sDirPath + _dtStart.ToString("yyyyMM") + ".csv";

            StreamWriter sw = null;
            try
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
                {
                    using (sw = new StreamWriter(fs, Encoding.GetEncoding("Shift_JIS")))
                    {
                        fs.Seek(_lSeekPos, SeekOrigin.Begin);
                        sw.WriteLine(_sRecipeName + "," + _sLotNo + "," + _dtStart.ToString("yyyy/MM/dd hh:mm:ss") + "," + dtEnd.ToString("yyyy/MM/dd hh:mm:ss") + "," + string.Format("{0:00000000.000}", dMeasMeter) + "," + string.Format("{0:00000000}", iTotalNGCnt));
                        _sRecipeName = "";
                        _sLotNo = "";
                    }
                }
            }
            catch (Exception e)
            {
                _sRecipeName = "";
                _sLotNo = "";
                TraceLogger(e.Message);
                return false;
            }

            return true;
        }

         private void TraceLogger(string sMessage)
         {
//             System.Diagnostics.Debug.WriteLine(sMessage);
             LogingDllWrap.LogingDll.Loging_SetLogString(sMessage);
         }

         string pathChar(string sPath)
         {
            if (sPath == "")
                return "";
            if (sPath[sPath.Length - 1] != '\\')
                return (sPath + '\\');
            return sPath;
         }
    }
}
