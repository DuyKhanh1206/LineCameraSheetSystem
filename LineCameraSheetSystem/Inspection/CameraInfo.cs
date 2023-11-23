using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LineCameraSheetSystem;

namespace InspectionNameSpace
{
    /// <summary>
    /// カメラ情報
    /// </summary>
    public class CameraInfo
    {
        /// <summary>
        /// カメラの有効・無効
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// カメラ部位
        /// </summary>
        public AppData.SideID CamSide { get; private set; }
        /// <summary>
        /// カメラ番号
        /// </summary>
        public AppData.CamID CamNo { get; private set; }
        /// <summary>
        /// 横分解能(mm)
        /// </summary>
        public double ResolutionX { get; set; }
        /// <summary>
        /// 縦分解能(mm)
        /// </summary>
        public double ResolutionY { get; set; }

        /// <summary>
        /// 基準カメラからの横オフセット値(mm)
        /// </summary>
        public double OffsetX { get; set; }
        //public int OffsetXpix { get; private set; }
        /// <summary>
        /// 基準カメラからの縦オフセット値(mm)
        /// </summary>
        public double OffsetY { get; set; }
        //public int OffsetYpix { get; private set; }

        /// <summary>
        /// イメージ横(pix)
        /// </summary>
        public int ImageWidth { get; private set; }
        /// <summary>
        /// イメージ縦(pix)
        /// </summary>
        public int ImageHeight { get; private set; }
        public int ConnectImageHeight { get; private set; }
        /// <summary>
        /// 結合後イメージ高さ(pix)
        /// </summary>
        public int ImageTileHeight { get; private set; }
        /// <summary>
        /// 結合サイズ(pix)
        /// </summary>
        public int OverLapLines { get; private set; }
        /// <summary>
        /// 破棄する枚数
        /// </summary>
        public int DiscardCount { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="enabled">カメラ有無 true:有効 false:無効</param>
		/// <param name="camSide">カメラ部位</param>
		/// <param name="camNo">カメラ番号</param>
		/// <param name="camPosition">カメラポジション</param>
		/// <param name="resolutionX">横分解能(mm)</param>
        /// <param name="resolutionY">縦分解能(mm)</param>
        /// <param name="offsetX">横オフセット値(mm)</param>
        /// <param name="offsetY">縦オフセット値(mm)</param>
        /// <param name="imageWidth">イメージ横サイズ(pix)</param>
        /// <param name="imageHeight">イメージ縦サイズ(pix)</param>
        /// <param name="overlapLines">イメージ連結サイズ(pix)</param>
        public CameraInfo(bool enabled, AppData.SideID camSide, AppData.CamID camNo, double resolutionX, double resolutionY, double offsetX, double offsetY, int imageWidth, int imageHeight, int connectOneImageHeight, int discardCnt)
        {
            this.Enabled = enabled;
            this.CamSide = camSide;
            this.CamNo = camNo;
            this.ResolutionX = resolutionX;
            this.ResolutionY = resolutionY;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
            this.ConnectImageHeight = connectOneImageHeight;
            this.ImageTileHeight = imageHeight;
            this.DiscardCount = discardCnt;
        }
    }
}
