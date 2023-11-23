using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HalconDotNet;
using KaTool;
using LineCameraSheetSystem;

namespace HalconCamera
{
    /// <summary>
    /// Args Class
    /// </summary>
    public class ConnectImageArgs : EventArgs, IDisposable
    {
        /// <summary>
        /// 連結イメージ
        /// </summary>
        public HObject ConnectedImage { get { return _connectedImage; } }
        /// <summary>
        /// 取込イメージ
        /// </summary>
        public HObject OriginalImage { get { return _originalImage; } }

        private HObject _connectedImage;
        private HObject _originalImage;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectImage">連結イメージ</param>
        /// <param name="originalImage">取込イメージ</param>
        public ConnectImageArgs(HObject connectImage, HObject originalImage)
        {
            ImageTool.CopyHalconImage(connectImage, out _connectedImage);
            ImageTool.CopyHalconImage(originalImage, out _originalImage);
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            ImageTool.ClearHalconObject(ref _connectedImage);
            ImageTool.ClearHalconObject(ref _originalImage);
        }
    }


    public class clsConnectImage : IDisposable
    {
        /// <summary>
        /// 取込イメージ幅
        /// </summary>
        public int OneImageWidth { get; private set; }
        /// <summary>
        /// 取込イメージ高さ
        /// </summary>
        public int OneImageHeight { get; private set; }
        /// <summary>
        /// 連結イメージ幅
        /// </summary>
        public int ConnectImageWidth { get; private set; }
        /// <summary>
        /// 連結イメージ高さ
        /// </summary>
        public int ConnectImageHeight { get; private set; }
        /// <summary>
        /// 連結イメージ数
        /// </summary>
        public int ConnectCount { get; private set; }
        /// <summary>
        /// 連結イメージ
        /// </summary>
        private HObject _connectedImage;
        private HObject _connectedShadingImage;
        private Object _lockConnectImage = new Object();

