using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class frmPassword : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public string DeveloperPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public frmPassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void num_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            string tag = control.Tag as string;
            if (tag == null)
                return;

            int n;
            if (int.TryParse(tag, out n))
            {
                if (n == 10)
                {
                    this.passwordBox.Text = string.Empty;
                }
                else
                {
                    this.passwordBox.Text = this.passwordBox.Text + n.ToString();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            string inputPassword = this.passwordBox.Text;
            string sHash = Fujita.Misc.MiscFunc.GetMD5Hash(inputPassword);

            if (sHash == DeveloperPassword)
            {
            }
            else
            {                // 正しくないパスワードが入力された
                Utility.ShowMessage(this, "パスワードが正しくありません。", this.Text, MessageType.Error);
                this.passwordBox.Text = string.Empty;
                this.passwordBox.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
