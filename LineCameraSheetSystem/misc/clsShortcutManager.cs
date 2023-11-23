using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.Misc
{
    interface IShortcutClient
    {
        bool ProcessShortcutKey(Keys keyData);
    }

    class clsShortcutManager
    {
        List<IShortcutClient> _lstShortcutClient;
        List<Control> _lstIgnoreCtrl;

        public bool Initialize()
        {
            if (_lstShortcutClient != null)
                return false;

            _lstShortcutClient = new List<IShortcutClient>();
            _lstIgnoreCtrl = new List<Control>();

            return true;
        }

        public void Terminate()
        {
            if (_lstShortcutClient == null)
                return;
            Clear();
            _lstShortcutClient = null;
        }

        public bool Add(IShortcutClient client)
        {
            if (_lstShortcutClient == null)
                return false;

            if (_lstShortcutClient.Contains(client))
                return false;

            if (!(client is UserControl))
                return false;

            _lstShortcutClient.Add(client);
            return true;
        }

        public bool AddIgnoreComponet(Control ctrl)
        {
            if (_lstShortcutClient == null)
                return false;

            if (_lstIgnoreCtrl.Contains(ctrl))
                return false;

            ctrl.GotFocus += new EventHandler(ctrl_GotFocus);
            ctrl.LostFocus += new EventHandler(ctrl_LostFocus);

            _lstIgnoreCtrl.Add(ctrl);

            return true;
        }

        bool _bIgnoreCtrlFocus = false;
        public bool IgnoreCtrlFocus
        {
            get { return _bIgnoreCtrlFocus; }
        }
        void ctrl_LostFocus(object sender, EventArgs e)
        {
            _bIgnoreCtrlFocus = false;
        }

        void ctrl_GotFocus(object sender, EventArgs e)
        {
            _bIgnoreCtrlFocus = true;
        }

        public void Clear()
        {
            if (_lstShortcutClient == null)
                return;

            _lstShortcutClient.Clear();

            for (int i = 0; i < _lstShortcutClient.Count; i++)
            {
                _lstIgnoreCtrl[i].GotFocus -= ctrl_GotFocus;
                _lstIgnoreCtrl[i].LostFocus -= ctrl_LostFocus;
            }
            _lstIgnoreCtrl.Clear();

        }

        public bool DoProcessCmdKey(Keys keyData)
        {
            if (_lstShortcutClient == null)
                return false;

            if (_bIgnoreCtrlFocus)
                return false;

            foreach( IShortcutClient sc in _lstShortcutClient )
            {
                UserControl uc = (UserControl)sc;
                if (uc.Visible)
                {
                    if( sc.ProcessShortcutKey(keyData))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
