using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Adjustment;
using Fujita.Misc;
using LineCameraSheetSystem.Adjust;
using HalconCamera;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class uclSystem : UserControl ,IShortcutClient
    {
        private List<TextBox> txtConnectImageTime = new List<TextBox>();
        private List<TextBox> txtOnGrabbedTime = new List<TextBox>();
        private List<TextBox> txtGetImageTotalTime = new List<TextBox>();

        private List<TextBox> txtCaptureCount = new List<TextBox>();
        private List<TextBox> txtCaptureFailCount = new List<TextBox>();

        private List<TextBox> txtRealGrabCount = new List<TextBox>();
        private List<TextBox> txtQueueGrabCount = new List<TextBox>();
        private List<TextBox> txtQueueCount = new List<TextBox>();


        public uclSystem()
        {
            InitializeComponent();

            shortcutKeyHelper1.SetShortcutKeys(btnSetting, Keys.S);
            shortcutKeyHelper1.SetShortcutKeys(btnAjustment, Keys.A);

            txtConnectImageTime.Add(txtConnectImage1);
            txtConnectImageTime.Add(txtConnectImage2);
            txtOnGrabbedTime.Add(txtOnGrabbed1);
            txtOnGrabbedTime.Add(txtOnGrabbed2);
            txtGetImageTotalTime.Add(txtGetImageThread1);
            txtGetImageTotalTime.Add(txtGetImageThread2);

            txtCaptureCount.Add(txtCaptureCnt1);
            txtCaptureCount.Add(txtCaptureCnt2);
            txtCaptureFailCount.Add(txtCaptureFailCount1);
            txtCaptureFailCount.Add(txtCaptureFailCount2);

            txtRealGrabCount.Add(textBox1);
            txtRealGrabCount.Add(textBox2);
            txtQueueGrabCount.Add(textBox5);
            txtQueueGrabCount.Add(textBox6);
            txtQueueCount.Add(textBox9);
            txtQueueCount.Add(textBox10);
        }

        //MainFormのインスタンス
        MainForm _mainForm { get; set; }
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            SystemSettingForm syssetform = new SystemSettingForm(_mainForm);
            syssetform.ShowDialog();
        }

        private void btnAjustment_Click(object sender, EventArgs e)
        {
          
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
            {
                //OpenAjustment();
                OpenCameraTest();
            }
            else
            {
                Utility.ShowMessage(_mainForm, "検査中は変更できません。", MessageType.Error);
            }
        }

        frmCameraTest _frmCameraTest = null;
        private void OpenCameraTest()
        {
            //自動検査の終了処理
            _mainForm.TermInsp();

            _frmCameraTest = new frmCameraTest();
            _frmCameraTest.FCFontsize = 18;
            _frmCameraTest.FCBoxEnable = true;
            _frmCameraTest.FCColor = "red";
            _frmCameraTest.FocusRoiTitleFontsize = 16;
            _frmCameraTest.FocusValueBox = true;
            _frmCameraTest.FocusRoiColor = "red";
            _frmCameraTest.WhiteBRoiColor = "blue";
            _frmCameraTest.WhiteBRoiTitleFontsize = 16;
            _frmCameraTest.WhiteBRoiTitleBox = true;
            _frmCameraTest.LightRoiTitleFontsize = 16;
            _frmCameraTest.LightRoiTitleBox = true;
            _frmCameraTest.LightRoiColor = "green";
            _frmCameraTest.MainteDirectory = clsMainteFunc.getInstance().MaintePath;
            _frmCameraTest.VisibleWhiteB = false;
            _frmCameraTest.ShowDialog(this);

            _frmCameraTest.Dispose();
            _frmCameraTest = null;

            //自動検査の初期化
            _mainForm.InitInsp();
            //LiveStart
            _mainForm.APcamLiveStart();

        }

        public bool ProcessShortcutKey(Keys keyData)
        {
            if (shortcutKeyHelper1.PerformClickByKeys(keyData))
            {
                return true;
            }
            return false;
        }

		/// <summary>
		/// 自動検査設定　ボタン
		/// V1020-001
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAutoInspSetting_Click(object sender, EventArgs e)
		{
			//自動検査設定画面を表示する
			using (FormAutoInspSetting fm = new FormAutoInspSetting(_mainForm))
			{
				fm.ShowDialog();
			}
		}

        private void uclSystem_DoubleClick(object sender, EventArgs e)
        {
            if (btnDevelopment.Visible == false)
            {
                frmPassword frm = new frmPassword();
                frm.DeveloperPassword = SystemParam.GetInstance().DeveloperPasswordHash;
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return;
            }
            btnDevelopment.Visible = !btnDevelopment.Visible;

            btnAjustment.Visible = btnDevelopment.Visible;
            panel1.Visible = btnDevelopment.Visible;
        }

        private void btnDevelopment_Click(object sender, EventArgs e)
        {
            frmDevelopmentSetting frmDev = new frmDevelopmentSetting(_mainForm);
            frmDev.Show();
        }

        private void uclSystem_Load(object sender, EventArgs e)
        {
            APCameraManager.getInstance().OnCaptureThread += UclSystem_OnCaptureThread;
            APCameraManager.getInstance().OnGetImageThread += UclSystem_OnGetImageThread;
            if (_mainForm != null)
                _mainForm.AutoInspection.OnAutoInspectGrabbed += AutoInspection_OnAutoInspectGrabbed;
        }


        /// <summary>
        /// APCameraManager getImageThread()から
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UclSystem_OnGetImageThread(object sender, APCameraManager.InspectTimeEventArgs e)
        {
            Action act = new Action(() =>
            {
                txtConnectImageTime[e.CamIndex].Text = e.ConnectImageTime.ToString();
                txtOnGrabbedTime[e.CamIndex].Text = e.OnGrabbedTime.ToString();
                txtGetImageTotalTime[e.CamIndex].Text = e.GetImageTotalTime.ToString();
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }


        /// <summary>
        /// キャプチャイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UclSystem_OnCaptureThread(object sender, APCameraManager.CaptureImageEventArgs e)
        {
            int iCamIndex = e.CamIndex;
            int iCaptureCount = e.CaptureCount;
            int iFailCount = e.FailCount;
            Action act = new Action(() =>
            {
                txtCaptureCount[iCamIndex].Text = iCaptureCount.ToString();
                txtCaptureFailCount[iCamIndex].Text = iFailCount.ToString();
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }
        private void AutoInspection_OnAutoInspectGrabbed(object sender, InspectionNameSpace.AutoInspection.AutoInspectGrabbedEventArgs e)
        {
            int iCamIndex = e.CamIndex;
            Action act = new Action(() =>
            {
                //検査タクト
                txtInspTime.Text = _mainForm.AutoInspection.InspTime.ToString("F1");
                //カウント
                txtRealGrabCount[iCamIndex].Text = _mainForm.AutoInspection.RealGrabCount[iCamIndex].ToString();
                txtQueueGrabCount[iCamIndex].Text = _mainForm.AutoInspection.QueueGrabCount[iCamIndex].ToString();
                txtQueueCount[iCamIndex].Text = _mainForm.AutoInspection.QueueCount[iCamIndex].ToString();
                txtSyncCount.Text = _mainForm.AutoInspection.SyncGrabCount.ToString();

            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }
    }
}
