using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ViewROI;
using Fujita.Misc;
using HalconDotNet;

namespace Fujita.InspectionSystem
{
    /// <summary>
    /// メインフォームの操作
    /// </summary>
    public interface IMainFormEqu : IUpdate
    {
        // メインウインドウに対して行う処理
        /// <summary>
        /// Rectangle1の矩形設定を行う
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool StartRoiRectangle1(double row1, double col1, double row2, double col2, string message, ICallbackRoiRectangle1 callback, object user);
        /// <summary>
        /// Rectangle2の矩形設定を行う
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool StartRoiRectangle2(double row, double col, double phi, double len1, double len2, string message, ICallbackRoiRectangle2 callback, object user);
        /// <summary>
        /// Circleの円設定を行う
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rad"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool StartRoiCircle(double row, double col, double rad, string message, ICallbackRoiCircle callback, object user);

        /// <summary>
        /// しきい値の設定を行う
        /// </summary>
        /// <param name="iLow">しきい値下限</param>
        /// <param name="iHigh">しきい値上限</param>
        /// <param name="row1">矩形Row1</param>
        /// <param name="col1">矩形Col1</param>
        /// <param name="row2">矩形Row2</param>
        /// <param name="col2">矩形Col2</param>
        /// <param name="message">表示メッセージ</param>
        /// <param name="callback">コールバックメソッド</param>
        /// <param name="user">ユーザー定義</param>
        /// <returns></returns>
        bool StartThreshold(int iLow, int iHigh, double row1, double col1, double row2, double col2, string message, ICallbackThreshold callback, object user);

        /// <summary>
        /// 複数点の設定
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="sMessage"></param>
        /// <param name="callback"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool StartRoiPointMulti(List<double> rows, List<double> cols, string sMessage, ICallbackPointMulti callback, object user);
        /// <summary>
        /// 複数矩形の設定
        /// </summary>
        /// <param name="lstRect"></param>
        /// <param name="rcInit"></param>
        /// <param name="sMessage"></param>
        /// <param name="callback"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool StartRoiRectangle1Multi(List<CRectangle1> lstRect, CRectangle1 rcInit, string sMessage, ICallbackRectangle1Multi callback, object user);

        /// <summary>
        /// メインウインドウに矩形表示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="color"></param>
        /// <param name="drawmode"></param>
        /// <param name="linewidth"></param>
        /// <returns></returns>
        bool AddRectangle1(string name, double row1, double col1, double row2, double col2, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1);
        /// <summary>
        /// メインウインドウに角度付き矩形表示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="color"></param>
        /// <param name="drawmode"></param>
        /// <param name="linewidth"></param>
        /// <returns></returns>
        bool AddRectangle2(string name, double row, double col, double phi, double len1, double len2, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1);
        /// <summary>
        /// メインウインドウに円表示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rad"></param>>
        /// <param name="color"></param>
        /// <param name="drawmode"></param>
        /// <param name="linewidth"></param>
        /// <returns></returns>
        bool AddCircle(string name, double row, double col, double rad, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1);
        /// <summary>
        /// メインウインドウにテキスト表示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="fontsize"></param>
        /// <param name="window"></param>
        /// <param name="box"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        bool AddText(string name, string message, double row, double col, int fontsize, bool window, bool box, string color);
        /// <summary>
        /// 描画オブジェクトを削除する
        /// </summary>
        /// <param name="name"></param>
        void DeleteObject(string name);
        /// <summary>
        /// すべての描画オブジェクトを削除する
        /// </summary>
        void DeleteAllObjects();

        /// <summary>
        /// 表示イメージを一時バッファ
        /// </summary>
        /// <param name="img"></param>
        void StoreImage(HObject img);

        /// <summary>
        /// 表示イメージ
        /// </summary>
        void RestoreImage();

        /// <summary>
        /// コントロール間で動作を行う
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arrayParams"></param>
        void ControlSender(string type, object[] arrayParams);

        //設定タブに対して行う処理
        void DisableControls();
        void EnableControls();
        void UpdateResults();
        void UpdateDatas(bool bUp);

        //カレントイメージの取得
        HalconDotNet.HObject GetCurrentImage();
        HWndCtrl GetMainWnd();

        // 動作を許すかどうか
        bool IsCanPopupWindow();

        bool IsRegistImageDisplay();
    }

    public interface IUpdate
    {
        /// <summary>
        /// ウインドウごとの特殊なアップデート処理を行う
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramArray"></param>
        void Update(string type, object[] paramArray);
    }
}
