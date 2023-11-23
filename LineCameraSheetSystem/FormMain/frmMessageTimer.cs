using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LineCameraSheetSystem;

namespace Fujita.InspectionSystem
{
    public partial class frmMessageTimer : Form
    {
        MessageType _mtMessageType;
        Button _autoSelectButton;
        int _iTimeCnt;

        public frmMessageTimer( string sMessage, MessageType mtType, int iTimeCnt, int iSelectButton = 1)
        {
            InitializeComponent();

            Text = AppData.DEFAULT_APP_NAME;

            Icon icon = getIcon(mtType);
            if (icon != null)
            {
                initializeIcon(this.iconPicture, icon);
            }

            messageLabel.Text = sMessage;
            initializeButtons(mtType);
            choiceAutoButton(iSelectButton, mtType);

            _iTimeCnt = iTimeCnt - 1;
            _mtMessageType = mtType;
        }

        public void ForceCancel()
        {
            if (_mtMessageType == MessageType.Question
                || _mtMessageType == MessageType.YesNo)
            {
                cancelButton.PerformClick();
            }
            else
            {
                okButton.PerformClick();
            }
        }

        private void choiceAutoButton(int iSelectButton, MessageType mtType)
        {
            if (mtType == MessageType.YesNo || mtType == MessageType.Question)
            {
                switch (iSelectButton)
                {
                    case 0:
                        _autoSelectButton = cancelButton;
                        break;
                    default:
                        _autoSelectButton = ok2Button;
                        break;
                }
            }
            else
            {
                _autoSelectButton = okButton;
            }
        }

        private Icon getIcon(MessageType messageType)
        {
            Icon icon = null;
            switch (messageType)
            {
                case MessageType.Error:
                    icon = SystemIcons.Error;
                    break;

                case MessageType.Warning:
                    icon = SystemIcons.Warning;
                    break;

                case MessageType.Information:
                    icon = SystemIcons.Information;
                    break;

                case MessageType.Question:
                case MessageType.YesNo:
                    icon = SystemIcons.Question;
                    break;

                default:
                    icon = null;
                    break;

            }
            return icon;
        }

        private void initializeIcon(PictureBox pic, Icon icon)
        {
            int w = pic.Width;
            int h = pic.Height;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawIcon(icon, new Rectangle(0, 0, w, h));
            g.Dispose();
            pic.Image = bmp;
        }

        private void initializeButtons(MessageType messageType)
        {
            this.okButton.Visible = false;
            this.ok2Button.Visible = false;
            this.cancelButton.Visible = false;

            switch (messageType)
            {
                case MessageType.Question:
                    this.ok2Button.Visible = true;
                    this.cancelButton.Visible = true;
                    break;

                case MessageType.YesNo:
                    this.ok2Button.Visible = true;
                    this.ok2Button.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    this.cancelButton.Visible = true;
                    this.ok2Button.Text = "はい";
                    this.cancelButton.Text = "いいえ";
                    break;

                default:
                    this.okButton.Visible = true;
                    return;
            }
        }

        private void timerMessage()
        {
            if (_mtMessageType == MessageType.Question || _mtMessageType == MessageType.YesNo)
            {
                lblTimerMessage.Text = string.Format("{0}秒後に自動的に[{1}]ﾎﾞﾀﾝが選択されます", _iTimeCnt, _autoSelectButton.Text);
            }
            else
            {
                lblTimerMessage.Text = string.Format("{0}秒後に自動的に閉じられます", _iTimeCnt);
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            _iTimeCnt--;
            timerMessage();
            if( _iTimeCnt == 0 )
            {
                _autoSelectButton.PerformClick();
            }
        }

        private void frmMessageTimer_Shown(object sender, EventArgs e)
        {
            timerMessage();
            timerTime.Enabled = true;
        }
    }
}
