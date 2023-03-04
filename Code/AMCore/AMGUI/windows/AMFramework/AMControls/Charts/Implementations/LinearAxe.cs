using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations
{
    public class LinearAxe : DrawObject_Abstract, IAxes
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
                _MaxValue = _MinValue + 0.00000000001 * _MinValue;
            }

            _Interval = (_MaxValue - _MinValue) / _ticks;
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
        // IAxes 
        public double MaxValue { get { return _MaxValue; } set { _MaxValue = value; Check_minmax_values(); } }

        public double MinValue { get { return _MinValue; } set { _MinValue = value; Check_minmax_values(); } }

        public string Name { get { return _Name; } set { _Name = value; } }

        public double Interval { get { return _Interval; } set { _Interval = value; Check_minmax_values(); } }

        public List<double> Intervals()
        {
            return new List<double> { _Interval };
        }

        public IAxes.Orientation AxisOrientation { get; set; }
        public int Ticks
        {
            get { return _ticks; }
            set
            {
                _ticks = value;
                Check_minmax_values();
            }
        }

        public double DrawInterval { get; set; }
        public double DrawLineLength { get; set; } = 0;
        public System.Windows.Point StartPoint { get; set; } = new();
        public System.Windows.Point EndPoint { get; set; } = new();

        // IDrawable
        public string IntervalNotation()
        {
            return _IntervalNotation;
        }

        public override void Mouse_Hover_Action(double x, double y)
        {
            // Do nothing
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            // Do nothing
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            // Do nothing
        }

        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            // Do nothing
        }

        public System.Windows.Point ValueToPoint(double value)
        {
            System.Windows.Point dPoint = new(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
            double dDistance = Math.Sqrt(dPoint.X* dPoint.X + dPoint.Y* dPoint.Y);
            System.Windows.Point dNorm = new(dPoint.X/dDistance, dPoint.Y / dDistance);

            double lineLength = DrawInterval / Interval * (value - MinValue);
            double xPos = StartPoint.X + lineLength * dNorm.X;
            double yPos = StartPoint.Y + lineLength * dNorm.Y;

            System.Windows.Point point = new System.Windows.Point(xPos, yPos);
            return point;
        }




        #endregion
    }
}
