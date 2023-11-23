using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adjustment
{
    class TrackbarValueChangeTimerElapsedEventArgs
    {
        public System.Windows.Forms.TrackBar TargetTrackBar { get; private set; }

        public TrackbarValueChangeTimerElapsedEventArgs(System.Windows.Forms.TrackBar tb)
        {
            TargetTrackBar = tb;
        }
    }

    delegate void TrackbarValueChangeTimerElapsedEvent( object sender, TrackbarValueChangeTimerElapsedEventArgs e );

    class clsTrackbarWait
    {
        public event TrackbarValueChangeTimerElapsedEvent TrackbarValueChangeTimerElapsed;
        public int Interval
        {
            get
            {
                return _tmrControl.Interval;
            }
            set
            {
                _tmrControl.Interval = value;
            }
        }

        public bool Start
        {
            get;
            set;
        }

        System.Windows.Forms.Timer _tmrControl = null;
        List<System.Windows.Forms.TrackBar> _lstTrackbar = new List<System.Windows.Forms.TrackBar>();
        System.Windows.Forms.TrackBar _CurrentTrackbar = null;

        public clsTrackbarWait(System.Windows.Forms.Timer tmr)
        {
            if (tmr != null)
                _tmrControl = tmr;
            else
                _tmrControl = new System.Windows.Forms.Timer();

            _tmrControl.Tick += new EventHandler(tmr_Tick);
        }


        void tmr_Tick(object sender, EventArgs e)
        {
            if (!Start)
            {
                _CurrentTrackbar = null;
                return;
            }

            if (TrackbarValueChangeTimerElapsed != null)
            {
                TrackbarValueChangeTimerElapsed(this, new TrackbarValueChangeTimerElapsedEventArgs(_CurrentTrackbar));
            }
            _tmrControl.Enabled = false;
            _CurrentTrackbar = null;
        }

        public bool AddTrackbar(System.Windows.Forms.TrackBar trb)
        {
            for (int i = 0; i < _lstTrackbar.Count; i++)
            {
                if (_lstTrackbar[i] == trb)
                    return false;
            }

            trb.ValueChanged += new EventHandler(trb_ValueChanged);
            _lstTrackbar.Add(trb);

            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < _lstTrackbar.Count; i++)
            {
                _lstTrackbar[i].ValueChanged -= new EventHandler(trb_ValueChanged);
            }
            _lstTrackbar.Clear();
        }

        void trb_ValueChanged(object sender, EventArgs e)
        {
            if (!Start)
                return;

            System.Windows.Forms.TrackBar newTarget = (System.Windows.Forms.TrackBar)sender;
            _tmrControl.Enabled = false;
            if (_CurrentTrackbar != null && _CurrentTrackbar != newTarget)
            {
                if (TrackbarValueChangeTimerElapsed != null)
                {
                    TrackbarValueChangeTimerElapsed(this, new TrackbarValueChangeTimerElapsedEventArgs(_CurrentTrackbar));
                }
                _CurrentTrackbar = null;
            }

            _CurrentTrackbar = newTarget;
            _tmrControl.Enabled = true;
        }
    }
}
