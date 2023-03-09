using AMControls.Charts.Fonts;
using AMControls.Charts.Implementations;
using AMControls.Charts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AMControls.Charts.Parallax
{
    public class ParallaxPlot : Canvas, IChart
    {
        // Axes
        private double _xLeft = 120;
        private double _yTop = 50;
        private double _xRight = 50;
        private double _yBottom = 50;

        private IAxes _yAxis = new LinearAxe();
        private List<IAxes> _yParallel = new();
        private double _axesSeparation = 120;
        private double _tickLength = 10;

        private Color _axisColor = Colors.Black;
        private double _axisLineWidth = 1;
        private bool _showGrid = true;
        private bool _useNormalizedData = true;

        // Measures
        private Rect _chartingArea = new();
        private Point _viewPosition;

        private double _distanceFitAll = 10;

        // Animations
        private bool[] _firstAnimation = { true };

        // Dataseries
        private List<IDataSeries> _dataSeries = new();

        // Legend
        private ILegend _legend = new Charts.Implementations.LegendBox();


        public ParallaxPlot()
        {
            this.Background = new SolidColorBrush(Colors.White);
            _viewPosition = new(_xLeft + 10, _yTop);
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            Draw_Main_Axes(dc);
            Draw_Parallel_Axes(dc);
            Draw_DataSeries(dc);

            _legend.Draw(dc, this, _chartingArea, _dataSeries);
        }

        #region Drawing
        private void Draw_Main_Axes(DrawingContext dc)
        {
            //_yAxis.MaxValue = _dataSeries.Max(e => e.DataPoints.Max(e => e.Y));
            //_yAxis.MinValue = _dataSeries.Min(e => e.DataPoints.Min(e => e.Y));


            _chartingArea = new(_xLeft,
                                _yTop,
                                this.ActualWidth - (_xLeft + _xRight),
                                this.ActualHeight - (_yTop + _yBottom));
            _yAxis.DrawInterval = _chartingArea.Height / _yAxis.Ticks;

            foreach (var item in _yParallel)
            {
                item.DrawInterval = _chartingArea.Height / item.Ticks;
            }

            // Draw Vertical y axes
            Point sVertical = new(_chartingArea.X, _chartingArea.Y);
            Point eVertical = new(_chartingArea.X, _chartingArea.Y + _chartingArea.Height);
            dc.DrawLine(new Pen(new SolidColorBrush(_axisColor), _axisLineWidth),
                        sVertical,
                        eVertical);

            // Draw Horizontal line
            Point sHorizontal = new(_chartingArea.X, _chartingArea.Y + _chartingArea.Height);
            Point eHorizontal = new(_chartingArea.X + _chartingArea.Width, _chartingArea.Y + _chartingArea.Height);
            dc.DrawLine(new Pen(new SolidColorBrush(_axisColor), _axisLineWidth),
                        sHorizontal,
                        eHorizontal);

        }

        private void Draw_Parallel_Axes(DrawingContext dc)
        {
            FONT_AxisLabel fLabel = new(this);

            //Draw Vertical data axis
            FONT_AxisData dLabel = new(this);
            for (int i = 0; i < _yAxis.Ticks; i++)
            {
                double cValue = i * _yAxis.DrawInterval;
                double tLength = _tickLength / 2;
                double dValue = _yAxis.MaxValue - i * _yAxis.Interval;

                Point sPoint = new(_chartingArea.X - tLength,
                                   _chartingArea.Y + cValue);
                Point ePoint = new(_chartingArea.X + tLength,
                                   _chartingArea.Y + cValue);

                dc.DrawLine(new Pen(new SolidColorBrush(_axisColor), _axisLineWidth),
                            sPoint,
                            ePoint);

                FormattedText dataLabel = dLabel.GetFormattedText(dValue.ToString("E2"));
                Point tPoint = new(sPoint.X - dataLabel.Width, sPoint.Y - dataLabel.Height/2);
                dLabel.Draw(dc, dValue.ToString("E2"), tPoint);
            }

            // Draw vertical series
            double xValue = _viewPosition.X;
            foreach (var item in _yParallel)
            {
                Rect axisR = new(xValue - _tickLength/2,
                                 _chartingArea.Y,
                                 _tickLength,
                                 _chartingArea.Height);


                if (!_chartingArea.IntersectsWith(axisR))
                {
                    xValue += _axesSeparation;
                    continue;
                }

                Point sPoint = new(xValue,
                                   axisR.Y);
                Point ePoint = new(xValue,
                                   axisR.Y + axisR.Height + _tickLength/2);
                dc.DrawLine(new Pen(new SolidColorBrush(_axisColor), _axisLineWidth),
                            sPoint,
                            ePoint);

                FormattedText dataLabel = dLabel.GetFormattedText(item.Name);
                Point tPoint = new(ePoint.X - dataLabel.Width/2, ePoint.Y + 5);
                dLabel.Draw(dc, item.Name, tPoint);

                xValue += _axesSeparation;
            }

        }

        private void Draw_DataSeries(DrawingContext dc)
        {
            List<double> aSep = new();
            List<double> yPar = new();
            List<double> vxStart = new();
            List<double> vyStart = new();

            foreach (var item in _yParallel)
            {
                aSep.Add(_axesSeparation);
                yPar.Add(item.DrawInterval/item.Interval);
                vxStart.Add(_viewPosition.X);
                vyStart.Add(item.MinValue);
            }

            int index = 0;
            foreach (var item in _dataSeries)
            {
                item.Draw(dc, this, _chartingArea, aSep, yPar, vxStart, vyStart);

                index++;
            }
        }
        #endregion

        #region Methods
        private void NormalizeData()
        {
            if (_dataSeries.Count == 0) return;
            if (_useNormalizedData)
            {
                _yAxis.MaxValue = 1;
                _yAxis.MinValue = 0;

                int index_series = 0;
                foreach (var item in _yParallel)
                {
                    item.DrawInterval = _chartingArea.Height / item.Ticks;

                    item.MaxValue = _dataSeries.Max(e => e.DataPoints[index_series].Y);
                    item.MinValue = _dataSeries.Min(e => e.DataPoints[index_series].Y);

                    item.MaxValue += item.MaxValue * 0.05;
                    item.MinValue -= item.MinValue * 0.05;

                    double tTest = (item.MaxValue - item.MinValue)/10;
                    index_series++;
                }
            }
            else
            {
                _yAxis.MaxValue = _dataSeries.Max(e => e.DataPoints.Max(e => e.Y));
                _yAxis.MinValue = _dataSeries.Min(e => e.DataPoints.Min(e => e.Y));
                _yAxis.MaxValue += _yAxis.MaxValue * 0.05;
                _yAxis.MinValue -= _yAxis.MinValue * 0.05;

                foreach (var item in _yParallel)
                {
                    item.DrawInterval = _yAxis.DrawInterval;
                    item.MaxValue = _yAxis.MaxValue;
                    item.MinValue = _yAxis.MaxValue;
                }
            }
        }

        private void Series_ToFront()
        {

        }
        #endregion

        #region Axes

        #endregion

        #region IChart
        public event EventHandler SelectionChanged;
        public event EventHandler ContextMenuClicked;
        public event EventHandler<MouseEventArgs> ChartAreaMouseMove;

        public void Add_series(IDataSeries value)
        {
            List<string> pList = value.Get_pointLabelList();
            if (_yParallel.Count != pList.Count)
            {
                _yParallel.Clear();

                foreach (var item in pList)
                {
                    _yParallel.Add(new LinearAxe() { Name = item });
                }
            }

            int index = 0;
            foreach (var item in value.DataPoints)
            {
                item.X = index;
                index++;
            }

            _dataSeries.Add(value);
            NormalizeData();
        }

        public void Adjust_axes_to_data()
        {
            throw new NotImplementedException();
        }

        public void Clear_Series()
        {
            throw new NotImplementedException();
        }

        public IDataPoint Get_Position(double x_mouse, double y_mouse)
        {
            throw new NotImplementedException();
        }

        public List<IDataPoint> Get_Selected_DataPoints()
        {
            throw new NotImplementedException();
        }

        public void Save_Image(string filename)
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
        #endregion

        #region MouseActions
        private Point Translate_StartPosition = new();
        private Point Scroll_Position = new();
        private bool _mouseDown = false;


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Point mouseLocation = e.GetPosition(this);
            if (_chartingArea.Contains(mouseLocation))
            {
                _mouseDown = true;
                Scroll_Position = new(_viewPosition.X, _viewPosition.Y);
                Translate_StartPosition = new(mouseLocation.X, mouseLocation.Y);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point mouseLocation = e.GetPosition(this);

            if (_mouseDown)
            {
                _viewPosition = new(Scroll_Position.X - Translate_StartPosition.X + mouseLocation.X,
                                    Scroll_Position.Y);
                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            _mouseDown = false;
        }

        #endregion
    }
}
