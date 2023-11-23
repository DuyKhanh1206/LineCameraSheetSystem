using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using HalconDotNet;

using ViewROI;
using Fujita.Misc;

using LineCameraSheetSystem;

namespace Fujita.InspectionSystem
{
    public class UserSettingEndEventArgs : EventArgs
    {
    }

    public delegate void UserSettingEndEventHandler( object sender, UserSettingEndEventArgs e );

    public class UserSettingManager
    {
        protected IMainFormEqu _MainForm;
        protected Form _Form;
        protected HWndCtrl _WndCtrl;
        protected ROIControllerCallback _RoiCtrl;
        protected Point _ptFormPos;

        public event UserSettingEndEventHandler UserSettingEnd = null;

        public UserSettingManager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos)
        {
            _MainForm = mainfromequ;
            _Form = form;
            _WndCtrl = wnd;
            _RoiCtrl = roictrl;
            _ptFormPos = frmPos;
        }

        protected void raiseEnd()
        {
            if (UserSettingEnd != null)
            {
                UserSettingEnd(this, new UserSettingEndEventArgs());
            }
        }

        /// <summary>
        /// 強制終了
        /// </summary>
        public virtual void ForceCancel()
        {

        }
    }

    /// <summary>
    /// ユーザー設定矩形
    /// </summary>
    public class Rectangle1Manager : UserSettingManager, ICallbackRoiRectangle1
    {
        ICallbackRoiRectangle1 _Callback;
        frmRoiRectangle1 _frmRect1;

        public Rectangle1Manager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos )
           : base( mainfromequ, form, wnd, roictrl, frmPos )
        {
        }

        public bool Start( double row1, double col1, double row2, double col2, string message,  ICallbackRoiRectangle1 callback, object user  )
        {
            if (_RoiCtrl.StartRoi_Rectangle1(row1, col1, row2, col2, this, user))
            {
                _Callback = callback;

                // 設定表示用
                _frmRect1 = new frmRoiRectangle1(row1, col1, row2, col2, message);
                _frmRect1.UserSettingChange += new RoiRectangle1UserSettingEventHandler(_frmRect1_UserSettingChange);
                _frmRect1.Location = _ptFormPos;
                _frmRect1.Show(_Form);

                AppData.GetInstance().status.UserSettingMode = AppData.EUserSettingMode.Rectangle1;
                _MainForm.Update("controls", null);
            }
            else
            {
                return false;
            }
            return true;

        }

        public override void ForceCancel()
        {
            _RoiCtrl.CancelRoi_Rectangle1();
        }

        void _frmRect1_UserSettingChange(object sender, RoiRectangle1UserSettingEventArgs e)
        {
            switch (e.Type)
            {
                case UserSettingChangeType.ValueChange:
                    _RoiCtrl.UpdateRoi_Rectangle1((int)e.Row1, (int)e.Col1, (int)e.Row2, (int)e.Col2, false);
                    break;
                case UserSettingChangeType.OK:
                    _RoiCtrl.DecideRoi_Rectangle1();
                    break;
                case UserSettingChangeType.Cancel:
                    _RoiCtrl.CancelRoi_Rectangle1();
                    break;
            }
        }

        public void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser)
        {
            if (_Callback != null)
            {
                _Callback.Rectangle1_Move((int)row1, (int)col1, (int)row2, (int)col2, oUser);
            }

            _frmRect1.Rectangle1_Move((int)row1, (int)col1, (int)row2, (int)col2, oUser);

        }

        public void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser)
        {
            // ダイアログを閉じる
            if (_frmRect1 != null)
            {
                _frmRect1.Close();
                _frmRect1.Dispose();
                _frmRect1 = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Rectangle1_Decide((int)row1, (int)col1, (int)row2, (int)col2, oUser);
            }

            raiseEnd();
        }

        public void Rectangle1_Cancel(object oUser)
        {
            // ダイアログを閉じる
            if (_frmRect1 != null)
            {
                _frmRect1.Close();
                _frmRect1.Dispose();
                _frmRect1 = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Rectangle1_Cancel(oUser);
            }

            raiseEnd();
        }
    }

    /// <summary>
    /// ユーザー設定角度付き矩形
    /// </summary>
    public class Rectangle2Manager : UserSettingManager, ICallbackRoiRectangle2
    {
        ICallbackRoiRectangle2 _Callback;
        frmRoiRectangle2 _frmRect2;

        public Rectangle2Manager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos )
            :base(mainfromequ, form, wnd, roictrl, frmPos )
        {
        }

        public bool Start(double row, double col, double phi, double len1, double len2, string message, ICallbackRoiRectangle2 callback, object user)
        {
            if (_RoiCtrl.StartRoi_Rectangle2(row, col, phi, len1, len2, this, user))
            {
                _Callback = callback;

                // 設定表示用
                _frmRect2 = new frmRoiRectangle2(row, col, phi, len1, len2, message);
                _frmRect2.UserSettingChanged += new RoiRectangle2UserSettingEventHandler(_frmRect2_UserSettingChanged);
                _frmRect2.Location = _ptFormPos;
                _frmRect2.Show(_Form);

                AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.Rectangle2;
                _MainForm.Update("controls", null);
                return false;
            }

            return true;
        }

        public override void ForceCancel()
        {
            _RoiCtrl.CancelRoi_Rectangle2();
        }

        void _frmRect2_UserSettingChanged(object sender, RoiRectangle2UserSettingEventArgs e)
        {
            switch (e.Type)
            {
                case UserSettingChangeType.ValueChange:
                    _RoiCtrl.UpdateRoi_Rectangle2(e.Row, e.Col, e.Phi, e.Len1, e.Len2, false);
                    break;
                case UserSettingChangeType.OK:
                    _RoiCtrl.DecideRoi_Rectangle2();
                    break;
                case UserSettingChangeType.Cancel:
                    _RoiCtrl.CancelRoi_Rectangle2();
                    break;
            }
        }

        /// <summary>
        /// 矩形が動いた
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="user"></param>
        public void Rectangle2_Move(double row, double col, double phi, double len1, double len2, object user)
        {
            if (_Callback != null)
            {
                _Callback.Rectangle2_Move(row, col, phi, len1, len2, user);
            }
            _frmRect2.Rectangle2_Move(row, col, phi, len1, len2, user);
        }

        /// <summary>
        /// 矩形2が決定された
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="user"></param>
        public void Rectangle2_Decide(double row, double col, double phi, double len1, double len2, object user)
        {
            if (_frmRect2 != null)
            {
                _frmRect2.Close();
                _frmRect2.Dispose();
                _frmRect2 = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Rectangle2_Decide(row, col, phi, len1, len2, user);
            }

            raiseEnd();
        }

        /// <summary>
        /// 矩形2がキャンセルされた
        /// </summary>
        /// <param name="user"></param>
        public void Rectangle2_Cancel(object user)
        {
            // ダイアログを閉じる
            if (_frmRect2 != null)
            {
                _frmRect2.Close();
                _frmRect2.Dispose();
                _frmRect2 = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Rectangle2_Cancel(user);
            }

            raiseEnd();
        }

    }

    /// <summary>
    /// ユーザー設定円
    /// </summary>
    public class CircleManager : UserSettingManager, ICallbackRoiCircle
    {
        ICallbackRoiCircle _Callback;
        frmRoiCircle _frmCircle;

        public CircleManager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos)
            : base(mainfromequ, form, wnd, roictrl, frmPos)
        {
        }

        public bool Start(double row, double col, double rad, string message, ICallbackRoiCircle callback, object user)
        {
            if (_RoiCtrl.StartRoi_Circle(row, col, rad, this, user))
            {
                _Callback = callback;

                // 設定表示用
                _frmCircle = new frmRoiCircle(row, col, rad, message);
                _frmCircle.UserSettingChange += new RoiCircleUserSettingEventHandler(_frmCircle_UserSettingChange);
                _frmCircle.Location = _ptFormPos;
                _frmCircle.Show(_Form);

                AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.Circle;
                _MainForm.Update("controls", null);
            }
            else
            {
                return false;
            }
            return true;

        }

        public override void ForceCancel()
        {
            _RoiCtrl.CancelRoi_Rectangle1();
        }

        void _frmCircle_UserSettingChange(object sender, RoiCircleUserSettingEventArgs e)
        {
            switch (e.Type)
            {
                case UserSettingChangeType.ValueChange:
                    _RoiCtrl.UpdateRoi_Circle((int)e.Row, (int)e.Col, (int)e.Rad, false);
                    break;
                case UserSettingChangeType.OK:
                    _RoiCtrl.DecideRoi_Circle();
                    break;
                case UserSettingChangeType.Cancel:
                    _RoiCtrl.CancelRoi_Circle();
                    break;
            }
        }

        public void Circle_Move(double row, double col, double rad, object oUser)
        {
            if (_Callback != null)
            {
                _Callback.Circle_Move((int)row, (int)col, (int)rad, oUser);
            }

            _frmCircle.Circle_Move((int)row, (int)col, (int)rad, oUser);

        }

        public void Circle_Decide(double row, double col, double rad, object oUser)
        {
            // ダイアログを閉じる
            if (_frmCircle != null)
            {
                _frmCircle.Close();
                _frmCircle.Dispose();
                _frmCircle = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Circle_Decide((int)row, (int)col, (int)rad, oUser);
            }

            raiseEnd();
        }

        public void Circle_Cancel(object oUser)
        {
            // ダイアログを閉じる
            if (_frmCircle != null)
            {
                _frmCircle.Close();
                _frmCircle.Dispose();
                _frmCircle = null;
            }

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
            _MainForm.Update("controls", null);

            if (_Callback != null)
            {
                _Callback.Circle_Cancel(oUser);
            }

            raiseEnd();
        }
    }

    /// <summary>
    /// ユーザー登録複数矩形設定
    /// </summary>
    public class Rectangle1MultiManager : UserSettingManager, ICallbackRoiRectangle1
    {
        object _oUser;
        frmRectangle1Multi _frmRect1Multi;
        List<CRectangle1> _lstRect1;
        ICallbackRectangle1Multi _Callback;

        int _iCurrent = -1;

        public Rectangle1MultiManager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos)
            :base( mainfromequ, form, wnd, roictrl, frmPos )
        {
        }

        public bool Start(List<CRectangle1> lstrect, CRectangle1 rcInit, string mes, ICallbackRectangle1Multi callback, object oUser)
        {
            _oUser = oUser;
            _Callback = callback;

            _lstRect1 = new List<CRectangle1>(lstrect);

            _frmRect1Multi = new frmRectangle1Multi(_lstRect1, rcInit, mes);
            _frmRect1Multi.UserSettingChange += new Rectangle1MultiUserSettingEventHandler(_frmRect1Multi_UserSettingChange);
            _frmRect1Multi.Location = _ptFormPos;
            _frmRect1Multi.Show(_Form);

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.Rectangle1Multi;

            _WndCtrl.RepaintRoiBefore += new RepaintEventHandler(_WndCtrl_RepaintRoiBefore);
            _WndCtrl.MouseDownAction += new MouseDownActionEventHandler(_WndCtrl_MouseDownAction);

            _MainForm.Update("controls", null);
            _WndCtrl.repaint();

            return true;

        }

        public override void ForceCancel()
        {

        }

        private DateTime _dtPrevDown = DateTime.Now;
        PointD _ptPrevDown = new PointD();
        /// <summary>
        /// ダブルクリックされたかどうか判断する
        /// </summary>
        /// <param name="pt">クリックされたマウス位置</param>
        /// <returns>
        /// true : ダブルクリックされた
        /// false : ダブルクリックされていない
        /// </returns>
        private bool checkDoubleClick(PointD pt)
        {
            DateTime dtNowDown = DateTime.Now;
            if ((dtNowDown - _dtPrevDown).TotalMilliseconds > SystemInformation.DoubleClickTime
                || Math.Abs(pt.X - _ptPrevDown.X) > (double)SystemInformation.DoubleClickSize.Width
                || Math.Abs(pt.Y - _ptPrevDown.Y) > (double)SystemInformation.DoubleClickSize.Height)
            {
                _dtPrevDown = dtNowDown;
                _ptPrevDown.X = pt.X;
                _ptPrevDown.Y = pt.Y;
                return false;
            }
            _dtPrevDown = dtNowDown.Subtract(TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime));
            return true;
        }

        /// <summary>
        /// マウスが押された時のアクション
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _WndCtrl_MouseDownAction(object sender, MouseDownActionEventArgs e)
        {
            int iNewCurrent = -1;
            List<int> lstCurrentTarget = new List<int>();

            if( !checkDoubleClick( new PointD( e.X, e.Y )))
            {
                return;
            }

            PointD pt = new PointD( e.X, e.Y );
            try
            {
                for (int i = 0; i < _lstRect1.Count; i++)
                {
                    if (_lstRect1[i].PtInRect(pt))
                    {
                        lstCurrentTarget.Add(i);
                    }
                }

                // 対応する選択肢がない場合
                if (lstCurrentTarget.Count == 0)
                    return;

                // 一つの場合、それに変更
                if (lstCurrentTarget.Count == 1)
                {
                    iNewCurrent = lstCurrentTarget[0];
                }
                else
                {
                    //２つ以上の場合、カレントの次を選択、カレントがない場合は最初のやつから
                    int iCur = lstCurrentTarget.Find( x => x == _iCurrent );
                    if ( iCur != -1)
                    {
                        if (iCur < lstCurrentTarget.Count - 1)
                            iNewCurrent = lstCurrentTarget[iCur + 1];
                        else
                            iNewCurrent = lstCurrentTarget[0];
                    }
                    else
                    {
                        iNewCurrent = lstCurrentTarget[0];
                    }
                }
            }
            catch (HOperatorException)
            {
                return;
            }

            // カレントを変更する
            if (_iCurrent != iNewCurrent)
            {
                _frmRect1Multi.CurrentChange(iNewCurrent);
            }
        }

        void _WndCtrl_RepaintRoiBefore(object sender, RepaintEventArgs e)
        {
            try
            {
                HOperatorSet.SetColor(e.HWindowID, "orange");
                HOperatorSet.SetDraw(e.HWindowID, "margin");
                for (int i = 0; i < _lstRect1.Count; i++)
                {
                    if (i == _iCurrent)
                        continue;
                    HOperatorSet.DispRectangle1(e.HWindowID,
                        _lstRect1[i].Row1, _lstRect1[i].Col1, _lstRect1[i].Row2, _lstRect1[i].Col2);

                }
            }
            catch (HOperatorException)
            {
            }
        }

        void _frmRect1Multi_UserSettingChange(object sender, Rectangle1MultiUserSettingEventArgs e)
        {
            switch (e.Type)
            {
                case UserSettingChangeType.ValueChange:
                    {
                        _RoiCtrl.UpdateRoi_Rectangle1(_lstRect1[e.Current].Row1, _lstRect1[e.Current].Col1, _lstRect1[e.Current].Row2, _lstRect1[e.Current].Col2, false);
                    }
                    break;

                case UserSettingChangeType.OK:
                case UserSettingChangeType.Cancel:
                    {
                        _RoiCtrl.CancelRoi_Rectangle1();
                        _frmRect1Multi.UserSettingChange -= _frmRect1Multi_UserSettingChange;
                        _frmRect1Multi.Close();
                        _frmRect1Multi.Dispose();
                        _frmRect1Multi = null;

                        _WndCtrl.RepaintRoiBefore -= _WndCtrl_RepaintRoiBefore;
                        _WndCtrl.MouseDownAction -= _WndCtrl_MouseDownAction;

                        AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
                        _MainForm.Update("controls", null);
                        _WndCtrl.repaint();

                        raiseEnd();

                        if (e.Type == UserSettingChangeType.OK)
                        {
                            _Callback.Rectangle1Multi_Decide(_lstRect1, _oUser);
                        }
                        else
                        {
                            _Callback.Rectangle1Multi_Cancel(_oUser);
                        }

                        _lstRect1.Clear();
                        _lstRect1 = null;
                        _MainForm = null;
                        _WndCtrl = null;
                        _RoiCtrl = null;
                        _oUser = null;
                    }
                    break;

                case UserSettingChangeType.MultiSettingChange:
                    switch (e.MultiSetting)
                    {
                        case MultiSettingChange.Current_Change:
                            {
                                _RoiCtrl.DecideRoi_Rectangle1();
                                _RoiCtrl.StartRoi_Rectangle1(_lstRect1[e.Current].Row1, _lstRect1[e.Current].Col1, _lstRect1[e.Current].Row2, _lstRect1[e.Current].Col2, this, _oUser);
                                _iCurrent = e.Current;
                                _WndCtrl.repaint();
                            }
                            break;
                        case MultiSettingChange.Add_Last:
                        case MultiSettingChange.Add_Next:
                            _WndCtrl.repaint();
                            break;
                        case MultiSettingChange.Delete:
                            {
                                if( e.Current == -1 )
                                    _RoiCtrl.DecideRoi_Rectangle1();
                                _iCurrent = e.Current;
                                _WndCtrl.repaint();
                            }
                            break;
                    }
                    break;
            }
        }

        public void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser)
        {
            _frmRect1Multi.Rectangle1_Move(row1, col1, row2, col2, oUser);
        }

        public void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser)
        {
        }

        public void Rectangle1_Cancel(object oUser)
        {
        }
    }

    public class ThresholdManager : UserSettingManager
    {
        frmThreshold _frmThreshold = null;
        ICallbackThreshold _callbackThreshold = null;
        object _oUser = null;

        public ThresholdManager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos)
            :base( mainfromequ, form, wnd, roictrl, frmPos )
        {
        }

        public bool Start(int iLow, int iHigh, double row1, double col1, double row2, double col2, string message, ICallbackThreshold callback, object user)
        {
            if (_callbackThreshold != null)
                return false;

            _callbackThreshold = callback;
            _oUser = user;

            frmThreshold.EThresholdType type = frmThreshold.EThresholdType.Both;
            if (iLow < 0)
                type = frmThreshold.EThresholdType.High;
            else if (iHigh < 0)
                type = frmThreshold.EThresholdType.Low;

            _frmThreshold = new frmThreshold(iLow, iHigh, row1, col1, row2, col2, message, _MainForm, type );
            _frmThreshold.UserSettingChange += new ThresholdUserSettingEventHandler(_frmThreshold_UserSettingChange);
            _frmThreshold.Location = _ptFormPos;
            _frmThreshold.Show(_Form);

            AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.Threshold;
            _MainForm.Update( "controls", null );

            _WndCtrl.DispBinPart = true;
            _WndCtrl.LowThresholdPart = iLow;
            _WndCtrl.HighThresholdPart = iHigh;
            _WndCtrl.DispBinPartRow1 = row1;
            _WndCtrl.DispBinPartCol1 = col1;
            _WndCtrl.DispBinPartRow2 = row2;
            _WndCtrl.DispBinPartCol2 = col2;

            _WndCtrl.repaint();

            return true;
        }

        public override void ForceCancel()
        {

        }

        void _frmThreshold_UserSettingChange(object sender, ThresholdUserSettingEventArgs e)
        {
            int iLow = e.Low < 0 ? 0 : e.Low;
            int iHigh = e.High < 0 ? 0 : e.High;

            switch (e.Type)
            {
                case UserSettingChangeType.ValueChange:
                    _WndCtrl.LowThresholdPart = iLow;
                    _WndCtrl.HighThresholdPart = iHigh;
                    _WndCtrl.repaint();
                    if (_callbackThreshold != null)
                    {
                        _callbackThreshold.Threshold_ValueChange(iLow, iHigh, _oUser);
                    }
                    break;

                case UserSettingChangeType.OK:
                case UserSettingChangeType.Cancel:
                    _frmThreshold.UserSettingChange -= _frmThreshold_UserSettingChange;
                    _frmThreshold.Close();
                    _frmThreshold.Dispose();

                    _frmThreshold = null;
                    _WndCtrl.DispBinPart = false;
                    _WndCtrl.repaint();

                    AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
                    _MainForm.Update("controls", null);
                    raiseEnd();

                    if (_callbackThreshold != null)
                    {
                        ICallbackThreshold callbackTemp = _callbackThreshold;
                        _callbackThreshold = null;
                        if (e.Type == UserSettingChangeType.OK)
                        {
                            callbackTemp.Threshold_Decide(iLow, iHigh, _oUser);
                        }
                        else
                        {
                            callbackTemp.Threshold_Cancel(_oUser);
                        }
                    }

                    break;
            }
        }
    }

    public class PointMultiManager : UserSettingManager, ICallbackRoiPoint
    {
        frmPointMulti _frmPointMulti = null;
        ICallbackPointMulti _callbackPointMulti = null;
        List<double> _daPointMultiRows = null;
        List<double> _daPointMultiCols = null;
        int _iPointMultiCurrent = -1;

        object _oUser;

        public PointMultiManager(IMainFormEqu mainfromequ, Form form, HWndCtrl wnd, ROIControllerCallback roictrl, Point frmPos)
            :base( mainfromequ, form, wnd, roictrl, frmPos )
        {
        }

        public bool Start(List<double> rows, List<double> cols, string sMessage, ICallbackPointMulti callback, object user)
        {
            _callbackPointMulti = callback;
            _oUser = user;

            _daPointMultiRows = new List<double>(rows);
            _daPointMultiCols = new List<double>(cols);

            try
            {
            }
            catch (HOperatorException)
            {
            }

            //_frmPointMulti = new frmPointMulti(_daPointMultiRows, _daPointMultiCols, 
            //    _daPointMultiRows.Count > 0 ? _daPointMultiRows[0] : _WndCtrl.ImageWidth / 2.0,
            //    _daPointMultiRows.Count > 0 ? _daPointMultiCols[0] : _WndCtrl.ImageHeight / 2.0, sMessage);
            //_frmPointMulti.UserSettingChange += new PointMultiUserSettingEventHandler(_frmPointMulti_UserSettingChange);
            //_frmPointMulti.Location = _ptFormPos;
            //_frmPointMulti.Show(_Form);

            //AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.PointMulti;
            //_MainForm.Update("controls", null);

            _WndCtrl.RepaintRoiBefore += new RepaintEventHandler(_WndCtrl_RepaintRoiBefore);
            _WndCtrl.MouseDownAction += new MouseDownActionEventHandler(_WndCtrl_MouseDownAction);

            if (rows.Count > 0)
            {
                _RoiCtrl.StartRoi_Point(rows[0], cols[0], this, user);
            }

            _WndCtrl.repaint();
            return true;
        }

        DateTime _dtPrevDown = DateTime.Now;
        void _WndCtrl_MouseDownAction(object sender, MouseDownActionEventArgs e)
        {
            int iNewCurrent = -1;
            double dMax = 10000;
            double dEpcilon = 35;

            DateTime dtNowDown = DateTime.Now;
            // 一番近い場所を探す
            // 現在のカレントが範囲内の場合無効
            if ((dtNowDown - _dtPrevDown).TotalMilliseconds > SystemInformation.DoubleClickTime)
            {
                _dtPrevDown = dtNowDown;
                return;
            }

            _dtPrevDown = dtNowDown;
            try
            {
                if (_iPointMultiCurrent != -1)
                {
                    HTuple htCurDistance;
                    HOperatorSet.DistancePp(e.Y, e.X, _daPointMultiRows[_iPointMultiCurrent], _daPointMultiCols[_iPointMultiCurrent], out htCurDistance);
                    if (htCurDistance.D < dEpcilon)
                        return;
                }


                for (int i = 0; i < _daPointMultiRows.Count; i++)
                {
                    HTuple htDistance;
                    HOperatorSet.DistancePp(e.Y, e.X, _daPointMultiRows[i], _daPointMultiCols[i], out htDistance);
                    if (htDistance.D < dMax && htDistance.D < dEpcilon)
                    {
                        dMax = htDistance.D;
                        iNewCurrent = i;
                    }
                }
            }
            catch (HOperatorException)
            {
                return;
            }
            if (_iPointMultiCurrent != iNewCurrent)
            {
                _frmPointMulti.CurrentChange(iNewCurrent);
            }
        }

        void _WndCtrl_RepaintRoiBefore(object sender, RepaintEventArgs e)
        {
            try
            {
                HOperatorSet.SetColor(e.HWindowID, "orange");
                // カレント以外のポイントを描画する
                for (int i = 0; i < _daPointMultiRows.Count; i++)
                {
                    HOperatorSet.DispCross(e.HWindowID, _daPointMultiRows[i], _daPointMultiCols[i], 20, 0);
                    HOperatorSet.DispRectangle2(e.HWindowID, _daPointMultiRows[i], _daPointMultiCols[i], 0, 5, 5);
                    if (i < _daPointMultiRows.Count - 1)
                    {
                        HOperatorSet.DispLine(e.HWindowID, _daPointMultiRows[i], _daPointMultiCols[i], _daPointMultiRows[i + 1], _daPointMultiCols[i + 1]);
                    }
                }
            }
            catch (HOperatorException)
            {
            }
        }

        //void _frmPointMulti_UserSettingChange(object sender, PointMultiUserSettingEventArgs e)
        //{
        //    switch (e.Type)
        //    {
        //        case UserSettingChangeType.ValueChange:
        //            {
        //                _RoiCtrl.UpdateRoi_Point(e.Rows[e.Current], e.Cols[e.Current], false);
                        
        //            }
        //            break;
        //        case UserSettingChangeType.OK:
        //        case UserSettingChangeType.Cancel:
        //            {
        //                _RoiCtrl.CancelRoi_Point();

        //                _frmPointMulti.UserSettingChange -= _frmPointMulti_UserSettingChange;

        //                _frmPointMulti.Close();
        //                _frmPointMulti.Dispose();
        //                _frmPointMulti = null;

        //                // 描画設定非表示
        //                _WndCtrl.RepaintRoiBefore -= _WndCtrl_RepaintRoiBefore;
        //                _WndCtrl.MouseDownAction -= _WndCtrl_MouseDownAction;

        //                AppData.getInstance().status.UserSettingMode = AppData.EUserSettingMode.NotSetting;
        //                _MainForm.Update("controls", null);
        //                raiseEnd();

        //                if (e.Type == UserSettingChangeType.OK)
        //                    _callbackPointMulti.PointMulti_Decide(e.Rows, e.Cols, _oUser);
        //                else
        //                    _callbackPointMulti.PointMulti_Cancel(_oUser);

        //                _WndCtrl.repaint();
        //            }
        //            break;
        //        case UserSettingChangeType.MultiSettingChange:
        //            switch (e.MultiSetting)
        //            {
        //                case MultiSettingChange.Current_Change:
        //                    {
        //                        _RoiCtrl.DecideRoi_Point();
        //                        _RoiCtrl.StartRoi_Point(e.Rows[e.Current], e.Cols[e.Current], this, _oUser);
        //                        _iPointMultiCurrent = e.Current;
        //                        _WndCtrl.repaint();
        //                    }
        //                    break;
        //                case MultiSettingChange.Add_Last:
        //                case MultiSettingChange.Add_Next:
        //                case MultiSettingChange.Delete:
        //                    {
        //                        //                                _roiCallback.DecideRoi_Point();
        //                        _WndCtrl.repaint();
        //                    }
        //                    break;
        //            }
        //            break;
        //    }
        //}

        public void Point_Move(double row, double col, object oUser)
        {
            if (_frmPointMulti != null)
            {
                _frmPointMulti.Point_Move(row, col, oUser);
                _WndCtrl.repaint();
            }
        }

        public void Point_Decide(double row, double col, object oUser)
        {
            //nop
        }

        public void Point_Cancel(object oUser)
        {
            //nop
        }
    }
}
