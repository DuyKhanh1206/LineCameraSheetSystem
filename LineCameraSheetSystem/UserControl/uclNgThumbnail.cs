using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using Fujita.Misc;
using ResultActionDataClassNameSpace;

namespace LineCameraSheetSystem
{
    public partial class uclNgThumbnail : UserControl,IShortcutClient
    {
        private string _strON = "     ON     ";
        private string _strOFF = "    OFF    ";
        private string _strNONE = "-";
        private Color _titleColor = SystemColors.Control;
        private Color _onColor = Color.LightGreen;
        private Color _offColor = Color.LightYellow;
        private Color _noneColor = SystemColors.Window;

        public uclNgThumbnail()
        {
            InitializeComponent();

            sumnailimgArray = new uclMiniImage[] {uclMiniImage1, uclMiniImage2, uclMiniImage3,
                                                uclMiniImage4, uclMiniImage5, uclMiniImage6};

            LoadSetPart();

            this.iPageNow = 1;
            this.iPageMax = 1;

            int i, j;

            //enumの数を取得する
            j = Enum.GetNames(typeof(AppData.ZoneID)).Length;
            for (i = 0; j > i; i++)
            {
                dgvOnOffZone.Rows.Add();
                dgvOnOffZone.Rows[i].Cells[0].Value =((AppData.ZoneID)i).ToString();
                SetNONE(dgvOnOffZone, i);
                dgvOnOffZone.Rows[i].Cells[0].Style.BackColor = _titleColor;
            }
            dgvOnOffZone.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgvOnOffZone.DefaultCellStyle.SelectionForeColor = Color.Transparent;

            //enumの数を取得する
            j = Enum.GetNames(typeof(AppData.SideID)).Length;
            for (i = 0; j > i; i++)
            {
                dgvOnOffSide.Rows.Add();
                dgvOnOffSide.Rows[i].Cells[0].Value = ((AppData.SideID)i).ToString();
                SetNONE(dgvOnOffSide, i);
                dgvOnOffSide.Rows[i].Cells[0].Style.BackColor = _titleColor;
            }
            dgvOnOffSide.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgvOnOffSide.DefaultCellStyle.SelectionForeColor = Color.Transparent;

            //enumの数を取得する
            j = Enum.GetNames(typeof(AppData.InspID)).Length;
            for (i = 0; j > i; i++)
            {
                dgvOnOffKind.Rows.Add();
                dgvOnOffKind.Rows[i].Cells[0].Value = ((AppData.InspID)i).ToString().Replace("暗", "");
                SetNONE(dgvOnOffKind, i);
                dgvOnOffKind.Rows[i].Cells[0].Style.BackColor = _titleColor;
            }
            dgvOnOffKind.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgvOnOffKind.DefaultCellStyle.SelectionForeColor = Color.Transparent;

            this.LockAutoChangePage = false;

            ClearResDataAll();

            shortcutKeyHelper1.SetShortcutKeys(btnBack, Keys.B);
            shortcutKeyHelper1.SetShortcutKeys(btnNext, Keys.N);
            shortcutKeyHelper1.SetShortcutKeys(btnFirst, Keys.T);
            shortcutKeyHelper1.SetShortcutKeys(btnLast, Keys.E);
        }


        //MainForm
        MainForm _mainForm { get; set; }
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;
        }

        public MainForm MainForm()
        {
            return _mainForm;
        }

        //uclMiniImageUserCtrlの配列
        private uclMiniImage[] sumnailimgArray;

        //SetPart用-画像左上隅の行
        private int RowLU { get; set; }
        //SetPart用-画像左上隅の列
        private int ColumnLU { get; set; }
        //SetPart用-画像右下隅の行
        private int RowRD { get; set; }
        //SetPart用-画像右下隅の列
        private int ColumnRD{ get; set; }

        //現在のページ
        public int iPageNow { get;  set; }
        //最大のページ数
        public int iPageMax{get; set;}
        //余りの数
        public int iRemainder { set; get; }

        //1NG画像に渡すよう
        private List<ResActionData> _resAcData;

        //スクロールロック
        public bool LockAutoChangePage { set; get; }

