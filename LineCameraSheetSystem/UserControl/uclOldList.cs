using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using ResultActionDataClassNameSpace;
using Fujita.InspectionSystem;
using Fujita.Misc;

namespace LineCameraSheetSystem
{
    public partial class uclOldList : UserControl　,IShortcutClient
    {
        public uclOldList()
        {
            InitializeComponent();

            //仮想モードにする
            dgvOldList.VirtualMode = true;

            shortcutKeyHelper1.SetShortcutKeys(btnOpen, Keys.O);
            shortcutKeyHelper1.SetShortcutKeys(btnDelete, Keys.D);
            shortcutKeyHelper1.SetShortcutKeys(btnOutResult, Keys.U);

            if (SystemParam.GetInstance().LotNoEnable == false)
            {
                dgvOldList.Columns[2].Width += dgvOldList.Columns[3].Width;
                dgvOldList.Columns[3].Visible = false;
            }
        }

        //MainForm
        MainForm _mainForm { get; set; }
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;
            //過去リストの作成
            GenResultListVirtual();
        }


        private void uclOldList_Load(object sender, EventArgs e)
        {
           // GenResultList();
          //  GenResultListVirtual();
      
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.OpenOldProduct();
        }

        private void OpenOldProduct()
        {
            string stSelrct;
            int iSelect;

            if (dgvOldList.CurrentRow == null)
            {
                return;
            }

            if (dgvOldList.SelectedRows.Count > 1)  //V1268 moteki
            {
                System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
                System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();

                frmMessageForm frmMsg = new frmMessageForm("複数選択されています。\r\n選択を1つに変更してください。", MessageType.Error);
                frmMsg.Text = mainAssemName.Name;
                frmMsg.Show();
                return;
            }

            LogingDllWrap.LogingDll.Loging_SetLogString("(ActionCheck):過去のリストを開いた");

            DataGridViewSelectedRowCollection selRow = dgvOldList.SelectedRows;
            iSelect = selRow[0].Index;      //V1263 moteki  
            //iSelect = dgvOldList.CurrentRow.Index;
            stSelrct = dgvOldList[6, iSelect].Value.ToString();

            //stSelrct = dgvOldList[1, iSelect].Value.ToString() + "_" + dgvOldList[2, iSelect].Value.ToString() + "_" +
            //    dgvOldList[3, iSelect].Value.ToString() + "_" + dgvOldList[4, iSelect].Value.ToString() + "_" + dgvOldList[5, iSelect].Value.ToString();
            SystemStatus.GetInstance().DataDispMode = SystemStatus.ModeID.Old;
            if (_mainForm.GenOldResultData(stSelrct))
            {
                // SystemStatus.GetInstance().DataDispMode = SystemStatus.ModeID.Old;
                // _mainForm.ChangeBtnName(SystemStatus.ModeID.Old);
                _mainForm.ChangeBtnName();


            }
            else
            {
                SystemStatus.GetInstance().DataDispMode = SystemStatus.ModeID.Real;
                return;
            }
        }
   
