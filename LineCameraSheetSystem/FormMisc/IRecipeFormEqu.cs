using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.InspectionSystem
{
    /// <summary>
    /// レシピの設定フォーム（ユーザーコントロール）が持つインターフェイス
    /// 品種設定用フォームはこのインターフェースを継承しないといけない
    /// </summary>
    public interface IRecipeFormEqu
    {
        /// <summary>
        /// GUIからデータ及びデータからGUIへの設定
        /// </summary>
        /// <param name="bUp"> </param>
        void UpdateDatas(bool bUp);
        
        /// <summary>
        /// コントロールに対する動作指令
        /// </summary>
        /// <param name="auther"></param>
        void UpdateControls(EAuthenticationType auther);

        /// <summary>
        /// 結果データを更新する
        /// </summary>
        void UpdateResult();

        /// <summary>
        /// 外部から定義された独自アップデート
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="arrayParams"></param>
        void ReceiveControl(string sType, object[] arrayParams);

        /// <summary>
        /// フォームが閉じられるときに呼ばれます
        /// </summary>
        void Closing();
        
        /// <summary>
        /// フォームがオープンされるときに呼ばれます
        /// </summary>
        void Opening();

    }
}
