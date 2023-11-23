#define GRABSYNC //デバックしたいときはコメントアウトする
#define SPEED_MONITOR_NEW   // スピードモニタ

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using HalconCamera;
using InspectionNameSpace;
using ResultActionDataClassNameSpace;
using SheetMapping;
using HalconDotNet;
using Fujita.InspectionSystem;
using Adjustment;
using Fujita.Communication;
using Fujita.LightControl;
using LogingDllWrap;
using Fujita.Misc;
using KaTool.Rs232c;
using System.IO.Ports;

namespace LineCameraSheetSystem
{
    public partial class MainForm : Form, ISheetTipItemContainer
    {
        public MainForm()
        {
            InitializeComponent();

            btnSuspend.Visible = SystemParam.GetInstance().SuspendButtonEnable;             //btnSuspend.Visible là nút button có hiển thị hay không. khác enabled là nó ẩn hẳn và k nhìn thấy
            label5.Visible = SystemParam.GetInstance().LotNoEnable;
            textLotNo.Visible = SystemParam.GetInstance().LotNoEnable;

            //V1319 開始時刻初期化
            textStartTime.Text = "";
        }


        string _sTittle = AppData.DEFAULT_APP_NAME; // lấy ra tên của app name
        private void initializeAll()
        {
            SplashForm.ProgressSplash(65, "画像の初期処理中です");            //Đang xử lý hình ảnh ban đầu

            // Halcon初期化                Khởi tạo Halcon
            initHalcon();

            btnImageMain.Visible = SystemParam.GetInstance().IM_Enabled;

            Color btnCol = SystemColors.Control;        // cài đặt màu background cho các button 
            btnStart.BackColor = btnCol;
            btnSuspend.BackColor = btnCol;
            btnEnd.BackColor = btnCol;
            btnReset.BackColor = btnCol;
            btnImageMain.BackColor = btnCol;
            btnRecipe.BackColor = btnCol;
            btnMap.BackColor = btnCol;
            btnNgMiniImg.BackColor = btnCol;
            btnTotal.BackColor = btnCol;
            btnOldList.BackColor = btnCol;
            btnNgList.BackColor = btnCol;
            btnNgReset.BackColor = btnCol;  //v1341
            btnSystem.BackColor = btnCol;

            //シグナルタワー黄色点灯           Tháp tín hiệu chiếu sáng màu vàng
            clsSignalControl.GetInstance().SetInspectStatus(ESignalStatus.InspectStop);
            //オフラインモード
            //APcamOffLineMode(SystemParam.GetInstance().OffLineMode);

            SplashForm.ProgressSplash(66, "UPSモニタの初期処理中です"); //Quá trình xử lý ban đầu của màn hình UPS đang được tiến hành.

            // UPSモニタの起動            vKhởi động màn hình UPS
            this.InitUpsMoni();

            SplashForm.ProgressSplash(67, "マイコン監視の初期処理中です");        //Quá trình xử lý ban đầu của giám sát máy vi tính đang được tiến hành.

#if !SPEED_MONITOR_NEW
            //検査開始忘れ機能
            this.InitSpeedMonitor();
#else
            //速度監視(マイコン)        Giám sát tốc độ (máy vi tính)
            this.InitSpeedMonitorNew();
#endif

            SplashForm.ProgressSplash(68, "検査処理の初期処理中です");          //Quá trình xử lý ban đầu của quá trình kiểm tra đang được tiến hành.

            //自動検査      kiểm tra tự động
            this.InitInspection();

            SplashForm.ProgressSplash(69, "カメラ露光値の初期処理中です");            //Quá trình xử lý ban đầu giá trị phơi sáng của máy ảnh đang được tiến hành.

            //基準露光値ロード          Tải giá trị phơi sáng tiêu chuẩn
            this.LoadExposureDefalt();

            SplashForm.ProgressSplash(70, "ライブスタートの初期処理中です");           //Quá trình xử lý ban đầu để bắt đầu trực tiếp đang được tiến hành.

            //APCameraManegerライブスタート        APCameraQuản lý bắt đầu trực tiếp
            APCameraManager.getInstance().OnGetImageThread += MainForm_OnGetImageThread;
            APcamLiveStart();

            SplashForm.ProgressSplash(71, "エラーモニタの初期処理中です");            //Quá trình giám sát lỗi ban đầu đang được tiến hành.

            // エラーモニタの起動        Khởi động trình giám sát lỗi
            if (this.initErrorMonitor())
            {
            }

            SplashForm.ProgressSplash(72, "LED点灯時間の初期処理中です");           //Quá trình xử lý ban đầu về thời gian chiếu sáng LED đang được tiến hành.

            //LED点灯時間のイベント登録            //Đăng ký sự kiện thời gian chiếu sáng đèn LED
            this.InitLightMeasPeriodMes();

            //
            this.ChangeTimeAll();

            //リセットボタンのDIO
            //V1057 手動外部修正 yuasa 20190122：手動検査開始終了にini追加
            //v1326 岐阜カスタムでもDioCommandMonitor初期化
            //v1338 PC電源ボタン押下対応でも初期化（InitDioCommandMonitor）が動くように追記
            if (SystemParam.GetInstance().OutsideResetButtonEnable == true || SystemParam.GetInstance().OutsideManualExtButtonEnable == true
                || SystemParam.GetInstance().GCustomEnable == true || SystemParam.GetInstance().PowerOffButtonEnable == true)
            {
                this.InitDioCommandMonitor();
            }
            if (SystemParam.GetInstance().OutsideManualExtButtonEnable == false) //V1057 手動外部修正 yuasa 20190115：ini無効時、ボタン無効化        // Sửa đổi bên ngoài thủ công V1057 yuasa 20190115: nút bị tắt khi ini bị tắt
            {
                chkExtDinInsp.Visible = false;
            }

            SplashForm.ProgressSplash(73, "測長監視の初期処理中です");          //Quá trình xử lý ban đầu của giám sát đo chiều dài đang được tiến hành.

            // 検査測長監視           Giám sát đo chiều dài kiểm tra
            this.initLengthMeasMonitor();

            //メインフォームのテキストの変更           Thay đổi văn bản biểu mẫu chính
            // C#
            Assembly mainAssembly = Assembly.GetEntryAssembly();
            AssemblyName mainAssemName = mainAssembly.GetName();
            _sTittle += " " + mainAssemName.Version.ToString();

            this.Text = _sTittle;

            SplashForm.ProgressSplash(74, "イベント登録の初期処理中です");            //Quá trình xử lý ban đầu của việc đăng ký sự kiện đang được tiến hành.


            //メイン画面の左側のウインドウの位置             Vị trí cửa sổ bên trái màn hình chính
            Point LeftPoint = new Point(AppData.LEFT_X, AppData.LEFT_Y);
            //メイン画面の右側のウインドウの位置             Vị trí cửa sổ bên phải màn hình chính
            Point RightPoint = new Point(AppData.RIGHT_X, AppData.RIGHT_Y);

            //各UserControlの表示位置         Vị trí hiển thị của từng UserControl    
            UclNgListReal.chkboxScrolRock.CheckedChanged += new EventHandler(UclNgListReal_chkboxScrolRock_CheckedChanged);
            UclNgListReal.listViewNGItem.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(UclNgListReal_listViewNGItem_RetrieveVirtualItem);
            UclNgListReal.OnDoubleClickList += new uclNgList.DoubleClickEventHandler(UclNgListReal_OnDoubleClickList);
            UclNgListReal.Location = LeftPoint;
            UclNgListReal.Width = AppData.LEFT_WIDTH;
            UclNgListReal.Height = AppData.HEIGHT;

            UclNgListOld.chkboxScrolRock.CheckedChanged += new EventHandler(UclNgListOld_chkboxScrolRock_CheckedChanged);
            UclNgListOld.listViewNGItem.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(UclNgListOld_listViewNGItem_RetrieveVirtualItem);
            UclNgListOld.OnDoubleClickList += new uclNgList.DoubleClickEventHandler(UclNgListOld_OnDoubleClickList);
            UclNgListOld.Location = LeftPoint;
            UclNgListOld.Width = AppData.LEFT_WIDTH;
            UclNgListOld.Height = AppData.HEIGHT;

            UclRecipeList.Location = LeftPoint;
            UclRecipeList.Width = AppData.LEFT_WIDTH;
            UclRecipeList.Height = AppData.HEIGHT;

            UclRecipeContentsReal.Location = RightPoint;
            UclRecipeContentsReal.Width = AppData.RIGHT_WIDTH;
            UclRecipeContentsReal.Height = AppData.HEIGHT;

            UclRecipeContentsOld.Location = RightPoint;
            UclRecipeContentsOld.Width = AppData.RIGHT_WIDTH;
            UclRecipeContentsOld.Height = AppData.HEIGHT;

            UclNgThumbnailReal.Location = RightPoint;
            UclNgThumbnailReal.OnButtonClick += new uclNgThumbnail.ButtonClickEventHandler(UclNgThumbnailReal_OnButtonClick);
            UclNgThumbnailReal.OnRefreshThumbnail += new uclNgThumbnail.RefreshThumbnailEventHandler(UclNgThumbnailReal_OnRefreshThumbnail);
            UclNgThumbnailReal.Width = AppData.RIGHT_WIDTH;
            UclNgThumbnailReal.Height = AppData.HEIGHT;

            UclNgThumbnailOld.Location = RightPoint;
            UclNgThumbnailOld.OnButtonClick += new uclNgThumbnail.ButtonClickEventHandler(UclNgThumbnailOld_OnButtonClick);
            UclNgThumbnailOld.OnRefreshThumbnail += new uclNgThumbnail.RefreshThumbnailEventHandler(UclNgThumbnailOld_OnRefreshThumbnail);
            UclNgThumbnailOld.Width = AppData.RIGHT_WIDTH;
            UclNgThumbnailOld.Height = AppData.HEIGHT;

            UclSystem.Location = RightPoint;
            UclSystem.Width = AppData.RIGHT_WIDTH;
            UclSystem.Height = AppData.HEIGHT;

            UclTotalReal.Location = RightPoint;
            UclTotalReal.Width = AppData.RIGHT_WIDTH;
            UclTotalReal.Height = AppData.HEIGHT;

            UclTotalOld.Location = RightPoint;
            UclTotalOld.Width = AppData.RIGHT_WIDTH;
            UclTotalOld.Height = AppData.HEIGHT;

            UclOldList.Location = LeftPoint;
            UclOldList.Width = AppData.LEFT_WIDTH + AppData.RIGHT_WIDTH + (AppData.RIGHT_X - AppData.LEFT_X - AppData.LEFT_WIDTH) + 5;
            UclOldList.Height = AppData.HEIGHT;

            UclSheetMapReal.Location = RightPoint;
            UclSheetMapReal.Width = AppData.RIGHT_WIDTH;
            UclSheetMapReal.Height = AppData.HEIGHT;
            UclSheetMapReal.SokucyouColumnWidth = 70;
            UclSheetMapReal.SheetTipItemContainer = this;
            UclSheetMapReal.TipSize = new System.Drawing.Size(7, 7);

            UclSheetMapOld.Location = RightPoint;
            UclSheetMapOld.Width = AppData.RIGHT_WIDTH;
            UclSheetMapOld.Height = AppData.HEIGHT;
            UclSheetMapOld.SokucyouColumnWidth = 70;
            UclSheetMapOld.SheetTipItemContainer = this;
            UclSheetMapOld.TipSize = new System.Drawing.Size(7, 7);

            UclImageMain.Location = new Point(AppData.LEFT_X, AppData.LEFT_Y);
            UclImageMain.Width = (AppData.RIGHT_X - AppData.LEFT_X) + AppData.RIGHT_WIDTH + 10;
            UclImageMain.Height = AppData.HEIGHT;

            _lsttsslSpeeds.Add(toolSpeed1);
            _lsttsslSpeeds.Add(toolSpeed2);

            //過去レシピは数値を変えられない
            UclRecipeContentsOld.Enabled = false;
            //過去用の品種名、LotNOの表示
            UclRecipeContentsOld.DispOldNameLot();
            //過去NGリストはスクロールロックボタンを隠す
            UclNgListOld.ScrollLockVisible();

            //現在の品種表示中                        Hiển thị loại sản phẩm hiện tại
            SystemStatus.GetInstance().DataDispMode = SystemStatus.ModeID.Real;

            //NGリストを表示中                       Hiển thị danh sách NG
            SystemStatus.GetInstance().DispNgList = true;
            //マップを表示中                         Hiển thị bản đồ
            SystemStatus.GetInstance().DispMap = true;

            //lotNoに"”を入れる
            this.LotNo = "";
            //スピードに０．０を入れる
            double dlength = 0.00;
            this.textSpeed.Text = dlength.ToString(SystemParam.GetInstance().SpeedMainDecimal);
            this.textLength.Text = dlength.ToString(SystemParam.GetInstance().LengthDecimal);

            //サムネイルのページリセット             Đặt lại trang hình thu nhỏ
            ChangePage(UclNgThumbnailReal, null, 0, -1);

            //初めのフォーカス                      trọng tâm ban đầu
            btnNgList.Select();

            // ChangeStartEndButtonEnable(true);
            btnSuspend.Enabled = false;
            btnEnd.Enabled = false;
            btnStart.Enabled = false;
            btnReset.Enabled = false;

            //現在の状態を停止中にする              Đặt trạng thái hiện tại thành dừng
            ChangeState(SystemStatus.State.Stop);

            //タイマー用のイベントハンドラの登録         Đăng ký trình xử lý sự kiện cho bộ hẹn giờ
            timer.Interval = AppData.TIMER_INTERVAL;
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();

            //欠点表示の色の登録                     Đăng ký màu hiển thị lỗi
            SetTipColors();

            //レシピのインスタンス            ///ví dụ về công thức
            Recipe.GetInstance();

            //Realのサムネイルボタンenable           Kích hoạt nút hình thu nhỏ thực
            UclNgThumbnailReal.ButtonEnable(false);

            _lstChildWnd.Add(UclSheetMapReal);
            _lstChildWnd.Add(UclSheetMapOld);
            _lstChildWnd.Add(UclNgListReal);
            _lstChildWnd.Add(UclNgListOld);
            _lstChildWnd.Add(UclOldList);
            _lstChildWnd.Add(UclNgThumbnailReal);
            _lstChildWnd.Add(UclNgThumbnailOld);
            _lstChildWnd.Add(UclRecipeList);
            _lstChildWnd.Add(UclRecipeContentsReal);
            _lstChildWnd.Add(UclRecipeContentsOld);
            _lstChildWnd.Add(UclSystem);

            _clsShortcutman.Initialize();
            //ショートカットキーを登録するユーザーコントロールの追加                                   //Thêm điều khiển người dùng để đăng ký phím tắt
            _clsShortcutman.Add(UclSheetMapReal);
            _clsShortcutman.Add(UclSheetMapOld);
            _clsShortcutman.Add(UclNgListReal);
            _clsShortcutman.Add(UclNgListOld);
            _clsShortcutman.Add(UclOldList);
            _clsShortcutman.Add(UclNgThumbnailReal);
            _clsShortcutman.Add(UclNgThumbnailOld);
            _clsShortcutman.Add(UclRecipeList);
            _clsShortcutman.Add(UclRecipeContentsReal);
            _clsShortcutman.Add(UclRecipeContentsOld);
            _clsShortcutman.Add(UclSystem);
            //文字の入力のあるコントロールを登録                                                 Đăng ký điều khiển bằng cách nhập văn bản
            _clsShortcutman.AddIgnoreComponet(textLotNo);
            _clsShortcutman.AddIgnoreComponet(UclRecipeList.textEditBox);

            UclNgListOld.ShortcutkeyClearOld();

            UclNgThumbnailReal.uclMiniImage1.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);
            UclNgThumbnailReal.uclMiniImage2.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);
            UclNgThumbnailReal.uclMiniImage3.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);
            UclNgThumbnailReal.uclMiniImage4.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);
            UclNgThumbnailReal.uclMiniImage5.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);
            UclNgThumbnailReal.uclMiniImage6.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailReal_OnThumbnailDoubleClick);

            UclNgThumbnailOld.uclMiniImage1.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);
            UclNgThumbnailOld.uclMiniImage2.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);
            UclNgThumbnailOld.uclMiniImage3.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);
            UclNgThumbnailOld.uclMiniImage4.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);
            UclNgThumbnailOld.uclMiniImage5.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);
            UclNgThumbnailOld.uclMiniImage6.OnThumbnailDoubleClick += new uclMiniImage.DoubleClickEventHandler(UclNgThumbnailOld_OnThumbnailDoubleClick);

            UclNgListReal.listViewNGItem.SelectedIndexChanged += new EventHandler(UclNgListReal_listViewNGItem_SelectedIndexChanged);
            UclNgListOld.listViewNGItem.SelectedIndexChanged += new EventHandler(UclNgListOld_listViewNGItem_SelectedIndexChanged);

            UclNgThumbnailReal.OnFilterUpdate += new uclNgThumbnail.FilterUpdateEventHandler(UclNgThumbnailReal_OnFilterUpdate);
            UclNgThumbnailOld.OnFilterUpdate += new uclNgThumbnail.FilterUpdateEventHandler(UclNgThumbnailOld_OnFilterUpdate);

            //UclRecipeList.btnSelect.Click += new EventHandler(UclRecipeList_btnSelect_Click);
            updateStatusBar();

            _keyMask = new clsTextboxKeyPressMask(new KeyPressMask_InvalidFileCharUnderBar());
            _keyMask.SetTextBox(textLotNo);

            UclNgThumbnailReal.btnFirst.Click += new EventHandler(UclNgThumbnailReal_btnFirst_Click);
            UclNgThumbnailReal.btnLast.Click += new EventHandler(UclNgThumbnailReal_btnLast_Click);
            UclNgThumbnailOld.btnFirst.Click += new EventHandler(UclNgThumbnailOld_btnFirst_Click);
            UclNgThumbnailOld.btnLast.Click += new EventHandler(UclNgThumbnailOld_btnLast_Click);

            UclNgThumbnailOld.uclMiniImage1.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage1_OnThumbnailClick);
            UclNgThumbnailOld.uclMiniImage2.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage2_OnThumbnailClick);
            UclNgThumbnailOld.uclMiniImage3.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage3_OnThumbnailClick);
            UclNgThumbnailOld.uclMiniImage4.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage4_OnThumbnailClick);
            UclNgThumbnailOld.uclMiniImage5.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage5_OnThumbnailClick);
            UclNgThumbnailOld.uclMiniImage6.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailOld_uclMiniImage6_OnThumbnailClick);

            UclNgThumbnailReal.uclMiniImage1.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage1_OnThumbnailClick);
            UclNgThumbnailReal.uclMiniImage2.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage2_OnThumbnailClick);
            UclNgThumbnailReal.uclMiniImage3.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage3_OnThumbnailClick);
            UclNgThumbnailReal.uclMiniImage4.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage4_OnThumbnailClick);
            UclNgThumbnailReal.uclMiniImage5.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage5_OnThumbnailClick);
            UclNgThumbnailReal.uclMiniImage6.OnThumbnailClick += new uclMiniImage.ClickEventHandler(UclNgThumbnailReal_uclMiniImage6_OnThumbnailClick);

            UclNgListOld.listViewNGItem.KeyPress += new KeyPressEventHandler(UclNgListOld_listViewNGItem_KeyPress);
            UclNgListReal.listViewNGItem.KeyPress += new KeyPressEventHandler(UclNgListReal_listViewNGItem_KeyPress);

            UclRecipeList.btnCopy.Click += new EventHandler(UclRecipeList_btnCopy_Click);
            UclRecipeList.btnPaste.Click += new EventHandler(UclRecipeList_btnPaste_Click);

            shortcutKeyHelper1.SetShortcutKeys(btnNgList, Keys.F1);
            shortcutKeyHelper1.SetShortcutKeys(btnMap, Keys.F2);
            shortcutKeyHelper1.SetShortcutKeys(btnNgMiniImg, Keys.F3);
            shortcutKeyHelper1.SetShortcutKeys(btnTotal, Keys.F4);
            shortcutKeyHelper1.SetShortcutKeys(btnRecipe, Keys.F5);
            shortcutKeyHelper1.SetShortcutKeys(btnOldList, Keys.F6);
            shortcutKeyHelper1.SetShortcutKeys(btnSystem, Keys.F7);
            //shortcutKeyHelper1.SetShortcutKeys(btnNgReset, Keys.F8);  //v1341 追加したが使わないのでコメントアウト
            shortcutKeyHelper1.SetShortcutKeys(btnReset, Keys.F9);
            shortcutKeyHelper1.SetShortcutKeys(btnStart, Keys.F10);
            shortcutKeyHelper1.SetShortcutKeys(btnSuspend, Keys.F11);
            shortcutKeyHelper1.SetShortcutKeys(btnEnd, Keys.F12);

            //Khởi tạo Kiểm soát tốc độ nối tiếp
            InitializeSerialSpeedControll();

            System.Diagnostics.Debug.WriteLine("----------------------1");
        }


        int _iCancelCounter = 0;
        private void MainForm_OnGetImageThread(object sender, APCameraManager.InspectTimeEventArgs e)
        {
            int iCamIndex = e.CamIndex;
            long lInspTime = e.GetImageTotalTime;
            int iCaptureBuffCount = e.CaptureBufferCount;

            Action act = new Action(() =>
            {
                if (_iCancelCounter > 0)
                {
                    lblCaptureBuffCount1.BackColor = SystemColors.Control;
                    lblCaptureBuffCount2.BackColor = SystemColors.Control;
                    _iCancelCounter--;
                    return;
                }
                ToolStripStatusLabel lblTime;
                ToolStripStatusLabel lblCapture;
                if (iCamIndex == 0)
                {
                    lblTime = lblUpInspTime;
                    lblCapture = lblCaptureBuffCount1;
                }
                else
                {
                    lblTime = lblDownInspTime;
                    lblCapture = lblCaptureBuffCount2;
                }
                lblTime.Text = lInspTime.ToString() + "ms";
                lblCapture.Text = iCaptureBuffCount.ToString();

                Color[] rankColor = new Color[] { Color.White, Color.Green, Color.Yellow, Color.Pink, Color.Red };
                if (iCaptureBuffCount > 0)
                {
                    bool bSetFlag = false;
                    for (int i = iCaptureBuffCount; i < rankColor.Length; i++)
                    {
                        if (lblCapture.BackColor == rankColor[i])
                        {
                            bSetFlag = true;
                            break;
                        }
                    }
                    if (bSetFlag == false)
                        lblCapture.BackColor = rankColor[(iCaptureBuffCount > 4) ? 4 : iCaptureBuffCount];
                }
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }

        // khởi tạo Halcon
        bool initHalcon()
        {
            HObject hoImage = null;

            try
            {
                // 横幅ｶﾒﾗの画素サイズ  chiều rộng camera
                // 縦幅ｶﾒﾗの縦サイズ       chiều dọc camere
                int iWidth = SystemParam.GetInstance().camParam[0].PixH;
                int iHeight = SystemParam.GetInstance().camParam[0].PixV;
                HOperatorSet.GenImageConst(out hoImage, "byte", iWidth, iHeight); // có thể là setup chiều rộng và dọc cho camera 
                // Hàm GenImageConst được sử dụng để tạo hình ảnh

            }
            catch (HOperatorException oe)
            {
                LogingDll.Loging_SetLogString(oe.Message + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
                if (hoImage != null)
                    hoImage.Dispose();
            }
            return true;
        }

        void UclRecipeList_btnPaste_Click(object sender, EventArgs e)
        {
            UclRecipeContentsReal.Paste();
        }

        void UclRecipeList_btnCopy_Click(object sender, EventArgs e)
        {
            UclRecipeContentsReal.Copy();
            UclRecipeList.UpdateRecipeList();
        }

        void UclNgListReal_listViewNGItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ListView lv = (ListView)sender;
                ResActionData res = _realResActionData[lv.SelectedIndices[0]];
                if (res.EventMode == ResultActionDataClass.EEventMode.NG)
                    FormNG1ImageDisp(res);
            }
        }

        void UclNgListOld_listViewNGItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ListView lv = (ListView)sender;
                ResActionData res = _oldResActionDataVirtual[lv.SelectedIndices[0]];
                if (res.EventMode == ResultActionDataClass.EEventMode.NG)
                    FormNG1ImageDisp(res);
            }
        }

        void UclNgListReal_OnDoubleClickList(object sender, uclNgList.DoubleClickEventArgs e)
        {
            ListView lv = e.ListViewControl;
            ResActionData res = _realResActionData[lv.SelectedIndices[0]];
            if (res.EventMode == ResultActionDataClass.EEventMode.NG)
                FormNG1ImageDisp(res);
        }

        void UclNgListOld_OnDoubleClickList(object sender, uclNgList.DoubleClickEventArgs e)
        {
            ListView lv = e.ListViewControl;
            ResActionData res = _oldResActionDataVirtual[lv.SelectedIndices[0]];
            if (res.EventMode == ResultActionDataClass.EEventMode.NG)
                FormNG1ImageDisp(res);
        }

        void UclNgThumbnailOld_OnRefreshThumbnail(object sender, EventArgs e)
        {
            RefreshThumbnailOld();
        }

        void UclNgThumbnailReal_OnRefreshThumbnail(object sender, EventArgs e)
        {
            RefreshThumbnailReal();
        }

        void UclNgThumbnailOld_OnButtonClick(object sender, uclNgThumbnail.ButtonClickEventArgs e)
        {
            if (e.ButtonType == uclNgThumbnail.EButtonType.Next)
            {
                ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, UclNgThumbnailOld.iPageNow + 1, -1);
            }
            else
            {
                if (UclNgThumbnailOld.iPageNow > 1)
                    ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, UclNgThumbnailOld.iPageNow - 1, -1);
            }
        }

        void UclNgThumbnailReal_OnButtonClick(object sender, uclNgThumbnail.ButtonClickEventArgs e)
        {
            if (e.ButtonType == uclNgThumbnail.EButtonType.Next)
            {
                ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, UclNgThumbnailReal.iPageNow + 1, -1);
            }
            else
            {
                if (UclNgThumbnailReal.iPageNow > 1)
                    ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, UclNgThumbnailReal.iPageNow - 1, -1);
            }
        }

        void UclNgThumbnailOld_OnFilterUpdate(object sender, uclNgThumbnail.FilterUpdateEventArgs e)
        {
            RefreshThumbnailOld();
        }

        void UclNgThumbnailReal_OnFilterUpdate(object sender, uclNgThumbnail.FilterUpdateEventArgs e)
        {
            bool[] zone;
            bool[] side;
            bool[] kind;

            this.GetRealFilterData(out zone, out side, out kind);
            this._realResActionDataThumbnail = this._realtimeResultActionDataClass.GetItemDatas(0, this._realtimeResultActionDataClass.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG, zone, side, kind);


            RefreshThumbnailReal();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!SystemParam.GetInstance().SpeedControlDispEnable)  // trả về true nếu ddocvj được dữ liệu từ tệp INIT 
            {
                label3.Visible = false;
                textSpeed.Visible = false;
                label11.Visible = false;
            }

            toolStripStatusLabel8.Visible = SystemParam.GetInstance().DownSideEnable;
            StatusLabelDownSide.Visible = SystemParam.GetInstance().DownSideEnable;

            //各画面にthisをセットする
            UclRecipeContentsReal.SetMainForm(this);
            UclRecipeContentsOld.SetMainForm(this);
            UclNgListReal.SetMainForm(this);
            UclNgListOld.SetMainForm(this);
            UclNgThumbnailReal.SetMainForm(this);
            UclNgThumbnailOld.SetMainForm(this);
            UclTotalReal.SetMainForm(this);
            UclTotalOld.SetMainForm(this);
            UclRecipeList.SetMainForm(this);
            UclOldList.SetMainForm(this);
            UclSystem.SetMainForm(this);
            UclImageMain.SetMainForm(this);

            initializeAll();
            //TextBox1のLostFocusイベントハンドラを追加する
            textLotNo.LostFocus += new EventHandler(textLotNo_LostFocus);

            DispChange(SystemStatus.DisplayPair.RealKindlistKinddata);
        }

        //現在時刻
        public DateTime NowTime { get; set; }
        //LotNo
        public string LotNo { get; private set; }

        //タイマー　時計用
        Timer timer = new Timer();

        //検査用のレシピコピー (これを検査に使う)
        public Recipe InspRecipe { get; private set; }
        //過去データ表示の時用一時Recipiコピー
        public Recipe TempoRecipe { get; set; }
        //キャンセル用のレシピコピー
        public Recipe CancelRecipe { get; set; }

        //自動検査結果データ
        //ResultActionDataClass _mainResultActionDataCls;
        //自動検査個別結果List
        List<ResActionData> _realResActionData = new List<ResActionData>();
        //マップ用結果データ
        //       MapResultData _realMapResultdata;

        //過去検査結果データ
        ResultActionDataClass _oldResultActionDataCls;
        //過去個別結果データList
        //        List<ResActionData> _oldResActionData;
        List<ResActionData> _oldResActionDataThumbnail;

        List<ResActionData> _oldResActionDataVirtual;
        //過去マップ用結果データ
        //MapResultData _oldResultdata;

        //1NG用
        public List<ResActionData> _realResActionDataThumbnail;

        List<ToolStripStatusLabel> _lsttsslSpeeds = new List<ToolStripStatusLabel>();

        /// <summary>
        /// 子ウインドウ
        /// </summary>
        List<UserControl> _lstChildWnd = new List<UserControl>();

        //ショートカットキー登録
        clsShortcutManager _clsShortcutman = new clsShortcutManager();

        // キー入力制限
        clsTextboxKeyPressMask _keyMask = null;

        //リセットボタンのDIOクラス
        clsDioCommandMonitor _clsDioCmdMon = new clsDioCommandMonitor();

        //NGダイアログ
        private frmNgDialog _instance = null;
        public frmNgDialog _frmNgdialog
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new frmNgDialog(400);
                    _instance.Closed += new EventHandler(OnNgDialog_Closed);
                    this.AddOwnedForm(_instance);
                }
                return _instance;
            }
        }

        private frmNg1Image _instanceNg1Image = null;
        public frmNg1Image _frmNg1ImageDialog
        {
            get
            {
                if (_instanceNg1Image == null || _instanceNg1Image.IsDisposed)
                {
                    _instanceNg1Image = new frmNg1Image(true);
                    _instanceNg1Image.Closed += new EventHandler(OnNg1ImageDialog_Closed);
                    this.AddOwnedForm(_instanceNg1Image);
                }
                return _instanceNg1Image;
            }
        }

        //システムエラーダイアログ
        private frmSystemErrorDialog _instanceSys = null;
        public frmSystemErrorDialog _frmSysErrorDialog
        {
            get
            {
                if (_instanceSys == null || _instanceSys.IsDisposed)
                {
                    _instanceSys = new frmSystemErrorDialog();
                    _instanceSys.Closed += new EventHandler(OnSysErrorDialog_Closed);
                    this.AddOwnedForm(_instanceSys);
                }
                return _instanceSys;
            }
        }

        //未検査監視ダイアログ
        private frmNoInspectionDialog _instanceNoInsp = null;
        public frmNoInspectionDialog _frmNoinspDialog
        {
            get
            {
                if (_instanceNoInsp == null || _instanceNoInsp.IsDisposed)
                {
                    _instanceNoInsp = new frmNoInspectionDialog(400);
                    _instanceNoInsp.SetText("検査が開始されていません。");
                    _instanceNoInsp.SetTittle("未検査監視ダイアログ");
                    _instanceNoInsp.SetBackColor(SystemParam.GetInstance().PopupColorNG);
                    _instanceNoInsp.Closed += new EventHandler(OnNoInspDialog_Closed);
                    this.AddOwnedForm(_instanceNoInsp);
                }
                return _instanceNoInsp;
            }
        }

        //測長監視ダイアログ
        private frmNoInspectionDialog _instanceLengthMeas = null;
        public frmNoInspectionDialog _frmLengthMeasDialog
        {
            get
            {
                if (_instanceLengthMeas == null || _instanceLengthMeas.IsDisposed)
                {
                    LogingDllWrap.LogingDll.Loging_SetLogString("検査が正常に開始されていません。検査を一度終了し、改めて検査を開始して下さい。");
                    _instanceLengthMeas = new frmNoInspectionDialog(400);
                    _instanceLengthMeas.SetText("検査が正常に開始されていません。\n検査を一度終了し、改めて検査を開始して下さい。");
                    _instanceLengthMeas.SetTittle("測長監視ダイアログ");
                    _instanceLengthMeas.Closed += new EventHandler(OnLengthMeasDialog_Closed);
                    this.AddOwnedForm(_instanceLengthMeas);
                }
                return _instanceLengthMeas;
            }
        }

        private frmExternalOutputCancelDialog _instanceExtOut1Cancel = null;
        public frmExternalOutputCancelDialog _frmExtOut1CancelDialog
        {
            get
            {
                if (_instanceExtOut1Cancel == null || _instanceExtOut1Cancel.IsDisposed)
                {
                    _instanceExtOut1Cancel = new frmExternalOutputCancelDialog(400, this.Location.X + 5);
                    _instanceExtOut1Cancel.Closed += new EventHandler(OnExternalOutputCancelDialog_Closed);
                    this.AddOwnedForm(_instanceExtOut1Cancel);
                }
                return _instanceExtOut1Cancel;
            }

        }

        private void DispChange(SystemStatus.DisplayPair disp)
        {
            // DispVisible(disp);
            if (SystemStatus.GetInstance().CheckChangeDisp(disp))
            {
                DispVisible2(disp);
                UclImageMain.Visible = false;
            }
            else
            {
                Utility.ShowMessage(this, "状態エラー", MessageType.Error);
            }
        }

        //左右の画面のユーザーコントロールをfalseにする
        private void DispUclVisibleAllFalse()
        {
            UclNgListReal.Visible = false;
            UclNgListOld.Visible = false;
            SystemStatus.GetInstance().DispNgList = false;

            UclRecipeList.Visible = false;
            SystemStatus.GetInstance().DispKindList = false;

            UclOldList.Visible = false;
            SystemStatus.GetInstance().DispOldProductList = false;

            UclSheetMapReal.Visible = false;
            UclSheetMapOld.Visible = false;
            SystemStatus.GetInstance().DispMap = false;

            UclNgThumbnailReal.Visible = false;
            UclNgThumbnailOld.Visible = false;
            SystemStatus.GetInstance().DispNgThumbnail = false;

            UclTotalReal.Visible = false;
            UclTotalOld.Visible = false;
            SystemStatus.GetInstance().DispTotal = false;

            UclRecipeContentsReal.Visible = false;
            UclRecipeContentsOld.Visible = false;
            SystemStatus.GetInstance().DispKindContents = false;

            UclSystem.Visible = false;
            SystemStatus.GetInstance().DispSystem = false;


            SystemStatus.GetInstance().DispNgListOld = false;
            SystemStatus.GetInstance().DispMapOld = false;
            SystemStatus.GetInstance().DispNgThumbnailOld = false;
            SystemStatus.GetInstance().DispTotalOld = false;
            SystemStatus.GetInstance().DispKindContentsOld = false;

        }

        private void DispVisible2(SystemStatus.DisplayPair disp)
        {
            //画面表示のvisibleをfalseにする
            DispUclVisibleAllFalse();

            if (disp == SystemStatus.DisplayPair.RealNglistMap)
            {
                //realNGリスト
                SystemStatus.GetInstance().DispNgList = true;
                UclNgListReal.Visible = true;
                //realMap
                SystemStatus.GetInstance().DispMap = true;
                UclSheetMapReal.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldNglistMap)
            {
                //oldNGリスト
                SystemStatus.GetInstance().DispNgListOld = true;
                UclNgListOld.Visible = true;
                //oldMap   
                SystemStatus.GetInstance().DispMapOld = true;
                UclSheetMapOld.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.RealKindlistKinddata)
            {
                //品種リスト
                SystemStatus.GetInstance().DispKindList = true;
                UclRecipeList.Visible = true;
                //real品種データ
                SystemStatus.GetInstance().DispKindContents = true;
                UclRecipeContentsReal.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldProductlist)
            {
                //過去生産リスト
                SystemStatus.GetInstance().DispOldProductList = true;
                UclOldList.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.RealNglistNgthumb)
            {
                //realNGリスト
                SystemStatus.GetInstance().DispNgList = true;
                UclNgListReal.Visible = true;
                //realNGサムネイル
                SystemStatus.GetInstance().DispNgThumbnail = true;
                UclNgThumbnailReal.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldNglistNgthumb)
            {
                //oldNGリスト
                SystemStatus.GetInstance().DispNgListOld = true;
                UclNgListOld.Visible = true;
                //oldNGサムネイル
                SystemStatus.GetInstance().DispNgThumbnailOld = true;
                UclNgThumbnailOld.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.RealNglistTotal)
            {
                //realNGリスト
                SystemStatus.GetInstance().DispNgList = true;
                UclNgListReal.Visible = true;
                //real累計
                SystemStatus.GetInstance().DispTotal = true;
                UclTotalReal.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldNglistTotal)
            {
                //oldNGリスト
                SystemStatus.GetInstance().DispNgListOld = true;
                UclNgListOld.Visible = true;
                //old累計
                SystemStatus.GetInstance().DispTotalOld = true;
                UclTotalOld.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldNglistKinddata)
            {
                //oldNGリスト
                SystemStatus.GetInstance().DispNgListOld = true;
                UclNgListOld.Visible = true;
                //old品種データ
                SystemStatus.GetInstance().DispKindContentsOld = true;
                UclRecipeContentsOld.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.RealNglistSystem)
            {
                //realNGリスト
                SystemStatus.GetInstance().DispNgList = true;
                UclNgListReal.Visible = true;
                //システム
                SystemStatus.GetInstance().DispSystem = true;
                UclSystem.Visible = true;
            }
            else if (disp == SystemStatus.DisplayPair.OldNglistSystem)
            {
                //oldNGリスト
                SystemStatus.GetInstance().DispNgListOld = true;
                UclNgListOld.Visible = true;
                //システム
                SystemStatus.GetInstance().DispSystem = true;
                UclSystem.Visible = true;
            }
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                DispChange(SystemStatus.DisplayPair.RealKindlistKinddata);
            }
            else
            {
                DispChange(SystemStatus.DisplayPair.OldNglistKinddata);
            }
        }

        private void btnNgList_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Old)
            {
                SystemStatus.GetInstance().DataDispMode = SystemStatus.ModeID.Real;
                ChangeBtnName();
            }

            DispChange(SystemStatus.DisplayPair.RealNglistMap);

        }

        private void btnNgMiniImg_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                DispChange(SystemStatus.DisplayPair.RealNglistNgthumb);
            }
            else
            {
                DispChange(SystemStatus.DisplayPair.OldNglistNgthumb);
            }
        }

        private void btnNg1Img_Click(object sender, EventArgs e)
        {
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                DispChange(SystemStatus.DisplayPair.RealNglistTotal);
            }
            else
            {
                DispChange(SystemStatus.DisplayPair.OldNglistTotal);
            }
        }

        private void btnOldList_Click(object sender, EventArgs e)
        {
            LogingDll.Loging_SetLogString("(ActionCheck):[過去リスト]ボタン押下した");

            //レシピに変更があるか。
            ChangeRecipeMessage();

            DispChange(SystemStatus.DisplayPair.OldProductlist);
        }

        private void btnSystem_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                DispChange(SystemStatus.DisplayPair.RealNglistSystem);
            }
            else
            {
                DispChange(SystemStatus.DisplayPair.OldNglistSystem);
            }
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                DispChange(SystemStatus.DisplayPair.RealNglistMap);
            }
            else
            {
                DispChange(SystemStatus.DisplayPair.OldNglistMap);
            }
        }

        public void ChangeBtnName()
        {
            SystemStatus.ModeID modeID = SystemStatus.GetInstance().DataDispMode;
            switch (modeID)
            {
                case SystemStatus.ModeID.Real:
                    //ボタンの名前変更               đổi tên Nút
                    btnNgList.Visible = false;
                    btnImageMain.Visible = true & SystemParam.GetInstance().IM_Enabled;
                    btnNgList.Text = AppData.BTN_NGLIST;
                    btnMap.Text = AppData.BTN_MAP;
                    btnNgMiniImg.Text = AppData.BTN_NGMINIIMG;
                    btnNg1Img.Text = AppData.BTN_NG1IMG;
                    btnTotal.Text = AppData.BTN_TOTAL;
                    btnRecipe.Text = AppData.BTN_RECIPE;
                    //ボタンの色変更               Thay đổi màu nút
                    btnNgList.BackColor = SystemColors.Control;
                    btnMap.BackColor = SystemColors.Control;
                    btnNgMiniImg.BackColor = SystemColors.Control;
                    btnNg1Img.BackColor = SystemColors.Control;
                    btnTotal.BackColor = SystemColors.Control;
                    btnRecipe.BackColor = SystemColors.Control;

                    break;
                case SystemStatus.ModeID.Old:
                    //ボタンの名前変更
                    btnNgList.Visible = true;
                    btnImageMain.Visible = false;
                    btnNgList.Text = AppData.BTN_OLD_NGLIST;
                    btnMap.Text = AppData.BTN_OLD_MAP;
                    btnNgMiniImg.Text = AppData.BTN_OLD_NGMINIIMG;
                    btnNg1Img.Text = AppData.BTN_OLD_NG1IMG;
                    btnTotal.Text = AppData.BTN_OLD_TOTAL;
                    btnRecipe.Text = AppData.BTN_OLD_RECIPE;
                    //ボタンの色変更
                    btnNgList.BackColor = Color.Aqua;
                    btnMap.BackColor = Color.Violet;
                    btnNgMiniImg.BackColor = Color.Violet;
                    btnNg1Img.BackColor = Color.Violet;
                    btnTotal.BackColor = Color.Violet;
                    btnRecipe.BackColor = Color.Violet;

                    DispChange(SystemStatus.DisplayPair.OldNglistMap);
                    break;
            }

            ChangeTitleText();
        }

        private void ChangeTitleText()
        {
            //メインフォームのテキストの変更
            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                this.Text = _sTittle;
            }
            else if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Old)
            {
                //  string stData ="  " + _oldResultActionDataCls.LotNo + "  " + _oldResultActionDataCls.HinsyuName + "  " + _oldResultActionDataCls.StTime;
                string stName = UclOldList.GetSelectRow();

                this.Text = _sTittle + "  " + stName;

                // this.Text = AppData.OLD_APP_NAME ;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //現在時間の取得
            NowTime = DateTime.Now;
            //表示する
            this.statusLabelNowTime.Text = NowTime.ToString();
        }

        private void InspectionRecipeCopy()
        {
            //検査用のレシピのコピー
            Recipe recipe = Recipe.GetInstance();
            InspRecipe = recipe.Copy();

            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
            {

                CheckTextWide(textKindName, InspRecipe.KindName);
                textKindName.Text = InspRecipe.KindName;
            }
        }

        private void StopLightTime()
        {
            SystemContext syscont = SystemContext.GetInstance();
            for (int i = 0; i < syscont.bLightMeas.Length; i++)
            {
                if (syscont.bLightMeas[i])
                {
                    syscont.LightMeasPeriod[i].Stop();
                    syscont.bLightMeas[i] = false;
                }
            }
        }

        /// <summary>
        /// 照明点灯時間の監視Start
        /// </summary>
        private void StartLightTime()
        {
            SystemContext syscont = SystemContext.GetInstance();
            LightControlManager ltCtrl = LightControlManager.getInstance();

            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                if (InspRecipe.LightParam[i].LightEnable)
                {
                    syscont.LightMeasPeriod[i].Start();
                    syscont.bLightMeas[i] = true;
                }
            }
        }

        public void LightOn()
        {
            Recipe rep = InspRecipe;
            if (rep == null)
                rep = Recipe.GetInstance();
            LightControlManager ltCtrl = LightControlManager.getInstance();
            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                int setLightValue = rep.LightParam[i].LightValue + SystemParam.GetInstance().MainteLightOffset[i]; //V1058 メンテナンス追加 yuasa 20190128：オフセット追加
                if (rep.LightParam[i].LightEnable)
                {
                    if (ltCtrl.GetLight(i).ValueMin > setLightValue)
                    {
                        setLightValue = ltCtrl.GetLight(i).ValueMin;
                    }
                    else if (ltCtrl.GetLight(i).ValueMax < setLightValue)
                    {
                        setLightValue = ltCtrl.GetLight(i).ValueMax;
                    }

                    if (ltCtrl.GetLight(i).GetSupplay() is LightPowerSupplay_IMAC)
                    {
                        ltCtrl.GetLight(i).LightOn(setLightValue, true);
                    }
                    else
                    {
                        //trueのときは、ACKチェックする  通常時：true 自動検査中：false
                        ltCtrl.GetLight(i).LightOn(setLightValue, (SystemStatus.GetInstance().NowState != SystemStatus.State.Inspection));
                    }
                }
                else
                {
                    ltCtrl.GetLight(i).LightOff();
                }
            }
        }
        private void StandbyLight()
        {
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
                return;
            //照明点灯
            LightOn();
            //照明点灯時間監視スタート
            StartLightTime();
        }

        public void StandbyInsp()
        {
            //検査レシピにコピー
            this.InspectionRecipeCopy();

            //照明の点灯
            StandbyLight();

            double[] dZone = new double[this.InspRecipe.Partition];
            for (int i = 0; this.InspRecipe.Partition > i; i++)
            {
                dZone[i] = this.InspRecipe.InspParam[0].Zone[i];
            }

            //マップに幅と高さをセットする
            this.SetMapParameter(dZone);
        }

        public void EndInsp()
        {
            //照明点灯時間監視終了
            StopLightTime();
            //照明の消灯
            LightControlManager.getInstance().AllLightOff();
            //検査レシピを空にする
            ReleaseInspRecipe();
        }

        //検査中のレシピ更新
        public void UpdateRecipe()
        {
            this.InspectionRecipeCopy();
            StandbyLight();
            _autoInsp.BindRecipe(InspRecipe);
        }

        public void ReleaseInspRecipe()
        {
            //品種未選択にもどす
            SystemStatus.GetInstance().SelectRecipe = false;

            //検査レシピを空にする
            this.InspRecipe = null;

            //品種名の表示を消す
            textKindName.Text = "";

            //検査幅の表示を消す
            textWidth.Text = "";

            //マスク幅の表示を消す
            textMask.Text = "";
        }

        private void ChangeState(SystemStatus.State state)
        {
            switch (state)
            {
                case SystemStatus.State.Stop:
                    labelState.BackColor = ColorTranslator.FromHtml(AppData.COLOR_YELLOW);
                    labelState.Text = "停止中";
                    //     btnSuspend.Text = "検査開始(F11)";
                    //btnStart.Text = "中断(Ctrl+F11)";
                    break;
                case SystemStatus.State.Inspection:
                    labelState.BackColor = ColorTranslator.FromHtml(AppData.COLOR_BLUE);
                    labelState.Text = "検査中";
                    //     btnSuspend.Text = "中断(F11)";
                    break;
                case SystemStatus.State.Suspend:
                    labelState.BackColor = ColorTranslator.FromHtml(AppData.COLOR_LIME);
                    labelState.Text = "中断中";
                    //     btnSuspend.Text = "再開(Ctrl+F11)";
                    break;
                default:
                    labelState.BackColor = ColorTranslator.FromHtml(AppData.COLOR_RED);
                    labelState.Text = "error";
                    break;
            }

            SystemStatus.GetInstance().NowState = state;

            ChangeSheetWidthStatus();
            ChangeSheetThicknessStatus();
        }

        private void ChangeSheetWidthStatus()
        {
        }
        private void ChangeSheetWidthStatus_White()
        {
        }
        private void ChangeSheetThicknessStatus()
        {
        }

        // 結果情報
        List<bool> _lstCameraCorrectResults = new List<bool>();
        int _iCameraCorrectWaitMaxCnt = 0;

        public bool CameraLightHosei(bool bCorrect)
        {
            bool result = true;
            Action act = new Action(() =>
                {
                    APCameraManager apCamManeg = APCameraManager.getInstance();
                    SystemParam sysp = SystemParam.GetInstance();

                    if (bCorrect == true)
                    {
                        //取込停止
                        apCamManeg.SyncStopLive();
                        //Close
                        apCamManeg.Terminate();
                        //Open
                        apCamManeg.Load(sysp.CameraPath + AppData.CAMERA_FILE, "");
                        //Rate,Exposure
                        Recipe.GetInstance().SetCamSpeed();
                        Recipe.GetInstance().SetCamExposure();
                        //取込開始
                        apCamManeg.SyncStartLive();
                    }

                    _lstCameraCorrectResults.Clear();
                    _iCameraCorrectWaitMaxCnt = 0;

                    foreach (CameraParam cp in sysp.camParam)
                    {
                        if (cp.OnOff)
                        {
                            BackgroundWorker worker = new BackgroundWorker();
                            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                            _iCameraCorrectWaitMaxCnt++;
                            worker.RunWorkerAsync(new object[] { apCamManeg.GetCamera((int)cp.CamID), bCorrect });
                        }
                    }

                    int iSubTani = (bCorrect == true) ? 1 : 4;
                    if (waitForResult(10000 / iSubTani))
                    {
                        System.Threading.Thread.Sleep(5000 / iSubTani);
                        result = true;
                        for (int i = 0; i < _lstCameraCorrectResults.Count; i++)
                        {
                            result |= _lstCameraCorrectResults[i];
                        }
                    }
                    else
                    {
                        result = false;
                    }
                });
            using (frmProgressForm form = new frmProgressForm(act, null))
            {
                form.Description = "しばらくお待ちください。";
                form.ShowDialog();
            }
            _iCancelCounter = 30;
            return result;
        }

        bool waitForResult(int iTimeout)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            do
            {
                if (_iCameraCorrectWaitMaxCnt == _lstCameraCorrectResults.Count)
                {
                    return true;
                }
                System.Threading.Thread.Sleep(100);
            }
            while (sw.ElapsedMilliseconds < iTimeout);

            return false;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_lstCameraCorrectResults)
            {
                _lstCameraCorrectResults.Add((bool)e.Result);
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] aoArg = (object[])e.Argument;
            e.Result = true;
            //try
            //{
            //    HalconCameraLinX camera = (HalconCameraLinX)aoArg[0];
            //    bool bCorrect = (bool)aoArg[1];
            //    bool bResult = true;
            //    if ((bool)bCorrect)
            //    {
            //        bResult |= camera.SetCameraStatus("shc", new int[] { 2, 512 });
            //        if (bResult)
            //        {
            //            bResult |= camera.SetCameraStatus("wht", new int[] { });
            //        }
            //    }
            //    else
            //    {
            //        bResult |= camera.SetCameraStatus("shc", new int[] { 1, 900 });
            //    }
            //    e.Result = bResult;
            //}
            //catch (Exception)
            //{
            //    e.Result = false;
            //}
        }

        //リセットと検査キューのクリア
        private void ReadyInspection()
        {
            APCameraManager.getInstance().ResetSyncTrigger();
            System.Threading.Thread.Sleep(1000);
            _autoInsp.CameraStartInitialize();
            APCameraManager.getInstance().SetSyncTrigger();
        }

        //Callされる
        private void btnStart_Click_1(object sender, EventArgs e) //V1057 手動外部修正 yuasa 20190115：検査開始をボタンと外部入力から受け付けるため、処理を分割
        {
            inspect_start(false);
        }

        private void inspect_start(bool extSignal) //V1057 手動外部修正 yuasa 20190115：検査開始をボタンと外部入力から受け付けるため、処理を分割
        {
            _clsDioCmdMon.extRecivePop = true; //V1057 手動外部修正 yuasa 20190115：ポップアップFlag：オン

            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
            {
                if (SystemParam.GetInstance().Input1MonitorEnable == true)
                {
                    CommunicationDIO dio = CommunicationManager.getInstance().getCommunicationDIO();
                    bool off = false;
                    dio.IN1(SystemParam.GetInstance().Input1MonitorDInNumber, ref off);
                    if (off == false)
                    {
                        string msg = SystemParam.GetInstance().Input1MonitorMessage1.Replace("\\n", "\n");
                        frmMessageForm frm = new frmMessageForm(msg, MessageType.Warning);
                        frm.ShowDialog();
                        _clsDioCmdMon.extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオフ
                        return;
                    }
                }

                if (!extSignal) //V1057 手動外部修正 yuasa 20190115：外部入力の場合メッセージ非表示
                    this.ChangeRecipeMessage();

                //検査の品種をセットする
                if (!UclRecipeList.SetRecipe(extSignal)) //V1057 手動外部修正 yuasa 20190122：外部信号引数追加
                {
                    _clsDioCmdMon.extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオフ
                    return;
                }

                frmInspectionStart _frmInspStart = new frmInspectionStart();
                this.AddOwnedForm(_frmInspStart);
                _frmInspStart._stKindame = Recipe.GetInstance().KindName;
                _frmInspStart._stLotNo = textLotNo.Text;

                if (extSignal || DialogResult.OK == _frmInspStart.ShowDialog()) //V1057 手動外部修正 yuasa 20190115：外部入力の場合はダイアログなし
                {
                    string stLotNo = _frmInspStart._stLotNo;
                    CheckTextWide(textLotNo, stLotNo);
                    textLotNo.Text = stLotNo;
                    this.LotNo = stLotNo;

                    //照明がついていたら一度消す
                    UclRecipeContentsReal.CheckLedOnOff();
                    _autoInsp.ShadingStart = true;
                    this.StandbyInsp();
                }
                else
                {
                    this.ReleaseInspRecipe();
                    _clsDioCmdMon.extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオフ
                    return;
                }

                //ボタンを押せなくする
                //btnStart.Enabled = false;
                this.ChangeStartEndButtonEnable(false);

                //カメラの自動補正の実行
                if (SystemParam.GetInstance().CamCloseOpenEnable == true)
                    this.CameraLightHosei(true);

                Recipe.GetInstance().SetCamGain();

                if (!_autoInsp.IsDebugForm)
                {
                    //リセットと検査キューのクリア
                    this.ReadyInspection();
                }

                //ステータスを検査中に変更
                // ChangeState(SystemStatus.State.Inspection);

                //サムネイル画像のONOFF
                SumNailOnOff(this.InspRecipe.Partition, SystemStatus.ModeID.Real);

                //結果データクラスのインスタンス
                //this._mainResultActionDataCls = new ResultActionDataClass();
                //                    _realMapResultdata = new MapResultData(UclSheetMapReal);

                //UclRecipeContentsReal.ChangeEnabed();

                clsSignalControl.GetInstance().Clear();

                //検査開始
                //  _autoInsp.SystemResultDir = AppData.PURODUCT_FOLDER;
                //  _autoInsp.SystemImageDir = AppData.IMAGE_FOLDER;
                _autoInsp.SystemResultDir = SystemParam.GetInstance().ProductFolder;
                _autoInsp.SystemImageDir = SystemParam.GetInstance().ImageFolder;
                _autoInsp.BindRecipe(InspRecipe);

                double omoteSpeed = InspRecipe.CamRealSpeedValue;
                double uraSpeed = InspRecipe.CamRealSpeedValueUra;
                if (InspRecipe.UseCommonCamRealSpeed == true)
                {
                    omoteSpeed = SystemParam.GetInstance().Common_RecipeRealSpeedOmote;
                    uraSpeed = SystemParam.GetInstance().Common_RecipeRealSpeedUra;
                }
                _autoInsp.CropNgImageSize_Speed(omoteSpeed, uraSpeed);

                _autoInsp.Start(InspRecipe.KindName, this.LotNo);

                this.UclImageMain.StartInsp();

                UclRecipeList.Enabled = false;
            }
            else if (SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
            {

                if (DialogResult.Yes == Utility.ShowMessage(this, "検査を再開しますか？", MessageType.YesNo))
                {
                    //ボタンを押せなくする
                    btnSuspend.Enabled = false;
                    ChangeState(SystemStatus.State.Inspection);

                    if (SystemParam.GetInstance().OutsideManualExtButtonEnable == true) ///V1057 手動外部修正 yuasa 20190121：中断解除で、手動外部ボタン有効化
                    {
                        chkExtDinInsp.Enabled = true;
                    }

                    //検査を再開する
                    _autoInsp.Start(InspRecipe.KindName, this.LotNo);
                }
            }
            _clsDioCmdMon.extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオフ
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnSuspend.Enabled == false)
                return;

            //  if (SystemStatus.GetInstance().SelectRecipe == true)
            // {

            //      System.Diagnostics.Debug.WriteLine("start click" + DateTime.Now.ToString("hh:mm:ss:fff"));

            //状態の切替
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection)
            {

                if (DialogResult.Yes == Utility.ShowMessage(this, "検査を中断しますか？", MessageType.YesNo))
                {
                    //ボタンを押せなくする
                    // btnStart.Enabled = false;
                    this.ChangeStartEndButtonEnable(false);

                    if (SystemParam.GetInstance().OutsideManualExtButtonEnable == true) ///V1057 手動外部修正 yuasa 20190121：中断解除で、手動外部ボタン無効化
                    {
                        chkExtDinInsp.Enabled = false;
                    }

                    // ChangeState(SystemStatus.State.Suspend);
                    //検査を中断する
                    _autoInsp.Suspend();
                }
            }
            /*
                else if (SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
                {

                    if (DialogResult.Yes == Utility.ShowMessage(this, "検査を再開しますか？", MessageType.YesNo))
                    {
                        //ボタンを押せなくする
                        btnSuspend.Enabled = false;
                        ChangeState(SystemStatus.State.Inspection);
                        //検査を再開する
                        _autoInsp.Start(InspRecipe.KindName, this.LotNo);
                    }
                }
                else if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
                {
                    this.ChangeRecipeMessage();

                    //検査の品種をセットする
                    if (!UclRecipeList.SetRecipe())
                    {
                        return;
                    }

                    frmInspectionStart _frmInspStart = new frmInspectionStart();
                    this.AddOwnedForm(_frmInspStart);
                    _frmInspStart._stKindame = Recipe.GetInstance().KindName;
                    _frmInspStart._stLotNo = textLotNo.Text;

                    if (DialogResult.OK == _frmInspStart.ShowDialog())
                    {
                        string stLotNo = _frmInspStart._stLotNo;
                        CheckTextWide(textLotNo, stLotNo);
                        textLotNo.Text = stLotNo;
                        this.LotNo = stLotNo;

                        this.StandbyInsp();
                    }
                    else
                    {
                        this.ReleaseInspRecipe();

                        return;
                    }

                    //ボタンを押せなくする
                    //btnStart.Enabled = false;
                    this.ChangeStartEndButtonEnable(false);

                    //カメラの自動補正の実行
                    this.CameraLightHosei(true);

                    if (!_autoInsp.IsDebugForm)
                    {
                        //リセットと検査キューのクリア
                        this.ReadyInspection();
                    }

                    //ステータスを検査中に変更
                    ChangeState(SystemStatus.State.Inspection);

                    //サムネイル画像のONOFF
                    SumNailOnOff(this.InspRecipe.Partition, SystemStatus.ModeID.Real);

                    //結果データクラスのインスタンス
                    this._mainResultActionDataCls = new ResultActionDataClass();
                    //                    _realMapResultdata = new MapResultData(UclSheetMapReal);

                    UclRecipeContentsReal.ChangeEnabed();

                    //検査開始
                    //  _autoInsp.SystemResultDir = AppData.PURODUCT_FOLDER;
                    //  _autoInsp.SystemImageDir = AppData.IMAGE_FOLDER;
                    _autoInsp.SystemResultDir = SystemParam.GetInstance().ProductFolder;
                    _autoInsp.SystemImageDir = SystemParam.GetInstance().ImageFolder;
                    _autoInsp.BindRecipe(InspRecipe);
                    _autoInsp.Start(InspRecipe.KindName, this.LotNo);




                }
            */
            //}
            //else
            //{
            //    Utility.ShowMessage(this, "品種が選択されていません", MessageType.Error);
            //}           
        }

        private void btnEnd_Click(object sender, EventArgs e)//V1057 手動外部修正 yuasa 20190115：検査終了をボタンと外部入力から受け付けるため、処理を分割
        {
            inspect_stop(false);
        }
        private void inspect_stop(bool extSignal) //V1057 手動外部修正 yuasa 20190115：検査終了をボタンと外部入力から受け付けるため、処理を分割
        {
            _clsDioCmdMon.extRecivePop = true; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオン
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection || SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
            {
                if (extSignal || DialogResult.Yes == Utility.ShowMessage(this, "検査を終了しますか？", MessageType.YesNo)) //V1057 手動外部修正 yuasa 20190115：外部入力の場合はダイアログなし
                {
                    //ボタンを押せなくする
                    //btnEnd.Enabled = false;
                    this.ChangeStartEndButtonEnable(false);

                    if (SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend && SystemParam.GetInstance().OutsideManualExtButtonEnable == true) ///V1057 手動外部修正 yuasa 20190121：中断解除で、手動外部ボタン有効化
                    {
                        chkExtDinInsp.Enabled = true;
                    }

                    //検査終了
                    _autoInsp.Stop();

                    clsSignalControl.GetInstance().Clear();

                    UclRecipeList.Enabled = true;

                    // ライト補正を停止する
                    //   this.CameraLightHosei(false);
                }
            }
            _clsDioCmdMon.extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：ポップアップFlagオフ
            this._realtimeResultActionDataClass.LastNgDataInit(); //V1057 NG表裏修正 yuasa 20190121：終了時前回Ngデータを初期化
        }

        //NGリストに追加する
        private void NgListAdd(uclNgList ucNGList, string[] item)
        {
            ucNGList.ListAdd(item);
        }


        private void NGListAddVirtual(uclNgList ucNGList, List<ResActionData> items, int iEventModeOK)
        {

            ucNGList.listViewNGItem.VirtualMode = true;
            ucNGList.listViewNGItem.BeginUpdate();
            ucNGList.listViewNGItem.VirtualListSize = items.Count;

            if (!ucNGList.chkboxScrolRock.Checked)
            {
                if (iEventModeOK != (int)ResultActionDataClass.EEventMode.OK)
                {
                    if (items.Count >= 1)
                        ucNGList.listViewNGItem.EnsureVisible(items.Count - 1);
                }
            }
            ucNGList.listViewNGItem.EndUpdate();
        }

        void retrieveVirtualItem(RetrieveVirtualItemEventArgs e, List<ResActionData> data)
        {
            if (e.ItemIndex >= data.Count)
                return;

            if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.NG)
            {
                e.Item = new ListViewItem(new string[]{
                            data[e.ItemIndex].LineNo.ToString(),
                            (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                            "",
                            data[e.ItemIndex].SideId.ToString() +"" + data[e.ItemIndex].CamId.ToString().Remove(0,3),
                            data[e.ItemIndex].InspId.ToString().Replace("暗", ""),
                            data[e.ItemIndex].PositionX.ToString(SystemParam.GetInstance().AddressDecimal),
                            data[e.ItemIndex].ZoneId.ToString(),
                            data[e.ItemIndex].Time.ToString()});
            }
            else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.AlarmSM)
            {
                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                "",
                                "ｼｰﾄｽﾞﾚ",
                                "ｱﾗｰﾑ",
                                data[e.ItemIndex].Width.ToString(SystemParam.GetInstance().SMNgDataDecimal),
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }
            else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.NGSM)
            {
                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                "",
                                "ｼｰﾄｽﾞﾚ",
                                "NG",
                                data[e.ItemIndex].Width.ToString(SystemParam.GetInstance().SMNgDataDecimal),
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }
            else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.AlarmSW)
            {
                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                "",
                                "ｼｰﾄ幅",
                                "ｱﾗｰﾑ",
                                data[e.ItemIndex].Width.ToString(SystemParam.GetInstance().SWNgDataDecimal),
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }
            else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.NGSW)
            {
                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                "",
                                "ｼｰﾄ幅",
                                "NG",
                                data[e.ItemIndex].Width.ToString(SystemParam.GetInstance().SWNgDataDecimal),
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }
            else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.NGST)
            {
                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                "",
                                "ｼｰﾄ厚",
                                "NG",
                                data[e.ItemIndex].Width.ToString(SystemParam.GetInstance().STNgDataDecimal),
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }
            else
            {
                string stEvent;
                if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.Start)
                {
                    stEvent = "開始";
                }
                else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.Stop)
                {
                    stEvent = "終了";
                }
                else if (data[e.ItemIndex].EventMode == ResultActionDataClass.EEventMode.Suspend)
                {
                    stEvent = "中断";
                }
                else
                {
                    stEvent = data[e.ItemIndex].EventMode.ToString();
                }

                e.Item = new ListViewItem(new string[]{
                                data[e.ItemIndex].LineNo.ToString(),
                                (data[e.ItemIndex].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                stEvent,
                                "",
                                "",
                                "",
                                "",
                                data[e.ItemIndex].Time.ToString()});
            }

        }

        void UclNgListReal_listViewNGItem_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            retrieveVirtualItem(e, _realResActionData);
        }

        void UclNgListOld_listViewNGItem_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            retrieveVirtualItem(e, _oldResActionDataVirtual);
        }


        private void NgListAdds(uclNgList ucNGList, List<ResActionData> items)
        {
            for (int i = 0; items.Count > i; i++)
            {
                string[] stItem = new string[]{items[i].LineNo.ToString(),
                                (items[i].PositionY/1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                items[i].EventMode.ToString(),
                                items[i].SideId.ToString(),
                                items[i].InspId.ToString().Replace("暗", ""),
                                items[i].PositionX.ToString(SystemParam.GetInstance().AddressDecimal),
                                items[i].ZoneId.ToString(),
                                items[i].Time.ToString()};
                NgListAdd(ucNGList, stItem);
            }
        }

        //NGリストをクリアする
        private void NgListClear(uclNgList ucNGList)
        {
            ucNGList.ListClear();
        }

        // Virtualモードのリストをクリアする
        private void NGListClearVirtual(uclNgList ucNGList)
        {
            ucNGList.listViewNGItem.VirtualListSize = 0;
        }

        //サムネイル画像のONOFFチェックの設定
        private void SumNailOnOff(int partition, SystemStatus.ModeID modeId)
        {
            if (modeId == SystemStatus.ModeID.Real)
            {
                UclNgThumbnailReal.CheckOn(partition);
            }
            else
            {
                UclNgThumbnailOld.CheckOn(partition);
            }
        }

        //現在時間を返す
        private DateTime GetNowTime()
        {
            DateTime NowTime;
            NowTime = DateTime.Now;

            return NowTime;
        }

        //現在の品種一時保存
        Recipe NowRec { get; set; }

        //過去検査結果データロード
        public bool GenOldResultData(string ResultFolder)
        {
            // クリアする
            NGListClearVirtual(UclNgListOld);
            UclNgThumbnailOld.ClearImageAll();
            UclNgThumbnailOld.ClearResDataAll();
            if (_oldResActionDataThumbnail != null)
                _oldResActionDataThumbnail.Clear();
            UclSheetMapOld.Repaint();

            this._oldResultActionDataCls = new ResultActionDataClass();
            _oldResultActionDataCls.SystemImageDir = SystemParam.GetInstance().ImageFolder;
            _oldResultActionDataCls.SystemResultDir = SystemParam.GetInstance().ProductFolder;

            //過去ファイルロード
            if (!this._oldResultActionDataCls.Load(ResultFolder))
            {
                Utility.ShowMessage(this, "過去のデータを表示できません。", MessageType.Error);
                return false;
            }

            //検査用のレシピのコピー
            Recipe recipe = Recipe.GetInstance();
            NowRec = recipe.Copy();

            Recipe rec = Recipe.GetInstance();
            if (!rec.Load(_oldResultActionDataCls.ResultDir, AppData.ModeID.Old))
            {
                Utility.ShowMessage(this, "品種ロードエラー", MessageType.Error);
                return false;
            }

            // サムネイルのフィルタ状態を元に戻す
            SumNailOnOff(rec.Partition, SystemStatus.ModeID.Old);

            // フィルタ情報を取得する
            bool[] zone;
            bool[] side;
            bool[] kind;
            GetOldFilterData(out zone, out side, out kind);

            // フィルタを考慮したデータを取得する
            _oldResActionDataVirtual = this._oldResultActionDataCls.GetItemDatas(0.0, _oldResultActionDataCls.EndLength, null, null, zone, side, kind);
            this.NGListAddVirtual(UclNgListOld, _oldResActionDataVirtual, -1);

            // サムネイル更新
            RefreshThumbnailOld();
            // ページ位置を初期位置にする
            ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, 1, -1);

            // シートマップ更新
            double[] dZone = new double[rec.Partition];
            for (int i = 0; rec.Partition > i; i++)
            {
                dZone[i] = rec.InspParam[0].Zone[i];
            }
            //マップに幅と高さをセットする
            SetMapParameterOld(dZone, rec.InspParam[0].Width);
            //マップにシート長を入れる
            UclSheetMapOld.SheetLengthMeter = _oldResultActionDataCls.EndLength / 1000;
            //カレントの位置をendlengthにする
            UclSheetMapOld.CurrentPosMeter = _oldResultActionDataCls.EndLength / 1000;

            UclSheetMapOld.Repaint();

            // 品種データ更新
            this.UclRecipeContentsOld.Recipe2Disp();
            //裏のdatagridviewの色の変更
            this.UclRecipeContentsOld.ChangeOldDataGridViewColor();

            //品種名をテキストボックスに入れる
            UclRecipeContentsOld.SetKindName(this._oldResultActionDataCls.HinsyuName);
            //ロットNoをテキストボックスに入れる
            UclRecipeContentsOld.SetOldLotNo(this._oldResultActionDataCls.LotNo);

            //累計NG個数の表示 
            UclTotalOld.SetNgCount(this._oldResultActionDataCls.CountNGCamera, this._oldResultActionDataCls.CountNGItems, this._oldResultActionDataCls.CountNGZone);

            if (recipe.Load(NowRec.SelectItem))
            {
                //キャンセル用のレシピコピー
                CancelRecipe = recipe.Copy();
                ////        _mainForm.UclRecipeContentsReal.RecipeDisp();
                SetRecipeDispReal();

                //レシピ編集フラグ
                SystemStatus.GetInstance().RecipeEdit = false;
                ClearRecipeEdit();

            }
            else
            {
                Utility.ShowMessage(this, "品種ロードエラー", MessageType.Error);
            }

            return true;

        }

        void fetchSheetTipItems(List<ResActionData> data, double dStart, double dEnd, out List<clsSheetTipItem> lstSheetTipItems, int iSelectLineIndex)
        {
            lstSheetTipItems = new List<clsSheetTipItem>();

            if (data == null)
                return;

            int iLineNo = -1;
            if (iSelectLineIndex != -1)
                iLineNo = data.FindIndex(x => x.LineNo == iSelectLineIndex);

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].PositionY >= dStart)
                {
                    if (i == iLineNo)
                        lstSheetTipItems.Add(new clsSheetTipItem(data[i].PositionX, data[i].PositionY, (int)-1, (object)data[i].LineNo));
                    else
                    {
                        int id;
                        id = (int)data[i].InspId;
                        id += (data[i].SideId == AppData.SideID.裏) ? Enum.GetNames(typeof(AppData.InspID)).Length : 0;
                        lstSheetTipItems.Add(new clsSheetTipItem(data[i].PositionX, data[i].PositionY, id, (object)data[i].LineNo));
                    }
                }
                if (data[i].PositionY > dEnd)
                    break;
            }
        }

        public void FetchSheetTipItems(uclSheetMap sender, double dStart, double dEnd, out List<clsSheetTipItem> lstSheetTipItems)
        {
            if (sender == UclSheetMapOld)
            {
                int iSelectIndex = -1;
                int iLineIndex = -1;
                if (UclNgListOld.listViewNGItem.SelectedIndices.Count > 0)
                    iSelectIndex = UclNgListOld.listViewNGItem.SelectedIndices[0];
                if (iSelectIndex != -1)
                    iLineIndex = _oldResActionDataVirtual[iSelectIndex].LineNo;

                fetchSheetTipItems(_oldResActionDataThumbnail, dStart, dEnd, out lstSheetTipItems, iLineIndex);
            }
            else
            {
                int iSelectIndex = -1;
                int iLineIndex = -1;
                if (UclNgListReal.listViewNGItem.SelectedIndices.Count > 0)
                    iSelectIndex = UclNgListReal.listViewNGItem.SelectedIndices[0];
                if (iSelectIndex != -1)
                    iLineIndex = _realResActionData[iSelectIndex].LineNo;
                fetchSheetTipItems(_realResActionDataThumbnail, dStart, dEnd, out lstSheetTipItems, iLineIndex);
            }
        }


        private void GetOldFilterData(out bool[] zone, out bool[] side, out bool[] kind)
        {
            zone = UclNgThumbnailOld.GetOnOffZone();
            side = UclNgThumbnailOld.GetOnOffSide();
            kind = UclNgThumbnailOld.GetOnOffKind();
        }
        private void GetRealFilterData(out bool[] zone, out bool[] side, out bool[] kind)
        {
            zone = UclNgThumbnailReal.GetOnOffZone();
            side = UclNgThumbnailReal.GetOnOffSide();
            kind = UclNgThumbnailReal.GetOnOffKind();
        }

        public void RefreshThumbnailReal()
        {
            bool[] zone;
            bool[] side;
            bool[] kind;

            this.GetRealFilterData(out zone, out side, out kind);

            // サムネイルの表示
            if (UclNgThumbnailReal.LockAutoChangePage)
            {
                // 現在のページを表示
                ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, UclNgThumbnailReal.iPageNow, -1);
            }
            else
            {
                // ラストページ表示
                ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, -1, -1);
            }

            //リスト
            _realResActionData = this._realtimeResultActionDataClass.GetItemDatas(0.0, _realtimeResultActionDataClass.EndLength, null, null, zone, side, kind);
            NGListAddVirtual(UclNgListReal, _realResActionData, -1);

            if (!this.CheckPrevFilterData(zone, side, kind))
            {
                NGListAddVirtual(UclNgListReal, _realResActionData, -1);
            }
        }

        public void RefreshThumbnailOld()
        {
            bool[] zone;
            bool[] side;
            bool[] kind;
            //絞込みデータを取得する
            this.GetOldFilterData(out zone, out side, out kind);

            //サムネイル
            _oldResActionDataThumbnail = this._oldResultActionDataCls.GetItemDatas(0.0, _oldResultActionDataCls.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG, zone, side, kind);
            if (_oldResActionDataThumbnail.Count >= 0)
            {
                ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, 1, -1);
            }
            else
            {
                ChangePage(UclNgThumbnailOld, null, 0, -1);
            }

            //リスト
            _oldResActionDataVirtual = this._oldResultActionDataCls.GetItemDatas(0.0, _oldResultActionDataCls.EndLength, null, null, zone, side, kind);
            NGListAddVirtual(UclNgListOld, _oldResActionDataVirtual, -1);

        }

        public void RefreshThumbnail()
        {
            bool[] zone;
            bool[] side;
            bool[] kind;

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Old)
            {
                //絞込みデータを取得する
                this.GetOldFilterData(out zone, out side, out kind);

                //サムネイル
                _oldResActionDataThumbnail = this._oldResultActionDataCls.GetItemDatas(0.0, _oldResultActionDataCls.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG, zone, side, kind);
                if (_oldResActionDataThumbnail.Count > 0)
                {
                    ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, 1, -1);
                    /*
                                        //サムネイル画像のページ数セット
                                        UclNgThumbnailOld.SetMaxPage(_oldResActionDataThumbnail.Count / 6, _oldResActionDataThumbnail.Count % 6);
                                        //サムネイル表示の始めの6個セット
                                        for (int i = 0; 6 > i; i++)
                                        {
                                            if (_oldResActionDataThumbnail.Count > i)
                                            {
                                                HObject hoj = _oldResActionDataThumbnail[i].GetImage();
                                                UclNgThumbnailOld.SetImage(i, hoj);
                                                UclNgThumbnailOld.SetResData(i, _oldResActionDataThumbnail[i]);
                                                if (hoj != null)
                                                {
                                                    hoj.Dispose();
                                                }
                                            }
                                            else
                                            {
                                                UclNgThumbnailOld.ClearImage(i);
                                                UclNgThumbnailOld.ClearResData(i);
                                            }
                                        }
                     */
                }
                else
                {
                    ChangePage(UclNgThumbnailOld, null, 0, -1);
                    //                    UclNgThumbnailOld.SetMaxPage(0, 0);
                    //                    ChangePage(0, 0);
                }
                //リスト
                _oldResActionDataVirtual = this._oldResultActionDataCls.GetItemDatas(0.0, _oldResultActionDataCls.EndLength, null, null, zone, side, kind);
                NGListAddVirtual(UclNgListOld, _oldResActionDataVirtual, -1);
                //                this.NgListClear(UclNgListOld);
                //                this.NgListAdds(UclNgListOld, this._oldResActionData);
            }
            else
            {
                this.GetRealFilterData(out zone, out side, out kind);
                //サムネイル
                //this._resAcDataThumb = this._realtimeResultActionDataClass.GetItemDatas(0, this._realtimeResultActionDataClass.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG, zone, side, kind);
                /*
                UclNgThumbnailReal.SetMaxPage(this._realResActionDataThumbnail.Count / 6, this._realResActionDataThumbnail.Count % 6);
                if (!UclNgThumbnailReal.LockAutoChangePage)
                {
                    UclNgThumbnailReal.SetNowPageMax();
                }
                ChangePage(this.UclNgThumbnailReal.iPageNow, this._realResActionDataThumbnail.Count % 6);
                */

                // サムネイルの表示
                if (UclNgThumbnailReal.LockAutoChangePage)
                {
                    // 現在のページを表示
                    ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, UclNgThumbnailReal.iPageNow, -1);
                }
                else
                {
                    // ラストページ表示
                    ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, -1, -1);
                }

                //リスト
                //this._mainResActionData = this._realtimeResultActionDataClass.GetItemDatas(0, this._realtimeResultActionDataClass.EndLength, null, null, zone, side, kind);

                if (!this.CheckPrevFilterData(zone, side, kind))
                {
                    NGListAddVirtual(UclNgListReal, _realResActionData, -1);
                    //                    this.NgListClear(UclNgListReal);
                    //                    this.NgListAdds(UclNgListReal, this._realResActionData);
                }
            }
        }

        private bool CheckPrevFilterData(bool[] zone, bool[] side, bool[] kind)
        {
            bool bb = true;

            for (int i = 0; zone.Length > i; i++)
            {
                if (zone[i] != zonePrev[i])
                {
                    bb = false;
                    break;
                }
            }

            for (int i = 0; side.Length > i; i++)
            {
                if (side[i] != sidePrev[i])
                {
                    bb = false;
                    break;
                }
            }

            for (int i = 0; i < kind.Length; i++)
            {
                if (kind[i] != kindPrev[i])
                {
                    bb = false;
                    break;
                }
            }
            zonePrev = zone;
            sidePrev = side;
            kindPrev = kind;

            return bb;
        }


        private bool[] zonePrev = new[] { true, true, true, true, true, true, true, true,
                                          true, true, true, true, true, true, true, true };
        private bool[] sidePrev = new[] { true, true };
        private bool[] kindPrev = new[] { true, true, true, true, true, true };

        /// <summary>
        /// 表示すべき画像データを表示する
        /// </summary>
        /// <param name="ctrlThumbnail">サムネイル表示コントロール</param>
        /// <param name="lstActData">結果データリスト</param>
        /// <param name="iPage">表示ページ
        /// >0 指定ページ
        /// =0 ページクリア
        /// <0 最後のページ
        /// </param>
        public void ChangePage(uclNgThumbnail ctrlThumbnail, List<ResActionData> lstActData, int iPage, int index)
        {
            int iMaxPage = 1;
            int iMaxCnt = 1;
            if (iPage == 0)
            {
                ctrlThumbnail.ClearImageAll();
                ctrlThumbnail.ClearResDataAll();
                ctrlThumbnail.ClearColorSelsectItemAll();
                iPage = 1;
            }
            else
            {
                if (lstActData == null)
                {
                    return;
                }

                //カウント指定
                iMaxCnt = lstActData.Count;
                iMaxPage = iMaxCnt / 6 + ((iMaxCnt % 6) == 0 ? 0 : 1);
                if (iMaxPage == 0)
                    iMaxPage = 1;

                // ページ位置の指定
                if (iPage < 0 || iPage > iMaxPage)
                    iPage = iMaxPage;

                // 現在のページを表示
                for (int i = 0; i < 6; i++)
                {
                    int iIndex = i + (iPage - 1) * 6;
                    if (iIndex < iMaxCnt)
                    {
                        ResActionData resData = lstActData[iIndex];
                        HObject dmy1 = null, dmy2 = null;
                        HObject hoImg = null;
                        HTuple htNum;
                        resData.GetImage(ref dmy1, ref dmy2);
                        HOperatorSet.CountObj(dmy1, out htNum);
                        if (htNum.I > 0)
                            HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                        else
                            ctrlThumbnail.ClearImage(i);
                        ctrlThumbnail.SetImage(i, hoImg);
                        ctrlThumbnail.SetResData(i, resData);
                        if (hoImg != null)
                        {
                            hoImg.Dispose();
                        }
                        if (i == index)
                        {
                            ctrlThumbnail.SetColorSelsectItem(i);
                        }
                        else
                        {
                            if (ctrlThumbnail.iPageNow != iPage)
                            {
                                ctrlThumbnail.ClearColorSelsectItem(i);
                            }

                            if (ctrlThumbnail.iPageNow == iPage && index >= 0)
                            {
                                ctrlThumbnail.ClearColorSelsectItem(i);
                            }
                        }
                    }
                    else
                    {
                        ctrlThumbnail.ClearImage(i);
                        ctrlThumbnail.ClearResData(i);
                        ctrlThumbnail.ClearColorSelsectItem(i);
                    }
                }
            }
            // ページ番号および最大ページを更新する
            ctrlThumbnail.SetNowPage(iPage);
            ctrlThumbnail.SetMaxPage(iMaxCnt / 6, iMaxCnt % 6);
        }

        //Page:現在のページ　Remaind:余りの数
        public void ChangePage(int Page, int Remaind)
        {

            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Old)
            {
                if (Remaind > 0 && UclNgThumbnailOld.iPageMax == Page)
                {
                    for (int i = 0; 6 > i; i++)
                    {
                        if (Remaind > i)
                        {

                            ResActionData resData = _oldResActionDataThumbnail[i + (Page - 1) * 6];
                            HObject dmy1 = null, dmy2 = null;
                            HObject hoImg = null;
                            HTuple htNum;
                            resData.GetImage(ref dmy1, ref dmy2);
                            HOperatorSet.CountObj(dmy1, out htNum);
                            if (htNum.I > 0)
                                HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                            UclNgThumbnailOld.SetImage(i, hoImg);
                            UclNgThumbnailOld.SetResData(i, resData);
                            if (hoImg != null)
                            {
                                hoImg.Dispose();
                            }
                        }
                        else
                        {
                            UclNgThumbnailOld.ClearImage(i);
                            UclNgThumbnailOld.ClearResData(i);
                        }
                    }
                }
                else if (Remaind == 0 && Page == 0)
                {
                    UclNgThumbnailOld.ClearImageAll();
                    UclNgThumbnailOld.ClearResDataAll();

                }
                else
                {
                    for (int i = 0; 6 > i; i++)
                    {
                        if (_oldResActionDataThumbnail.Count > 0)
                        {
                            ResActionData resreal = _oldResActionDataThumbnail[i + (Page - 1) * 6];
                            HObject dmy1 = null, dmy2 = null;
                            HObject hoImg = null;
                            HTuple htNum;
                            resreal.GetImage(ref dmy1, ref dmy2);
                            HOperatorSet.CountObj(dmy1, out htNum);
                            if (htNum.I > 0)
                                HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                            UclNgThumbnailOld.SetImage(i, hoImg);
                            UclNgThumbnailOld.SetResData(i, resreal);
                            if (hoImg != null)
                            {
                                hoImg.Dispose();
                            }
                        }
                    }
                }
                UclNgThumbnailOld.SetNowPage(Page);
            }
            else
            {
                //サムネイル用個別結果リスト
                //List<ResActionData> _thumbResAcData = new List<ResActionData>();
                //_thumbResAcData = this._realtimeResultActionDataClass.GetItemDatas(0, this._realtimeResultActionDataClass.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG,null,null,null);
                if (Remaind > 0 && UclNgThumbnailReal.iPageMax == Page)
                {
                    for (int i = 0; 6 > i; i++)
                    {
                        if (Remaind > i)
                        {
                            ResActionData resreal = this._realResActionDataThumbnail[i + (Page - 1) * 6];
                            HObject dmy1 = null, dmy2 = null;
                            HObject hoImg = null;
                            HTuple htNum;
                            resreal.GetImage(ref dmy1, ref dmy2);
                            HOperatorSet.CountObj(dmy1, out htNum);
                            if (htNum.I > 0)
                                HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                            UclNgThumbnailReal.SetImage(i, hoImg);
                            UclNgThumbnailReal.SetResData(i, resreal);
                            if (hoImg != null)
                            {
                                hoImg.Dispose();
                            }
                        }
                        else
                        {
                            UclNgThumbnailReal.ClearImage(i);
                            UclNgThumbnailReal.ClearResData(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; 6 > i; i++)
                    {
                        if (i < _realResActionDataThumbnail.Count)
                        {
                            ResActionData resreal = _realResActionDataThumbnail[i + (Page - 1) * 6];
                            HObject dmy1 = null, dmy2 = null;
                            HObject hoImg = null;
                            HTuple htNum;
                            resreal.GetImage(ref dmy1, ref dmy2);
                            HOperatorSet.CountObj(dmy1, out htNum);
                            if (htNum.I > 0)
                                HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                            UclNgThumbnailReal.SetImage(i, hoImg);
                            UclNgThumbnailReal.SetResData(i, resreal);
                            if (hoImg != null)
                            {
                                hoImg.Dispose();
                            }
                        }
                        else
                        {
                            UclNgThumbnailReal.ClearImage(i);
                            UclNgThumbnailReal.ClearResData(i);
                        }
                    }
                }

                UclNgThumbnailReal.SetNowPage(Page);
            }
        }

        //欠点表示色の登録          Đăng ký màu hiển thị lỗi
        public void SetTipColors()
        {
            SystemParam syspara = SystemParam.GetInstance();

            Color[] _upSideColor = new Color[6];
            for (int i = 0; syspara.markColorUpSide.Count > i; i++)
            {
                _upSideColor[i] = ColorTranslator.FromHtml(syspara.markColorUpSide[i].colorARGB);
            }
            Color[] _downSideColor = new Color[6];
            for (int i = 0; syspara.markColorDownSide.Count > i; i++)
            {
                _downSideColor[i] = ColorTranslator.FromHtml(syspara.markColorDownSide[i].colorARGB);
            }

            UclSheetMapReal.TipColors = _upSideColor.Concat(_downSideColor).ToArray();
            UclSheetMapOld.TipColors = _upSideColor.Concat(_downSideColor).ToArray();
        }

        //APcameraのライブスタート
        public void APcamLiveStart()
        {
#if GRABSYNC
            APCameraManager.getInstance().SyncStartLive();
#else
            APCameraManager.getInstance().StartLive();
#endif
        }

        //自動検査の終了処理
        public void TermInsp()
        {
            termInspection();
        }
        //自動検査の初期化
        public void InitInsp()
        {
            InitInspection();
        }

        //アプリ終了時、調整画面移行時に行う
        #region 自動検査の終了処理
        private void termInspection()
        {
            //APcameraの停止
#if GRABSYNC
            APCameraManager.getInstance().SyncStopLive();

            // ライブが確実にとまるまで待つ
            //            while (APCameraManager.getInstance().IsLive()) ;

#else
            APCameraManager.getInstance().StopLive();
#endif
            //OK・NG・開始・停止・中断のイベントを解除する
            this._autoInsp.EventMonitor.OnEventUpdateResultAction -= OnReceiveUpdateAction;

            //画像取得したい場合には、イベントを登録する（Inspection　→　メイン）
            this._autoInsp.ClearAllRefreshImageEvent();

            //停止
            beginTermInspection();
            this._autoInsp.EndAutoInspection();
            this._autoInsp.OnEventEndAutoInspectionCompletion += new InspectionNameSpace.AutoInspection.EndAutoInspectionCompletionEventHandler(_autoInsp_OnEventEndAutoInspectionCompletion);
            waitTermInspection();

            APCameraManager camManage = APCameraManager.getInstance();
            SystemParam sysp = SystemParam.GetInstance();
            List<int> camno = new List<int>();
            foreach (CameraParam cp in sysp.camParam)
            {
                //OnGrabbedの配列番号を生成する
                camno.Add((int)cp.CamID);
            }
            //Ongrabbedイベントを解除する
            camManage.ResetGrabbedImageEventAll();
            //            foreach (CameraParam cp in sysp.camParam)
            //            {
            //                HalconCameraBase cam = camManage.GetCamera(camno[(int)cp.CamID]);
            //#if GRABSYNC
            //                if (cam != null)
            //                    camManage.ResetGrabbedImageEvent(cam.Index, this._autoInsp.OnGrabbedEventHander);
            //#else             
            //                cam.OnGrabbedImage -= this._autoInsp.OnGrabbedEventHander;
            //#endif
            //            }
            _autoInsp.Dispose();
        }

        #endregion

        bool _bWaitAutoInspectionCompletion = false;
        void beginTermInspection()
        {
            _bWaitAutoInspectionCompletion = false;
        }

        void _autoInsp_OnEventEndAutoInspectionCompletion(object sender, EventArgs e)
        {
            _bWaitAutoInspectionCompletion = true;
        }

        void waitTermInspection()
        {
            do
            {
                Application.DoEvents();
            } while (!_bWaitAutoInspectionCompletion);
        }


        #region 自動検査の初期化
        private AutoInspection _autoInsp;
        /// <summary>
        /// RecipeContentsで使用するためにプロパティ科
        /// </summary>
        public AutoInspection AutoInspection { get { return _autoInsp; } }
        private ResultActionDataClass _realtimeResultActionDataClass;
        private void InitInspection()
        {
            this._realtimeResultActionDataClass = new ResultActionDataClass();

            APCameraManager camManage = APCameraManager.getInstance();
            SystemParam sysp = SystemParam.GetInstance();
            //有効なカメラの一番早い番号を取得する
            int enableCamNo = 0;
            foreach (CameraParam cp in sysp.camParam)
            {
                if (cp.OnOff == true)
                {
                    enableCamNo = (int)cp.CamID;
                    break;
                }
            }
            List<CameraInfo> cInfo = new List<CameraInfo>();
            List<int> camno = new List<int>();
            foreach (CameraParam cp in sysp.camParam)
            {
                //カメラ情報を作成する 
                cInfo.Add(new CameraInfo(cp.OnOff, cp.CamParts, cp.CamID, cp.ResoH, cp.ResoV, cp.ShiftH, cp.ShiftV, cp.PixH, cp.PixV, camManage.GetCamera(0).ImageHeight, cp.DiscardCount));
                //OnGrabbedの配列番号を生成する
                camno.Add((cp.OnOff == true) ? (int)cp.CamID : enableCamNo);
            }
            //検査インスタンスを生成する
            this._autoInsp = new AutoInspection(cInfo, _realtimeResultActionDataClass);

            //Ongrabbedイベントを登録する
            foreach (CameraParam cp in sysp.camParam)
            {
                HalconCameraBase cam = camManage.GetCamera(camno[(int)cp.CamID]);
#if GRABSYNC
                if (cp.OnOff == true)
                    camManage.SetGrabbedImageEvent((int)cp.CamID, this._autoInsp.OnGrabbedEventHander);
                //camManage.SetGrabbedImageEvent(cam.Index, this._autoInsp.OnGrabbedEventHander);
#else    
                cam.OnGrabbedImage += this._autoInsp.OnGrabbedEventHander;
#endif
            }
            //画像取得したい場合には、イベントを登録する（Inspection　→　メイン）
            this._autoInsp.SetRefreshImageEvent(OnReceiveRefreshImage);
            // 自動調光の時に設定する必要があるためpublic化

            //OK・NG・開始・停止・中断のイベントを登録する
            this._autoInsp.EventMonitor.OnEventUpdateResultAction += OnReceiveUpdateAction;

            //NGイメージの切り抜きサイズ
            this._autoInsp.CropNgImageSize(sysp.CropWidth, sysp.CropHeight, sysp.ScaleWidth, sysp.ScaleHeight);

            //開始する
            this._autoInsp.BeginAutoInspection();
            this._autoInsp.CameraStartInitialize();

            //停止
            //this._autoInsp.EndAutoInspection();

            //検査の開始

            //this._realtimeResultActionDataClass.Start(@"c:\dir", "hinsyu", "lot", DateTime.Now);
            //this._autoInsp.BindRecipe(InspRecipe);
            //this._autoInsp.Start();
            //this._autoInsp.Stop();
            //this._realtimeResultActionDataClass.End(DateTime.Now, 1000.0);
            //this._autoInsp.Suspend();
            //EInspectionStatus sts = this._autoInsp.GetStatus();

            if (SystemParam.GetInstance().EnableLengthMeasMonitor)
            {
                clsLengthMeasMonitor.getInstance().SetAutoInspection(_autoInsp);
            }

        }
        #endregion

        //画像取得したい場合のイベントを受信　
        void OnReceiveRefreshImage(object sender, RefreshImageEvent.RefreshImageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                    {
                        OnReceiveRefreshImage(sender, e);
                    }
                ));
                return;
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();

            int len = Enum.GetValues(typeof(AppData.SideID)).Length;

            for (int i = 0; i < len; i++)
            {
                UclImageMain.KandoDarkBase[i] = e.SideDatas[i].DarkKandoBase - 1;
                UclImageMain.KandoLightBase[i] = e.SideDatas[i].BrightKandoBase;
            }

            //v1325 LightRepeatON値によるライト連続点灯判定追加
            if (true == SystemParam.GetInstance().LightRepeatON)
            {
                if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
                    LightOn();
            }

            //StatusLabelBaseKando.Text = string.Format(" 基準{0},{1},{2},{3} ", e.CameraDatas[0].BrightKandoBase, e.CameraDatas[1].BrightKandoBase, e.CameraDatas[2].BrightKandoBase, e.CameraDatas[3].BrightKandoBase);


            sw.Stop();
            //Console.WriteLine("OnReceiveRefreshImage()01======================== {0}", sw.ElapsedMilliseconds.ToString("F2"));

            UclRecipeContentsReal.RefreshImage(e.ImageTargets, e.UpDownEnbled);
            UclImageMain.RefreshImage(e.UpDownEnbled, e.ImageOriginals, e.ImageTargets, e.ImageInspScales);


            string stValue;
            int iMei, iAnn;
            int iAve, iMin, iMax;
            ToolStripStatusLabel[] lbl = new ToolStripStatusLabel[len];
            lbl[0] = StatusLabelUpSide;
            lbl[1] = StatusLabelDownSide;

            int[] iAveArray = new int[len];

            string[] KandoStr = new string[len];
            string[] MeiValue = new string[len];
            string[] AnnValue = new string[len];

            for (int i = 0; i < len; i++)
            {
                if (e.UpDownEnbled[i] == false)
                    continue;

                iMei = (int)UclImageMain.KandoMax[i] - e.SideDatas[i].BrightKandoBase;
                iMei = (iMei < 0) ? -1 : iMei;

                iAnn = (e.SideDatas[i].DarkKandoBase - 1) - (int)UclImageMain.KandoMin[i];
                iAnn = (iAnn < 0) ? -1 : iAnn;

                iAve = (int)UclImageMain.KandoAve[i];
                iMin = (int)UclImageMain.KandoMin[i];
                iMax = (int)UclImageMain.KandoMax[i];

                MeiValue[i] = "明" + ((iMei < 0) ? "---" : iMei.ToString("D03"));
                AnnValue[i] = "暗" + ((iAnn < 0) ? "---" : iAnn.ToString("D03"));

                stValue = string.Format("{0} {1}({2},{3},{4})",
                    MeiValue[i],
                    AnnValue[i],
                    iAve.ToString("D03"), iMin.ToString("D03"), iMax.ToString("D03"));
                lbl[i].Text = stValue;

                iAveArray[i] = iAve;

                if (SystemParam.GetInstance().InspBrightEnable == true)
                    KandoStr[i] = MeiValue[i];
                if (SystemParam.GetInstance().InspDarkEnable == true)
                {
                    if (KandoStr[i] != "")
                        KandoStr[i] += " ";
                    KandoStr[i] += AnnValue[i];
                }
            }
        }

        #region OK・NG・開始・停止・中断のイベントを受信する
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnReceiveUpdateAction(object sender, EventMonitor.EventMonitorEventArgs e)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new Action(() =>
                {
                    this.OnReceiveUpdateAction(this, e);
                }));
                return;
            }

            //   if (e.EventId == ResultActionDataClass.EEventId.Action || e.EventMode == ResultActionDataClass.EEventMode.NG)
            //   {

            bool[] zone, side, kind;
            this.GetRealFilterData(out zone, out side, out kind);
            this._realResActionData = this._realtimeResultActionDataClass.GetItemDatas(0, e.EndLength, null, null, zone, side, kind);
            this._realResActionDataThumbnail = this._realtimeResultActionDataClass.GetItemDatas(0, e.EndLength, ResultActionDataClass.EEventId.Result, ResultActionDataClass.EEventMode.NG, zone, side, kind);

            NGListAddVirtual(UclNgListReal, _realResActionData, (int)e.EventMode);
            //   }

            //測長の表示
            textLength.Text = (e.EndLength / 1000).ToString(SystemParam.GetInstance().LengthDecimal);
            //マップにシート長を入れる
            UclSheetMapReal.SheetLengthMeter = e.EndLength / 1000;
            //マップの現在ライン
            UclSheetMapReal.CurrentPosMeter = e.EndLength / 1000;

            lblCropSaveBufferCount.Text = _autoInsp.EventMonitor.GetCropImageQueueCount.ToString();

            // EventModeがOKの場合これでぬける
            if (e.EventMode == ResultActionDataClass.EEventMode.OK)
            {
                return;
            }

            if (e.EventMode == ResultActionDataClass.EEventMode.NG)
            {
                //IDがNGのとき
                //シグナルタワー赤とブザー発生
                ResActionData resDt = null;
                int frontReverseSide = -1; //V1057 NG表裏修正 yuasa 20190118：0：表、1：裏、2：表裏
                bool isBothSide = false;//V1333 GetItemSideResult用引数
                bool isCenter = false;//V1333 GetItemSideResult用引数

                List<AppData.ZoneID> lstZones = new List<AppData.ZoneID>();//V1333 ゾーン用リスト

                if (SystemParam.GetInstance().ExtOut1CancelEnable == false)
                {
                    this._realtimeResultActionDataClass.GetItemSideResult(out resDt, out frontReverseSide,out lstZones,out isBothSide,out isCenter); //V1057 NG表裏修正 yuasa 20190118：SideIdがほしいのでここで呼び出し
                    //V1333 lstZones、isBothSide、isCenterを引数に追加。
                    //resDt = this._realtimeResultActionDataClass.GetItemLastResult();                               
                }

                //resDtのposition Recipe recipe = Recipe.GetInstance();でレシピを確認
                //V1333 lstZonesを引数に追加。
                clsSignalControl.GetInstance().SetNG(frontReverseSide, resDt.ZoneId, lstZones, isBothSide ,isCenter); //V1057 NG表裏修正 yuasa 20190118：引数を追加
                
                if (SystemParam.GetInstance().ExtOut1CancelEnable == false)
                {
                    if (SystemParam.GetInstance().NGPopupWindowMode == false)
                    {
                        string colName = SystemParam.GetInstance().PopupColorNG;
                        AppData.SideID sideNo = AppData.SideID.表;
                        //ResActionData resDt = this._realtimeResultActionDataClass.GetItemLastResult();//V1057 NG表裏修正 yuasa 20190118：SideIdがほしいので上に移動
                        if (resDt != null)
                        {
                            sideNo = resDt.SideId;
                            if (sideNo == 0)
                                colName = SystemParam.GetInstance().markColorUpSide[(int)resDt.InspId].colorARGB;
                            else
                                colName = SystemParam.GetInstance().markColorDownSide[(int)resDt.InspId].colorARGB;
                        }
                        //NGダイアログの表示
                        _frmNgdialog.SetText("欠点NG", sideNo.ToString() + "の欠点を検出しました。",
                            (sideNo == AppData.SideID.表) ? Color.White : Color.Black,
                            (sideNo == AppData.SideID.表) ? Color.Black : Color.White
                            );
                        _frmNgdialog.SetBackColor(colName);
                        _frmNgdialog.Show();
                    }
                    else
                    {
                        NgGraphPopup(resDt);
                    }
                    LogingDll.Loging_SetLogString("(ActionCheck):NGポップアップ画面を表示した");

                    UclImageMain.AutoSave((int)resDt.SideId);
                }
                else
                {
                    //外部出力１信号のキャンセル確認
                    _frmExtOut1CancelDialog.SetBackColor(SystemParam.GetInstance().PopupColorNG);
                    _frmExtOut1CancelDialog.Show();
                }

                //最大ページ数のセット
                RefreshThumbnailReal();

                //累計NG個数の表示 
                UclTotalReal.SetNgCount(this._realtimeResultActionDataClass.CountNGCamera, this._realtimeResultActionDataClass.CountNGItems, this._realtimeResultActionDataClass.CountNGZone);

            }

            //IDがSuspendのとき
            if (e.Id == EResultActionId.Suspend)
            {
                //シグナルタワー緑の点灯
                clsSignalControl.GetInstance().SetInspectStatus(ESignalStatus.InspectSuspend);

                ChangeState(SystemStatus.State.Suspend);

                //ボタンを押せるようにする
                //btnStart.Enabled = true;
                this.ChangeStartEndButtonEnable(true);

                System.Diagnostics.Debug.WriteLine("event suspend" + DateTime.Now.ToString("hh:mm:ss:fff"));

                if (SystemParam.GetInstance().EnableLengthMeasMonitor)
                {
                    clsLengthMeasMonitor.getInstance().Stop();
                }
            }

            //IDがStartのとき
            if (e.Id == EResultActionId.Start)
            {
                //シグナルタワー青の点灯
                clsSignalControl.GetInstance().SetInspectStatus(ESignalStatus.InspectStart);

                //ステータスを検査中に変更
                ChangeState(SystemStatus.State.Inspection);

                UclRecipeContentsReal.ChangeEnabled();

                //レシピリストの右側の表示
                UclRecipeList.UpdateRecipeList();


                //開始時間の表示
                textStartTime.Text = e.Time.ToString();
                //puroducttimeの開始
                SystemContext.GetInstance().ProductTime.WriteProductTimeStart(_realtimeResultActionDataClass.HinsyuName,
                                                                               _realtimeResultActionDataClass.LotNo,
                                                                               _realtimeResultActionDataClass.StTime);

                //producttimeの開始
                SystemContext.GetInstance().ProductTime.WriteProductTimeStart(_realtimeResultActionDataClass.HinsyuName,
                                                                               _realtimeResultActionDataClass.LotNo,
                                                                               _realtimeResultActionDataClass.StTime);

                //ボタンを押せるようにする
                //btnStart.Enabled = true;
                this.ChangeStartEndButtonEnable(true);

                System.Diagnostics.Debug.WriteLine("event start" + DateTime.Now.ToString("hh:mm:ss:fff"));

                if (SystemParam.GetInstance().EnableLengthMeasMonitor && SystemParam.GetInstance().LengthMeasMonitorLimitSec > 0)
                {
                    clsLengthMeasMonitor.getInstance().Start();
                }

                _iCancelCounter = 30;
            }

            //IDがResetのとき
            if (e.Id == EResultActionId.Reset)
            {
                //累計のリセット
                //UclTotalReal.ClearCount();

                //サムネイルの選択を消す
                UclNgThumbnailReal.ClearColorSelsectItemAll();
                //サムネイル表示を消す
                UclNgThumbnailReal.ClearImageAll();
                //サムネイルの情報部分を消す
                UclNgThumbnailReal.ClearResDataAll();
                //ページのリセット
                UclNgThumbnailReal.SetMaxPage(0, 0);
                //サムネイルデータのクリア
                _realResActionDataThumbnail.Clear();

                //測長の表示クリア
                //  double dlength = 0.00;
                //  textLength.Text = dlength.ToString(SystemParam.GetInstance().LengthDecimal);

                //過去リストの更新
                //UclOldList.GenResultList();
                UclOldList.GenResultListVirtual();
            }

            //IDがStopのとき
            if (e.Id == EResultActionId.Stop)
            {
                //シグナルタワー黄色点灯
                clsSignalControl.GetInstance().SetInspectStatus(ESignalStatus.InspectStop);

                ChangeState(SystemStatus.State.Stop);

                UclNgThumbnailReal.CheckOFF();

                //puroducttimeの終了
                SystemContext.GetInstance().ProductTime.WriteProductTimeEnd(e.Time, e.EndLength / 1000, this._realtimeResultActionDataClass.CountNG);

                textStartTime.Text = "";


                //NGリストのアイテム削除
                //                NgListClear(UclNgListReal);
                NGListClearVirtual(UclNgListReal);

                //NGダイアログが表示されていたら消す。
                if (_frmNgdialog.Visible)
                {
                    _frmNgdialog.Close();
                }
                if (_frmNg1ImageDialog.Visible)
                {
                    _frmNg1ImageDialog.Close();
                }
                if (_frmExtOut1CancelDialog.Visible)
                {
                    _frmExtOut1CancelDialog.Close();
                }

                this.UclImageMain.StopInsp();

                //累計のリセット
                //UclTotalReal.ClearCount();

                //サムネイルの選択を消す
                UclNgThumbnailReal.ClearColorSelsectItemAll();
                //サムネイル表示を消す
                UclNgThumbnailReal.ClearImageAll();
                //サムネイルの情報部分を消す
                UclNgThumbnailReal.ClearResDataAll();
                //ページのリセット
                UclNgThumbnailReal.SetMaxPage(0, 0);
                //サムネイルデータのクリア
                _realResActionDataThumbnail.Clear();

                //マップ表示のリセット
                ClearSheetMap();

                //照明のOFF　検査レシピを空にする
                this.EndInsp();

                //ロットNOのクリア
                this.textLotNo.Text = "";

                //測長の表示クリア
                double dlength = 0.00;
                textLength.Text = dlength.ToString(SystemParam.GetInstance().LengthDecimal);


                //レシピリストの右側の表示
                UclRecipeList.UpdateRecipeList();

                //指定月以上のProductFolderの削除
                DeleteProductFolder();

                //　品種バックアップ(検査中に品種が更新されている場合があるため)
                clsFilebackup.Backup(SystemParam.GetInstance().RecipeFoldr, SystemParam.GetInstance().BackupFolder);

                //過去リストの更新
                //UclOldList.GenResultList();
                UclOldList.GenResultListVirtual();

                UclRecipeContentsReal.ChangeEnabled();

                this.SetRecipeDispReal();

                // ライト補正を停止する
                if (SystemParam.GetInstance().CamCloseOpenEnable == true)
                    this.CameraLightHosei(false);

                //照明追従のリセット
                this._iLightUpDown = null;

                //UPS強制シャットダウン
                if (SystemStatus.GetInstance().UpsShutDown)
                {
                    tmrUpsShutdown.Enabled = true;
                }

                // 長さ変更チェック
                if (SystemParam.GetInstance().EnableLengthMeasMonitor)
                {
                    clsLengthMeasMonitor.getInstance().Stop();
                }

                _iCancelCounter = 30;
            }

            // btnEnd.Enabled = true;
            this.ChangeStartEndButtonEnable(true);

            //v1338 PC電源ボタン押下対応で停止のときは、このメソッドで検査を停止してから
            //「OnDioCommandEventHandlerPowerOffButton」で終了処理に遷移する
            if ((SystemParam.GetInstance().PowerOffButtonEnable == true) && (e.Id == EResultActionId.Stop)&&(_clsDioCmdMon.PowerOffButtonFlag == true))
            {
                _clsDioCmdMon.PowerOffButtonFlag = false;//フラグの回収
                OnDioCommandEventHandlerPowerOffButton(this, new clsDioCommandMonitor.DioCommandEventArgs(-1, clsDioCommandMonitor.EPublishEventType.Active, this));
            }
        }

        private ResActionData _resDt;
        System.Threading.Thread _thNgGraph;
        private void NgGraphPopup(ResActionData resDt)
        {
            _resDt = resDt.Copy();
            _thNgGraph = new System.Threading.Thread(new System.Threading.ThreadStart(this.WorkPopup));
            _thNgGraph.Name = "NgGraphPopup.WorkPopup";
            _thNgGraph.Start();
        }
        private void WorkPopup()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                _frmNg1ImageDialog.SetNgListDatas(this._realtimeResultActionDataClass.GetNgDatas());
                _frmNg1ImageDialog.SetNgData(_resDt, UclSheetMapReal.SheetWidth, UclSheetMapReal.Zones);
                _frmNg1ImageDialog.RefreshWindow();
                _frmNg1ImageDialog.Show();
            }));
        }
        #endregion

