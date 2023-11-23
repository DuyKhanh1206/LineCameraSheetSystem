using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using LineCameraSheetSystem;

namespace Fujita.InspectionSystem
{
    public partial class frmMessageForm : Form
    {
        MessageFormAction _delMessageFormAction;
        MessageType _mtMessageType;

        public frmMessageForm(string message, MessageType messageType, MessageFormAction action = null)
        {
            InitializeComponent(); 
//            this.Text = AppData.DEFAULT_APP_NAME;

            // C#
            this.Text = AppData.DEFAULT_APP_NAME;

            Icon icon = this.GetIcon(messageType);
            if (icon != null)
            {
                this.InitializeIcon(this.iconPicture, icon);
            }
            this.messageLabel.Text = message;
            this.InitializeButtons(messageType);
            _mtMessageType = messageType;
            _delMessageFormAction = action;
        }

        public void ForceCancel()
        {
            if (_mtMessageType == MessageType.Question 
                || _mtMessageType == MessageType.YesNo)
                cancelButton.PerformClick();
            else
                okButton.PerformClick();
        }

        private Icon GetIcon(MessageType messageType)
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

        private void InitializeIcon(PictureBox pic, Icon icon)
        {
            int w = pic.Width;
            int h = pic.Height;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawIcon(icon, new Rectangle(0, 0, w, h));
            g.Dispose();
            pic.Image = bmp;
        }

        private void InitializeButtons(MessageType messageType)
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

        private void okButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (_delMessageFormAction != null)
            {
                _delMessageFormAction(this, new MessageFormActionEventArgs(string.Format("[{0}]クリック", button.Text), _mtMessageType));
            }
            this.DialogResult = button.DialogResult;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (_delMessageFormAction != null)
            {
                _delMessageFormAction(this, new MessageFormActionEventArgs(string.Format("[{0}]クリック", button.Text), _mtMessageType));
            }
            this.DialogResult = button.DialogResult;
            this.Close();
        }

        private void MessageForm_Shown(object sender, EventArgs e)
        {
            if (_delMessageFormAction != null)
            {
                _delMessageFormAction(this, new MessageFormActionEventArgs(string.Format("メッセージ表示:{0}", messageLabel.Text), _mtMessageType));
            }
        }
    }

    public delegate void MessageFormAction(object sender, MessageFormActionEventArgs e);
    public class MessageFormActionEventArgs : EventArgs
    {
        public MessageFormActionEventArgs(string message, MessageType e)
        {
            Message = message;
            MessageType = e;
        }

        public override string ToString()
        {
            return Message + "[" + MessageType.ToStringExt() + "]";
        }

        public string Message { get; private set; }
        public MessageType MessageType { get; private set; }
    }

    public enum MessageType
    {
        Error,
        Warning,
        Information,
        Question,
        YesNo,
    }

    public static class MessageTypeExt
    {
        public static string ToStringExt(this MessageType e)
        {
            switch (e)
            {
                case MessageType.Error: return "ｴﾗｰ";
                case MessageType.Warning: return "ﾜｰﾆﾝｸﾞ";
                case MessageType.Information: return "ｲﾝﾌｫﾒｰｼｮﾝ";
                case MessageType.Question: return "ｸｴｽﾁｮﾝ";
                case MessageType.YesNo: return "ｲｴｽﾉｰ";
            }
            return e.ToString();
        }
    }
}
