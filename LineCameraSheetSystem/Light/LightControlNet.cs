using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Fujita.Misc;
using Fujita.Communication;

namespace Fujita.LightControl
{
    public class LightPowerSupplay_IMAC : LightPowerSupplayBase
    {
        public LightPowerSupplay_IMAC(CommunicationNet net, string name)
            : base(null, null, net, name, "IMAC", 1, 0, 255, false)
        {
        }

        private Thread _thPolling = null;
        private bool _bStopPolling = false;
        public override bool Initialize()
        {
            if (_net != null)
            {
                if (SetTimeOut(ConnectTimeOut) == false)
                    return false;

                _thPolling = new Thread(new ThreadStart(monitor));// khởi tạo Thread 
                _thPolling.Name = _net.Name + ".PollingMonitor";
                _thPolling.IsBackground = true;
                _thPolling.Start();
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Terminate()
        {
            LightOffAll();

            if (_thPolling != null)
            {
                _bStopPolling = true;
                _thPolling = null;
            }
            return true;
        }


        // hàm kết nối 
        private void monitor()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            while (!_bStopPolling)
            {
                if (MonitorEnable == true)
                {
                    int ret = CheckStatus();
                    if (ret != 0)
                    {
                        string msgStr = "IMAC Monitor() - CheckStatus() ret : " + ret.ToString();
                        Console.WriteLine(msgStr);
                        LogingDllWrap.LogingDll.Loging_SetLogString(msgStr);

                        _net.Close();
                        _net.Open();
                    }
                }
                sw.Restart();
                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                    if (sw.ElapsedMilliseconds >= 3000)
                        break;
                    if (_bStopPolling == true)
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual int CheckStatus(int iRepeat = 5)
        {
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iTime"></param>
        /// <returns></returns>
        protected bool SetTimeOut(int iTime = 10)
        {
            if (_net == null)
                return false;

            lock (_net)
            {
                string sCmd = "W2701" + iTime.ToString("0000");
                return sendCommand(sCmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSend"></param>
        /// <param name="isCheckAck"></param>
        /// <param name="iRepeat"></param>
        /// <returns></returns>
        protected bool sendCommand(string sSend, bool isCheckAck = true, int iRepeat = 5)
        {
            string sReceive = "";
            int iError = 0;
            string sRetValue = "";
            bool bSuccess = false;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_objSendLock)
            {
                for (int n = 0; n < iRepeat; n++)
                {
                    if (!_net.WriteString(sSend))
                    {
                        continue;
                    }

                    if (isCheckAck == true)
                    {
                        for (int i = 0; i < iRepeat; i++)
                        {
                            sw.Restart();
                            while (true)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                System.Threading.Thread.Sleep(1);
                                if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                                    break;
                            }
                            _net.ReadString(ref sReceive); // đọc dữu liệu phản hồi và ném vào  string sReceive
                            if (sReceive.Length > 0)
                            {
                                // 受信したデータの確認 Xác nhận dữ liệu đã nhận
                                if (!AnalizeReceiveMesssage(sSend, sReceive, ref iError, ref sRetValue))   
                                {
                                    continue;
                                }

                                if (!IsAck(sRetValue))
                                {
                                    continue;
                                }
                                bSuccess = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        bSuccess = true;
                    }

                    if (bSuccess)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool LightOffAll()
        {
            bool bRet = true;
            for (int i = 0; i < ChannelNum; i++)
            {
                bRet &= LightOff(i);
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sTest"></param>
        /// <returns></returns>
        protected bool IsAck(string sTest)  // kiểm tra string sTesst có bằng ack không??? trả về true hoặc false
        {
            if (sTest.Length < 3)
                return false;
            return (sTest.Substring(0, 3).ToLower() == "ack");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSend"></param>
        /// <param name="sReceive"></param>
        /// <param name="iError"></param>
        /// <param name="sRetValue"></param>
        /// <returns></returns>
        private bool AnalizeReceiveMesssage(string sSend, string sReceive, ref int iError, ref string sRetValue)   // kiểm tra lệnh gửi và lệnh phản hồi có tương thích không và lỗi như nào
        {
            iError = 0;

            if (sSend.Length <= 3 && sReceive.Length <= 3)
            {
                iError = -1;
                return false;
            }

            // コマンドエラーの場合
            if (sReceive == "WR00NAK")
            {
                iError = -2;
                return false;
            }

            string sReceiveCmd = sReceive.Substring(0, 3);
            string sSendCmd = sSend.Substring(0, 3);

            if (sSendCmd != sReceiveCmd)
            {
                iError = -3;
                return false;
            }

            string sRet = sReceive.Substring(3, sReceive.Length - 3);
            sRetValue = sRet;

            return true;
        }
    }

    public class LightPowerSupplay_IMAC_IDGB : LightPowerSupplay_IMAC
    {
        // bắt buộc có khi khai báo bằng singleton bên class lightControlManager
        public LightPowerSupplay_IMAC_IDGB(CommunicationNet net, string name, int channel)
            : base(net, name)
        {
        }

        public override bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            if (_net == null || !_net.IsOpen())
            {
                return false;
            }

            if (iValue < ValueMin || iValue > ValueMax)
            {
                return false;
            }
            bool ret = true;
            lock (_net)
            {
                ret &= sendCommand("W11" + iChannel.ToString("00") + iValue.ToString("0000"), isCheckAck);
                LightType lt = getLight(iChannel); // dictionary trả về 1 kiểu lightType
                if (lt != null)
                {
                    foreach (int ch in lt.PareChannel)
                        ret &= sendCommand("W11" + ch.ToString("00") + iValue.ToString("0000"), isCheckAck);
                }
            }
            return ret;
        }

        public override bool LightOff(int iChannel) // send command tắt đèn thứ iChannel
        {
            if (_net == null || !_net.IsOpen())
                return false;

            bool ret = true;
            lock (_net)
            {
                ret &= sendCommand("W11" + iChannel.ToString("00") + "0000");           // tắt công iChannel
                LightType lt = getLight(iChannel);
                if (lt != null)
                {
                    foreach (int ch in lt.PareChannel) // tắt tất cả các cổng CH
                        ret &= sendCommand("W11" + ch.ToString("00") + "0000");
                }
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override int CheckStatus(int iRepeat)
        {
            int ret = -5;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_net)
            {
                //Ack確認なしで、コマンド送信               Gửi lệnh mà không cần xác nhận Ack
                string sCmd = "R080000";
                if (!sendCommand(sCmd, false))
                    return -1;

                //受信チェック                    Kiểm nhận được
                for (int i = 0; i < iRepeat; i++)
                {
                    sw.Restart();
                    while (true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1);
                        if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                            break;
                    }
                    string sReceive = "";
                    _net.ReadString(ref sReceive);                  // đọc kết quả và đẩy vào sReceive
                    if (sReceive.Length > 0)
                    {
                        try
                        {
                            if (!int.TryParse(sReceive.Substring(5, 4), out ret))
                                return -3;
                            else
                                break;
                        }
                        catch (Exception)
                        {
                            return -2;
                        }
                    }
                }
            }
            return ret;
        }
    }

    public class LightPowerSupplay_IMAC_IWDV_300S_24 : LightPowerSupplay_IMAC
    {
        public LightPowerSupplay_IMAC_IWDV_300S_24(CommunicationNet net, string name)
            : base(net, name)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iChannel"></param>
        /// <param name="iValue"></param>
        /// <param name="isCheckAck"></param>
        /// <returns></returns>
        public override bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            if (_net == null || !_net.IsOpen())
            {
                return false;
            }

            if (iValue < ValueMin || iValue > ValueMax)
            {
                return false;
            }
            bool ret = true;
            lock (_net)
            {
                int iRealValue = (int)(iValue * (999.0 / 255.0));
                ret = sendCommand("W12" + iRealValue.ToString("0000"), isCheckAck);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iChannel"></param>
        /// <returns></returns>
        public override bool LightOff(int iChannel)
        {
            if (_net == null || !_net.IsOpen())
                return false;
            bool ret = true;
            lock (_net)
            {
                ret = sendCommand("W120000");
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override int CheckStatus(int iRepeat)
        {
            int ret = -5;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_net)
            {
                //Ack確認なしで、コマンド送信
                string sCmd = "R080000";
                if (!sendCommand(sCmd, false))
                    return -1;

                //受信チェック
                for (int i = 0; i < iRepeat; i++)
                {
                    sw.Restart();
                    while (true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1);
                        if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                            break;
                    }
                    string sReceive = "";
                    _net.ReadString(ref sReceive);
                    if (sReceive.Length > 0)
                    {
                        try
                        {
                            if (!int.TryParse(sReceive.Substring(3, 4), out ret))
                                return -3;
                            else
                                break;
                        }
                        catch (Exception)
                        {
                            return -2;
                        }
                    }
                }
            }
            return ret;
        }
    }

    public class LightPowerSupplay_IMAC_IWDV_600M2_24 : LightPowerSupplay_IMAC
    {
        public LightPowerSupplay_IMAC_IWDV_600M2_24(CommunicationNet net, string name)
            : base(net, name)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iChannel"></param>
        /// <param name="iValue"></param>
        /// <param name="isCheckAck"></param>
        /// <returns></returns>
        public override bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            if (_net == null || !_net.IsOpen())
            {
                return false;
            }

            if (iValue < ValueMin || iValue > ValueMax)
            {
                return false;
            }
            bool ret = true;
            lock (_net)
            {
                int iRealValue = (int)(iValue * (999.0 / 255.0));
                ret = sendCommand("W12" + iChannel.ToString("00") + iRealValue.ToString("0000"), isCheckAck);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iChannel"></param>
        /// <returns></returns>
        public override bool LightOff(int iChannel)
        {
            if (_net == null || !_net.IsOpen())
                return false;
            bool ret = true;
            lock (_net)
            {
                ret = sendCommand("W12" + iChannel.ToString("00") + "0000");
            }
            return ret;
        }
        protected override int CheckStatus(int iRepeat)
        {
            int ret = -5;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_net)
            {
                //Ack確認なしで、コマンド送信
                string sCmd = "R080000";
                if (!sendCommand(sCmd, false))
                    return -1;

                //受信チェック
                for (int i = 0; i < iRepeat; i++)
                {
                    sw.Restart();
                    while (true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1);
                        if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                            break;
                    }
                    string sReceive = "";
                    _net.ReadString(ref sReceive);
                    if (sReceive.Length > 0)
                    {
                        try
                        {
                            if (!int.TryParse(sReceive.Substring(5, 4), out ret))
                                return -3;
                            else
                                break;
                        }
                        catch (Exception)
                        {
                            return -2;
                        }
                    }
                }
            }
            return ret;
        }
    }

    
    public class LightPowerSupplay_KYOTO : LightPowerSupplayBase
    {
        //C0:"C0"
        //5[0101][35]: Bit3:0=80kHz 1=157kHz Bit2:一括制御有効 Bit1:通信設定有効
        //4[0100][34]: Bit43: 00=通信on/off 01=外部on/off 10=内部on/off     Bit21:10=内部調光 01=外部調光 00=通信調光
        //1[0001][31]: 固定
        //0[0000][30]: 点灯=OFF
        //00:固定
        protected string COMMAND_HEADER = "C0541000";
        //private const string COMMAND_HEADER = "C0581000";     //フロントパネルで点灯・消灯

        public LightPowerSupplay_KYOTO(CommunicationNet net, string name, int channel)
            : base(null, null, net, name, "KYOTO", channel, 0, 255, false)
        {
        }

        private Thread _thPolling = null;
        private bool _bStopPolling = false;
        public override bool Initialize()
        {
            if (_net != null)
            {
                _thPolling = new Thread(new ThreadStart(monitor));
                _thPolling.Name = _net.Name + ".PollingMonitor";
                _thPolling.IsBackground = true;
                _thPolling.Start();
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Terminate()
        {
            LightOffAll();

            if (_thPolling != null)
            {
                _bStopPolling = true;
                _thPolling = null;
            }
            return true;
        }
        private void monitor()
        {
            int iCh = 1;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            while (!_bStopPolling)
            {
                if (MonitorEnable == true)
                {
                    int ret = CheckStatus(iCh);
                    if (ret != 0)
                    {
                        string msgStr = "KYOTO Monitor() - CheckStatus(iCh=" + iCh.ToString() + ") ret : " + ret.ToString();
                        Console.WriteLine(msgStr);
                        LogingDllWrap.LogingDll.Loging_SetLogString(msgStr);

                        _net.Close();
                        _net.Open();
                    }
                    iCh++;
                    if (iCh >= 4)
                        iCh = 1;
                }
                sw.Restart();
                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                    if (sw.ElapsedMilliseconds >= 3000)
                        break;
                    if (_bStopPolling == true)
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual int CheckStatus(int iChannel, int iRepeat = 5)
        {
            return 0;
        }

        public override bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            if (_net == null || !_net.IsOpen())
            {
                return false;
            }

            if (iValue < ValueMin || iValue > ValueMax)
            {
                return false;
            }

            bool ret = true;
            ret &= sendCommand(COMMAND_HEADER + iChannel.ToString() + iValue.ToString("000"), isCheckAck);
            LightType lt = getLight(iChannel);
            if (lt != null)
            {
                foreach (int ch in lt.PareChannel)
                    ret &= sendCommand(COMMAND_HEADER + ch.ToString() + iValue.ToString("000"), isCheckAck);
            }
            return ret;
        }

        public override bool LightOff(int iChannel)
        {
            if (_net == null || !_net.IsOpen())
                return false;

            bool ret = true;
            ret &= sendCommand(COMMAND_HEADER + iChannel.ToString() + (0).ToString("000"));
            LightType lt = getLight(iChannel);
            if (lt != null)
            {
                foreach (int ch in lt.PareChannel)
                    ret &= sendCommand(COMMAND_HEADER + ch.ToString() + (0).ToString("000"));

            }
            return ret;
        }

        public override bool LightOffAll()
        {
            bool bRet = true;
            for (int i = 0; i < ChannelNum; i++)
            {
                bRet &= LightOff(i);
            }
            return bRet;
        }

        /// <summary>
        /// チェックＳＵＭ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string CheckSumAdd(char stx, string str)
        {
            int startData = stx;
            string startString = ((char)startData).ToString();
            int sum = 0;
            string tmpStr = null;

            tmpStr = startString + str;

            for (int i = 0; i < tmpStr.Length; i++)
            {
                sum = sum + (int)tmpStr[i];
            }

            string tmp = Convert.ToString(sum, 16);

            tmp = tmp.Substring(tmp.Length - 2, 2);
            tmp = tmp.ToUpper();
            str = str + tmp;

            return str;
        }

        protected bool sendCommand(string sSend, bool isCheckAck = true, int iRepeat = 5)
        {
            string sReceive = "";
            int iError = 0;
            string sRetValue = "";
            bool bSuccess = false;

            char stx = (char)0x2;
            char etx = (char)0x3;

            string chkSumStr = CheckSumAdd(stx, sSend);
            string totalSend = stx.ToString() + chkSumStr + etx.ToString();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_objSendLock)
            {
                for (int n = 0; n < iRepeat; n++)
                {
                    if (!_net.WriteString(totalSend))
                    {
                        continue;
                    }

                    if (isCheckAck == true)
                    {
                        for (int i = 0; i < iRepeat; i++)
                        {
                            sw.Restart();
                            while (true)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                System.Threading.Thread.Sleep(1);
                                if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                                    break;
                            }
                            _net.ReadString(ref sReceive);
                            if (sReceive.Length > 0)
                            {
                                // 受信したデータの確認
                                if (!AnalizeReceiveMesssage(sSend, sReceive, ref iError, ref sRetValue) || !IsAck(sRetValue))
                                {
                                    continue;
                                }
                                bSuccess = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        bSuccess = true;
                    }
                    if (bSuccess)
                        return true;
                }
            }
            return false;
        }

        private bool IsAck(string sTest)
        {
            if (sTest.Length < 2)
                return false;
            return (sTest.Substring(0, 2).ToLower() == "r0");
        }

        private bool AnalizeReceiveMesssage(string sSend, string sReceive, ref int iError, ref string sRetValue)
        {
            iError = 0;
            int pt = sReceive.IndexOf("R0");
            if (pt < 0)
            {
                iError = -2;
                return false;
            }
            sRetValue = "R0";
            return true;
        }
    }

    /// <summary>
    /// 京都電機器：LEKシリーズ
    /// </summary>
    public class LightPowerSupplay_KYOTO_LEK : LightPowerSupplay_KYOTO
    {
        //C0:"C0"
        //5[0101][35]: Bit3:0=80kHz 1=157kHz Bit2:一括制御有効 Bit1:通信設定有効
        //4[0100][34]: Bit43: 00=通信on/off 01=外部on/off 10=内部on/off     Bit21:10=内部調光 01=外部調光 00=通信調光
        //1[0001][31]: 固定
        //0[0000][30]: 点灯=OFF
        //00:固定
        //private const string COMMAND_HEADER = "C0581000";     //フロントパネルで点灯・消灯

        public LightPowerSupplay_KYOTO_LEK(CommunicationNet net, string name, int channel)
            : base(net, name, channel)
        {
            COMMAND_HEADER = "C0541000";
        }
    }

    /// <summary>
    /// 京都電機器：LDAシリーズ
    /// </summary>
    public class LightPowerSupplay_KYOTO_LDA : LightPowerSupplay_KYOTO
    {
        //C0:"C0"
        //1[0001][31]: Bit3:0=80kHz 1=157kHz Bit2:一括制御有効 Bit1:通信設定有効
        //0[0000][30]: Bit43: 00=通信on/off 01=外部on/off 10=内部on/off     Bit21:10=内部調光 01=外部調光 00=通信調光
        //1[0001][31]: 固定
        //?[1111][3F]: 点灯=ON
        //00:固定
        //private const string COMMAND_HEADER = "C0581000";     //フロントパネルで点灯・消灯

        public LightPowerSupplay_KYOTO_LDA(CommunicationNet net, string name, int channel)
            : base(net, name, channel)
        {
            COMMAND_HEADER = "C0101?00";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override int CheckStatus(int iChannel, int iRepeat)
        {
            int ret = -5;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            lock (_net)
            {
                //Ack確認なしで、コマンド送信
                string sCmd = "C4" + iChannel.ToString();
                if (!sendCommand(sCmd, false))
                    return -1;

                //受信チェック
                for (int i = 0; i < iRepeat; i++)
                {
                    sw.Restart();
                    while (true)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1);
                        if (sw.ElapsedMilliseconds >= RESPONSE_WAIT_SLEEPTIME)
                            break;
                    }
                    string sReceive = "";
                    _net.ReadString(ref sReceive);
                    if (sReceive.Length > 0)
                    {
                        try
                        {
                            if (sReceive.IndexOf("R4") < 0)
                            {
                                return -3;
                            }
                            else
                            {
                                ret = 0;
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            return -2;
                        }
                    }
                }
            }
            return ret;
        }
    }
}
