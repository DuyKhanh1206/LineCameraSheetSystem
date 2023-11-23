using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class frmAutoLightValueCalibration : Form
    {
        clsAutoLightCalibration _calib = null;
        public void SetAutoLightValueCalibration(clsAutoLightCalibration calib)
        {
            _calib = calib;
        }

        public frmAutoLightValueCalibration()
        {
            InitializeComponent();

            btnAllPurpose.Click += new EventHandler(btnButton_Click);
            btnYes.Click += new EventHandler(btnButton_Click);
            btnNo.Click += new EventHandler(btnButton_Click);

#if DEBUG
            Size = new System.Drawing.Size(765, 569);
#else
//            lstDebug.Visible = false;
#endif


            SetMode(EDisplayMode.Run);
        }

        const string TITTLE_RUN = "照明自動調整を実行中です\nしばらくお待ちください";
        const string TITTLE_CANCEL = "照明自動調整がキャンセルされました";
        const string TITTLE_ERROR = "照明自動調整に失敗しました";
        const string TITTLE_UPDATE = "照明自動調整が完了しました";

        private bool _errOkFlag = false;
        void btnButton_Click(object sender, EventArgs e)
        {
            if (_mode == EDisplayMode.Run)
            {
                // キャンセル発動
                if (sender == btnAllPurpose)
                {
                    _calib.Stop();
                }
            }
            else if (_mode == EDisplayMode.Cancel)
            {
                if (sender == btnAllPurpose)
                {
                    _errOkFlag = true;
                    _calib.Stop();
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    Close();
                }
            }
            else if (_mode == EDisplayMode.UpdateQuery)
            {
                if (sender == btnYes)
                    DialogResult = System.Windows.Forms.DialogResult.Yes;
                else
                    DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
        }

        public void SetMessage(string sMessage, EDisplayMode mode = EDisplayMode.Unknown)
        {
            if (_errOkFlag == true)
                return;
            Action act = new Action(() =>
                {
                    lblMessage.Text = sMessage;
                    if (mode != EDisplayMode.Unknown)
                        SetMode(mode);
                });

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        public void AddDebugMessage(clsAutoLightValueCalibration.ECameraSide eCamSide, string sMessage)
        {
            Action act = new Action(() =>
                {
                    if (eCamSide == clsAutoLightValueCalibration.ECameraSide.UpSide)
                    {
                        lstDebugUp.Items.Insert(0, sMessage);
                        LogingDllWrap.LogingDll.Loging_SetLogString(lstDebugUp.ToString() + ":" + sMessage);
                    }
                    else
                    {
                        lstDebugDown.Items.Insert(0, sMessage);
                        LogingDllWrap.LogingDll.Loging_SetLogString(lstDebugDown.ToString() + ":" + sMessage);
                    }
                });

            Invoke(act);

        }

        public void SetProgressMessage(int iIndex, string sMessage)
        {
            Action act = new Action(() =>
            {
                if( iIndex == 0 )
                    lblProgress1.Text = sMessage;
                else
                    lblProgress2.Text = sMessage;
            });

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        public enum EDisplayMode 
        {
            Run,
            Cancel,
            UpdateQuery,
            Error,
            Unknown = -1,
        }

        EDisplayMode _mode = EDisplayMode.Unknown;
        public void SetMode(EDisplayMode mode)
        {
            if (_mode == mode)
                return;

            _mode = mode;

            Action act = new Action(() =>
                {

                    if (_mode == EDisplayMode.Run)
                    {
                        btnAllPurpose.Visible = true;
                        btnAllPurpose.Text = "ｷｬﾝｾﾙ";
                        lblTittle.Text = TITTLE_RUN;

                        lblMessage.Visible = false;
                        lblProgress1.Visible = true;
                        lblProgress2.Visible = true;
                        prbProgres.Visible = true;
                        btnYes.Visible = false;
                        btnNo.Visible = false;
                    }
                    else if (_mode == EDisplayMode.Cancel || _mode == EDisplayMode.Error )
                    {
                        btnAllPurpose.Visible = true;
                        btnAllPurpose.Text = "OK";
                        btnYes.Visible = false;
                        btnNo.Visible = false;

                        if (_mode == EDisplayMode.Cancel)
                        {
                            lblTittle.Text = TITTLE_CANCEL;
                        }
                        else
                        {
                            lblTittle.Text = TITTLE_ERROR;
                        }
                        prbProgres.Visible = false;
                        lblMessage.Visible = true;
                        lblProgress1.Visible = false;
                        lblProgress2.Visible = false;
                    }
                    else if (_mode == EDisplayMode.UpdateQuery)
                    {
                        lblTittle.Text = TITTLE_UPDATE;
                        btnAllPurpose.Visible = false;
                        btnYes.Visible = true;
                        btnNo.Visible = true;
                        prbProgres.Visible = false;

                        lblMessage.Visible = true;
                        lblProgress1.Visible = false;
                        lblProgress2.Visible = false;
                    }
                });

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        public void ProgressCapacity(int iCnt)
        {
            Action act = new Action(() =>
                {
                    prbProgres.Maximum = 100 * iCnt;
                }
            );

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        public void UpdateProgress( int iIncrease )
        {
            Action act = new Action(() =>
                {
                    if (prbProgres.Maximum >= prbProgres.Value + iIncrease)
                        prbProgres.Value += iIncrease;
                    else
                        prbProgres.Value = prbProgres.Maximum;
                }
            );

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        /// <summary>自動調光をキャンセルする　v1338のPC電源ボタン押下対応</summary>//v1338 yuasa
        public void AutoLightValueCalibrationCancel()
        {
            _calib.Stop();
        }
    }
}
