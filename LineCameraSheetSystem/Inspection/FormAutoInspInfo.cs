using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using LineCameraSheetSystem;
using HalconCamera;
using System.Threading;
using System.Globalization;

namespace InspectionNameSpace
{
	public partial class FormAutoInspInfo : Form
	{
		public delegate void EnddingEventHandler(object sender, EventArgs e);
		public event EnddingEventHandler EnddingEvent = null;

		List<bool> _camQueue = new List<bool>();
		/// <summary>
		/// 再表示
		/// </summary>
		public void RefreshDatas(int? camno, HObject[] orgImage, HObject[] conImage)
		{
			Action act = new Action(() =>
				{
                    //if (chkBmpSave.Checked == true && camno == 0)
                    //{
                    //    foreach (CameraInfo c in _aInsp.CamInfos)
                    //    {
                    //        if (c.Enabled == true)
                    //            this._bmpSaveThread.Add((int)c.CamNo, orgImage[(int)c.CamNo]);
                    //    }
                    //}

                    //検査タクト
                    txtInspTime.Text = _aInsp.InspTime.ToString("F1");
					int i;
					//カウント
					i = 0;
					foreach (TextBox txt in txtRealGrabCount)
					{
						txtRealGrabCount[i].Text = _aInsp.RealGrabCount[i].ToString();
						txtQueueGrabCount[i].Text = _aInsp.QueueGrabCount[i].ToString();
						txtQueueCount[i].Text = _aInsp.QueueCount[i].ToString();
						i++;
					}
					txtSyncCount.Text = _aInsp.SyncGrabCount.ToString();
					if (chkRefreshImage.Checked == true)
					{
						//カメラ毎イメージ
						i = 0;
						lock (_winCam)
						{
							foreach (WindowControl win in _winCam)
							{
								win.WinManager.addIconicVar(orgImage[i]);
								win.WinManager.repaint();
                                i++;
							}
						}
					}
					if (chkRefreshImage.Checked == true)
					{
						//連結イメージ
						i = 0;
						lock (_winConn)
						{
							foreach (WindowControl win in _winConn)
							{
								if (chkDispBuff[i].Checked == false)
								{
									win.WinManager.addIconicVar(conImage[i]);
									win.WinManager.repaint();
								}
								i++;
							}
						}
					}
					this.RefreshWinCtrTools();

					if (camno != null)
						this._camQueue.Add(true);

					if (camno == null || this._camQueue.Count == 4)
					{
						this._camQueue.Clear();

						//保持
						int no = 0;
						foreach (CheckBox chk in chkSaveBuff)
						{
							if (chk.Checked == true)
							{
								if (this._buffImage[no].Count >= _buffMax)
								{
									this._buffImage[no][0].Dispose();
									this._buffImage[no].RemoveAt(0);
								}
								if (conImage[no] != null)
								{
									HObject img;
									HOperatorSet.CopyObj(conImage[no], out img, 1, -1);
									this._buffImage[no].Add(img);
								}
							}
							no++;
						}
					}

				});

			if (this.InvokeRequired)
				this.Invoke(act);
			else
				act();
		}

