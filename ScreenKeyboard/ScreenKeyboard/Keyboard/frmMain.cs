using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fujita.Misc;
#if FUJITA_INSPECTION_SYSTEM
using Fujita.FormMisc;
#endif

namespace Fujita.ScreenKeyboard
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            _ClientSize = new Size();
            _ClientSize.Height = this.Size.Height;
            _ClientSize.Width = this.Size.Width;

            SetListCtrl();
            fFontSize = btnOk.Font.Size;
            fTexFontSize = txtInputStr.Font.Size;

        }

        private Size _ClientSize;

	    // キー入力制限
        clsTextboxKeyPressMask _keyMask = null;
      
        private void frmMain_Load(object sender, EventArgs e)
        {
            //フォームの位置のロード
            SetWindow();    
            
            uclKeyJpnAiu1.SetMainForm(this);
            uclKeyEngQwe1.SetMainForm(this);

            uclKeyJpnAiu1.kyepush += new uclKeyJpnAiu.KeyPushEventHandler(OnRecieveKeyPush);
            uclKeyEngQwe1.kyepush += new uclKeyEngQwe.KeyPushEventHandler(OnRecieveKeyPush);

            SetStart();

            SetKeyBoard();

            SetInputString();

            txtInputStr.Select(txtInputStr.Text.Length, 0);

            sizeChanger = new DAndDSizeChanger(this.label1, this, DAndDArea.All, 8);

            
            //_keyMask = new clsTextboxKeyPressMask(new KeyPressMask_InvalidFileCharUnderBar());
          
            SystemSetting sys = new SystemSetting();
            List<char> listch = sys.LoadNgKey();
            KeyPressMask_List keypress = new KeyPressMask_List(listch);
            _keyMask = new clsTextboxKeyPressMask(keypress);
            _keyMask.SetTextBox(txtInputStr);

  

        }
        DAndDSizeChanger sizeChanger;

        private string _strInputText;
        private string _strOutputText;

        public string strInputText
        {
            set { this._strInputText = value; }
            get { return this._strInputText; }
        }

        public string strOutputText
        {
            set { this._strOutputText = value; }
            get { return this._strOutputText; }
        }

        private void SetInputString()
        {
            txtInputStr.Text = _strInputText;
        }

        //チェックイベント--------
        public delegate void CheckBoxEventHandler(object sender, CheckEventArgs e);
        public event CheckBoxEventHandler checkevent;
        protected virtual void OnCheckBox(CheckEventArgs e)
        {
            if (checkevent != null)
                checkevent(this, e);
        }

        public class CheckEventArgs : EventArgs
        {
            public CheckBox chkbox;
           

        }
        //------------------------

        private void SetStart()
        {
            SystemSetting setting = new SystemSetting();

            if (setting.LoadStatLanguage())
            {
                SetStatus(true);
            }
            else
            {
                SetStatus(false);
            }
        }

        private void SetStatus(bool bJpn)
        {
            Status status = Status.GetInstance();
            status.bJapan = bJpn;
            status.bSmall = !bJpn;
            status.bDaku = !bJpn;
            status.bHandaku = !bJpn;
        }

        private void SetKeyBoard()
        {
            if (Status.GetInstance().bJapan)
            {
                uclKeyEngQwe1.Hide(); 

                tableLayoutPanel1.SetColumnSpan(uclKeyEngQwe1,1);
                tableLayoutPanel1.SetRowSpan(uclKeyEngQwe1, 1);
                tableLayoutPanel1.Controls.Add(uclKeyEngQwe1, 10, 1);
                tableLayoutPanel1.Controls.Add(uclKeyJpnAiu1, 1, 1);
                tableLayoutPanel1.SetColumnSpan(uclKeyJpnAiu1, 21);
                tableLayoutPanel1.SetRowSpan(uclKeyJpnAiu1, 8);

                uclKeyJpnAiu1.Show();
                

                chkDaku.Show();
                chkHandaku.Show();
                chkSmall.Show();
                chkSmall.Checked = false;
                btnLang.Text = "英数字";
            }
            else
            {
                uclKeyJpnAiu1.Hide();

                tableLayoutPanel1.SetColumnSpan(uclKeyJpnAiu1, 1);
                tableLayoutPanel1.SetRowSpan(uclKeyJpnAiu1, 1);
                tableLayoutPanel1.Controls.Add(uclKeyJpnAiu1, 10, 2);
                tableLayoutPanel1.Controls.Add(uclKeyEngQwe1, 1, 1);
                tableLayoutPanel1.SetColumnSpan(uclKeyEngQwe1, 21);
                tableLayoutPanel1.SetRowSpan(uclKeyEngQwe1, 8);

                uclKeyEngQwe1.Show();



                chkDaku.Hide();
                chkHandaku.Hide();
                chkSmall.Show();
                chkSmall.Checked = true;
                btnLang.Text = "ひらがな";
            }

        }

        private void btnLang_Click(object sender, EventArgs e)
        {
            if (Status.GetInstance().bJapan)
            {
                Status.GetInstance().bJapan = false;

                SetKeyBoard();
            }
            else
            {
                Status.GetInstance().bJapan = true; ;

                SetKeyBoard();
            }
        }

        private void OnRecieveKeyPush(object sender, String strKeys)
        {
            txtInputStr.Focus();

            Status status = Status.GetInstance();
            if (Status.GetInstance().bJapan)
            {
                txtInputStr.ImeMode = ImeMode.Hiragana;

                SystemSetting sysset =new SystemSetting();
                if (sysset.KeyReturn())
                {
                    KeyReturn(strKeys);
                }
            }
            else
            {
                if (txtInputStr.ImeMode == ImeMode.Hiragana || txtInputStr.ImeMode == ImeMode.Alpha)
                {
                    txtInputStr.ImeMode = ImeMode.Alpha;
                }
                else
                {
                    txtInputStr.ImeMode = ImeMode.Off;
                }
               
               //txtInputStr.ImeMode = ImeMode.Hiragana;
                
            }

            SendKeys.Flush();
            SendKeys.Send(strKeys);
        }

        private void KeyReturn(string strTag)
        {
            Status status = Status.GetInstance();
            if (!status.bJapan)
            {
                return;
            }

            
            if (status.bSmall)
            {
                
                int iCount = strTag.IndexOf('x');
                if (iCount > -1)
                {
                    status.bSmall = false;
                    chkSmall.Checked = false;
                }
            }
            else if (status.bDaku)
            {
                int iCountD = strTag.IndexOf('d');
                int iCountG = strTag.IndexOf('g');
                int iCountZ = strTag.IndexOf('z');
                int iCountB = strTag.IndexOf('b');

                if (iCountD > -1 || iCountB > -1 || iCountG > -1 || iCountZ > -1)
                {
                    status.bDaku = false;
                    chkDaku.Checked = false;
                }
            }
            else
            {
                int iCount = strTag.IndexOf('p');
                if (iCount > -1)
                {
                    status.bHandaku = false;
                    chkHandaku.Checked = false;
                }
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckEventArgs ce = new CheckEventArgs();
            

            SwitchChecked((CheckBox)sender);

            if (!chkSmall.Checked && !chkDaku.Checked && !chkHandaku.Checked)
            {
                OnCheckBox(ce);
            }
            else
            {
                ce.chkbox = (CheckBox)sender;
                OnCheckBox(ce);   
            }

        }

        private void SwitchChecked(CheckBox chk)
        {
            if (!chk.Checked)
            {
                return;
            }

            if (chkSmall == chk)
            {
                chkDaku.Checked = false;
                chkHandaku.Checked = false;
            }
            else if (chkDaku == chk)
            {
                chkSmall.Checked = false;
                chkHandaku.Checked = false;
            }
            else
            {
                chkSmall.Checked = false;
                chkDaku.Checked = false;
            }

        }

        private void btnBackSp_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{bs}");
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtInputStr.Text = "";
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtInputStr.ImeMode == ImeMode.Hiragana)
            {
                SendKeys.Send(" ");
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
           // SendKeys.Send("{LEFT}");

            if (chkShift.Checked)
            {
                SendKeys.Send("+{LEFT}");
            }
            else
            {
                SendKeys.Send("{LEFT}");
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{RIGHT}");

            if (chkShift.Checked)
            {
                SendKeys.Send("+{RIGHT}");
            }
            else
            {
                SendKeys.Send("{RIGHT}");
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{ENTER}");

            txtInputStr.ImeMode = ImeMode.Off;
        }

        private void btnLeftS_Click(object sender, EventArgs e)
        {
            SendKeys.Send("+{LEFT}");
        }

        private void btnRghtS_Click(object sender, EventArgs e)
        {
            SendKeys.Send("+{RIGHT}");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            SendKeys.Send("{ENTER}");
            SendKeys.Flush();

            _strOutputText = txtInputStr.Text;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _strOutputText = _strInputText;

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ウインドウ位置のセーブ
            SaveWindow();
        }

        private void SetWindow()
        {
            //ウィンドウの位置・サイズを復元
            /*
            Rectangle recRe = Properties.Settings.Default.Bounds;
            Rectangle recDe = new Rectangle(0, 0, 0, 0);
            if (recRe == recDe)
            {
                //フォームを画面の中央に配置
                this.SetBounds((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                                   (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2, this.Width, this.Height);
            }
            else
            {
                Bounds = Properties.Settings.Default.Bounds;
            }

            WindowState = Properties.Settings.Default.WindowState;
             */
#if FUJITA_INSPECTION_SYSTEM
            Fujita.FormMisc.clsControlSerialize.Restore(this);
#endif

        }

        private void SaveWindow()
        {
            /*
            // ウィンドウの位置・サイズを保存  
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Bounds = Bounds;
            }
            else
            {
                Properties.Settings.Default.Bounds = RestoreBounds;
            }

            //  Properties.Settings.Default.WindowState = WindowState;
            Properties.Settings.Default.Save();

            // Properties.Settings.Default.Bounds = RestoreBounds;
            // Properties.Settings.Default.Save();
             */
#if FUJITA_INSPECTION_SYSTEM
            Fujita.FormMisc.clsControlSerialize.Store(this, FormMisc.clsControlSerialize.ESerializeType.PositionSize, true);
#endif

        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            double dHeight = (double)this.Size.Height / (double)_ClientSize.Height;
            double dWidth  = (double)this.Size.Width / (double)_ClientSize.Width;

            
            if (dHeight >= dWidth)
            {
                uclKeyEngQwe1.ChengeFontSize((float)dWidth);
                uclKeyJpnAiu1.ChengeFontSize((float)dWidth);
                this.ChangeFontSize(dWidth);
            }
            else
            {
                uclKeyEngQwe1.ChengeFontSize((float)dHeight);
                uclKeyJpnAiu1.ChengeFontSize((float)dHeight);
                this.ChangeFontSize(dHeight);
            }

           
        }

        private float fFontSize;
        private float fTexFontSize;
        private void ChangeFontSize(double dMagnify)
        {
            string strFontName = btnOk.Font.Name;

            for (int i = 0; _listCtrl.Count > i; i++)
            {
                _listCtrl[i].Font = new Font(strFontName, (float)(fFontSize * dMagnify));

            }

            float fH = tableLayoutPanel1.RowStyles[0].Height;
            int iH = tableLayoutPanel1.Height;
            float fHeight = iH * (fH / 100);
            bool b = true;
            int j = 1;
            Size size;
            txtInputStr.Font = new Font(txtInputStr.Font.Name, 1);

            while (b)
            {  
                //size = TextRenderer.MeasureText("A", txtInputStr.Font);
                Font font = new Font(txtInputStr.Font.Name,  j);
                size = TextRenderer.MeasureText("A",font);
                
                if (fHeight - 15 >= size.Height)
                {
                 //   txtInputStr.Font = new Font(txtInputStr.Font.Name, (float)(1 * j));
                   
                    j++;

                    continue;
                }
                else
                {
                    txtInputStr.Font = new Font(txtInputStr.Font.Name, (float)(1 * j));
                    b = false;
                }
            }
           
        }

        private List<Control> _listCtrl;

        private void SetListCtrl()
        {
            _listCtrl = new List<Control>();

            _listCtrl.Add(btnOk);
            _listCtrl.Add(btnCancel);
            _listCtrl.Add(btnBackSp);
            _listCtrl.Add(btnChange);
            _listCtrl.Add(btnDelete);
            _listCtrl.Add(btnEnter);
            _listCtrl.Add(btnLang);
            _listCtrl.Add(btnLeft);
            _listCtrl.Add(btnRight);
            _listCtrl.Add(chkDaku);
            _listCtrl.Add(chkHandaku);
            _listCtrl.Add(chkShift);
            _listCtrl.Add(chkSmall);
            _listCtrl.Add(btnLeftS);
            _listCtrl.Add(btnRghtS);
            _listCtrl.Add(btnEsc);
        }

        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShift.Checked)
            {
                btnLeft.Text = "s←";
                btnRight.Text = "s→";
            }
            else
            {
                btnLeft.Text = "←";
                btnRight.Text = "→";
            }
        }

	    private void btnOk_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void btnOk_MouseUp(object sender, MouseEventArgs e)
        {
            txtInputStr.Focus();
        }

       
    }

    public class Status
    {
        private static Status _singleton = new Status();

        public static Status GetInstance()
        {
            return _singleton;
        }
        
        public Status()
        {
            
        }

        //文字の大小
        public bool bSmall { get;  set; }
        //英字かひらがな
        public bool bJapan { get;  set; }
        //濁点
        public bool bDaku { get;  set; }
        //半濁点
        public bool bHandaku { get;  set; }

        public void SetCheck(string fxval)
        {
            if (clsFixedValue.STR_CHK_SMALL == fxval)
            {
                bSmall = true;
                bDaku = false;
                bHandaku = false;
            }
            else if (clsFixedValue.STR_CHK_DAKU == fxval)
            {
                bSmall = false;
                bDaku = true;
                bHandaku = false;
            }
            else
            {
                bSmall = false;
                bDaku = false;
                bHandaku = true;
            }
                

        }
    }

    public class clsFixedValue
    {
        public const string STR_CHK_SMALL = "small";
        public const string STR_CHK_DAKU = "daku";
        public const string STR_CHK_HANDAKU = "handaku";

    }
}