#if !SPEED_MONITOR_NEW
        #region 速度監視の初期処理
        private SpeedMonitor _speedManager;
        private void InitSpeedMonitor()
        {
            APCameraManager camManage = APCameraManager.getInstance();
            SystemParam sysParam = SystemParam.GetInstance();


            int camNo=0;
            foreach (CameraParam p in sysParam.camParam)
            {
                if (p.OnOff == true)
                {
                    camNo = (int)p.CamID;
                    break;
                }
            }

            HalconCameraBase cam = camManage.GetCamera(camNo);

            //インスタンスを生成する
            _speedManager = new SpeedMonitor();
            //イメージ取得イベントを登録する（Camera　→　SpeedMonitor）
#if GRABSYNC
            camManage.SetGrabbedImageEvent(cam.Index, this._speedManager.OnGrabedEventHandler);
#else
            cam.OnGrabbedImage += this._speedManager.OnGrabedEventHandler;
#endif
            //速度通知を登録する（SpeedMonitor　→ メイン）
            _speedManager.OnEventUpdateSpeedEvent += this.OnReceiveUpdateSpeed;
            //開始する
            this._speedManager.BeginSpeedMonitor(SystemParam.GetInstance().camParam[camNo].ResoV, SystemParam.GetInstance().camParam[camNo].PixV);
            //通知を開始する
            this._speedManager.Start();

            //通知を停止する
            //this._speedManager.Stop();
            //停止する
            //this._speedManager.EndSpeedMonitor();
            
        }
        #endregion
        #region 速度監視クラスからのイベントを受信する
        private void OnReceiveUpdateSpeed(object sender, SpeedMonitor.SpeedEventArgs spdEventArgs)
        {
            if (InvokeRequired == true)
            {
                Invoke(new Action(() =>
                {
                    this.OnReceiveUpdateSpeed(sender, spdEventArgs);
                    
                }));
                return;
            }

            double speed = spdEventArgs.Speed;
            textSpeed.Text = string.Format("{0}", speed.ToString("F1"));
          //  textSpeed.BackColor = (textSpeed.BackColor != Color.Green) ? Color.Green : Color.Yellow;
            //検査開始忘れ機能
            clsNoInspectionSpeedMonitor.GetInstance().SetSpeed(speed);
        }
        #endregion
        #region 速度監視の終了処理
        private void TermSpeedMonitor()
        {
            //通知を停止する
            this._speedManager.Stop();
            //停止する
            this._speedManager.EndSpeedMonitor();

            //速度通知を登録する（SpeedMonitor　→ メイン）
            _speedManager.OnEventUpdateSpeedEvent -= this.OnReceiveUpdateSpeed;

            APCameraManager camManage = APCameraManager.getInstance();
            SystemParam sysParam = SystemParam.GetInstance();

            int camNo = 0;
            foreach (CameraParam p in sysParam.camParam)
            {
                if (p.OnOff == true)
                {
                    camNo = (int)p.CamID;
                    break;
                }
            }

            HalconCameraBase cam = camManage.GetCamera(camNo);
            //インスタンスを生成する
            //  _speedManager = new SpeedMonitor();
            //イメージ取得イベントを解除する（Camera　→　SpeedMonitor）
#if GRABSYNC
            camManage.ResetGrabbedImageEvent(cam.Index, this._speedManager.OnGrabedEventHandler);
#else
            cam.OnGrabbedImage -= this._speedManager.OnGrabedEventHandler;
#endif
        }
        #endregion

