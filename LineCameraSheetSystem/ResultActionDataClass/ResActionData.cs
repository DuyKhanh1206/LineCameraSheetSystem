using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using LineCameraSheetSystem;
using HalconDotNet;

namespace ResultActionDataClassNameSpace
{
    /// <summary>
    /// 項目データ
    /// </summary>
    public class ResActionData : IDisposable
    {
        /// <summary>
        /// Image\Numberフォルダ内の最大イメージ数
        /// </summary>
        public int ImageNumDirFileMax { get; set; }
        /// <summary>
        /// イメージを保存するディレクトリ
        /// </summary>
        private string _systemImageDir;

        #region 共通項目
        /// <summary>
        /// 通し番号(1～)
        /// </summary>
        public int LineNo { get; private set; }
		/// <summary>
		/// 結果通し番号(1～)
		/// </summary>
		public int ResultNo { get; private set; }
        /// <summary>
        /// イベント識別ID
        /// </summary>
        public ResultActionDataClass.EEventId EventId { get; private set; }
        /// <summary>
        /// イベントモード
        /// </summary>
        public ResultActionDataClass.EEventMode EventMode { get; private set; }

        /// <summary>
        /// 開始位置
        /// </summary>
        public double StartPosition { get; private set; }
        /// <summary>
        /// 終了位置
        /// </summary>
        public double EndPosition { get; private set; }
        /// <summary>
        /// 発生時刻
        /// </summary>
        public DateTime Time { get; private set; }
        #endregion


        #region 結果データ項目
        /// <summary>
        /// カメラ番号
        /// </summary>
        public AppData.CamID CamId { get; private set; }
        /// <summary>
        /// カメラ部位
        /// </summary>
        public AppData.SideID SideId { get; private set; }
        /// <summary>
        /// NG項目
        /// </summary>
        public AppData.InspID InspId { get; private set; }

        /// <summary>
        /// 縦位置
        /// </summary>
        public double PositionY { get; private set; }
        /// <summary>
        /// 横位置（横アドレス）
        /// </summary>
        public double PositionX { get; private set; }
        /// <summary>
        /// 横サイズ
        /// </summary>
        public double Width { get; private set; }
        /// <summary>
        /// 縦サイズ
        /// </summary>
        public double Height { get; private set; }
        /// <summary>
        /// 面積
        /// </summary>
        public double Area { get; private set; }
        /// <summary>
        /// ゾーン番号(1-...)
        /// </summary>
        public AppData.ZoneID ZoneId { get; private set; }
        /// <summary>
        /// NGイメージ
        /// </summary>
        private HObject _image = null;
        /// <summary>
        /// NGイメージファイル名
        /// </summary>
        public string ImageFileName { get; private set; }
        #endregion


        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="lineNo">通し番号</param>
        /// <param name="id">イベント識別</param>
        /// <param name="mode">イベントモード</param>
        /// <param name="st">開始位置</param>
        /// <param name="ed">終了位置</param>
        /// <param name="time">発生時刻</param>
        public ResActionData(int lineNo, int resultNo, ResultActionDataClass.EEventId id, ResultActionDataClass.EEventMode mode, double st, double end, DateTime time)
        {
            this.LineNo = lineNo;
			this.ResultNo = resultNo;
            this.EventId = id;
            this.EventMode = mode;
            this.StartPosition = st;
            this.EndPosition = end;
            this.Time = time;
            this._systemImageDir = null;
            this.PositionY = end;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            if (this._image != null)
            {
                this._image.Dispose();
            }
        }
        #endregion


        #region 詳細部のデータ(Result)を格納する
        /// <summary>
        /// 詳細部データを格納する
        /// </summary>
        /// <param name="camId"></param>
        /// <param name="inspId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="area"></param>
        /// <param name="zoneId"></param>
        /// <param name="image"></param>
        /// <param name="imageFileName"></param>
        public void CreateDetail(AppData.CamID camId, AppData.SideID sideId, AppData.InspID inspId, double x, double y, double w, double h, double area, AppData.ZoneID zoneId, HObject image, string imageFileName, string imageDir, out HObject hoSaveImages)
        {
            hoSaveImages = null;

            this.CamId = camId;
            this.SideId = sideId;
            this.InspId = inspId;
            this.PositionX = x;
            this.PositionY = y;
            this.Width = w;
            this.Height = h;
            this.Area = area;
            this.ZoneId = zoneId;
            this._systemImageDir = imageDir;

			this.ImageFileName = imageFileName;
            if(_image!=null)
            {
                _image.Dispose();
                _image = null;
            }
			if (image != null)
            {
                CreateSaveImages(image, out _image);
                HOperatorSet.CopyObj(_image, out hoSaveImages, 1, -1);
            }
        }
        #endregion

