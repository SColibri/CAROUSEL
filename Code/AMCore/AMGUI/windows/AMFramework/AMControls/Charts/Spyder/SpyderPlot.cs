using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using AMControls.Charts.Fonts;
using AMControls.Charts.Interfaces;

namespace AMControls.Charts.Spyder
{
    public class SpyderPlot : Canvas, IChart
    {
        // Axes
        private List<IAxes> _axes;
        private List<System.Windows.Point> _axisUnitVector;

        // Data
        private List<IDataSeries> _dataSeries;

        // Drawing parameters
        private double _angleSplit = 0;
        private double _currentAngle = 0;
        private double _margin = 50;
        private double _drawingRadius = 0;
        private System.Windows.Point _center;
        private System.Windows.Rect _chartArea;
        private System.Windows.Rect _chartSpyderArea = new();

        // Base
        private System.Windows.Media.Color _base_Background = Colors.White;
        private System.Windows.Media.Color _base_BorderColor = Colors.Black;
        private double _base_Thickness = 1;

        // Axes
        private System.Windows.Media.Color _axis_BorderColor = Colors.Black;
        private double _axis_Thickness = 1;

        // Intermediate Tick lines
        private System.Windows.Media.Color _axisTick_BorderColor = Colors.Black;
        private double _axisTick_Thickness = 0.5;
        private FontFamily _axisTick_FontFamily = new("Lucida Sans");
        private int _axisTick_FontSize = 8;
        private System.Windows.FontStyle _axisTick_FontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisTick_FontWeight = System.Windows.FontWeights.Light; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisTick_FontStretch = System.Windows.FontStretches.Normal;
        private System.Windows.Media.Color _axisTickFontColor = Colors.Black;

        // Axis Header
        private FONT_AxisLabel _axisLabel;
        private FontFamily _axisHeader_FontFamily = new("Lucida Sans");
        private int _axisHeader_FontSize = 12;
        private System.Windows.FontStyle _axisHeader_FontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisHeader_FontWeight = System.Windows.FontWeights.Light; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisHeader_FontStretch = System.Windows.FontStretches.Normal;
        private System.Windows.Media.Color _axisHeaderFontColor = Colors.Black;

        public event EventHandler SelectionChanged;
        public event EventHandler ContextMenuClicked;
        public event EventHandler<MouseEventArgs> ChartAreaMouseMove;

        // Legend
        private ILegend _legend = new Charts.Implementations.LegendBox();

        public SpyderPlot() 
        {
            _axes = new();
            _axisUnitVector = new();
            _dataSeries = new();

            _axisLabel = new(this);
        }


        public void Add_axis(IAxes newaxis) 
        {
            _axes.Add(newaxis);
            _angleSplit = 360 / _axes.Count;
            _axisUnitVector.Add(new());
        }

        public void Add_series(IDataSeries dSeries) 
        { 
            if(_axes.Count == dSeries.DataPoints.Count) 
            {
                _dataSeries.Add(dSeries);
            }
            else 
            { 
                // send error
            }
        }

        public void Save_Image(string filename)
        {
            throw new NotImplementedException();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            _center = new(this.ActualWidth / 2, this.ActualHeight / 2);
            _drawingRadius = Math.Min(_center.X, _center.Y) - _margin;
            _chartSpyderArea = new(new System.Windows.Point(_center.X - _drawingRadius, _center.Y - _drawingRadius), 
                                   new System.Windows.Size(_drawingRadius * 2, _drawingRadius * 2));

            Draw_Base(dc);
            Draw_Axes(dc);

            foreach (var item in _dataSeries)
            {
                item.Draw(dc, this, _chartArea, _axes);
            }

            _legend.Draw(dc, this, _chartArea, _dataSeries);
        }

        private void Draw_Base(DrawingContext dc) 
        {

            dc.DrawEllipse(new SolidColorBrush(Colors.White), 
                           new Pen(new SolidColorBrush(_base_BorderColor), _base_Thickness), 
                           _center, _drawingRadius, _drawingRadius);

        }

        private void Draw_Axes(DrawingContext dc) 
        {
            _currentAngle = _currentAngle + 1;
            double currentAngle = _currentAngle;
            _chartArea = new(new System.Windows.Point(0,0), new System.Windows.Size(this.ActualWidth, this.ActualHeight));

            for (int n1 = 0; n1 < _axes.Count; n1++) 
            {
                System.Windows.Point _pointToBorder = new(_center.X + Math.Cos(currentAngle * 0.0174533)* _drawingRadius, 
                                                          _center.Y + Math.Sin(currentAngle * 0.0174533) *_drawingRadius);

                double lineLength = Math.Abs(System.Windows.Point.Subtract(_center, _pointToBorder).Length);

                _axisUnitVector[n1] = new System.Windows.Point((_pointToBorder.X - _center.X) / lineLength,
                                                               (_pointToBorder.Y - _center.Y) / lineLength);

                _axes[n1].EndPoint = _pointToBorder;
                _axes[n1].StartPoint = _center;
                _axes[n1].DrawInterval = lineLength / _axes[n1].Ticks;
                _axes[n1].DrawLineLength = lineLength;

                dc.DrawLine(new Pen(new SolidColorBrush(_axis_BorderColor), _axis_Thickness), _center, _pointToBorder);
                Draw_AxisLabel(dc, _axes[n1], currentAngle);

                currentAngle += _angleSplit;
            }

            Draw_AxisConnectingLines(dc);
            Draw_AxisTicks(dc);
        }

