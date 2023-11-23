using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Fujita.ScreenKeyboard
{
    /// <summary>
    /// ドラッグされると属するフォームを移動する  ラベル  /////テキストボックス/////
    /// </summary>
    public class DAndDMoveLabel : Label
    {
        const int WM_LBUTTONDOWN = 0x201;
        Point mouseDownPoint = new Point();
        bool dAndDMoveFlag = false;
    //    bool isMoved = false;
    //    int selectionStart, selectionLength;

        /// <summary>
        /// マウスの左ボタンが押下され、その後にD&Dされるとフォームが移動されると判定された時に発生します。
        /// e.Cancelプロパティにtrueをセットすると移動をキャンセル出来ます。
        /// </summary>
        public event DAndDMovingEventHandler DAndDMoving;

        protected override void WndProc(ref Message m)
        {
            //switch (m.Msg)
            //{
            //    case WM_LBUTTONDOWN:
            //        selectionStart = this.SelectionStart;
            //        selectionLength = this.SelectionLength;
            //        break;
            //}

            base.WndProc(ref m);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = e.Location;
                bool flag = true;
                int charHeight = this.Font.Height;

                //for (int i = 0; i < this.Lines.Length; i++)
                //{
                //    Point point = new Point(0, i * charHeight);
                //    Size size =
                //        TextRenderer.MeasureText(
                //            this.Lines[i],
                //            this.Font);
                //    Rectangle rect = new Rectangle(point, size);

                //    if (rect.Contains(e.Location))
                //    {
                //        flag = false;
                //        break;
                //    }
                //}

                DAndDMovingEventArgs args =
                    new DAndDMovingEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, false);

                if (DAndDMoving != null && flag)
                {
                    DAndDMoving(this, args);

                    if (args.Cancel)
                    {
                        flag = false;
                    }
                }

                this.dAndDMoveFlag = flag;
        //        this.isMoved = false;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Form form = this.FindForm();

            if (dAndDMoveFlag && form != null)
            {
                Point p = form.Location;
                p.Offset(
                    new Point(e.X - mouseDownPoint.X, e.Y - mouseDownPoint.Y));
                form.Location = p;

    //            isMoved = true;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            //if (dAndDMoveFlag && isMoved)
            //{
            //    this.Select(selectionStart, selectionLength);
            //}

            dAndDMoveFlag = false;
    //        isMoved = false;

            base.OnMouseUp(mevent);
        }
    }

    public class DAndDMovingEventArgs : MouseEventArgs
    {
        public DAndDMovingEventArgs(MouseButtons button, int clicks, int x, int y, int delta, bool cancel)
            : base(button, clicks, x, y, delta)
        {

            this.Cancel = cancel;
        }
        public bool Cancel { set; get; }
    }

    public delegate void DAndDMovingEventHandler(object sender, DAndDMovingEventArgs e);
}