        //SetPart用のrow,columnのiniファイルからの取得して反映させる
        private void LoadSetPart()
        {
            IniFileAccess ini = new IniFileAccess();
            RowLU = ini.GetIniInt("SetPart", "RowLU", AppData.EXE_FOLDER + AppData.SYSTEM_FILE, 0);
            ColumnLU = ini.GetIniInt("SetPart", "ColumnLU", AppData.EXE_FOLDER + AppData.SYSTEM_FILE, 0);
            RowRD = ini.GetIniInt("SetPart", "RowRD", AppData.EXE_FOLDER + AppData.SYSTEM_FILE, 300);
            ColumnRD = ini.GetIniInt("SetPart", "ColumnRD", AppData.EXE_FOLDER + AppData.SYSTEM_FILE, 300);

            foreach (uclMiniImage SumImg in sumnailimgArray)
            {             
              //  SumImg.SetPart(RowLU, ColumnLU, RowRD, ColumnRD);
            }
        }

        //画像の表示
        public void SetImage(int index ,HObject hoImage)
        {
            sumnailimgArray[index].SetImage(hoImage);      
        }

        //ONOFF　をセットする
        private void OnOffSet()
        {
            IniFileAccess ini = new IniFileAccess();
        }

        public delegate void RefreshThumbnailEventHandler(object sender, EventArgs e);
        public event RefreshThumbnailEventHandler OnRefreshThumbnail;
       
        public class FilterUpdateEventArgs : EventArgs
        {
        }

        public delegate void FilterUpdateEventHandler(object sender, FilterUpdateEventArgs e);
        public event FilterUpdateEventHandler OnFilterUpdate = null;

        private void EventFilterUpdate()
        {
            if (OnFilterUpdate != null)
            {
                OnFilterUpdate(this, new FilterUpdateEventArgs());
            } 

        }

        //レシピの分割数を見て、ONOFFの設定する。(検査開始時,過去レシピを見るとき)
        public void CheckOn(int partition)
        {
           // int part,i;

           // Recipe recipe = _mainForm.InspRecipe;
           // part = recipe.Partition;
            int i;

            //分割数のONOFF
            for(i=0; i < AppData.MAX_PARTITION ; i++)
            {
                if (i < partition)
                {
                    SetON(dgvOnOffZone, i);
                }
                else
                {
                    SetNONE(dgvOnOffZone, i);
                }
            }

            //裏表のONOFF
            SetON(dgvOnOffSide, 0);
            SetON(dgvOnOffSide, 1);

            //品種のONOFF
             //enumの数を取得する
            int j;
            j = Enum.GetNames(typeof(AppData.InspID)).Length;
            for(i=0;j>i;i++)
            {
                SetON(dgvOnOffKind, i);
            }
        }

        //dgvのOFFの設定をする
        public void CheckOFF()
        {
            int i, j;

            //分割数のONOFF
            for (i = 0; i < AppData.MAX_PARTITION; i++)
            {
                SetNONE(dgvOnOffZone, i);
            }

            //裏表のONOFF
            SetNONE(dgvOnOffSide, 0);
            SetNONE(dgvOnOffSide, 1);

            //品種のONOFF
            //enumの数を取得する
            j = Enum.GetNames(typeof(AppData.InspID)).Length;
            for (i = 0; j > i; i++)
            {
                SetNONE(dgvOnOffKind, i);
            }
        }

        private string GetONOFF(DataGridView dgv, int row)
        {
            if (row >= dgv.RowCount)
                return _strOFF;
            return dgv[1, row].Value.ToString();
        }
        private void SetON(DataGridView dgv, int row)
        {
            if (row >= dgv.RowCount)
                return;
            dgv[1, row].Selected = true;
            dgv[1, row].Value = _strON;
            dgv[1, row].Style.BackColor = _onColor;
            dgv[1, row].Selected = false;
        }
        private void SetOFF(DataGridView dgv, int row)
        {
            if (row >= dgv.RowCount)
                return;
            dgv[1, row].Selected = true;
            dgv[1, row].Value = _strOFF;
            dgv[1, row].Style.BackColor = _offColor;
            dgv[1, row].Selected = false;
        }
        private void SetNONE(DataGridView dgv, int row)
        {
            if (row >= dgv.RowCount)
                return;
            dgv[1, row].Selected = true;
            dgv[1, row].Value = _strNONE;
            dgv[1, row].Style.BackColor = _noneColor;
            dgv[1, row].Selected = false;
        }

        public enum EButtonType
        {
            Prev,
            Next,
        }

        public class ButtonClickEventArgs : EventArgs
        {
            public EButtonType ButtonType { get; private set; }

