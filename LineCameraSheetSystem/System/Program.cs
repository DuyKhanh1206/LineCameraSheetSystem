using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;

using LogingDllWrap;
using Fujita.Communication;
using Fujita.LightControl;
using HalconCamera;
using Fujita.InspectionSystem;
using Adjustment;
using LineCameraSheetSystem.Adjust;
using InterfaceCorpDllWrap;

namespace LineCameraSheetSystem
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ロガーのスタート       khởi động Dll ghi nhật ký
            LogingDll.Loging_Init("-LineCameraSheetSystem", "");

            try　                        // khởi tạo camera 
            {   
                HalconDotNet.HObject image;
                HalconDotNet.HOperatorSet.GenImageConst(out image, "byte", 4096, 4096);
                image.Dispose();
            }
            catch (HalconDotNet.HalconException exc)
            {
                SplashForm.CloseSplash();
                string msgStr = "ライセンス認証に失敗しました。";
                MessageBox.Show(msgStr, "LineCameraSheet System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Trace.WriteLine(string.Format("{0} : {1}", msgStr, exc.Message));
                return;
            }

            // 二重起動防止       ngăn chặn khởi động kép
            Mutex mutex = new System.Threading.Mutex(false, "LineCameraSheetSystem");
            if (!mutex.WaitOne(0, false))
            {
                //mutexErrorLog("システムプロセスが存在しています[二重起動]");
                //frmMessageForm frm = new frmMessageForm("システムプロセスが存在しています。\nシステムを再起動します。", MessageType.Warning);
                //frm.ShowDialog();
                //clsShutdown _clsShutdown = new clsShutdown();
                //_clsShutdown.Reboot();
                MessageBox.Show("アプリケーションはすでに起動しています", "LineCameraSheet System");                   //Ứng dụng đã được bắt đầu
                return;
            }

            SplashForm.ShowSplash();
            SplashForm.ProgressSplash(10, "起動を開始しました");             //Bắt đầu khởi động , đưa ra hiển thị các tiến trình hoàn thành đến đâu và hiển thị ra màn hình

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // エラーイベント登録                Đăng ký sự kiện lỗi sẽ thông báo nếu có lỗi
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

                // Even thông báo ngoại lệ
                Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Program_UnhandledException);

                LogingDll.Loging_SetLogString("-------------------- System Start --------------------");
                // 
                if (initApplication())   // chạy hết hàm này để load data thì chạy tới main form load tiếp từ 65% nếu k có lỗi thì chạy sang form MainForm.
                {
#if !ADJUSTMENT_FORM
                    MainForm fm = new MainForm();
                    SystemStatus.GetInstance().MainForm = fm;
                    Application.Run(fm);            // Run Form mainForm
#else
            //速度度監視cls
 //           clsNoInspectionSpeedMonitor.GetInstance();
            frmAdjust frmAd = new frmAdjust();
            frmAd.SetOffsetParamContainer(clsAdjustmentAppData.getInstance());
            frmAd.SetResolutionParamContainer(clsAdjustmentAppData.getInstance());
            Application.Run(frmAd);
#endif
                }
            }
            finally
            {
                // buộc dừng, tắt đen, tắt cam, tắt com
                termApplication();

                // シャットダウン処理             xử lý tắt máy tính
                queryShutdown();

                LogingDll.Loging_SetLogString("#################### System End ####################");

                // 二重起動防止用ミューテックスの解放            Phát hành một mutex để ngăn kích hoạt kép
                mutex.ReleaseMutex();
                mutex.Close();

            }
        }

        private static void mutexErrorLog(string msg)
        {
            string dir = SystemParam.LogFolder;
            string filename = string.Format("MutexException-{0}.log", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            string path = System.IO.Path.Combine(dir, filename);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, Encoding.GetEncoding("Shift-JIS")))
            {
                sw.Write(msg);
            }
        }

        /// <summary> load data từ 0% tới 60% ở forrm này  </summary> 
        private static bool initApplication()
        {
            AppData.getInstance().Initialize(); // khởi tạo và gán giá trị 

            SplashForm.ProgressSplash(15, "システムのロード中です"); // hiển thị thông báo đang đang nạp tiến trình

            //システムiniのロード
            SystemParam.GetInstance().SystemLoad();  // load thông tin hệ thống
            //システムカウンターのロード v1330
            SystemCounter.GetInstance().Load(AppData.EXE_FOLDER + AppData.SYSTEM_COUNTER);    // load system counter

            SplashForm.ProgressSplash(20, "システムの初期処理中です");              // đang khởi tạo hệ thống

            //システムコンテキスト
            SystemContext syscont = SystemContext.GetInstance();
            syscont.Initialize();

            SplashForm.ProgressSplash(25, "システムステータスの初期処理中です");  // xử lý trạng thái ban đầu hệ thống

            //システムステータス
            SystemStatus syssta = SystemStatus.GetInstance();
            syssta.Initialize();

            SplashForm.ProgressSplash(30, "通信の初期処理中です");                //Quá trình xử lý ban đầu của giao tiếp đang được tiến hành.

            CommunicationManager com = CommunicationManager.getInstance();
            com.Initialize(true);
            if (!com.Load(SystemParam.GetInstance().CommunicatioPath + AppData.COMMUNICATION_FILE, ""))
            {
                SplashForm.CloseSplash();
                Utility.ShowMessage(null, com.LastErrorMessage, AppData.DEFAULT_APP_NAME, MessageType.Error);
                return false;
            }

            SplashForm.ProgressSplash(35, "シグナルタワー制御の初期処理中です");   // điều khiển ban đầu của tháp tín hiệu

            //シグナルタワーコントロール
            clsSignalControl SignalCtrl = clsSignalControl.GetInstance();
            SignalCtrl.Initialize(com.getCommunicationDIO());
            SignalCtrl.Load(AppData.EXE_FOLDER + AppData.SYSTEM_FILE);
            SignalCtrl.Start();

            SplashForm.ProgressSplash(40, "カメラ設定の初期処理中です");        // tiến trình cài đặt máy ảnh

            //カメラマネージャー
            APCameraManager cam = APCameraManager.getInstance();
            cam.Initialize();
            if (!cam.Load(SystemParam.GetInstance().CameraPath + AppData.CAMERA_FILE, ""))          // load CAM
            {
                SplashForm.CloseSplash();
                SignalCtrl.SetError();
                Utility.ShowMessage(null, cam.LastErrorMessage, AppData.DEFAULT_APP_NAME, MessageType.Error);
                SignalCtrl.ResetError();
                return false;
            }
            for (int i = 0; i < SystemParam.GetInstance().camParam.Count; i++)
            {
                int iCamIndex = SystemParam.GetInstance().camParam[i].CamNo;
                HalconCameraBase camBase = cam.GetCamera(iCamIndex);    //lấy ra tên CAM trong list
                int iVerConnectCnt;
                if (camBase == null)
                {
                    camBase = cam.GetCamera(0);
                }
                else
                {
                }
                iVerConnectCnt = camBase.ImageConnectCnt;
                SystemParam.GetInstance().camParam[i].PixH = camBase.ImageWidth;
                SystemParam.GetInstance().camParam[i].PixV = camBase.ImageHeight * ((iVerConnectCnt <= 1) ? 1 : iVerConnectCnt);
            }

            SplashForm.ProgressSplash(45, "カメラの設定中です"); /// đang tiến hành cài đặt máy ảnh

            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                AppData.SideID side;
                if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
                {
                    int exp = (side == AppData.SideID.表) ? SystemParam.GetInstance().CamExposure : SystemParam.GetInstance().CamExposureUra;
                    double spd = (side == AppData.SideID.表) ? SystemParam.GetInstance().CamSpeed : SystemParam.GetInstance().CamSpeedUra;

                    HalconCameraBase camBase = APCameraManager.getInstance().GetCamera(i);
                    camBase.SetExposureTime(exp);
                    double hz = SystemParam.GetInstance().Speed2Hz(spd);
                    camBase.SetLineRate(hz);
                    camBase.InitPolling();//v1328
                }

                SplashForm.ProgressSplash(46 + i, "カメラの設定中です");
            }

            SplashForm.ProgressSplash(50, "照明制御の初期処理中です");          // quá trình xử lý ban đầu của điều khiển ánh sáng đang được tiến hành

            //照明コントロール          khởi tạo và điều khiển ánh sáng đèn
            LightControlManager light = LightControlManager.getInstance();   
            light.Initialize(true);   // khởi tạo list  để chứa danh sách đèn và list Type các loại đèn
            light.ConnectTimeOut = SystemParam.GetInstance().NetConnectMonitor_ConnectTimeOut;  // đặt thời gian chờ ( time Out)
            light.InitNetMonitorEnable = SystemParam.GetInstance().NetConnectMonitor_Enable;    // nhận true/ false ---- kích hoạt màn hình
            if (!light.Load(SystemParam.GetInstance().LightCtrlPath + AppData.LIGHTCONTROL_FILE, ""))
            {
                SplashForm.CloseSplash();
                SignalCtrl.SetError();
                Utility.ShowMessage(null, light.LastErrorMessage, AppData.DEFAULT_APP_NAME, MessageType.Error);
                SignalCtrl.ResetError();
                return false;
            }
            light.AllLightOff();

            SplashForm.ProgressSplash(55, "監視制御の初期処理中です"); //Quá trình xử lý ban đầu của kiểm soát giám sát đang được tiến hành.

            //照明点灯監視Init
            syscont.InitializeLightMeasPeriod();

            SplashForm.ProgressSplash(60, "メイン画面の起動中です");           //Màn hình chính đang khởi động được 60%

            clsRirekiCount.getInstance().Initialize();

            //メンテナンス        (Bảo trì)
            clsMainteFunc.getInstance().Initialize(CameraManager.getInstance().CameraNum, LightControlManager.getInstance().LightCount);        //cài đặt thông số cho đèn và camera
            string sMainte = Path.Combine(clsMainteFunc.getInstance().MaintePath, clsMainteFunc.DEFAULT_MAINTE_FILE) + clsMainteFunc.DEFAULT_MAINTE_FILE_EXIST; // ghép chuỗi Path
            if (File.Exists(sMainte) == true)// kiểm tra chuỗi Path có tồn tại không
                clsMainteFunc.getInstance().Load(sMainte);                  // Load dữu liệu, cài đặt thông số cho đèn và camera

            return true;// kết thúc quá tình 1

        }

        /// <summary>nếu xảy ra lỗi thì TẮT đèn tắt cam và tắt com lưu lại các chỉ số mặc định ...... </summary>
        private static void termApplication()
        {
            // tắt all Right
            LightControlManager light = LightControlManager.getInstance();
            light.Terminate();

            APCameraManager cam = APCameraManager.getInstance();
            cam.Terminate();

            clsSignalControl.GetInstance().Terminate();

            CommunicationManager com = CommunicationManager.getInstance();
            com.Terminate();

            //v1329 terminate時にセーブしないように変更
            //SystemParam.GetInstance().SystemSave();

        }


        // ghi thông tin các thuws rồi tắt máy
        private static void queryShutdown()
        {
            // シャットダウン処理                                    xử lý tắt máy
            // UPSシャットダウン時はメッセージを表示しない           Không hiển thị thông báo khi UPS tắt
            if (SystemStatus.GetInstance().RestoreShutdown == false)// khởi động lại k thành công 
            {
                if (SystemParam.GetInstance().AutoShutdownEnable && !SystemStatus.GetInstance().UpsShutDown)
                {
                    frmMessageTimer frmMesTmr = new frmMessageTimer("PCをシャットダウンします。", MessageType.YesNo, SystemParam.GetInstance().AutoShutdownWaitSec);
                    if (DialogResult.Yes == frmMesTmr.ShowDialog())
                    {
                        LogingDll.Loging_SetLogString("[PCをシャットダウンします。]ー[ Yes ]ボタンを押下した　または、ほっておいてシャットダウン実行");
                        clsShutdown _clsShutdown = new clsShutdown();
                        _clsShutdown.Shutdown();
                    }
                    else
                    {
                        LogingDll.Loging_SetLogString("[PCをシャットダウンします。]ー[ No ]ボタンを押下した");
                    }
                }
            }
            else// khởi động thành công thì tắt máy
            {
                clsShutdown _clsShutdown = new clsShutdown();
                _clsShutdown.Shutdown();
            }
        }

        /// <summary> sự kiện thông báo nếu có lỗi khởi động xảy ra. </summary>
        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ShowErrorMessage(e.Exception, "ThreadException による例外通知です。");
        }

        /// <summary>Thông báo ngoại lệ do UnhandledException </summary>    EVENT
        static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ShowErrorMessage(ex, "UnhandledException による例外通知です。");
            }
        }

        /// <summary>thông báo lỗi bằng messageBox và ghi lỗi bằng          EVENT
        /// StringBuilder sb = new StringBuilder();        sb.AppendLine(message); thay cho cộng string </summary>
        /// rồi ghi lỗi ra đường link. hiển thị thông báo rồi chạy hàm OutputError(false); để tắt máy khởi động lại.
        public static void ShowErrorMessage(Exception e, string message)
        {
            System.Diagnostics.Trace.WriteLine(e.Message);
            System.Diagnostics.Trace.WriteLine(message);
            try
            {
                //                // システム終了中ならエラー表示しない
                //                if (Program.Context.IsExiting)
                //                {
                //                    return;
                //                }

                OutputError(true);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(message);
                sb.AppendLine();
                sb.AppendLine("アプリケーションでエラーが発生しました。");
                sb.AppendLine();
                sb.AppendLine("【エラー内容】");
                sb.AppendLine(e.Message);
                sb.AppendLine();
                sb.AppendLine("【スタックトレース】");
                sb.AppendLine(e.StackTrace);
                sb.AppendLine();

                string dir = SystemParam.LogFolder;
                string filename = string.Format("Exception-{0}.log", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff", DateTimeFormatInfo.InvariantInfo));
                string path = Path.Combine(dir, filename);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("Shift-JIS")))
                {
                    sw.Write(sb.ToString());
                }

                sb.AppendLine();
                sb.AppendLine("システムを終了します。");

                var result = MessageBox.Show(sb.ToString(), AppData.DEFAULT_APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);

                OutputError(false);

            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.WriteLine(exc.Message);
            }
            finally
            {
                Application.Exit();
            }
        }

        // ghi lỗi vào dll hoặc khởi động lại nếu value = false
        private static void OutputError(bool value)
        {
            try
            {
                if (value)
                    clsSignalControl.GetInstance().SetError();
                else
                    clsSignalControl.GetInstance().ResetError();// khởi động lại máy bằng tư viện user32.dll
            }
            catch (Exception e)
            {
                LogingDll.Loging_SetLogString(string.Format("OutputError({0}) e.Message:{1}", value.ToString(), e.Message));
            }
        }

    }
}
