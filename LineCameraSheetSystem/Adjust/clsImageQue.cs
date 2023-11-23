using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;

namespace Adjustment
{
    class clsImageBuffer: IDisposable
    {
        LinkedList<HObject> _llstImageQue;
        public void Dispose()
        {
            Terminate();

        }

        public int BufferCount
        {
            get { return _llstImageQue.Count; }
        }

        public bool Initialize()
        {
            if (_llstImageQue != null)
                return false;

            _llstImageQue = new LinkedList<HObject>();
            return true;
        }

        public bool Terminate()
        {
            if (_llstImageQue == null)
                return false;

            Clear();
            _llstImageQue = null;
            return true;
        }

        public bool AddImage(HObject hoImg)
        {
            if (_llstImageQue == null)
                return false;

            HObject hoCopyImg = null;
            try
            {
                HOperatorSet.CopyObj(hoImg, out hoCopyImg, 1, -1);
                _llstImageQue.AddLast(hoCopyImg);
            }
            catch (HOperatorException)
            {
                return false;
            }

            return true;
        }

        public void Clear()
        {
            if (_llstImageQue == null)
                return;

            LinkedListNode<HObject> llNode = _llstImageQue.First;
            while (llNode != null)
            {
                llNode.Value.Dispose();
                llNode = llNode.Next;
            }
            _llstImageQue.Clear();
        }

        public bool IsReadyImage(int iIndex)
        {
            if (_llstImageQue == null)
                return false;

            if (iIndex < 0 || iIndex >= _llstImageQue.Count)
                return false;

            if (iIndex == 0)
            {
                if (_llstImageQue.Count >= 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (_llstImageQue.Count > iIndex + 1 )
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 対象イメージの前後に合成したイメージをくっつけて生成
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="iAttachSize"></param>
        /// <param name="hoImg"></param>
        /// <param name="iOffsetY"></param>
        /// <returns></returns>
        public bool GetAttachedImage(int iIndex, int iAttachSize, out HObject hoImg, out int iOffsetY, out int iYMin, out int iYMax )
        {
            hoImg = null;
            iOffsetY = 0;
            iYMin = 0;
            iYMax = 0;

            if( !IsReadyImage( iIndex ))
                return false;

            HObject[] ahoTemp = new HObject[10];
            try
            {
                LinkedListNode<HObject> llNode = _llstImageQue.First;
                for (int i = 0; i < iIndex; i++)
                {
                    llNode = llNode.Next;
                }

                HTuple htWidth, htHeight;
                HOperatorSet.GetImageSize(llNode.Value, out htWidth, out htHeight);


                HTuple htOffsetRow = new HTuple(), htOffsetCol = new HTuple(),
                    htRow1 = new HTuple(), htCol1 = new HTuple(), htRow2 = new HTuple(), htCol2 = new HTuple();

                int iWidth = htWidth.I;
                int iHeight = 0;
                int iOffset = -htHeight.I;

                // 先頭を取り付ける
                if (iIndex - 1 >= 0 )
                {
                    HOperatorSet.ConcatObj(llNode.Previous.Value, llNode.Value, out ahoTemp[0]);
                    htCol1 = htCol1.TupleConcat(-1);
                    htCol2 = htCol2.TupleConcat(-1);
                    htRow1 = htRow1.TupleConcat(-1);
                    htRow2 = htRow2.TupleConcat(-1);
                    iHeight += iAttachSize;
                    iOffset += iAttachSize;
                    htOffsetRow = htOffsetRow.TupleConcat(iOffset);
                    htOffsetCol = htOffsetCol.TupleConcat(0);
                    iYMin = iAttachSize;
                }
                else
                {
                    HOperatorSet.CopyObj(llNode.Value, out ahoTemp[0], 1, -1);
                    iYMin = 0;
                }

                iHeight += htHeight.I;
                iYMax = iYMin + htHeight.I;
                iOffset += htHeight.I;
                htOffsetRow = htOffsetRow.TupleConcat(iOffset);
                htOffsetCol = htOffsetCol.TupleConcat(0);
                htCol1 = htCol1.TupleConcat(-1);
                htCol2 = htCol2.TupleConcat(-1);
                htRow1 = htRow1.TupleConcat(-1);
                htRow2 = htRow2.TupleConcat(-1);

                if (iIndex + 1 < _llstImageQue.Count)
                {
                    HOperatorSet.ConcatObj(ahoTemp[0], llNode.Next.Value, out ahoTemp[1]);
                    iHeight += iAttachSize;
                    iOffset += htHeight.I;
                    htOffsetRow = htOffsetRow.TupleConcat(iOffset);
                    htOffsetCol = htOffsetCol.TupleConcat(0);
                    htCol1 = htCol1.TupleConcat(-1);
                    htCol2 = htCol2.TupleConcat(-1);
                    htRow1 = htRow1.TupleConcat(-1);
                    htRow2 = htRow2.TupleConcat(-1);
                }
                else
                {
                    HOperatorSet.CopyObj(ahoTemp[0], out ahoTemp[1], 1, -1);
                }
                HOperatorSet.TileImagesOffset(ahoTemp[1], out hoImg, htOffsetRow, htOffsetCol, htRow1, htCol1, htRow1, htCol1, iWidth, iHeight);

                if (iIndex > 0)
                {
                    iOffsetY = htHeight.I * iIndex - iAttachSize;
                }

            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                foreach (HObject o in ahoTemp)
                {
                    if (o != null)
                        o.Dispose();
                }
            }
            return true;
        }
    }
}
