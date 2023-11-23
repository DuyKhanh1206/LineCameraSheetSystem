using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ViewROI;

namespace Fujita.InspectionSystem
{
    public delegate void PointMultiUserSettingEventHandler(object sender, PointMultiUserSettingEventArgs e);

    public partial class frmPointMulti : Form, ICallbackRoiPoint
    {
        public event PointMultiUserSettingEventHandler UserSettingChange;

        public frmPointMulti()
        {
            InitializeComponent();
        }

        double _dInitRow;
        double _dInitCol;

        List<double> _daRows;
        List<double> _daCols;

        public frmPointMulti(List<double> daRows, List<double> daCols, double row, double col, string sMessage)
        {
            InitializeComponent();

            _dInitRow = row;
            _dInitCol = col;

            _daRows = daRows;
            _daCols = daCols;
            lblMessage.Text = sMessage;

            updateList();
        }

        private void updateListMono(int iIndex)
        {
            if (iIndex < 0 || iIndex >= lstPoints.Items.Count)
                return;
            lstPoints.Items[iIndex].SubItems[1].Text = _daRows[iIndex].ToString("F2");
            lstPoints.Items[iIndex].SubItems[2].Text = _daCols[iIndex].ToString("F2");
        }

        private void updateList( int iSelect = -1 )
        {
            int iSelectedIndex = -1;
            if ( iSelect == -1 && lstPoints.SelectedIndices.Count > 0)
            {
                iSelectedIndex = lstPoints.SelectedIndices[0];
            }
            lstPoints.Items.Clear();

            for (int i = 0; i < _daRows.Count; i++)
            {
                lstPoints.Items.Add(new ListViewItem( new string [] { ( i + 1 ).ToString(), _daRows[i].ToString( "F2" ), _daCols[i].ToString( "F2" ) } )); 
            }

            if (iSelectedIndex >= 0 )
            {
                if (iSelectedIndex < lstPoints.Items.Count)
                {
                    lstPoints.Items[iSelectedIndex].Selected = true;
                }
                else
                {
					if (lstPoints.Items.Count > 0)
						lstPoints.Items[lstPoints.Items.Count - 1].Selected = true;
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
            if( lstPoints.SelectedIndices.Count > 0 )
                lstPoints.EnsureVisible(lstPoints.SelectedIndices[0]);
        }

        private void lstPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択されたものを表示
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            int iIndex = lstPoints.SelectedIndices[0];

            nudRow.Value = (decimal)_daRows[iIndex];
            nudCol.Value = (decimal)_daCols[iIndex];

            if (UserSettingChange != null)
            {
                UserSettingChange( this, new PointMultiUserSettingEventArgs( UserSettingChangeType.MultiSettingChange, 
                    MultiSettingChange.Current_Change, _daRows, _daCols, lstPoints.SelectedIndices.Count > 0 ?lstPoints.SelectedIndices[0]: -1 ));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndices.Count == 0)
                return;

            //選択項目の削除を行う
            int iDeleteIndex = lstPoints.SelectedIndices[0];

            _daRows.RemoveAt(iDeleteIndex);
            _daCols.RemoveAt(iDeleteIndex);

            updateList();

            if (UserSettingChange != null)
            {
                UserSettingChange( this, new PointMultiUserSettingEventArgs( UserSettingChangeType.MultiSettingChange, 
                    MultiSettingChange.Delete, _daRows, _daCols, lstPoints.SelectedIndices.Count > 0 ?lstPoints.SelectedIndices[0]: -1 ));
            }

        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndices.Count == 0 || lstPoints.Items.Count == 0)
            {
                _daRows.Add(_dInitRow);
                _daCols.Add(_dInitCol);
                updateList(-2);
            }
            else
            {
                int iSelect = lstPoints.SelectedIndices[0];
                if( iSelect < _daRows.Count - 1 )
                {
                    _daRows.Insert( iSelect + 1, (int)((_daRows[iSelect] + _daRows[iSelect+1] ) / 2.0) );
                    _daCols.Insert( iSelect + 1, (int)((_daCols[iSelect] + _daCols[iSelect+1] ) / 2.0) );
                }
                else
                {
                    if (iSelect == 0)
                    {
                        _daRows.Insert(iSelect + 1, (int)(1.5 * _daRows[iSelect] - 0.5 * _dInitRow));
                        _daCols.Insert(iSelect + 1, (int)(1.5 * _daCols[iSelect] - 0.5 * _dInitCol));
                    }
                    else
                    {
                        _daRows.Insert(iSelect + 1, (int)(1.5 * _daRows[iSelect] - 0.5 * _daRows[iSelect-1]));
                        _daCols.Insert(iSelect + 1, (int)(1.5 * _daCols[iSelect] - 0.5 * _daCols[iSelect-1]));
                    }
                }
                updateList( iSelect + 1 );
            }

            if (UserSettingChange != null)
            {
                UserSettingChange( this, new PointMultiUserSettingEventArgs( UserSettingChangeType.MultiSettingChange, 
                    MultiSettingChange.Add_Next, _daRows, _daCols, lstPoints.SelectedIndices.Count > 0 ?lstPoints.SelectedIndices[0]: -1 ));
            }

        }

