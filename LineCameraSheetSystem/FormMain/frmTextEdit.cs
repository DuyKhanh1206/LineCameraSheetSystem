using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Fujita.InspectionSystem;
using Fujita.Misc;

namespace LineCameraSheetSystem
{
    public partial class frmTextEdit : Form
    {
        public frmTextEdit(MainForm mf ,string name)
        {
            InitializeComponent();

            _mainForm = mf;



            textRecipeName.Text = name;

            _keyMask = new clsTextboxKeyPressMask(new KeyPressMask_InvalidFileCharUnderBar());
            _keyMask.SetTextBox(textRecipeName);

            this.textRecipeName.Focus();
           // this.textRecipeName.Select();
            this.AcceptButton = btnEnter;
        }

        // キー入力制限
        clsTextboxKeyPressMask _keyMask = null;

        private MainForm _mainForm { set; get; }

        public string NewName { get; private set; }

        private void textRecipeName_TextChanged(object sender, EventArgs e)
        {
            if (textRecipeName.TextLength > 20)
            {
                textRecipeName.Text = textRecipeName.Text.Remove(20);
                textRecipeName.Select(textRecipeName.Text.Length, 0);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (textRecipeName.Text == "")
            {
                Utility.ShowMessage(_mainForm, "品種名を入力して下さい。", MessageType.Error);

                return;
            }
            
            char[] invalidChars = Path.GetInvalidFileNameChars();
            char[] chU = new char[] { '_' };
            //ファイル名に使えるかチェック

            char[] invalidCharsU = invalidChars.Concat(chU).ToArray();
            if (textRecipeName.Text.IndexOfAny(invalidCharsU) > 0)
            {
                Utility.ShowMessage(_mainForm, @"使用できない文字が含まれています。\/:*?<>|_", MessageType.Error);
                
                return;
            }

            if (textRecipeName.Text == "未登録")
            {
                Utility.ShowMessage(this, "品種名に「未登録」は使えません。", MessageType.Error);
                return;
            }

            if (textRecipeName.Text.Length > 20)
            {
                Utility.ShowMessage(this, "品種名は20文字までです。", MessageType.Error);

                textRecipeName.Text = textRecipeName.Text.Remove(20);
                textRecipeName.Select(textRecipeName.Text.Length, 0);

                return;
            }


            int i = _mainForm.CheckNeme(textRecipeName.Text);
            if (i == 1)
            {
             //   _mainForm.TempoRecipe.KindName = textRecipeName.Text;
                this.NewName = textRecipeName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (i == 0)
            {
                
                Utility.ShowMessage(this, "同じ品種名が存在します。", MessageType.Error);
                return;
               
                    
            }
            else if(i==-1)
            {

                this.NewName = textRecipeName.Text;
                this.DialogResult = DialogResult.No;
                this.Close();
            }

       
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void frmTextEdit_Load(object sender, EventArgs e)
        {

        }

#if USE_OSK_KEYBOARD
        OSKController _OskCtrl = null;
#else
        Fujita.ScreenKeyboard.frmMain _keyboardForm = null;
#endif

        private void textRecipeName_MouseClick(object sender, MouseEventArgs e)
        {
#if USE_OSK_KEYBOARD
#else
            Color colOld = textRecipeName.BackColor;
            try
            {
                _keyboardForm = new Fujita.ScreenKeyboard.frmMain();
                _keyboardForm.strInputText = textRecipeName.Text;
                textRecipeName.BackColor = Color.Pink;

                if (DialogResult.OK != _keyboardForm.ShowDialog())
                    return;

                textRecipeName.Text = _keyboardForm.strOutputText;
            }
            finally
            {
                textRecipeName.BackColor = colOld;
            }
#endif
        }
    }
}