        private string StringToTime(string stTime)
        {
            stTime = stTime.Substring(0, 4) + "/" + stTime.Substring(4, 2) + "/" + stTime.Substring(6, 2) + " " +
                       stTime.Substring(8, 2) + ":" + stTime.Substring(10, 2) + ":" + stTime.Substring(12, 2);

            return stTime;        
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //string stSelrct;
            int iSelect;

            if (dgvOldList.CurrentRow == null)
            {
                return;

            }
    #region //1つのみ選択(コメントアウト)
            //if (dgvOldList.SelectedRows.Count > 1)  //V1268 moteki
            //{
            //    System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //    System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();

            //    frmMessageForm frmMsg = new frmMessageForm("複数選択されています。\r\n選択は1つに変更してください。", MessageType.Error);
            //    frmMsg.Text = mainAssemName.Name;
            //    frmMsg.Show();
            //    return;
            //}

            // DataGridViewSelectedRowCollection selRow = dgvOldList.SelectedRows;
            // iSelect = selRow[0].Index;      //V1263 moteki
            // //iSelect = dgvOldList.CurrentRow.Index;
            // stSelrct = dgvOldList[6, iSelect].Value.ToString();
            // DialogResult dialogresult;

            //// dialogresult = MessageBox.Show("選択したデータを消しますか？", "確認", MessageBoxButtons.OKCancel);
            // dialogresult = Utility.ShowMessage(_mainForm, "選択したデータを消しますか？", MessageType.YesNo);

            // if(dialogresult == DialogResult.Yes)
            // {
            //     if (Directory.Exists(stSelrct))
            //     {
            //         //選択しているデータを消す
            //         Directory.Delete(stSelrct, true);
            //     }
            //     //GenResultList();
            //     GenResultListVirtual();
            //     if (dgvOldList.RowCount > iSelect)
            //     {

            //         dgvOldList.Rows[iSelect].Cells[0].Selected = true;
            //     }
            //     else
            //     {
            //         if(iSelect>0)       //V1263 moteki
            //             dgvOldList.Rows[iSelect-1].Cells[0].Selected = true;
            //     }
            // }
    #endregion  //

            //複数選択対応
            DialogResult dialogresult;
            // dialogresult = MessageBox.Show("選択したデータを消しますか？", "確認", MessageBoxButtons.OKCancel);
            dialogresult = Utility.ShowMessage(_mainForm, "選択したデータを消しますか？\r\n合計："+dgvOldList.SelectedRows.Count.ToString()+"件", MessageType.YesNo);

            if (dialogresult == DialogResult.Yes)
            {
                DataGridViewSelectedRowCollection selRow = dgvOldList.SelectedRows;
                iSelect = selRow[0].Index;
                string[] stArray = new string[selRow.Count];

                for (int i = 0; i < selRow.Count; i++)
                {
                    int iIndex = selRow[i].Index;      //V1263 moteki
                    //iSelect = dgvOldList.CurrentRow.Index;
                    stArray[i] = dgvOldList[6, iIndex].Value.ToString();
                    iSelect = iIndex;
                }

                foreach (string st in stArray)
                {
                    if (Directory.Exists(st))
                    {
                        //選択しているデータを消す
                        Directory.Delete(st, true);
                    }
                }
                
                //GenResultList();
                GenResultListVirtual();
                dgvOldList.ClearSelection();
                if (dgvOldList.RowCount > iSelect)
                {
                    dgvOldList.Rows[iSelect].Cells[0].Selected = true;
                }
                else
                {
                    if (dgvOldList.RowCount > 0)       //V1263 moteki                    
                        dgvOldList.Rows[dgvOldList.RowCount - 1].Cells[0].Selected = true;
                }
            }
        }

