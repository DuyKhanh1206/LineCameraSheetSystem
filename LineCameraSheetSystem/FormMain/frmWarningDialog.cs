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
	public partial class frmWarningDialog : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public frmWarningDialog()
		{
			InitializeComponent();
		}

		public void SetText(string title, string msg)
		{
			if (title != null)
				this.Text = title;
			labelText.Text = msg;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
