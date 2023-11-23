using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Reflection;

using System.Threading;
using Fujita.Misc;
using Fujita.InspectionSystem;
using System.Drawing;

namespace Fujita.Communication
{
    public class CommunicationNet : CommunicationBase, IDisposable
    {
        protected TcpClient _tcpClient;
        protected NetworkStream _nwStream;
        protected ManualResetEvent _mreConnect;
        protected Exception _connEx;

        private string _sIP = "192.168.1.1";
        public string IP
        {
            get
            {
                return _sIP;
            }
            set
            {
                if (CommunicationNet.IsIPAddressCorrect(value)) // chỉ định địa chỉ IP
                    _sIP = value;
            }
        }
        private int _iPort = 1000;
        public int Port
        {
            get
            {
                return _iPort;
            }
            set
            {
                if (CommunicationNet.IsPortCorrect(value))
                    _iPort = value;
            }
        }

        public override bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();
            _sIP = ifa.GetIni(sSection, "IP", _sIP, sPath);
            _iPort = ifa.GetIni(sSection, "Port", _iPort, sPath);
            return base.Load(sPath, sSection);
        }

        // kiểm tra địa chỉ IP có đúng hay k
        static public bool IsIPAddressCorrect(string sTest)
        {
            // IPアドレス指定の場合
            Regex reg = new Regex(@"^(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])$");
            return reg.IsMatch(sTest);
        }

        static public bool IsPortCorrect(int iTest)
        {
            if (iTest < 0 || iTest > 65535)
                return false;
            return true;
        }


        public CommunicationNet(string name, string jpnName)
            : base(name, jpnName, ECommunicationType.Net)
        {
        }

