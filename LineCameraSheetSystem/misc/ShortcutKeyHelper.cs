using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Extension
{
    [ProvideProperty("ShortcutKeys", typeof(Control))]
    public partial class ShortcutKeyHelper : Component, IExtenderProvider
    {
        Dictionary<Control, Keys> shortcutKeys = new Dictionary<Control, Keys>();
        public ShortcutKeyHelper()
        {
            InitializeComponent();
        }

        public ShortcutKeyHelper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #region IExtenderProvider メンバ

        // 拡張プロバイダを提供する型の場合は True を返します。
        bool IExtenderProvider.CanExtend(object extendee)
        {
            if ((extendee is Button) 
                //|| (extendee is ToolStripButton) 
                || (extendee is CheckBox))
            {
                return true;
            }

            return false;
        }

        #endregion

        [DefaultValue(typeof(Keys), "None")]
        public Keys GetShortcutKeys(Control ctrl)
        {
            if (this.shortcutKeys.ContainsKey(ctrl))
            {
                return this.shortcutKeys[ctrl];
            }
            else
            {
                return Keys.None;
            }
        }

        public void SetShortcutKeys(Control ctrl, Keys value)
        {
            if (this.shortcutKeys.ContainsKey(ctrl))
            {
                this.shortcutKeys[ctrl] = value;
            }
            else
            {
                this.shortcutKeys.Add(ctrl, value);
            }
        }

        // ショートカットキーを割り当てたボタンのクリックイベントを発生させます。
        public bool PerformClickByKeys(Keys keyData)
        {
            if (this.shortcutKeys.ContainsValue(keyData) == false)
            {
                return false;
            }

            foreach (KeyValuePair<Control, Keys> t in this.shortcutKeys)
            {
                if (t.Value == keyData)
                {
                    if (t.Key is Button)
                    {
                        ((Button)t.Key).PerformClick();
                        return true;
                    }
                    //if (t.Key is ToolStripButton)
                    //{
                    //    ((ToolStripButton)t.Key).PerformClick();
                    //    return true;
                    //}
                    if (t.Key is CheckBox)
                    {
                        ((CheckBox)t.Key).Checked = !((CheckBox)t.Key).Checked;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