		private AutoInspection _aInsp;
		public List<WindowControl> _winCam = new List<WindowControl>();
		public List<WindowControl> _winConn = new List<WindowControl>();
		private List<TextBox> txtRealGrabCount = new List<TextBox>();
		private List<TextBox> txtQueueGrabCount = new List<TextBox>();
		private List<TextBox> txtQueueCount = new List<TextBox>();
		private List<CheckBox> chkThresCam = new List<CheckBox>();
		private List<CheckBox> chkThresConn = new List<CheckBox>();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ainsp"></param>
		public FormAutoInspInfo(AutoInspection ainsp)
		{
			InitializeComponent();
			this._aInsp = ainsp;
		}
		private void FormAutoInspInfo_Load(object sender, EventArgs e)
		{
			initializeProc();
		}
		private void initializeProc()
		{
			_winCam.Add(new WindowControl(hWindowControl1, null, null, false, _aInsp.CamInfos[0].ImageWidth, _aInsp.CamInfos[0].ImageHeight));
			_winCam.Add(new WindowControl(hWindowControl2, null, null, false, _aInsp.CamInfos[1].ImageWidth, _aInsp.CamInfos[1].ImageHeight));

			int cam1 = 0;
			int cam2 = 1;
			int cam3 = 0;
			int cam4 = 1;

			int conWidth1 = _aInsp.CamInfos[cam1].ImageWidth + _aInsp._connectOffsetXpix[0];
			int conHeight1 = _aInsp.CamInfos[cam1].ImageTileHeight + _aInsp.CamInfos[cam1].OverLapLines;

			int conWidth2 = _aInsp.CamInfos[cam3].ImageWidth + _aInsp._connectOffsetXpix[1];
			int conHeight2 = _aInsp.CamInfos[cam3].ImageTileHeight + _aInsp.CamInfos[cam3].OverLapLines;

			_winConn.Add(new WindowControl(hWindowControl5, hScrollBar1, vScrollBar1, true, conWidth1, conHeight1));
			_winConn.Add(new WindowControl(hWindowControl6, hScrollBar2, vScrollBar2, true, conWidth2, conHeight2));

			_imageWidth.Add(conWidth1);
			_imageWidth.Add(conWidth2);
			_imageHeight.Add(conHeight1);
			_imageHeight.Add(conHeight2);
			_leftLine.Add(_aInsp._connectOffsetXpix[0]);
			_leftLine.Add(_aInsp._connectOffsetXpix[1]);
			_rightLine.Add(_aInsp.CamInfos[cam2].ImageWidth);
			_rightLine.Add(_aInsp.CamInfos[cam4].ImageWidth);
			_topLine.Add(_aInsp.CamInfos[cam1].OverLapLines);
			_topLine.Add(_aInsp.CamInfos[cam3].OverLapLines);
			_bottomLine.Add(_aInsp.CamInfos[cam1].ImageTileHeight);
			_bottomLine.Add(_aInsp.CamInfos[cam3].ImageTileHeight);
			_centerLine.Add(_aInsp._connectCenterPix[0]);
			_centerLine.Add(_aInsp._connectCenterPix[1]);

			lblConn12Size.Text = string.Format("W{0} H{1}", _imageWidth[0].ToString("D04"), _imageHeight[0].ToString("D04"));
			lblConn34Size.Text = string.Format("W{0} H{1}", _imageWidth[1].ToString("D04"), _imageHeight[1].ToString("D04"));

			chkThresCam.Add(checkBox1);
			chkThresCam.Add(checkBox2);
			for (int i = 0; i < chkThresCam.Count; i++)
			{
				chkThresCam[i].Tag = i;
			}
			chkThresConn.Add(checkBox5);
			chkThresConn.Add(checkBox6);
			for (int i = 0; i < chkThresConn.Count; i++)
			{
				chkThresConn[i].Tag = i;
			}

			txtRealGrabCount.Add(textBox1);
			txtRealGrabCount.Add(textBox2);
			txtQueueGrabCount.Add(textBox5);
			txtQueueGrabCount.Add(textBox6);
			txtQueueCount.Add(textBox9);
			txtQueueCount.Add(textBox10);

			setting = true;
			chkInfoDisp.Checked = _aInsp.IniAccess.InfoDispEnable;
			spinBasePoint.Value = (decimal)_aInsp.IniAccess.BasePoint;
			spinNGOverlopRange.Value = (decimal)_aInsp.IniAccess.OverlapRange;
			spinImageSaveDirNum.Value = (decimal)_aInsp.IniAccess.ImageNumDirMax;
			spinImageSaveFileNum.Value = (decimal)_aInsp.IniAccess.ImageNumDirFileMax;
            spinDefaultMaskWidth.Value = (decimal)SystemParam.GetInstance().DefaultMaskWidth;
            spinDefaultMaskShift.Value = (decimal)SystemParam.GetInstance().DefaultMaskShift;
            spinDefaultInspWidth.Value = (decimal)SystemParam.GetInstance().DefaultInspWidth;
			spinImageSyncQueueCnt.Value = (decimal)_aInsp.IniAccess.ImageSyncQueueCnt;
			setting = false;

			_aInsp.SetMinMaxAveCalcRange(
                new double[] { (double)spinMaskWidth.Value, (double)spinMaskWidth.Value },
                new double[] { (double)spinMaskShift.Value, (double)spinMaskShift.Value },
                new double[] { (double)spinInspWidth.Value, (double)spinInspWidth.Value });

            spinBuffMax.Minimum = 1;
            spinBuffMax.Maximum = 30;
			spinBuffMax.Value = _buffMax;
			chkSaveBuff.Add(chkSaveBuffer1);
			chkSaveBuff.Add(chkSaveBuffer2);
			chkDispBuff.Add(chkDispBuffer1);
			chkDispBuff.Add(chkDispBuffer2);
			spinBuffNo.Add(spinBufferNo1);
			spinBuffNo.Add(spinBufferNo2);
			btnFileSave.Add(btnFileSave1);
			btnFileSave.Add(btnFileSave2);
			_buffImage = new List<List<HObject>>();
			foreach (WindowControl win in _winConn)
			{
				_buffImage.Add(new List<HObject>());
			}
			for (int i = 0; i < spinBuffNo.Count; i++)
			{
				this.SetBufferSpinDatas(i);
				btnFileSave[i].Enabled = false;
			}

			//InspOrder
			for (int i = 0; i < _aInsp.IniAccess.InspOrder.Length; i++)
			{
				lstInspOrder.Items.Add(_aInsp.IniAccess.InspOrder[i]);
			}
		}

