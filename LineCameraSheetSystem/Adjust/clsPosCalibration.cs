using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;
using Fujita.Misc;

namespace Adjustment
{
    class clsPosCalibration : IDisposable
    {
        public clsPosCalibration()
        {
            MeasOffsetX = 0d;
            MeasOffsetY = 0d;

            _diclstTestedImage = new Dictionary<EAdjustmentCameraType, List<TestedImage>>();

            _eBaseCamNo = EAdjustmentCameraType.Camera1;
            _eTargetCamNo = EAdjustmentCameraType.Camera2;

            _eOffsetType = EAdjustmentOffsetType.Camera1_2;
            assignTestedImageList();

            BaseResolutionHorz = 1d;
            BaseResolutionVert = 1d;

            TargetResolutionHorz = 1d;
            TargetResolutionVert = 1d;

            // 画像の最大量は10とする
            LimitCount = 10;

        }

        // 概要:
        //     アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        public void Dispose()
        {
            clearTestedImageList();
        }

        public bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            _eBaseCamNo = (EAdjustmentCameraType)ifa.GetIni("PosCalibration", "BaseCamNo", typeof(EAdjustmentCameraType), _eBaseCamNo, sPath);
            _eTargetCamNo = (EAdjustmentCameraType)ifa.GetIni("PosCalibration", "TargetCamNo", typeof(EAdjustmentCameraType), _eTargetCamNo, sPath);

            ThresholdParamBaseCam.ThresholdLow = ifa.GetIni("PosCalibration", "CamB_ThresholdLow", ThresholdParamBaseCam.ThresholdLow, sPath);
            ThresholdParamBaseCam.ThresholdHigh = ifa.GetIni("PosCalibration", "CamB_ThresholdHig", ThresholdParamBaseCam.ThresholdHigh, sPath);
            ThresholdParamBaseCam.OpeningRadius = ifa.GetIni("PosCalibration", "CamB_OpeningRadius", ThresholdParamBaseCam.OpeningRadius, sPath);
            ThresholdParamBaseCam.ClosingRadius = ifa.GetIni("PosCalibration", "CamB_ClosingRadius", ThresholdParamBaseCam.ClosingRadius, sPath);
            ThresholdParamBaseCam.InspectOffset = ifa.GetIni("PosCalibration", "CamB_InspectOffset", ThresholdParamBaseCam.InspectOffset, sPath);
            ThresholdParamBaseCam.InspectWidth = ifa.GetIni("PosCalibration", "CamB_InspectWidth", ThresholdParamBaseCam.InspectWidth, sPath);

            ThresholdParamTargetCam.ThresholdLow = ifa.GetIni("PosCalibration", "CamN_ThresholdLow", ThresholdParamTargetCam.ThresholdLow, sPath);
            ThresholdParamTargetCam.ThresholdHigh = ifa.GetIni("PosCalibration", "CamN_ThresholdHig", ThresholdParamTargetCam.ThresholdHigh, sPath);
            ThresholdParamTargetCam.OpeningRadius = ifa.GetIni("PosCalibration", "CamN_OpeningRadius", ThresholdParamTargetCam.OpeningRadius, sPath);
            ThresholdParamTargetCam.ClosingRadius = ifa.GetIni("PosCalibration", "CamN_ClosingRadius", ThresholdParamTargetCam.ClosingRadius, sPath);
            ThresholdParamTargetCam.InspectOffset = ifa.GetIni("PosCalibration", "CamN_InspectOffset", ThresholdParamTargetCam.InspectOffset, sPath);
            ThresholdParamTargetCam.InspectWidth = ifa.GetIni("PosCalibration", "CamN_InspectWidth", ThresholdParamTargetCam.InspectWidth, sPath);

