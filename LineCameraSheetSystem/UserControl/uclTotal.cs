using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class uclTotal : UserControl
    {
        public bool EnableResetButton
        {
            get { return btnReset.Enabled; }
            set { btnReset.Enabled = value; }
        }
        public uclTotal()
        {
            InitializeComponent();

            dgvZone.Rows.Add(5);
            dgvZone[0, 0].Value = "表";
            dgvZone[0, 1].Value = "裏";
            dgvZone[0, 3].Value = "表";
            dgvZone[0, 4].Value = "裏";
            
            dgvZone[0, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvZone[0, 1].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvZone[0, 2].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvZone[0, 3].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvZone[0, 4].Style.BackColor = Color.FromArgb(255, 212, 208, 200);

            dgvZone[0, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[0, 1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[0, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[0, 3].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[0, 4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; 8 >= i; i++)
            {
                dgvZone[i, 2].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            }
            dgvZone[1, 2].Value = "Z9";
            dgvZone[2, 2].Value = "Z10";
            dgvZone[3, 2].Value = "Z11";
            dgvZone[4, 2].Value = "Z12";
            dgvZone[5, 2].Value = "Z13";
            dgvZone[6, 2].Value = "Z14";
            dgvZone[7, 2].Value = "Z15";
            dgvZone[8, 2].Value = "Z16";

            dgvZone[1, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[2, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[3, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[4, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[5, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[6, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[7, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvZone[8, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            
            dgvItem.Rows.Add(2);
            dgvItem[0, 0].Value = "表";
            dgvItem[0, 1].Value = "裏";
            dgvItem[0, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvItem[0, 1].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dgvItem[0, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvItem[0, 1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int inspID = 0; inspID < Enum.GetNames(typeof(AppData.InspID)).Length; inspID++)
            {
                string ttl = Enum.GetNames(typeof(AppData.InspID))[inspID];
                dgvItem.Columns[inspID + 1].HeaderText = ttl.Replace("暗", "");
            }

            dgvCamera.Rows.Add(1);
            dgvCameraTotal.Rows.Add(1);
            //   dgvCamera[0, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);

            CameraNgCount(new int[] { 0, 0, 0, 0 });
            ItemsNgCount(new int[,] { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } });
            ZoneNgCount(new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });
        }
        MainForm _mainForm { get; set; }
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;
        }
        private void uclTotal_Load(object sender, EventArgs e)
        {

        }

        //カメラ別NG個数表示
        private void CameraNgCount(int[] iCount)
        {
            for (int i = 0; iCount.Length > i; i++)
            {
                dgvCamera[i, 0].Value = iCount[i];
            }
            dgvCameraTotal[0, 0].Value = (int)dgvCamera[0, 0].Value;
            dgvCameraTotal[1, 0].Value = (int)dgvCamera[1, 0].Value;
            //dgvCameraTotal[0, 0].Value = (int)dgvCamera[0, 0].Value + (int)dgvCamera[1, 0].Value;
            //dgvCameraTotal[1, 0].Value = (int)dgvCamera[2, 0].Value + (int)dgvCamera[3, 0].Value;
        }

        //項目別NG個数表示
        private void ItemsNgCount(int[,] iCount)
        {
            for (int i = 0; iCount.GetLength(0) > i; i++)
            {
                for (int j = 0; iCount.GetLength(1) > j; j++)
                {
                    dgvItem[j+1, i].Value = iCount[i, j];
                }
            }
        }

        //ゾーン別NG個数表意
        private void ZoneNgCount(int[,] iCount)
        {
            
            
            for (int j = 0; iCount.GetLength(1) > j; j++)
            {
                if (j < 8)
                {
                    dgvZone[j+1, 0].Value = iCount[0, j];
                }
                else
                {
                    dgvZone[j+1-8, 3].Value = iCount[0, j];
                }

            }

            for (int j = 0; iCount.GetLength(1) > j; j++)
            {
                if (j < 8)
                {
                    dgvZone[j+1, 1].Value = iCount[1, j];
                }
                else
                {
                    dgvZone[j+1-8, 4].Value = iCount[1, j];
                }

            }
            
        }

        //各NGカウントに入れる
        public void SetNgCount(int[] camera, int[,] items, int[,] zone)
        {
            CameraNgCount(camera);
            ItemsNgCount(items);
            ZoneNgCount(zone);

        }

        public void ClearCount()
        {
            CameraNgCount(new int[] { 0, 0, 0, 0 });
            ItemsNgCount(new int[,] { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } });
            ZoneNgCount(new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == Utility.ShowMessage(this, "累計カウントをリセットしてよいですか？", MessageType.YesNo))
                _mainForm.ResetRuikei();
        }
    }

   



}
