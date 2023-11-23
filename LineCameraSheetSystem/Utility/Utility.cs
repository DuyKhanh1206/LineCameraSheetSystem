using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    /// <summary>
    /// 全体で使用するユーティリティ。
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// ディスク容量をチェックする。
        /// </summary>
        /// <param name="path">ドライブを含んだパス。</param>
        /// <param name="capacity">残容量（パーセント）。</param>
        /// <returns>容量が足りない場合は true、足りている場合は false。</returns>
        public static bool IsDiskFull(string path, int capacity)
        {
            bool diskFull = true;
            try
            {
                DriveInfo di = new DriveInfo(path.Substring(0, 1));
                if (di.IsReady)
                {
                    long used = (di.TotalSize - di.AvailableFreeSpace) * 100 / di.TotalSize;
                    if (used < capacity)
                    {
                        diskFull = false;
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
            return diskFull;
        }

        /// <summary>
        /// 値を最小値以上、最大値以下に補正する。
        /// </summary>
        /// <param name="value">値。</param>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>補正値。</returns>
        public static double CorrectValue(double value, double minValue, double maxValue)
        {
            double v = value;
            double min = Math.Min(minValue, maxValue);
            double max = Math.Max(minValue, maxValue);
            v = Math.Min(v, max);
            v = Math.Max(v, min);
            return v;
        }

        /// <summary>
        /// 値を最小値以上、最大値以下に補正する。
        /// </summary>
        /// <param name="value">値。</param>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>補正値。</returns>
        public static int CorrectValue(int value, int minValue, int maxValue)
        {
            int v = value;
            int min = Math.Min(minValue, maxValue);
            int max = Math.Max(minValue, maxValue);
            v = Math.Min(v, max);
            v = Math.Max(v, min);
            return v;
        }

        /// <summary>
        /// 実数配列の HTuple を実数の列挙文字列に変換する。
        /// </summary>
        /// <param name="realList">実数配列の HTuple。</param>
        public static string ToRealList(HTuple realList)
        {
            List<string> ss = new List<string>();
            double[] scores = realList.TupleReal();
            foreach (double score in scores)
                ss.Add(score.ToString("F1"));
            string list = string.Join(",", ss.ToArray());

            return list;
        }

        /// <summary>
        /// バイト配列を16進数文字列に変換する。
        /// </summary>
        /// <param name="bytes">バイト配列。</param>
        /// <returns>16進数文字列。</returns>
        public static string ToBinaryString(byte[] bytes)
        {
            string s = BitConverter.ToString(bytes);
            s = s.Replace("-", "");

            return s;
        }

        /// <summary>
        /// 16進数文字列をバイト配列に変換する。
        /// </summary>
        /// <param name="binaryString">16進数文字列。</param>
        /// <returns>バイト配列。</returns>
        public static byte[] GetBytesFromBinaryString(string binaryString)
        {
            int cx = binaryString.Length / 2;
            byte[] bytes = new byte[cx];
            for (int i = 0; i < cx; i++)
            {
                byte b;
                try
                {
                    b = Convert.ToByte(binaryString.Substring(i * 2, 2), 16);
                }
                catch
                {
                    b = 0x00;
                }
                bytes[i] = b;
            }

            return bytes;
        }

        static frmMessageForm _frmMessage = null;
        /// <summary>
        /// メッセージフォームを表示する。
        /// </summary>
        /// <param name="owner">オーナーウィンドウ。</param>
        /// <param name="message">メッセージ。</param>
        /// <param name="caption">タイトルバーに表示する文字列。</param>
        /// <param name="messageType">メッセージ種別。</param>
        /// <returns>メッセージフォームで選択された結果。</returns>
        public static DialogResult ShowMessage(IWin32Window owner, string message, string caption, MessageType messageType)
        {
            System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();
            DialogResult result;

            _frmMessage = new frmMessageForm(message, messageType);
            _frmMessage.Text = mainAssemName.Name;
            result = _frmMessage.ShowDialog(owner);
            _frmMessage = null;

            return result;
        }

        public static DialogResult ShowMessageActive(IWin32Window owner, string message, string caption, MessageType messageType)
        {
            System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();

            DialogResult result;
            _frmMessage = new frmMessageForm(message, messageType);
            _frmMessage.Text = mainAssemName.Name;
            SystemContext.GetInstance().ActiveForm = _frmMessage as IFormForceCancel;
         //   AppData.getInstance().ActiveForm = _frmMessage as IFormForceCancel;
            result = _frmMessage.ShowDialog(owner);
            SystemContext.GetInstance().ActiveForm = owner as IFormForceCancel;
         //   AppData.getInstance().ActiveForm = owner as IFormForceCancel;
            _frmMessage = null;
            return result;
        }

        /// <summary>
        /// メッセージボックスを表示する。
        /// キャプションは親フォームと同じ文字列を表示する。
        /// </summary>
        /// <param name="owner">親フォーム。</param>
        /// <param name="message">表示するメッセージ。</param>
        /// <param name="messageType">メッセージタイプ。</param>
        /// <returns>ユーザーが指定したダイアログの結果。</returns>
        public static DialogResult ShowMessage(this Form owner, string message, MessageType messageType)
        {
            return Utility.ShowMessageActive(owner, message, owner.Text, messageType);
        }

        public static DialogResult ShowMessage(this UserControl owner, string message, MessageType messageType)
        {
            return Utility.ShowMessageActive(owner, message, owner.Text, messageType);
        }

        public static void ForceCancelShowMessage(this Form owner)
        {
            if (_frmMessage != null)
            {
                _frmMessage.ForceCancel();
            }
        }

        public static void ForceCancelShowMessage(this UserControl owner)
        {
            if (_frmMessage != null)
            {
                _frmMessage.ForceCancel();
            }
        }

        public static DialogResult ShowFormModal(this Form thisForm, Form owner)
        {
           
            //AppData.getInstance().ActiveForm = thisForm as IFormForceCancel;
            SystemContext.GetInstance().ActiveForm=thisForm as IFormForceCancel;
            DialogResult result = thisForm.ShowDialog(owner);
            SystemContext.GetInstance().ActiveForm =  owner as IFormForceCancel;
            //AppData.getInstance().ActiveForm = owner as IFormForceCancel;

            return result;
        }

        public static void ShowUpsShutdownMessage(this Form thisForm)
        {
            frmMessageTimer frmMesTmr = new frmMessageTimer("外部から強制シャットダウン指令が発生しました。", MessageType.Error, SystemParam.GetInstance().AutoShutdownWaitSec);
            frmMesTmr.ShowDialog(thisForm);
        }

        /// <summary>
        /// コントロールの基になるウィンドウ ハンドルを所有するスレッド上で、指定したデリゲートを実行します。
        /// ラムダ式が使えます。
        /// </summary>
        /// <param name="c">コントロールのインスタンス。</param>
        /// <param name="method">コントロールのスレッド コンテキストで呼び出されるメソッドを格納しているデリゲート。</param>
        /// <returns>
        /// 呼び出されているデリゲートからの戻り値。デリゲートに戻り値がない場合は null。
        /// このメソッドの場合、MethodInvokerは戻り値がnullなので常にnullが返ります。
        /// </returns>
        public static object Invoke(this Control c, MethodInvoker method)
        {
            if (c.InvokeRequired)
            {
                return c.Invoke(method);
            }
            method.Invoke();
            return null;
        }

        /// <summary>
        /// 指定した列挙型の値をすべて取得する。
        /// </summary>
        /// <typeparam name="TEnum">値を取得する列挙型。</typeparam>
        /// <returns>取得した値のリスト。</returns>
        public static IEnumerable<TEnum> GetEnumValues<TEnum>()
        {
            Type enumType = typeof(TEnum);
            Array arr = Enum.GetValues(enumType);
            foreach (var value in arr)
            {
                yield return (TEnum)value;
            }
        }

        /// <summary>
        /// イベントを発行する。
        /// </summary>
        /// <param name="handler">イベントハンドラ。</param>
        /// <param name="sender">イベント送信主。</param>
        public static void InvokeEvent(this EventHandler handler, object sender)
        {
            if (null != handler)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// イベントを発行する。
        /// </summary>
        /// <typeparam name="TEventArgs">イベントパラメータの型。</typeparam>
        /// <param name="handler">イベントハンドラ。</param>
        /// <param name="sender">イベント送信主。</param>
        /// <param name="args">イベントパラメータ。</param>
        public static void InvokeEvent<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs args)
            where TEventArgs : EventArgs
        {
            if (null != handler)
            {
                handler(sender, args);
            }
        }

        /// <summary>
        /// イベントパラメータを生成するデリゲート。
        /// </summary>
        /// <typeparam name="TEventArgs">イベントパラメータの型。</typeparam>
        /// <returns>生成したイベントパラメータ。</returns>
        public delegate TEventArgs CreateEventArgs<TEventArgs>()
            where TEventArgs : EventArgs;

        /// <summary>
        /// イベントを発行する。
        /// </summary>
        /// <typeparam name="TEventArgs">イベントパラメータの型。</typeparam>
        /// <param name="handler">イベントハンドラ。</param>
        /// <param name="sender">イベント送信主。</param>
        /// <param name="args">イベントパラメータ。</param>
        public static void InvokeEvent<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, CreateEventArgs<TEventArgs> argCreator)
            where TEventArgs : EventArgs
        {
            if (null != handler)
            {
                var args = argCreator();
                handler(sender, args);
            }
        }

        /// <summary>
        /// 指定したデリゲートを非同期で実行する。
        /// </summary>
        /// <param name="method">実行するデリゲート。</param>
        /// <remarks>
        /// BeginInvokeを使う場合、EndInvokeを呼ばないとメモリリークが発生する可能性がある。
        /// この拡張メソッドを使用すると、EndInvokeを呼び出す手間が省けます。
        /// </remarks>
        public static void InvokeAsync(this Action method)
        {
            method.BeginInvoke((ar) =>
            {
                method.EndInvoke(ar);
            },
            null);
        }

        /// <summary>
        ///     指定した精度の数値に切り捨てします。
        /// </summary>
        /// <param name="dValue">丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">戻り値の有効桁数の精度。</param>
        /// <returns>
        /// iDigits に等しい精度の数値に切り捨てられた数値。
        /// </returns>
        public static double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Floor(dValue * dCoef) / dCoef :
                                System.Math.Ceiling(dValue * dCoef) / dCoef;
        }

        /// <summary>
        ///     指定した精度の数値に切り上げします。
        /// </summary>
        /// <param name="dValue">丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">戻り値の有効桁数の精度。</param>
        /// <returns>
        /// iDigits に等しい精度の数値に切り上げられた数値。
        /// </returns>
        public static double ToRoundUp(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Ceiling(dValue * dCoef) / dCoef :
                                System.Math.Floor(dValue * dCoef) / dCoef;
        }
    }
}
