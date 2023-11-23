using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineCameraSheetSystem
{
    class SystemStatus
    {
        private static SystemStatus _singleton = new SystemStatus();

        private SystemStatus()
        {
            //カメラ基準露光値の初期化[カメラ数 ,valの数 ]
          //  ExposureDefault = new int[4, 2] { { 0, 61 }, { 0, 61 }, { 0, 61 }, { 0, 61 } };
        }

        public static SystemStatus GetInstance()
        {
            return _singleton;
        }


        //ラインスピード
        public int LineSpeed { get; set; }
        //現在測長
        public int NowLength { get; set; }
        //現在の状態
        public State NowState { get; set; }

        //画面に表示しているか　表示中：true　　非表表示：false
        public bool DispNgList { get; set; }
        public bool DispMap { get; set; }
        public bool DispNgThumbnail { set; get; }
        public bool DispTotal { set; get; }
        public bool DispKindList { set; get; }
        public bool DispKindContents { set; get; }
        public bool DispOldProductList { set; get; }
        public bool DispSystem { set; get; }

        public bool DispNgListOld { get; set; }
        public bool DispMapOld { get; set; }
        public bool DispNgThumbnailOld { set; get; }
        public bool DispTotalOld { set; get; }
        public bool DispKindContentsOld { set; get; }

        //現在品種か過去品種か Real:現在　Old:過去
       public ModeID DataDispMode { get; set; }
        //レシピ選択済み
        public bool SelectRecipe { get; set; }
        //レシピ数値変更未保存
        public bool RecipeEdit { set; get; }
        //現在左右の表示画面
        public DisplayPair NowPairDisplay { set; get; }

        //強制シャットダウン
        public bool UpsShutDown { set; get; }

        public bool RestoreShutdown { get; set; }
        public System.Windows.Forms.Form MainForm { get; set; }


        /// <summary>
        /// 状態
        /// </summary>
        public enum State
        {
            Stop,
            Inspection,
            Suspend
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
        /// 画面組み合わせ
        /// </summary>
        public enum DisplayPair
        {
            RealNglistMap,
            RealNglistNgthumb,
            RealNglistTotal,
            RealNglistSystem,
            OldNglistMap,
            OldNglistNgthumb,
            OldNglistTotal,
            OldNglistKinddata,
            OldNglistSystem,
            OldProductlist,
            RealKindlistKinddata
        }

        public bool CheckChangeDisp(DisplayPair dispPair)
        {
            if (dispPair == DisplayPair.RealNglistMap)
            {
                if (DataDispMode == ModeID.Real)
                {
                    NowPairDisplay = dispPair;
                    return true;
                }
            }
            else if (dispPair == DisplayPair.RealNglistNgthumb)
            {
                if (DataDispMode == ModeID.Real)
                {
                    NowPairDisplay = dispPair;
                    return true;
                }
            }
            else if (dispPair == DisplayPair.RealNglistTotal)
            {
                if (DataDispMode == ModeID.Real)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.RealNglistSystem)
            {
                if (DataDispMode == ModeID.Real)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldNglistMap)
            {
                if (DataDispMode == ModeID.Old)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldNglistNgthumb)
            {
                if (DataDispMode == ModeID.Old)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldNglistKinddata)
            {
                if (DataDispMode == ModeID.Old)
                {
                    NowPairDisplay = dispPair;
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldNglistTotal)
            {
                if (DataDispMode == ModeID.Old)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldNglistSystem)
            {
                if (DataDispMode == ModeID.Old)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.RealKindlistKinddata)
            {
                if (DataDispMode == ModeID.Real)
                {
                    NowPairDisplay = dispPair; 
                    return true;
                }
            }
            else if (dispPair == DisplayPair.OldProductlist)
            {
                NowPairDisplay = dispPair;
                return true;      
            }
        
            return false;
        }

        public void Initialize()
        {
            this.RecipeEdit = false;
            this.UpsShutDown = false;
        }
    }
}
