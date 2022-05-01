using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting
{
    internal class Axes
    {
        private String _Name = "";
        public String Name { get { return _Name; } set { _Name = value; } }

        private double _MinValue = 0;
        public double MinValue { get { return _MinValue; } set { _MinValue = value; } }

        private double _MaxValue = 0;
        public double MaxValue { get { return _MaxValue; } set { _MaxValue = value; } }

        private double _Interval = 1;
        public double Interval { 
            get { return _Interval; } 
            set { 
                
                if(value > 0)
                {
                   _Interval = value;
                }
                
            } 
        }

        private bool _IsVisible = true;
        public bool IsVisible { get { return _IsVisible; }}

        public Axes()
        {

        }

        public Axes(String Name)
        {
            this._Name = Name;
        }



    }
}
