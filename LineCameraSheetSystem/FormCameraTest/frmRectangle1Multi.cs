using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fujita.Misc;
using ViewROI;

namespace Fujita.InspectionSystem
{

    public delegate void Rectangle1MultiUserSettingEventHandler(object sender, Rectangle1MultiUserSettingEventArgs e);

    public partial class frmRectangle1Multi : Form, ICallbackRoiRectangle1
    {
        public event Rectangle1MultiUserSettingEventHandler UserSettingChange;

        private List<CRectangle1> _lstRect1;
        CRectangle1 _rcInit;

        public frmRectangle1Multi()
        {
            InitializeComponent();

            setEvents();

        }

        public frmRectangle1Multi(List<CRectangle1> lstrect, CRectangle1 rcinit, string sMessage)
        {
            InitializeComponent();

            lblMessage.Text = sMessage;
            _lstRect1 = lstrect;
            _rcInit = new CRectangle1(rcinit);

            updateList();

        }

        private void setEvents()
        {
            nudRow1.ValueChanged += new EventHandler(nudVal_ValueChanged);
            nudCol1.ValueChanged += new EventHandler(nudVal_ValueChanged);
            nudRow2.ValueChanged += new EventHandler(nudVal_ValueChanged);
            nudCol2.ValueChanged += new EventHandler(nudVal_ValueChanged);
        }

        private void resetEvents()
        {
            nudRow1.ValueChanged -= new EventHandler(nudVal_ValueChanged);
            nudCol1.ValueChanged -= new EventHandler(nudVal_ValueChanged);
            nudRow2.ValueChanged -= new EventHandler(nudVal_ValueChanged);
            nudCol2.ValueChanged -= new EventHandler(nudVal_ValueChanged);
        }

        private void updateList( int iSelect = -1 )
        {
            int iSelectedIndex = -1;
            if (iSelect == -1 && lstPoints.SelectedIndices.Count > 0)
            {
                iSelectedIndex = lstPoints.SelectedIndices[0];
            }
            lstPoints.Items.Clear();

            for (int i = 0; i < _lstRect1.Count; i++)
            {
                lstPoints.Items.Add(new ListViewItem(new string[] 
                { 
                    (i + 1).ToString(), 
                    _lstRect1[i].Row1.ToString("F2"), 
                    _lstRect1[i].Col1.ToString("F2"), 
                    _lstRect1[i].Row2.ToString("F2"), 
                    _lstRect1[i].Col2.ToString("F2"), 
                }));
            }

            if (iSelectedIndex >= 0)
            {
                if (iSelectedIndex < lstPoints.Items.Count)
                {
                    lstPoints.Items[iSelectedIndex].Selected = true;
                }
                else if (lstPoints.Items.Count > 0)
                {
                    lstPoints.Items[lstPoints.Items.Count - 1].Selected = true;
                }
                else
                {
                }
            }
            else if (iSelect == -2)
            {
                lstPoints.Items[lstPoints.Items.Count - 1].Selected = true;
            }
            else if (iSelect >= 0 && iSelect < lstPoints.Items.Count)
            {
                lstPoints.Items[iSelect].Selected = true;
            }
            else
            {
                if (lstPoints.Items.Count > 0)
                {
                    lstPoints.Items[lstPoints.Items.Count - 1].Selected = true;
                }
            }

            // 選択中が画面に入るようにスクロール
            if (lstPoints.SelectedIndices.Count > 0)
                lstPoints.EnsureVisible(lstPoints.SelectedIndices[0]);

        }

        private bool updateListMono(int iIndex)
        {
            if (iIndex < 0 || iIndex >= lstPoints.Items.Count)
                return false;

            lstPoints.Items[iIndex].SubItems[1].Text = _lstRect1[iIndex].Row1.ToString("F2");
            lstPoints.Items[iIndex].SubItems[2].Text = _lstRect1[iIndex].Col1.ToString("F2");
            lstPoints.Items[iIndex].SubItems[3].Text = _lstRect1[iIndex].Row2.ToString("F2");
            lstPoints.Items[iIndex].SubItems[4].Text = _lstRect1[iIndex].Col2.ToString("F2");

            return true;
        }

        public void CurrentChange(int iCurrent)
        {
            if (iCurrent < 0 || iCurrent >= lstPoints.Items.Count)
                return;

            lstPoints.Items[iCurrent].Selected = true;
            lstPoints.EnsureVisible(iCurrent);

        }

