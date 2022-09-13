using System.Collections.Generic;
using System.Drawing;

namespace AMControls.Charts
{
    public class LinearAxe : IAxes
    {
        private string _Name = "";
        private double _MinValue = 0;
        private double _MaxValue = 0.00001;
        private double _Interval = 1;
        private bool _IsVisible = true;
        private Rectangle _Bounds;
        private string _IntervalNotation = "E2";
        private IAxes.Orientation _AxisOrientation = IAxes.Orientation.HORIZONTAL;
        private int _ticks = 10;

        public LinearAxe() { }
        public LinearAxe(string name) { _Name = name; }

        #region Check
        private void Check_minmax_values()
        {
            if (_MaxValue <= _MinValue)
            {
                _MaxValue = _MinValue + 0.00000000001* _MinValue;
            }

            if ( (_MaxValue - _MinValue)/Interval != _ticks)
            {
                _Interval = (_MaxValue - _MinValue) / _ticks;
            }
        }
        #endregion
        #region Setters
        public void Set_IsVisible(bool value)
        {
            _IsVisible = value;
        }

        public void Set_MaxValue(double value)
        {
           _MaxValue = value; Check_minmax_values();
        }

        public void Set_MinValue(double value)
        {
            _MinValue = value; Check_minmax_values();
        }

        public void Set_Name(string value)
        {
            _Name = value;
        }

        public void Set_Interval(double value)
        {
            _Interval = value;
        }

        public void Set_IntervalNotation(string value) 
        {
            _IntervalNotation = value;
        }
        
        #endregion
        #region Implementation
        public bool IsVisible()
        {
            return _IsVisible;
        }

        public double MaxValue { get { return _MaxValue; } set { _MaxValue = value; Check_minmax_values(); } }

        public double MinValue { get { return _MinValue; } set { _MinValue = value; Check_minmax_values(); } }

        public string Name { get { return _Name; } set { _Name = value; } }

        public double Interval { get { return _Interval; } set { _Interval = value; Check_minmax_values(); } }

        public List<double> Intervals()
        {
            return new List<double> { _Interval };
        }

        public IAxes.Orientation AxisOrientation { get; set; }
        public int Ticks { get { return _ticks; } set { _ticks = value; } } 
        
        // IObjectInteraction
        public Rectangle Bounds { get { return _Bounds; } set { _Bounds = value; } }

        public bool IsInside(int x, int y)
        {
            Point LocationPoint = new(x, y);
            return Bounds.Contains(LocationPoint);
        }

        public string IntervalNotation() 
        {
            return _IntervalNotation;
        }

        #endregion
    }
}
