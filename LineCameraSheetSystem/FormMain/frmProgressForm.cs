using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Fujita.InspectionSystem
{
    public partial class frmProgressForm : System.Windows.Forms.Form, IFormForceCancel
    {
        private Action _action;
        private Action _abort;
        private Thread _thread = null;

        private Thread _thKeikaTime = null;
        private bool _stopKeikaTime = false;

        public bool HasAborted { get; private set; }

        public Color ColorBackground
        {
            get
            {
                return _colorBack;
            }
            set
            {
                _colorBack = value;
                this.BackColor = _colorBack;
                descriptionLabel.BackColor = _colorBack;
                lblKeikaTime.BackColor = _colorBack;
            }
        }
        private Color _colorBack;
        private Color _colorBackDefault;

        public string Description
        {
            get { return this.descriptionLabel.Text; }
            set { this.descriptionLabel.Text = value; }
        }

        public frmProgressForm(Action action, Action abort)
        {
            InitializeComponent();
            this.Text = Application.ProductName;
            _action = action;
            _abort = abort;
            this.HasAborted = false;
            if (_abort == null)
                this.abortButton.Visible = false;
            _colorBackDefault = this.BackColor;
            ColorBackground = this.BackColor;
        }

        public bool VisibleKeikaTime
        {
            get { return lblKeikaTime.Visible; }
            set { lblKeikaTime.Visible = value; }
        }

        public void ForceCancel()
        {
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.HasAborted = true;
            if (_abort != null)
            {
                _abort();
            }
        }

        private void ProgressForm_Shown(object sender, EventArgs e)
        {
            _thread = new Thread(new ThreadStart(this.DoWork));
            _thread.Name = "ProgressForm";
            _thread.IsBackground = true;
            _thread.Start();

            if (VisibleKeikaTime==true)
            {
                _stopKeikaTime = false;
                _thKeikaTime = new Thread(new ThreadStart(this.DoKeikaTime));
                _thKeikaTime.Name = "ProgressForm.KeikaTime";
                _thKeikaTime.IsBackground = true;
                _thKeikaTime.Start();
            }
        }

        private void DoKeikaTime()
        {
            int iLoopCounter = 0;
            int iCnt = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while(!_stopKeikaTime)
            {
                Action act = new Action(() =>
                {
                    lblKeikaTime.Text = "経過時間： " + sw.Elapsed.TotalSeconds.ToString("F0") + " 秒";
                    if (iCnt == 0)
                    {
                        this.Activate();
                        iCnt = 10;
                    }
                    iCnt--;

                    Color col = ((iLoopCounter % 5) == 0) ? _colorBackDefault : _colorBack;
                    this.BackColor = col;
                    descriptionLabel.BackColor = col;
                    lblKeikaTime.BackColor = col;
                    iLoopCounter++;
                });
                if (InvokeRequired)
                    Invoke(act);
                else
                    act.Invoke();
                System.Threading.Thread.Sleep(500);
            }
        }

        private void DoWork()
        {
            if (_action != null)
            {
                _action();
            }

            if (_thKeikaTime!=null)
            {
                _stopKeikaTime = true;
                _thKeikaTime.Join();
                _thKeikaTime = null;
            }

            this.Invoke(new MethodInvoker(() =>
            {
                this.Close();
            }));
        }
    }
}
