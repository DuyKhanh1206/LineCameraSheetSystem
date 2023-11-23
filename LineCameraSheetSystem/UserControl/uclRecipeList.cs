using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Fujita.Misc;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class uclRecipeList : UserControl ,IShortcutClient
    {
        public uclRecipeList()
        {
            InitializeComponent();
        }

        // キー入力制限
        clsTextboxKeyPressMask _keyMask = null;

        //MainForm
        MainForm _mainForm { get; set; }
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;

            int i;
            string[] stName = { "", "", "" };

            if (!Directory.Exists(SystemParam.GetInstance().RecipeFoldr))
            {
                Directory.CreateDirectory(SystemParam.GetInstance().RecipeFoldr);
            }
            string[] dirs = Directory.GetDirectories(SystemParam.GetInstance().RecipeFoldr);

            //listviewに256個行を作る
            for (i = 1; AppData.ENTRY_COUNT > i; i++)
            {
                stName[0] = i.ToString("D3");
                listviewKindName.Items.Insert(i - 1, new ListViewItem(stName));

            }

            //レシピフォルダを読み込み名前を入れる
            for (i = 0; dirs.Length >= i; i++)
            {
                IniFileAccess ini = new IniFileAccess();

                if (dirs.Length > i)
                {
                    string kindpath = "";
                    kindpath = dirs[i].ToString();

                    FolderKindName(kindpath, out stName[1]);

                    stName[0] = kindpath.Remove(0, kindpath.Length - 3);

                    //stName[0] = ini.GetIniString("recipe", "No", dirs[i] + "\\" + AppData.RCP_FILE, "999");
                    //stName[1] = ini.GetIniString("recipe", "KindName", dirs[i] + "\\" + AppData.RCP_FILE, "");

                }
                listviewKindName.Items.RemoveAt(Convert.ToInt32(stName[0]) - 1);
                listviewKindName.Items.Insert(Convert.ToInt32(stName[0]) - 1, new ListViewItem(stName));
            }
            ItemIndex = -1;

            _keyMask = new clsTextboxKeyPressMask(new KeyPressMask_InvalidFileCharUnderBar());
            _keyMask.SetTextBox(textEditBox);

            //shortcutKeyHelper1.SetShortcutKeys(btnSelect, Keys.F10 | Keys.Control);
            shortcutKeyHelper1.SetShortcutKeys(btnDelete, Keys.D);
            shortcutKeyHelper1.SetShortcutKeys(btnCopy, Keys.C);
            shortcutKeyHelper1.SetShortcutKeys(btnPaste, Keys.P);

        }

        //レシピフォルダからレシピファイルの品種名を読み込む
        private void FolderKindName(string folder, out string kindname)
        {
            Recipe recipe = Recipe.GetInstance();
            kindname = recipe.LoadKindName(folder + "\\" + AppData.RCP_FILE);
        }

        private DateTime MessagePreTime=DateTime.Now; //V1057 手動外部修正 yuasa 20190122：外部信号引数追加
        public bool SetRecipe(bool extSignal=false) //V1057 手動外部修正 yuasa 20190122：外部信号引数追加
        {
            bool result = false;
            if (ItemIndex >= 0)
            {

                Recipe recipe = Recipe.GetInstance();
                //if (recipe.KindName == "未登録" || recipe.KindName == "Default")
                if (recipe.KindName == "未登録")
                {
                    if (!extSignal || DateTime.Now > MessagePreTime.AddSeconds(5)) //V1057 手動外部修正 yuasa 20190122：/外部信号ではない、もしくは前回より5秒以上経過
                    {
                        Utility.ShowMessage(_mainForm, "品種データがありません", MessageType.Error);
                        MessagePreTime = DateTime.Now; //V1057 手動外部修正 yuasa 20190122：前回時間代入
                    }
                    
                }
                else
                {
                    SystemStatus.GetInstance().SelectRecipe = true;
                    //  _mainForm.InspectionRecipeCopy();
                  //  _mainForm.StandbyInsp();

                    ////        _mainForm.UclSheetMapReal.Zones = dZone;
                    // _mainForm.UclSheetMapReal.SheetWidth = recipe.Width;
                    result = true;
                }

            }
            else
            {
                if (!extSignal || DateTime.Now > MessagePreTime.AddSeconds(5)) //V1057 手動外部修正 yuasa 20190122：/外部信号ではない、もしくは前回より5秒以上経過
                {
                    Utility.ShowMessage(this, "品種が選択されていません", MessageType.Error);
                    MessagePreTime = DateTime.Now; //V1057 手動外部修正 yuasa 20190122：前回時間代入
                }

            }
            return result;
        }

        /// <summary>
        //品種リスト
        /// </summary>
        public List<string> KindName = new List<string>();

        //ﾘｽﾄ右の番号
        public string selectItem { get; set; }

        //品種リストが選択された
        private void listviewKindName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_mainForm == null)
            {
                return;
            }

            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                _mainForm.RecipeContentsEnabled();

                _mainForm.ChangeRecipeMessage();

                if (listviewKindName.FocusedItem == null)
                {
                    ItemIndex = -1;
                }
                else
                {
                    ItemIndex = listviewKindName.FocusedItem.Index;
                }

                string Kind;
                int ind;

                if (listviewKindName.FocusedItem != null)
                {

                    ind = listviewKindName.FocusedItem.Index;
                    Kind = listviewKindName.FocusedItem.Text;
                    this.selectItem = Kind;

                    Recipe recpe = Recipe.GetInstance();
                    if (recpe.Load(Kind))
                    {
                        _mainForm.SetRirekiCount();

                        //キャンセル用のレシピコピー
                        _mainForm.CancelRecipe = recpe.Copy();
                        ////        _mainForm.UclRecipeContentsReal.RecipeDisp();
                        _mainForm.SetRecipeDispReal();

                        //レシピ編集フラグ
                        SystemStatus.GetInstance().RecipeEdit = false;
                        _mainForm.ClearRecipeEdit();

                    }
                    else
                    {
                        Utility.ShowMessage(_mainForm, "品種ロードエラー", MessageType.Error);
                    }
                }
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        //ダブルクリックで品種名編集
        private void listviewKindName_DoubleClick(object sender, EventArgs e)
        {
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection || SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
            {
                if (_mainForm.InspRecipe.KindName == listviewKindName.FocusedItem.SubItems[1].Text)
                {
                    Utility.ShowMessage(_mainForm, "検査に使用している品種は変更できません。", MessageType.Error);
                    return;
                }
            }

            ForcusEditBox();

/*
            listviewKindName.Enabled = false;

            ItemIndex = listviewKindName.FocusedItem.Index;
            
            Point pointEditBox = new Point();
            Point pointListView = new Point();
            pointListView = listviewKindName.Location;

            pointEditBox.X = listviewKindName.FocusedItem.Position.X + 82;
            pointEditBox.Y = listviewKindName.FocusedItem.Position.Y + pointListView.Y + 4;
            textEditBox.Location = pointEditBox;

            textEditBox.Text = listviewKindName.FocusedItem.SubItems[1].Text;

            textEditBox.Visible = true;
            textEditBox.Focus();
 */
        }

        private void ForcusEditBox()
        {
            listviewKindName.Enabled = false;

            ItemIndex = listviewKindName.FocusedItem.Index;

            Point pointEditBox = new Point();
            Point pointListView = new Point();
            pointListView = listviewKindName.Location;

            pointEditBox.X = listviewKindName.FocusedItem.Position.X + 65;
            pointEditBox.Y = listviewKindName.FocusedItem.Position.Y + pointListView.Y + 4;
            textEditBox.Location = pointEditBox;

            textEditBox.Text = listviewKindName.FocusedItem.SubItems[1].Text;

            textEditBox.Visible = true;
            textEditBox.Focus();
        }

        //選んだアイテムのインデックス
        private int ItemIndex { get; set; }

        //textEditBoxのフォーカスが外れたら
        private void textEditBox_Leave(object sender, EventArgs e)
        {
            if (textEditBox.Text != "")
            {
                if (textEditBox.Text == "未登録")
                {
                    Utility.ShowMessage(_mainForm,"品種名に「未登録」は使えません。", MessageType.Error);
                    ForcusEditBox();
                    return;
                }

            
                char[] invalidChars = Path.GetInvalidFileNameChars();
                char[] chU = new char[]{'_'};
                //ファイル名に使えるかチェック

                char[] invalidCharsU = invalidChars.Concat(chU).ToArray();

                for (int i = 0; AppData.ENTRY_COUNT-1 > i; i++)
                {
                    if (listviewKindName.Items[i].SubItems[1].Text != "")
                    {
                        if (listviewKindName.FocusedItem.Index != i)
                        {
                            if (listviewKindName.Items[i].SubItems[1].Text == textEditBox.Text)
                            {
                                Utility.ShowMessage(_mainForm, "同じ名前が存在します。", MessageType.Error);
                                ForcusEditBox();
                                return;
                            }
                        } 
                    }
               
                }

                if (textEditBox.Text.Length > 20)
                {
                    Utility.ShowMessage(this, "品種名は20文字までです。", MessageType.Error);

                   // textEditBox.Text = textEditBox.Text.Remove(20);
                   // textEditBox.Select(textEditBox.Text.Length, 0);
                    ForcusEditBox();
                    return;
                }

                if (textEditBox.Text.IndexOfAny(invalidCharsU) < 0)
                {
                    listviewKindName.Items[ItemIndex].SubItems[1].Text = textEditBox.Text;

                    Recipe recipe = Recipe.GetInstance();
                    recipe.KindName = textEditBox.Text;

                    recipe.SelectItem = this.selectItem;

                    textEditBox.Text = "";

                    recipe.SaveKindName();


                    //テキストボックスを消す
                    textEditBox.Visible = false;
                    listviewKindName.Enabled = true;
                               
                    string Kind;
                    Kind = listviewKindName.FocusedItem.Text;
                    recipe.Path =SystemParam.GetInstance().RecipeFoldr+ Kind + "\\" + AppData.RCP_FILE;
                   　//フォルダがなかったら作って保存
                    if (System.IO.File.Exists(recipe.Path) == false)
                    {
                        int length;
                        string path = "";

                        length = recipe.Path.Length - AppData.RCP_FILE.Length;
                        path = recipe.Path.Remove(length, AppData.RCP_FILE.Length);

                        System.IO.Directory.CreateDirectory(path);
                        recipe.Save();
                        clsFilebackup.Backup(SystemParam.GetInstance().RecipeFoldr, SystemParam.GetInstance().BackupFolder);
                    
                        _mainForm.SetRecipeDispReal();
                    }
                    _mainForm.SetRecipeNameDispReal();
                 //   _mainForm.SetRecipeDispReal();

                }
                else
                {
                    Utility.ShowMessage(_mainForm, @"使用できない文字が含まれています　\/:*?<>|_", MessageType.Error);
                    ForcusEditBox();
                }
            }
            else
            {
                //テキストボックスを消す
                textEditBox.Visible = false;
                listviewKindName.Enabled = true;
            }

        }

        //品種の選択状態の解除
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _mainForm.ReleaseInspRecipe();
        }

        //選択したアイテムの削除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                if (listviewKindName.Items[ItemIndex].SubItems[1].Text == _mainForm.InspRecipe.KindName)
                {
                    Utility.ShowMessage(_mainForm, "検査に使用している品種は削除できません。", MessageType.Error);
                    return;
                }            
            }

            DialogResult dialogresult;
          
           // if (Directory.Exists(AppData.RCP_FOLDER + listviewKindName.Items[ItemIndex].Text))
            if (Directory.Exists(SystemParam.GetInstance().RecipeFoldr + listviewKindName.Items[ItemIndex].Text))
            {
                dialogresult = Utility.ShowMessage(_mainForm, "選択したデータを消しますか？", MessageType.YesNo);
                if (dialogresult == DialogResult.Yes)
                {
                  //  Directory.Delete(AppData.RCP_FOLDER + listviewKindName.Items[ItemIndex].Text, true);
                    Directory.Delete(SystemParam.GetInstance().RecipeFoldr + listviewKindName.Items[ItemIndex].Text, true);
                    listviewKindName.Items[ItemIndex].SubItems[1].Text = "";

                    UpdateRecipeList();

                    Recipe recipe = Recipe.GetInstance();
                    if (recipe.Load(recipe.SelectItem))
                    {
                        //レシピ編集フラグ
                        SystemStatus.GetInstance().RecipeEdit = false;
                        _mainForm.ClearRecipeEdit();

                        _mainForm.SetRecipeDispReal();
                        //キャンセル用のレシピコピー
                        _mainForm.CancelRecipe = recipe.Copy();
                    }
                    else
                    {
                        Utility.ShowMessage(this, "品種ロードエラー", MessageType.Error);
                    }
                }
            }
            else
            {
                listviewKindName.Items[ItemIndex].SubItems[1].Text = "";
            }
        }

        private void textEditBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listviewKindName.Enabled = true;
                listviewKindName.Focus();
            }
        }

        public void ChangeListItemName(string selItem , string name)
        {
            int i;

            i = Convert.ToInt32( selItem);

            listviewKindName.Items[i-1].SubItems[1].Text = name;
        }

        public void SelsectItem(string selItem)
        {
            int i;

            i = Convert.ToInt32( selItem);

            listviewKindName.Items[i - 1 ].Selected = false;
            listviewKindName.Items[i - 1].Selected = true;
        }

        private void uclRecipeList_Click(object sender, EventArgs e)
        {
            listviewKindName.Enabled = true;
            listviewKindName.Focus();

        }

        public int CheckCopyName(string name)
        {
            string st = name;
           // bool bb = true;
          //  int j = 1;

            if (listviewKindName.SelectedItems[0].SubItems[1].Text == st)
            {

                return -1;
            }

         //   while (bb)
         //   {
                for (int i = 0; AppData.ENTRY_COUNT - 1 > i; i++)
                {
                        
                   // if (listviewKindName.Items[i].SubItems[1].Text != "")
                   // {
                 //   if (listviewKindName.FocusedItem.Index != i)
                 //   {
                        if (listviewKindName.Items[i].SubItems[1].Text == st)
                        {
                            return 0;
                       
                           // st = name + "-コピー" + j.ToString();
                           // j++;

                            //i = -1;
                          
                          
                        }
                //    }

                   // }

                }

               // bb = false;
           // }
            
            return 1;
        }

        public string SelsectItemString()
        {
            if (listviewKindName.FocusedItem != null)
            {
                return listviewKindName.FocusedItem.SubItems[1].Text;
            }
            return "";
        }

        private void uclRecipeList_Load(object sender, EventArgs e)
        {
            
           
        }

        private void uclRecipeList_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (listviewKindName.SelectedIndices.Count > 0)
            {
                listviewKindName.Select();
                listviewKindName.Focus();
                return;
            }

            listviewKindName.Select();
            listviewKindName.Focus();
            if (listviewKindName.Items.Count!=0)
            {
                listviewKindName.Items[0].Focused = true;
                listviewKindName.Items[0].Selected = true;
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

        private void textEditBox_TextChanged(object sender, EventArgs e)
        {
            if (textEditBox.TextLength > 20)
            {
                textEditBox.Text = textEditBox.Text.Remove(20);
                textEditBox.Select(textEditBox.Text.Length, 0);
            }
        }

        public void UpdateRecipeList()
        {
            for (int i = 0; listviewKindName.Items.Count > i; i++)
            {

                if (_mainForm.InspRecipe != null && listviewKindName.Items[i].SubItems[1].Text == _mainForm.InspRecipe.KindName)
                {
                    listviewKindName.Items[i].SubItems[2].Text = "検";
                }
                else if (_mainForm.TempoRecipe != null && listviewKindName.Items[i].SubItems[1].Text == _mainForm.TempoRecipe.KindName)
                {
                    listviewKindName.Items[i].SubItems[2].Text = "コ";
                }
                else
                {
                    listviewKindName.Items[i].SubItems[2].Text = "";
                }
            }
        }

#if USE_OSK_KEYBOARD
        OSKController _OskCtrl = null;
#else
        Fujita.ScreenKeyboard.frmMain _keyboardForm = null;
#endif
        private void textEditBox_MouseClick(object sender, MouseEventArgs e)
        {
#if USE_OSK_KEYBOARD
#else
            Color colOld = textEditBox.BackColor;
            try
            {
                _keyboardForm = new Fujita.ScreenKeyboard.frmMain();
                _keyboardForm.strInputText = textEditBox.Text;
                textEditBox.BackColor = Color.Pink;

                if (DialogResult.OK != _keyboardForm.ShowDialog())
                    return;

                textEditBox.Text = _keyboardForm.strOutputText;
            }
            finally
            {
                textEditBox.BackColor = colOld;
            }
#endif
        }

        private void btnListDown_Click(object sender, EventArgs e)
        {
            int max = listviewKindName.Items.Count;
            int pos = listviewKindName.TopItem.Index + 18;
            listviewKindName.EnsureVisible(max - 1);
            listviewKindName.EnsureVisible((pos < max) ? pos : (max - 1));
        }

        private void btnListUp_Click(object sender, EventArgs e)
        {
            int max = listviewKindName.Items.Count;
            int pos = listviewKindName.TopItem.Index - 18;
            listviewKindName.EnsureVisible(max - 1);
            listviewKindName.EnsureVisible((pos > 0) ? pos : 0);
        }
    }
}
