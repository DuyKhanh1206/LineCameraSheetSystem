using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Windows.Forms;

namespace Fujita.Misc
{
    interface IThreadSafeFinish
    {
        void ThreadSafeEnd();
    }

    public class ThreadEndedEventArgs : EventArgs
    {

    }
    public delegate void ThreadEndedEventHandler( object sender, ThreadEndedEventArgs e );

    /// <summary>
    /// スレッドを安全に終了するクラス
    /// 使用するには、スレッドクラスにIThreadSafeFinishインターフェイスを継承させ
    /// そのメソッドの中で、スレッドの終了を実行する
    /// </summary>
    class clsThreadSafeFinish
    {
        public event ThreadEndedEventHandler OnThreadEnded;

        IThreadSafeFinish _iThread;
        public clsThreadSafeFinish(IThreadSafeFinish iThread)
        {
            _iThread = iThread;
        }

        Thread _tThread = null;
        public void SafeFinish( bool bWaitFinish = false)
        {
            _tThread = new Thread(SafeFinishThread);
            _tThread.Name = "ｾｰﾌｽﾚｯﾄﾞ終了";
            _tThread.Start();
            if (bWaitFinish)
            {
                waitThreadEnd();
            }
        }

        private void SafeFinishThread()
        {
            _iThread.ThreadSafeEnd();

            if (OnThreadEnded != null)
            {
                OnThreadEnded(this, new ThreadEndedEventArgs());
            }
        }

        private void waitThreadEnd()
        {
            do
            {
                Application.DoEvents();
            } while (_tThread.IsAlive);
        }
    }
}
