using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.ScreenKeyboard
{
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMain _frmmain = new frmMain();
           
            if (textBox1.Focused)
            {
                _frmmain.strInputText = textBox1.Text;
            }
            else if (textBox2.Focused)
            {
                _frmmain.strInputText = textBox2.Text;
            }
            else if (textBox3.Focused)
            {
                _frmmain.strInputText = textBox3.Text;
            }

            DialogResult dl;
            dl = _frmmain.ShowDialog();
            string str = _frmmain.strOutputText;

            if (textBox1.Focused)
            {
                textBox1.Text = str;
            }
            else if (textBox2.Focused)
            {
                textBox2.Text = str;
            }
            else if (textBox3.Focused)
            {
                textBox3.Text = str;
            }
        }

       
    }
}