        private void _bmpSaveThread_OnBufferCount(object sender, int cnt)
        {
            Action act = new Action(() =>
            {
                lblBmpSaveBuffCnt.Text = cnt.ToString();
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }

        public void FormEnd()
        {
            //_bmpSaveThread.OnBufferCount -= _bmpSaveThread_OnBufferCount;
            //this._bmpSaveThread.End();
        }

        //clsBmpSaveThread _bmpSaveThread;

		List<int> _imageWidth = new List<int>();
		List<int> _imageHeight = new List<int>();
		List<int> _leftLine = new List<int>();
		List<int> _rightLine = new List<int>();
		List<int> _bottomLine = new List<int>();
		List<int> _topLine = new List<int>();
		List<int> _centerLine = new List<int>();
		/// <summary>
		/// イメージの表示の仕方
		/// </summary>
		private void RefreshWinCtrTools()
		{
			int i;
			lock (_winCam)
			{
				i = 0;
				foreach (WindowControl win in _winCam)
				{
					win.WinManager.CenterLine = chkLineDisp.Checked;
					win.WinManager.LowThreshold = (int)spinThresholdLow.Value;
					win.WinManager.HighThreshold = (int)spinThresholdHigh.Value;
					win.WinManager.DispBin = chkThresCam[i].Checked;

					if (chkLineDisp.Checked == true)
					{
						//検査範囲
						int leftLine, rightLine;
						_aInsp.GetMinMaxAveCalcRangePix((AppData.CamID)i, out leftLine, out rightLine);
						win.GraphManager.AddLine("left", 0, leftLine, _aInsp.CamInfos[i].ImageHeight - 1, leftLine, "green");
						win.GraphManager.AddLine("right", 0, rightLine, _aInsp.CamInfos[i].ImageHeight - 1, rightLine, "green");
					}
					else
					{
						win.GraphManager.DeleteObject("left");
						win.GraphManager.DeleteObject("right");
					}
					i++;
				}
			}

			lock (_winConn)
			{
				i = 0;
				foreach (WindowControl win in _winConn)
				{
					if (chkLineDisp.Checked == true)
					{
						//top
						win.GraphManager.AddLine("line1", _topLine[i], 0, _topLine[i], _imageWidth[i] - 1, "red");
						//bottom
						win.GraphManager.AddLine("line2", _bottomLine[i], 0, _bottomLine[i], _imageWidth[i] - 1, "red");
						//left
						win.GraphManager.AddLine("line3", 0, _leftLine[i], _imageHeight[i] - 1, _leftLine[i], "red");
						//right
						win.GraphManager.AddLine("line4", 0, _rightLine[i], _imageHeight[i] - 1, _rightLine[i], "red");
						//Center
						win.GraphManager.AddLine("line5", 0, _centerLine[i], _imageHeight[i] - 1, _centerLine[i], "red");

						//検査範囲
						int leftLine, rightLine;
						if (i == 0)
							_aInsp.GetMinMaxAveCalcRangePix(AppData.SideID.表, out leftLine, out rightLine);
						else
							_aInsp.GetMinMaxAveCalcRangePix(AppData.SideID.裏, out leftLine, out rightLine);
						win.GraphManager.AddLine("left", 0, leftLine, _imageHeight[i] - 1, leftLine, "green");
						win.GraphManager.AddLine("right", 0, rightLine, _imageHeight[i] - 1, rightLine, "green");
					}
					else
					{
						win.GraphManager.DeleteObject("line1");
						win.GraphManager.DeleteObject("line2");
						win.GraphManager.DeleteObject("line3");
						win.GraphManager.DeleteObject("line4");
						win.GraphManager.DeleteObject("line5");
						win.GraphManager.DeleteObject("left");
						win.GraphManager.DeleteObject("right");
					}

					win.WinManager.LowThreshold = (int)spinThresholdLow.Value;
					win.WinManager.HighThreshold = (int)spinThresholdHigh.Value;
					win.WinManager.DispBin = chkThresConn[i].Checked;
					i++;
				}
			}

			int no = 0;
			foreach (WindowControl win in _winConn)
			{
				if (chkDispBuff[no].Checked == true)
					win.WinManager.repaint();
			}
		}
		/// <summary>
		/// クローズ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormAutoInspInfo_FormClosed(object sender, FormClosedEventArgs e)
		{
			ClearBufferImage();

			if (EnddingEvent != null)
			{
				EnddingEvent(this, new EventArgs());
			}
		}

		/// <summary>
		/// 基準位置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinBasePoint_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			_aInsp.IniAccess.BasePoint = (double)spinBasePoint.Value;
		}
		/// <summary>
		/// NG集約範囲
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinNGOverlopRange_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			_aInsp.IniAccess.OverlapRange = (double)spinNGOverlopRange.Value;
		}
		/// <summary>
		/// イメージ保存フォルダ数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinImageSaveDirNum_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			_aInsp.IniAccess.ImageNumDirMax = (int)spinImageSaveDirNum.Value;
		}
		/// <summary>
		/// イメージ保存ファイル数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinImageSaveFileNum_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			_aInsp.IniAccess.ImageNumDirFileMax = (int)spinImageSaveFileNum.Value;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinDefaultMaskWidth_ValueChanged(object sender, EventArgs e)
        {
            if (setting == true)
                return;
            SystemParam.GetInstance().DefaultMaskWidth = (int)spinDefaultMaskWidth.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinDefaultMaskShift_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
            SystemParam.GetInstance().DefaultMaskShift = (int)spinDefaultMaskShift.Value;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinDefaultInspWidth_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
            SystemParam.GetInstance().DefaultInspWidth = (int)spinDefaultInspWidth.Value;
		}