            ThresholdParamBaseCam.LimitMaxHeightMili = ifa.GetIni("PosCalibration", "LimitMaxHeightMili", ThresholdParamBaseCam.LimitMaxHeightMili, sPath);
            ThresholdParamBaseCam.LimitMaxWidthMili = ifa.GetIni("PosCalibration", "LimitMaxWidthMili", ThresholdParamBaseCam.LimitMaxWidthMili, sPath);
            ThresholdParamBaseCam.LimitMinHeightMili = ifa.GetIni("PosCalibration", "LimitMinHeightMili", ThresholdParamBaseCam.LimitMinHeightMili, sPath);
            ThresholdParamBaseCam.LimitMinWidthMili = ifa.GetIni("PosCalibration", "LimitMinWidthMili", ThresholdParamBaseCam.LimitMinWidthMili, sPath);

            ThresholdParamTargetCam.LimitMaxHeightMili = ifa.GetIni("PosCalibration", "LimitMaxHeightMili", ThresholdParamTargetCam.LimitMaxHeightMili, sPath);
            ThresholdParamTargetCam.LimitMaxWidthMili = ifa.GetIni("PosCalibration", "LimitMaxWidthMili", ThresholdParamTargetCam.LimitMaxWidthMili, sPath);
            ThresholdParamTargetCam.LimitMinHeightMili = ifa.GetIni("PosCalibration", "LimitMinHeightMili", ThresholdParamTargetCam.LimitMinHeightMili, sPath);
            ThresholdParamTargetCam.LimitMinWidthMili = ifa.GetIni("PosCalibration", "LimitMinWidthMili", ThresholdParamTargetCam.LimitMinWidthMili, sPath);



            return true;
        }

