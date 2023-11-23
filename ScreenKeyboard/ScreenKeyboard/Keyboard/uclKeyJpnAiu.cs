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
    public partial class uclKeyJpnAiu : UserControl
    {
        public uclKeyJpnAiu()
        {
           
            InitializeComponent();

            SystemSetting setting = new SystemSetting();
            setting.LoadKeyJpn(btnSp1, btnSp2, btnSp3, btnSp4, btnSp5, btnSp6, btnSp7, btnSp8);

            fFontSize = btnA.Font.Size;

            SetListButton();
        }

        frmMain _frmmain;
        public void SetMainForm(frmMain frm)
        {
            _frmmain = frm;
            _frmmain.checkevent += new frmMain.CheckBoxEventHandler(_frmmain_checkevent);
        }

        void _frmmain_checkevent(object sender, frmMain.CheckEventArgs e)
        {
            if(!Status.GetInstance().bJapan)
            {
               return;
            }

            ResetKeyJpn();

            if (e.chkbox == null)
            {
                
                return;
            }

            if ((string)e.chkbox.Tag == clsFixedValue.STR_CHK_SMALL)
            {
                ChangeKeyJpnSmall(e.chkbox.Checked);
                
                
            }
            else if ((string)e.chkbox.Tag == clsFixedValue.STR_CHK_DAKU)
            {
                ChangeKeyJpnDaku(e.chkbox.Checked);
            }
            else
            {
                ChangeKeyJpnHandaku(e.chkbox.Checked);
            }

            Status.GetInstance().SetCheck(e.chkbox.Tag.ToString());
        }

        

        private void uclKeyJpnAiu_Load(object sender, EventArgs e)
        {
           
              
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


        //小文字に切り替える
        private void ChangeKeyJpnSmall(bool bOn)
        {
            if (bOn)
            {
                btnA.Text = "ぁ";
                btnA.Tag = "xa";
                btnI.Text = "ぃ";
                btnI.Tag = "xi";
                btnU.Text = "ぅ";
                btnU.Tag = "xu";
                btnE.Text = "ぇ";
                btnE.Tag = "xe";
                btnO.Text = "ぉ";
                btnO.Tag = "xo";

                btnTu.Text = "っ";
                btnTu.Tag = "xtu";

                btnYa.Text = "ゃ";
                btnYa.Tag = "xya";
                btnYu.Text = "ゅ";
                btnYu.Tag = "xyu";
                btnYo.Text = "ょ";
                btnYo.Tag = "xyo";
              
                btnWa.Text = "ゎ";
                btnWa.Tag = "xwa";


            }
            else
            {
                //btnA.Text = "あ";
                //btnA.Tag = "a";
                //btnI.Text = "い";
                //btnI.Tag = "i";
                //btnU.Text = "う";
                //btnU.Tag = "u";
                //btnE.Text = "え";
                //btnE.Tag = "e";
                //btnO.Text = "お";
                //btnO.Tag = "o";

                //btnTu.Text = "つ";
                //btnTu.Tag = "tu";

                //btnYa.Text = "や";
                //btnYa.Tag = "ya";
                //btnYu.Text = "ゆ";
                //btnYu.Tag = "yu";
                //btnYo.Text = "よ";
                //btnYo.Tag = "yo";

                //btnWa.Text = "わ";
                //btnWa.Tag = "wa";
            }
        }

        //濁点の付いた文字に変える
        private void ChangeKeyJpnDaku(bool bOn)
        {
            if (bOn)
            {
                btnKa.Text = "が";
                btnKa.Tag = "ga";
                btnKi.Text = "ぎ";
                btnKi.Tag = "gi";
                btnKu.Text = "ぐ";
                btnKu.Tag = "gu";
                btnKe.Text = "げ";
                btnKe.Tag = "ge";
                btnKo.Text = "ご";
                btnKo.Tag = "go";

                btnSa.Text = "ざ";
                btnSa.Tag = "za";
                btnSi.Text = "じ";
                btnSi.Tag = "zi";
                btnSu.Text = "ず";
                btnSu.Tag = "zu";
                btnSe.Text = "ぜ";
                btnSe.Tag = "ze";
                btnSo.Text = "ぞ";
                btnSo.Tag = "zo";

                btnTa.Text = "だ";
                btnTa.Tag = "da";
                btnTi.Text = "ぢ";
                btnTi.Tag = "di";
                btnTu.Text = "づ";
                btnTu.Tag = "du";
                btnTe.Text = "で";
                btnTe.Tag = "de";
                btnTo.Text = "ど";
                btnTo.Tag = "do";

                btnHa.Text = "ば";
                btnHa.Tag = "ba";
                btnHi.Text = "び";
                btnHi.Tag = "bi";
                btnHu.Text = "ぶ";
                btnHu.Tag = "bu";
                btnHe.Text = "べ";
                btnHe.Tag = "be";
                btnHo.Text = "ぼ";
                btnHo.Tag = "bo";
            }
            else
            {
                //btnKa.Text = "か";
                //btnKa.Tag = "ka";
                //btnKi.Text = "き";
                //btnKi.Tag = "ki";
                //btnKu.Text = "く";
                //btnKu.Tag = "ku";
                //btnKe.Text = "け";
                //btnKe.Tag = "ke";
                //btnKo.Text = "こ";
                //btnKo.Tag = "ko";

                //btnSa.Text = "さ";
                //btnSa.Tag = "sa";
                //btnSi.Text = "し";
                //btnSi.Tag = "si";
                //btnSu.Text = "す";
                //btnSu.Tag = "su";
                //btnSe.Text = "せ";
                //btnSe.Tag = "se";
                //btnSo.Text = "そ";
                //btnSo.Tag = "so";

                //btnTa.Text = "た";
                //btnTa.Tag = "ta";
                //btnTi.Text = "ち";
                //btnTi.Tag = "ti";
                //btnTu.Text = "つ";
                //btnTu.Tag = "tu";
                //btnTe.Text = "て";
                //btnTe.Tag = "te";
                //btnTo.Text = "と";
                //btnTo.Tag = "to";

                //btnHa.Text = "は";
                //btnHa.Tag = "ha";
                //btnHi.Text = "ひ";
                //btnHi.Tag = "hi";
                //btnHu.Text = "ふ";
                //btnHu.Tag = "hu";
                //btnHe.Text = "へ";
                //btnHe.Tag = "he";
                //btnHo.Text = "ほ";
                //btnHo.Tag = "ho";
            }
        }

        //半濁点の付いた文字に変える
        private void ChangeKeyJpnHandaku(bool bOn)
        {
            if (bOn)
            {
                btnHa.Text = "ぱ";
                btnHa.Tag = "pa";
                btnHi.Text = "ぴ";
                btnHi.Tag = "pi";
                btnHu.Text = "ぷ";
                btnHu.Tag = "pu";
                btnHe.Text = "ぺ";
                btnHe.Tag = "pe";
                btnHo.Text = "ぽ";
                btnHo.Tag = "po";

            }
            else
            {
                //btnHa.Text = "は";
                //btnHa.Tag = "ha";
                //btnHi.Text = "ひ";
                //btnHi.Tag = "hi";
                //btnHu.Text = "ふ";
                //btnHu.Tag = "hu";
                //btnHe.Text = "へ";
                //btnHe.Tag = "he";
                //btnHo.Text = "ほ";
                //btnHo.Tag = "ho";
            }
        }

        //始めの状態に戻す
        private void ResetKeyJpn()
        {
            btnA.Text = "あ";
            btnA.Tag = "a";
            btnI.Text = "い";
            btnI.Tag = "i";
            btnU.Text = "う";
            btnU.Tag = "u";
            btnE.Text = "え";
            btnE.Tag = "e";
            btnO.Text = "お";
            btnO.Tag = "o";

            btnKa.Text = "か";
            btnKa.Tag = "ka";
            btnKi.Text = "き";
            btnKi.Tag = "ki";
            btnKu.Text = "く";
            btnKu.Tag = "ku";
            btnKe.Text = "け";
            btnKe.Tag = "ke";
            btnKo.Text = "こ";
            btnKo.Tag = "ko";

            btnSa.Text = "さ";
            btnSa.Tag = "sa";
            btnSi.Text = "し";
            btnSi.Tag = "si";
            btnSu.Text = "す";
            btnSu.Tag = "su";
            btnSe.Text = "せ";
            btnSe.Tag = "se";
            btnSo.Text = "そ";
            btnSo.Tag = "so";

            btnTa.Text = "た";
            btnTa.Tag = "ta";
            btnTi.Text = "ち";
            btnTi.Tag = "ti";
            btnTu.Text = "つ";
            btnTu.Tag = "tu";
            btnTe.Text = "て";
            btnTe.Tag = "te";
            btnTo.Text = "と";
            btnTo.Tag = "to";

            btnHa.Text = "は";
            btnHa.Tag = "ha";
            btnHi.Text = "ひ";
            btnHi.Tag = "hi";
            btnHu.Text = "ふ";
            btnHu.Tag = "hu";
            btnHe.Text = "へ";
            btnHe.Tag = "he";
            btnHo.Text = "ほ";
            btnHo.Tag = "ho";

            btnYa.Text = "や";
            btnYa.Tag = "ya";
            btnYu.Text = "ゆ";
            btnYu.Tag = "yu";
            btnYo.Text = "よ";
            btnYo.Tag = "yo";

            btnWa.Text = "わ";
            btnWa.Tag = "wa";


        }

        public void ChengeFontSize(double dMagnify)
        {
            string strFontName = btnA.Font.Name;

            for(int i = 0;_listButton.Count>i;i++)
            {
                _listButton[i].Font = new Font(strFontName, (float)(fFontSize * dMagnify));
            }
        }

        private float fFontSize; 
        private List<Button> _listButton;
        private void SetListButton()
        {
            _listButton = new List<Button>();

            _listButton.Add(btnA);
            _listButton.Add(btnI);
            _listButton.Add(btnU);
            _listButton.Add(btnE);
            _listButton.Add(btnO);
            _listButton.Add(btnKa);
            _listButton.Add(btnKi);
            _listButton.Add(btnKu);
            _listButton.Add(btnKe);
            _listButton.Add(btnKo);
            _listButton.Add(btnSa);
            _listButton.Add(btnSi);
            _listButton.Add(btnSu);
            _listButton.Add(btnSe);
            _listButton.Add(btnSo);
            _listButton.Add(btnTa);
            _listButton.Add(btnTi);
            _listButton.Add(btnTu);
            _listButton.Add(btnTe);
            _listButton.Add(btnTo);
            _listButton.Add(btnNa);
            _listButton.Add(btnNi);
            _listButton.Add(btnNu);
            _listButton.Add(btnNe);
            _listButton.Add(btnNo);
            _listButton.Add(btnHa);
            _listButton.Add(btnHi);
            _listButton.Add(btnHu);
            _listButton.Add(btnHe);
            _listButton.Add(btnHo);
            _listButton.Add(btnMa);
            _listButton.Add(btnMi);
            _listButton.Add(btnMu);
            _listButton.Add(btnMe);
            _listButton.Add(btnMo);
            _listButton.Add(btnYa);
            _listButton.Add(btnYu);
            _listButton.Add(btnYo);
            _listButton.Add(btnRa);
            _listButton.Add(btnRi);
            _listButton.Add(btnRu);
            _listButton.Add(btnRe);
            _listButton.Add(btnRo);
            _listButton.Add(btnWa);
            _listButton.Add(btnWo);
            _listButton.Add(btnN);
            _listButton.Add(btnSp1);
            _listButton.Add(btnSp2);
            _listButton.Add(btnSp3);
            _listButton.Add(btnSp4);
            _listButton.Add(btnSp5);
            _listButton.Add(btnSp6);
            _listButton.Add(btnSp7);
            _listButton.Add(btnSp8);
        }
    }
}
