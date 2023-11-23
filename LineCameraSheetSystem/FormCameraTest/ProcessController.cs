using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Fujita.Misc
{
    public class ProcessController : IDisposable
    {
        Process _myProc = null;
        public string ExePath { get; private set; }
        public string Argument {get; private set; }

        public ProcessController(string exepath, string argument)
        {
            ExePath = exepath;
            Argument = argument;
        }

        public bool Start()
        {
            if (_myProc != null)
                return false;

            _myProc = new Process();
            _myProc.StartInfo.FileName = ExePath;
            _myProc.StartInfo.Arguments = Argument;
            _myProc.Start();
            return true;
        }

        public bool IsExistProcessWindow(long timeout)
        {
            TimeSpan timespan = new TimeSpan(timeout);
            DateTime startnow = DateTime.Now;

            do
            {
                Thread.Sleep(1);
                try
                {
                    if (_myProc.MainWindowHandle != IntPtr.Zero)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            } while (DateTime.Now - startnow < timespan);

            return false;
        }

        public bool Close()
        {
            if (_myProc == null)
                return false;

            _myProc.CloseMainWindow();
            _myProc.Dispose();
            _myProc = null;

            return true;
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class TenkeyController : IDisposable
    {
        const string PROCESS_NAME = "pboard";

        string _sExePath = "";

        public int Top{get; set;}
        public int Left{get; set;}
        public int Width{get; private set;}
        public int Height{get; private set;}

        public TenkeyController(string sExePath)
        {
            _sExePath = sExePath;
            if (_sExePath[_sExePath.Length - 1] != '\\')
            {
                _sExePath = _sExePath + "\\";
            }

            getTenkeyInfo();
        }

        private void getTenkeyInfo()
        {
            string sPath = _sExePath + PROCESS_NAME + ".ini";
            IniFileAccess ifa = new IniFileAccess();

            Top = ifa.GetIni("Form", "Top", Top, sPath);
            Left = ifa.GetIni("Form", "Left", Left, sPath);
            Width = ifa.GetIni("Form", "Width", Width, sPath);
            Height = ifa.GetIni("Form", "Height", Height, sPath);
        }

        Process _proc = null;

        public bool Start()
        {
            if (_proc != null)
                Close();

            string sExeFullPath = _sExePath + PROCESS_NAME + ".exe";
            if (!System.IO.File.Exists(sExeFullPath))
                return false;

            _proc = new Process();
            _proc.StartInfo.FileName = sExeFullPath;
            _proc.StartInfo.Arguments = "";
            return _proc.Start();
        }

        public bool Start(int iX, int iY)
        {
            string sFileName = _sExePath + PROCESS_NAME + ".ini";
            IniFileAccess ifa = new IniFileAccess();
            ifa.SetIni("Form", "Left", iX, sFileName);
            ifa.SetIni("Form", "Top", iY, sFileName);

            return Start();
        }

        public bool Close()
        {
            Process proc = null;
            if (_proc != null && isProcessExist(ref proc))
            {
                Process procDummy = new Process();
                procDummy.StartInfo.FileName = _sExePath + PROCESS_NAME + ".exe";
                procDummy.StartInfo.Arguments = "";
                procDummy.Start();
                while (isProcessExist(ref proc)) ;
            }
            _proc = null;
            return true;
        }

        public bool IsProcessExist()
        {
            if (_proc == null)
                return false;

            Process dummy = null;
            return isProcessExist(ref dummy);
        }

        public bool isProcessExist(ref Process procosk)
        {
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.ToLower() == PROCESS_NAME)
                {
                    procosk = proc;
                    return true;
                }
            }
            return false;
        }

        // 概要:
        //     アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        public void Dispose()
        {
            Close();
        }

    }

    public class OSKController: IDisposable
    {
        const string PROCESS_NAME = "osk";

        Process _procOSK = null;
        bool _bAlreadyExist = false;
        public bool Start()
        {
            // すでにプロセスが存在している場合
            if (isProcessExist(ref _procOSK))
            {
                _bAlreadyExist = true;
                return true;
            }

            _bAlreadyExist = false;
            _procOSK = new Process();
            _procOSK.StartInfo.FileName = PROCESS_NAME;
            _procOSK.Start();

            return true;
        }

        public bool IsProcessExited()
        {
            if (_procOSK == null)
                return true;

            return _procOSK.HasExited;
        }

        public bool WaitWindowValidate(long timeout)
        {
            if (_procOSK == null)
                return false;

            if (IsProcessExited())
                return true;

            TimeSpan timewait = new TimeSpan(timeout);
            DateTime starttime = DateTime.Now;
            do
            {
                try
                {
                    if (_procOSK.MainWindowHandle != IntPtr.Zero)
                        return true;
                }
                catch (Exception)
                {
                }

            } while (DateTime.Now - starttime < timewait);

            // タイムアウトした
            return false;
        }

        public bool isProcessExist(ref Process procosk)
        {
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.ToLower() == PROCESS_NAME)
                {
                    procosk = proc;
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            Close();
        }

        public bool Close()
        {
            if (_procOSK == null)
                return false;

            if (IsProcessExited())
            {
                _procOSK = null;
                return true;
            }

            if (_bAlreadyExist)
            {
                _procOSK = null;
            }
            else
            {
                try
                {
                    while (!IsProcessExited())
                    {
                        _procOSK.Kill();
                    }


                    _procOSK = null;
                }
                catch (Exception)
                {
                }
            }
            return true;
        }
    }
}
