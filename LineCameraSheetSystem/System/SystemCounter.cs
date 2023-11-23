using Fujita.InspectionSystem;
using Fujita.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineCameraSheetSystem
{
    class SystemCounter //v1330追加
    {
        #region ■Singleton
        private static SystemCounter _singleton = new SystemCounter();
        public static SystemCounter GetInstance()
        {
            return _singleton;
        }
        #endregion
        /// <summary>カメラ通信エラー</summary>
        public int CamCommunicationError { get; set; }

        public void Load(string sPath)
        {
            IniFileAccess ini = new IniFileAccess();
            string sec;

            //Cam
            sec = "Cam";
            CamCommunicationError = ini.GetIni(sec, GetNameClass.GetName(() => CamCommunicationError), 0, sPath);
        }

        public void Save(string sPath)
        {
            bool bFileExist = System.IO.File.Exists(sPath);
            string sMoveFile = "";

            if (bFileExist == true)
            {
                sMoveFile = sPath + DateTime.Now.ToString("_yyyyMMdd_HHmmss");
                System.IO.File.Move(sPath, sMoveFile);
            }

            try
            {
                IniFileAccess ini = new IniFileAccess();
                string sec;

                //Cam
                sec = "Cam";
                ini.SetIni(sec, GetNameClass.GetName(() => CamCommunicationError), CamCommunicationError, sPath);

                //Flush
                ini.Flush(sPath);
            }
            catch (Exception)
            {
                if(bFileExist == true)
                    System.IO.File.Copy(sMoveFile, sPath, true);
            }
            finally
            {
                if (bFileExist == true)
                    System.IO.File.Delete(sMoveFile);
            }
        }
    }
}
