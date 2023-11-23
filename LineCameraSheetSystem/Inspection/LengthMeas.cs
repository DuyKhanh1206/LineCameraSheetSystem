using HalconCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InspectionNameSpace
{
    /// <summary>
    /// 測長監視クラス
    /// </summary>
    public class LengthMeas : IDisposable
    {
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// トータル長さ[mm]（イメージの開始位置）
        /// </summary>
        public double TotalStartLength { get; private set; }
        /// <summary>
        /// トータル長さ[mm]（イメージの終了位置）
        /// </summary>
        public double TotalLength { get; private set; }
        /// <summary>
        /// トータル長さ[pix]（イメージの開始位置）
        /// </summary>
        public long TotalStartPixel { get; private set; }
        /// <summary>
        /// トータル長さ[pix]（イメージの終了位置）
        /// </summary>
        public long TotalPixel { get; private set; }

        /// <summary>
        /// カメラ情報
        /// </summary>
        public CameraInfo CamInfo { get; private set; }

        /// <summary>
        /// １イメージ横の長さ[mm]
        /// </summary>
        public double ImageWidthLength { get; private set; }
        /// <summary>
        /// １イメージ縦の長さ[mm]
        /// </summary>
        public double ImageHeightLength { get; private set; }
        /// <summary>
        /// イメージ連結時の取込画像１枚の分可能長さ
        /// </summary>
        public double ConnectImageHeightLength { get; private set; }
        /// <summary>
        /// １連結済みイメージ縦の長さ[mm]
        /// </summary>
        public double ImageTileHeightLength { get; private set; }
        /// <summary>
        /// イメージ数
        /// </summary>
        private long _imageCount;

		public long ImageCount
		{
			get { return this._imageCount; }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="imageParam"></param>
        public LengthMeas(CameraInfo camInfo)
        {
            //カメラ情報
            this.CamInfo = camInfo;
        }
        public void Dispose()
        {
            EndLengthMeas();
        }

		public void SetImageLength()
		{
			System.Diagnostics.Debug.WriteLine(string.Format("LengthMeas  x = {0:F3} y = {1:F3}", this.CamInfo.ResolutionX, this.CamInfo.ResolutionY));

			this.ImageWidthLength = this.CamInfo.ImageWidth * this.CamInfo.ResolutionX;
			this.ImageHeightLength = this.CamInfo.ImageHeight * this.CamInfo.ResolutionY;
            this.ConnectImageHeightLength = this.CamInfo.ConnectImageHeight * this.CamInfo.ResolutionY;
			this.ImageTileHeightLength = this.CamInfo.ImageTileHeight * this.CamInfo.ResolutionY;
		}

        public void BeginLengthMeas()
        {
            //イメージの長さ[mm]
			this.SetImageLength();
            //
            Clear();
        }
        public void EndLengthMeas()
        {
            Clear();
        }

        /// <summary>
        /// 更新する
        /// </summary>
        public void Updates()
        {
            this.Time = DateTime.Now;

            HalconCameraBase cam = APCameraManager.getInstance().GetCamera(0);
            bool IsConnectImage = cam.IsConnectVerticalImage;
            int iConnectPoint = cam.ImageConnectCnt - LineCameraSheetSystem.SystemParam.GetInstance().InspArea_ConnectMode_ImagePoint;
            //開始[mm]
            this.TotalStartLength = this.ConnectImageHeightLength * (this._imageCount + iConnectPoint);
            this.TotalStartLength = Utility.ToRound(this.TotalStartLength);
            //開始[pix]
            this.TotalStartPixel = this.CamInfo.ConnectImageHeight * this._imageCount;

            //取得イメージをカウントアップする
            this._imageCount += 1;

            //終了[mm]
            //this.TotalLength = this.ImageTileHeightLength * this._imageCount;
            this.TotalLength = this.ConnectImageHeightLength * (this._imageCount + iConnectPoint);
            this.TotalLength = Utility.ToRound(this.TotalLength);
            //終了[pix]
            //this.TotalPixel = this._cameraInfo.ImageTileHeight * this._imageCount;
            this.TotalPixel = this.CamInfo.ImageHeight * (this._imageCount + iConnectPoint);
        }
        /// <summary>
        /// クリアする
        /// </summary>
        public void Clear()
        {
            //更新時間
            this.Time = new DateTime();
            //[mm]
            this.TotalStartLength = 0.0;
            this.TotalLength = 0.0;
            //[pix]
            this.TotalStartPixel = 0;
            this.TotalPixel = 0;
            //イメージ数
            this._imageCount = 0;
        }
    }
}
