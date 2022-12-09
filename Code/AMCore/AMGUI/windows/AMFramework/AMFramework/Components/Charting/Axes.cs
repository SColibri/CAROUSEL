using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting
{
    [Obsolete("Use AMControls instead")]
    public class Axes
    {
        public Axes()
        {

        }
        public Axes(String Name)
        {
            this._Name = Name;
        }

        private String _Name = "";
        private double _MinValue = 0;
        private double _MaxValue = 0.00001;
        private double _Interval = 1;
        private bool _IsVisible = true;

        #region Getters_setters
        public String Name { get { return _Name; } set { _Name = value; } }
        public double MinValue { get { return _MinValue; } set { _MinValue = value; Check_minmax_values(); } }
        public double MaxValue { get { return _MaxValue; } set { _MaxValue = value; Check_minmax_values(); } }
        public double Interval { get { return _Interval; } 
            set { 
                
                if(value > 0)
                {
                   _Interval = value;
                }
                
            } 
        }
        public bool IsVisible { get { return _IsVisible; } set { _IsVisible = value; } }
        #endregion

        #region checkers
        private void Check_minmax_values() 
        {
            if (_MaxValue <= _MinValue)
            {
                _MaxValue = _MinValue + 1;
            }

            if (Interval >= _MaxValue - _MinValue) 
            { 
                Interval = (_MaxValue - _MinValue)/5;
            }
        }
        #endregion

    }
}
