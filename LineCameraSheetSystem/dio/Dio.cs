using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;

namespace Fujita.Communication
{
    /// <summary>
    /// DIOを制御する。
    /// </summary>
    public class Dio : IDio
    {
        /// <summary>
        /// DIOのデバイス名。
        /// </summary>
        public string DeviceName { get; private set; }

        /// <summary>
        /// インスタンスを初期化する。
        /// </summary>
        /// <param name="deviceName">DIOのデバイス名。</param>
        public Dio(string deviceName)
        {
            this.DeviceName = deviceName;
        }

        /// <summary>
        /// リソースを破棄する。Loại bỏ nguồn
        /// </summary>
        public void Dispose() // đóng, vứt bỏ
        {
            DioCtrl.Close();
        }

        /// <summary>
        /// DIOをオープンする。 Mở DIO
        /// </summary>
        public void Open()
        {
            int ret;
            ret = DioCtrl.Open(this.DeviceName);
        }

        /// <summary>
        /// 割り込みを開始する。       bắt đầu ngắt
        /// </summary>
        public void BeginIr()
        {
            // デリゲートがGCされないように参照を保持する       Giữ nguyên các tham chiếu đến đại biểu, để tránh GC
            _onIrIn1 = OnIrIn1;
            _onIrIn2 = OnIrIn2;

            // 割り込み信号が発生したときのコールバックを設定する        Đặt cuộc gọi lại khi xảy ra tín hiệu ngắt
            DioCtrl.SetOnIrIn1(_onIrIn1);
            DioCtrl.SetOnIrIn2(_onIrIn2);

            // 割り込みを開始する                bắt đầu ngắt
            DioCtrl.IrStart();
        }

        /// <summary>
        /// 割り込みを終了する。      kết thúc ngắt
        /// </summary>
        public void EndIr()
        {
            // 割り込みを終了する        kết thúc ngắt
            DioCtrl.IrStop();
        }

        /// <summary>
        /// 1点に出力する。        Đầu ra tới 1 điểm number
        /// </summary>
        /// <param name="number">出力ポート番号。</param>
        /// <param name="value">出力値。</param>
        public void OutputPoint(uint number, bool value)
        {
            int ret;
            ret = DioCtrl.OutputPoint(number, value ? 1 : 0);
        }

        /// <summary>
        /// 1点に指定時間出力する。        Xuất ra một điểm trong một thời gian nhất định
        /// </summary>
        /// <param name="number">出力ポート番号。</param>
        /// <param name="value">出力値。</param>
        /// <param name="holdTime">出力時間(ミリ秒)</param>
        public void OutputPoint(uint number, bool value, int holdTime)
        {
            int ret;
            ret = DioCtrl.OutputPoint(number, value ? 1 : 0);
            Thread.Sleep(holdTime);
            ret = DioCtrl.OutputPoint(number, !value ? 1 : 0);
        }

        /// <summary>
        /// 1点から入力する。           Đầu vào từ 1 điểm
        /// </summary>
        /// <param name="number">入力ポート番号。</param>
        /// <returns>入力値。</returns>
        public bool InputPoint(uint number)
        {
            int ret;
            ret = DioCtrl.InputPoint(number);

            return (ret != 0);
        }

        /// <summary>
        /// 1バイト(8bit)出力する。     Đầu ra 1 byte (8bit).
        /// </summary>
        /// <param name="number">出力グループ番号。</param>
        /// <param name="value">出力値。</param>
        public void OutputByte(uint number, byte value)
        {
            int ret;
            ret = DioCtrl.OutputByte(number, (int)value);
        }

        /// <summary>
        /// 4ビット出力する。       Đầu ra 4 bit
        /// </summary>
        /// <param name="numbers">ポート番号を格納した配列。</param>
        /// <param name="value">出力値。</param>
        public void Output4Bit(uint[] numbers, byte value)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                this.OutputPoint(numbers[i], (((value >> i) & 1) > 0));
            }
        }


        #region 擬似エンコーダと排出信号の割り込み           Bộ mã hóa giả và ngắt tín hiệu phóng
        /// <summary>
        /// IrIN-1のコールバック関数。            Chức năng gọi lại IrIN-1
        /// </summary>
        private static void OnIrIn1()
        {
//            Program.Context.Result.SetIrIn1Event();
        }
        /// <summary>
        /// IrIN-2のコールバック関数。            Chức năng gọi lại IrIN-2.
        /// </summary>
        private static void OnIrIn2()
        {
//            Program.Context.Result.SetIrIn2Event();
        }
        private DioCtrl.OnIrIn1 _onIrIn1;
        private DioCtrl.OnIrIn2 _onIrIn2;
        #endregion
    }

    /// <summary>
    /// DioCtrl.DLL のラッパー。          Trình gói cho DioCtrl.DLL.
    /// </summary>
    public class DioCtrl
    {
        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern int Open(string lpszName);

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern void Close();

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern int OutputPoint(uint number, int value);

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern int InputPoint(uint number);

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern int OutputByte(uint number, int value);

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern void SetOnIrIn1(OnIrIn1 onIrIn1);
        public delegate void OnIrIn1();

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern void SetOnIrIn2(OnIrIn2 onIrIn2);
        public delegate void OnIrIn2();

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern void IrStart();

        [DllImport("DioCtrl.dll", CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity]
        public static extern void IrStop();
    }
}