        private void uclOldList_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {            
                dgvOldList.Select();
                dgvOldList.Focus();

                if (dgvOldList.SelectedRows.Count > 0)
                {
                    dgvOldList.Rows[dgvOldList.SelectedRows[0].Index].Selected = true;
                    dgvOldList.CurrentCell = dgvOldList.Rows[dgvOldList.SelectedRows[0].Index].Cells[0];
                    dgvOldList.CurrentRow.Cells[0].Selected = true;
                }
                
                //リスト更新
                //GenResultList();
      //          GenResultListVirtual();
            }
        }



        List<string[]> OldProduct = new List<string[]>();
        public void GenResultListVirtual()
        {
            chkHinsyu.Checked = false;
            chkMonth.Checked = false;
            chkMultiSelect.Checked = false;          

            //フォルダ名取得　　(using System.IOを追加した)
            string[] dirs = Directory.GetDirectories(SystemParam.GetInstance().ProductFolder);
            int i = 0;
            string[] stSplit;
           

            dgvOldList.Rows.Clear();
            OldProduct.Clear();

            foreach (string datesDir in dirs)
            {
                string[] datesDirs = Directory.GetDirectories(datesDir);
                foreach (string stResult in datesDirs)
                {
                    stSplit = stResult.Split('\\');
                    stSplit = stSplit[stSplit.Length - 1].Split('_');

                    string sNGCount;
                    if (stSplit.Length == 6)
                        sNGCount = stSplit[5];
                    else
                        sNGCount = GetNgCount(Path.Combine(stResult, "Result.txt"));

                    string[] item = new string[]{(i + 1).ToString("D4"),
                                                    StringToTime(stSplit[0]),
                                                    stSplit[1],stSplit[2],
                                                    StringToTime(stSplit[3]),
                                                    (Convert.ToDouble(stSplit[4]) / 1000).ToString(SystemParam.GetInstance().LengthDecimal),
                                                    stResult,
                                                    sNGCount };
                    i++;
                    OldProduct.Add(item);
                }
            }

            dgvOldList.RowCount = OldProduct.Count;

            for (i = 0; i < dgvOldList.Rows.Count; i++)
                dgvOldList.Rows[i].Height = 50;

            dgvOldList.ClearSelection();

            if (dgvOldList.Rows.Count > 0)
            {
                //一番下までスクロールする
                dgvOldList.FirstDisplayedScrollingRowIndex = OldProduct.Count - 1;
                //一番下を選択する
                dgvOldList.Rows[OldProduct.Count - 1].Selected = true;
            }
        }

        private string GetNgCount(string sPath)
        {
            string sNgCnt = "";
            using (FileStream fs = new FileStream(sPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("Shift-JIS")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] s = line.Split('\t');
                        if (s[0] == "060")
                        {
                            int iNgCnt;
                            if (int.TryParse(s[2], out iNgCnt))
                                sNgCnt = iNgCnt.ToString();
                            break;
                        }
                    }
                }
            }
            return sNgCnt;
        }
        private void dgvOldList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (OldProduct.Count > 0)
            {
                e.Value = OldProduct[e.RowIndex][e.ColumnIndex].ToString();

            }
        }

        public bool ProcessShortcutKey(Keys keyData)
        {
            if (shortcutKeyHelper1.PerformClickByKeys(keyData))
            {
                return true;
            }
            return false;
        }

        private void dgvOldList_DoubleClick(object sender, EventArgs e)
        {
            this.OpenOldProduct();
        }

        public string GetSelectRow()
        {
            string stName ="";
            for (int i = 0; dgvOldList.ColumnCount-1 > i; i++)
            {
                if (i != 0)
                {
                    stName += "_";
                }
              stName +=  dgvOldList.CurrentRow.Cells[i].Value.ToString() ;
            }
            

            return stName;
        }
        /// <summary>
        /// 結果出力の強制終了
        /// </summary>
        bool _bOutputAbort = false;

        private void btnOutResult_Click(object sender, EventArgs e) //V1263 moteki
        {                     
            string stResultFolder;
            int iSelect;

            if (dgvOldList.CurrentRow == null)
            {
                return;
            }

            DataGridViewSelectedRowCollection selRow = dgvOldList.SelectedRows;
            iSelect = selRow[0].Index;
            //iSelect = dgvOldList.CurrentRow.Index;
            stResultFolder = dgvOldList[6, iSelect].Value.ToString();
            string stImageFolsr = SystemParam.GetInstance().ImageFolder;

            string stText = "";
            if (selRow.Count == 1)
            {
                stText =
                "検査品種:" + dgvOldList[2, iSelect].Value.ToString() + "\r\n" +
                "検査開始日:" + dgvOldList[1, iSelect].Value.ToString() + "\r\n" +
                "検査終了日:" + dgvOldList[4, iSelect].Value.ToString() + "\r\n";
            }
            else
            {
                if (chkHinsyu.Checked)
                {
                    stText = "検査品種:" + dgvOldList[2, iSelect].Value.ToString() + "\r\n" ;
                }
                if(chkMonth.Checked)
                {
                    stText += "検査開始日:" + dgvOldList[1, iSelect].Value.ToString().Substring(0, 7) + "\r\n";
                }
                    
                //"検査終了日:" + dgvOldList[4, iSelect].Value.ToString() + "\r\n" +
                stText += "合計：" + selRow.Count.ToString() + "件";
            }

            System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();

            frmListSelect frmSelect = new frmListSelect(stText);
            frmSelect.Text = mainAssemName.Name;
            DialogResult dres = frmSelect.ShowDialog();
            if (dres == System.Windows.Forms.DialogResult.OK)
            {
                _bOutputAbort = false;
                Cursor.Current = Cursors.WaitCursor;
                Action act = new Action(() =>
                {
                    string stDrive = frmSelect._drivePath;
                    SystemParam.GetInstance().OutDrive = stDrive;

                    ResultActionDataClass resadc = new ResultActionDataClass();
                    resadc.SystemImageDir = SystemParam.GetInstance().ImageFolder;
                    resadc.SystemResultDir = SystemParam.GetInstance().ProductFolder;

                    for (int i = 0; i < selRow.Count; i++)
                    {
                        if(_bOutputAbort)
                        {
                            resadc.Clear();
                            resadc.Dispose();
                            return;
                        }

                        iSelect = selRow[i].Index;
                        stResultFolder = dgvOldList[6, iSelect].Value.ToString();

                        //過去ファイルロード
                        if (!resadc.Load(stResultFolder))
                        {
                            //Utility.ShowMessage(this, "過去のデータの読み込みができません。", MessageType.Error);                        
                            frmMessageForm frmMsg = new frmMessageForm("過去のデータの読み込みができません。", MessageType.Error);
                            frmMsg.Text = mainAssemName.Name;
                            frmMsg.ShowDialog();
                        }
                        else
                        {
                            try
                            {
                                //出力先にコピーする                   
                                string stFolderName = Path.GetFileName(stResultFolder);
                                DirectoryCopy(stResultFolder, stDrive + stFolderName);

                                //出力先に画像をコピーする
                                ImageCopy(resadc, stImageFolsr, stResultFolder, stDrive, frmSelect.CtrlKeyPress);
                            }
                            catch (Exception ee)
                            {
                                //Utility.ShowMessage(this, ee.Message, MessageType.Error);                            
                                frmMessageForm frmMsg = new frmMessageForm(ee.Message, MessageType.Error);
                                frmMsg.Text = mainAssemName.Name;
                                frmMsg.TopMost = true;
                                frmMsg.ShowDialog();

                                resadc.Clear();
                                resadc.Dispose();
                                return;
                            }
                        }

                        resadc.Clear();
                        resadc.Dispose();
                    }
                });
                Action actAbort = new Action(() =>
                {
                    _bOutputAbort = true;
                });
                frmProgressForm frmProg = new frmProgressForm(act, actAbort);
                frmProg.Description = "データを出力しています\nしばらくお待ちください";
                frmProg.ShowDialog(this);
                Cursor.Current = Cursors.Default;
            }           
        }
     
        /// <summary>
        /// フォルダコピー
        /// </summary>
        /// <param name="sourcePath"> コピー元パス</param>
        /// <param name="destinationPath">コピー先パス</param>
        private void DirectoryCopy(string sourcePath, string destinationPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);

            //コピー先のディレクトリがなければ作成する
            if (destinationDirectory.Exists == false)
            {
                destinationDirectory.Create();
                destinationDirectory.Attributes = sourceDirectory.Attributes;
            }

            //ファイルのコピー
            foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
            {
                //同じファイルが存在していたら、常に上書きする
                fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach (System.IO.DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
            {
                DirectoryCopy(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
            }
        }
        /// <summary>
        /// 画像をコピーする
        /// </summary>
        /// <param name="resadc">結果データの集まりクラス</param>
        /// <param name="stImageFolder">結果画像が保存されているフォルダパス</param>
        /// <param name="stResultFolder">結果データのフォルダ</param>
        /// <param name="stDrive">出力先のドライブ</param>
        /// <param name="bCtrlKey">コントロールキーは押されているか</param>
        private void ImageCopy(ResultActionDataClass resadc,string stImageFolder,string stResultFolder,string stDrive,bool bCtrlKey)
        {                       
            List<ResActionData> listResAc = resadc.GetNgDatas();

            foreach (ResActionData res in listResAc)
            {
                if(_bOutputAbort)
                   return; 
                               
                string stImagePath = Path.Combine(stImageFolder, res.ImageFileName);
                string stFileName = Path.GetFileName(res.ImageFileName);
                string stFolder = Path.GetFileName(stResultFolder);
                string stPastePath = Path.Combine(stDrive, stFolder, stFileName);

                string stDirectory = Path.GetDirectoryName(stImagePath);
                string stFileNameWithOutExt = Path.GetFileNameWithoutExtension(res.ImageFileName);
                string stExtention = Path.GetExtension(res.ImageFileName);
                string stPastePath2 = Path.Combine(stDrive, stFolder);

                string[] stFileName2;
                if (bCtrlKey)
                {
                    //画像名"a","b","c"を付加して作成
                    stFileName2 = new string[4];
                    stFileName2[0] = stFileName;
                    stFileName2[1] = stFileNameWithOutExt + "a" + stExtention;
                    stFileName2[2] = stFileNameWithOutExt + "b" + stExtention;
                    stFileName2[3] = stFileNameWithOutExt + "c" + stExtention;
                }
                else
                {
                    //カラー画像のみ出力
                    stFileName2 = new string[1];
                    stFileName2[0] = stFileName;
                }

                for (int i = 0; i < stFileName2.Length; i++)
                {
                    string stCombine = Path.Combine(stDirectory, stFileName2[i]);
                    string stCombine2 = Path.Combine(stPastePath2, stFileName2[i]);
                    if (File.Exists(stCombine))
                    {
                        //stImagePathをststResultFolderの中にコピー
                        FileInfo fi = new FileInfo(stCombine);
                        FileInfo copyFi = fi.CopyTo(stCombine2, true); //上書きtrue                                          
                    }                    
                }
            }                                   
        }
        /// <summary>
        /// 結果リストにフィルターをかける
        /// </summary>
        /// <param name="SelectAll">全選択の場合Trueを入れる</param>
        private void ResulFilter(bool SelectAll)  //V1268 moteki
        {
            int iSelect;

            if (dgvOldList.CurrentRow == null)
            {
                return;
            }
            
            DataGridViewSelectedRowCollection selRow = dgvOldList.SelectedRows;
            iSelect = selRow[selRow.Count-1].Index;
            string stSelDate = selRow[selRow.Count - 1].Cells[1].Value.ToString().Substring(0, 7);
            string stSelHinsyu = selRow[selRow.Count - 1].Cells[2].Value.ToString();

            dgvOldList.ClearSelection();
            dgvOldList[0, iSelect].Selected = true;

           // string st = dgvOldList[1, 1].Value.ToString();
            List<int> listIndex = new List<int>();

            if (SelectAll)
            {
                dgvOldList.SelectAll();
                return;
            }

            for (int i = 0; i < dgvOldList.RowCount; i++)
            {
                string stDate = dgvOldList[1, i].Value.ToString();
                string stHinsyu = dgvOldList[2, i].Value.ToString();
              
                    if ((stDate.IndexOf(stSelDate) > -1 || !chkMonth.Checked) && (stHinsyu == stSelHinsyu || !chkHinsyu.Checked))
                    {
                        dgvOldList[0, i].Selected = true;
                    }                
            }            
        }

        private void chkMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonth.Checked || chkHinsyu.Checked && !_bCellClick)
            {
                chkMultiSelect.Checked = false;
                ResulFilter(false);
            }
        }

        private void chkHinsyu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHinsyu.Checked || chkMonth.Checked && !_bCellClick)
            {
                chkMultiSelect.Checked = false;
                ResulFilter(false);
            }
        }
      
        /// <summary>
        /// 過去リストのセルがクリックされたフラグ
        /// </summary>
        bool _bCellClick = false;
        private void dgvOldList_CellClick(object sender, DataGridViewCellEventArgs e)
        {                           
            _bCellClick = true;

            chkMonth.Checked = false;
            chkHinsyu.Checked = false;            

            _bCellClick = false;
          
            MultiSelectCheck();            
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            chkHinsyu.Checked = false;
            chkMonth.Checked = false;
            chkMultiSelect.Checked = false;         

            ResulFilter(true);
        }
        /// <summary>
        /// 複数選択時の選択されたリストのインデックス保持用
        /// </summary>
        List<int> listSelRowMulti ;
        private void chkMultiSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMultiSelect.Checked)
            {
                chkHinsyu.Checked = false;
                chkMonth.Checked = false;

                listSelRowMulti = new List<int>();
                if (dgvOldList.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvOldList.SelectedRows.Count; i++)
                    {
                        listSelRowMulti.Add(dgvOldList.SelectedRows[i].Index);
                    }
                }
            }
            else
            {
                dgvOldList.NoUpdate = false;

                if (listSelRowMulti != null)
                {
                    listSelRowMulti.Clear();
                    listSelRowMulti = null;
                }             
            }            
        }            
        /// <summary>
        /// 複数選択のチェック時の行選択の時の処理
        /// </summary>
        private void MultiSelectCheck()
        {
            //複数選択チェックON
            if (chkMultiSelect.Checked)
            {
                if (dgvOldList.SelectedRows.Count > 0)
                {
                    int iIndex = dgvOldList.SelectedRows[0].Index;
                    int iFind = listSelRowMulti.FindIndex(x => x == iIndex);

                    if (-1 == iFind)
                    {
                        listSelRowMulti.Insert(0, iIndex);
                    }
                    else
                    {
                        if (listSelRowMulti.Count > 1)
                            listSelRowMulti.RemoveAt(iFind);
                    }

                    dgvOldList.ClearSelection();
                    foreach (int i in listSelRowMulti)
                    {
                        dgvOldList[0, i].Selected = true;
                    }
                }                
            }

            dgvOldList.NoUpdate = false;
        }

        
        private void dgvOldList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //このイベントの後にアップデートしないようにする　選択が一度解除されることを回避している
            if(chkMultiSelect.Checked)
                dgvOldList.NoUpdate = true;
        }

        private void dgvOldList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dgvOldList.NoUpdate = false;
        }    

        private void dgvOldList_VisibleChanged(object sender, EventArgs e)
        {
            dgvOldList.NoUpdate = false;
            chkHinsyu.Checked = false;
            chkMonth.Checked = false;
            chkMultiSelect.Checked = false;
        }

        private void btnListDown_Click(object sender, EventArgs e)
        {
            int max = dgvOldList.Rows.Count;
            int pos = dgvOldList.FirstDisplayedScrollingRowIndex + 13;
            dgvOldList.FirstDisplayedScrollingRowIndex = (pos < max) ? pos : (max - 1);
        }

        private void btnListUp_Click(object sender, EventArgs e)
        {
            int max = dgvOldList.Rows.Count;
            int pos = dgvOldList.FirstDisplayedScrollingRowIndex - 13;
            dgvOldList.FirstDisplayedScrollingRowIndex = (pos > 0) ? pos : 0;

        }
    }
    /// <summary>
    /// タッチパネルで複数選択用に作ったDataGridView
    /// </summary>
    class DataGridViewMulti:DataGridView
    {
        public DataGridViewMulti():base()
        {
            NoUpdate = false;
        }
        /// <summary>
        /// アップデートしないフラグ、true：アップデートしない
        /// </summary>
        public bool NoUpdate { get; set; }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!NoUpdate)
            {
                base.OnPaint(e);
            }
            else
            {

            }
        }

    }
}
