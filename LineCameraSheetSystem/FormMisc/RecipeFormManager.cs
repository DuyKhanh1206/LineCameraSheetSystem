using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ViewROI;
using Fujita.Misc;

namespace Fujita.InspectionSystem
{

    /// <summary>
    /// オブジェクトを設定するためのマネージャー
    /// </summary>
    public class RecipeFormManager
    {
        /// <summary>
        /// 設定を追加するタブ
        /// </summary>
        TabControl _tabMain;
        /// <summary>
        /// ユーザーコントロール
        /// </summary>
        List<UserControl> _lstRecipeForms;
        /// <summary>
        /// メインフォーム
        /// </summary>
        IMainFormEqu _mainForm;
        public IMainFormEqu MainForm
        {
            get
            {
                return _mainForm;
            }
        }

        EAuthenticationType _author;

        private bool _InspectionAuto;
        public RecipeFormManager(IMainFormEqu mainform, TabControl tab, bool bAuto, EAuthenticationType author )
        {
            _mainForm = mainform;
            _tabMain = tab;
            _InspectionAuto = bAuto;
            _author = author;
            _lstRecipeForms = new List<UserControl>();
        }

        /// <summary>
        /// すべてのタブを削除する
        /// </summary>
        public void DeleteTabs(bool bUpdate)
        {
            // 既存のリストがある場合、削除
            // GUI->データを実行
            if( bUpdate )
                UpdateDatas(true);

            notifyClosing();

            // 各タブページの終了処理
            _tabMain.TabPages.Clear();
            _lstRecipeForms.Clear();
            _bTabAddEnd = false;
        }

        /// <summary>
        /// コントロールをすべて無効にする
        /// </summary>
        public void DisableControls()
        {
            _tabMain.Enabled = false;
            // タブに含まれる全てのコントロールを無効にする
            foreach (UserControl ucl in _lstRecipeForms)
            {
                foreach (Control ctrl in ucl.Controls)
                {
                    enableControls(false, ctrl.Controls);
                }
            }
        }

        private void enableControls(bool bEnable, Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label)
                    continue;

                if (ctrl.Controls.Count > 0)
                    enableControls(bEnable, ctrl.Controls);
                ctrl.Enabled = bEnable;
            }
        }

        /// <summary>
        /// コントロールを有効にする
        /// 権限にあわせて
        /// </summary>
        public void EnableControls()
        {
            _tabMain.Enabled = true;
            // タブに含まれる全てのコントロールを有効にする
            foreach (UserControl ucl in _lstRecipeForms)
            {
                enableControls(true, ucl.Controls);
            }

            // 権限とモードにあわせて、各コントロールを有効にする
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.UpdateControls(_author);
            }
        }

        /// <summary>
        /// タブの設定が終了した
        /// </summary>
        private bool _bTabAddEnd = false;
        public bool TabAddEnd
        {
            get
            {
                return _bTabAddEnd;
            }
        }
        /// <summary>
        /// タブを追加する
        /// </summary>
        /// <param name="tabname">タブの表示名</param>
        /// <param name="u">UserControlを継承したGUI</param>
        /// <returns>true 正常終了 false 異常終了</returns>
        public bool AddTab(string tabname, UserControl u)
        {
            // ユーザーコントロールがIRecipeFormEquを機能を持っているか？
            IRecipeFormEqu ife = u as IRecipeFormEqu;
            if (ife == null)
                return false;

            if (tabname.Length <= 3)
            {
                tabname = "　" + tabname;
                tabname += "　";
            }

            // 新しいタブページを生成する
            _tabMain.TabPages.Add(tabname);

            //新しく生成されたページにユーザーコントロールを貼り付ける
            TabPage tp = _tabMain.TabPages[_tabMain.TabPages.Count - 1];
            tp.Controls.Add(u);
            u.Location = new Point(3, 6);

            // インターフェイスをリストに追加
            _lstRecipeForms.Add(u);
            return true;
        }


        /// <summary>
        /// タブの追加を終わらせる
        /// </summary>
        /// <param name="bSetting">設定タブを追加するかどうか</param>
        /// <param name="bResult">結果タブを追加するかどうか</param>
        /// <returns></returns>
        public void AddTabEnd(bool bSetting, bool bResult)
        {
            if (!_InspectionAuto)
            {
                //設定一欄タブを追加するか
                if (bSetting)
                {
                    AddTab("設定内容", new uclSettingData());
                }

                //結果表示タブを追加するか
                if (bResult)
                {
                    AddTab("結果", new uclResultData());
                }
            }

            // すべてのタブが登録されたことを伝える
            notifyOpening();

            // データ->GUIに設定
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.UpdateDatas(false);
            }

            // 権限にあわせてモードを変更する
            EnableControls();

            _bTabAddEnd = true;
        }

        /// <summary>
        /// GUIとデータの更新を行う
        /// </summary>
        /// <param name="bUp">true GUIからデータへ、false データからGUIへ </param>
        public void UpdateDatas(bool bUp)
        {
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.UpdateDatas(bUp);
            }
        }

        /// <summary>
        /// 結果の更新を行う
        /// </summary>
        public void UpdateResults()
        {
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.UpdateResult();
            }
        }

        public void NotifyControls(string type, object[] arrayParams)
        {
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.ReceiveControl(type, arrayParams);
            }
        }

        private void notifyOpening()
        {
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.Opening();
            }
        }

        private void notifyClosing()
        {
            foreach (IRecipeFormEqu ife in _lstRecipeForms)
            {
                ife.Closing();
            }
        }
    }
}
