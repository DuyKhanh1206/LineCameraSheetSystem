using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class frmRecipeExternalOutput : Form
    {
        public bool ExternalEnable
        {
            get { return chkExtEnable.Checked; }
            set { chkExtEnable.Checked = value; }
        }
        public int ExternalDelay1
        {
            get { return (int)spinExtDelay1.Value; }
            set { spinExtDelay1.Value = value; }
        }
        public int ExternalDelay2
        {
            get { return (int)spinExtDelay2.Value; }
            set { spinExtDelay2.Value = value; }
        }
        public int ExternalDelay3 //V1057 NG表裏修正 yuasa 20190118：外部３追加
        {
            get { return (int)spinExtDelay3.Value; }
            set { spinExtDelay3.Value = value; }
        }
        public int ExternalDelay4 //V1057 NG表裏修正 yuasa 20190118：外部４追加
        {
            get { return (int)spinExtDelay4.Value; }
            set { spinExtDelay4.Value = value; }
        }
        public int ExternalReset1
        {
            get { return (int)spinExtTimer1.Value; }
            set { spinExtTimer1.Value = value; }
        }
        public int ExternalReset2
        {
            get { return (int)spinExtTimer2.Value; }
            set { spinExtTimer2.Value = value; }
        }
        public int ExternalReset3 //V1057 NG表裏修正 yuasa 20190118：外部３追加
        {
            get { return (int)spinExtTimer3.Value; }
            set { spinExtTimer3.Value = value; }
        }
        public int ExternalReset4 //V1057 NG表裏修正 yuasa 20190118：外部４追加
        {
            get { return (int)spinExtTimer4.Value; }
            set { spinExtTimer4.Value = value; }
        }
        public int ExternalShot1
        {
            get { return (int)spinExtShot1.Value; }
            set { spinExtShot1.Value = value; }
        }
        public int ExternalShot2
        {
            get { return (int)spinExtShot2.Value; }
            set { spinExtShot2.Value = value; }
        }
        public int ExternalShot3 //V1057 NG表裏修正 yuasa 20190118：外部３追加
        {
            get { return (int)spinExtShot3.Value; }
            set { spinExtShot3.Value = value; }
        }
        public int ExternalShot4 //V1057 NG表裏修正 yuasa 20190118：外部４追加
        {
            get { return (int)spinExtShot4.Value; }
            set { spinExtShot4.Value = value; }
        }

        public frmRecipeExternalOutput()
        {
            InitializeComponent();

            if (!SystemParam.GetInstance().ExternalFrontReverseDivide) //V1057 NG表裏修正 yuasa 20190118：iniファイルに応じてグレーアウト
            {
                lblExtTitle1.Text = "外部１(品種)";
                lblExtTitle2.Text = "外部２(品種)";
                lblExtTitle3.Visible = false;
                lblExtTitle4.Visible = false;

                spinExtTimer3.Visible = false;
                spinExtTimer4.Visible = false;

                spinExtDelay3.Visible = false;
                spinExtDelay4.Visible = false;

                spinExtShot3.Visible = false;
                spinExtShot4.Visible = false;
            }
        }

        private void frmRecipeExternalOutput_Load(object sender, EventArgs e)
        {
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