		/// <summary>
		/// 検査順番　入れ替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnInspOrderUp_Click(object sender, EventArgs e)
		{
			//↑
			int selPos = lstInspOrder.SelectedIndex;
			if (selPos > 0)
				this.InspOrderUpDown(-1);
		}
		private void btnInspOrderDown_Click(object sender, EventArgs e)
		{
			//↓
			int selPos = lstInspOrder.SelectedIndex;
			if (selPos != -1 && selPos < (lstInspOrder.Items.Count - 1))
				this.InspOrderUpDown(1);
		}
		private void InspOrderUpDown(int upDown)
		{
			int selPos = lstInspOrder.SelectedIndex;
			int prePos = selPos + upDown;
			AppData.InspID preId = (AppData.InspID)lstInspOrder.Items[prePos];
			lstInspOrder.Items[prePos] = lstInspOrder.Items[selPos];
			lstInspOrder.Items[selPos] = preId;
			lstInspOrder.SelectedIndex = prePos;

			_aInsp.IniAccess.InspOrder[prePos] = (AppData.InspID)lstInspOrder.Items[prePos];
			_aInsp.IniAccess.InspOrder[selPos] = (AppData.InspID)lstInspOrder.Items[selPos];
		}











		/// <summary>
		/// 画面を表示する・しない
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkInfoDisp_CheckedChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;

