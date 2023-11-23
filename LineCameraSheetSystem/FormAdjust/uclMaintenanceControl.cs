using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using ViewROI;
using HalconCamera;

//namespace LineCameraSheetSystem
namespace Adjustment
{
    public partial class uclMaintenanceControl : UserControl
    {
        public uclMaintenanceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ユーザコントロールのデータ
        /// </summary>
        public class userContData
        {
            public int x = 0,y =0 ,w=0 ,h=0;//座標位置、幅、高さ
            public string stdGrayValue="";//グレー値
            public string nowValue = "";//現在値
            public bool ControlEnable = false;//ユーザコントロール有効無効
        }

        public userContData UserContData = new userContData(); //インスタンス
        
        /// <summary>
        /// 設定読込時、ユーザコントロールclassの値を表示器に反映
        /// </summary>
        public void userContDataUpdate()
        {
            if (UserContData.x >= numOriginX.Minimum && UserContData.x <= numOriginX.Maximum)
            {
                numOriginX.Value = UserContData.x;
            }
            else
            {
                MessageBox.Show(labelTitle.Text+"のROI設定（X）に設定範囲外の値が入力されました。");
            }

            if (UserContData.y >= numOriginY.Minimum && UserContData.y <= numOriginY.Maximum)
            {
                numOriginY.Value = UserContData.y;
            }
            else
            {
                MessageBox.Show(labelTitle.Text + "のROI設定（Y）に設定範囲外の値が入力されました。");
            }

            if (UserContData.w >= numWidth.Minimum && UserContData.w <= numWidth.Maximum)
            {
                numWidth.Value = UserContData.w;
            }
            else
            {
                MessageBox.Show(labelTitle.Text + "のROI設定（W）に設定範囲外の値が入力されました。");
            }

            if (UserContData.h >= numHeight.Minimum && UserContData.h <= numHeight.Maximum)
            {
                numHeight.Value = UserContData.h;
            }
            else
            {
                MessageBox.Show(labelTitle.Text + "のROI設定（H）に設定範囲外の値が入力されました。");
            }

            textStdGrayValue.Text = UserContData.stdGrayValue;
        }

        private void numOriginX_TextChanged(object sender, EventArgs e)
        {
            UserContData.x = (int)numOriginX.Value;
            string[] HalcomCameraType = new string[] { "HalconCameraPylonGigELineSensor", "HalconCameraPylonGigE" };

            if (_Camera!=null&& Array.IndexOf(HalcomCameraType, _Camera.GetType().Name) >= 0)
                 numWidth.Maximum = _Camera.ImageWidth - 1 - (int)numOriginX.Value;//幅Wの最大値を（画像サイズ－X新値）で設定

        }//ユーザ値変更に伴い、UserContDataに値代入
        private void numOriginY_TextChanged(object sender, EventArgs e)
        {
            UserContData.y = (int)numOriginY.Value;
            string[] HalcomCameraType = new string[] { "HalconCameraPylonGigELineSensor", "HalconCameraPylonGigE" };

            if (_Camera != null&& Array.IndexOf(HalcomCameraType, _Camera.GetType().Name)>=0)
                numHeight.Maximum = _Camera.ImageHeight - 1 - (int)numOriginY.Value;//高さHの最大値を（画像サイズ－Y新値）で設定
        }//ユーザ値変更に伴い、UserContDataに値代入
        private void numWidth_TextChanged(object sender, EventArgs e)
        {
            UserContData.w = (int)numWidth.Value;
        }//ユーザ値変更に伴い、UserContDataに値代入
        private void numHeight_TextChanged(object sender, EventArgs e)
        {
            UserContData.h = (int)numHeight.Value;
        }//ユーザ値変更に伴い、UserContDataに値代入

        HalconCamera.HalconCameraBase _Camera = null;

        /// <summary>
        /// cameraハンドルを受け取り、XやYのMin、Max、Increment、Valueを決定する。
        /// また、updateControls()で有効無効も併せて設定。
        /// 初回呼び出し専用。
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public bool SetCamera(HalconCamera.HalconCameraBase camera)
        {
            _Camera = camera;

            if (_Camera == null)
            {
                updateControls();
                return false;
            }
            try
            {
                if (_Camera!=null)
                {
                    int imageWidth=4096, imageHeight=4096;
                    if (_Camera.ImageWidth > 0)
                        imageWidth = _Camera.ImageWidth;

                    if (_Camera.ImageHeight > 0)
                        imageHeight = _Camera.ImageHeight;

                    labelTitle.Text = _Camera.Name;

                    numOriginX.Minimum = 0;
                    numOriginX.Maximum= imageWidth - 2;//0からなので-1、widthとheightがあるのでさらに-1
                    numOriginX.Increment = 1;
                    numOriginX.Value = 0;

                    numOriginY.Minimum = 0;
                    numOriginY.Maximum = imageHeight - 2;//0からなので-1、widthとheightがあるのでさらに-1
                    numOriginY.Increment = 1;
                    numOriginY.Value = 0;

                    numWidth.Minimum = 1;
                    numWidth.Maximum = imageWidth - 1 - numOriginX.Value;
                    numWidth.Increment = 1;
                    numWidth.Value = 1;

                    numHeight.Minimum = 1;
                    numHeight.Maximum = imageHeight - 1 - numOriginY.Value;
                    numHeight.Increment = 1;
                    numHeight.Value = 1;

                    UserContData.ControlEnable = true;//ユーザコントロール有効
                }
                else
                {
                    UserContData.ControlEnable = false;//ユーザコントロール無効
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                updateControls();
            }
            return true;
        }

        /// <summary>
        /// 有効無効の設定。
        /// 各表示器は、デフォルトEnabled＝falseのため、有効にする場合のみ、EnabledをTrueにする。
        /// </summary>
        private void updateControls()
        {
            string[] ctrlNames = new string[] { "textStdGrayValue","textGrayValue", "btnLine1", "btnLine2" };//ここのコントロールは有効にしない（無効のまま）
            foreach (Control c in Controls)
            {
                if (_Camera != null && Array.IndexOf(ctrlNames, c.Name)<0)
                    c.Enabled=true;
            }
        }

        /// <summary>
        /// グレー値の表示受け取り用
        /// </summary>
        /// <param name="value"></param>
        public void glay(double value)
        {
            textGrayValue.Text = value.ToString("f2");
            this.UserContData.nowValue = textGrayValue.Text;
        }

        /// <summary>
        /// グレー値（現在値）を基準グレー値にコピー
        /// </summary>
        public void CopyStdGrayValue()
        {
            UserContData.stdGrayValue = textGrayValue.Text;
            textStdGrayValue.Text = textGrayValue.Text;
        }

        /// <summary>
        ///　基準グレー値に代入
        /// </summary>
        /// <param name="value"></param>
        public void SettingStdGrayValue(string value)
        {
            UserContData.stdGrayValue = value;
            textStdGrayValue.Text = value;
        }
    }
}
