using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InspectionNameSpace;

namespace LineCameraSheetSystem
{
    public class CameraParam                        // thông số máy ảnh
    {
        /// <summary>
        //　ID
        /// </summary>
        public AppData.CamID CamID { get; set; }
        /// <summary>
        /// CameraManagerでの使用するカメラ№           Số camera được sử dụng trong CameraManager
        /// </summary>
        public int CamNo { get; set; }
        /// <summary>
        //有効/無効                                     bật/ tắt
        /// </summary>
        public bool OnOff { get; set; }
        /// <summary>
        // カメラ部位
        /// </summary>
        public  AppData.SideID CamParts { get;  set; }
        /// <summary>
        //画像縦ピクセル数                               Số pixel dọc của hình ảnh
        /// </summary>
        public int PixV { get; set; }
        /// <summary>
        //画像横ピクセル数                               Số pixel ngang của hình ảnh
        /// </summary>
        public int PixH { get; set; }
        /// <summary>
        //分解能-縦                                      Độ phân giải dọc
        /// </summary>
        public double ResoV { get; set; }
        /// <summary>
        //分解能-横                                     Độ phân giải ngang
        /// </summary>
        public double ResoH { get; set; }
        /// <summary>
        //補正量-縦                                     Số lượng chỉnh sửa - Dọc
        /// </summary>
        public double ShiftV { get; set; }
        /// <summary>
        //補正量-横                                     Số lượng chỉnh sửa - ngang
        /// </summary>
        public double ShiftH { get; set; }

        /// <summary>
        /// キャプチャ開始時に破棄する画像枚数       Số lượng ảnh cần loại bỏ khi bắt đầu chụp
        /// </summary>
        public int DiscardCount { get; set; }

        /// <summary>
        /// 使用する照明番号(0～)                    Số đèn sử dụng (0~)
        /// </summary>
        public List<int> UseLightNo;
    }
}