#else

        clsSpeedMonitor _speedMonitorNew;
        //マイコンから受けるスピードモニタの初期処理         Xử lý ban đầu của màn hình tốc độ nhận được từ vi điều khiển
        private bool InitSpeedMonitorNew()
        {
            if (_speedMonitorNew != null)
                return false;

            //インスタンスを生成する       tạo một cá thể
            _speedMonitorNew = new clsSpeedMonitor();
            _speedMonitorNew.Initialize(CommunicationManager.getInstance().getCommunicationSIO("SpeedMonitor"), CommunicationManager.getInstance().getCommunicationDIO(), -1);
            _speedMonitorNew.Load(AppData.EXE_FOLDER + AppData.SYSTEM_FILE, "");

            //ｶﾒﾗ1の分解能(縦)をセットする         Đặt độ phân giải (dọc) của camera 1
            SetSpeedMoniResoV();

            //スピードモニタのイベント登録            Đăng ký sự kiện giám sát tốc độ
            _speedMonitorNew.SpeedMonitor += this.OnReceiveSpeedMonitor;
            _speedMonitorNew.Start();

            return true;

        }

        public void SetSpeedMoniResoV()
        {
            //ｶﾒﾗ1の分解能(縦)をセットする
            _speedMonitorNew.Resolution = SystemParam.GetInstance().camParam[0].ResoV;
        }

        //マイコンから受けるスピードモニタの終了処理
        private void termSpeedMonitorNew()
        {
            _speedMonitorNew.Stop();

            //スピードモニタのイベント解除
            _speedMonitorNew.SpeedMonitor -= this.OnReceiveSpeedMonitor;

            _speedMonitorNew.Terminate();

        }

        //スピードモニタ(マイコン)のイベント        Sự kiện giám sát tốc độ (vi điều khiển)
        int _iSpeedMonitorCnt = 0;
        private void OnReceiveSpeedMonitor(object sender, SpeedMonitorEventArgs e)
        {
            if (InvokeRequired == true)
            {
                Invoke(new Action(() =>
                {
                    this.OnReceiveSpeedMonitor(sender, e);

                }));
                return;
            }

            _iSpeedMonitorCnt++;
            double speed = e.GetSpeed(ESpeedType.MeterPerMinute);
            textSpeed.Text = string.Format("{0}", speed.ToString(SystemParam.GetInstance().SpeedMainDecimal));
        }
