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
    public partial class frmListSelect : Form
    {
        public frmListSelect(string stText)
        {
            InitializeComponent();
            
            this.Text = AppData.DEFAULT_APP_NAME;

            _text = stText;
        }

        public string _drivePath;
        private string _text;

        /// <summary>
        /// OKボタンを押したときにCtrlキーが押されているか
        /// </summary>
        public bool CtrlKeyPress { get; private set; }

        private void frmListSelect_Load(object sender, EventArgs e)
        {
            listDrive.Items.Clear();
            
            // 論理ドライブ名をすべて取得する
            string[] stDrives = System.IO.Directory.GetLogicalDrives();

            // 取得した論理ドライブ名をリストに追加
            foreach (string stDrive in stDrives)
            {
                listDrive.Items.Add(stDrive);
            }

            //保存先ドライブ名取得
            string stOutDrive = SystemParam.GetInstance().OutDrive;

            int iIndex = listDrive.FindStringExact(stOutDrive);
            if (iIndex > -1)
            {
                //取得したドライブ名が存在すれば選択
                listDrive.SelectedIndex = iIndex;
            }
            else
            {
                //ドライブが無ければ一番下を選択
                listDrive.SelectedIndex = listDrive.Items.Count-1;
            }
            lblText.Text = "結果データを出力します。\r\n保存するドライブを選んでください。\r\n"+_text;
        }
       
        private void btnOk_Click(object sender, EventArgs e)
        {            
            //ctrlキーが押されているか確認。押されていたら出力される画像が増える
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                this.CtrlKeyPress = true;
            }
            else
            {
                this.CtrlKeyPress = false;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmListSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            _drivePath = listDrive.SelectedItem.ToString();
        }
    }
}