        /// <summary>
        /// 保存するイメージ群を生成する
        /// </summary>
        private void CreateSaveImages(HObject image, out HObject saveImages)
        {
            HOperatorSet.GenEmptyObj(out saveImages);

            HObject targetImage;
            HObject hoImageGray;
            HObject hoImageR, hoImageG, hoImageB;
            HObject hoImageMin1;
            HObject hoImageMin2;
            HObject hoImageMax1;
            HObject hoImageMax2;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out targetImage);
            HOperatorSet.GenEmptyObj(out hoImageGray);
            HOperatorSet.GenEmptyObj(out hoImageR);
            HOperatorSet.GenEmptyObj(out hoImageG);
            HOperatorSet.GenEmptyObj(out hoImageB);
            HOperatorSet.GenEmptyObj(out hoImageMin1);
            HOperatorSet.GenEmptyObj(out hoImageMin2);
            HOperatorSet.GenEmptyObj(out hoImageMax1);
            HOperatorSet.GenEmptyObj(out hoImageMax2);
            HOperatorSet.GenEmptyObj(out dmy);

            HTuple htCount;
            int iCnt;
            try
            {
                if (image != null)
                {
                    HOperatorSet.SelectObj(image, out hoImageGray, 3);

                    HOperatorSet.CountObj(image, out htCount);
                    if (htCount.I == 6)
                    {
                        HOperatorSet.SelectObj(image, out hoImageR, 4);
                        HOperatorSet.SelectObj(image, out hoImageG, 5);
                        HOperatorSet.SelectObj(image, out hoImageB, 6);
                    }
                    else
                    {
                        HOperatorSet.SelectObj(image, out hoImageR, 3);
                        HOperatorSet.SelectObj(image, out hoImageG, 3);
                        HOperatorSet.SelectObj(image, out hoImageB, 3);
                    }

                    string dir;
                    string file;
                    string[] fooder;
                    SystemParam.GetInstance().GetDirAndFile(this._systemImageDir, this.ImageFileName, out dir, out file, out fooder);
                    HOperatorSet.CountObj(image, out htCount);
                    iCnt = fooder.Length;
                    for (int i = 0; i < iCnt && i < SystemParam.GetInstance().IM_NgCropSaveCount; i++)
                    {
                        targetImage.Dispose();
                        if (i == 2)
                        {
                            //暗生成
                            hoImageMin1.Dispose();
                            hoImageMin2.Dispose();
                            HOperatorSet.MinImage(hoImageGray, hoImageR, out hoImageMin1);
                            HOperatorSet.MinImage(hoImageMin1, hoImageG, out hoImageMin2);
                            HOperatorSet.MinImage(hoImageMin2, hoImageB, out targetImage);
                        }
                        else if (i == 3)
                        {
                            //明作成
                            hoImageMax1.Dispose();
                            hoImageMax2.Dispose();
                            HOperatorSet.MaxImage(hoImageGray, hoImageR, out hoImageMax1);
                            HOperatorSet.MaxImage(hoImageMax1, hoImageG, out hoImageMax2);
                            HOperatorSet.MaxImage(hoImageMax2, hoImageB, out targetImage);
                        }
                        else
                        {
                            //検査・オリジナル
                            HOperatorSet.SelectObj(image, out targetImage, i + 1);
                        }

                        dmy.Dispose();
                        HOperatorSet.CopyObj(saveImages, out dmy, 1, -1);
                        saveImages.Dispose();
                        HOperatorSet.ConcatObj(dmy, targetImage, out saveImages);
                    }
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                targetImage.Dispose();
                hoImageGray.Dispose();
                hoImageR.Dispose();
                hoImageG.Dispose();
                hoImageB.Dispose();
                hoImageMin1.Dispose();
                hoImageMin2.Dispose();
                hoImageMax1.Dispose();
                hoImageMax2.Dispose();
                dmy.Dispose();
            }
        }


        public ResActionData Copy()
        {
            ResActionData resData = (ResActionData)this.MemberwiseClone();
            return resData;
        }