        public bool Save(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            ifa.SetIni("PosCalibration", "BaseCamNo", _eBaseCamNo, sPath);
            ifa.SetIni("PosCalibration", "TargetCamNo", _eTargetCamNo, sPath);

            ifa.SetIni("PosCalibration", "CamB_ThresholdLow", ThresholdParamBaseCam.ThresholdLow, sPath);
            ifa.SetIni("PosCalibration", "CamB_ThresholdHig", ThresholdParamBaseCam.ThresholdHigh, sPath);
            ifa.SetIni("PosCalibration", "CamB_OpeningRadius", ThresholdParamBaseCam.OpeningRadius, sPath);
            ifa.SetIni("PosCalibration", "CamB_ClosingRadius", ThresholdParamBaseCam.ClosingRadius, sPath);
            ifa.SetIni("PosCalibration", "CamB_InspectOffset", ThresholdParamBaseCam.InspectOffset, sPath);
            ifa.SetIni("PosCalibration", "CamB_InspectWidth", ThresholdParamBaseCam.InspectWidth, sPath);

            ifa.SetIni("PosCalibration", "CamN_ThresholdLow", ThresholdParamTargetCam.ThresholdLow, sPath);
            ifa.SetIni("PosCalibration", "CamN_ThresholdHig", ThresholdParamTargetCam.ThresholdHigh, sPath);
            ifa.SetIni("PosCalibration", "CamN_OpeningRadius", ThresholdParamTargetCam.OpeningRadius, sPath);
            ifa.SetIni("PosCalibration", "CamN_ClosingRadius", ThresholdParamTargetCam.ClosingRadius, sPath);
            ifa.SetIni("PosCalibration", "CamN_InspectOffset", ThresholdParamTargetCam.InspectOffset, sPath);
            ifa.SetIni("PosCalibration", "CamN_InspectWidth", ThresholdParamTargetCam.InspectWidth, sPath);

            ifa.SetIni("PosCalibration", "LimitMaxHeightMili", ThresholdParamBaseCam.LimitMaxHeightMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMaxWidthMili", ThresholdParamBaseCam.LimitMaxWidthMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMinHeightMili", ThresholdParamBaseCam.LimitMinHeightMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMinWidthMili", ThresholdParamBaseCam.LimitMinWidthMili, sPath);

            ifa.SetIni("PosCalibration", "LimitMaxHeightMili", ThresholdParamTargetCam.LimitMaxHeightMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMaxWidthMili", ThresholdParamTargetCam.LimitMaxWidthMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMinHeightMili", ThresholdParamTargetCam.LimitMinHeightMili, sPath);
            ifa.SetIni("PosCalibration", "LimitMinWidthMili", ThresholdParamTargetCam.LimitMinWidthMili, sPath);

            return true;
        }


        Dictionary< EAdjustmentCameraType, List<TestedImage>> _diclstTestedImage = new Dictionary<EAdjustmentCameraType,List<TestedImage>>();

        EAdjustmentCameraType _eBaseCamNo;
        public EAdjustmentCameraType BaseCamNo
        {
            get
            {
                return _eBaseCamNo;
            }
            set
            {
                if (_eBaseCamNo != value)
                {
                    _bDoneMeas = false;
                }
                _eBaseCamNo = value;
                if (_eBaseCamNo == _eTargetCamNo)
                {
                    _eTargetCamNo = (EAdjustmentCameraType)(((int)_eBaseCamNo + 1) % Enum.GetNames(typeof(EAdjustmentCameraType)).Length);
                }
            }
        }
        
        EAdjustmentCameraType _eTargetCamNo;
        public EAdjustmentCameraType TargetCamNo
        {
            get
            {
                return _eTargetCamNo;
            }
            set
            {
                if (_eBaseCamNo == value)
                    return;
                _eTargetCamNo = value;
            }
        }


        EAdjustmentOffsetType _eOffsetType;
        public EAdjustmentOffsetType OffsetType
        {
            get
            {
                return _eOffsetType;
            }

            set
            {
                if (_eOffsetType == value)
                    return;
                _eOffsetType = value;

                _bDoneMeas = false;
                MeasOffsetX = 0d;
                MeasOffsetY = 0d;

                clearTestedImageList();
                assignTestedImageList();
            }
        }

        private void assignTestedImageList()
        {
            _diclstTestedImage.Add(_eBaseCamNo, new List<TestedImage>());
            //switch (_eOffsetType)
            //{
            //    case EAdjustmentOffsetType.Camera1_2:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera2;
            //        break;
            //    case EAdjustmentOffsetType.Camera1_3:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera3;
            //        break;
            //    case EAdjustmentOffsetType.Camera1_4:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera4;
            //        break;
            //}
            _diclstTestedImage.Add(_eTargetCamNo, new List<TestedImage>());
        }

        private void clearTestedImageList()
        {
            foreach (EAdjustmentCameraType no in _diclstTestedImage.Keys)
            {
                _diclstTestedImage[no].ForEach(x => x.Dispose());
                _diclstTestedImage[no].Clear();
            }
            _diclstTestedImage.Clear();
        }

        class BlobResults
        {
            List<BlobResult> _lstBlobResult = new List<BlobResult>();
            public List<BlobResult> ListBlobResult
            {
                get { return _lstBlobResult; }
            }
            public bool IsFindBlob()
            {
                return (_lstBlobResult.Count != 1);
            }
            public void Clear()
            {
                _lstBlobResult.Clear();
            }
        }

        Dictionary<EAdjustmentCameraType, clsImageBuffer> _diclstImageBuffer = new Dictionary<EAdjustmentCameraType, clsImageBuffer>();
        Dictionary<EAdjustmentCameraType, int> _dicImageCount = new Dictionary<EAdjustmentCameraType, int>();
        Dictionary<EAdjustmentCameraType, List<BlobResults>> _diclstResult = new Dictionary<EAdjustmentCameraType, List<BlobResults>>();

        private void assignImageBuffer()
        {
            _diclstImageBuffer.Add(_eBaseCamNo, new clsImageBuffer());
            _dicImageCount.Add(_eBaseCamNo, 0);
            _diclstResult.Add(_eBaseCamNo, new List<BlobResults>());

            //switch (_eOffsetType)
            //{
            //    case EAdjustmentOffsetType.Camera1_2:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera2;
            //        break;
            //    case EAdjustmentOffsetType.Camera1_3:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera3;
            //        break;
            //    case EAdjustmentOffsetType.Camera1_4:
            //        _eTargetCamNo = EAdjustmentCameraType.Camera4;
            //        break;
            //}

            _diclstImageBuffer.Add(_eTargetCamNo, new clsImageBuffer());
            _dicImageCount.Add(_eTargetCamNo, 0);
            _diclstResult.Add(_eTargetCamNo, new List<BlobResults>());
        }

        private void clearImageBuffer()
        {
            foreach (EAdjustmentCameraType no in _diclstImageBuffer.Keys)
            {
                foreach (BlobResults lst in _diclstResult[no])
                {
                    lst.Clear();
                }
                _diclstImageBuffer[no].Clear();
            }
            _diclstResult.Clear();
            _diclstImageBuffer.Clear();
            _dicImageCount.Clear();
        }

        public void ClearMeasData()
        {
            MeasOffsetX = 0d;
            MeasOffsetY = 0d;

            clearTestedImageList();
            assignTestedImageList();

            clearImageBuffer();
            assignImageBuffer();
        }

        public double MeasOffsetX
        {
            get;
            private set;
        }

        public double MeasOffsetY
        {
            get;
            private set;
        }

        public int BaseImageCount
        {
            get
            {
                //                return _diclstTestedImage[_eBaseCamNo].Count;
                return _diclstTestedImage[_eBaseCamNo].Count;
            }
        }
        public int TargetImageCount
        {
            get
            {
                return _diclstTestedImage[_eTargetCamNo].Count;
            }
        }

        public bool IsBaseResultReady(int iNo)
        {
            if (BaseImageCount <= iNo)
                return false;
            return _diclstTestedImage[_eBaseCamNo][iNo].IsResultReady();
        }

        public bool IsTargetResultReady(int iNo)
        {
            if (TargetImageCount <= iNo)
                return false;
            return _diclstTestedImage[_eTargetCamNo][iNo].IsResultReady();
        }

        bool _bDoneMeas = false;
        public bool DoneMeas
        {
            get
            {
                return _bDoneMeas;
            }
        }

        double _dBaseResolutionHorz = 1.0;
        public double BaseResolutionHorz
        {
            get
            {
                return _dBaseResolutionHorz;
            }
            set
            {
                if (value <= 0.0)
                    return;
                _dBaseResolutionHorz = value;
            }
        }

        double _dBaseResolutionVert = 1.0;
        public double BaseResolutionVert
        {
            get
            {
                return _dBaseResolutionVert;
            }
            set
            {
                if (_dBaseResolutionVert <= 0.0)
                    return;
                _dBaseResolutionVert = value;
            }
        }

        double _dTargetResolutionHorz = 1.0;
        public double TargetResolutionHorz
        {
            get
            {
                return _dTargetResolutionHorz;
            }
            set
            {
                if (_dTargetResolutionHorz <= 0.0)
                    return;
                _dTargetResolutionHorz = value;
            }
        }

        double _dTargetResolutionVert = 1.0;
        public double TargetResolutionVert
        {
            get
            {
                return _dTargetResolutionVert;
            }
            set
            {
                if (_dTargetResolutionVert <= 0.0)
                    return;
                _dTargetResolutionVert = value;
            }
        }

        int _iLimitCount = 10;
        public int LimitCount
        {
            get
            {
                return _iLimitCount;
            }
            set
            {
                if (_iLimitCount <= 0)
                    return;
                _iLimitCount = value;
                          
            }
        }

        ThresholdParams [] _tpThresholdParam = new ThresholdParams[]{new ThresholdParams(), new ThresholdParams()};
        public ThresholdParams  ThresholdParamBaseCam
        {
            get
            {
                return _tpThresholdParam[0];
            }
        }

        public ThresholdParams ThresholdParamTargetCam
        {
            get
            {
                return _tpThresholdParam[1];
            }
        }

        private bool checkImage(EAdjustmentCameraType camno)
        {
            if (camno == _eBaseCamNo 
                || camno == _eTargetCamNo)
                return true;
            return false;
        }


        private bool checkMeasReady2()
        {
            bool bResult = true;
            foreach (EAdjustmentCameraType key in _diclstResult.Keys)
            {
                bResult |= _diclstResult[key].Exists(x => x.IsFindBlob());
            }
            return bResult;
        }

        private bool measPos2()
        {
            _bDoneMeas = false;

            if (!checkMeasReady2())
                return false;

            // カメラ１の画像が何枚目に来たかどうかを判断する
            int iCam1No = _diclstResult[_eBaseCamNo].FindIndex(x => x.IsFindBlob());
            double dCam1X = _diclstResult[_eBaseCamNo][iCam1No].ListBlobResult[0].GravityX;
            double dCam1Y = _diclstResult[_eBaseCamNo][iCam1No].ListBlobResult[0].GravityY;
            
            int iTargetCamNo = _diclstResult[_eTargetCamNo].FindIndex(x => x.IsFindBlob());
            double dTargetCamX = _diclstResult[_eTargetCamNo][iTargetCamNo].ListBlobResult[0].GravityX;
            double dTargetCamY = _diclstResult[_eTargetCamNo][iTargetCamNo].ListBlobResult[0].GravityY;

            // カメラ１の画面の高さを取得する
            double dOffsetScreen = (iCam1No - iTargetCamNo) * _iHeightSize * _dBaseResolutionVert;
            // 結果情報を求める
            MeasOffsetX = dCam1X - dTargetCamX;
            MeasOffsetY = dCam1Y - dTargetCamY + dOffsetScreen;
            _bDoneMeas = true;
            
            return true;

        }

        int _iHeightSize = -1;
        public bool AddImage2(HObject img, EAdjustmentCameraType eCamNo, out bool bMeasComplete )
        {
            bMeasComplete = false;

            if (!checkImage(eCamNo))
                return false;

            try
            {
                // 高さのサイズが
                if( _iHeightSize == -1 )
                {
                    HTuple htWidth, htHeight;
                    HOperatorSet.GetImageSize( img, out htWidth, out htHeight );
                    _iHeightSize = htHeight.I;
                }

                double dResH = (eCamNo == _eBaseCamNo) ? BaseResolutionHorz : TargetResolutionHorz;
                double dResV = (eCamNo == _eBaseCamNo) ? BaseResolutionVert : TargetResolutionVert;

                int iIndex = eCamNo == _eBaseCamNo ? 0 : 1;

                _diclstImageBuffer[eCamNo].AddImage(img);
                if (_diclstImageBuffer[eCamNo].IsReadyImage(_dicImageCount[eCamNo]))
                {
                    BlobResults BlobRes;
                    HObject hoAttachedImg;
                    int iOffsetY;
                    int iYMin;
                    int iYMax;

                    _diclstImageBuffer[eCamNo].GetAttachedImage(_dicImageCount[eCamNo], _iHeightSize / 4, out hoAttachedImg, out iOffsetY, out iYMin, out iYMax);
                    run(hoAttachedImg, _tpThresholdParam[iIndex], dResH, dResV, iYMin, iYMax, out BlobRes );
                    _diclstResult[eCamNo].Add(BlobRes);
                    _dicImageCount[eCamNo]++;

                    if (checkMeasReady2())
                    {
                        measPos2();
                        bMeasComplete = true;
                    }
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool run(HObject img, ThresholdParams param, double dResH, double dResV, double dYMin, double dYMax, out BlobResults result )
        {
            result = new BlobResults();
            HObject[] ahoObj = new HObject[10];
            HTuple htNumber, htArea, htRow, htCol;
            HTuple htRow1, htCol1, htRow2, htCol2;

            try
            {
                HOperatorSet.Threshold(img, out ahoObj[0], param.ThresholdLow, param.ThresholdHigh);
                HOperatorSet.OpeningCircle(ahoObj[0], out ahoObj[1], param.OpeningRadius);
                HOperatorSet.Connection(ahoObj[1], out ahoObj[2]);
                HOperatorSet.SelectShape(ahoObj[2], out ahoObj[3], 
                    new string[]{ "area", "width", "height", "row" }, 
                    "and", 
                    new double[]{ 1, param.LimitMinWidthMili / dResH, param.LimitMinHeightMili / dResV, dYMin }, 
                    new double[]{ 99999999, param.LimitMaxWidthMili / dResH , param.LimitMaxHeightMili / dResV, dYMax });

                HOperatorSet.CountObj(ahoObj[3], out htNumber);
                if (htNumber.I > 0)
                {
                    HOperatorSet.AreaCenter( ahoObj[3], out htArea, out htRow, out htCol );
                    HOperatorSet.SmallestRectangle1( ahoObj[3], out htRow1, out htCol1, out htRow2, out htCol2 );
                    for (int i = 0; i < htArea.Length; i++)
                    {
                        result.ListBlobResult.Add( new BlobResult( 
                            htArea[i].D * dResH * dResV, 
                            htCol[i].D * dResH, 
                            htRow[i].D * dResV,  
                            (htCol2[i].D - htCol1[i].D + 1.0) * dResH,  
                            (htRow2[i].D - htRow1[i].D + 1.0) * dResV
                            ));
                    }
                }
            }
            catch (HOperatorException) 
            {
                return false;
            }
            finally
            {
                for (int i = 0; i < ahoObj.Length; i++)
                    if (ahoObj[i] != null)
                        ahoObj[i].Dispose();
            }
            return true;
        }

        public bool AddImage(HObject img, EAdjustmentCameraType eCamNo)
        {
            if (!checkImage(eCamNo))
                return false;

            try
            {
                double dResH = (eCamNo == _eBaseCamNo) ? BaseResolutionHorz : TargetResolutionHorz;
                double dResV = (eCamNo == _eBaseCamNo) ? BaseResolutionVert : TargetResolutionVert;
                int iIndex = eCamNo == _eBaseCamNo ? 0 : 1;

                TestedImage timg = new TestedImage(img, dResH, dResV, _tpThresholdParam[iIndex]);
                if (!timg.Run())
                {
                    timg.Dispose();
                    return false;
                }
                else
                {
                    _diclstTestedImage[eCamNo].Add(timg);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool CheckLimitCountOver()
        {
            if (_diclstTestedImage[_eBaseCamNo].Count > LimitCount 
                || _diclstTestedImage[_eTargetCamNo].Count > LimitCount )
                return true;
            return false;
        }

        private bool checkMeasReady()
        {
            if (_diclstTestedImage[_eBaseCamNo].Count == 0 
                || _diclstTestedImage[_eTargetCamNo].Count == 0)
                return false;

            if (_diclstTestedImage[_eBaseCamNo].Exists(o => o.IsResultReady())
                && _diclstTestedImage[_eTargetCamNo].Exists( o => o.IsResultReady()))
            {
                return true;                
            }
            return false;
        }

        public bool MeasPos()
        {
            _bDoneMeas = false;

            if (!checkMeasReady())
                return false;

            // カメラ１の画像が何枚目に来たかどうかを判断する
            int iCam1No = -1;
            double dCam1X = 0d, dCam1Y = 0d;
            for (int i = 0; i < _diclstTestedImage[_eBaseCamNo].Count; i++)
            {
                if (_diclstTestedImage[_eBaseCamNo][i].IsResultReady())
                {
                    iCam1No = i;
                    dCam1X = _diclstTestedImage[_eBaseCamNo][i].BlobResults[0].GravityX;
                    dCam1Y = _diclstTestedImage[_eBaseCamNo][i].BlobResults[0].GravityY;
                    break;
                }
            }

            // 検査対象のカメラが何枚目に来たか判断する
            int iTargetCamNo = -1;
            double dTargetCamX = 0d, dTargetCamY = 0d;
            for (int i = 0; i < _diclstTestedImage[_eTargetCamNo].Count; i++)
            {
                if (_diclstTestedImage[_eTargetCamNo][i].IsResultReady())
                {
                    iTargetCamNo = i;
                    dTargetCamX = _diclstTestedImage[_eTargetCamNo][i].BlobResults[0].GravityX;
                    dTargetCamY = _diclstTestedImage[_eTargetCamNo][i].BlobResults[0].GravityY;
                    break;
                }
            }

            if (iCam1No == -1 || iTargetCamNo == -1)
                return false;

            // カメラ１の画面の高さを取得する
            double dOffsetScreen = (iCam1No - iTargetCamNo) * _diclstTestedImage[_eBaseCamNo][0].ImageHeight;

            // 結果情報を求める
            MeasOffsetX = dCam1X - dTargetCamX;
            MeasOffsetY = dCam1Y - dTargetCamY + dOffsetScreen;
            _bDoneMeas = true;

            return true;
        }
    }

    /// <summary>
    /// ブロブ結果
    /// </summary>
    class BlobResult
    {
        public double Area
        {
            get;
            private set;
        }
        public double GravityX
        {
            get;
            private set;
        }
        public double GravityY
        {
            get;
            private set;
        }

        public double Width
        {
            get;
            private set;
        }

        public double Height
        {
            get;
            private set;
        }

        public BlobResult(double area, double gx, double gy, double w, double h )
        {
            Area = area;
            GravityX = gx;
            GravityY = gy;
            Width = w;
            Height = h;
        }
    }

    /// <summary>
    /// しきい値パラメーター
    /// </summary>
    class ThresholdParams
    {
        public ThresholdParams()
        {
            ThresholdLow = 0;
            ThresholdHigh = 64;
            OpeningRadius = 0.5;
            ClosingRadius = 0.5;

            LimitMinHeightMili = 5;
            LimitMaxHeightMili = 50;

            LimitMinWidthMili = 5;
            LimitMaxWidthMili = 50;

        }

        public ThresholdParams(int iLow, int iHigh, double dRad )
        {
            ThresholdLow = iLow;
            ThresholdHigh = iHigh;
            OpeningRadius = dRad;
        }


        public int ThresholdLow
        {
            get;
            set;
        }

        public int ThresholdHigh
        {
            get;
            set;
        }

        public double OpeningRadius
        {
            get;
            set;
        }

        public double ClosingRadius
        {
            get;
            set;
        }

        public double LimitMinWidthMili
        {
            get;
            set;
        }
        public double LimitMaxWidthMili
        {
            get;
            set;
        }

        public double LimitMinHeightMili
        {
            get;
            set;
        }

        public double LimitMaxHeightMili
        {
            get;
            set;
        }

        public int InspectOffset
        {
            get;
            set;
        }

        public int InspectWidth
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 検査済みイメージ
    /// </summary>
    class TestedImage : IDisposable
    {
        HObject _hoImage;
        List<BlobResult> _lstBlobResult;
        ThresholdParams _params;

        // 概要:
        //     アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        public void Dispose()
        {
            if (_hoImage != null)
            {
                _hoImage.Dispose();
            }
        }

        public TestedImage(HObject img, double dResH, double dResV, ThresholdParams tparam )
        {
            if (img == null)
            {
                throw new ArgumentNullException();
            }

            _lstBlobResult = new List<BlobResult>();
            if (tparam == null)
            {
                tparam = new ThresholdParams();
            }
            _params = tparam;

            try
            {
                HTuple htClassVal;
                HOperatorSet.GetObjClass(img, out htClassVal);
                if (htClassVal.S != "image")
                {
                    throw new ArgumentException();
                }
                HOperatorSet.CopyObj(img, out _hoImage, 1, -1);
                ResolutionHorz = dResH;
                ResolutionVert = dResV;
            }
            catch (HOperatorException e)
            {
                throw e;
            }
            finally
            {
            }
        }

        public double ImageWidth
        {
            get
            {
                HTuple htWidth, htHeight;
                try
                {
                    HOperatorSet.GetImageSize(_hoImage, out htWidth, out htHeight);
                    return htWidth.D * ResolutionHorz;
                }
                catch (HOperatorException)
                {
                    return 0d;
                }
            }
        }

        public double ImageHeight
        {
            get
            {
                HTuple htWidth, htHeight;
                try
                {
                    HOperatorSet.GetImageSize(_hoImage, out htWidth, out htHeight);
                    return htHeight * ResolutionVert;
                }
                catch (HOperatorException)
                {
                    return 0d;
                }
            }
        }

        public bool IsResultReady()
        {
            return (_lstBlobResult.Count > 0);
        }

        public bool Run()
        {
            HObject [] hoObjs = new HObject[7];
            HTuple htNumber, htAreas, htRows, htColumns, htRow1, htCol1, htRow2, htCol2;
            HTuple htSortedAreaIndex;
            HTuple htWidth, htHeight;
            try
            {
                HOperatorSet.GetImageSize( _hoImage, out htWidth, out htHeight );

                int iCol1 = _params.InspectOffset;
                int iCol2 = _params.InspectWidth == 0? htWidth.I: iCol1 + _params.InspectWidth;
                HOperatorSet.GenRectangle1(out hoObjs[4], 0, iCol1, htHeight, iCol2);
                HOperatorSet.ReduceDomain(_hoImage, hoObjs[4], out hoObjs[5]);

                HOperatorSet.Threshold(hoObjs[5], out hoObjs[0], _params.ThresholdLow, _params.ThresholdHigh);
                HOperatorSet.OpeningCircle(hoObjs[0], out hoObjs[1], _params.OpeningRadius);
                HOperatorSet.ClosingCircle(hoObjs[1], out hoObjs[6], _params.ClosingRadius);
                HOperatorSet.Connection(hoObjs[6], out hoObjs[2]);
                HOperatorSet.SelectShape(hoObjs[2], out hoObjs[3], "area", "and", 1, 99999999.9);
                HOperatorSet.CountObj(hoObjs[3], out htNumber);
                if (htNumber > 0)
                {
                    HOperatorSet.AreaCenter(hoObjs[3], out htAreas, out htRows, out htColumns);
                    HOperatorSet.SmallestRectangle1(hoObjs[3], out htRow1, out htCol1, out htRow2, out htCol2);
                    HOperatorSet.TupleSortIndex(htAreas, out htSortedAreaIndex);
                    for (int i = htSortedAreaIndex.Length - 1; i >= 0 ; i--)
                    {
                        int iIndex = htSortedAreaIndex[i].I;
                        double dWidth = (htCol2[iIndex].D - htCol1[iIndex].D + 1d) * ResolutionHorz;
                        double dHeight = (htRow2[iIndex].D - htRow1[iIndex].D + 1d) * ResolutionVert;

                        if( dWidth >= _params.LimitMinWidthMili && dWidth <= _params.LimitMaxWidthMili 
                            && dHeight >= _params.LimitMinHeightMili && dHeight <= _params.LimitMaxHeightMili )
                        {
                            double dArea = htAreas[iIndex].D * ResolutionHorz * ResolutionVert;
                            double dGravX = htColumns[iIndex].D * ResolutionHorz;
                            double dGravY = htRows[iIndex].D * ResolutionVert;
                            _lstBlobResult.Add(new BlobResult(dArea, dGravX, dGravY, dWidth, dHeight));
                            System.Diagnostics.Debug.WriteLine(string.Format("area={0}/{1} : gravX={2}/{3} : gravY={4}/{5}", htAreas[iIndex].D, dArea, htColumns[iIndex].D, dGravX, htRows[iIndex].D, dGravY));

                        }
                    }
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                for (int i = 0; i < hoObjs.Length; i++)
                {
                    if (hoObjs[i] != null)
                    {
                        hoObjs[i].Dispose();
                    }
                }
            }
            return true;
        }

        public HObject Image
        {
            get
            {
                return _hoImage;
            }
        }

        public double ResolutionHorz
        {
            get;
            private set;
        }

        public double ResolutionVert
        {
            get;
            private set;
        }

        public List<BlobResult> BlobResults
        {
            get
            {
                return _lstBlobResult;
            }
        }
    }
}
