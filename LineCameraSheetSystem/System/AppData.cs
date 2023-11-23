using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Linq.Expressions;
using Fujita.InspectionSystem;
using System.Reflection;

namespace Fujita.InspectionSystem
{
    public static class GetNameClass
    {
        public static string GetName<TResult>(Expression<Func<TResult>> propertyName)
        {
            var memberEx = propertyName.Body as MemberExpression;
            return memberEx.Member.Name;
        }
    }
}

namespace LineCameraSheetSystem
{
    public class AppData                                                    // class này lưu toàn bộ dữ liệu chương trình
    {
        private static AppData _singleton = new AppData();

        private AppData()
        {
        }
        
        public static AppData GetInstance()
        {
            return _singleton;
        }
        public static AppData getInstance()
        {
            return _singleton;
        }

        public void Initialize()
        {
            _status = new Status();
        }

        //exeフォルダパス
        public const string　EXE_FOLDER = @"c:\fujitaSheet\exe\";
        //systemパラメータファイル名
        public const string SYSTEM_FILE = "System.ini";
        //systemカウンターファイル名 /v1330追加
        public const string SYSTEM_COUNTER = "Counter.ini";
        //レシピフォルダのパス デフォルト用
        public const string RCP_FOLDER  = @"c:\fujitaSheet\Recipe\";
        //レシピファイル名
        public const string RCP_FILE    = "Recipe.rcp";
        //検査結果フォルダのパス　デフォルト用
        public const string PURODUCT_FOLDER = @"c:\fujitaSheet\Product\";
        //結果ファイル名
        public const string RRESULT_FILE = "result.txt";
        //communication.ini
        public const string COMMUNICATION_FILE = "communication.ini";
        //camera.ini
        public const string CAMERA_FILE = "camera.ini";
        //LightControl
        public const string LIGHTCONTROL_FILE = "lightcontrol.ini";
        //producttimeのパス　デフォル用
        public const string PURODUCT_TIME_FOLDER = @"c:\fujitaSheet\ProductTime\";
        //imageフォルダのパス　デフォルト用
        public const string IMAGE_FOLDER = @"c:\fujitaSheet\Image\";
        //backupフォルダのパス　デフォルト用
        public const string BACKUP_FOLDER = @"c:\fujitaSheet\backup\";
        //Logフォルダのパス　
        public const string LOG_FOLDER = @"c:\fujitaSheet\Log\";

        //メイン画面のText(タイトル)
        public const string DEFAULT_APP_NAME = "シート外観検査システム";
        public const string OLD_APP_NAME = "シート外観検査システム    　-過去データ表示中-";

        //カメラ台数
        public const int CAM_COUNT = 2;

        //メイン画面の左側のウインドウの位置
        public const int LEFT_X = 5;
        public const int LEFT_Y = 86;
        
        //メイン画面の右側のウインドウの位置
        public const int RIGHT_X = 530;
        public const int RIGHT_Y = 86;

        //ユーザーコントロール幅高さ
        public const int LEFT_WIDTH = 498;
        public const int RIGHT_WIDTH = 598;
        public const int HEIGHT = 840;

        //タイマーの更新間隔ms
        public const int TIMER_INTERVAL = 300;

        //現在品種でのボタンtext
        public const string BTN_NGLIST = "欠点ﾘｽﾄ";
        public const string BTN_MAP　　　 = "マップ";
        public const string BTN_NGMINIIMG = "画像";
        public const string BTN_NG1IMG　  = "NG1画像";
        public const string BTN_TOTAL　   = "累計";
        public const string BTN_RECIPE    = "品種";
        public const string BTN_OLDLIST   = "過去リスト";　　//共通
        public const string BTN_SYSTEM    = "システム";  //共通

        //過去品種でのボタンtext
        public const string BTN_OLD_NGLIST    = "戻る";
        public const string BTN_OLD_MAP       = "マップ";
        public const string BTN_OLD_NGMINIIMG = "画像";
        public const string BTN_OLD_NG1IMG    = "NG1画像";
        public const string BTN_OLD_TOTAL     = "累計";
        public const string BTN_OLD_RECIPE    = "品種";
    
        //最大分割数
        public const int MAX_PARTITION = 8;

        //色　
        public const string COLOR_RED     = "#FFFF0000";
        public const string COLOR_BLUE    = "#FF00FFFF";
        public const string COLOR_LIME    = "#FF00FF00";
        public const string COLOR_YELLOW  = "#FFFFFF00";
        public const string COLOR_CYAN    = "#FF00FFFF";
        public const string COLOR_MAGENDA = "#FFFF00FF";

        //登録品種数
        public const int ENTRY_COUNT = 256;