        private void Draw_AxisLabel(DrawingContext dc, IAxes axis, double currentAngle) 
        {
            double realAngle =  (currentAngle % 360);

            _axisLabel.Text = axis.Name + " " + currentAngle;

            FormattedText fText = _axisLabel.GetFormattedText();
            System.Windows.Size textSize = new(fText.Width, fText.Height);

            _axisLabel.Location = new(axis.EndPoint.X, axis.EndPoint.Y);
            
            if(realAngle <= 25 && realAngle >= 0) 
            {
                _axisLabel.Location = new(axis.EndPoint.X + 20, axis.EndPoint.Y - textSize.Height/2);
            }
            else if (realAngle <= 50 && realAngle >= 25)
            {
                _axisLabel.Location = new(axis.EndPoint.X + 20, axis.EndPoint.Y + textSize.Height);
            }
            else if (realAngle <= 75 && realAngle >= 50)
            {
                _axisLabel.Location = new(axis.EndPoint.X, axis.EndPoint.Y + textSize.Height);
            }
            else if (realAngle <= 100 && realAngle >= 75)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width/2, axis.EndPoint.Y + textSize.Height);
            }
            else if (realAngle <= 125 && realAngle >= 100)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 10, axis.EndPoint.Y + textSize.Height/2);
            }
            else if (realAngle <= 150 && realAngle >= 125)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 10, axis.EndPoint.Y + textSize.Height / 2);
            }
            else if (realAngle <= 175 && realAngle >= 150)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 10, axis.EndPoint.Y + textSize.Height / 2);
            }
            else if (realAngle <= 200 && realAngle >= 175)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 20, axis.EndPoint.Y - textSize.Height / 2);
            }
            else if (realAngle <= 225 && realAngle >= 200)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 20, axis.EndPoint.Y - textSize.Height);
            }
            else if (realAngle <= 250 && realAngle >= 225)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width - 10, axis.EndPoint.Y - textSize.Height - 10);
            }
            else if (realAngle <= 275 && realAngle >= 250)
            {
                _axisLabel.Location = new(axis.EndPoint.X - textSize.Width/2, axis.EndPoint.Y - textSize.Height - 10);
            }
            else if (realAngle <= 300 && realAngle >= 275)
            {
                _axisLabel.Location = new(axis.EndPoint.X + 10, axis.EndPoint.Y - textSize.Height - 10);
            }
            else if (realAngle <= 325 && realAngle >= 300)
            {
                _axisLabel.Location = new(axis.EndPoint.X + 10, axis.EndPoint.Y - textSize.Height - 10);
            }
            else if (realAngle <= 350 && realAngle >= 325)
            {
                _axisLabel.Location = new(axis.EndPoint.X + 20, axis.EndPoint.Y - textSize.Height);
            }
            else if (realAngle <= 360 && realAngle >= 350)
            {
                _axisLabel.Location = new(axis.EndPoint.X + 20, axis.EndPoint.Y - textSize.Height/2);
            }

            RotateTransform rt = new(currentAngle, axis.StartPoint.X, axis.StartPoint.Y);

            _axisLabel.Draw(dc);

        }

        private void Draw_AxisTicks(DrawingContext dc) 
        {
            for (int n1 = 0; n1 < _axes.Count; n1++) 
            { 
                System.Windows.Point perpendicular_vector = new System.Windows.Point(-_axisUnitVector[n1].Y, _axisUnitVector[n1].X);

                double currValue = _axes[n1].MinValue;
                for (int n2 = 0; n2 < _axes[n1].Ticks - 1; n2++)
                {
                    currValue += _axes[n1].Interval;

                    System.Windows.Point locAxis = _axes[n1].ValueToPoint(currValue);
                    System.Windows.Point topLine = new(locAxis.X + perpendicular_vector.X * 3, locAxis.Y + perpendicular_vector.Y * 3);
                    System.Windows.Point botLine = new(locAxis.X - perpendicular_vector.X * 3, locAxis.Y - perpendicular_vector.Y * 3);

                    dc.DrawLine(new Pen(new SolidColorBrush(_axis_BorderColor), _axis_Thickness), locAxis, topLine);
                    dc.DrawLine(new Pen(new SolidColorBrush(_axis_BorderColor), _axis_Thickness), locAxis, botLine);
                }
            }
        }

        private void Draw_AxisConnectingLines(DrawingContext dc) 
        {
            if (_axes.Count <= 1) return;

            System.Windows.Point pInitial;
            for (int i = 1; i < _axes[0].Ticks; i++)
            {
                System.Windows.Point pStart = _axes[0].ValueToPoint(_axes[0].MinValue + _axes[0].Interval * i);
                pInitial = new(pStart.X, pStart.Y);

                foreach (var item in _axes.Skip(1))
                {
                    System.Windows.Point pEnd = item.ValueToPoint(item.MinValue + item.Interval * i);
                    dc.DrawLine(new Pen(new SolidColorBrush(Colors.Silver), 0.4), pStart, pEnd);
                    pStart = pEnd;
                }

                dc.DrawLine(new Pen(new SolidColorBrush(Colors.Silver), 0.4), pInitial, pStart);
            }
        }

        public void Clear_Series()
        {
            throw new NotImplementedException();
        }

        public void Adjust_axes_to_data()
        {
            throw new NotImplementedException();
        }

        public List<IDataPoint> Get_Selected_DataPoints()
        {
            throw new NotImplementedException();
        }

        public List<IDataPoint> Search(string searchString)
        {
            throw new NotImplementedException();
        }

        public List<IDataPoint> Search(double x, double y, double tolerance)
        {
            throw new NotImplementedException();
        }

        public IDataPoint Get_Position(double x_mouse, double y_mouse)
        {
            throw new NotImplementedException();
        }
    }
}
