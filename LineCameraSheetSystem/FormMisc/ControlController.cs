using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.InspectionSystem
{
    public class ControlAuthority
    {
        internal Dictionary<string, Tuple<bool,bool,bool>> _dic;
        public ControlAuthority(ControlController ctrol)
        {
            _dic = new Dictionary<string, Tuple<bool, bool, bool>>();
            ctrol.setControlAuthority(this);
        }

        public void setAuthority(Control ctrl, bool bOperator, bool bAdministrator, bool bDeveloper)
        {
            _dic.Add(ctrl.Name, new Tuple<bool, bool, bool>(bOperator, bAdministrator, bDeveloper));
        }
    }

    public class InspectControlAuthority 
    {
        internal Dictionary<string, bool> _dic;
        public InspectControlAuthority(ControlController ctrol)
        {
            _dic = new Dictionary<string, bool>();
            ctrol.setInspectControlAuthority(this);
        }

        public void setAuthority(Control ctrl, bool bEnable)
        {
            _dic.Add(ctrl.Name, bEnable);
        }
    }
    
    public class ControlController
    {
        Control.ControlCollection _controls;

        ControlAuthority _ctrlAuthority = null;
        InspectControlAuthority _insCtrlAuthority = null;

        public bool UpdateDefault { get; set; }
        public bool IgnoreUnset { get; set; }

        internal void setControlAuthority( ControlAuthority ctrlauthor )
        {
            _ctrlAuthority = ctrlauthor;
        }
        public ControlAuthority CtrlAuthority { get { return _ctrlAuthority; } }

        internal void setInspectControlAuthority(InspectControlAuthority inspctrlauthor)
        {
            _insCtrlAuthority = inspctrlauthor;
        }
        public InspectControlAuthority InspectAuthority { get { return _insCtrlAuthority; } }

        public ControlController(Control.ControlCollection controls)
        {
            IgnoreUnset = false;
            UpdateDefault = true;
            _controls = controls;
        }

        public void EnableAll(List<string> exception = null)
        {
            enableAll(_controls, true, exception);
        }

        public void DisableAll(List<string> exception = null)
        {
            enableAll(_controls, false, exception);
        }

        private void enableAll(Control.ControlCollection ctrl, bool enable, List<string> exception = null)
        {
            if (exception == null)
            {
                foreach (Control o in ctrl)
                {
                    if (o.Controls.Count > 0)
                        enableAll(o.Controls, enable, exception);
                    o.Enabled = enable;
                }
            }
            else
            {
                foreach (Control o in ctrl)
                {
                    if (!exception.Contains(o.Name))
                    {
                        if (o.Controls.Count > 0)
                            enableAll(o.Controls, enable, exception);
                        o.Enabled = enable;
                    }
                }
            }
        }

        private void updateByAuthor(Control.ControlCollection ctrls, EAuthenticationType type)
        {
            if (_ctrlAuthority == null)
            {
                if (UpdateDefault)
                    EnableAll();
                else
                    DisableAll();
                return;
            }
            switch (type)
            {
                case EAuthenticationType.Operator:
                    foreach (Control ctrl in ctrls)
                    {
                        if (ctrl is Label)
                            continue;

                        if (ctrl.Controls.Count > 0)
                            updateByAuthor(ctrl.Controls, type);

                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if( ctrl.Enabled )
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item1;
                        }
                        else
                        {
                            if (!IgnoreUnset)
                            {
                                if (ctrl.Enabled)
                                    ctrl.Enabled = UpdateDefault;
                            }
                        }
                    }
                    break;
                case EAuthenticationType.Administrator:
                    foreach (Control ctrl in ctrls)
                    {
                        if (ctrl is Label)
                            continue;

                        if (ctrl.Controls.Count > 0)
                            updateByAuthor(ctrl.Controls, type);

                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if( ctrl.Enabled )
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item2;
                        }
                        else
                        {
                            if (!IgnoreUnset)
                            {
                                if( ctrl.Enabled )
                                    ctrl.Enabled = UpdateDefault;
                            }
                        }
                    }
                    break;
                case EAuthenticationType.Developer:
                    foreach (Control ctrl in ctrls)
                    {
                        if (ctrl is Label)
                            continue;

                        if (ctrl.Controls.Count > 0)
                            updateByAuthor(ctrl.Controls, type);

                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if (ctrl.Enabled)
                            {
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item3;
                            }
                        }
                        else
                        {
                            if (!IgnoreUnset)
                            {
                                if( ctrl.Enabled )
                                    ctrl.Enabled = UpdateDefault;
                            }
                        }
                    }
                    break;
            }
        }

        public void UpdateByAuthor( EAuthenticationType type )
        {
            updateByAuthor(_controls, type);
        }


        private void updateByInspectControls(Control.ControlCollection ctrols, EAuthenticationType type, int iLevel)
        {
            switch (type)
            {
                case EAuthenticationType.Operator:
                    foreach (Control ctrl in ctrols)
                    {
                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name) && _insCtrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if( ctrl.Enabled )
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item1 & _insCtrlAuthority._dic[ctrl.Name];
                        }
                        else
                        {
                            if (!IgnoreUnset)
                                ctrl.Enabled = UpdateDefault;
                        }
                        if ( !(ctrl is UserControl) && ctrl.Controls.Count > 0)
                            updateByInspectControls(ctrl.Controls, type, ++iLevel);
                    }
                    break;
                case EAuthenticationType.Administrator:
                    foreach (Control ctrl in ctrols)
                    {
                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name) && _insCtrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if (ctrl.Enabled)
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item2 & _insCtrlAuthority._dic[ctrl.Name];
                        }
                        else
                        {
                            if (!IgnoreUnset)
                                ctrl.Enabled = UpdateDefault;
                        }
                        if (!(ctrl is UserControl) && ctrl.Controls.Count > 0)
                            updateByInspectControls(ctrl.Controls, type, ++iLevel);
                    }
                    break;
                case EAuthenticationType.Developer:
                    foreach (Control ctrl in ctrols)
                    {
                        if (_ctrlAuthority._dic.Keys.Contains(ctrl.Name) && _insCtrlAuthority._dic.Keys.Contains(ctrl.Name))
                        {
                            if (ctrl.Enabled)
                                ctrl.Enabled = _ctrlAuthority._dic[ctrl.Name].Item3 & _insCtrlAuthority._dic[ctrl.Name];
                        }
                        else
                        {
                            if (!IgnoreUnset)
                                ctrl.Enabled = UpdateDefault;
                        }
                        if( !(ctrl is UserControl) && ctrl.Controls.Count > 0 )
                            updateByInspectControls(ctrl.Controls, type, ++iLevel);
                    }
                    break;
            }
        }

        public void UpdateByInspect(EAuthenticationType type )
        {
            if (_ctrlAuthority == null && _insCtrlAuthority == null)
            {
                if (UpdateDefault)
                    EnableAll();
                else
                    DisableAll();
                return;
            }
            updateByInspectControls(_controls, type, 0);
        }
    }
}
