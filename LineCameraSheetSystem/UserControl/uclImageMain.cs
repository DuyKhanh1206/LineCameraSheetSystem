using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using ViewROI;
using System.Globalization;
using InspectionNameSpace;
using HalconCamera;

namespace LineCameraSheetSystem
{
    public partial class uclImageMain : UserControl
    {
        /// <summary>
        /// MainForm
        /// </summary>
        MainForm _mainForm { get; set; }
        /// <summary>
        /// Image Window
        /// </summary>
        HWndCtrl[] _hwnCtrl;
        /// <summary>
        /// 倍率
        /// </summary>
        ComboBox[] _cmbMagnify;
        /// <summary>
        /// 
        /// </summary>
        HScrollBar[] _horScroll;
        /// <summary>
        /// 
        /// </summary>
        VScrollBar[] _verScroll;
        /// <summary>
        /// 感度ラベル
        /// </summary>
        Label[] _lblKando;

        CheckBox[] _chkBoxDisplayMode;

        /// <summary>
        /// 表示モード(0:検査画像 1:取込画像)
        /// </summary>
        int _displayMode;
        /// <summary>
        /// 表示対象画像(0:Original 1:Gray 2:Red 3:Green 4:Blue)
        /// </summary>
        public int _displayTargetImage;

        /// <summary>
        /// 倍率リスト
        /// </summary>
        private double[] _magnifyList = new double[] { 0.05, 0.06, 0.07, 0.08, 0.09, 0.1, 0.12, 0.13, 0.14, 0.15, 0.16, 0.17, 0.18, 0.2, 0.35, 0.4, 0.6, 0.8, 1.0, 2.0, 4.0 };

        private bool _setPartsFlag = false;
        private bool _setMagnifyFlag = false;
        private int _displayNo = 0;

        /// <summary>
        /// 元画像（加工されていない）
        /// </summary>
        public List<HObject> _imageOrgOmote = new List<HObject>();
        public List<HObject> _imageOrgUra = new List<HObject>();
        /// <summary>
        /// 検査時の画像（加工されている）
        /// </summary>
        private List<HObject> _imageTargetOmote = new List<HObject>();
        private List<HObject> _imageTargetUra = new List<HObject>();
        /// <summary>
        /// 
        /// </summary>
        private List<HObject> _imageInspScaleOmote = new List<HObject>();
        private List<HObject> _imageInspScaleUra = new List<HObject>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public uclImageMain()
        {
            InitializeComponent();

            SetLayout();

            //Disposeイベント登録
            this.Disposed += UclImageMain_Disposed;
        }

        /// <summary>
        /// 後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UclImageMain_Disposed(object sender, EventArgs e)
        {
            SystemParam.GetInstance().IM_AutoSaveEnable = _bBackAutoSaveEnable;
            ClearImage();
        }

        /// <summary>
        /// SetMainForm
        /// </summary>
        /// <param name="_mf"></param>
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;

            SystemParam sysp = SystemParam.GetInstance();

            //Image Window
            _hwnCtrl = new HWndCtrl[] { new HWndCtrl(hwcImageWnd1), new HWndCtrl(hwcImageWnd2) };
            _cmbMagnify = new ComboBox[] { cmbMagnify1, cmbMagnify2 };
            _horScroll = new HScrollBar[] { hScrollBar1, hScrollBar2 };
            _verScroll = new VScrollBar[] { vScrollBar1, vScrollBar2 };
            _lblKando = new Label[] { lblKandoOmote, lblKandoUra };

            grpTitleUra.Visible = sysp.DownSideEnable;
            lblTitleUra.Visible = sysp.DownSideEnable;
            _lblKando[1].Visible = sysp.DownSideEnable;
            lblInspStatusUra.Visible = sysp.DownSideEnable;
            hwcImageWnd2.Visible = sysp.DownSideEnable;
            uclKandoControl2.Visible = sysp.DownSideEnable;

            chkVerDisplayMode.Visible = sysp.DownSideEnable;

            _setMagnifyFlag = true;
            int no = 0;
            foreach (HWndCtrl hw in _hwnCtrl)
            {
                hw.Fitting = false;
                hw.SetScrollbarControl(_horScroll[no], _verScroll[no]);
                hw.useGraphManager(new GraphicsManager());
                hw.FirstTimeFitting = true;
                hw.SetMagnifyList(_magnifyList);
                hw.SetMagnifyComboControl(_cmbMagnify[no]);
                hw.SetBackColor();
                no++;
            }
            chkZoomEnable.Checked = true;
            _setMagnifyFlag = false;

            SetSystemParam();

            _iPrevLeft = new int[] { -1, -1 };
            _iPrevRight = new int[] { -1, -1 };
            _iPrevLeftOffset = new int[] { -1, -1 };
            _iPrevRightOffset = new int[] { -1, -1 };
            _iCropHeightSize = new int[] { 0, 0 };

            _chkBoxDisplayMode = new CheckBox[] { chkDisplayModeTarget, chkDisplayModeBase };

            for (int i = 0; i < _chkBoxDisplayMode.Length; i++)
                _chkBoxDisplayMode[i].Tag = i;

            _displayMode = 0;
            _displayTargetImage = 0;

            _lstKandoCtrl = new uclKandoControl[] { uclKandoControl1, uclKandoControl2 };
            for (int i = 0; i < _lstKandoCtrl.Length; i++)
                _lstKandoCtrl[i].SetMainForm(_mf);

            ResetKandoMessage();
            SetRecipeFile2Disp();
            SetRecipeFile2Buffer();
            DisableKandoParts();
        }

        private void SetSystemParam()
        {
            SystemParam sysp = SystemParam.GetInstance();

            _setPartsFlag = true;

            chkVerDisplayMode.Checked = sysp.IM_VerDisplayMode;
            spinImageDispNo.Maximum = sysp.IM_ImageBufferCount;
            spinImageDispNo.Value = 1;
            spinImageDispNo.Enabled = false;
            chkPause.Checked = false;

            spinNgHistoryNumber.Maximum = SystemParam.GetInstance().NGHistoryCount;
            spinNgHistoryNumber.Value = 1;
            spinNgHistoryNumber.Enabled = false;
            chkNgHistory.Checked = false;

            if (_frmImageMainParam != null)
                _frmImageMainParam.SetSystemParam();

            _setPartsFlag = false;
        }


