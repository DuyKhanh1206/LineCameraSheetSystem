using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResultActionDataClassNameSpace;
using HalconDotNet;
using ViewROI;

namespace LineCameraSheetSystem
{
    public partial class frmNg1Image : Form
    {
        ViewROI.HWndCtrl _hWndCtrl = null;
        private bool _autoInspectionMode;

        private bool _setMouseMoveFlg = false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmNg1Image(bool autoInspectionMode = false)
        {
            _autoInspectionMode = autoInspectionMode;
            InitializeComponent();

            dataGridView1.Rows.Add(5);
            dataGridView1[0, 0].Value = "カメラ";
            dataGridView1[0, 1].Value = "面";
            dataGridView1[0, 2].Value = "測長[m]";
            dataGridView1[0, 3].Value = "ｱﾄﾞﾚｽ[㎜]";
            dataGridView1[0, 4].Value = "発生時間";

            dataGridView1[2, 0].Value = "欠点種";
            dataGridView1[2, 1].Value = "縦[㎜]";
            dataGridView1[2, 2].Value = "横[㎜]";
            dataGridView1[2, 3].Value = "面積[㎜²]";
            dataGridView1[2, 4].Value = "ゾーン";

            dataGridView1[0, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[0, 1].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[0, 2].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[0, 3].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[0, 4].Style.BackColor = Color.FromArgb(255, 212, 208, 200);

            dataGridView1[2, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[2, 1].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[2, 2].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[2, 3].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            dataGridView1[2, 4].Style.BackColor = Color.FromArgb(255, 212, 208, 200);

            btnClose.Focus();
            btnClose.Select();

            //ViewROI
            _hWndCtrl = new HWndCtrl(hwndctrlImage);
            _hWndCtrl.FittingActualMagnification = false;
            _hWndCtrl.Repaint += new RepaintEventHandler(_hWndCtrl_Repaint);
            _hWndCtrl.MouseMoveOnImage += new MouseMoveOnImageEventHandler(_hWndCtrl_MouseMoveOnImage);
            _setMouseMoveFlg = true;

            initMap();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmNg1Image_Load(object sender, EventArgs e)
        {
            if (_autoInspectionMode == false)
                chkEnableGraph.Checked = SystemParam.GetInstance().OneNGImageGraphEnable;
            else
                chkEnableGraph.Checked = false;

            if (_autoInspectionMode == false)
                chkEnableBaseImage.Checked = SystemParam.GetInstance().OneNGImageBaseImageEnable;
            else
                chkEnableBaseImage.Checked = false;

            _hWndCtrl.SetBackColor();
        }
        /// <summary>
        /// Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmNg1Image_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_hoImg != null)
                _hoImg.Dispose();
            if (_hoXLDHorz != null)
                _hoXLDHorz.Dispose();
            if (_hoXLDVert != null)
                _hoXLDVert.Dispose();

            if (_penLine1 != null)
                _penLine1.Dispose();
            if (_brhHeaderSheetInfo != null)
                _brhHeaderSheetInfo.Dispose();
            if (_brhBackGround != null)
                _brhBackGround.Dispose();
            if (_bmpBG != null)
                _bmpBG.Dispose();
        }

        /// <summary>
        /// 波形の表示・非表示　チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkEnableGraph_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            if (_autoInspectionMode == false)
                SystemParam.GetInstance().OneNGImageGraphEnable = chkEnableGraph.Checked;

            if (chkEnableGraph.Checked == true)
                GetGraphData(_grayMultiImage);
            _hWndCtrl.repaint();
            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 閉じる　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int _dispPosition;
        private int _resCount;
        private List<ResActionData> _resDatas;
        public void SetNgListDatas(List<ResActionData> dt)
        {
            _resDatas = null;
            _dispPosition = 0;
            _resCount = 0;
            if (dt != null)
            {
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                _resDatas = dt;
                _resCount = _resDatas.Count;
            }
            else
            {
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
            }
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            _dispPosition--;

            double[] bufZones = new double[_adZones.Length];
            _adZones.CopyTo(bufZones, 0);
            SetNgData(_resDatas[_dispPosition], _dSheetWidth, bufZones);

            initMap();
            drawMap();
            picMap.Refresh();
            _hWndCtrl.repaint();

            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            _dispPosition++;
            double[] bufZones = new double[_adZones.Length];
            _adZones.CopyTo(bufZones, 0);
            SetNgData(_resDatas[_dispPosition], _dSheetWidth, bufZones);

            initMap();
            drawMap();
            picMap.Refresh();
            _hWndCtrl.repaint();

            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// NG情報セット
        /// </summary>
        /// <param name="resacdata"></param>
        public void SetNgData(ResActionData resacdata, double sheetWidth, double[] sheetZones)
        {
            HObject hoBufImage = null;

            if (resacdata != null)
            {
                _dSheetWidth = sheetWidth;
                _adZones = new double[sheetZones.Length];
                sheetZones.CopyTo(_adZones, 0);
                AppData.SideID sideNo = resacdata.SideId;
                string colName;
                if (sideNo == 0)
                    colName = SystemParam.GetInstance().markColorUpSide[(int)resacdata.InspId].colorARGB;
                else
                    colName = SystemParam.GetInstance().markColorDownSide[(int)resacdata.InspId].colorARGB;
                _tipColor = ColorTranslator.FromHtml(colName);
                _tipAddress = resacdata.PositionX;

                _setNGInfo = true;
                if (_colorMultiImage != null)
                    _colorMultiImage.Dispose();
                if (_grayMultiImage != null)
                    _grayMultiImage.Dispose();
                HOperatorSet.GenEmptyObj(out _colorMultiImage);
                HOperatorSet.GenEmptyObj(out _grayMultiImage);
                resacdata.GetImage(ref _colorMultiImage, ref _grayMultiImage);
                if (_hoImg != null)
                {
                    _hoImg.Dispose();
                    _hoImg = null;
                }
                HTuple htNum;
                HOperatorSet.CountObj(_colorMultiImage, out htNum);
                if (chkEnableBaseImage.Checked == false && htNum.I >= 1)
                    HOperatorSet.SelectObj(_colorMultiImage, out _hoImg, 1);
                else if (chkEnableBaseImage.Checked == true && htNum.I >= 2)
                    HOperatorSet.SelectObj(_colorMultiImage, out _hoImg, 2);

                if (resacdata.InspId == AppData.InspID.明小 || resacdata.InspId == AppData.InspID.明中 || resacdata.InspId == AppData.InspID.明大)
                    _hanteiAkarui = true;
                else
                    _hanteiAkarui = false;

                SetImage(_hoImg);
                DrawScale(_hoImg);

                GetGraphData(_grayMultiImage);
                DrawGraph();
                SetInfo(resacdata);

                //ライン番号の表示
                if (_resDatas != null)
                {
                    lblLineNo.Text = string.Format("No. {0}", resacdata.LineNo.ToString());
                    _dispPosition = _resDatas.FindIndex(x => x.LineNo == resacdata.LineNo);
                    btnPrev.Enabled = (_dispPosition <= 0) ? false : true;
                    btnNext.Enabled = (_dispPosition >= (_resCount - 1)) ? false : true;
                }
            }
            if (hoBufImage != null)
                hoBufImage.Dispose();
        }

        /// <summary>
        /// 画像セット
        /// </summary>
        /// <param name="hoimg"></param>
        private void SetImage(HObject hoimg)
        {
            if (hoimg != null)
            {
                if (_setMouseMoveFlg == false)
                {
                    _setMouseMoveFlg = true;
                    _hWndCtrl.MouseMoveOnImage += new MouseMoveOnImageEventHandler(_hWndCtrl_MouseMoveOnImage);
                }
                //画面に表示
                _hWndCtrl.Fitting = true;
                _hWndCtrl.addIconicVar(hoimg);
                _hWndCtrl.FittingImage(false);
            }
            else
            {
                if (_setMouseMoveFlg == true)
                {
                    _setMouseMoveFlg = false;
                    _hWndCtrl.MouseMoveOnImage -= new MouseMoveOnImageEventHandler(_hWndCtrl_MouseMoveOnImage);
                }
                _hWndCtrl.addIconicVar(null);
                //_hWndCtrl.clearList();
            }
        }

        /// <summary>
        /// 数値情報
        /// </summary>
        /// <param name="resacdata"></param>
        private void SetInfo(ResActionData resacdata)
        {
            dataGridView1[1, 0].Value = resacdata.CamId.ToString().Remove(0, 3);
            dataGridView1[1, 1].Value = resacdata.SideId.ToString();
            dataGridView1[1, 2].Value = (resacdata.PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal) ;
            dataGridView1[1, 3].Value = resacdata.PositionX.ToString(SystemParam.GetInstance().AddressDecimal);
            dataGridView1[1, 4].Value = resacdata.Time.ToString();

            dataGridView1[3, 0].Value = resacdata.InspId.ToString().Replace("暗", "");
            dataGridView1[3, 1].Value = resacdata.Height.ToString(SystemParam.GetInstance().NgDataDecimal);
            dataGridView1[3, 2].Value = resacdata.Width.ToString(SystemParam.GetInstance().NgDataDecimal);
            dataGridView1[3, 3].Value = resacdata.Area.ToString(SystemParam.GetInstance().NgDataDecimal);
            dataGridView1[3, 4].Value = resacdata.ZoneId.ToString();

            lblOmoteUra.Text = resacdata.SideId.ToString();
            lblDateTime.Text = resacdata.Time.ToString();
            lblVertical.Text = resacdata.Height.ToString(SystemParam.GetInstance().NgDataDecimal);
            lblHorizontal.Text = resacdata.Width.ToString(SystemParam.GetInstance().NgDataDecimal);
        }

        /// <summary>
        /// マウスMoveイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _hWndCtrl_MouseMoveOnImage(object sender, MouseMoveOnImageEventArgs e)
        {
            if (_hanteiAkarui == true)
                _grayValue = e.GrayValue - 128;
            else
                _grayValue = 127 - e.GrayValue;

            _hWndCtrl.repaint();
        }
        /// <summary>
        /// ViewROI　再描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _hWndCtrl_Repaint(object sender, RepaintEventArgs e)
        {
            DrawScale(_hoImg);
            DrawGraph();
        }

        int _grayValue = -1;
        private bool _setNGInfo = false;
        private HObject _hoImg = null;
        private bool _hanteiAkarui = false;
        private int _imgWidth = 0;
        private int _imgHeight = 0;
        private int[] _htHorz = null;
        private int[] _htVert = null;
        private int _kando = -1;
        HObject _hoXLDHorz = null;
        HObject _hoXLDVert = null;
        private HObject _colorMultiImage;
        private HObject _grayMultiImage;

        /// <summary>
        /// 波形データ　生成
        /// </summary>
        /// <param name="hoimg"></param>
        private void GetGraphData(HObject hoimg)
        {
            if (hoimg != null)
            {
                ImgProjection(hoimg);
                getProjection(hoimg);
            }
        }

        /// <summary>
        /// スケール描画
        /// </summary>
        /// <param name="hoimg"></param>
        private void DrawScale(HObject hoimg)
        {
            if (chkEnableGraph.Checked == false)
                return;
            if (_setNGInfo == false)
                return;

            if (hoimg != null)
            {
                HTuple width, height;
                HOperatorSet.GetImageSize(hoimg, out width, out height);

                int lw10 = 3;
                int lw05 = 1;

                double pitchWidth = width / 255.0;
                double pitchHeight = height / 255.0;
                double dRate = width / 255.0;
                //横線
                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "green");
                HOperatorSet.DispLine(hwndctrlImage.HalconWindow, height / 2, 0, height / 2, width);
                for (int i = 0; i < 128; i += 5)
                {
                    int lw;
                    if ((i % 10) == 0)
                        lw = lw10;
                    else if ((i % 5) == 0)
                        lw = lw05;
                    else
                        lw = 1;
                    double pw = (pitchWidth * i);
                    HOperatorSet.DispLine(hwndctrlImage.HalconWindow, height / 2 - lw, width / 2 + pw, height / 2 + lw, width / 2 + pw);
                    HOperatorSet.DispLine(hwndctrlImage.HalconWindow, height / 2 - lw, width / 2 - pw, height / 2 + lw, width / 2 - pw);
                }
                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "blue");
                HOperatorSet.SetFont(hwndctrlImage.HalconWindow, Fujita.HalconMisc.HalconExtFunc.GetFontFormat(Fujita.HalconMisc.HalconExtFunc.BaseFontName, 12));
                HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, _imgHeight / 2 + 5, 5);
                HOperatorSet.WriteString(hwndctrlImage.HalconWindow, "明");
                HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, _imgHeight / 2 + 5, _imgWidth - 10);
                HOperatorSet.WriteString(hwndctrlImage.HalconWindow, "暗");

                //縦線
                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "green");
                HOperatorSet.DispLine(hwndctrlImage.HalconWindow, 0, width / 2, height, width / 2);
                for (int i = 0; i < 128; i += 5)
                {
                    int lw;
                    if ((i % 10) == 0)
                        lw = lw10;
                    else if ((i % 5) == 0)
                        lw = lw05;
                    else
                        lw = 1;
                    double pw = (pitchHeight * i);
                    HOperatorSet.DispLine(hwndctrlImage.HalconWindow, height / 2 + pw, width / 2 - lw, height / 2 + pw, width / 2 + lw);
                    HOperatorSet.DispLine(hwndctrlImage.HalconWindow, height / 2 - pw, width / 2 - lw, height / 2 - pw, width / 2 + lw);
                }
                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "red");
                HOperatorSet.SetFont(hwndctrlImage.HalconWindow, Fujita.HalconMisc.HalconExtFunc.GetFontFormat(Fujita.HalconMisc.HalconExtFunc.BaseFontName, 12));
                HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, 5, _imgWidth / 2 + 5);
                HOperatorSet.WriteString(hwndctrlImage.HalconWindow, "暗");
                HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, _imgHeight - 10, _imgWidth / 2 + 5);
                HOperatorSet.WriteString(hwndctrlImage.HalconWindow, "明");
            }
        }

        /// <summary>
        /// 波形描画
        /// </summary>
        private void DrawGraph()
        {
            if (_setNGInfo == false)
                return;

            if (_hoImg != null)
            {
                if (_htHorz != null && _htVert != null)
                {
                    if (chkEnableGraph.Checked == false)
                        return;
                    drawProjection();
                }
            }
            else
            {
                HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, 150, 120);
                HOperatorSet.SetFont(hwndctrlImage.HalconWindow, Fujita.HalconMisc.HalconExtFunc.GetFontFormat(Fujita.HalconMisc.HalconExtFunc.BaseFontName, 20));
                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "white");
                HOperatorSet.WriteString(hwndctrlImage.HalconWindow, "イメージなし");
            }
        }

        /// <summary>
        /// 波形データ配列を生成
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        unsafe private bool ImgProjection(HObject img)
        {
            if (chkEnableGraph.Checked == false)
                return true;

            _htHorz = null;
            _htVert = null;
            _kando = -1;
            _imgWidth = 0;
            _imgHeight = 0;

            if (img == null)
                return false;

            HObject hoImage;
            HObject backImg;
            HObject yLineRegion;
            HObject hoSelectedObj;
            HObject[] hoConnectImage = new HObject[0];
            HOperatorSet.GenEmptyObj(out hoImage);
            HOperatorSet.GenEmptyObj(out backImg);
            HOperatorSet.GenEmptyObj(out yLineRegion);
            HOperatorSet.GenEmptyObj(out hoSelectedObj);

            HTuple[] htPointer;
            HTuple htType, htWidth, htHeight;

            byte*[] pPixOrg;
            byte*[] pPix;


            int imgCnt = 0;

            try
            {
                HTuple htNum;
                HOperatorSet.CountObj(img, out htNum);
                if (htNum.I <= 0)
                    return false;
                if (htNum.I <= 1)
                {
                    HOperatorSet.SelectObj(img, out hoImage, 1);
                }
                else
                {
                    HOperatorSet.CopyObj(img, out hoImage, 1, -1);
                }
                HTuple htChannel;
                HOperatorSet.CountChannels(hoImage, out htChannel);

                HOperatorSet.GetImageSize(hoImage, out htWidth, out htHeight);
                _imgWidth = htWidth.I;
                _imgHeight = htHeight.I;

                HTuple min, max, range;

                HOperatorSet.GenEmptyObj(out backImg);
                HOperatorSet.GenEmptyObj(out yLineRegion);

                HOperatorSet.CopyImage(hoImage, out backImg);
                HOperatorSet.CountObj(backImg, out htNum);
                HOperatorSet.CountChannels(backImg, out htChannel);

                imgCnt = htNum.I;

                hoConnectImage = new HObject[imgCnt];
                htPointer = new HTuple[imgCnt];
                pPixOrg = new byte*[imgCnt];
                pPix = new byte*[imgCnt];
                for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                {
                    hoSelectedObj.Dispose();
                    HOperatorSet.SelectObj(backImg, out hoConnectImage[imgNo], imgNo + 1);
                    HOperatorSet.GetImagePointer1(hoConnectImage[imgNo], out htPointer[imgNo], out htType, out htWidth, out htHeight);

                    pPixOrg[imgNo] = (byte*)htPointer[imgNo].L;
                }

                double[] dMinimum = new double [imgCnt];
                double[] dMaximum = new double[imgCnt];
                for (int x = 0; x < (_imgWidth - 0); x++)
                {
                    yLineRegion.Dispose();
                    HOperatorSet.GenRectangle1(out yLineRegion, 0, x, _imgHeight - 1, x);

                    for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                    {
                        hoSelectedObj.Dispose();
                        HOperatorSet.SelectObj(backImg, out hoSelectedObj, imgNo + 1);
                        HOperatorSet.MinMaxGray(yLineRegion, hoSelectedObj, 0, out min, out max, out range);
                        dMinimum[imgNo] = min.D;
                        dMaximum[imgNo] = max.D;
                    }
                    if (dMinimum.Min() == 0.0 && dMaximum.Max() == 0.0)
                    {
                        for (int y = 0; y < (_imgHeight - 0); y++)
                        {
                            for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                            {
                                HOperatorSet.SetGrayval(hoConnectImage[imgNo], y, x, 128);
                            }
                        }
                    }
                }
                
                int maxVal;
                //HTuple grayVal;
                int[] gValue = new int[imgCnt];
                //
                maxVal = (_hanteiAkarui == true) ? int.MinValue : int.MaxValue;
                //_htHorz = new HTuple(_imgWidth);
                _htHorz = new int[_imgWidth];
                for (int x = 0; x < (_imgWidth - 0); x++)
                {
                    int val = (_hanteiAkarui == true) ? int.MinValue : int.MaxValue;
                    for (int y = 0; y < (_imgHeight - 0); y++)
                    {
                        for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                        {
                            pPix[imgNo] = pPixOrg[imgNo] + (_imgWidth * x) + y;
                            gValue[imgNo] = *pPix[imgNo];
                        }
                        int iMax = gValue.Max();
                        int iMin = gValue.Min();
                        if (_hanteiAkarui == true)
                        {
                            if (iMax > val)
                                val = iMax;
                        }
                        else
                        {
                            if (iMin < val)
                                val = iMin;
                        }
                    }
                    _htHorz[x] = val;
                    if (_hanteiAkarui == true)
                    {
                        if (val > maxVal)
                        {
                            _kando = val - 128;
                            maxVal = val;
                        }
                    }
                    else
                    {
                        if (val < maxVal)
                        {
                            _kando = 127 - val;
                            maxVal = val;
                        }
                    }
                }
                //
                //_htVert = new HTuple(_imgHeight);
                _htVert = new int[_imgHeight];
                for (int y = 0; y < (_imgHeight - 0); y++)
                {
                    int val = (_hanteiAkarui == true) ? int.MinValue : int.MaxValue;
                    for (int x = 0; x < (_imgWidth - 0); x++)
                    {
                        for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                        {
                            pPix[imgNo] = pPixOrg[imgNo] + (_imgWidth * x) + y;
                            gValue[imgNo] = *pPix[imgNo];
                        }
                        int iMax = gValue.Max();
                        int iMin = gValue.Min();
                        if (_hanteiAkarui == true)
                        {
                            if (iMax > val)
                                val = iMax;
                        }
                        else
                        {
                            if (iMin < val)
                                val = iMin;
                        }
                    }
                    _htVert[y] = val;
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                hoImage.Dispose();
                backImg.Dispose();
                yLineRegion.Dispose();
                hoSelectedObj.Dispose();
                for (int i = 0; i < imgCnt; i++)
                {
                    hoConnectImage[i].Dispose();
                }
            }
            return true;
        }
        /// <summary>
        /// 波形データ配列からXLDを生成
        /// </summary>
        /// <param name="img"></param>
        private void getProjection(HObject img)
        {
            if (chkEnableGraph.Checked == false)
                return;

            _hoXLDHorz = null;
            _hoXLDVert = null;

            if (img == null)
                return;
            HTuple htNum;
            HOperatorSet.CountObj(img, out htNum);
            if (htNum.I <= 0)
                return;

            int iLength;
            int[] adVert, adHorz;

            double dRateVer = _imgHeight / 255.0;
            double dBottom = _imgHeight;

            adVert = null;
            adHorz = null;
            adVert = _htVert;
            adHorz = new int[adVert.Length];
            iLength = adVert.Length;
            for (int i = 0; i < iLength; i++)
            {
                adVert[i] = (int)(adVert[i] * dRateVer);
                adHorz[i] = i;
            }
            HOperatorSet.GenContourPolygonXld(out _hoXLDHorz, adVert, adHorz);


            double dRateHor = _imgWidth / 255.0;
            double dRight = _imgWidth;

            adVert = null;
            adHorz = null;
            adHorz = _htHorz;
            adVert = new int[adHorz.Length];

            iLength = adHorz.Length;
            for (int i = 0; i < iLength; i++)
            {
                adHorz[i] = (int)(dRight - adHorz[i] * dRateHor);
                adVert[i] = i;
            }
            HOperatorSet.GenContourPolygonXld(out _hoXLDVert, adVert, adHorz);
        }
        /// <summary>
        /// 波形XLDデータで波形（Graph）描画
        /// </summary>
        private void drawProjection()
        {
            HTuple htChannel;

            try
            {
                Adjustment.clsHalconWindowControlLite.DispObj(_hoXLDHorz, hwndctrlImage, Adjustment.clsHalconWindowControlLite.EHalconColor.red);
                Adjustment.clsHalconWindowControlLite.DispObj(_hoXLDVert, hwndctrlImage, Adjustment.clsHalconWindowControlLite.EHalconColor.blue);

                HOperatorSet.SetColor(hwndctrlImage.HalconWindow, "green");
                HOperatorSet.SetFont(hwndctrlImage.HalconWindow, Fujita.HalconMisc.HalconExtFunc.GetFontFormat(Fujita.HalconMisc.HalconExtFunc.BaseFontName, 18));

                if (_hoImg != null)
                {
                    string s = string.Format("感度:" + _kando.ToString());
                    HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, 10, 10);
                    HOperatorSet.WriteString(hwndctrlImage.HalconWindow, s);

                    HOperatorSet.CountChannels(_hoImg, out htChannel);
                    if (htChannel.I == 1)
                    {
                        s = string.Format("カーソル位置-感度:{0}", (_grayValue >= 0) ? _grayValue : 0);
                        HOperatorSet.SetTposition(hwndctrlImage.HalconWindow, 10, _imgWidth - 120);
                        HOperatorSet.WriteString(hwndctrlImage.HalconWindow, s);
                    }
                }
            }
            catch (HOperatorException)
            {
            }
            finally
            {
            }
        }



        #region Map

        Color _colHeaderSheetInfo = Color.CadetBlue;
        public Color HeaderSheetInfoColor
        {
            set
            {
                if (_colHeaderSheetInfo == value)
                    return;
                _colHeaderSheetInfo = value;
                if (_brhHeaderSheetInfo != null)
                    _brhHeaderSheetInfo.Dispose();
                _brhHeaderSheetInfo = new SolidBrush(_colHeaderSheetInfo);
                repaint();
            }

            get
            {
                return _colHeaderSheetInfo;
            }
        }
        SolidBrush _brhHeaderSheetInfo = new SolidBrush(Color.CadetBlue);

        Color _colBackGroundColor = Color.Black;
        public Color BackGroundColor
        {
            get
            {
                return _colBackGroundColor;
            }
            set
            {
                if (_colBackGroundColor == value)
                    return;
                _colBackGroundColor = value;
                if (_brhBackGround != null)
                    _brhBackGround.Dispose();
                _brhBackGround = new SolidBrush(_colBackGroundColor);
                repaint();
            }
        }
        SolidBrush _brhBackGround = new SolidBrush(Color.Black);

        private Bitmap _bmpBG;
        private void initMap()
        {
            _bmpBG = new Bitmap(picMap.Size.Width, picMap.Size.Height);
            picMap.Image = _bmpBG;

            _penLine1 = new Pen(Brushes.White, 1.0f);
        }

        private void repaint()
        {
            drawMap();
            picMap.Refresh();
        }

        private void drawMap()
        {
            Graphics g = Graphics.FromImage(_bmpBG);

            clearScreen(g);

            drawHeader(g);

            drawTips(g);
        }


        private void clearScreen(Graphics g)
        {
            g.FillRectangle(_brhBackGround, 0, 0, _bmpBG.Width, _bmpBG.Height);
        }

        private float getSheetLeft()
        {
            return _fOffsetLeft + _iSokucyouColumnWidth;
        }

        private PointF center2LT(PointF ptCenter, SizeF szSize)
        {
            return new PointF(ptCenter.X - szSize.Width / 2.0f, ptCenter.Y - szSize.Height / 2.0f);
        }

        Font _fntHeaderDisp = new Font("ＭＳ ゴシック", 10);
        Pen _penLine1;
        float _fOffsetLeft = 0;
        float _fOffsetRight = 0;
        float _fOffsetTop = 0;
        int _iHeaderTopHeight = 0;
        int _iHeaderSheetInfoHeight = 24;
        int _iSokucyouColumnWidth = 0;
        private double _dSheetWidth = 800;
        private double[] _adZones = new double[0];
        Color _tipColor;
        double _tipAddress;
        private void drawHeader(Graphics g)
        {
            // シート情報表示
            g.FillRectangle(_brhHeaderSheetInfo, getSheetLeft(), _fOffsetTop + _iHeaderTopHeight, _bmpBG.Width - _iSokucyouColumnWidth - _fOffsetLeft - _fOffsetRight, _iHeaderSheetInfoHeight);

            double fSheetSizePix = _bmpBG.Width - _fOffsetLeft - _fOffsetRight - _iSokucyouColumnWidth;
            // ゾーンとｻｲｽﾞの計算
            if (_dSheetWidth != 0.0)
            {
                _penLine1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                float[] afZonePix = new float[_adZones.Length];
                for (int i = 0; i < _adZones.Length; i++)
                {
                    afZonePix[i] = (float)(_adZones[i] / _dSheetWidth * fSheetSizePix);
                }

                // ゾーン位置に対して、数字の描画
                float fHorZoneNow = _fOffsetLeft + _iSokucyouColumnWidth;
                for (int i = 0; i <= _adZones.Length; i++)
                {
                    if (i != _adZones.Length)
                        g.DrawString((i + 1).ToString(), _fntHeaderDisp, Brushes.Black, center2LT(new PointF(fHorZoneNow + afZonePix[i] / 2.0f, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight * 3.0f / 4.0f), g.MeasureString((i + 1).ToString(), _fntHeaderDisp)));
                    if (i != 0)
                        g.DrawLine(_penLine1, fHorZoneNow, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight, fHorZoneNow, _bmpBG.Height);
                    if (i != _adZones.Length)
                        fHorZoneNow += afZonePix[i];
                }
            }
        }

        private void drawTips(Graphics g)
        {
            PointF ptDevice;
            getSheetRealToDevice(_tipAddress, _iHeaderSheetInfoHeight + 10, out ptDevice);

            g.FillRectangle(new SolidBrush(_tipColor), new RectangleF(ptDevice, new SizeF(10,10)));
        }
        private bool getSheetRealToDevice(double dRealX, double dRealY, out PointF ptDevice)
        {
            ptDevice = new PointF();

            // ミリ->メートル変換
            //            dRealY /= 1000.0;

            // マップ左上
            float fTopX = _fOffsetLeft + _iSokucyouColumnWidth;
            float fTopY = _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight;

            double fWidth = _bmpBG.Width - _fOffsetLeft - _iSokucyouColumnWidth - _fOffsetRight;

            double fWidthRate = fWidth / _dSheetWidth;

            ptDevice.X = (float)(dRealX * fWidthRate) + fTopX;
            ptDevice.Y = (float)dRealY;

            if (ptDevice.X >= (picMap.Size.Width - 5))
                ptDevice.X = picMap.Size.Width - 5;

            return true;
        }

        #endregion

        private void frmNg1Image_Paint(object sender, PaintEventArgs e)
        {
            drawMap();
        }

        private void chkEnableBaseImage_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            if (_autoInspectionMode == false)
                SystemParam.GetInstance().OneNGImageBaseImageEnable = chkEnableBaseImage.Checked;

            double[] bufZones = new double[_adZones.Length];
            _adZones.CopyTo(bufZones, 0);
            SetNgData(_resDatas[_dispPosition], _dSheetWidth, bufZones);
            initMap();
            drawMap();
            picMap.Refresh();
            _hWndCtrl.repaint();
            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }
        public void RefreshWindow()
        {
            initMap();
            drawMap();
            picMap.Refresh();
            _hWndCtrl.repaint();
        }

        /// <summary>NGポップアップ画面を閉じる。　v1338のPC電源ボタン押下対応</summary>//v1338 yuasa
        public void frmNg1ImageClose()
        {
            this.Close();
        }
    }
}