			if (chkInfoDisp.Checked == false)
			{
				setting = true;
				DialogResult res = MessageBox.Show("次回から表示されなくなります。\nよいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				if (res == DialogResult.No)
				{
					chkInfoDisp.Checked = true;
				}
				setting = false;
			}
			_aInsp.IniAccess.InfoDispEnable = chkInfoDisp.Checked;
		}
		/// <summary>
		/// ライン　ON/OFF　チェックボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkLineDisp_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshWinCtrTools();
		}


		bool setting = false;

		/// <summary>
		/// ２値化しきい値　スピンボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinThreshold_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;

			this.RefreshWinCtrTools();
		}

		/// <summary>
		/// イメージ　２値化　チェックボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkThresCam_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox obj = (CheckBox)sender;
			SetThresholdCheckBox(chkThresCam, obj);

			this.RefreshWinCtrTools();
		}
		/// <summary>
		/// 結合イメージ　２値化　チェックボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkThresConn_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox obj = (CheckBox)sender;
			SetThresholdCheckBox(chkThresConn, obj);
			this.RefreshWinCtrTools();
		}
		/// <summary>
		/// ２値化チェックボックス　状態制御
		/// </summary>
		/// <param name="chks"></param>
		/// <param name="targetCheck"></param>
		private void SetThresholdCheckBox(List<CheckBox> chks, CheckBox targetCheck)
		{
			if (setting)
				return;

			setting = true;
			if (targetCheck.Checked == true)
			{
				int no = (int)targetCheck.Tag;
				for (int i = 0; i < chks.Count; i++)
				{
					if (no != i)
						chks[i].Checked = false;
				}
			}
			setting = false;
		}

		/// <summary>
		/// 明　感度　スピンボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinBright_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			SetThresholdSpinValue(spinBright);
			this.RefreshWinCtrTools();
		}
		/// <summary>
		/// 暗　感度　スピンボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinDark_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			SetThresholdSpinValue(spinDark);
			this.RefreshWinCtrTools();
		}
		/// <summary>
		/// しきい値を　感度合わせて設定する
		/// </summary>
		/// <param name="spin"></param>
		private void SetThresholdSpinValue(NumericUpDown spin)
		{
			setting = true;
			int low = (int)spinThresholdLow.Value;
			int high = (int)spinThresholdHigh.Value;
			bool run;
			if (spin == spinBright)
				run = Utility.GetBrightThreshold(128, (int)spin.Value, ref low, ref high);
			else
				run = Utility.GetDarkThreshold(128, (int)spin.Value, ref low, ref high);
			if (run == true)
			{
				spinThresholdLow.Value = (decimal)low;
				spinThresholdHigh.Value = (decimal)high;
			}
			setting = false;
		}

		#region 画像上をダブルクリックでフィットサイズにする
		private MouseButtons mb;
		private double mX;
		private double mY;
		private DateTime dt = DateTime.Now;
		private void WinConn_HMouseDown(object sender, HMouseEventArgs e)
		{
			HWindowControl obj = (HWindowControl)sender;
			int index = int.Parse((string)obj.Tag);

			DateTime nowTime = DateTime.Now;
			if ((nowTime - dt).TotalMilliseconds <= SystemInformation.DoubleClickTime)
			{
				if ((Math.Abs(mX - e.X) <= SystemInformation.DoubleClickSize.Width) && (Math.Abs(mY - e.Y) <= SystemInformation.DoubleClickSize.Height))
				{
					if ((mb == e.Button) && (mb == MouseButtons.Left))
					{
						_winConn[index].WinManager.FittingImage(false);
					}
				}
			}
			mb = e.Button;
			mX = e.X;
			mY = e.Y;
			dt = nowTime;
		}
		#endregion

		bool bufsetting = false;
		int _buffMax = 5;
		List<CheckBox> chkSaveBuff = new List<CheckBox>();
		List<CheckBox> chkDispBuff = new List<CheckBox>();
		List<NumericUpDown> spinBuffNo = new List<NumericUpDown>();
		List<Button> btnFileSave = new List<Button>();
		private List<List<HObject>> _buffImage = new List<List<HObject>>();
		private void chkSaveBuffer_CheckedChanged(object sender, EventArgs e)
		{
			if (bufsetting == true)
				return;

			CheckBox chk = (CheckBox)sender;
			int no = int.Parse(chk.Tag.ToString());
			bufsetting = true;
			if (chk.Checked == true)
			{
				chkDispBuff[no].Checked = false;
				chkDispBuff[no].Enabled = false;
				spinBuffNo[no].Enabled = false;
				spinBuffMax.Enabled = false;
				btnFileSave[no].Enabled = false;
			}
			else
			{
				//chkDispBuff[no].Checked = true;
				chkDispBuff[no].Enabled = true;
				spinBuffNo[no].Enabled = true;
				bool en = true;
				foreach (CheckBox ch in chkSaveBuff)
				{
					en &= !ch.Checked;
				}
				spinBuffMax.Enabled = en;
				SetBufferSpinDatas(no);
			}

			bufsetting = false;
		}

		private void chkDispBuffer_CheckedChanged(object sender, EventArgs e)
		{
			if (bufsetting == true)
				return;

			CheckBox chk = (CheckBox)sender;
			int no = int.Parse(chk.Tag.ToString());
			bufsetting = true;
			btnFileSave[no].Enabled = chk.Checked;
			if (chk.Checked == true)
			{
				int imgNo = (int)(spinBuffNo[no].Value - 1);
				DispBufferImage(no, imgNo);
			}
			bufsetting = false;
		}

		private void spinBufferNo_ValueChanged(object sender, EventArgs e)
		{
			if (bufsetting == true)
				return;

			NumericUpDown spin = (NumericUpDown)sender;
			int no = int.Parse(spin.Tag.ToString());
			bufsetting = true;
			int imgNo = (int)(spin.Value - 1);
			DispBufferImage(no, imgNo);
			bufsetting = false;
		}

		private void DispBufferImage(int no, int imgNo)
		{
			lock (_winConn)
			{
				if (chkDispBuff[no].Checked == true)
				{
                    _winConn[no].WinManager.addIconicVar(this._buffImage[no][imgNo]);
					_winConn[no].WinManager.repaint();
				}
			}
		}

		private void SetBufferSpinDatas(int no)
		{
			if (this._buffImage[no].Count == 0)
			{
				spinBuffNo[no].Enabled = false;
				spinBuffNo[no].Minimum = 0;
				spinBuffNo[no].Maximum = 0;
				spinBuffNo[no].Value = 0;
				chkDispBuff[no].Enabled = false;
			}
			else
			{
				spinBuffNo[no].Enabled = true;
				spinBuffNo[no].Minimum = 1;
				spinBuffNo[no].Maximum = this._buffImage[no].Count;
				spinBuffNo[no].Value = 1;
				chkDispBuff[no].Enabled = true;
			}
		}

		private void spinBuffMax_ValueChanged(object sender, EventArgs e)
		{
			bufsetting = true;
			ClearBufferImage();
			for (int i = 0; i < spinBuffNo.Count; i++)
			{
				chkDispBuff[i].Checked = false;
				SetBufferSpinDatas(i);
				btnFileSave[i].Enabled = false;
			}
			_buffMax = (int)spinBuffMax.Value;
			bufsetting = false;
		}
		private void ClearBufferImage()
		{
			foreach (List<HObject> imgs in this._buffImage)
			{
				foreach (HObject img in imgs)
				{
					img.Dispose();
				}
				imgs.Clear();
			}
		}

		private void btnFileSave_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			int no = int.Parse(btn.Tag.ToString());

			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				dlg.AddExtension = true;
				dlg.Filter = "BMPファイル(*.bmp)|*.bmp";
				bool loop = true;
				while (loop)
				{
					DialogResult res = dlg.ShowDialog();
					System.Diagnostics.Debug.WriteLine(string.Format("FileName = {0}", dlg.FileName));
					if (res == DialogResult.OK)
					{
						if (System.IO.Path.GetExtension(dlg.FileName) != ".bmp")
						{
							MessageBox.Show("bmp拡張子を指定して下さい。");
							dlg.FileName = System.IO.Path.GetFileName(dlg.FileName);
						}
						else
						{
							//保存
                            int imgNo = (int)spinBuffNo[no].Value - 1;
							HOperatorSet.WriteImage(this._buffImage[no][imgNo], "bmp", 0, dlg.FileName);
							loop = false;
						}
					}
					else
					{
						loop = false;
					}
				}
			}
		}

		private void spinImageSyncQueueCnt_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
			_aInsp.IniAccess.ImageSyncQueueCnt = (int)spinImageSyncQueueCnt.Value;
		}

		private void chkRefreshImage_CheckedChanged(object sender, EventArgs e)
		{
			DispEnable = chkRefreshImage.Checked;
		}
		public bool DispEnable { get; private set; }

		/// <summary>
		/// マスク幅
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void spinMaskWidth_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
            double maskWidth = (double)spinMaskWidth.Value;
            double maskShift = (double)spinMaskShift.Value;
            double inspWidth = (double)spinInspWidth.Value;
			_aInsp.SetMinMaxAveCalcRange(
                new double[] { maskWidth, maskWidth },
                new double[] { maskShift, maskShift },
                new double[] { inspWidth, inspWidth });
		}
        private void spinMaskShift_ValueChanged(object sender, EventArgs e)
        {
            if (setting == true)
                return;
            double maskWidth = (double)spinMaskWidth.Value;
            double maskShift = (double)spinMaskShift.Value;
            double inspWidth = (double)spinInspWidth.Value;
            _aInsp.SetMinMaxAveCalcRange(
                new double[] { maskWidth, maskWidth },
                new double[] { maskShift, maskShift },
                new double[] { inspWidth, inspWidth });
        }

        /// <summary>
        /// 検査幅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinInspWidth_ValueChanged(object sender, EventArgs e)
		{
			if (setting == true)
				return;
            double maskWidth = (double)spinMaskWidth.Value;
            double maskShift = (double)spinMaskShift.Value;
            double inspWidth = (double)spinInspWidth.Value;
            _aInsp.SetMinMaxAveCalcRange(
                new double[] { maskWidth, maskWidth },
                new double[] { maskShift, maskShift },
                new double[] { inspWidth, inspWidth });
        }

        public void GetInput1(int iAddr, ref bool blnData)
        {
            if (iAddr == 1)
            {
                blnData = chkInput1.Checked;
            }
        }
        public void ChangeOutput1(int iAddr, bool onoff)
        {
            foreach (ESignalControl e in Enum.GetValues(typeof(ESignalControl)))
            {
                if (iAddr == clsSignalControl.GetInstance().DipMap(e))
                {
                    SetOutSignal(e, onoff);
                    break;
                }
            }
        }
        private void SetOutSignal(ESignalControl sig, bool onoff)
        {
            if (sig == ESignalControl.Red)
            {
                lblRed.BackColor = (onoff == false) ? Color.White : Color.Red;
            }
            else if (sig == ESignalControl.Yellow)
            {
                lblYellow.BackColor = (onoff == false) ? Color.White : Color.Yellow;
            }
            else if (sig == ESignalControl.Green)
            {
                lblGreen.BackColor = (onoff == false) ? Color.White : Color.GreenYellow;
            }
            else if (sig == ESignalControl.Blue)
            {
                lblBlue.BackColor = (onoff == false) ? Color.White : Color.Blue;
            }
            else if (sig == ESignalControl.Buzzer)//V1333 表示灯（中央）はそのまま
            {
                lblBuzzer.BackColor = (onoff == false) ? Color.White : Color.Pink;
            }
            else if (sig == ESignalControl.External1)
            {
                lblExtOut1.BackColor = (onoff == false) ? Color.White : Color.Magenta;
            }
            else if (sig == ESignalControl.External2)
            {
                lblExtOut2.BackColor = (onoff == false) ? Color.White : Color.Magenta;
            }
            else if (sig == ESignalControl.External3) //V1057 NG表裏修正 yuasa 20190118：外部３追加
            {
                lblExtOut3.BackColor = (onoff == false) ? Color.White : Color.Magenta;
            }
            else if (sig == ESignalControl.External4) //V1057 NG表裏修正 yuasa 20190118：外部４追加
            {
                lblExtOut4.BackColor = (onoff == false) ? Color.White : Color.Magenta;
            }
            else if (sig == ESignalControl.BuzzerBothSide)//V1333 表示灯（両端）を追加
            {
                lblBuzzerBoth.BackColor = (onoff == false) ? Color.White : Color.Pink;
            }
        }

        private void chkBmpSave_CheckedChanged(object sender, EventArgs e)
        {

        }
    }




    public class clsBmpSaveThread
    {
        public class BmpSaveArgs : IDisposable
        {
            public HObject Img { get { return _img; } }
            private HObject _img;
            public DateTime DTime { get; }
            public int CamNo { get; private set; }
            public BmpSaveArgs(int camno, HObject img)
            {
                HOperatorSet.CopyImage(img, out _img);
                DTime = DateTime.Now;
                this.CamNo = camno;
            }
            public void Dispose()
            {
                _img.Dispose();
            }
        }
    }
}