        // có thể là kiểm tra kết nối (ping IP)
        private bool IsPing()
        {
            using (var ping = new System.Net.NetworkInformation.Ping())
            {
                System.Net.NetworkInformation.PingReply rep;
                try
                {
                    rep = ping.Send(_sIP, _iPort);
                    if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                        return true;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }
        private bool IsNetConnect(int iWaitTime)
        {
            _connEx = null;
            _mreConnect = new ManualResetEvent(false);
            _tcpClient = new TcpClient();
            IAsyncResult res = _tcpClient.BeginConnect(_sIP, _iPort, new AsyncCallback(ConnectedCallback), _tcpClient);
            if (!_mreConnect.WaitOne(iWaitTime))
            {
                // タイムアウトした場合
                _tcpClient.Close();
                //_tcpClient.Dispose();
                return false;
            }
            else if (_connEx == null)
            {
                return true;
            }
            return false;
        }

        public override bool Open()
        {
            if (IsOpen())// kiểm tra xem đã kết nối hay chưa
                return false;

            if (!IsIPAddressCorrect(_sIP))//iểm tra địa chỉ IP có đúng hay k
            {
                IsError = true;
                return false;
            }

            TryPing:

            try
            {
                bool isPingNg = false;
                bool isAbort = false;
                bool runCheckLoop = true;
                Action act;
                Action abort;
                frmProgressForm frmProg;
                if (IsPing() == false)
                {
                    isAbort = false;
                    runCheckLoop = true;
                    act = new Action(() =>
                    {
                        while (runCheckLoop)
                        {
                            if (IsPing() == true)
                                break;
                            else
                                Thread.Sleep(1000);
                        }
                    });
                    abort = new Action(() =>
                    {
                        isAbort = true;
                        runCheckLoop = false;
                    });
                    SplashForm.VisibleSplashwindow(false);
                    frmProg = new frmProgressForm(act, abort);
                    frmProg.TopMost = true;
                    frmProg.VisibleKeikaTime = true;
                    frmProg.ColorBackground = Color.HotPink;
                    frmProg.Description = this.JpnName + " 通信中[ping]...\nお待ちください。\n" + "本メッセージがしばらく表示される場合、\nこのままの状態で、\n" + this.JpnName + "のLANケーブルの抜き差しを\n行ってください。";
                    frmProg.ShowDialog();
                    SplashForm.VisibleSplashwindow(true);
                    if (isAbort == true)
                    {
                        return false;
                    }
                }

                if (IsNetConnect(1000) == false)
                {
                    isPingNg = false;
                    isAbort = false;
                    runCheckLoop = true;
                    act = new Action(() =>
                    {
                        while (runCheckLoop)
                        {
                            if (IsNetConnect(500) == true)
                            {
                                break;
                            }
                            else
                            {
                                if (IsPing() == false)
                                {
                                    isPingNg = true;
                                    break;
                                }
                                Thread.Sleep(500);
                            }
                        }
                    });
                    abort = new Action(() =>
                    {
                        isAbort = true;
                        runCheckLoop = false;
                    });
                    SplashForm.VisibleSplashwindow(false);
                    frmProg = new frmProgressForm(act, abort);
                    frmProg.TopMost = true;
                    frmProg.VisibleKeikaTime = true;
                    frmProg.ColorBackground = Color.GreenYellow;
                    frmProg.Description = this.JpnName + " 接続中[Connect]...\nお待ちください。\n" + "本メッセージがしばらく表示される場合、\nこのままの状態で、\n" + this.JpnName + "の電源を再投入してください。";
                    frmProg.ShowDialog();
                    SplashForm.VisibleSplashwindow(true);
                    if (isAbort == true)
                    {
                        return false;
                    }
                    if (isPingNg == true)
                        goto TryPing;
                }

                if (_connEx == null)
                {
                    if (_tcpClient.Connected == true)
                        _nwStream = _tcpClient.GetStream();
                }
                else
                {
                    setError(true, _connEx.Message);
                }
            }
            catch (ArgumentNullException e)
            {
                setError(true, e.Message);

            }
            catch (SocketException e)
            {
                setError(true, e.Message);
            }
            finally
            {
                if (_tcpClient != null && (_tcpClient.Client == null || !_tcpClient.Connected))
                {
                    _nwStream = null;
                    _tcpClient = null;
                }
                else
                {
                    setError(false, "");
                }
            }

            return (_tcpClient != null);
        }

        private void ConnectedCallback(IAsyncResult result)
        {
            try
            {
                TcpClient client = result.AsyncState as TcpClient;
                if (client.Client != null)
                {
                    client.EndConnect(result);
                }
                else
                {
                    _connEx = new Exception();
                    // BeginConnect中にTcpClient.Close
                    // を呼んだことにより発生
                    // 無視する
                }
            }
            catch (Exception ex)
            {
                _connEx = ex;
            }
            finally
            {
                _mreConnect.Set();
            }
        }


        public override bool Close()
        {
            if (!IsOpen())
                return true;
            try
            {
                _nwStream.Close();
                _tcpClient.Close();
            }
            catch (Exception)
            {
            }
            _nwStream = null;
            _tcpClient = null;
            IsError = false;

            return true;
        }

        public bool IsOpen()
        {
            return (_tcpClient != null);
        }

        public bool IsConnected()
        {
            if (!IsOpen())
                return false;

            return (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Connected);
        }

        public void Dispose()
        {
            Close();
        }


        public bool WriteString(string sData)
        {
            if (!IsConnected())
            {
                return false;
            }

            byte[] abytSendBuffer = System.Text.Encoding.ASCII.GetBytes(sData);
            try
            {
                _nwStream.Write(abytSendBuffer, 0, abytSendBuffer.Length);
                _nwStream.Flush();
            }
            catch (Exception)
            {
                return false;
            }
            setError(false);
            return true;
        }

        public bool ReadString(ref string sData) 
        {
            sData = "";

            if (!IsConnected())
            {
                return false;
            }

            if (_tcpClient.Available > 0)
            {
                byte[] bytReceive = new byte[_tcpClient.Available];
                try
                {
                    _nwStream.Read(bytReceive, 0, bytReceive.Length);
                    sData = Encoding.ASCII.GetString(bytReceive);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            setError(false);
            return true;
       } 
    }
}