#endif
        bool _bRecursiveFormClosing = false;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //v1338 yuasa：何もしないように修正。代わりに「MainFormCloseProcess」を呼び出す。
        }

        /// <summary>終了処理。FormClosingからコードを移動</summary>//v1338 yuasa
        private bool MainFormCloseProcess(bool bPowerOffButton = false)
        {
            bool bRes = true;
            //終了済みかどうか
            bRes &= !_bRecursiveFormClosing;

            //状態チェック。bPowerOffButtonがtrueなら検査中でもなにもしない。
            if ((bRes == true) && (bPowerOffButton == false))
            {
                if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection
                    || SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
                {
                    Utility.ShowMessage(this, "検査中はシステムを終了できません。", MessageType.Error);
                    bRes = false;
                }
            }

            //レシピ確認。bPowerOffButtonがtrueならレシピが変わっていてもなにもしない。
            if ((bRes == true) && (bPowerOffButton == false))
            {
                this.ChangeRecipeMessage();
            }

            //UPS?シートの名残？？？
            if ((bRes == true) && (bPowerOffButton == false))
            {
                try
                {
                    if (SystemStatus.GetInstance().UpsShutDown)
                    {
                        updateControls(false);
                        termVarious();
                        ShutDown();
                    }
                }
                catch
                {
                    bRes = false;
                }
            }

            //終了処理
            if (bRes == true)
            {
                try
                {
                    //v1330 追加
                    SystemCounter.GetInstance().Save(AppData.EXE_FOLDER + AppData.SYSTEM_COUNTER);

                    //v1329 terminate時にセーブしないように変更。代わりにここでセーブする。
                    SystemParam.GetInstance().SystemSave();

                    if (bPowerOffButton == true)//v1338 PC電源ボタン押下で入る
                    {
                        updateControls(false);
                        termVarious();
                    }
                    else
                    {
                        if (SystemStatus.GetInstance().RestoreShutdown == false)
                        {
                            string sMessage = "システムを終了しますか？";

                            if (true == SystemParam.GetInstance().GCustomEnable)
                            {
                                if (clsSignalControl.GetInstance().GetPatLiteTimerNum() > 0)
                                {
                                    sMessage = "システムを終了しますか？\nパトライトタイマーは解除されます。";
                                }
                            }

                            if (DialogResult.Yes == Utility.ShowMessage(this, sMessage, MessageType.YesNo))
                            {
                                LogingDll.Loging_SetLogString("[システムを終了しますか？]ー[ Yes ]ボタンを押下した");
                                //各Termをまとめてるところ
                                updateControls(false);
                                termVarious();
                            }
                            else
                            {
                                LogingDll.Loging_SetLogString("[システムを終了しますか？]ー[ Cancel ]ボタンを押下した");
                                bRes = false;
                            }
                        }
                        else
                        {
                            LogingDll.Loging_SetLogString("[システム復旧]ー[ Yes ]ボタンを押下した");
                            updateControls(false);
                            termVarious();
                        }
                    }
                }
                catch
                {
                    bRes = false;
                }
            }
            _bRecursiveFormClosing = bRes;
            return bRes;
        }



        private void updateControls(bool bEnable)
        {
            btnMap.Enabled = bEnable;
            btnNg1Img.Enabled = bEnable;
            btnNgList.Enabled = bEnable;
            btnNgMiniImg.Enabled = bEnable;
            btnOldList.Enabled = bEnable;
            btnRecipe.Enabled = bEnable;
            btnTotal.Enabled = bEnable;
            btnNgReset.Enabled = bEnable;   //v1341
            btnSystem.Enabled = bEnable;

            btnImageMain.Enabled = bEnable;

            btnReset.Enabled = bEnable;
            btnStart.Enabled = bEnable;
            btnSuspend.Enabled = bEnable;
            btnEnd.Enabled = bEnable;

            btnStart.BackColor = (btnStart.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_BLUE) : SystemColors.Control;
            btnSuspend.BackColor = (btnSuspend.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_LIME) : SystemColors.Control;
            btnEnd.BackColor = (btnEnd.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_YELLOW) : SystemColors.Control;

            foreach (UserControl ucl in _lstChildWnd)
            {
                ucl.Enabled = bEnable;
            }

            if (bEnable)
            {
                ChangeButtonStatus();
            }
        }

        public void ChangeStartEndButtonEnable(bool bEnable)
        {
            btnSuspend.Enabled = bEnable;
            btnEnd.Enabled = bEnable;
            btnStart.Enabled = bEnable;
            btnReset.Enabled = bEnable;

            //btnStart.BackColor = (btnStart.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_BLUE) : SystemColors.Control;
            //btnSuspend.BackColor = (btnSuspend.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_LIME) : SystemColors.Control;
            //btnEnd.BackColor = (btnEnd.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_YELLOW) : SystemColors.Control;

            if (bEnable)
            {
                this.ChangeButtonStatus();
            }
        }

        private void textLotNo_MouseClick(object sender, MouseEventArgs e)
        {
            /* 
                    if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop && SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
                    {

                        if (textLotNo.ReadOnly == true)
                        {
                            textLotNo.ReadOnly = false;
                            this.LotNo = textLotNo.Text;
                        }
                        else if (textLotNo.ReadOnly == false)
                        {
                            textLotNo.ReadOnly = true;
                        }
                        else
                        {
                            textLotNo.ReadOnly = true;
                        }
                    }
                    else
                    {
                        textLotNo.ReadOnly = true;
                    }
              */
        }

        //LostFocusイベントハンドラ
        private void textLotNo_LostFocus(object sender, EventArgs e)
        {
            //LotNo = textLotNo.Text;
            // textLotNo.ReadOnly = true; 
        }

        public void SetPageSumNailImage(List<ResActionData> resAcData, int NowPage)
        {
            for (int i = 0; 6 > i; i++)
            {
                HObject dmy1 = null, dmy2 = null;
                HObject hoImg = null;
                HTuple htNum;
                resAcData[resAcData.Count - 1 - i - (NowPage - 1) * 6].GetImage(ref dmy1, ref dmy2);
                HOperatorSet.CountObj(dmy1, out htNum);
                if (htNum.I > 0)
                    HOperatorSet.SelectObj(dmy1, out hoImg, 1);
                UclNgThumbnailReal.SetImage(i, hoImg);
                if (hoImg != null)
                {
                    hoImg.Dispose();
                }
            }
        }

        //各term処理のまとめ
        private void termVarious()
        {
            //エラーモニタの終了
            termErrorMonitor();

            //リセットボタン監視の終了           
            termDioCommandMonitor();

            //自動検査の終了処理
            termInspection();

            // 測長監視の終了
            termLengthMeasMonitor();

#if !SPEED_MONITOR_NEW
    //スピードモニタの終了処理
    TermSpeedMonitor();
#else
            //スピードモニタ(マイコンの終了処理)
            termSpeedMonitorNew();
#endif
            //upsの終了処理
            termUpsMoni();

            TerminateSerialSpeedControll();
        }

        public void initLengthMeasMonitor()
        {
            if (SystemParam.GetInstance().EnableLengthMeasMonitor)
            {
                clsLengthMeasMonitor.getInstance().Initialize();
                clsLengthMeasMonitor.getInstance().LengthMeasMonitorSec = SystemParam.GetInstance().LengthMeasMonitorLimitSec;
                clsLengthMeasMonitor.getInstance().LengthMeasError += MainForm_LengthMeasError;
            }
        }


        void MainForm_LengthMeasError(object sender, EventArgs e)
        {
            if (clsSignalControl.GetInstance().IsInspectStop == true)
                return;

            if (this.InvokeRequired == true)
            {
                this.Invoke(new Action(() =>
                {
                    this.MainForm_LengthMeasError(sender, e);
                }));
                return;
            }

            clsSignalControl.GetInstance().SetLengthMeasError();
            _frmLengthMeasDialog.Show();
            clsLengthMeasMonitor.getInstance().SetIdle(true);
        }

        private void termLengthMeasMonitor()
        {
            if (SystemParam.GetInstance().EnableLengthMeasMonitor)
            {
                clsLengthMeasMonitor.getInstance().SetAutoInspection(null);
                clsLengthMeasMonitor.getInstance().LengthMeasError -= MainForm_LengthMeasError;

            }
        }

        //UPSイベント登録
        private void InitUpsMoni()
        {
            SystemContext.GetInstance().UpsMonitor.UpsShutdown += this.OnReceiveUpsShutdown;
        }
        //UPSのイベント(clsUpsdMonitor -->  MainForm)
        private void OnReceiveUpsShutdown(object sender, UpsShutdownEventArg e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                    {
                        OnReceiveUpsShutdown(sender, e);
                    }));
                return;
            }

            LogingDll.Loging_SetLogString("UPSｼｬﾄﾀﾞｳﾝｲﾍﾞﾝﾄを検知しました");
            SystemStatus.GetInstance().UpsShutDown = true;
            //シグナルタワー赤とブザー発生
            //            clsSignalControl.GetInstance().SetInspectStatus(ESignalStatus.Shutdown);
            clsSignalControl.GetInstance().SetError();

            //  Utility.ShowMessage(this, "UPSから停電通知を受信しました。秒後にシャットダウンします。",MessageType.Warning);
            Utility.ShowUpsShutdownMessage(this);

            //シャットダウン処理
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                //ボタンを押せなくする
                //  btnEnd.Enabled = false;
                this.ChangeStartEndButtonEnable(false);

                //検査終了
                _autoInsp.Stop();
            }
            else
            {
                this.Close();
            }
        }
        //UPSイベント解除
        private void termUpsMoni()
        {
            SystemContext.GetInstance().UpsMonitor.UpsShutdown -= this.OnReceiveUpsShutdown;
        }

        //LED点灯時間のイベント登録
        private void InitLightMeasPeriodMes()
        {
            SystemContext sysCont = SystemContext.GetInstance();
            for (int i = 0; i < sysCont.LightMeasPeriod.Length; i++)
                sysCont.LightMeasPeriod[i].ChangeTime += this.OnReceiveChangeTime;
        }

        //LED点灯時間のイベント(clsMeasPeriod[0] -->  MainForm)
        private void OnReceiveChangeTime(object sender, ChangeTimeHandlerEventArgs e)
        {
            this.ChangeTimeAll();
        }

        //LED点灯時間のイベント解除
        private void TermLightMeasPeriodMes()
        {
            SystemContext sysCont = SystemContext.GetInstance();
            for (int i = 0; i < sysCont.LightMeasPeriod.Length; i++)
                sysCont.LightMeasPeriod[i].ChangeTime -= this.OnReceiveChangeTime;
        }

        //シートマップの幅とゾーンのセット
        public void SetMapParameter(double[] dZone)
        {
            UclSheetMapReal.SheetWidth = Recipe.GetInstance().InspParam[0].Width;
            UclSheetMapReal.Zones = dZone;
        }
        //シートマップの幅とゾーンのセット
        public void SetMapParameterOld(double[] dZone, double dWidth)
        {
            UclSheetMapOld.SheetWidth = dWidth;
            UclSheetMapOld.Zones = dZone;
        }
        //レシピの表示Real用
        public void SetRecipeDispReal()
        {
            UclRecipeContentsReal.SetRecipeDisp();

            if (Recipe.GetInstance().KindName == "" || Recipe.GetInstance().KindName == "未登録")
            {
                this.ChangeStartEndButtonEnable(false);
            }
            else
            {
                this.ChangeStartEndButtonEnable(true);
            }



            //UclRecipeContentsReal.RecipeDisp();

            //if (Recipe.GetInstance().KindName == "未登録")
            //{
            //    UclRecipeContentsReal.NotRegistaredEnable(true);
            //}
            //else
            //{
            //    UclRecipeContentsReal.NotRegistaredEnable(false);
            //    UclRecipeContentsReal.ChangeEnabed();
            //    UclRecipeContentsReal.ChangecmbInspType();
            //}
        }

        public void SetRecipeNameDispReal()
        {
            UclRecipeContentsReal.textKindName.Text = Recipe.GetInstance().KindName;
        }


        clsErrorMonitor _errorMonitor = null;
        private bool initErrorMonitor()
        {
            if (_errorMonitor != null)
                return false;

            _errorMonitor = new clsErrorMonitor();
            _errorMonitor.Initialize();

            // カメラ
            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                _errorMonitor.Add(APCameraManager.getInstance().GetCamera(i));
            }

            // DIO
            _errorMonitor.Add(CommunicationManager.getInstance().getCommunicationDIO());