        /// <summary>
        /// イメージファイル名を生成する
        /// </summary>
        private string MakeImageFileName(ref HObject image)
        {
            //Imageフォルダ内の全ディレクトリ名を取得する
            DirectoryInfo di = new DirectoryInfo(this._systemImageDir);
            DirectoryInfo[] dirs = di.GetDirectories();

            //Imageフォルダ内のディレクトリ名を降順にソートする
            Array.Sort(dirs, delegate(DirectoryInfo d1, DirectoryInfo d2)
            {
                return d2.Name.CompareTo(d1.Name);
            });

            //最大番号のフォルダ名
            string numdir = (dirs.Length == 0) ? "0000000000" : dirs[0].Name;
            string numPath = Path.Combine(this._systemImageDir, numdir);
            if (Directory.Exists(numPath) == false)
            {
                Directory.CreateDirectory(numPath);
            }

            //フォルダ内のファイル数を取得して、MAX以上ならば新規の番号フォルダにする
            string[] files = Directory.GetFiles(numPath);
            int maxFileNo = files.Length;
            if (files.Length >= this.ImageNumDirFileMax)
            {
                //最大番号+1のフォルダ名にする
                numdir = string.Format("{0}", (int.Parse(numdir) + 1).ToString("D010"));
                numPath = Path.Combine(this._systemImageDir, numdir);
                if (Directory.Exists(numPath) == false)
                {
                    Directory.CreateDirectory(numPath);
                }
                maxFileNo = 0;
            }
            string fileName = Path.Combine(numdir, maxFileNo.ToString("D04")) + ".bmp";
            return fileName;
        }

        /// <summary>
        /// イメージを取得する
        /// </summary>
        /// <returns>イメージデータ</returns>
        public void GetImage(ref HObject colorMultiImage, ref HObject grayMultiImage)
        {
            HOperatorSet.GenEmptyObj(out colorMultiImage);
            HOperatorSet.GenEmptyObj(out grayMultiImage);

            HObject img = null;
            HObject hoReadImage;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoReadImage);
            HOperatorSet.GenEmptyObj(out dmy);

            HTuple htCount;

            string dir;
            string file;
            string[] fooder;

            try
            {
                if (this._image != null)
                {
                    HOperatorSet.CopyObj(this._image, out img, 1, -1);
                }

                if (img != null || (string.IsNullOrEmpty(this._systemImageDir) == false && string.IsNullOrEmpty(this.ImageFileName) == false))
                {
                    SystemParam.GetInstance().GetDirAndFile(this._systemImageDir, this.ImageFileName, out dir, out file, out fooder);

                    for (int i = 0; i < fooder.Length; i++)
                    {
                        string fPath;
                        if (img == null)
                        {
                            fPath = Path.Combine(dir, file) + fooder[i] + ".bmp";
                            if (File.Exists(fPath) == true)
                            {
                                hoReadImage.Dispose();
                                HOperatorSet.ReadImage(out hoReadImage, fPath);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            HOperatorSet.CountObj(img, out htCount);
                            hoReadImage.Dispose();
                            if ((i + 1) <= htCount.I)
                                HOperatorSet.SelectObj(img, out hoReadImage, i + 1);
                            else
                                HOperatorSet.SelectObj(img, out hoReadImage, 1);
                        }

                        if (i==0)
                            HOperatorSet.ConcatObj(hoReadImage, hoReadImage, out grayMultiImage);

                        if (i < 2)
                        {
                            //カラー２枚
                            dmy.Dispose();
                            HOperatorSet.CopyObj(colorMultiImage, out dmy, 1, -1);
                            colorMultiImage.Dispose();
                            HOperatorSet.ConcatObj(dmy, hoReadImage, out colorMultiImage);
                        }
                        else
                        {
                            if (i == 2)
                            {
                                grayMultiImage.Dispose();
                                HOperatorSet.GenEmptyObj(out grayMultiImage);
                            }
                            //モノクロ2枚
                            dmy.Dispose();
                            HOperatorSet.CopyObj(grayMultiImage, out dmy, 1, -1);
                            grayMultiImage.Dispose();
                            HOperatorSet.ConcatObj(dmy, hoReadImage, out grayMultiImage);
                        }
                    }
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                if (img != null)
                    img.Dispose();
                hoReadImage.Dispose();
                dmy.Dispose();
            }
        }
        private void GetFileNamePath(out string targetFilePath, out string baseFilePath)
        {
            targetFilePath = "";
            baseFilePath = "";

            string path = Path.Combine(this._systemImageDir, this.ImageFileName);
            string dir = Path.GetDirectoryName(path);
            string targetName = Path.GetFileNameWithoutExtension(path);
            string baseName = targetName + "a";
            targetFilePath = Path.Combine(dir, targetName) + ".bmp";
            baseFilePath = Path.Combine(dir, baseName) + ".bmp";
        }
    }
}