        private int _cameraNum;
        private List<int> _horizontalOffset = new List<int>();
        private List<int> _verticalOffset = new List<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="imgWidth">取込イメージ幅</param>
        /// <param name="imgHeight">取込イメージ高さ</param>
        public clsConnectImage(int cameraNum, int imgWidth, int imgHeight)
        {
            //カメラ台数
            _cameraNum = cameraNum;
            //カメラサイズ
            OneImageWidth = imgWidth;
            OneImageHeight = imgHeight;
        }
        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="connectNum"></param>
        /// <param name="horizontalOffset"></param>
        /// <param name="verticalOffset"></param>
		public void InitSetImageParameters(int iChannel, int connectNum, List<int> horizontalOffset, List<int> verticalOffset)
        {
            lock (_lockConnectImage)
            {
                //オフセット
                _horizontalOffset.Clear();
                _verticalOffset.Clear();
                for (int i = 0; i < _cameraNum; i++)
                {
                    _horizontalOffset.Add(horizontalOffset[i]);
                    _verticalOffset.Add(verticalOffset[i]);
                }
                //イメージ縦方向の連結数
                ConnectCount = connectNum;
                //連結イメージ　Width
                ConnectImageWidth = _horizontalOffset[_cameraNum - 1] + OneImageWidth;
                //連結イメージ　Height
                int min = int.MaxValue;
                int max = int.MinValue;
                foreach (int voff in _verticalOffset)
                {
                    if (voff < min)
                        min = voff;
                    if (voff > max)
                        max = voff;
                }
                int h = OneImageHeight - (Math.Abs(min) + Math.Abs(max));
                OneImageHeight = h;
                ConnectImageHeight = OneImageHeight * this.ConnectCount;
                //
                ImageTool.ClearHalconObject(ref _connectedImage);
                ImageTool.ClearHalconObject(ref _connectedShadingImage);
                if (iChannel == 1)
                {
                    ImageTool.InitializeImage(ConnectImageWidth, OneImageHeight * 2, out _connectedImage, 255);
                    ImageTool.InitializeImage(ConnectImageWidth, ConnectImageHeight, out _connectedShadingImage, 255);
                }
                else
                {
                    ImageTool.InitializeImage3(ConnectImageWidth, OneImageHeight * 2, out _connectedImage, 255);
                    ImageTool.InitializeImage3(ConnectImageWidth, ConnectImageHeight, out _connectedShadingImage, 255);
                }
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            ImageTool.ClearHalconObject(ref _connectedImage);
            ImageTool.ClearHalconObject(ref _connectedShadingImage);
        }

        int[] _zeroOffset = new int[] { 0, 0 };
        int[] _minusOffset = new int[] { -1, -1 };

        /// <summary>
        /// 連結イメージの取得
        /// </summary>
        /// <param name="hoMultiImage">横連結時の複数イメージ</param>
        /// <param name="hoConnectionImage">連結イメージ</param>
        public void ConnectImage(List<HObject> hoMultiImage, out HObject hoConnectionImage, out HObject hoShadingImage)
        {
            hoConnectionImage = null;
            hoShadingImage = null;
            //
            HObject objHorizontalTileImage = null;
            HObject objConcatImage = null;
            HObject OTemp = null;
            //
            HObject tiledImageMinusOldest = null;
            HObject imagesToTile = null;

            HOperatorSet.GenEmptyObj(out objHorizontalTileImage);
            HOperatorSet.GenEmptyObj(out objConcatImage);
            HOperatorSet.GenEmptyObj(out OTemp);
            HOperatorSet.GenEmptyObj(out tiledImageMinusOldest);
            HOperatorSet.GenEmptyObj(out imagesToTile);

            HObject img;
            HOperatorSet.GenEmptyObj(out img);

            int connCnt = ConnectCount;

            try
            {
                lock (_lockConnectImage)
                {
                    for (int no = 0; no < 2; no++)
                    {
                        img.Dispose();
                        switch (no)
                        {
                            case 0:
                                if (SystemParam.GetInstance().IM_OrgImageConnectMode == false)
                                {
                                    if (ConnectCount > 1)
                                        connCnt = SystemParam.GetInstance().InspArea_ConnectMode_ImagePoint;
                                }
                                else
                                {
                                    connCnt = ConnectCount;
                                }
                                HOperatorSet.CopyObj(_connectedImage, out img, 1, -1);
                                break;
                            default:
                                connCnt = ConnectCount;
                                HOperatorSet.CopyObj(_connectedShadingImage, out img, 1, -1);
                                break;
                        }

                        //横方向のイメージ連結
                        HTuple htHorizontalOffset = new HTuple();
                        HTuple htVerticalOffset = new HTuple();
                        HTuple defZero = new HTuple();
                        HTuple defMinus = new HTuple();
                        objConcatImage.Dispose();
                        HOperatorSet.GenEmptyObj(out objConcatImage);
                        int camNo = 0;
                        foreach (HObject image in hoMultiImage)
                        {
                            //
                            OTemp.Dispose();
                            HOperatorSet.ConcatObj(objConcatImage, image, out OTemp);
                            KaTool.ImageTool.ClearHalconObject(ref objConcatImage);
                            objConcatImage = OTemp;

                            HOperatorSet.TupleConcat(htHorizontalOffset, _horizontalOffset[camNo], out htHorizontalOffset);
                            HOperatorSet.TupleConcat(htVerticalOffset, _verticalOffset[camNo], out htVerticalOffset);
                            HOperatorSet.TupleConcat(defZero, 0, out defZero);
                            HOperatorSet.TupleConcat(defMinus, -1, out defMinus);
                            camNo++;
                        }
                        objHorizontalTileImage.Dispose();
                        HOperatorSet.TileImagesOffset(objConcatImage, out objHorizontalTileImage, htVerticalOffset, htHorizontalOffset, defMinus, defMinus, defMinus, defMinus, ConnectImageWidth, OneImageHeight);

                        if (connCnt > 1)
                        {
                            //縦方向のイメージ連結
                            tiledImageMinusOldest.Dispose();
                            HOperatorSet.CropPart(img, out tiledImageMinusOldest, OneImageHeight, 0, ConnectImageWidth, OneImageHeight * (connCnt - 1));

                            imagesToTile.Dispose();
                            HOperatorSet.ConcatObj(tiledImageMinusOldest, objHorizontalTileImage, out imagesToTile);

                            HTuple htWidth, htHeight;
                            HOperatorSet.GetImageSize(imagesToTile, out htWidth, out htHeight);

                            int iHeight = 0;
                            for (int i = 0; i < htHeight.Length; i++)
                                iHeight += htHeight[i].I;

                            int[] _connectOffset = new int[] { 0, (connCnt - 1) * OneImageHeight };
                            img.Dispose();
                            HOperatorSet.TileImagesOffset(imagesToTile, out img,
                                _connectOffset,
                                _zeroOffset,
                                _minusOffset, _minusOffset, _minusOffset, _minusOffset,
                                ConnectImageWidth, iHeight);
                        }
                        else
                        {
                            img.Dispose();
                            ImageTool.CopyHalconImage(objHorizontalTileImage, out img);
                        }

                        switch (no)
                        {
                            case 0:
                                _connectedImage.Dispose();
                                HOperatorSet.CopyObj(img, out _connectedImage, 1, -1);
                                break;
                            default:
                                _connectedShadingImage.Dispose();
                                HOperatorSet.CopyObj(img, out _connectedShadingImage, 1, -1);
                                break;
                        }
                    }
                }
                HOperatorSet.CopyObj(_connectedImage, out hoConnectionImage, 1, -1);
                HOperatorSet.CopyObj(_connectedShadingImage, out hoShadingImage, 1, -1);
            }
            catch (Exception exc)
            {
                string msgStr = string.Format("{0} : {1}", this.GetType() + " " + System.Reflection.MethodBase.GetCurrentMethod(), exc.Message);
                LogingDllWrap.LogingDll.Loging_SetLogString(msgStr);
            }
            finally
            {
                ImageTool.ClearHalconObject(ref objHorizontalTileImage);
                ImageTool.ClearHalconObject(ref objConcatImage);
                ImageTool.ClearHalconObject(ref OTemp);
                ImageTool.ClearHalconObject(ref tiledImageMinusOldest);
                ImageTool.ClearHalconObject(ref imagesToTile);
                ImageTool.ClearHalconObject(ref img);
            }
        }
    }
}
