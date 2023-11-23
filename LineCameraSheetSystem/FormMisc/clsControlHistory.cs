using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Fujita.Misc;
using ViewROI;

#if FUJITA_INSPECTION_SYSTEM
using Fujita.InspectionSystem;
#endif

namespace Fujita.FormMisc
{
    class clsControlHistory
    {
        Dictionary<Control, object> _dicParams = new Dictionary<Control, object>();

        List<Control> _lstUnregist = new List<Control>();

        public delegate void RestoreBeginEventHandler(object sender, EventArgs e);
        public delegate void RestoreEndEventHandler(object sender, EventArgs e);

        public event RestoreBeginEventHandler OnRestoreBegin = null;
        public event RestoreEndEventHandler OnRestoreEnd = null;

        public bool UnregistTextBox
        {
            get;
            set;
        }

        public bool UnregistCheckBox
        {
            get;
            set;
        }

        public bool UnregistRadioButton
        {
            get;
            set;
        }

        public bool UnregistNumericUpDown
        {
            get;
            set;
        }

        public bool UnregistTrackBar
        {
            get;
            set;
        }

        public bool UnregistNumericInput
        {
            get;
            set;
        }
        public bool UnregistComboBox
        {
            get;
            set;
        }   

        public void UnregistControls(Control[] unregist)
        {
            _lstUnregist.AddRange(unregist);
        }

        public void SetControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if( _dicParams.ContainsKey( ctrl ))
                    continue;
                if (_lstUnregist.Contains(ctrl))
                    continue;
                if (ctrl.Visible == false)
                    continue;

                if (ctrl is TextBox)
                {
                    if( !UnregistTextBox )
                        _dicParams.Add(ctrl, ((TextBox)ctrl).Text);
                }
                else if (ctrl is CheckBox)
                {
                    if( !UnregistCheckBox )
                        _dicParams.Add(ctrl, ((CheckBox)ctrl).Checked);
                }
                else if (ctrl is RadioButton)
                {
                    if( !UnregistRadioButton )
                        _dicParams.Add(ctrl, ((RadioButton)ctrl).Checked);
                }
                else if (ctrl is NumericUpDown)
                {
                    if( !UnregistNumericUpDown )
                        _dicParams.Add(ctrl, ((NumericUpDown)ctrl).Value);
                }
                else if (ctrl is TrackBar)
                {
                    if( !UnregistTrackBar )
                        _dicParams.Add(ctrl, ((TrackBar)ctrl).Value);
                }
                else if (ctrl is ComboBox)
                {
                    if (!UnregistComboBox)
                        _dicParams.Add(ctrl, (EDispPlane)((ComboBox)ctrl).SelectedValue);
                }
#if FUJITA_INSPECTION_SYSTEM
                else if (ctrl is uclNumericInput)
                {
                    if (!UnregistNumericInput)
                        _dicParams.Add(ctrl, ((uclNumericInput)ctrl).Value);
                }
#endif
                else if (ctrl.Controls.Count > 0)
                {
                    SetControls(ctrl.Controls);
                }
            }
        }

        public void StoreParam()
        {
            Control[] ctrls =  _dicParams.Keys.ToArray();
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    _dicParams[ctrl] = ((TextBox)ctrl).Text;
                if (ctrl is CheckBox)
                    _dicParams[ctrl] = ((CheckBox)ctrl).Checked;
                if (ctrl is RadioButton)
                    _dicParams[ctrl] = ((RadioButton)ctrl).Checked;
                if (ctrl is NumericUpDown)
                    _dicParams[ctrl] = ((NumericUpDown)ctrl).Value;
                if (ctrl is TrackBar)
                    _dicParams[ctrl] = ((TrackBar)ctrl).Value;
                if (ctrl is ComboBox)
                    _dicParams[ctrl] = (EDispPlane)((ComboBox)ctrl).SelectedValue;
#if FUJITA_INSPECTION_SYSTEM
                if (ctrl is uclNumericInput)
                    _dicParams[ctrl] = ((uclNumericInput)ctrl).Value;
#endif
            }
        }

        public void RestoreParam()
        {
            if (OnRestoreBegin != null)
                OnRestoreBegin(this, new EventArgs());

            foreach (Control ctrl in _dicParams.Keys)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Text = (string)_dicParams[ctrl];
                }
                if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Checked = (bool)_dicParams[ctrl];
                }
                if (ctrl is RadioButton)
                {
                    ((RadioButton)ctrl).Checked = (bool)_dicParams[ctrl];
                }
                if (ctrl is NumericUpDown)
                {
                    ((NumericUpDown)ctrl).Value = (decimal)_dicParams[ctrl];
                }
                if (ctrl is TrackBar)
                {
                    ((TrackBar)ctrl).Value = (int)_dicParams[ctrl];
                }
                if (ctrl is ComboBox)
                {
                    ((ComboBox)ctrl).SelectedValue = (EDispPlane)_dicParams[ctrl];
                }