        private void nudVal_ValueChanged(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            _lstRect1[lstPoints.SelectedIndices[0]].Row1 = (double)nudRow1.Value;
            _lstRect1[lstPoints.SelectedIndices[0]].Col1 = (double)nudCol1.Value;
            _lstRect1[lstPoints.SelectedIndices[0]].Row2 = (double)nudRow2.Value;
            _lstRect1[lstPoints.SelectedIndices[0]].Col2 = (double)nudCol2.Value;

            updateListMono(lstPoints.SelectedIndices[0]);

            if (UserSettingChange != null)
            {
                UserSettingChange(this, 
                    new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.ValueChange,
                    MultiSettingChange.Add_Last, _lstRect1,lstPoints.SelectedIndices[0]));
            }


        }

        private void lstPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択されたものを表示
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            int iIndex = lstPoints.SelectedIndices[0];

            nudRow1.Value = (decimal)_lstRect1[iIndex].Row1;
            nudCol1.Value = (decimal)_lstRect1[iIndex].Col1;
            nudRow2.Value = (decimal)_lstRect1[iIndex].Row2;
            nudCol2.Value = (decimal)_lstRect1[iIndex].Col2;

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.MultiSettingChange,
                    MultiSettingChange.Current_Change,_lstRect1, iIndex));
            }

        }

        private void btnAddLast_Click(object sender, EventArgs e)
        {
            _lstRect1.Add(new CRectangle1(_rcInit));
            updateList(-2);

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.MultiSettingChange,
                    MultiSettingChange.Add_Last, _lstRect1, lstPoints.SelectedIndices.Count > 0 ? lstPoints.SelectedIndices[0] : -1));
            }

        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndices.Count == 0 || lstPoints.Items.Count == 0)
            {
                _lstRect1.Add(new CRectangle1( _rcInit ));
                updateList(-2);
            }
            else
            {
                int iSelect = lstPoints.SelectedIndices[0];
                _lstRect1.Insert(iSelect + 1, new CRectangle1(_rcInit));
                updateList(iSelect + 1);
            }

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.MultiSettingChange,
                    MultiSettingChange.Add_Next, _lstRect1, lstPoints.SelectedIndices.Count > 0 ? lstPoints.SelectedIndices[0] : -1));
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            //選択項目の削除を行う
            int iDeleteIndex = lstPoints.SelectedIndices[0];

            _lstRect1.RemoveAt(iDeleteIndex);

            updateList();

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.MultiSettingChange,
                    MultiSettingChange.Delete, _lstRect1, lstPoints.SelectedIndices.Count > 0 ? lstPoints.SelectedIndices[0] : -1));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.OK, MultiSettingChange.None, _lstRect1, lstPoints.SelectedIndices.Count > 0 ? lstPoints.SelectedIndices[0]: -1 ));
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new Rectangle1MultiUserSettingEventArgs(UserSettingChangeType.Cancel, MultiSettingChange.None, _lstRect1, lstPoints.SelectedIndices.Count > 0 ? lstPoints.SelectedIndices[0]: -1 ));
            }
        }

        public void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser)
        {
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            _lstRect1[lstPoints.SelectedIndices[0]].Row1 = row1;
            _lstRect1[lstPoints.SelectedIndices[0]].Col1 = col1;
            _lstRect1[lstPoints.SelectedIndices[0]].Row2 = row2;
            _lstRect1[lstPoints.SelectedIndices[0]].Col2 = col2;

            nudRow1.Value = (decimal)row1;
            nudCol1.Value = (decimal)col1;
            nudRow2.Value = (decimal)row2;
            nudCol2.Value = (decimal)col2;

            updateListMono(lstPoints.SelectedIndices[0]);

        }

        public void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser)
        {
            //
        }

        public void Rectangle1_Cancel(object oUser)
        {
            //
        }

    }

    public class Rectangle1MultiUserSettingEventArgs : EventArgs
    {
        public UserSettingChangeType Type { get; private set; }
        public MultiSettingChange MultiSetting { get; private set; }

        public List<CRectangle1> Rect { get; private set; }

        public int Current { get; private set; }

        public Rectangle1MultiUserSettingEventArgs(UserSettingChangeType type, MultiSettingChange multi, List<CRectangle1> daRect1, int iCurrent)
        {
            Type = type;
            MultiSetting = multi;
            Rect = daRect1;
            Current = iCurrent;
        }
    }

}