        #region イメージ操作（追加[FiFo]・取得・クリア）
        private void AddImage(HObject[] imageOrg, HObject[] imageTarget, HObject[] imageInspScale)
        {
            SystemParam sysp = SystemParam.GetInstance();
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                List<HObject> __imageOrg = (side == 0) ? _imageOrgOmote : _imageOrgUra;
                List<HObject> __imageTarget = (side == 0) ? _imageTargetOmote : _imageTargetUra;
                List<HObject> __imageInspScale = (side == 0) ? _imageInspScaleOmote : _imageInspScaleUra;
                if (imageOrg[side] != null)
                {
                    lock (imageOrg[side])
                    {
                        __imageOrg.Insert(0, imageOrg[side]);
                        __imageTarget.Insert(0, imageTarget[side]);
                        __imageInspScale.Insert(0, imageInspScale[side]);
                        if (sysp.IM_ImageBufferCount < __imageOrg.Count)
                        {
                            int delImgNo = __imageOrg.Count - 1;
                            if (__imageOrg[delImgNo] != null)
                            {
                                __imageOrg[delImgNo].Dispose();
                                __imageTarget[delImgNo].Dispose();
                                __imageInspScale[delImgNo].Dispose();
                            }

                            __imageOrg.RemoveAt(delImgNo);
                            __imageTarget.RemoveAt(delImgNo);
                            __imageInspScale.RemoveAt(delImgNo);
                        }
                    }
                }
            }
        }
        private void GetImage(int side, int getImgNo, out HObject imageOrg, out HObject imageTarget, out HObject imageInspScale)
        {
            imageOrg = null;
            imageTarget = null;
            imageInspScale = null;

            int iCnt = 0;

            List<HObject> __imageOrg = (side == 0) ? _imageOrgOmote : _imageOrgUra;
            List<HObject> __imageTarget = (side == 0) ? _imageTargetOmote : _imageTargetUra;
            List<HObject> __imageInspScale = (side == 0) ? _imageInspScaleOmote : _imageInspScaleUra;

            lock (__imageOrg)
            {
                if (getImgNo < __imageOrg.Count)
                {
                    for (int i = 0; i < __imageOrg.Count; i++)
                    {
                        if (__imageOrg[i] != null)
                            iCnt++;
                        if (iCnt == (getImgNo + 1))
                        {
                            HOperatorSet.CopyObj(__imageOrg[i], out imageOrg, 1, -1);
                            HOperatorSet.CopyObj(__imageTarget[i], out imageTarget, 1, -1);
                            HOperatorSet.CopyObj(__imageInspScale[i], out imageInspScale, 1, -1);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearImage()
        {
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                List<HObject> __imageOrg = (side == 0) ? _imageOrgOmote : _imageOrgUra;
                List<HObject> __imageTarget = (side == 0) ? _imageTargetOmote : _imageTargetUra;
                List<HObject> __imageInspScale = (side == 0) ? _imageInspScaleOmote : _imageInspScaleUra;

                lock (__imageOrg)
                {
                    for (int imgNo = 0; imgNo < __imageOrg.Count; imgNo++)
                    {
                        if (__imageOrg[imgNo] != null)
                            __imageOrg[imgNo].Dispose();
                    }
                    __imageOrg.Clear();
                }
                lock (__imageTarget)
                {
                    for (int imgNo = 0; imgNo < __imageTarget.Count; imgNo++)
                    {
                        if (__imageTarget[imgNo] != null)
                            __imageTarget[imgNo].Dispose();
                    }
                    __imageTarget.Clear();
                }
                lock (__imageInspScale)
                {
                    for (int imgNo = 0; imgNo < __imageInspScale.Count; imgNo++)
                    {
                        if (__imageInspScale[imgNo] != null)
                            __imageInspScale[imgNo].Dispose();
                    }
                    __imageInspScale.Clear();
                }
            }
        }
        #endregion


        #region イメージ描画
        bool _bFittingEnable = false;
        /// <summary>
        /// バッファ画像の表示
        /// </summary>
        public void refreshImageBuffer()
        {
            if (chkPause.Checked == false && chkNgHistory.Checked == false)
                return;
            refreshImage();
        }
        /// <summary>
        /// 描画処理
        /// </summary>
        private void refreshImage()
        {
            SystemParam sysp = SystemParam.GetInstance();

            HObject hoImageOrg = null;
            HObject hoImageTarget = null;
            HObject hoImageInspScale = null;

            HObject hoDisplayImage1;
            HObject hoDisplayImage2;
            HObject hoGray;
            HObject hoImage1, hoImage2, hoImage3;
            HObject selectedGraphImage;
            HObject createGraphImage;
            HObject cutGraphImage;
            HObject dmy1, dmy2;
            HOperatorSet.GenEmptyObj(out hoDisplayImage1);
            HOperatorSet.GenEmptyObj(out hoDisplayImage2);
            HOperatorSet.GenEmptyObj(out hoGray);
            HOperatorSet.GenEmptyObj(out hoImage1);
            HOperatorSet.GenEmptyObj(out hoImage2);
            HOperatorSet.GenEmptyObj(out hoImage3);
            HOperatorSet.GenEmptyObj(out selectedGraphImage);
            HOperatorSet.GenEmptyObj(out createGraphImage);
            HOperatorSet.GenEmptyObj(out cutGraphImage);
            HOperatorSet.GenEmptyObj(out dmy1);
            HOperatorSet.GenEmptyObj(out dmy2);

            HTuple htCountObj;
            HTuple htCountCannels;

            try
            {
                lblNgHistorySide.Text = "-";
                lblNgHistoryTime.Text = "-";
                for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
                {
                    if (hoImageOrg != null)
                    {
                        hoImageOrg.Dispose();
                        hoImageTarget.Dispose();
                        hoImageInspScale.Dispose();
                    }

                    if (chkNgHistory.Checked == false)
                    {
                        GetImage(side, _displayNo, out hoImageOrg, out hoImageTarget, out hoImageInspScale);
                        //モノクロカメラ：CountObj=1,1,1　ChannelCount=1,1,1
                        //カラー　カメラ：CountObj=1,1,4　ChannelCount=3,3,1
                    }
                    else
                    {
                        hoImageOrg = null;
                        hoImageTarget = null;
                        hoImageInspScale = null;
                        int imgno = (int)spinNgHistoryNumber.Value - 1;
                        EventMonitor.NgHistoryData res = _mainForm.AutoInspection.EventMonitor.GetNgHistoryData(imgno);
                        if (res != null)
                        {
                            if (res.CamNo == side)
                            {
                                HOperatorSet.CopyObj(res.ImageOrgs, out hoImageOrg, 1, -1);
                                HOperatorSet.CopyObj(res.ImageTargets, out hoImageTarget, 1, -1);
                                HOperatorSet.CopyObj(res.ImageInspScales, out hoImageInspScale, 1, -1);
                                lblNgHistorySide.Text = (side == 0) ? "表" : "裏";
                                //HTuple htMSecond, htSecond, htMinute, htHour, htDay, htYDay, htMonth, htYear;
                                //HOperatorSet.GetImageTime(res.ImageOrgs, out htMSecond, out htSecond, out htMinute, out htHour, out htDay, out htYDay, out htMonth, out htYear);
                                //lblNgHistoryTime.Text = htHour.I.ToString("D02") + ":" + htMinute.I.ToString("D02") + ":" + htSecond.I.ToString("D02") + ":" + htMSecond.I.ToString("D03");
                                lblNgHistoryTime.Text = _mainForm.AutoInspection.EventMonitor.GetNgHistoryDataTime(imgno);
                            }
                            else
                            {
                                ClearProjection(_hwnCtrl[side]);
                                _hwnCtrl[side].addIconicVar2(null);
                                if (_bFittingEnable)
                                    _hwnCtrl[side].FittingImage(true);
                            }
                        }
                    }
                    if (hoImageTarget == null || hoImageOrg == null || hoImageInspScale == null)
                        continue;

                    hoDisplayImage1.Dispose();
                    if (_displayMode == 0)
                    {
                        //シェーディングされた画像
                        if (_displayTargetImage == 0)
                        {
                            //カラー画像
                            HOperatorSet.CopyObj(hoImageTarget, out hoDisplayImage1, 1, -1);
                        }
                        else
                        {
                            HOperatorSet.CountObj(hoImageInspScale, out htCountObj);
                            //検査画像（シェーディングされたモノクロ画像）
                            if (htCountObj.I > 1)
                            {
                                hoGray.Dispose();
                                hoImage1.Dispose();
                                hoImage2.Dispose();
                                hoImage3.Dispose();
                                if (_displayTargetImage == 0)
                                {
                                    HOperatorSet.SelectObj(hoImageInspScale, out hoImage1, 2);
                                    HOperatorSet.SelectObj(hoImageInspScale, out hoImage2, 3);
                                    HOperatorSet.SelectObj(hoImageInspScale, out hoImage3, 4);
                                    HOperatorSet.Compose3(hoImage1, hoImage2, hoImage3, out hoDisplayImage1);
                                }
                                else
                                {
                                    HOperatorSet.CopyObj(hoImageInspScale, out hoDisplayImage1, _displayTargetImage, 1);
                                }
                            }
                            else
                            {
                                //モノクロ
                                HOperatorSet.CopyObj(hoImageInspScale, out hoDisplayImage1, 1, -1);
                            }
                        }
                    }
                    else
                    {
                        //取り込み画像（生画像）
                        if (_displayTargetImage == 0)
                        {
                            HOperatorSet.CopyObj(hoImageOrg, out hoDisplayImage1, 1, -1);
                        }
                        else if (_displayTargetImage == 1)
                        {
                            HOperatorSet.Rgb1ToGray(hoImageOrg, out hoDisplayImage1);
                        }
                        else
                        {
                            hoImage1.Dispose();
                            hoImage2.Dispose();
                            hoImage3.Dispose();
                            HOperatorSet.Decompose3(hoImageOrg, out hoImage1, out hoImage2, out hoImage3);
                            if (_displayTargetImage == 2)
                                HOperatorSet.CopyObj(hoImage1, out hoDisplayImage1, 1, -1);
                            else if (_displayTargetImage == 3)
                                HOperatorSet.CopyObj(hoImage2, out hoDisplayImage1, 1, -1);
                            else
                                HOperatorSet.CopyObj(hoImage3, out hoDisplayImage1, 1, -1);
                        }
                    }

                    HOperatorSet.CountChannels(hoImageOrg, out htCountCannels);
                    if (_displayMode == 1 && htCountCannels.I > 1)
                    {
                        hoGray.Dispose();
                        HOperatorSet.Rgb1ToGray(hoImageOrg, out hoGray);
                        hoImage1.Dispose();
                        hoImage2.Dispose();
                        hoImage3.Dispose();
                        HOperatorSet.Decompose3(hoImageOrg, out hoImage1, out hoImage2, out hoImage3);
                        selectedGraphImage.Dispose();
                        dmy1.Dispose();
                        dmy2.Dispose();
                        HOperatorSet.GenEmptyObj(out selectedGraphImage);
                        HOperatorSet.ConcatObj(hoGray, hoImage1, out dmy1);
                        HOperatorSet.ConcatObj(dmy1, hoImage2, out dmy2);
                        HOperatorSet.ConcatObj(dmy2, hoImage3, out selectedGraphImage);
                    }
                    else
                    {
                        HOperatorSet.CopyObj(hoImageInspScale, out selectedGraphImage, 1, -1);
                    }

                    //連結イメージ表示？
                    hoDisplayImage2.Dispose();
                    cutGraphImage.Dispose();
                    //部分を切り出す
                    CutOverlapImage(hoDisplayImage1, out hoDisplayImage2);
                    CutOverlapImage(selectedGraphImage, out cutGraphImage);

                    //表示する画像をセットする
                    _hwnCtrl[side].addIconicVar2(hoDisplayImage2);
                    if (_bFittingEnable)
                        _hwnCtrl[side].FittingImage(true);

                    //イメージサイズ
                    HTuple htWidth, htHeight;
                    HOperatorSet.GetImageSize(hoDisplayImage2, out htWidth, out htHeight);
                    int imgWidth = htWidth.I;
                    int imgHeight = htHeight.I;

                    //グラフ描画
                    HTuple htMin, htMax, htAvg;
                    HTuple htCalcMin, htCalcMax, htCalcAvg;
                    HTuple htChannes;
                    HOperatorSet.CountChannels(hoDisplayImage2, out htChannes);
                    createGraphImage.Dispose();
                    if (htChannes.I == 1)
                    {
                        PROJ_WIDTH = sysp.IM_DispGraphWidth1ch;
                        HOperatorSet.CopyObj(hoDisplayImage2, out createGraphImage, 1, -1);
                    }
                    else
                    {
                        PROJ_WIDTH = sysp.IM_DispGraphWidth3ch;
                        HOperatorSet.CopyObj(cutGraphImage, out createGraphImage, 1, -1);
                    }

                    int maskLeft, maskRight;
                    bool ret = ImgProjection(side, createGraphImage, imgWidth, imgHeight, out htMin, out htMax, out htAvg, out htCalcMin, out htCalcMax, out htCalcAvg, out maskLeft, out maskRight);
                    if (ret == true)
                    {
                        KandoMin[side] = (int)htCalcMin.ToDArr().Min();
                        KandoMax[side] = (int)htCalcMax.ToDArr().Max();
                        KandoAve[side] = (int)htCalcAvg.ToDArr().Average();
                        KandoLightValue[side] = KandoMax[side] - KandoLightBase[side];
                        KandoLightValue[side] = (KandoLightValue[side] < 0) ? -1 : KandoLightValue[side];
                        KandoDarkValue[side] = KandoDarkBase[side] - KandoMin[side];
                        KandoDarkValue[side] = (KandoDarkValue[side] < 0) ? -1 : KandoDarkValue[side];
                    }

                    if (Recipe.GetInstance().InspParam.Count != 0)
                    {
                        //検査高さ範囲Line
                        drawInspHeightLine(side, imgWidth, imgHeight);

                        //検査幅Line
                        drawInspWidthLine(side);

                        //マスクLine
                        drawMaskLine(side);

                        //感度基準Line
                        drawBaseKandoLine(side, imgWidth, imgHeight);

                        //感度Line
                        drawKandoLine(side, imgWidth, imgHeight);

                        //検査幅Line・マスクLineの描画位置を算出する
                        _iCropHeightSize[side] = htHeight.I;
                        int[] iLeft, iRight;
                        iLeft = new int[Enum.GetNames(typeof(AppData.SideID)).Length];
                        iRight = new int[Enum.GetNames(typeof(AppData.SideID)).Length];
                        _mainForm.AutoInspection.GetMinMaxAveCalcRangePix((side == 0) ? AppData.SideID.表 : AppData.SideID.裏, out iLeft[side], out iRight[side]);
                        if (iLeft[side] <= 0) iLeft[side] = 1;
                        if (iLeft[side] > htWidth.I) iLeft[side] = htWidth.I;
                        if (iRight[side] <= 0) iRight[side] = 1;
                        if (iRight[side] > htWidth.I) iRight[side] = htWidth.I;
                        _iPrevLeft[side] = iLeft[side];
                        _iPrevRight[side] = iRight[side];
                        double resH = SystemParam.GetInstance().camParam[0].ResoH;
                        if (side != 0)
                        {
                            int index = SystemParam.GetInstance().camParam.FindIndex(x => x.CamParts == AppData.SideID.裏);
                            if (index > 0)
                                resH = SystemParam.GetInstance().camParam[index].ResoH;
                        }

                        double MaskWidth;
                        if (Recipe.GetInstance().CommonInspAreaEnable == false)
                        {
                            MaskWidth = Recipe.GetInstance().InspParam[side].MaskWidth;
                        }
                        else
                        {
                            MaskWidth = SystemParam.GetInstance().InspArea_CmnMaskWidth[side];
                        }

                        int maskWidthPix = (int)(MaskWidth / resH);
                        _iPrevLeftOffset[side] = _iPrevLeft[side] + maskWidthPix;
                        _iPrevRightOffset[side] = _iPrevRight[side] - maskWidthPix;
                    }

                    if (ret == true)
                    {
                        drawProjection(imgHeight,
                            (_iPrevLeft[side] < maskLeft) ? maskLeft : _iPrevLeft[side],
                            (maskRight < _iPrevRight[side]) ? maskRight : _iPrevRight[side], htMin, htMax, htAvg, _hwnCtrl[side]);
                    }

                    _hwnCtrl[side].repaint();

                    //感度文字列
                    string MeiValue = "明" + ((KandoLightValue[side] < 0) ? "---" : KandoLightValue[side].ToString("D03"));
                    string AnnValue = "暗" + ((KandoDarkValue[side] < 0) ? "---" : KandoDarkValue[side].ToString("D03"));
                    string KandoStr = "";
                    if (SystemParam.GetInstance().InspBrightEnable == true)
                        KandoStr = MeiValue;
                    if (SystemParam.GetInstance().InspDarkEnable == true)
                    {
                        if (KandoStr != "")
                            KandoStr += " ";
                        KandoStr += AnnValue;
                    }
                    if (KandoStr != null && KandoStr != "")
                        _lblKando[side].Text = KandoStr;
                }
                _bFittingEnable = false;
            }
            catch (Exception oe)
            {
                LogingDllWrap.LogingDll.Loging_SetLogString(oe.Message);
            }
            finally
            {
                hoDisplayImage1.Dispose();
                hoDisplayImage2.Dispose();

                if (hoImageOrg != null)
                    hoImageOrg.Dispose();

                if (hoImageTarget != null)
                    hoImageTarget.Dispose();

                if (hoImageInspScale != null)
                    hoImageInspScale.Dispose();

                hoGray.Dispose();
                hoImage1.Dispose();
                hoImage2.Dispose();
                hoImage3.Dispose();
                selectedGraphImage.Dispose();
                createGraphImage.Dispose();
                cutGraphImage.Dispose();
                dmy1.Dispose();
                dmy2.Dispose();
            }
        }

        public int[] KandoLightBase = new int[Enum.GetNames(typeof(AppData.SideID)).Length];
        public int[] KandoDarkBase = new int[Enum.GetNames(typeof(AppData.SideID)).Length];

        public int[] KandoMin = new int[Enum.GetNames(typeof( AppData.SideID)).Length];
        public int[] KandoMax = new int[Enum.GetNames(typeof(AppData.SideID)).Length];
        public int[] KandoAve = new int[Enum.GetNames(typeof(AppData.SideID)).Length];

        public int[] KandoLightValue = new int[Enum.GetNames(typeof(AppData.SideID)).Length];
        public int[] KandoDarkValue = new int[Enum.GetNames(typeof(AppData.SideID)).Length];

        /// <summary>
        /// 外部スレッドからの　描画イベント
        /// </summary>
        /// <param name="imageOrg"></param>
        /// <param name="imageTarget"></param>
        /// <param name="kando"></param>
        public void RefreshImage(bool[] updown, HObject[] imageOrg, HObject[] imageTarget, HObject[] imageInspScale)
        {
            Action act = new Action(() =>
            {
                //中断中？
                if (chkPause.Checked == true)
                    return;
                if (chkNgHistory.Checked == true)
                    return;

                //格納領域の確保
                HObject[] hoImageOrg = new HObject[imageOrg.Length];
                HObject[] hoImageTarget = new HObject[imageTarget.Length];
                HObject[] hoImageInspScale = new HObject[imageInspScale.Length];

                HTuple htChCnt;
                int chCnt;
                HObject tarImg = imageTarget[0];
                if (imageTarget[0] == null)
                    tarImg = imageTarget[1];
                HOperatorSet.CountChannels(tarImg, out htChCnt);
                chCnt = htChCnt.I;
                for (int i = 0; i < imageOrg.Length; i++)
                {
                    if (updown[i] == true)
                    {
                        if (imageOrg[i] != null && imageTarget[i] != null && imageInspScale[i] != null)
                        {
                            HOperatorSet.CopyObj(imageOrg[i], out hoImageOrg[i], 1, -1);
                            HOperatorSet.CopyObj(imageTarget[i], out hoImageTarget[i], 1, -1);
                            HOperatorSet.CopyObj(imageInspScale[i], out hoImageInspScale[i], 1, -1);
                        }
                    }
                    else
                    {
                        hoImageOrg[i] = null;
                        hoImageTarget[i] = null;
                        hoImageInspScale[i] = null;
                    }
                }
                //イメージ保持
                AddImage(hoImageOrg, hoImageTarget, hoImageInspScale);
                //描画
                _displayNo = 0;
                refreshImage();
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
            return;
        }
        #endregion


        #region 自動画像保存
        private Thread _thAutoSave = null;
        private string _autoSavePath;
        private int _autoSaveSideNo;
        private HObject[] _autoSaveImage;
        /// <summary>
        /// NG発生時に画像を自動保存する
        /// </summary>
        public void AutoSave(int side)
        {
            //保存中？
            if (_thAutoSave != null)
                return;
            //保存するモード？
            if (SystemParam.GetInstance().IM_AutoSaveEnable == false)
                return;

            HObject hoCutImage;
            HOperatorSet.GenEmptyObj(out hoCutImage);

            int saveCnt = SystemParam.GetInstance().IM_AutoSaveOneNGsaveCount;
            try
            {
                if (_autoSaveImage != null)
                {
                    for (int i = 0; i < _autoSaveImage.Length; i++)
                        if (_autoSaveImage[i] != null)
                            _autoSaveImage[i].Dispose();
                }
                _autoSaveImage = new HObject[saveCnt];

                List<HObject> __imageOrg = (side == 0) ? _imageOrgOmote : _imageOrgUra;

                lock (__imageOrg)
                {
                    int top, bottom;
                    int counter = 0;
                    top = bottom = -1;
                    bool exist = false;
                    int firstNo = saveCnt - 1;/*1NGで保存する数(１NG＝５枚）*/
                    for (int i = firstNo; i < __imageOrg.Count; i++)
                    {
                        counter = 0;
                        for (int j = i; j >= 0; j--)
                        {
                            if (__imageOrg[j] != null)
                                counter++;
                            if (counter == saveCnt)
                            {
                                top = i;
                                bottom = j;
                                exist = true;
                                break;
                            }
                        }
                        if (exist == true)
                            break;
                    }
                    if (exist == false)
                        return;
                    int iNumber = 0;
                    for (int i = top; i >= bottom; i--)
                    {
                        if (__imageOrg[i] == null)
                            continue;
                        if (iNumber < saveCnt)
                        {
                            hoCutImage.Dispose();
                            CutOverlapImage(__imageOrg[i], out hoCutImage);
                            HOperatorSet.CopyObj(hoCutImage, out _autoSaveImage[iNumber], 1, -1);
                            iNumber++;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("#############################################");
                }
            }
            catch(HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoCutImage.Dispose();
            }

            _autoSavePath = _mainForm.AutoInspection.IniAccess.BmpSaveDir;
            _autoSaveSideNo = side;
            _thAutoSave = new Thread(new ThreadStart(ThreadMain));
            _thAutoSave.Start();
        }
        private void ThreadMain()
        {
            string fName;
            string fPath;

            HTuple htMSecond, htSecond, htMinute, htHour, htDay, htYDay, htMonth, htYear;

            try
            {
                SystemParam sysp = SystemParam.GetInstance();

                string sideStr = System.IO.Path.Combine(_autoSavePath, _autoSaveSideNo.ToString());
                if (System.IO.Directory.Exists(sideStr) == false)
                    System.IO.Directory.CreateDirectory(sideStr);

                string dateStr = DateTime.Now.ToString("yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo);
                string sDir = System.IO.Path.Combine(sideStr, dateStr);
                if (System.IO.Directory.Exists(sDir) == false)
                    System.IO.Directory.CreateDirectory(sDir);

                for (int imgNo = 0; imgNo < _autoSaveImage.Length; imgNo++)
                {
                    if (_autoSaveImage[imgNo] == null)
                        continue;

                    HOperatorSet.GetImageTime(_autoSaveImage[imgNo], out htMSecond, out htSecond, out htMinute, out htHour, out htDay, out htYDay, out htMonth, out htYear);
                    fName = dateStr + "_"
                        + _autoSaveSideNo.ToString() + "_" + imgNo.ToString("D2") + "_"
                        + string.Format("{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}_{6:D3}.bmp", htYear.I, htMonth.I, htDay.I, htHour.I, htMinute.I, htSecond.I, htMSecond.I);
                    fPath = System.IO.Path.Combine(sDir, fName);

                    HOperatorSet.WriteImage(_autoSaveImage[imgNo], "bmp", 0, fPath);
                }

                string[] dirNames = System.IO.Directory.GetDirectories(sideStr);
                Array.Sort(dirNames);
                int existDirCount = dirNames.Length;
                if (sysp.IM_AutoSaveCount < existDirCount)
                {
                    int delCnt = existDirCount - sysp.IM_AutoSaveCount;
                    for (int i = 0; i < delCnt; i++)
                    {
                        System.IO.Directory.Delete(dirNames[i], true);
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception)
            {
                //throw exc;
            }
            finally
            {
                for (int i = 0; i < _autoSaveImage.Length; i++)
                {
                    if (_autoSaveImage[i] != null)
                    {
                        _autoSaveImage[i].Dispose();
                        _autoSaveImage[i] = null;
                    }
                }
                _thAutoSave = null;
            }
        }
        #endregion

        /// <summary>
        /// イメージ中央部を取得する（元画像[取込画像]にする）
        /// </summary>
        /// <param name="inImage"></param>
        /// <param name="outCutImage"></param>
        public void CutOverlapImage(HObject inImage, out HObject outCutImage)
        {
            HOperatorSet.GenEmptyObj(out outCutImage);
            CameraParam camp = SystemParam.GetInstance().camParam[0];
            try
            {
                HOperatorSet.CopyObj(inImage, out outCutImage, 1, -1);
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
            }
        }



        #region グラフ描画
        private int PROJ_WIDTH = 16;
        private bool ImgProjection(int side, HObject img, int imgWidth, int imgHeight, out HTuple htMin, out HTuple htMax, out HTuple htAvg, out HTuple htCalcMin, out HTuple htCalcMax, out HTuple htCalcAvg, out int maskLeft, out int maskRight)
        {
            htMin = null;
            htMax = null;
            htAvg = null;
            htCalcMin = null;
            htCalcMax = null;
            htCalcAvg = null;

            maskLeft = 0;
            maskRight = imgWidth;

            SystemParam sysp = SystemParam.GetInstance();

            if (img == null)
                return false;

            HObject hoRectangle;
            HObject hoPartitionRect;
            HObject hoSelectedObj;
            HObject hoDomain;
            HObject hoSelectedShapeObj;
            HObject hoSelcttedImg;//v1331

            HOperatorSet.GenEmptyObj(out hoRectangle);
            HOperatorSet.GenEmptyObj(out hoPartitionRect);
            HOperatorSet.GenEmptyObj(out hoSelectedObj);
            HOperatorSet.GenEmptyObj(out hoDomain);
            HOperatorSet.GenEmptyObj(out hoSelectedShapeObj);
            HOperatorSet.GenEmptyObj(out hoSelcttedImg);//v1331

            HTuple htMinBuff, htMaxBuff, htAvgBuff;
            HTuple htImgCnt;
            HTuple row1, col1, row2, col2;

            int imgCnt;
            int graphLine;
            double[] dMin, dMax, dAvg;
            double[] dCalcMin, dCalcMax, dCalcAvg;

            HTuple htRange, htDeviation;
            try
            {
                HOperatorSet.GetDomain(img, out hoDomain);
                HOperatorSet.SelectShapeStd(hoDomain, out hoSelectedShapeObj, "max_area", 70);
                HOperatorSet.CountObj(hoSelectedShapeObj, out htImgCnt);
                maskLeft = _iPrevLeftOffset[side];
                maskRight = _iPrevRightOffset[side];
                if (htImgCnt.I > 0)
                {
                    HOperatorSet.SmallestRectangle1(hoSelectedShapeObj, out row1, out col1, out row2, out col2);
                    if (col1.I != 0 || col2.I != (imgWidth - 1))
                    {
                        maskLeft = col1.I;
                        maskRight = col2.I;
                    }
                }

                int startHeight, endHeight, underHeight;
                if (sysp.IM_GraphCalcAreaAll == true)
                {
                    startHeight = 0;
                    endHeight = imgHeight - 1;
                }
                else
                {
                    SystemParam.GetInstance().GetImageHeightArea(imgHeight, out startHeight, out endHeight, out underHeight);
                }

                HOperatorSet.GenRectangle1(out hoRectangle, startHeight, 0, endHeight, imgWidth - 1);
                HOperatorSet.PartitionRectangle(hoRectangle, out hoPartitionRect, PROJ_WIDTH, imgHeight);

                HOperatorSet.SelectObj(img, out hoSelcttedImg, 1);//v1331 MinMaxGray、Intensity用にimg（配列）の1番目を選択
                HOperatorSet.MinMaxGray(hoPartitionRect, hoSelcttedImg, 0, out htMinBuff, out htMaxBuff, out htRange);//v1331 Halcon19からimgのままだとcatch入るため修正
                HOperatorSet.Intensity(hoPartitionRect, hoSelcttedImg, out htAvgBuff, out htDeviation);//v1331 Halcon19からimgのままだとcatch入るため修正

                HOperatorSet.CountObj(img, out htImgCnt);
                imgCnt = htImgCnt.I;
                graphLine = htMinBuff.Length;
                dMin = new double[graphLine];
                dMax = new double[graphLine];
                dAvg = new double[graphLine];

                int kansiLineCnt = (maskRight / PROJ_WIDTH) - (maskLeft / PROJ_WIDTH) - 1;
                dCalcMin= new double[kansiLineCnt];
                dCalcMax = new double[kansiLineCnt];
                dCalcAvg = new double[kansiLineCnt];

                int setImgCnt = 0;
                int iCheckNo = 0;
                for (int imgNo = 0; imgNo < imgCnt; imgNo++)
                {
                    if (imgCnt != 1)
                    {
                        if (SystemParam.GetInstance().ColorCamInspImage[imgNo] == false)
                        {
                            iCheckNo = imgNo + 1;
                            continue;
                        }
                    }

                    hoSelectedObj.Dispose();
                    HOperatorSet.SelectObj(img, out hoSelectedObj, imgNo + 1);
                    HOperatorSet.MinMaxGray(hoPartitionRect, hoSelectedObj, 0, out htMinBuff, out htMaxBuff, out htRange);
                    HOperatorSet.Intensity(hoPartitionRect, hoSelectedObj, out htAvgBuff, out htDeviation);
                    int kansiLineNo = 0;
                    for (int lineNo = 0; lineNo < graphLine; lineNo++)
                    {
                        if (lineNo <= (maskLeft / PROJ_WIDTH) || (maskRight / PROJ_WIDTH) <= lineNo)
                        {
                            dMin[lineNo] = 128;
                            dMax[lineNo] = 128;
                            dAvg[lineNo] = 128;
                            continue;
                        }
                        if (imgNo == iCheckNo)
                        {
                            dMin[lineNo] = htMinBuff[lineNo].D;
                            dMax[lineNo] = htMaxBuff[lineNo].D;
                            dCalcMin[kansiLineNo] = htMinBuff[lineNo].D;
                            dCalcMax[kansiLineNo] = htMaxBuff[lineNo].D;
                        }
                        if (dMin[lineNo] > htMinBuff[lineNo].D)
                        {
                            dMin[lineNo] = htMinBuff[lineNo].D;
                            dCalcMin[kansiLineNo] = htMinBuff[lineNo].D;
                        }
                        if (dMax[lineNo] < htMaxBuff[lineNo].D)
                        {
                            dMax[lineNo] = htMaxBuff[lineNo].D;
                            dCalcMax[kansiLineNo] = htMaxBuff[lineNo].D;
                        }
                        dAvg[lineNo] += htAvgBuff[lineNo].D;
                        dCalcAvg[kansiLineNo] += htAvgBuff[lineNo].D;
                        kansiLineNo++;
                    }
                    setImgCnt++;
                }
                htMin = new HTuple(dMin);
                htMax = new HTuple(dMax);
                htCalcMin = new HTuple(dCalcMin);
                htCalcMax = new HTuple(dCalcMax);
                //if (sysp.IM_DispGraphAvg == true)
                {
                    for (int lineNo = 0; lineNo < graphLine; lineNo++)
                        dAvg[lineNo] = dAvg[lineNo] / setImgCnt;
                    htAvg = new HTuple(dAvg);

                    for (int lineNo = 0; lineNo < kansiLineCnt; lineNo++)
                        dCalcAvg[lineNo] = dCalcAvg[lineNo] / setImgCnt;
                    htCalcAvg = new HTuple(dCalcAvg);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                hoRectangle.Dispose();
                hoPartitionRect.Dispose();
                hoSelectedObj.Dispose();
                hoDomain.Dispose();
                hoSelectedShapeObj.Dispose();
                hoSelcttedImg.Dispose();//v1331
            }
            return true;
        }
        void drawProjection(int imgHeight, int prevLeft, int prevRight, HTuple htMin, HTuple htMax, HTuple htAvg, HWndCtrl hWndC)
        {
            SystemParam sysp = SystemParam.GetInstance();

            double dMagnify = hWndC.Magnify;
            double dRate = 1 / dMagnify;

            HObject hoMinXld;
            HObject hoMaxXld;
            HObject hoAvgXld;
            HObject hoMaxSmoothXld;
            HOperatorSet.GenEmptyObj(out hoMinXld);
            HOperatorSet.GenEmptyObj(out hoMaxXld);
            HOperatorSet.GenEmptyObj(out hoAvgXld);
            HOperatorSet.GenEmptyObj(out hoMaxSmoothXld);

            double[] adMin = null, adMax = null, adAvg = null;
            double[] adHorz = null;
            double dBottom = 255 * dRate;
            dBottom = imgHeight;
            dRate = imgHeight / 255.0;

            int iLength = 0;
            if (sysp.IM_DispGraphDark == true)
            {
                adMin = new double[htMin.DArr.Length * PROJ_WIDTH];
                adHorz = new double[adMin.Length];
                iLength = adMin.Length;
            }
            if (sysp.IM_DispGraphLight == true)
            {
                adMax = new double[htMax.DArr.Length * PROJ_WIDTH];
                adHorz = new double[adMax.Length];
                iLength = adMax.Length;
            }
            if (sysp.IM_DispGraphAvg == true)
            {
                adAvg = new double[htAvg.DArr.Length * PROJ_WIDTH];
                adHorz = new double[adAvg.Length];
                iLength = adAvg.Length;
            }
            for (int i = 0; i < iLength; i++)
            {
                if (adMin != null)
                {
                    adMin[i] = dBottom - htMin.DArr[(int)(i / PROJ_WIDTH)] * dRate;
                    adHorz[i] = i;
                }
                if (adMax != null)
                {
                    adMax[i] = dBottom - htMax.DArr[(int)(i / PROJ_WIDTH)] * dRate;
                    adHorz[i] = i;
                }
                if (adAvg != null)
                {
                    adAvg[i] = dBottom - htAvg.DArr[(int)(i / PROJ_WIDTH)] * dRate;
                    adHorz[i] = i;
                }
                double basePosition = dBottom - 128 * dRate;
                if (i <= (prevLeft + PROJ_WIDTH) || (prevRight - PROJ_WIDTH) <= i)
                {
                    if (adMin != null)
                        adMin[i] = basePosition;
                    if (adMax != null)
                        adMax[i] = basePosition;
                    if (adAvg != null)
                        adAvg[i] = basePosition;
                }
            }

            try
            {
                Color c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineKandoGrapthColor);
                string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                int iLineWidth = SystemParam.GetInstance().LineKandoGrapthThick;

                //Min
                if (sysp.IM_DispGraphDark == true && iLineWidth > 0)
                {
                    HOperatorSet.GenContourPolygonXld(out hoMinXld, new HTuple(adMin), new HTuple(adHorz));
                    hWndC.GraphicManager.AddObject("XldObject_Min", hoMinXld,
                        mColName, "margin", iLineWidth);
                }
                else
                {
                    hWndC.GraphicManager.DeleteObject("XldObject_Min");
                }
                //Max
                if (sysp.IM_DispGraphLight == true)
                {
                    HOperatorSet.GenContourPolygonXld(out hoMaxXld, new HTuple(adMax), new HTuple(adHorz));
                    hWndC.GraphicManager.AddObject("XldObject_Max", hoMaxXld,
                        mColName, "margin", iLineWidth);
                }
                else
                {
                    hWndC.GraphicManager.DeleteObject("XldObject_Max");
                }
                //Avg
                if (sysp.IM_DispGraphAvg == true)
                {
                    HOperatorSet.GenContourPolygonXld(out hoAvgXld, new HTuple(adAvg), new HTuple(adHorz));
                    hWndC.GraphicManager.AddObject("XldObject_Avg", hoAvgXld,
                        mColName, "margin", iLineWidth);
                }
                else
                {
                    hWndC.GraphicManager.DeleteObject("XldObject_Avg");
                }
            }
            catch (HOperatorException exc)
            {
                throw exc;
            }
            finally
            {
                hoMinXld.Dispose();
                hoMaxXld.Dispose();
                hoAvgXld.Dispose();
                hoMaxSmoothXld.Dispose();
            }
        }
        void ClearProjection(HWndCtrl hWndC)
        {
            hWndC.GraphicManager.DeleteObject("XldObject_Min");
            hWndC.GraphicManager.DeleteObject("XldObject_Max");
            hWndC.GraphicManager.DeleteObject("XldObject_Avg");
        }
        #endregion

        #region ライン描画
        /// <summary>
        /// 検査高さ方向の線
        /// </summary>
        /// <param name="side"></param>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        private void drawInspHeightLine(int side, int imgWidth, int imgHeight)
        {
            try
            {
                SystemParam sysp = SystemParam.GetInstance();
                string[] objName = new string[] { "ToptLine", "ButtomLine" };
                int startHeight, endHeight, underHeight;
                if (sysp.IM_DispInspHeight == true)
                {
                    Color c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineInspHeightColor);
                    string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                    int iLineWidth = SystemParam.GetInstance().LineInspHeightThick;

                    SystemParam.GetInstance().GetImageHeightArea(imgHeight, out startHeight, out endHeight, out underHeight);
                    if (iLineWidth > 0)
                    {
                        _hwnCtrl[side].GraphicManager.AddLine(objName[0], startHeight, 0, startHeight, imgWidth, mColName, iLineWidth);
                        _hwnCtrl[side].GraphicManager.AddLine(objName[1], endHeight, 0, endHeight, imgWidth, mColName, iLineWidth);
                    }
                    else
                    {
                        _hwnCtrl[side].GraphicManager.DeleteObject(objName[0]);
                        _hwnCtrl[side].GraphicManager.DeleteObject(objName[1]);
                    }
                }
                else
                {
                    _hwnCtrl[side].GraphicManager.DeleteObject(objName[0]);
                    _hwnCtrl[side].GraphicManager.DeleteObject(objName[1]);
                }
            }
            catch(Exception exc)
            {
                throw exc;
            }
        }


        int[] _iPrevLeft, _iPrevRight;
        int[] _iPrevLeftOffset, _iPrevRightOffset;
        int[] _iCropHeightSize;
        /// <summary>
        /// 状態監視
        /// </summary>
        /// <param name="side"></param>
        private void drawInspWidthLine(int side)
        {
            SystemParam sysp = SystemParam.GetInstance();
            int iLineWidth = SystemParam.GetInstance().LineStateMonitorThick;
            string[] objName = new string[] { "LeftLine", "RightLine" };
            if (sysp.IM_DispMaskWidth == true && iLineWidth > 0)
            {
                Color c;
                c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineStateMonitorColor);
                string mColName1 = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineStateMonitorColor);
                string mColName2 = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);

                _hwnCtrl[side].GraphicManager.AddLine(objName[0], 0, _iPrevLeftOffset[side], _iCropHeightSize[side], _iPrevLeftOffset[side], mColName1, iLineWidth);
                _hwnCtrl[side].GraphicManager.AddLine(objName[1], 0, _iPrevRightOffset[side], _iCropHeightSize[side], _iPrevRightOffset[side], mColName2, iLineWidth);
            }
            else
            {
                _hwnCtrl[side].GraphicManager.DeleteObject(objName[0]);
                _hwnCtrl[side].GraphicManager.DeleteObject(objName[1]);
            }
        }
        /// <summary>
        /// 検査幅
        /// </summary>
        /// <param name="side"></param>
        private void drawMaskLine(int side)
        {
            SystemParam sysp = SystemParam.GetInstance();
            string[] objName = new string[] { "LeftMaskLine", "RightMaskLine" };
            if (sysp.IM_DispInspWidth == true)
            {
                Color c　= ColorTranslator.FromHtml(SystemParam.GetInstance().LineInspWidthColor);
                string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                int iLineWidth = SystemParam.GetInstance().LineInspWidthThick;

                _hwnCtrl[side].GraphicManager.AddLine(objName[0], 0, _iPrevLeft[side], _iCropHeightSize[side], _iPrevLeft[side], mColName, iLineWidth);
                _hwnCtrl[side].GraphicManager.AddLine(objName[1], 0, _iPrevRight[side], _iCropHeightSize[side], _iPrevRight[side], mColName, iLineWidth);
            }
            else
            {
                _hwnCtrl[side].GraphicManager.DeleteObject(objName[0]);
                _hwnCtrl[side].GraphicManager.DeleteObject(objName[1]);
            }
        }
        /// <summary>
        /// 感度ライン
        /// </summary>
        /// <param name="side"></param>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        private void drawKandoLine(int side, int imgWidth, int imgHeight)
        {
            InspKandoParam[,] kandoP;
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                kandoP = _mainForm.AutoInspection.GetKandoData();
                if (kandoP == null)
                    return;
            }
            else
            {
                kandoP = new InspKandoParam[Enum.GetNames(typeof(AppData.SideID)).Length, Enum.GetNames(typeof(AppData.InspID)).Length];
                for (int sideId = 0; sideId < Enum.GetNames(typeof(AppData.SideID)).Length; sideId++)
                {
                    for (int inspId = 0; inspId < Enum.GetNames(typeof(AppData.InspID)).Length; inspId++)
                    {
                        kandoP[sideId, inspId] = Recipe.GetInstance().InspParam[sideId].Kando[inspId];
                    }
                }
            }

            SystemParam sysp = SystemParam.GetInstance();

            double dMagnify = _hwnCtrl[side].Magnify;
            double dRate = 1 / dMagnify;

            SystemParam syspara = SystemParam.GetInstance();
            List<MarkColor> mCol = (side == 0) ? syspara.markColorUpSide : syspara.markColorDownSide;

            double dBottom = 255 * dRate;
            dBottom = imgHeight;
            dRate = imgHeight / 255.0;
            for (int i = 0; i < Enum.GetNames(typeof(AppData.InspID)).Length; i++)
            {
                double row;
                int pt = 0;
                if (Recipe.GetInstance().UpDownSideCommon == false)
                    pt = side;
                int bufKando = kandoP[pt, i].Threshold;

                if (i < 3/*明*/)
                    row = 128 + bufKando;
                else
                    row = 128 - bufKando;
                double level;
                level = dBottom - row * dRate;

                Color c = ColorTranslator.FromHtml(mCol[i].colorARGB);
                string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);

                string keyName = string.Format("KandoLine{0}_{1}", i.ToString(), side.ToString());
                int iLineWidth = SystemParam.GetInstance().LineKandoThick[i];
                if (sysp.IM_DispKandoLine == true && iLineWidth > 0)
                {
                    _hwnCtrl[side].GraphicManager.AddLine(keyName, level, 0, level, imgWidth, mColName, iLineWidth);
                }
                else
                {
                    _hwnCtrl[side].GraphicManager.DeleteObject(keyName);
                }
            }
        }
        private void drawBaseKandoLine(int side, int imgWidth, int imgHeight)
        {
            SystemParam sysp = SystemParam.GetInstance();

            double dMagnify = _hwnCtrl[side].Magnify;
            double dRate = 1 / dMagnify;

            SystemParam syspara = SystemParam.GetInstance();
            List<MarkColor> mCol = (side == 0) ? syspara.markColorUpSide : syspara.markColorDownSide;

            double dBottom = 255 * dRate;
            dBottom = imgHeight;
            dRate = imgHeight / 255.0;

            {
                double row;
                row = 128;
                double level;
                level = dBottom - row * dRate;

                int iLineWidth = SystemParam.GetInstance().LineBaseKandoThick;
                string keyName = string.Format("KandoBaseLine{0}", side.ToString());
                if (sysp.IM_DispKandoLine == true && iLineWidth > 0)
                {
                    Color c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineBaseKandoColor);
                    string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                    _hwnCtrl[side].GraphicManager.AddLine(keyName, level, 0, level, imgWidth, mColName, iLineWidth);
                }
                else
                {
                    _hwnCtrl[side].GraphicManager.DeleteObject(keyName);
                }
            }
        }
        #endregion


        #region Callback
        public bool _bBackAutoSaveEnable;
        /// <summary>
        /// 取込中断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPause_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNgHistory.Checked == true)
            {
                chkPause.Checked = false;
                return;
            }

            if (chkPause.Checked == true)
            {
                if (_frmImageMainParam != null)
                {
                    SaveFuncPartsEnable(true);
                }
                spinImageDispNo.Enabled = true;

                _setPartsFlag = true;
                _bBackAutoSaveEnable = SystemParam.GetInstance().IM_AutoSaveEnable;
                SystemParam.GetInstance().IM_AutoSaveEnable = false;
                if (_frmImageMainParam != null)
                {
                    _frmImageMainParam.chkAutoSave.Checked = false;
                }
                _setPartsFlag = false;

                chkPause.BackColor = Color.Green;
                chkPause.ForeColor = Color.White;
            }
            else
            {
                if (_frmImageMainParam != null)
                {
                    SaveFuncPartsEnable(false);
                }
                spinImageDispNo.Enabled = false;
                spinImageDispNo.Value = 1;

                _setPartsFlag = true;
                SystemParam.GetInstance().IM_AutoSaveEnable = _bBackAutoSaveEnable;
                if (_frmImageMainParam != null)
                {
                    _frmImageMainParam.chkAutoSave.Checked = _bBackAutoSaveEnable;
                }
                _setPartsFlag = false;

                chkPause.BackColor = SystemColors.Control;
                chkPause.ForeColor = Color.Black;
            }
        }
        /// <summary>
        /// バッファされている画像の表示する番号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinImageDispNo_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;

            int dispNo = (int)spinImageDispNo.Value - 1;
            if (dispNo < _imageOrgOmote.Count)
                _displayNo = dispNo;

            refreshImageBuffer();
        }

        private void chkNgHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPause.Checked == true)
            {
                chkNgHistory.Checked = false;
                return;
            }
            if (chkNgHistory.Checked == true)
            {
                if (_frmImageMainParam != null)
                {
                    SaveFuncPartsEnable(true);
                }
                spinNgHistoryNumber.Enabled = true;

                _setPartsFlag = true;
                _bBackAutoSaveEnable = SystemParam.GetInstance().IM_AutoSaveEnable;
                SystemParam.GetInstance().IM_AutoSaveEnable = false;
                if (_frmImageMainParam != null)
                {
                    _frmImageMainParam.chkAutoSave.Checked = false;
                }
                _setPartsFlag = false;

                refreshImageBuffer();

                chkNgHistory.BackColor = Color.Green;
                chkNgHistory.ForeColor = Color.White;
            }
            else
            {
                if (_frmImageMainParam != null)
                {
                    SaveFuncPartsEnable(false);
                }
                spinNgHistoryNumber.Enabled = false;
                spinNgHistoryNumber.Value = 1;

                _setPartsFlag = true;
                SystemParam.GetInstance().IM_AutoSaveEnable = _bBackAutoSaveEnable;
                if (_frmImageMainParam != null)
                {
                    _frmImageMainParam.chkAutoSave.Checked = _bBackAutoSaveEnable;
                }
                _setPartsFlag = false;

                chkNgHistory.BackColor = SystemColors.Control;
                chkNgHistory.ForeColor = Color.Black;
            }
        }
        private void spinNgHistoryNumber_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;

            refreshImageBuffer();
        }



        /// <summary>
        /// 表示・非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclImageMain_VisibleChanged(object sender, EventArgs e)
        {
            if (Recipe.GetInstance().UpSideInspEnable == true)
            {
                lblInspStatusOmote.Text = "表面検査有効";
                lblInspStatusOmote.BackColor = Color.LightGreen;
            }
            else
            {
                lblInspStatusOmote.Text = "表面検査無効";
                lblInspStatusOmote.BackColor = Color.Pink;
            }
            if (Recipe.GetInstance().DownsideInspEnable == true)
            {
                lblInspStatusUra.Text = "裏面検査有効";
                lblInspStatusUra.BackColor = Color.LightGreen;
            }
            else
            {
                lblInspStatusUra.Text = "裏面検査無効";
                lblInspStatusUra.BackColor = Color.Pink;
            }

            SetSystemParam();

            VisibleEnableKandoSetting();
        }

        #region ダブルクリック
        frmImageMainParam _frmImageMainParam = null;
        /// <summary>
        /// ダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclImageMain_DoubleClick(object sender, EventArgs e)
        {
            if (_frmImageMainParam == null)
            {
                frmPassword frm = new frmPassword();
                frm.DeveloperPassword = SystemParam.GetInstance().DeveloperPasswordHash;
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return;
                _frmImageMainParam = new frmImageMainParam(this);
                SaveFuncPartsEnable(chkPause.Checked|chkNgHistory.Checked);
                _frmImageMainParam.FormClosed += _frmImageMainParam_FormClosed;
                _frmImageMainParam.Show();
            }
            else
            {
                if (_frmImageMainParam != null)
                    _frmImageMainParam.Close();
            }
        }
        private void _frmImageMainParam_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmImageMainParam = null;
        }
        #endregion


        private void chkDisplayMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            int index = (int)((CheckBox)sender).Tag;
            _setPartsFlag = true;
            for (int i=0; i<_chkBoxDisplayMode.Length;i++)
            {
                _chkBoxDisplayMode[i].Checked = (i == index);
                if (i == index)
                    _displayMode = index;
            }
            _setPartsFlag = false;

            refreshImageBuffer();
        }


        /// <summary>
        /// 倍率１
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMagnify1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_setMagnifyFlag || cmbMagnify1.SelectedIndex == -1)
                return;
            _setMagnifyFlag = true;
            cmbMagnify2.SelectedIndex = cmbMagnify1.SelectedIndex;
            _setMagnifyFlag = false;
        }
        /// <summary>
        /// 倍率２
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMagnify2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_setMagnifyFlag || cmbMagnify2.SelectedIndex == -1)
                return;
            _setMagnifyFlag = true;
            cmbMagnify1.SelectedIndex = cmbMagnify2.SelectedIndex;
            _setMagnifyFlag = false;
        }
        #endregion



        private uclKandoControl[] _lstKandoCtrl;
        #region 感度操作

        private void VisibleEnableKandoSetting()
        {
            if (_mainForm == null)
                return;

            //
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                //検査中 ＆＆　非表示
                //if (this.Visible == false)
                {
                    ResetKandoMessage();
                    SetRecipeFile2Inspection();
                    SetRecipeFile2Disp();
                    SetRecipeFile2Buffer();
                }
            }
            else
            {
                //検査停止中　＆＆　表示
                if (this.Visible == true)
                {
                    ResetKandoMessage();
                    SetRecipeFile2Inspection();
                    SetRecipeFile2Disp();
                    SetRecipeFile2Buffer();
                    DisableKandoParts();
                }
            }
            SetRecipeFile2KandoBackColor();
        }

        private void EnableKandoParts()
        {
            this.btnKandoSave.Enabled = true;
            this.btnKandoApplyCancel.Enabled = true;
            this.uclKandoControl1.Enabled = true;
            this.uclKandoControl2.Enabled = true;

            this.uclKandoControl1.EnableLight = SystemParam.GetInstance().InspBrightEnable;
            this.uclKandoControl2.EnableLight = SystemParam.GetInstance().InspBrightEnable;
            this.uclKandoControl1.EnableDark = SystemParam.GetInstance().InspDarkEnable;
            this.uclKandoControl2.EnableDark = SystemParam.GetInstance().InspDarkEnable;
        }
        private void DisableKandoParts()
        {
            this.btnKandoSave.Enabled = false;
            this.btnKandoApplyCancel.Enabled = false;
            this.uclKandoControl1.Enabled = false;
            this.uclKandoControl2.Enabled = false;
        }

        private int[,] _BufferFileData = new int[Enum.GetNames(typeof(AppData.SideID)).Length, Enum.GetNames(typeof(AppData.InspID)).Length];

        private void SetRecipeFile2Disp()
        {
            int sideIndex = 0;
            foreach (InspParameter p in Recipe.GetInstance().InspParam)
            {
                int kandoIndex = 0;
                foreach (InspKandoParam k in p.Kando)
                {
                    _lstKandoCtrl[sideIndex].SetValue(kandoIndex, k.Threshold);
                    kandoIndex++;
                }
                sideIndex++;
            }
            _lstKandoCtrl[1].Enabled = !Recipe.GetInstance().UpDownSideCommon;
        }

        private void SetRecipeFile2Buffer()
        {
            int sideIndex = 0;
            foreach (InspParameter p in Recipe.GetInstance().InspParam)
            {
                int kandoIndex = 0;
                foreach (InspKandoParam k in p.Kando)
                {
                    _BufferFileData[sideIndex, kandoIndex] = k.Threshold;
                    kandoIndex++;
                }
                sideIndex++;
            }
        }

        private void SetRecipeFile2Inspection()
        {
            InspKandoParam[,] kandoP = _mainForm.AutoInspection.GetKandoData();
            if (kandoP == null)
                return;

            int sideIndex = 0;
            foreach (InspParameter p in Recipe.GetInstance().InspParam)
            {
                int kandoIndex = 0;
                foreach (InspKandoParam k in p.Kando)
                {
                    kandoP[sideIndex, kandoIndex].Threshold = k.Threshold;
                    kandoIndex++;
                }
                sideIndex++;
            }
        }

        private void SetRecipeFile2KandoBackColor()
        {
            int sideIndex = 0;
            foreach (InspParameter p in Recipe.GetInstance().InspParam)
            {
                int kandoIndex = 0;
                foreach (InspKandoParam k in p.Kando)
                {
                    string strColor = (sideIndex == 0) ? SystemParam.GetInstance().markColorUpSide[kandoIndex].colorARGB : SystemParam.GetInstance().markColorDownSide[kandoIndex].colorARGB;
                    Color col = ColorTranslator.FromHtml(strColor);
                    if (sideIndex == 1 && Recipe.GetInstance().UpDownSideCommon == true)
                        col = Color.Gray;
                    _lstKandoCtrl[sideIndex].SetBackColor(kandoIndex, col);
                    kandoIndex++;
                }
                sideIndex++;
            }
        }

        /// <summary>
        /// 反映を戻す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKandoApplyCancel_Click(object sender, EventArgs e)
        {
            InspKandoParam[,] kandoP = _mainForm.AutoInspection.GetKandoData();
            if (kandoP == null)
                return;
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int inspId = 0; inspId < Enum.GetNames(typeof(AppData.InspID)).Length; inspId++)
                {
                    kandoP[side, inspId].Threshold = _BufferFileData[side, inspId];
                    _lstKandoCtrl[side].SetValue(inspId, _BufferFileData[side, inspId]);
                }
            }
            ResetKandoMessage();
        }

        /// <summary>
        /// ファイルに保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKandoSave_Click(object sender, EventArgs e)
        {
            bool isChange = false;
            for (int sideId = 0; sideId < Enum.GetNames(typeof(AppData.SideID)).Length; sideId++)
            {
                for (int inspId = 0; inspId < Enum.GetNames(typeof(AppData.InspID)).Length; inspId++)
                {
                    if (Recipe.GetInstance().InspParam[sideId].Kando[inspId].Threshold != _lstKandoCtrl[sideId].GetValue(inspId))
                    {
                        Recipe.GetInstance().InspParam[sideId].Kando[inspId].Threshold = _lstKandoCtrl[sideId].GetValue(inspId);
                        isChange = true;
                    }
                }
            }
            if (isChange)
            {
                Recipe.GetInstance().Save();
                ResetKandoMessage();
                SetRecipeFile2Buffer();
            }
        }

        /// <summary>
        /// 値変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclKandoControl_KandoValueChanged(object sender, EventArgs e)
        {
            bool isChange = false;
            InspKandoParam[,] kandoP = _mainForm.AutoInspection.GetKandoData();
            if (kandoP == null)
                return;
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int inspId = 0; inspId < Enum.GetNames(typeof(AppData.InspID)).Length; inspId++)
                {
                    if (kandoP[side, inspId].Threshold != _lstKandoCtrl[side].GetValue(inspId))
                    {
                        kandoP[side, inspId].Threshold = _lstKandoCtrl[side].GetValue(inspId);
                    }
                    if (kandoP[side, inspId].Threshold != _BufferFileData[side, inspId])
                        isChange = true;
                }
            }

            if (isChange == true)
            {
                lblKandoMessage.Text = "検査中の感度は、保存されていません";
                lblKandoMessage.BackColor = Color.HotPink;
                tmKandoMessage.Interval = 1000;
                tmKandoMessage.Start();
            }
            else
            {
                ResetKandoMessage();
            }
        }
        private void ResetKandoMessage()
        {
            lblKandoMessage.Text = "";
            lblKandoMessage.BackColor = Color.White;
            tmKandoMessage.Stop();
        }
        #endregion



        private void tmKandoMessage_Tick(object sender, EventArgs e)
        {
            if (tmKandoMessage.Interval == 1000)
            {
                tmKandoMessage.Interval = 100;
                lblKandoMessage.BackColor = Color.White;
            }
            else
            {
                tmKandoMessage.Interval = 1000;
                lblKandoMessage.BackColor = Color.HotPink;
            }
        }


        private void SetLayout()
        {
            if (SystemParam.GetInstance().DownSideEnable)
            {
                //裏あり
                if (SystemParam.GetInstance().IM_VerDisplayMode == true)
                {
                    //縦並び
                    grpTitleOmote.Location = new Point(3, 3);
                    hwcImageWnd1.Location = new Point(3, 40);
                    hwcImageWnd1.Size = new Size(738, 360);

                    grpTitleUra.Location = new Point(3, 406);
                    hwcImageWnd2.Location = new Point(3, 443);
                    hwcImageWnd2.Size = new Size(738, 360);
                }
                else
                {
                    //横並び
                    grpTitleOmote.Location = new Point(3, 3);
                    hwcImageWnd1.Location = new Point(3, 40);
                    hwcImageWnd1.Size = new Size(369, 763);

                    grpTitleUra.Location = new Point(374, 3);
                    hwcImageWnd2.Location = new Point(374, 40);
                    hwcImageWnd2.Size = new Size(369, 763);
                }
            }
            else
            {
                grpTitleOmote.Location = new Point(3, 3);
                hwcImageWnd1.Location = new Point(3, 40);
                hwcImageWnd1.Size = new Size(738, 763);


            }
        }
        private void uclImageMain_Load(object sender, EventArgs e)
        {
            _bBackAutoSaveEnable = SystemParam.GetInstance().IM_AutoSaveEnable;
        }
        private void chkVerDisplayMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_VerDisplayMode = chkVerDisplayMode.Checked;
            SetLayout();
        }

        private void hwcImageWnd1_MouseUp(object sender, MouseEventArgs e)
        {
            //if (_hwnCtrl[0].IsDragZoom == true)
            //    _hwnCtrl[1].ChangeMagnify(_hwnCtrl[0].Magnify);
        }

        private void hwcImageWnd2_MouseUp(object sender, MouseEventArgs e)
        {
            //if (_hwnCtrl[1].IsDragZoom == true)
            //    _hwnCtrl[0].ChangeMagnify(_hwnCtrl[1].Magnify);
        }

        public void StartInsp()
        {
            ResetKandoMessage();
            EnableKandoParts();
            SetRecipeFile2Disp();
            SetRecipeFile2Buffer();
        }

        private bool _setFlag = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkZoomEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;

            _setFlag = true;
            if (chkZoomEnable.Checked == true)
                chkMoveEnable.Checked = false;
            SetZoomMoveControl();
            _setFlag = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMoveEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;

            _setFlag = true;
            if (chkMoveEnable.Checked == true)
                chkZoomEnable.Checked = false;
            SetZoomMoveControl();
            _setFlag = false;
        }
        private void SetZoomMoveControl()
        {
            foreach (HWndCtrl hw in _hwnCtrl)
            {
                hw.IsNonShiftKeyImageZoom = chkZoomEnable.Checked;
                hw.IsNonCtrlKeyDragImageMove = chkMoveEnable.Checked;
            }
            SetWindowCtrlCheckButton(chkZoomEnable);
            SetWindowCtrlCheckButton(chkMoveEnable);
        }
        private void SetWindowCtrlCheckButton(CheckBox chk)
        {
            chk.BackColor = (chk.Checked == true) ? Color.GreenYellow : SystemColors.Control;
        }

        public void StopInsp()
        {
            ResetKandoMessage();
            SetRecipeFile2Disp();
            SetRecipeFile2Buffer();
            DisableKandoParts();
        }

        private void SaveFuncPartsEnable(bool enable)
        {
            _frmImageMainParam.btnSaveImage.Enabled = enable;
            _frmImageMainParam.spinImageBufferCount.Enabled = !enable;
            _frmImageMainParam.grpImageAuto.Enabled = !enable;
        }
    }
}
