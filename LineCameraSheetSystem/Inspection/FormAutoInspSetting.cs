using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
	public partial class FormAutoInspSetting : Form
	{
		private MainForm _mainFrom;

		public FormAutoInspSetting(MainForm mf)
		{
			InitializeComponent();
			_mainFrom = mf;
		}

		private void FormAutoInspSetting_Shown(object sender, EventArgs e)
		{
			chkAutoKandoBrightEnable.Checked = this._mainFrom.AutoInspection.IniAccess.AutoKandoBrightEnabled;
			chkAutoKandoDarkEnable.Checked = this._mainFrom.AutoInspection.IniAccess.AutoKandoDarkEnabled;
			spinAutoKandoLimit.Value = this._mainFrom.AutoInspection.IniAccess.AutoKandoLimit;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this._mainFrom.AutoInspection.IniAccess.AutoKandoBrightEnabled = chkAutoKandoBrightEnable.Checked;
			this._mainFrom.AutoInspection.IniAccess.AutoKandoDarkEnabled = chkAutoKandoDarkEnable.Checked;
			this._mainFrom.AutoInspection.IniAccess.AutoKandoLimit = (int)spinAutoKandoLimit.Value;
			this._mainFrom.AutoInspection.SetAutoKandoDatas();
			this.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

	}
}