            public ButtonClickEventArgs(EButtonType bt)
            {
                ButtonType = bt;
            }
        }

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);

        public event ButtonClickEventHandler OnButtonClick = null;
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (OnButtonClick != null)
                OnButtonClick(this, new ButtonClickEventArgs( EButtonType.Prev ));
/*
            if (iPageNow > 1)
            {
                iPageNow--;
                labelPageNow.Text = iPageNow.ToString();
                _mainForm.ChangePage(iPageNow, iRemainder);
            }
 */
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (OnButtonClick != null)
                OnButtonClick(this, new ButtonClickEventArgs(EButtonType.Next));
/*         
            if (iPageMax > iPageNow)
            {
                iPageNow++;
                labelPageNow.Text = iPageNow.ToString();

                _mainForm.ChangePage(iPageNow,iRemainder);
            }
 */
        }

        public void SetNowPage(int page)
        {
            iPageNow = page;
            labelPageNow.Text = page.ToString();
        }

        public void ResetPage()
        {
            iPageNow = 1;
            labelPageNow.Text = "1";

            _mainForm.ChangePage(iPageNow, iRemainder);

        }

        //引数1：6で割った数　引数2：あまりの数
        public void SetMaxPage(int iPage ,int iRemaind)
        {
       
            iRemainder = iRemaind;
            if (iRemaind == 0)
            {
                iPageMax = (iPage == 0) ? 1 : iPage;
            }
            else
            {
                iPageMax = iPage + 1;
              
            }
            if (iPageNow > iPageMax)
            {
                iPageNow = iPageMax;
                labelPageNow.Text = iPageNow.ToString();
            }
            labelPageMax.Text = iPageMax.ToString();

        }

        public void SetNowPageMax()
        {
            this.iPageNow = this.iPageMax;
        }

        private void uclMiniImage1_DoubleClick(object sender, EventArgs e)
        {
            frmNg1Image Ng1Image = new frmNg1Image();
            Ng1Image.ShowDialog();
        }

        //イメージを消す
        public void ClearImage(int index)
        {
           
           sumnailimgArray[index].ClearImage();
            
        }
        //サムネイル6個を消す
        public void ClearImageAll()
        {
            foreach (uclMiniImage miniimg in sumnailimgArray)
            {
                miniimg.ClearImage();
            }
        }

        public void SetResData(int index,ResActionData resAcData)
        {

            sumnailimgArray[index].TextData = "縦:" + resAcData.Height.ToString(SystemParam.GetInstance().NgDataDecimal) + " 横:" + resAcData.Width.ToString(SystemParam.GetInstance().NgDataDecimal) + " 積:"+resAcData.Area.ToString(SystemParam.GetInstance().NgDataDecimal);
            sumnailimgArray[index].TextLength ="欠: " + resAcData.InspId.ToString().Replace("暗", "") + " 測: " + (resAcData.PositionY / 1000).ToString(SystemParam.GetInstance().LengthDecimal) + " ｱﾄﾞ:" + resAcData.PositionX.ToString(SystemParam.GetInstance().AddressDecimal);
            sumnailimgArray[index].TextSpot ="面: "+ resAcData.SideId.ToString() + " カ: " + resAcData.CamId.ToString().Remove(0, 3) +" ゾ: " + resAcData.ZoneId.ToString();
            sumnailimgArray[index]._miniImgResAcData = resAcData;

            sumnailimgArray[index].LineIndex = resAcData.LineNo;
        }

        public void ClearResData(int index)
        {
            sumnailimgArray[index].TextData = "縦: " + "   " + " 横" + "   " + "積" + "   ";
            sumnailimgArray[index].TextLength = "欠: " + "   " +  "測: " + "   " + " ｱﾄﾞ: " + "   ";
            sumnailimgArray[index].TextSpot = "面:" + "   " + "カ:" + "   " + " ゾ:" + "   ";

            sumnailimgArray[index].LineIndex = -1;
        }

        public void ClearResDataAll()
        {
            foreach (uclMiniImage miniimg in sumnailimgArray)
            {
                miniimg.TextData = "縦: " + "   " + " 横:" + "   " + "積:" + "   ";
                miniimg.TextLength = "欠: " + "   " + " 測: " + "   " + " ｱﾄﾞ: " + "   ";
                miniimg.TextSpot = "面:" + "   " + " カ:" + "   " + " ゾ:" + "   ";

            }
        }

        private void uclNgThumbnail_Load(object sender, EventArgs e)
        {
            for (int i = 0; sumnailimgArray.Length > i ; i++)
            {
                sumnailimgArray[i].CountOwn = i;
           //     sumnailimgArray[i]._uclngthnmb = this;

            }

            dgvOnOffZone[0, 0].Selected = false;
            dgvOnOffSide[0, 0].Selected = false;
            dgvOnOffKind[0,0].Selected = false;
        }

        public void SetResAcData(List<ResActionData> res)
        {
            _resAcData = res;
        }

        public bool[] GetOnOffZone()
        {
            bool[] zone = new bool[Enum.GetValues(typeof(AppData.ZoneID)).Length];
            for (int i = 0; i < zone.Length; i++)
            {
                if (GetONOFF(dgvOnOffZone, i) == _strON)
                    zone[i] = true;
                else
                    zone[i] = false;
            }
            return zone;
        }
        public bool[] GetOnOffSide()
        {
            bool[] side = new bool[Enum.GetValues(typeof(AppData.SideID)).Length];
            for (int i = 0; i < side.Length; i++)
            {
                if (GetONOFF(dgvOnOffSide, i) == _strON)
                    side[i] = true;
                else
                    side[i] = false;
            }
            return side;
        }
        public bool[] GetOnOffKind()
        {
            bool[] kind = new bool[Enum.GetValues(typeof(AppData.InspID)).Length];
            for (int i = 0; i < kind.Length; i++)
            {
                if (GetONOFF(dgvOnOffKind, i) == _strON)
                    kind[i] = true;
                else
                    kind[i] = false;
            }
            return kind;
        }

        public void SetColorSelsectItem(int index)
        {
            sumnailimgArray[index].SelectItem = true;
        }

        public void ClearColorSelsectItem(int index)
        {
            sumnailimgArray[index].SelectItem = false;
        }

        public void ClearColorSelsectItemAll()
        {
            for (int i = 0; sumnailimgArray.Length > i; i++)
            {
                sumnailimgArray[i].SelectItem = false;
            }
        }
        public bool CheckColorSelectItem(int index)
        {
            return sumnailimgArray[index].SelectItem;

        }

        public bool ProcessShortcutKey(Keys keyData)
        {
            if (shortcutKeyHelper1.PerformClickByKeys(keyData))
            {
                return true;
            }
            return false;
        }

        public void ButtonEnable(bool bCheck)
        {
            btnBack.Enabled = bCheck;
            btnNext.Enabled = bCheck;
            btnFirst.Enabled = bCheck;
            btnLast.Enabled = bCheck;

        }

        private void dgvOnOffZone_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex < 0)
            {
                return;
            }

            if (GetONOFF(dgvOnOffZone, e.RowIndex) == _strON)
            {
                SetOFF(dgvOnOffZone, e.RowIndex);
            }
            else if (GetONOFF(dgvOnOffZone, e.RowIndex) == _strOFF)
            {
                SetON(dgvOnOffZone, e.RowIndex);
            }
            else
            {
                SetNONE(dgvOnOffZone, e.RowIndex);
            }

            if (GetONOFF(dgvOnOffZone, e.RowIndex) != _strNONE)
            {
                EventFilterUpdate();
            }
        }

        private void dgvOnOffSide_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex < 0)
            {
                return;
            }

            if (GetONOFF(dgvOnOffSide, e.RowIndex) == _strON)
            {
                SetOFF(dgvOnOffSide, e.RowIndex);
            }
            else if (GetONOFF(dgvOnOffSide, e.RowIndex) == _strOFF)
            {
                SetON(dgvOnOffSide, e.RowIndex);
            }
            else
            {
                SetNONE(dgvOnOffSide, e.RowIndex);
            }

            if (GetONOFF(dgvOnOffSide, e.RowIndex) != _strNONE)
            {
                EventFilterUpdate();
            }
        }

        private void dgvOnOffKind_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex < 0)
            {
                return;
            }

            if (GetONOFF(dgvOnOffKind, e.RowIndex) == _strON)
            {
                SetOFF(dgvOnOffKind, e.RowIndex);
            }
            else if (GetONOFF(dgvOnOffKind, e.RowIndex) == _strOFF)
            {
                SetON(dgvOnOffKind, e.RowIndex);
            }
            else
            {
                SetNONE(dgvOnOffKind, e.RowIndex);
            }

            if (GetONOFF(dgvOnOffKind, e.RowIndex) != _strNONE)
            {
                if (OnRefreshThumbnail != null)
                    OnRefreshThumbnail(this, e);
                EventFilterUpdate();
            }
        }
    }
}