        //照明255→999に変換の倍率
        public const double LED_CONVERT = 3.92;
        /// <summary>
        /// 検査ID
        /// </summary>
        public enum InspID 
        {
            明大,
            明中,
            明小,
            暗大,
            暗中,
            暗小
        }

        /// <summary>
        /// カメラID
        /// </summary>
        public enum CamID 
        {
            cam1,
            cam2,
            cam3,
            cam4
        }

        /// <summary>
        /// ゾーンID
        /// </summary>
        public enum ZoneID
        {
            Z1 , Z2, Z3, Z4, Z5, Z6, Z7, Z8
        }

        /// <summary>
        /// 表裏
        /// </summary>
        public enum SideID
        {
            表,
            裏
        }

        /// <summary>
        /// Real/Old
        /// </summary>
        public enum ModeID
        {
            Real,
            Old
        }

        /// <summary>
        /// カメラポジション(左,右)
        /// </summary>
        public enum CamPosition
        {
            Left,
            Right,
        }

        /// <summary>
        /// ユーザー設定モード
        /// </summary>
        public enum EUserSettingMode
        {
            NotSetting,
            Rectangle1,
            Rectangle2,
            Threshold,
            Histgram,
            ManualMeasure,
            PointMulti,
            Rectangle1Multi,
            Circle,
        }
        /// <summary>
        /// 現在のモード
        /// </summary>
        public enum EMode
        {
            Main,
            RecipeSetting,
            Inspection,
            CameraSetting,
            DeviceSetting,
        }

        /// <summary>
        /// 動作モード
        /// </summary>
        public enum EActionMode
        {
            Normal,
            ShutdownSequence,
            RebootSequence,
        }

        private Status _status;
        public Status status
        {
            get
            {
                return _status;
            }
        }

        public class Status
        {
            public EUserSettingMode UserSettingMode { get; set; }

            public EMode Mode { get; set; }
            public EActionMode ActionMode { get; set; }

            public string ExePath { get; private set; }
            public string ExeName { get; private set; }

            /// <summary>
            /// OSのシャットダウンシーケンスが発生した
            /// </summary>
            public bool QueryEndSession { get; set; }
            /// <summary>
            /// UPSからシャットダウン指令が来た
            /// </summary>
            public bool UpsShutdown { get; set; }
            /// <summary>
            /// OSシャットダウンを行うか
            /// </summary>
            public bool OsShutdown { get; set; }

            /// <summary>
            /// アプリケーション起動開始時間
            /// </summary>
            public DateTime ExeStartTime { get; private set; }

            /// <summary>
            /// 現在のユーザーモード
            /// </summary>
            private User _user;
            public EAuthenticationType User
            {
                get
                {
                    return _user.Authentication;
                }
                set
                {
                    _user.Authentication = value;
                }
            }

            public string UserJpn
            {
                get
                {
                    return _user.AuthenticationJpn;
                }
            }

            int _iMajorVersion;
            public int MajorVersion
            {
                get
                {
                    return _iMajorVersion;
                }
            }

            int _iMinorVersion;
            public int MinorVersion
            {
                get
                {
                    return _iMinorVersion;
                }
            }

            int _iRevision;
            public int Revision
            {
                get
                {
                    return _iRevision;
                }
            }

            int _iBuild;
            public int Build
            {
                get
                {
                    return _iBuild;
                }
            }

            public string VersionShort
            {
                get
                {
                    return _iMajorVersion.ToString() + "." + _iMinorVersion.ToString();
                }
            }

            public string VersionFull
            {
                get
                {
                    return _iMajorVersion.ToString() + "." + _iMinorVersion.ToString() + "." + _iBuild.ToString() + "." + _iRevision.ToString();
                }
            }

            public Status() // khởi tạo và gán giá trị
            {
                ExeStartTime = DateTime.Now;
                _user = new User(EAuthenticationType.Developer);
                Mode = EMode.Main;
                ActionMode = EActionMode.Normal;
                Assembly myAsm = Assembly.GetEntryAssembly();
                string path = myAsm.Location;
                ExePath = path.Substring(0, path.LastIndexOf("\\") + 1);
                ExeName = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);

                QueryEndSession = false;
                OsShutdown = false;
                UpsShutdown = false;

                // C#
                Assembly mainAssembly = Assembly.GetEntryAssembly();
                AssemblyName mainAssemName = mainAssembly.GetName();
                // バージョン名（AssemblyVersion属性）を取得
                Version vVersion = mainAssemName.Version;
                _iMajorVersion = vVersion.Major;
                _iMinorVersion = vVersion.Minor;
                _iRevision = vVersion.Revision;
                _iBuild = vVersion.Build;

                UserSettingMode = EUserSettingMode.NotSetting;
            }
        }

    }
}
