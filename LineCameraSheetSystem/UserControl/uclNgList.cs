using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultActionDataClassNameSpace;
using Fujita.Misc;

namespace LineCameraSheetSystem
{
    public partial class uclNgList : UserControl ,IShortcutClient 
    {
        public uclNgList()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 30);
            listViewNGItem.SmallImageList = imgList;
        }

        MainForm _mainForm { get; set; }
         public void SetMainForm(MainForm _mf)
         {
             _mainForm = _mf;
         }

            
        //Listに追加する。
        public void ListAdd(string[] stItem)
        {        

              //  string[] item = { i.ToString(), i.ToString(), i.ToString(), i.ToString() };
          //  stItem[0] = (listViewNGItem.Items.Count+1).ToString();
            if (stItem[2] == ResultActionDataClass.EEventMode.Start.ToString() || 
                     stItem[2] == ResultActionDataClass.EEventMode.Stop.ToString() ||
                     stItem[2] == ResultActionDataClass.EEventMode.Suspend.ToString())
            {
                stItem[3] = "";
                stItem[4] = "";
                stItem[5] = "";
                stItem[6] = "";
               
            }

            //一番下の行に追加
          //  listViewNGItem.Items.Insert(listViewNGItem.Items.Count, new ListViewItem(stItem));
            listViewNGItem.Items.Add( new ListViewItem(stItem));
            System.Diagnostics.Debug.WriteLine("list count = " + listViewNGItem.Items.Count.ToString());

            if (!chkboxScrolRock.Checked)
            {
                //一番下までスクロールする
                listViewNGItem.EnsureVisible(listViewNGItem.Items.Count - 1);
            }
            else
            {
                if (listViewNGItem.SelectedIndices.Count > 0 )
                {
                    listViewNGItem.EnsureVisible(listViewNGItem.SelectedIndices[0]);
                }
            }
        }

        //Listの全消去
        public void ListClear()
        {
            listViewNGItem.Items.Clear();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        }

        //NGリストのkazuwo返す。
        public int NGListcount()
        {
            return listViewNGItem.Items.Count;
        }

        private void listViewNGItem_SelectedIndexChanged(object sender, EventArgs e)
        {
      //      if (listViewNGItem.SelectedItems.Count > 0)
      //      {
    //            int i = listViewNGItem.FocusedItem.Index;
      //          string ss = listViewNGItem.FocusedItem.Text;
               
      //      }
        }

        //スクロールロックのチェックをtrueにして非表示にする
        public void ScrollLockVisible()
        {
            chkboxScrolRock.Visible = false;
            chkboxScrolRock.Checked = true;
        }

        public class DoubleClickEventArgs : EventArgs
        {
            public ListView ListViewControl { get; private set; }

            public DoubleClickEventArgs(ListView listview)
            {
                ListViewControl = listview;
            }
        }

        public delegate void DoubleClickEventHandler(object sender, DoubleClickEventArgs e);
        public event DoubleClickEventHandler OnDoubleClickList;
        private void listViewNGItem_DoubleClick(object sender, EventArgs e)
        {
            if (listViewNGItem.SelectedIndices.Count > 0)
            {
                if (OnDoubleClickList != null)
                    OnDoubleClickList(this, new DoubleClickEventArgs(listViewNGItem));
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

        public void ShortcutkeyClearOld()
        {
            shortcutKeyHelper1.SetShortcutKeys(chkboxScrolRock, Keys.None);
        }

        private void chkboxScrolRock_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnListDown_Click(object sender, EventArgs e)
        {
            if (listViewNGItem.Items.Count >= 1)//v1327 チェック追加
            {
                int max = listViewNGItem.Items.Count;
                int pos = listViewNGItem.TopItem.Index + 22;
                listViewNGItem.EnsureVisible(max - 1);
                listViewNGItem.EnsureVisible((pos < max) ? pos : (max - 1));
            }
        }

        private void btnListUp_Click(object sender, EventArgs e)
        {
            if(listViewNGItem.Items.Count >= 1)//v1327 チェック追加
            {
                int max = listViewNGItem.Items.Count;
                int pos = listViewNGItem.TopItem.Index - 22;
                listViewNGItem.EnsureVisible(max - 1);
                listViewNGItem.EnsureVisible((pos > 0) ? pos : 0);
            }
        }
    }

    public class BufferdListView : ListView
    {
        protected override bool DoubleBuffered
        {
            get
            {
                return true;
            }
            set
            {
                
            }
        }
    }

}
