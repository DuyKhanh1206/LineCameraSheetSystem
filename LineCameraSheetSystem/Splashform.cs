using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.InspectionSystem
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
        }


        //Splashフォーム
        private static SplashForm _form = null;
        //メインフォーム
        private static Form _mainForm = null;
        //Splashを表示するスレッド
        private static System.Threading.Thread _thread = null;
        //lock用のオブジェクト
        private static readonly object syncObject = new object();
        //Splashが表示されるまで待機するための待機ハンドル
        private static System.Threading.ManualResetEvent splashShownEvent = null;

        /// <summary>
        /// Splashフォーム
        /// </summary>
        public static SplashForm Form
        {
            get { return _form; }
        }

        /// <summary>
        /// Splashフォームを表示する                                     Hiển thị dạng Splash   
        /// </summary>
        /// <param name="mainForm">メインフォーム</param>            form chính

        public static void ShowSplash(Form mainForm)
        {
            lock (syncObject)
            {
                if (_form != null || _thread != null)
                {
                    return;
                }

                _mainForm = mainForm;
                //メインフォームのActivatedイベントでSplashフォームを消す
                if (_mainForm != null)
                {
                    _mainForm.Activated += new EventHandler(_mainForm_Activated);
                }

                //待機ハンドルの作成
                splashShownEvent = new System.Threading.ManualResetEvent(false);

                //スレッドの作成
                _thread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(StartThread));
                _thread.Name = "SplashForm";
                _thread.IsBackground = true;
                _thread.SetApartmentState(System.Threading.ApartmentState.STA);
                //.NET Framework 1.1以前では、以下のようにする
                //_thread.ApartmentState = System.Threading.ApartmentState.STA;
                //スレッドの開始
                _thread.Start();
            }
        }

        /// <summary>
        /// Splashフォームを表示する                             Hiển thị dạng Splash ????
        /// </summary>
        public static void ShowSplash()
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("Splash ShowSplash()"); // dọc dữ liệu với string truyền vào là hàm Splash ShowSplash()
            ShowSplash(null);
        }

        public static void VisibleSplashwindow(bool visible)
        {
            if (_form == null)
                return;
            Action act = new Action(() =>
            {
                _form.Visible = visible;
            });
            if (_form.InvokeRequired)
                _form.Invoke(act);
            else
                act.Invoke();
        }

        public static void ProgressSplash(int iIndicater, string sMessage)  // đưa ra màn hình tiến trình cài đặt, lượng công việc được cài đặt hoàn thành kiểu thanh loading và kèm theo %
        {
            if( _form == null )
                return ;

            Action act = new Action(() =>
                {
                    LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("Splash ProgressSplash() [{0}:{1}]", iIndicater.ToString(), sMessage));

                    if (iIndicater >= _form.pgbProgress.Minimum && iIndicater <= _form.pgbProgress.Maximum)
                    {
                        _form.pgbProgress.Value = iIndicater;
                    }
                    _form.lblProgressContent.Text = sMessage;
                });

            if (_form.InvokeRequired)
            {
                _form.Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        /// <summary>
        /// Splashフォームを消す
        /// </summary>
        public static void CloseSplash()
        {
            lock (syncObject)
            {
                if (_thread == null)
                {
                    return;
                }

                if (_mainForm != null)
                {
                    _mainForm.Activated -= new EventHandler(_mainForm_Activated);
                }

                //Splashが表示されるまで待機する
                if (splashShownEvent != null)
                {
                    splashShownEvent.WaitOne();
                    splashShownEvent.Close();
                    splashShownEvent = null;
                }

                //Splashフォームを閉じる
                //Invokeが必要か調べる
                if (_form != null)
                {
                    if (_form.InvokeRequired)
                    {
                        _form.Invoke(new MethodInvoker(CloseSplashForm));
                    }
                    else
                    {
                        LogingDllWrap.LogingDll.Loging_SetLogString("Splash CloseSplash()");
                        CloseSplashForm();
                    }
                }

                //メインフォームをアクティブにする
                if (_mainForm != null)
                {
                    if (_mainForm.InvokeRequired)
                    {
                        _mainForm.Invoke(new MethodInvoker(ActivateMainForm));
                    }
                    else
                    {
                        ActivateMainForm();
                    }
                }

                _form = null;
                _thread = null;
                _mainForm = null;
            }
        }

        //スレッドで開始するメソッド
        private static void StartThread()
        {
            //Splashフォームを作成
            _form = new SplashForm();
            //Splashフォームをクリックして閉じられるようにする
            _form.Click += new EventHandler(_form_Click);
            //Splashが表示されるまでCloseSplashメソッドをブロックする
            _form.Activated += new EventHandler(_form_Activated);
            //Splashフォームを表示する
            Application.Run(_form);
        }

        //SplashのCloseメソッドを呼び出す
        private static void CloseSplashForm()
        {
            if (_form != null && !_form.IsDisposed)
            {
                _form.Close();
                _form = null;
            }
        }

        //メインフォームのActivateメソッドを呼び出す
        private static void ActivateMainForm()
        {
            if (!_mainForm.IsDisposed)
            {
                _mainForm.Activate();
            }
        }

        //Splashフォームがクリックされた時
        private static void _form_Click(object sender, EventArgs e)
        {
            //Splashフォームを閉じる
//            CloseSplash();
        }

        //メインフォームがアクティブになった時
        private static void _mainForm_Activated(object sender, EventArgs e)
        {
            //Splashフォームを閉じる
            CloseSplash();
        }

        //Splashフォームが表示された時
        private static void _form_Activated(object sender, EventArgs e)
        {
            _form.Activated -= new EventHandler(_form_Activated);
            //CloseSplashメソッドの待機を解除
            if (splashShownEvent != null)
            {
                splashShownEvent.Set();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SplashForm
            // 
            this.ClientSize = new System.Drawing.Size(988, 164);
            this.Name = "SplashForm";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.ResumeLayout(false);

        }

        private void SplashForm_Load(object sender, EventArgs e)
        {

        }
    }
}