#if SPEED_MONITOR_NEW
            // SPEED MONITOR
            _errorMonitor.Add(_speedMonitorNew);
#endif

            // 照明
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                _errorMonitor.Add(LightControlManager.getInstance().GetLight(i));
            }

            //AutoInspection
            _errorMonitor.Add(_autoInsp);

            //Eventmonitor
            _errorMonitor.Add(_autoInsp.EventMonitor);

            _errorMonitor.OnErrorAll += _errorMonitor_OnErrorAll;
            _errorMonitor.Start();

            return true;
        }

        void _errorMonitor_OnErrorAll(object sender, ErrorOccuredAllEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                  {
                      _errorMonitor_OnErrorAll(sender, e);
                  }));
                return;
            }

            updateStatusBar();

            int iError = 0;
            for (int i = 0; e.Error.Length > i; i++)
            {
                if (e.Error[i])
                {
                    iError++;
                }

            }

            if (iError == 0)
            {
                return;
            }

            string stError = "";

            int iAddCount = APCameraManager.getInstance().CameraNum;

            for (int i = 0; e.Error.Length > i; i++)
            {
                //e.Error[i]はinitErrorMonitorでのAddの順番
                if (e.Error[i])
                {
                    if (i < iAddCount)
                    {
                        stError = "ｶﾒﾗ:" + e.Reasons[i];
                    }
                    else if (i == iAddCount)
                    {
                        stError = "ﾃﾞｼﾞﾀﾙIO:" + e.Reasons[i];
                    }
#if SPEED_MONITOR_NEW
                    else if (i == iAddCount + 1)
                    {
                        stError = "ｽﾋﾟｰﾄﾞ監視:" + e.Reasons[i];
                    }
#endif
                    else if (i > iAddCount + 1 && i < iAddCount + 1 + LightControlManager.getInstance().LightCount)
                    {
                        stError = "照明:" + e.Reasons[i];
                    }
                    else if (i == iAddCount + 1 + LightControlManager.getInstance().LightCount)
                    {
                        stError = "自動検査:" + e.Reasons[i];
                    }
                    else if (i == iAddCount + 1 + LightControlManager.getInstance().LightCount + 1)
                    {
                        stError = "ｲﾍﾞﾝﾄ監視:" + e.Reasons[i];
                    }
                    _frmSysErrorDialog._stError = stError;
                }

                LogingDll.Loging_SetLogString(stError);
                _frmSysErrorDialog._stError = stError;
                //シグナルタワー赤とブザー発生
                clsSignalControl.GetInstance().SetError();
                _frmSysErrorDialog.Show();
                // ブザー停止
                //                clsSignalControl.GetInstance().ResetError();
            }
        }

        private void termErrorMonitor()
        {
            if (_errorMonitor == null)
                return;
            _errorMonitor.OnErrorAll -= _errorMonitor_OnErrorAll;
            _errorMonitor.Stop();
        }

        //過去データの削除
        private void DleteProductData()
        {
            string[] dirs = Directory.GetDirectories(SystemParam.GetInstance().ProductFolder);


            int i = SystemParam.GetInstance().DeleteMonth;
        }

        //過去データの削除
        private void DeleteProductFolder()
        {
            int month = SystemParam.GetInstance().DeleteMonth;
            if (month > 0)
            {
                string[] dirs = Directory.GetDirectories(SystemParam.GetInstance().ProductFolder);

                for (int j = 0; dirs.Length > j; j++)
                {
                    int i = dirs[j].LastIndexOf("\\");
                    string ss = dirs[j].Substring(i + 1);
                    ss = ss.Insert(4, "/");

                    DateTime dt = DateTime.Parse(ss);
                    DateTime today = DateTime.Today;

                    TimeSpan ts = today - dt;
                    if (ts.Days > (month + 1) * 31)
                    {
                        //指定月以上
                        Directory.Delete(dirs[j], true);
                    }
                }
            }
        }

        private void textLotNo_TextChanged(object sender, EventArgs e)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            char[] chU = new char[] { '_' };
            char[] invalidCharsU = invalidChars.Concat(chU).ToArray();
            //ファイル名に使えるかチェック
            if (textLotNo.Text.IndexOfAny(invalidCharsU) < 0)
            {

            }
            else
            {
                textKindName.Focus();
                Utility.ShowMessage(this, @"使用できない文字が含まれています　\/:*?<>|_", MessageType.Error);
                bool bb = textLotNo.Focused;
                int index = textLotNo.Text.IndexOfAny(invalidCharsU);
                //   textLotNo.Text = textLotNo.Text.Remove(textLotNo.Text.Length - 1, 1);
                textLotNo.Text = textLotNo.Text.Remove(index, 1);
                textLotNo.Select(textLotNo.Text.Length, 0);
                textLotNo.Focus();

            }

            if (textLotNo.Text.Length > 20)
            {
                //    Utility.ShowMessage(this, "LotNoは20文字までです。", MessageType.Error);

                textLotNo.Text = textLotNo.Text.Remove(20);
                textLotNo.Select(textLotNo.Text.Length, 0);
            }

        }

        private void SaveRecipe()
        {
            UclRecipeContentsReal.SaveRecipe();
        }

        public void ChangeRecipeMessage()
        {
            UclRecipeContentsReal._clsCheckRecipeEdit.CheckEdit();

            if (SystemStatus.GetInstance().RecipeEdit == true)
            {
                //if (Recipe.GetInstance().KindName == "未登録" || Recipe.GetInstance().KindName == "Default")
                if (Recipe.GetInstance().KindName == "未登録")
                {

                }
                else
                {
                    if (DialogResult.Yes == Utility.ShowMessage(this, "品種の変更がありました。\r\n保存しますか？", MessageType.YesNo))
                    {
                        this.SaveRecipe();

                    }
                    else
                    {
                        UclRecipeContentsReal.SetRecipeDisp();
                    }
                }
            }

            SystemStatus.GetInstance().RecipeEdit = false;
            this.ClearRecipeEdit();
        }

        private void ShutDown()
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();

            psi.FileName = "shutdown.exe";

            //コマンドラインパラメータ　-s:シャットダウン　-f:強制的に
            psi.Arguments = "-s";
            //psi.Arguments = "-s -f";
            psi.CreateNoWindow = true;
            //シャットダウン
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
        }

        void OnNgDialog_Closed(object sender, EventArgs e)
        {
            //シグナルタワーリセット
            clsSignalControl.GetInstance().ResetNG();

            _instance = null;
        }
        void OnNg1ImageDialog_Closed(object sender, EventArgs e)
        {
            clsSignalControl.GetInstance().ResetNG();
            _instanceNg1Image = null;
        }
        void OnExternalOutputCancelDialog_Closed(object sender, EventArgs e)
        {
            clsSignalControl.GetInstance().ResetExternalOutput((_instanceExtOut1Cancel.DialogResult == DialogResult.Cancel) ? true : false);
            _instanceExtOut1Cancel = null;
        }

        void OnSysErrorDialog_Closed(object sender, EventArgs e)
        {
            //シグナルタワーリセット
            clsSignalControl.GetInstance().ResetError();
            _instanceSys = null;
        }

        void OnLengthMeasDialog_Closed(object sender, EventArgs e)
        {
            // シグナルタワーのリセット
            clsSignalControl.GetInstance().ResetLengthMeasError();
            // インスタンスをNULLに設定する
            _instanceLengthMeas = null;
            clsLengthMeasMonitor.getInstance().SetIdle(false);
        }

        void OnNoInspDialog_Closed(object sender, EventArgs e)
        {
            //シグナルタワーのリセット
            clsSignalControl.GetInstance().ResetInspectNotStart();
            _instanceNoInsp = null;
        }

        //マップ　Realのダブルクリックイベント
        private void UclSheetMapReal_TipDoubleClicked(object sender, TipClickedEventArgs e)
        {
            FormNg1ImageDisp((int)e.SheetTipItems[0].User);

        }
        //マップ　Oldのダブルクリックイベント
        private void UclSheetMapOld_TipDoubleClicked(object sender, TipClickedEventArgs e)
        {
            FormNg1ImageDisp((int)e.SheetTipItems[0].User);
        }
        //サムネイル　Oldのダブルクリックイベント
        void UclNgThumbnailOld_OnThumbnailDoubleClick(object sender, uclMiniImage.DoubleClickEventArgs e)
        {
            FormNg1ImageDisp(e.LineIndex);
        }
        //サムネイル　Realのダブルクリックイベント
        void UclNgThumbnailReal_OnThumbnailDoubleClick(object sender, uclMiniImage.DoubleClickEventArgs e)
        {
            FormNg1ImageDisp(e.LineIndex);
        }

        private frmNg1Image _frmNg1Img;//v1338 yuasa ShowDialogを外部から落とせるようにここに移動。

        private void FormNg1ImageDisp(int Line)
        {
            double sheetWidth;
            double[] sheetZones;

            List<ResActionData> allDatas;
            List<ResActionData> resacdata;
            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                allDatas = this._realtimeResultActionDataClass.GetNgDatas();
                resacdata = this._realtimeResultActionDataClass.GetItemDataIndex(Line);
                sheetWidth = UclSheetMapReal.SheetWidth;
                sheetZones = UclSheetMapReal.Zones;
            }
            else
            {
                allDatas = this._oldResultActionDataCls.GetNgDatas();
                resacdata = this._oldResultActionDataCls.GetItemDataIndex(Line);
                sheetWidth = UclSheetMapOld.SheetWidth;
                sheetZones = UclSheetMapOld.Zones;
            }
            if (resacdata.Count == 1)
            {
                using (_frmNg1Img = new frmNg1Image())//v1338 yuasa
                {
                    _frmNg1Img.SetNgListDatas(allDatas);
                    _frmNg1Img.SetNgData(resacdata[0], sheetWidth, sheetZones);
                    _frmNg1Img.ShowDialog();
                    _frmNg1Img = null;
                }

                //_frmNg1Img = new frmNg1Image();
                //_frmNg1Img.SetNgListDatas(allDatas);
                //_frmNg1Img.SetNgData(resacdata[0], sheetWidth, sheetZones);
                //_frmNg1Img.ShowDialog();
            }
        }

        private frmNg1Image _frmNg1Img2;//v1338 yuasa ShowDialogを外部から落とせるようにここに移動。

        private void FormNG1ImageDisp(ResActionData data)
        {
            if (data == null)
                return;

            List<ResActionData> allDatas;
            double sheetWidth;
            double[] sheetZones;
            if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Real)
            {
                allDatas = this._realtimeResultActionDataClass.GetNgDatas();
                sheetWidth = UclSheetMapReal.SheetWidth;
                sheetZones = UclSheetMapReal.Zones;
            }
            else
            {
                allDatas = this._oldResultActionDataCls.GetNgDatas();
                sheetWidth = UclSheetMapOld.SheetWidth;
                sheetZones = UclSheetMapOld.Zones;
            }

            using (_frmNg1Img2 = new frmNg1Image())//v1338 yuasa
            {
                _frmNg1Img2.SetNgListDatas(allDatas);
                _frmNg1Img2.SetNgData(data, sheetWidth, sheetZones);
                _frmNg1Img2.ShowDialog();
                _frmNg1Img2 = null;
            }

            //_frmNg1Img2 = new frmNg1Image();
            //_frmNg1Img2.SetNgListDatas(allDatas);
            //_frmNg1Img2.SetNgData(data, sheetWidth, sheetZones);
            //_frmNg1Img2.ShowDialog();
        }

        //スクロールロックのイベント　Real
        void UclNgListReal_chkboxScrolRock_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;

            UclSheetMapReal.LockPosition = chkbox.Checked;
            UclNgThumbnailReal.LockAutoChangePage = chkbox.Checked;

            UclNgThumbnailReal.ButtonEnable(chkbox.Checked);


        }
        //スクロールロックのイベント　Old
        void UclNgListOld_chkboxScrolRock_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;

            UclSheetMapOld.LockPosition = chkbox.Checked;
            UclNgThumbnailOld.LockAutoChangePage = chkbox.Checked;
        }
        //NGリストのクリックイベント　Old
        void UclNgListOld_listViewNGItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listOld = (ListView)sender;
            //   List<ResActionData> resacdata;

            if (UclSheetMapOld.LockPosition)
            {
                /*
                                if (listOld.SelectedItems.Count > 0)
                                {
                                    //マップ
                                    resacdata = this._oldResultActionDataCls.GetItemDataIndex(Convert.ToInt32(listOld.SelectedItems[0].SubItems[0].Text));
                                    UclSheetMapOld.DispMeter(resacdata[0].PositionY / 1000);

                                    //サムネイル
                                }
                 */
                if (listOld.SelectedIndices.Count > 0)
                {
                    //マップ
                    UclSheetMapOld.DispMeter(_oldResActionDataVirtual[listOld.SelectedIndices[0]].PositionY / 1000.0);

                    //サムネイル
                    if (_oldResActionDataVirtual[listOld.SelectedIndices[0]].EventId == ResultActionDataClass.EEventId.Result)
                    {
                        for (int i = 0; _oldResActionDataThumbnail.Count > i; i++)
                        {
                            ResActionData oldResAcData = _oldResActionDataVirtual[listOld.SelectedIndices[0]];
                            if (_oldResActionDataThumbnail[i].ResultNo == oldResAcData.ResultNo)
                            {
                                ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, i / 6 + 1, i % 6);
                                //    ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, i / 6 + 1, (oldResAcData.ResultNo - 1) % 6);
                                //                                ChangePage(i / 6+1, _oldResActionDataThumbnail.Count % 6);

                                break;
                            }
                        }
                    }
                    else
                    {
                        UclNgThumbnailOld.ClearColorSelsectItemAll();
                    }

                }
                UclSheetMapOld.Repaint();
            }
        }
        //NGリストのクリックイベント　Real
        void UclNgListReal_listViewNGItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listReal = (ListView)sender;

            if (UclSheetMapReal.LockPosition)
            {
                if (listReal.SelectedIndices.Count > 0)
                {
                    int listselno = listReal.SelectedIndices[0];
                    int resno = _realResActionData[listselno].ResultNo;

                    if (resno != -1)
                    {
                        Predicate<ResActionData> jdg = new Predicate<ResActionData>(x => x.ResultNo == resno);
                        int listresselno = _realResActionDataThumbnail.FindIndex(jdg);
                        ResActionData resData = _realResActionDataThumbnail.Find(jdg);

                        if (resData != null)
                        {
                            //マップ
                            UclSheetMapReal.DispMeter(resData.PositionY / 1000);

                            //サムネイル
                            int page = listresselno / 6 + 1; //0～
                            ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, page, listresselno % 6);
                            // ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, page, (resno - 1) % 6);
                            //                        this.ChangePage(page , _realResActionDataThumbnail.Count % 6);
                            //                        UclNgThumbnailReal.SetNowPage(page + 1);
                        }
                    }
                    else
                    {
                        UclNgThumbnailReal.ClearColorSelsectItemAll();
                    }

                }

                UclSheetMapReal.Repaint();
            }
        }

        Color STATUS_COLOR_NORMAL = Color.Green;
        Color STATUS_COLOR_ERROR = Color.Red;
        Color STATUS_COLOR_DISABLE = SystemColors.ControlDarkDark;
        void updateStatusBar()
        {
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!_clsShortcutman.IgnoreCtrlFocus)
            {
                if (this.shortcutKeyHelper1.PerformClickByKeys(keyData))
                {
                    return true;
                }

                if (_clsShortcutman.DoProcessCmdKey(keyData))
                {
                    return true;
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tmrUpsShutdown_Tick(object sender, EventArgs e)
        {
            tmrUpsShutdown.Enabled = false;
            Close();
        }

        public string GetSelectItem()
        {
            return UclRecipeList.selectItem;
        }

        public void ChangeListName(string selitem, string name)
        {
            UclRecipeList.ChangeListItemName(selitem, name);
            UclRecipeList.SelsectItem(selitem);
        }

        public void RecipeContentsEnabled()
        {
            UclRecipeContentsReal.Enabled = true;
        }

        public int CheckNeme(string name)
        {
            int i;
            i = UclRecipeList.CheckCopyName(name);
            return i;
        }

        public void ChangeTimeAll()
        {
            //透過および反射　照明リミット未設定
            if (SystemParam.GetInstance().LightWarningTime == 0)
            {
                btnLightTime.BackColor = SystemColors.Control;
                return;
            }

            bool over = false;

            SystemContext sysCont = SystemContext.GetInstance();
            SystemParam sysParam = SystemParam.GetInstance();
            for (int i = 0; i < sysCont.LightMeasPeriod.Length; i++)
            {
                if (sysCont.LightMeasPeriod[i].AccumulateHour >= sysParam.LightWarningTime)
                {
                    over = true;
                    break;
                }
            }
            //どれか１つでもリミットオーバー時は、警告色にする
            btnLightTime.BackColor = (over == true) ? Color.Red : SystemColors.Control;
        }

        private void ClearSheetMap()
        {
            //マップの再描画
            //    UclSheetMapReal.Refresh();
            //シートのゾーン数を0にする
            UclSheetMapReal.Zones = new double[] { 0.0 };

            //シートの現在位置を0にする
            UclSheetMapReal.CurrentPosMeter = 0;
            //シートの幅を0にする
            UclSheetMapReal.SheetWidth = 0;
            //シート長を0にする。
            UclSheetMapReal.SheetLengthMeter = 0;
        }

        public string SelectRecipeListItem()
        {
            return UclRecipeList.SelsectItemString();
        }

        void UclNgThumbnailOld_btnLast_Click(object sender, EventArgs e)
        {

            ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, -1, -1);
        }

        void UclNgThumbnailOld_btnFirst_Click(object sender, EventArgs e)
        {

            ChangePage(UclNgThumbnailOld, _oldResActionDataThumbnail, 1, -1);
        }

        void UclNgThumbnailReal_btnLast_Click(object sender, EventArgs e)
        {
            ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, -1, -1);
        }

        void UclNgThumbnailReal_btnFirst_Click(object sender, EventArgs e)
        {

            ChangePage(UclNgThumbnailReal, _realResActionDataThumbnail, 1, -1);
        }

        void UclNgThumbnailReal_uclMiniImage6_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(5))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailReal_uclMiniImage5_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(4))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailReal_uclMiniImage4_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(3))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailReal_uclMiniImage3_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(2))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailReal_uclMiniImage2_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(1))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailReal_uclMiniImage1_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailReal.CheckColorSelectItem(0))
            {
                UclNgThumbnailReal.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage6_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(5))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage5_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(4))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage4_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(3))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage3_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(2))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage2_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(1))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        void UclNgThumbnailOld_uclMiniImage1_OnThumbnailClick(object sender, uclMiniImage.ClickEventArgs e)
        {
            if (!UclNgThumbnailOld.CheckColorSelectItem(0))
            {
                UclNgThumbnailOld.ClearColorSelsectItemAll();
            }
        }

        private void InitDioCommandMonitor()
        {
            IniFileAccess ini = new IniFileAccess();
            int iResetNg, iResetError;
            int dioInspectionSignal; //V1057 手動外部修正 yuasa 20190115：検査開始終了DIO追加
            string sPath = AppData.EXE_FOLDER + AppData.SYSTEM_FILE;

            iResetNg = SystemParam.GetInstance().InNgReset;     //V1293 moteki
            iResetError = SystemParam.GetInstance().InErorrReset;   //V1293 moteki
            dioInspectionSignal = SystemParam.GetInstance().InInspectionStart;  //V1293 moteki

            //iResetNg = ini.GetIniInt("ResetButton_DioAssign", "NgReset", sPath, 3);
            //iResetError = ini.GetIniInt("ResetButton_DioAssign", "ErrorReset", sPath, 4);
            //dioInspectionSignal = ini.GetIniInt("Dio_Inspection_Signal", "DioNum", sPath, 5); //V1057 手動外部修正 yuasa 20190115：検査開始終了DIO追加           

            //ini.SetIni("ResetButton_DioAssign", "NgReset", iResetNg, sPath);
            //ini.SetIni("ResetButton_DioAssign", "ErrorReset", iResetError, sPath);
            //ini.SetIni("Dio_Inspection_Signal", "DioNum", dioInspectionSignal, sPath);

            string sDeviceName = ini.GetIni("LiveTrigger", "CommunicationDioName", "", sPath);
            CommunicationDIO dio = CommunicationManager.getInstance().getCommunicationDIO(sDeviceName);

            _clsDioCmdMon.Initialize(dio);

            //v1326 条件によってコマンド追加を変更
            if(true == SystemParam.GetInstance().OutsideResetButtonEnable)
            {
                _clsDioCmdMon.AddCommand(iResetNg, OnDioCommandEventHandler, 0);
                _clsDioCmdMon.AddCommand(iResetError, OnDioCommandEventHandler, 1);
            }
            if (true == SystemParam.GetInstance().OutsideManualExtButtonEnable)
            {
                _clsDioCmdMon.AddCommand(dioInspectionSignal, OnDioCommandEventHandler, 2, clsDioCommandMonitor.EPublishEventMode.Inspect, clsDioCommandMonitor.EPublishEventType.Negative); //V1057 手動外部修正 yuasa 20190115：検査開始終了DIO追加
            }
            if(true == SystemParam.GetInstance().GCustomEnable)
            {
                //v1326
                //フリッパ監視CH Alternate:の場合、パルス：Whileの場合レベル（ただし、コマンドがどんどんたまっていくので、_iCycleTimeを調整したほうがいいかも）
                //立ち上がりエッジを見ればいいのでAlternate固定で良い
                _clsDioCmdMon.AddCommand(SystemParam.GetInstance().InFlipperoOserveCH, OnDioCommandEventHandler, 3, clsDioCommandMonitor.EPublishEventMode.Alternate, clsDioCommandMonitor.EPublishEventType.Active);
                //パトライト停止スイッチ
                _clsDioCmdMon.AddCommand(SystemParam.GetInstance().InPatLiteStopSWCH, OnDioCommandEventHandler, 4, clsDioCommandMonitor.EPublishEventMode.Alternate, clsDioCommandMonitor.EPublishEventType.Active);
            }
            if (true == SystemParam.GetInstance().PowerOffButtonEnable)// v1338 yuasa：Enableにならないとメソッド追加しないので実行されない。
            {
                clsDioCommandMonitor.EPublishEventType HithLow = SystemParam.GetInstance().PowerOffButtonHighLow ? clsDioCommandMonitor.EPublishEventType.Active : clsDioCommandMonitor.EPublishEventType.Negative;
                //コマンド（画面側で動かすメソッドの登録含む）を追加。
                _clsDioCmdMon.AddCommand(SystemParam.GetInstance().PowerOffButtonDioIndex, OnDioCommandEventHandlerPowerOffButton,
                    null, clsDioCommandMonitor.EPublishEventMode.PowerOffButton, HithLow); 
            }
            _clsDioCmdMon.Start();
        }

        private void termDioCommandMonitor()
        {
            _clsDioCmdMon.Stop();
        }

        private void OnDioCommandEventHandler(object sender, clsDioCommandMonitor.DioCommandEventArgs e)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new Action(() =>
                {
                    this.OnDioCommandEventHandler(sender, e);
                }));
                return;
            }

            if ((int)e.Args == 0)
            {
                if (_frmNgdialog.Visible == true)
                {
                    _frmNgdialog.Close();
                }
                else if (_frmNg1ImageDialog.Visible == true)
                {
                    _frmNg1ImageDialog.Close();
                }
                else if (_frmNoinspDialog.Visible == true)
                {
                    _frmNoinspDialog.Close();
                }
                else if (_frmLengthMeasDialog.Visible == true)
                {
                    _frmLengthMeasDialog.Close();
                }
            }
            else if ((int)e.Args == 1) //V1057 手動外部修正 yuasa 20190115：検査開始終了DIO追加のため条件分岐追加
            {
                if (_frmSysErrorDialog.Visible == true)
                {
                    _frmSysErrorDialog.Close();
                }

            }
            else if ((int)e.Args == 2) //V1057 手動外部修正 yuasa 20190115：検査開始終了DIO追加のため条件分岐追加
            {
                if ((int)e.PublishEventType == 0x2) // 0x2：Negative
                {
                    inspect_start(true);
                }
                else
                {
                    inspect_stop(true);
                }
            }
            else if ((int)e.Args == 3) //v1326 フリッパ監視CH
            {
                if (true == SystemParam.GetInstance().GCustomEnable)
                    clsSignalControl.GetInstance().AddFlipperNGCmd();
            }
            else if ((int)e.Args == 4) //v1326 パトライト停止スイッチ
            {
                if (true == SystemParam.GetInstance().GCustomEnable)
                    clsSignalControl.GetInstance().ResetPatLite();
            }
        }

        //v1338 yuasa
        /// <summary>電源オフボタンを長押しした際に呼ばれるメソッド。検査中なら検査を抜けて、「システム終了」ボタン押下と同じ動作をする。</summary>
        private void OnDioCommandEventHandlerPowerOffButton(object sender, clsDioCommandMonitor.DioCommandEventArgs e)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new Action(() =>
                {
                    this.OnDioCommandEventHandlerPowerOffButton(sender, e);
                }));
                return;
            }

            //ShowDialogを閉じる
            if (_frmNg1Img != null)
                _frmNg1Img.frmNg1ImageClose();
            if (_frmNg1Img2 != null)
                _frmNg1Img2.frmNg1ImageClose();

            //検査中なら検査を抜ける。
            if (LineCameraSheetSystem.SystemStatus.GetInstance().NowState == LineCameraSheetSystem.SystemStatus.State.Inspection)
            {
                //検査を抜ける。
                inspect_stop(true);
                //検査を抜けて検査終了イベント（OnReceiveUpdateAction）で「EResultActionId.Stop」を受け取るまで待つ。
            }
            else
            {
                //検査中でなければ、そのままカスタムシステム終了。
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    CustomFormClose();
                }));
                thread.Name = "PowerOffButton";
                thread.Start();
            }
        }

        /// <summary>カスタムシステム終了</summary>//v1338 yuasa
        private void CustomFormClose()
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new Action(() =>
                {
                    this.CustomFormClose();
                }));
                return;
            }
            MainFormCloseProcess(true);
            this.Close();
        }

        public void ClearRecipeEdit()
        {
            UclRecipeContentsReal._clsCheckRecipeEdit.AllFalse();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {


        }


        private void CheckTextWide(TextBox textbox, string name)
        {
            textbox.Font = new Font("ＭＳ UI Gothic", 14);

            Graphics g = label1.CreateGraphics();
            //SizeF size = g.MeasureString(name, textbox.Font, textbox.Width);
            SizeF size = g.MeasureString(name, textbox.Font);
            if (textbox.Width < size.Width)
            {
                float ff = size.Width / textbox.Width;

                textbox.Font = new Font("ＭＳ UI Gothic", textbox.Font.Size / ff);
                //     size = g.MeasureString(name, textbox.Font);
                // textbox.Width  = (int)Math.Ceiling(size.Width) ;
            }
            else
            {
                textbox.Font = new Font("ＭＳ UI Gothic", 14);
            }
            g.Dispose();
        }

        private void UclSystem_Load(object sender, EventArgs e)
        {

        }

        public void SetNoInspMoni()
        {
        }

        public bool GetAjustButtonVisible()
        {
            return UclSystem.btnAjustment.Visible;
        }

        private void ChangeButtonStatus()
        {
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection)
            {
                btnSuspend.Enabled = true;
                btnEnd.Enabled = true;
                btnStart.Enabled = false;
                btnReset.Enabled = true;
            }
            else if (SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
            {
                btnSuspend.Enabled = false;
                btnEnd.Enabled = true;
                btnStart.Enabled = true;
                btnReset.Enabled = false;

            }
            else if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
            {
                btnSuspend.Enabled = false;
                btnEnd.Enabled = false;
                btnStart.Enabled = true;
                btnReset.Enabled = false;

                ////初回調光ができていれば開始を表示
                //if (Recipe.GetInstance().AutoLight)
                //{
                //    btnStart.Enabled = true;
                //}
                //else
                //{
                //    btnStart.Enabled = false;
                //}
            }
            else
            {
                btnSuspend.Enabled = false;
                btnEnd.Enabled = false;
                btnStart.Enabled = false;
                btnReset.Enabled = false;

            }

            btnStart.BackColor = (btnStart.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_BLUE) : SystemColors.Control;
            btnSuspend.BackColor = (btnSuspend.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_LIME) : SystemColors.Control;
            btnEnd.BackColor = (btnEnd.Enabled == true) ? ColorTranslator.FromHtml(AppData.COLOR_YELLOW) : SystemColors.Control;
        }



        public bool IsControlReal(UserControl ucl)
        {
            if (ucl == UclNgListReal || ucl == UclNgThumbnailReal || ucl == UclRecipeList || ucl == UclSheetMapReal || ucl == UclRecipeContentsReal)
                return true;
            return false;
        }

        //public void SetRefreshImage()
        //{
        //    AutoInspection.SetRefreshImageEvent(this.OnReceiveRefreshImage);
        //}

        //public void ResetRefreshImage()
        //{
        //    // 自動調光中はステータスバーの更新は行わない
        //    AutoInspection.ClearRefreshImageEvent(this.OnReceiveRefreshImage);
        //}



        //カメラから露光値のロード
        private bool LoadExposureDefalt()
        {
            bool b = false;

            APCameraManager ApCam = APCameraManager.getInstance();
            SystemParam SysPara = SystemParam.GetInstance();

            bool bLoad = false;
            for (int i = 0; SysPara.ExposureDefault.GetLength(0) > i; i++)
            {
                //iniファイルの値が正常値か確認。
                if (SysPara.ExposureDefault[i, 0] >= 0 && SysPara.ExposureDefault[i, 0] <= 11)
                {
                    if (SysPara.ExposureDefault[i, 1] >= 61 && SysPara.ExposureDefault[i, 1] <= 1023)
                    {

                    }
                    else
                    {
                        bLoad = true;
                    }

                }
                else
                {
                    bLoad = true;
                }

            }

            //iniファイルの値が正常値ならロードしない。
            if (!bLoad)
            {
                return true;
            }

            //カメラ基準露光値の初期化[カメラ数 ,valの数 ]
            //   SysPara.ExposureDefault = new int[ApCam.CameraNum, 2];

            for (int i = 0; ApCam.CameraNum > i; i++)
            {
                HalconCameraLinX_NEDLineCameraXCM4040SAT2 cam = ApCam.GetCamera(i) as HalconCameraLinX_NEDLineCameraXCM4040SAT2;
                if (cam != null)
                {

                    int val1 = -1;
                    int val2 = -1;
                    b = cam.GetProgramableExposureTime(ref val1, ref val2);
                    if (b)
                    {
                        SysPara.ExposureDefault[i, 0] = val1;
                        SysPara.ExposureDefault[i, 1] = val2;

                    }
                    else
                    {
                        // Utility.ShowMessage(this, "GetProgramableExposureTime Cam" + (i + 1).ToString(), MessageType.Error);
                        LogingDll.Loging_SetLogString("GetProgramableExposureTime Cam" + (i + 1).ToString() + "  Error");

                        return b;
                    }
                }
            }

            if (b)
            {
                SysPara.SaveExposureDefault();
            }

            return b;
        }

        private int[] _iLightUpDown { get; set; }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //検査途中でのリセット
            if (DialogResult.Yes == Utility.ShowMessage(this,
                "検査をリセットしますか？\r\n(検査を続けたまま検査履歴を作成し,\r\n測長などをリセットします。)", MessageType.YesNo))
            {
                _autoInsp.Reset();
                this._realtimeResultActionDataClass.LastNgDataInit(); //V1057 NG表裏修正 yuasa 20190121：終了時前回Ngデータを初期化
            }
        }

        //時間補正警告ダイアログ
        private frmWarningDialog _instTimeWarning = null;
        public frmWarningDialog _frmTimeWarning
        {
            get
            {
                if (_instTimeWarning == null || _instTimeWarning.IsDisposed)
                {
                    _instTimeWarning = new frmWarningDialog();
                    _instTimeWarning.Closed += new EventHandler(OnTimeWarningDialog_Closed);
                    this.AddOwnedForm(_instTimeWarning);
                }
                return _instTimeWarning;
            }
        }
        void OnTimeWarningDialog_Closed(object sender, EventArgs e)
        {
            _instTimeWarning = null;
        }

        private string _dispTimeWarningDDHH;
        private void tmTimeWorningPopup_Tick(object sender, EventArgs e)
        {
            if (SystemParam.GetInstance().TimeWarningPopupHour < 0)
                return;

            string nowDDHH = DateTime.Now.ToString("ddhh", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string tarDDHH = string.Format("01{0}", SystemParam.GetInstance().TimeWarningPopupHour.ToString("D2"));
            if (nowDDHH == _dispTimeWarningDDHH || nowDDHH != tarDDHH)
                return;

            _dispTimeWarningDDHH = nowDDHH;
            _frmTimeWarning.SetText(null, "時刻を補正して下さい。");
            _frmTimeWarning.Show();
        }

        private bool _updSheetWidthFlag = false;
        private void chkSheetWidth_CheckedChanged(object sender, EventArgs e)
        {
            if (_updSheetWidthFlag == true)
                return;

            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
            }
            ChangeSheetWidthStatus();
        }

        private bool _updSheetThicknessFlag = false;
        private void chkSheetThickness_CheckedChanged(object sender, EventArgs e)
        {
            if (_updSheetThicknessFlag == true)
                return;

            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
            }
            ChangeSheetThicknessStatus();
        }

        private void btnLightTime_Click(object sender, EventArgs e)
        {
            frmLightTime frmLgt = new frmLightTime();
            frmLgt.ShowDialog(this);
        }

        private void btnApplicationClose_Click(object sender, EventArgs e)
        {
            LogingDll.Loging_SetLogString("[システム終了]ボタンを押下した");

            if (MainFormCloseProcess(false) == true)//v1338 元々FormClosingだったが、PC電源ボタン押下対応のため、別メソッドに変更
                this.Close();
        }

        #region 移動禁止・最小化禁止
        //移動禁止
        const int WM_SYSCOMMAND = 0x0112;
        const long SC_MOVE = 0xF010L;
        //
        const long SC_MINIMIZE = 0xF020;
        protected override void WndProc(ref Message m)
        {
            //移動禁止
            if (SystemParam.GetInstance().ForceCenteringMode
                && ((Control.ModifierKeys & Keys.Control) == 0))
            {

                if (!_moveOK)
                {
                    if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_MOVE)
                    {
                        m.Result = IntPtr.Zero;
                        return;
                    }
                }
            }

            if ((Control.ModifierKeys & Keys.Control) == 0)
            {
                if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_MINIMIZE)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private void _tmNotMinimied_Tick(object sender, EventArgs e)
        {
            _tmNotMinimied.Stop();
            _tmNotMinimied.Tick -= _tmNotMinimied_Tick;
            this.WindowState = FormWindowState.Normal;
        }

        private Timer _tmNotMinimied = new Timer();
        #endregion

        private bool _moveOK = false;
        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
            _moveOK = !_moveOK;
        }

        private void MainForm_Activated(object sender, EventArgs e)         // thông báo khởi động hoàn tất
        {
            SplashForm.ProgressSplash(100, "起動を完了しました");
            SplashForm.CloseSplash();
        }

        private void chkExtDinInsp_CheckedChanged(object sender, EventArgs e) //V1057 手動外部修正 yuasa 20190115：手動外部ボタン追加。
        {
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Inspection) //検査中は変更確認しない。手動モードと同じ動作とするため。
            {
                //レシピに変更があるか。
                ChangeRecipeMessage();
            }
            changeAutoManualStatus(chkExtDinInsp.Checked);
        }
        private void changeAutoManualStatus(bool ExtEnable = true) //V1057 手動外部修正 yuasa 20190115：手動外部ボタン追加。
        {
            if (ExtEnable)
            {
                chkExtDinInsp.Text = "外部";
                btnStart.Visible = false; //開始ボタン非表示
                btnSuspend.Visible = false; //中断ボタン非表示
                btnEnd.Visible = false; //終了ボタン非表示
                _clsDioCmdMon.extReciveEnableBtnValue = true; //手動外部ボタン：自動
                UclRecipeContentsReal.ExtDisplayModeChange(true); //uclRecipeContentsの外部ボタンプロパティを変更
            }
            else
            {
                chkExtDinInsp.Text = "手動";
                btnStart.Visible = true; //開始ボタン表示
                btnSuspend.Visible = SystemParam.GetInstance().SuspendButtonEnable; //中断ボタン表示（設定による）
                btnEnd.Visible = true; //終了ボタン表示
                _clsDioCmdMon.extReciveEnableBtnValue = false; //手動外部ボタン：手動
                UclRecipeContentsReal.ExtDisplayModeChange(false); //uclRecipeContentsの外部ボタンプロパティを変更
            }
        }

        public void SetRirekiCount()
        {
            clsRirekiCount.getInstance().Load(Path.GetDirectoryName(Recipe.GetInstance().Path));
            int[,] ZoneCnt = clsRirekiCount.getInstance().Zone;
            int[,] ItemsCnt = clsRirekiCount.getInstance().Items;
            int[] CamCnt = clsRirekiCount.getInstance().Cam;
            UclTotalReal.SetNgCount(CamCnt, ItemsCnt, ZoneCnt);

            //カメラ部位別
            foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
            {
                //ゾーン別
                foreach (AppData.ZoneID zone in Enum.GetValues(typeof(AppData.ZoneID)))
                {
                    _realtimeResultActionDataClass.CountNGZone[(int)side, (int)zone] = ZoneCnt[(int)side, (int)zone];
                }
                //項目別
                foreach (AppData.InspID inspId in Enum.GetValues(typeof(AppData.InspID)))
                {
                    _realtimeResultActionDataClass.CountNGItems[(int)side, (int)inspId] = ItemsCnt[(int)side, (int)inspId];
                }
            }
            //カメラ別
            foreach (AppData.CamID cam in Enum.GetValues(typeof(AppData.CamID)))
            {
                _realtimeResultActionDataClass.CountNGCamera[(int)cam] = CamCnt[(int)cam];
            }
        }
        public void ResetRuikei()
        {
            _realtimeResultActionDataClass.ClearRirekiCount();
            UclTotalReal.ClearCount();
            clsRirekiCount.getInstance().Save(Path.GetDirectoryName(Recipe.GetInstance().Path), _realtimeResultActionDataClass);
        }

        private void btnImageMain_Click(object sender, EventArgs e)
        {
            //レシピに変更があるか。
            ChangeRecipeMessage();

            UclImageMain.Visible = true;
        }

        private void CheckDriveUseSize()
        {
            if (SystemParam.GetInstance().IM_AutoSaveEnable == true)
            {
                string sDrive = Path.GetPathRoot(_autoInsp.IniAccess.BmpSaveDir);
                DriveInfo info = new DriveInfo(sDrive);
                long lTotalSize = info.TotalSize;
                long lFreeSpace = info.TotalFreeSpace;
                double UsePercent = ((lTotalSize - lFreeSpace) / (double)lTotalSize * 100);
                if (UsePercent >= 70.0)
                {
                    frmMessageForm frm = new frmMessageForm("ディスク使用量が７０％を超えました。\nメーカにご連絡ください。", MessageType.Information);
                    frm.ShowDialog();
                    SystemParam.GetInstance().IM_AutoSaveEnable = false;
                }
            }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            CheckDriveUseSize();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //最小化された
            if (this.WindowState == FormWindowState.Minimized)
            {
                if ((Control.ModifierKeys & Keys.Control) == 0)
                {
                    _tmNotMinimied.Tick += _tmNotMinimied_Tick;
                    _tmNotMinimied.Interval = 10000;
                    _tmNotMinimied.Start();
                }
            }
        }

        Rs232cSerial _rs232c1;
        Rs232cSerial _rs232c2;
        SpeedController _spController1;
        SpeedController _spController2;

        /// <summary>Khởi tạo Kiểm soát tốc độ nối tiếp </summary>
        private void InitializeSerialSpeedControll()
        {
            if (SystemParam.GetInstance().SerialEnable1)
            {
                _rs232c1 = new Rs232cSerial(
                    SystemParam.GetInstance().SerialComPort1,
                    SystemParam.GetInstance().SerialBaudRate1,
                    (Parity)Enum.Parse(typeof(Parity), SystemParam.GetInstance().SerialParity1),
                    SystemParam.GetInstance().SerialDataBits1,
                    (StopBits)Enum.Parse(typeof(StopBits), SystemParam.GetInstance().SerialStopBits1),
                    (Handshake)Enum.Parse(typeof(Handshake), SystemParam.GetInstance().SerialHandshake1),
                    SystemParam.GetInstance().SerialEnCoding1,
                    (Rs232cSerial.ReturnCode)Enum.Parse(typeof(Rs232cSerial.ReturnCode), SystemParam.GetInstance().SerialRtnCode1));
                _spController1 = new SpeedController(_rs232c1, SystemParam.GetInstance().SerialCycleWaitTime1);
                _spController1.OnGetSpeed += _spController1_OnGetSpeed;
                _spController1.Open();
            }
            if (SystemParam.GetInstance().SerialEnable2)
            {
                _rs232c2 = new Rs232cSerial(
                    SystemParam.GetInstance().SerialComPort2,
                    SystemParam.GetInstance().SerialBaudRate2,
                    (Parity)Enum.Parse(typeof(Parity), SystemParam.GetInstance().SerialParity2),
                    SystemParam.GetInstance().SerialDataBits2,
                    (StopBits)Enum.Parse(typeof(StopBits), SystemParam.GetInstance().SerialStopBits2),
                    (Handshake)Enum.Parse(typeof(Handshake), SystemParam.GetInstance().SerialHandshake2),
                    SystemParam.GetInstance().SerialEnCoding2,
                    (Rs232cSerial.ReturnCode)Enum.Parse(typeof(Rs232cSerial.ReturnCode), SystemParam.GetInstance().SerialRtnCode2));
                _spController2 = new SpeedController(_rs232c2, SystemParam.GetInstance().SerialCycleWaitTime2);
                _spController2.OnGetSpeed += _spController2_OnGetSpeed;
                _spController2.Open();
            }
        }

        private void _spController1_OnGetSpeed(object sender, double val)
        {
            Action act = new Action(() =>
            {
                _lsttsslSpeeds[0].Text = val.ToString("F1");
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }

        private void _spController2_OnGetSpeed(object sender, double val)
        {
            Action act = new Action(() =>
            {
                _lsttsslSpeeds[1].Text = val.ToString("F1");
            });
            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }

        private void TerminateSerialSpeedControll()
        {
            if (SystemParam.GetInstance().SerialEnable1)
            {
                _spController1.Close();
            }
            if (SystemParam.GetInstance().SerialEnable2)
            {
                _spController2.Close();
            }
        }

        private void btnNgReset_Click(object sender, EventArgs e)
        {
            clsSignalControl.GetInstance().ResetNG();
        }
    }
}