        private void btnAddLast_Click(object sender, EventArgs e)
        {
            _daRows.Add(_daRows[_daRows.Count - 1]);
            _daCols.Add(_daCols[_daCols.Count - 1]);

            updateList(-2);

            if (UserSettingChange != null)
            {
                UserSettingChange( this, new PointMultiUserSettingEventArgs( UserSettingChangeType.MultiSettingChange, 
                    MultiSettingChange.Add_Last, _daRows, _daCols, lstPoints.SelectedIndices.Count > 0 ?lstPoints.SelectedIndices[0]: -1 ));
            }
        }

        public void Point_Move(double row, double col, object oUser)
        {
            // カレントを選択する
            if (lstPoints.SelectedIndices.Count > 0)
            {
                int iSelect = lstPoints.SelectedIndices[0];

                nudRow.Value = (decimal)row;
                nudCol.Value = (decimal)col;

                _daRows[iSelect] = row;
                _daCols[iSelect] = col;

                updateListMono(iSelect);
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

        public void CurrentChange(int iCurrent)
        {
            if (iCurrent < 0 || iCurrent >= lstPoints.Items.Count)
                return;

            lstPoints.Items[iCurrent].Selected = true;
            lstPoints.EnsureVisible(iCurrent);
        }

        private void nudRow_ValueChanged(object sender, EventArgs e)
        {
            _daRows[lstPoints.SelectedIndices[0]] = (double)nudRow.Value;
            updateListMono(lstPoints.SelectedIndices[0]);

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new PointMultiUserSettingEventArgs(UserSettingChangeType.ValueChange,
                    MultiSettingChange.Add_Last, _daRows, _daCols, lstPoints.SelectedIndices[0]));
            }
        }

        private void nudCol_ValueChanged(object sender, EventArgs e)
        {
            _daCols[lstPoints.SelectedIndices[0]] = (double)nudCol.Value;
            updateListMono(lstPoints.SelectedIndices[0]);

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new PointMultiUserSettingEventArgs(UserSettingChangeType.ValueChange,
                    MultiSettingChange.Add_Last, _daRows, _daCols, lstPoints.SelectedIndices[0]));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
				int iCurrent;
				iCurrent = (lstPoints.SelectedIndices.Count > 0) ? lstPoints.SelectedIndices[0] : 0;
				UserSettingChange(this, new PointMultiUserSettingEventArgs(UserSettingChangeType.OK, MultiSettingChange.Current_Change, _daRows, _daCols, iCurrent));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
				int iCurrent;
				iCurrent = (lstPoints.SelectedIndices.Count > 0) ? lstPoints.SelectedIndices[0] : 0;
                UserSettingChange(this, new PointMultiUserSettingEventArgs(UserSettingChangeType.Cancel, MultiSettingChange.Current_Change, _daRows, _daCols, iCurrent));
            }
        }

    }

    public class PointMultiUserSettingEventArgs : EventArgs
    {
        public UserSettingChangeType Type { get; private set; }
        public MultiSettingChange MultiSetting { get; private set; }

        public List<double> Rows { get; private set; }
        public List<double> Cols { get; private set; }

        public int Current { get; private set; }

        public PointMultiUserSettingEventArgs(UserSettingChangeType type, MultiSettingChange multi, List<double> daRows, List<double> daCols, int iCurrent)
        {
            Type = type;
            MultiSetting = multi;
            Rows = daRows;
            Cols = daCols;
            Current = iCurrent;
        }
    }
}
