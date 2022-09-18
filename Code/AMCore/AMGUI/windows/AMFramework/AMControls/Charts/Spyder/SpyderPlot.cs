using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Spyder
{
    internal class SpyderPlot : Canvas, IChart
    {
        // Axes
        private List<IAxes> _axes;
        private List<System.Windows.Point> _axisUnitVector;

        // Data
        private List<IDataSeries> _dataSeries;

        // Drawing parameters
        private double _angleSplit = 0;
        private double _margin = 70;
        private double _drawingRadius = 0;
        private System.Windows.Point _center;

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
        private FontFamily _axisHeader_FontFamily = new("Lucida Sans");
        private int _axisHeader_FontSize = 12;
        private System.Windows.FontStyle _axisHeader_FontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisHeader_FontWeight = System.Windows.FontWeights.Light; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisHeader_FontStretch = System.Windows.FontStretches.Normal;
        private System.Windows.Media.Color _axisHeaderFontColor = Colors.Black;


        public SpyderPlot() 
        {
            _axes = new();
            _axisUnitVector = new();
            _dataSeries = new();

            _center = new(this.Width/2, this.Height/2);
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

            _center = new(this.Width / 2, this.Height / 2);
            _drawingRadius = Math.Min(_center.X, _center.Y) - _margin;
            Draw_Base(dc);
        }

        private void Draw_Base(DrawingContext dc) 
        {

            dc.DrawEllipse(new SolidColorBrush(_base_BorderColor), 
                           new Pen(new SolidColorBrush(_base_BorderColor), _base_Thickness), 
                           _center, _drawingRadius, _drawingRadius);

        }

        private void Draw_Axes(DrawingContext dc) 
        {
            double currentAngle = 0;
            for (int n1 = 0; n1 < _axes.Count; n1++) 
            {
                System.Windows.Point _pointToBorder = new(_center.X + Math.Cos(currentAngle)* _drawingRadius, 
                                                          _center.Y + Math.Sin(currentAngle)*_drawingRadius);

                double lineLength = Math.Abs(System.Windows.Point.Subtract(_center, _pointToBorder).Length);

                _axisUnitVector[n1] = new System.Windows.Point(_pointToBorder.X / lineLength,
                                                               _pointToBorder.Y / lineLength);

                System.Windows.Point perpendicular_vector = new System.Windows.Point(-_axisUnitVector[n1].Y, _axisUnitVector[n1].X);

                //_axes[n1].
                //dc.DrawLine(new Pen(new SolidColorBrush(_axis_BorderColor), _axis_Thickness), _center, );

                currentAngle += _angleSplit;
            }
        }
    }
}
