using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.ScreenKeyboard
{
    public partial class uclKeyEngQwe : UserControl
    {
        public uclKeyEngQwe()
        {
            this.SetStyle(ControlStyles.Selectable, false);

            InitializeComponent();

            SystemSetting setting = new SystemSetting();
            setting.LoadKeyEng(btnSp1, btnSp2, btnSp3, btnSp4, btnSp5, btnSp6, btnSp7, btnSp8);

            fFontSize = btn0.Font.Size;

            SetListButton();
        }


        private void uclKeyEngQwe_Load(object sender, EventArgs e)
        {

        }

        frmMain _frmmain;
        public void SetMainForm(frmMain frm)
        {
            _frmmain = frm;
            _frmmain.checkevent += new frmMain.CheckBoxEventHandler(_frmmain_checkevent);
        }

        void _frmmain_checkevent(object sender, frmMain.CheckEventArgs e)
        {
            //if (Status.GetInstance().bJapan)
            //{
            //    return;
            //}

            if (e.chkbox == null)
            {
                ChangeKeyEngSmall(false);

            }
            else
            {
                ChangeKeyEngSmall(true);
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            string str = ((Button)sender).Tag.ToString();
           
            OnKeyPush(str);
        }

        //ボタンイベント
        public delegate void KeyPushEventHandler(object sender, String strKeys);
        public event KeyPushEventHandler kyepush;
        protected virtual void OnKeyPush(String strKeys)
        {
            if (kyepush != null)
                kyepush(this, strKeys);
        }

        private void ChangeKeyEngSmall(bool bOn)
        {
            if (bOn)
            {
                btnA.Text = "a";
                btnA.Tag = "a";
                btnB.Text = "b";
                btnB.Tag = "b";
                btnC.Text = "c";
                btnC.Tag = "c";
                btnD.Text = "d";
                btnD.Tag = "d";
                btnE.Text = "e";
                btnE.Tag = "e";
                btnF.Text = "f";
                btnF.Tag = "f";
                btnG.Text = "g";
                btnG.Tag = "g";
                btnH.Text = "h";
                btnH.Tag = "h";
                btnI.Text = "i";
                btnI.Tag = "i";
                btnJ.Text = "j";
                btnJ.Tag = "j";
                btnK.Text = "k";
                btnK.Tag = "k";
                btnL.Text = "l";
                btnL.Tag = "l";
                btnM.Text = "m";
                btnM.Tag = "m";
                btnN.Text = "n";
                btnN.Tag = "n";
                btnO.Text = "o";
                btnO.Tag = "o";
                btnP.Text = "p";
                btnP.Tag = "p";
                btnQ.Text = "q";
                btnQ.Tag = "q";
                btnR.Text = "r";
                btnR.Tag = "r";
                btnS.Text = "s";
                btnS.Tag = "s";
                btnT.Text = "t";
                btnT.Tag = "t";
                btnU.Text = "u";
                btnU.Tag = "u";
                btnV.Text = "v";
                btnV.Tag = "v";
                btnW.Text = "w";
                btnW.Tag = "w";
                btnX.Text = "x";
                btnX.Tag = "x";
                btnY.Text = "y";
                btnY.Tag = "y";
                btnZ.Text = "z";
                btnZ.Tag = "z";

                //
                btn1.Text = "1";
                btn1.Tag = "1";
                btn2.Text = "2";
                btn2.Tag = "2";
                btn3.Text = "3";
                btn3.Tag = "3";
                btn4.Text = "4";
                btn4.Tag = "4";
                btn5.Text = "5";
                btn5.Tag = "5";
                btn6.Text = "6";
                btn6.Tag = "6";
                btn7.Text = "7";
                btn7.Tag = "7";
                btn8.Text = "8";
                btn8.Tag = "8";
                btn9.Text = "9";
                btn9.Tag = "9";
                btn0.Text = "0";
                btn0.Tag = "0";

                //
                btnSymbol1.Text = "-";
                btnSymbol1.Tag = "-";
                btnSymbol2.Text = "";   //^
                btnSymbol2.Tag = "";
                btnSymbol3.Text = "";   //\
                btnSymbol3.Tag = "";
                //
                btnSymbol4.Text = "@";
                btnSymbol4.Tag = "@";
                btnSymbol5.Text = "[";
                btnSymbol5.Tag = "[";
                //
                btnSymbol6.Text = ";";
                btnSymbol6.Tag = ";";
                btnSymbol7.Text = "";   //:
                btnSymbol7.Tag = "";
                btnSymbol8.Text = "]";
                btnSymbol8.Tag = "]";
                //
                btnSymbol9.Text = ",";
                btnSymbol9.Tag = ",";
                btnSymbol10.Text = ".";
                btnSymbol10.Tag = ".";
                btnSymbol11.Text = "";      // /
                btnSymbol11.Tag = "";
                btnSymbol12.Text = "";      // \
                btnSymbol12.Tag = "";

                btnSpace.Text = "Space";
                btnSpace.Tag = " ";
            }
            else
            {
                btnA.Text = "A";
                btnA.Tag = "+a";
                btnB.Text = "B";
                btnB.Tag = "+b";
                btnC.Text = "C";
                btnC.Tag = "+c";
                btnD.Text = "D";
                btnD.Tag = "+d";
                btnE.Text = "E";
                btnE.Tag = "+e";
                btnF.Text = "F";
                btnF.Tag = "+f";
                btnG.Text = "G";
                btnG.Tag = "+g";
                btnH.Text = "H";
                btnH.Tag = "+h";
                btnI.Text = "I";
                btnI.Tag = "+I";
                btnJ.Text = "J";
                btnJ.Tag = "+j";
                btnK.Text = "K";
                btnK.Tag = "+k";
                btnL.Text = "L";
                btnL.Tag = "+l";
                btnM.Text = "M";
                btnM.Tag = "+m";
                btnN.Text = "N";
                btnN.Tag = "+n";
                btnO.Text = "O";
                btnO.Tag = "+o";
                btnP.Text = "P";
                btnP.Tag = "+p";
                btnQ.Text = "Q";
                btnQ.Tag = "+q";
                btnR.Text = "R";
                btnR.Tag = "+r";
                btnS.Text = "S";
                btnS.Tag = "+s";
                btnT.Text = "T";
                btnT.Tag = "+t";
                btnU.Text = "U";
                btnU.Tag = "+u";
                btnV.Text = "V";
                btnV.Tag = "+v";
                btnW.Text = "W";
                btnW.Tag = "+w";
                btnX.Text = "X";
                btnX.Tag = "+x";
                btnY.Text = "Y";
                btnY.Tag = "+y";
                btnZ.Text = "Z";
                btnZ.Tag = "+z";

                //
                btn1.Text = "!";
                btn1.Tag = "!";
                btn2.Text = "";
                btn2.Tag = "";
                btn3.Text = "#";
                btn3.Tag = "#";
                btn4.Text = "$";
                btn4.Tag = "$";
                btn5.Text = "%";
                btn5.Tag = "{%}";
                btn6.Text = "&&";
                btn6.Tag = "&";
                btn7.Text = "'";
                btn7.Tag = "'";
                btn8.Text = "(";
                btn8.Tag = "{(}";
                btn9.Text = ")";
                btn9.Tag = "{)}";
                btn0.Text = "";
                btn0.Tag = "";

                //
                btnSymbol1.Text = "=";
                btnSymbol1.Tag = "=";
                btnSymbol2.Text = "~";
                btnSymbol2.Tag = "{~}";
                btnSymbol3.Text = "";
                btnSymbol3.Tag = "";
                //
                btnSymbol4.Text = "`";
                btnSymbol4.Tag = "`";
                btnSymbol5.Text = "{";
                btnSymbol5.Tag = "{{}";
                //
                btnSymbol6.Text = "+";
                btnSymbol6.Tag = "{+}";
                btnSymbol7.Text = "";   //*
                btnSymbol7.Tag = "";
                btnSymbol8.Text = "}";
                btnSymbol8.Tag = "{}}";
                //
                btnSymbol9.Text = "";       //<
                btnSymbol9.Tag = "";
                btnSymbol10.Text = "";     //>
                btnSymbol10.Tag = "";
                btnSymbol11.Text = "";      // ?
                btnSymbol11.Tag = "";
                btnSymbol12.Text = "_";
                btnSymbol12.Tag = "_";

                btnSpace.Text = "Space";
                btnSpace.Tag = " ";
            }
        }

        private float fFontSize;

        public void ChengeFontSize(double dMagnify)
        {
            string strFontName = btn0.Font.Name;
            //float fFontSize = btn0.Font.Size;

          //  btn0.Font = new Font( strFontName, (float)(fFontSize * dMagnify));

            for (int i = 0; _listButton.Count > i; i++)
            {
                _listButton[i].Font = new Font(strFontName, (float)(fFontSize * dMagnify));
            }
            
        }

        private List<Button> _listButton;
        private void SetListButton()
        {
            _listButton = new List<Button>();
            
            _listButton.Add(btn0);
            _listButton.Add(btn1);
            _listButton.Add(btn2);
            _listButton.Add(btn3);
            _listButton.Add(btn4); 
            _listButton.Add(btn5);
            _listButton.Add(btn6);
            _listButton.Add(btn7);
            _listButton.Add(btn8);
            _listButton.Add(btn9);
            _listButton.Add(btn0);
            _listButton.Add(btnA);
            _listButton.Add(btnB);
            _listButton.Add(btnC);
            _listButton.Add(btnD);
            _listButton.Add(btnE);
            _listButton.Add(btnF);
            _listButton.Add(btnG);
            _listButton.Add(btnH);
            _listButton.Add(btnI);
            _listButton.Add(btnJ);
            _listButton.Add(btnK);
            _listButton.Add(btnL);
            _listButton.Add(btnM);
            _listButton.Add(btnN);
            _listButton.Add(btnO);
            _listButton.Add(btnP);
            _listButton.Add(btnQ);
            _listButton.Add(btnR);
            _listButton.Add(btnS);
            _listButton.Add(btnT);
            _listButton.Add(btnU);
            _listButton.Add(btnV);
            _listButton.Add(btnW);
            _listButton.Add(btnX);
            _listButton.Add(btnY);
            _listButton.Add(btnZ);
            _listButton.Add(btnSp1);
            _listButton.Add(btnSp2);
            _listButton.Add(btnSp3);
            _listButton.Add(btnSp4);
            _listButton.Add(btnSp5);
            _listButton.Add(btnSp6);
            _listButton.Add(btnSp7);
            _listButton.Add(btnSp8);
            _listButton.Add(btnSymbol1);
            _listButton.Add(btnSymbol2);
            _listButton.Add(btnSymbol3);
            _listButton.Add(btnSymbol4);
            _listButton.Add(btnSymbol5);
            _listButton.Add(btnSymbol6);
            _listButton.Add(btnSymbol7);
            _listButton.Add(btnSymbol8);
            _listButton.Add(btnSymbol9);
            _listButton.Add(btnSymbol10);
            _listButton.Add(btnSymbol11);
            _listButton.Add(btnSymbol12);
            _listButton.Add(btnSpace);

        }




    }
}