#if FUJITA_INSPECTION_SYSTEM
                if (ctrl is uclNumericInput)
                {
                    ((uclNumericInput)ctrl).Value = (decimal)_dicParams[ctrl];
                }
#endif
            }

            if (OnRestoreEnd != null)
                OnRestoreEnd(this, new EventArgs());
        }

        private string getcontrolPath(Control ctrl)
        {
            if (ctrl.Parent != null)
                return getcontrolPath(ctrl.Parent) + ":" + ctrl.Name;
            return ctrl.Name;
        }


        public bool SaveParam(string sPath)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sRealSection;
            foreach (Control ctrl in _dicParams.Keys)
            {
                sRealSection = getcontrolPath(ctrl);
                if (ctrl is TextBox)
                {
                    ifa.SetIni(sRealSection, "Type", "TextBox", sPath);
                    ifa.SetIni(sRealSection, "Value", ((TextBox)ctrl).Text, sPath);
                }
                if (ctrl is CheckBox)
                {
                    ifa.SetIni(sRealSection, "Type", "CheckBox", sPath);
                    ifa.SetIni(sRealSection, "Value", ((CheckBox)ctrl).Checked.ToString(), sPath);
                }
                if (ctrl is RadioButton)
                {
                    ifa.SetIni(sRealSection, "Type", "RadioButton", sPath);
                    ifa.SetIni(sRealSection, "Value", ((RadioButton)ctrl).Checked.ToString(), sPath);
                }

                if (ctrl is NumericUpDown)
                {
                    ifa.SetIni(sRealSection, "Type", "NumericUpDown", sPath);
                    ifa.SetIni(sRealSection, "Value", ((NumericUpDown)ctrl).Value.ToString(), sPath);
                }
                if (ctrl is TrackBar)
                {
                    ifa.SetIni(sRealSection, "Type", "TrackBar", sPath);
                    ifa.SetIni(sRealSection, "Value", ((TrackBar)ctrl).Value.ToString(), sPath);
                }
                if (ctrl is ComboBox)
                {
                    ifa.SetIni(sRealSection, "Type", "ComboBox", sPath);
                    ifa.SetIni(sRealSection, "Value", (EDispPlane)((ComboBox)ctrl).SelectedValue, sPath);
                }
#if FUJITA_INSPECTION_SYSTEM
                if (ctrl is uclNumericInput)
                {
                    ifa.SetIni(sRealSection, "Type", "uclNumericInput", sPath);
                    ifa.SetIni(sRealSection, "Value", ((uclNumericInput)ctrl).Value.ToString(), sPath);
                }
#endif
                if (ctrl.Tag != null)
                {
                    ifa.SetIni(sRealSection, "Tag", ctrl.Tag.ToString(), sPath);
                }

            }

            return true;
        }

        public class Params
        {
            public Params(string t, object v, string tag, string secname)
            {
                Type = t;
                Value = v;
                Tags = tag;
                SectionName = secname;
            }

            public string Type;
            public object Value;
            public string Tags;
            public string SectionName;
        }

        public static bool LoadParam( string sPath, out List<Params> lstParams )
        {
            lstParams = new List<Params>();

            if (!File.Exists(sPath))
                return false;

            IniFileAccess ifa = new IniFileAccess();
            string [] sSections = ifa.GetIniSectionNames(sPath);

            if (sSections == null || sSections.Length == 0)
                return false;

            bool bResult = true;
            for (int i = 0; i < sSections.Length; i++)
            {
                string sType = ifa.GetIni(sSections[i], "Type", "", sPath);

                if (sType == "")
                {
                    bResult = false;
                    continue;
                }

                object obj = null;
                try
                {
                    switch (sType)
                    {
                        case "TextBox":
                            obj = ifa.GetIni(sSections[i], "Value", "", sPath);
                            break;
                        case "CheckBox":
                            obj = bool.Parse(ifa.GetIni(sSections[i], "Value", "", sPath));
                            break;
                        case "RadioButton":
                            obj = bool.Parse(ifa.GetIni(sSections[i], "Value", "", sPath));
                            break;
                        case "NumericUpDown":
                            obj = decimal.Parse(ifa.GetIni(sSections[i], "Value", "", sPath));
                            break;
                        case "TrackBar":
                            obj = int.Parse(ifa.GetIni(sSections[i], "Value", "", sPath));
                            break;
                        case "ComboBox":
                            obj = ifa.GetIni(sSections[i],"Value","",sPath);
                            break;
#if FUJITA_INSPECTION_SYSTEM
                        case "uclNumericInput":
                            obj = decimal.Parse(ifa.GetIni(sSections[i], "Value", "", sPath));
                            break;
#endif
                        default:
                            {
                                bResult = false;
                                continue;
                            }
                    }
                }
                catch (FormatException)
                {
                    bResult = false;
                    continue;
                }
                string sTags = ifa.GetIni( sSections[i], "Tag", "", sPath );
                lstParams.Add(new Params(sType, obj, sTags, sSections[i]));
            }
            return bResult;
        }

        public bool LoadParam(string sPath)
        {
            IniFileAccess ifa = new IniFileAccess();
            string sRealSection;
            bool bResult = true;

            if (OnRestoreBegin != null)
                OnRestoreBegin(this, new EventArgs());

            foreach (Control ctrl in _dicParams.Keys)
            {
                sRealSection = getcontrolPath(ctrl);
                if (ctrl is TextBox)
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "TextBox")
                    {
                        ((TextBox)ctrl).Text = ifa.GetIni(sRealSection, "Value", "", sPath);
                    }
                    else
                    {
                        bResult = false;
                    }
                }

                if (ctrl is CheckBox)
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "CheckBox")
                    {
                        ((CheckBox)ctrl).Checked = bool.Parse(ifa.GetIni(sRealSection, "Value", "false", sPath));
                    }
                    else
                    {
                        bResult = false;
                    }
                }

                if (ctrl is RadioButton)
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "RadioButton")
                    {
                        ((RadioButton)ctrl).Checked = bool.Parse(ifa.GetIni(sRealSection, "Value", "false", sPath));
                    }
                    else
                    {
                        bResult = false;
                    }
                }

                if (ctrl is NumericUpDown )
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "NumericUpDown")
                    {
                        ((NumericUpDown)ctrl).Value = decimal.Parse(ifa.GetIni(sRealSection, "Value", ((NumericUpDown)ctrl).MinimumSize.ToString(), sPath));
                    }
                    else
                    {
                        bResult = false;
                    }
                }

                if( ctrl is TrackBar )
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "TrackBar")
                    {
                        ((TrackBar)ctrl).Value = int.Parse(ifa.GetIni(sRealSection, "Value", ((TrackBar)ctrl).Minimum.ToString(), sPath));
                    }
                    else
                    {
                        bResult = false;
                    }
                }

                if (ctrl is ComboBox)
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "ComboBox")
                    {
                        //((ComboBox)ctrl).SelectedValue = ifa.GetIni(sRealSection, "Value",((EDispPlane)((ComboBox)ctrl).SelectedValue).ToString(), sPath);
                        ((ComboBox)ctrl).SelectedValue = (EDispPlane)ifa.GetIniEnum(sRealSection, "Value", typeof(EDispPlane), sPath, (EDispPlane)((ComboBox)ctrl).SelectedValue);
                    }
                    else
                    {
                        bResult = false;
                    }
                }
#if FUJITA_INSPECTION_SYSTEM
                if (ctrl is uclNumericInput)
                {
                    if (ifa.GetIni(sRealSection, "Type", "", sPath) == "uclNumericInput")
                    {
                        ((uclNumericInput)ctrl).Value = decimal.Parse(ifa.GetIni(sRealSection, "Value", ((uclNumericInput)ctrl).Minimum.ToString(), sPath));
                    }
                    else
                    {
                        bResult = false;
                    }
                }
#endif
            }

            if (OnRestoreEnd != null)
                OnRestoreEnd(this, new EventArgs());

            return bResult;
        }
    }
}
