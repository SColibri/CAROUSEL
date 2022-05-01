using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AMFramework.Components.Charting
{
    internal class ChartingWindow:Canvas
    {
        private int _DataLength = 0;
        private Point _chartCenter = new();
        private List<Point> _AxesUnitVectors = new();
        private List<double> _AxesStepSize = new();

        private List<Axes> _AxesList = new();
        public List<Axes> AxesList { get { return _AxesList; } }

        private List<List<double>> _Data = new();
        public List <List<double>> Data { 
            get { return _Data; } 
            set 
            {
                bool dataPass = true;

                if(AxesList.Count != value.Count)
                {
                    dataPass = false;
                }

                int dataLength = 0;
                for(int n1 = 0; n1 < value.Count; n1++)
                {
                   if(n1 == 0) { dataLength = value[0].Count; }
                   if(dataLength != value[n1].Count) { dataPass = false; break; }
                }

                if (dataPass) 
                { 
                    _Data = value;
                    _DataLength = dataLength;
                    UpdateImage(); 
                }
            }
        }

        private SolidColorBrush _AxesColor = Brushes.Silver;
        public SolidColorBrush AxesColor { get { return _AxesColor; } set { _AxesColor = value; } }

        private Int16 _AxeThickness = 2;
        public Int16 AxeThickness { 
            get { return _AxeThickness; } 
            set { 
                if(value > 0)
                {
                    _AxeThickness = value;
                    UpdateImage();
                } 
            } 
        }

        private List<SolidColorBrush> _PalleteData = new List<SolidColorBrush>() 
        { 
            Brushes.YellowGreen,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Orange,
            Brushes.Orchid,
            Brushes.PaleGoldenrod,
            Brushes.PaleGreen
        };

        public enum ChartingType
        {
            RADAR2D,
            RADAR3D
        }


        public ChartingWindow()
        {
            this.Background = new SolidColorBrush(Colors.Black);
            Rectangle Background = new();
            this.Children.Add(Background);
            Background.Fill = Brushes.White;
        }

        #region Methods

        public void Add_Axe(Axes NewAxe)
        {
            _AxesList.Add(NewAxe);
            UpdateImage();
        }
        
        public void UpdateImage()
        {
            this.Children.Clear();

            BuildBaseAxe();
            BuildDataPoints();
            BuildBaseAxe();

        }



        #endregion

        #region ImageBuild
        private void BuildBaseAxe()
        {
            double SplitBase = 360 / (_AxesList.Count);
            Int16 Index = 0;
            double CurrentAngle = 45;
            double CenterX = this.ActualWidth  / 2;
            double CenterY = this.ActualHeight / 2;
            _chartCenter.X = CenterX;
            _chartCenter.Y = CenterY;

            Path path = new Path();
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            path.Data = ellipseGeometry;
            path.Stroke = _AxesColor;
            path.Fill = _AxesColor;
            path.Opacity = 0.1;
            ellipseGeometry.Center = new Point(CenterX, CenterY);
            ellipseGeometry.RadiusX = CenterX;
            ellipseGeometry.RadiusY = CenterY;

            this.Children.Add(path);

            _AxesUnitVectors = new();
            _AxesStepSize = new();
            foreach (Axes obj in _AxesList) 
            {
                double angle = -Math.PI * CurrentAngle / 180.0;
                Line BaseLine = new();

                BaseLine.SnapsToDevicePixels = true;
                BaseLine.StrokeThickness = _AxeThickness;
                BaseLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                BaseLine.Stroke = _AxesColor;
                BaseLine.Effect = new System.Windows.Media.Effects.BlurEffect
                {
                    Radius = 3
                };

                BaseLine.X1 = CenterX;
                BaseLine.Y1 = CenterY;

                BaseLine.X2 = BaseLine.X1 + Math.Cos(angle) * BaseLine.X1;
                BaseLine.Y2 = BaseLine.Y1 + Math.Sin(angle) * BaseLine.Y1;

                int SpacingLines = (int)((obj.MaxValue-obj.MinValue)/obj.Interval);
                

                if (SpacingLines > 0)
                {
                    Point point_difference = (Point)Point.Subtract(
                                            new Point(CenterX, CenterY),
                                            new Point(BaseLine.X2, BaseLine.Y2));

                    double Line_length = Math.Abs(Point.Subtract(
                                            new Point(CenterX, CenterY), 
                                            new Point(BaseLine.X2, BaseLine.Y2)).Length);

                    Point unit_vector = new Point(point_difference.X/ Line_length, 
                                                  point_difference.Y/ Line_length);
                    _AxesUnitVectors.Add(unit_vector);

                    Point perpendicular_vector = new Point(-unit_vector.Y, unit_vector.X);

                    
                    double LinePerp_length = Math.Abs(Point.Subtract(
                                            new Point(unit_vector.X, unit_vector.Y),
                                            new Point(perpendicular_vector.X, perpendicular_vector.Y)).Length);

                    Point unitPerp_vector = new Point(perpendicular_vector.X / LinePerp_length,
                                                  perpendicular_vector.Y / LinePerp_length);

                    //double TestPerp = perpendicular_vector.X * unit_vector.X + perpendicular_vector.Y* unit_vector.Y;
                    double StepSize = Line_length / SpacingLines;
                    double CurrentLength = StepSize;

                    _AxesStepSize.Add(StepSize);
                    for (int n1 = 1; n1 < SpacingLines; n1++)
                    {
                        Point point_toDistance = new Point(CenterX - unit_vector.X * CurrentLength, 
                                                           CenterY - unit_vector.Y * CurrentLength);
                        double X_Calc = point_toDistance.X;
                        double Y_Calc = point_toDistance.Y;

                        if(Double.IsNaN(X_Calc) != true)
                        {
                            Line TestLine = new Line();
                            TestLine.Stroke = _AxesColor;
                            TestLine.StrokeThickness = _AxeThickness - 1;

                            TestLine.X1 = X_Calc;
                            TestLine.Y1 = Y_Calc;

                            TestLine.X2 = X_Calc - perpendicular_vector.X * 7;
                            TestLine.Y2 = Y_Calc - perpendicular_vector.Y * 7;

                            double valuePosition = (obj.MinValue) + obj.Interval * n1;
                            FormattedText formattedText = new System.Windows.Media.FormattedText(Math.Round(valuePosition, 2).ToString(),
                                        CultureInfo.GetCultureInfo("en-us"),
                                        FlowDirection.RightToLeft,
                                        new Typeface(
                                        new FontFamily(),
                                        FontStyles.Normal,
                                        FontWeights.Normal,
                                        FontStretches.Condensed),
                                        11, Brushes.Yellow, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                            Geometry geometry = formattedText.BuildGeometry(new Point(TestLine.X2-12, TestLine.Y2-12));
                            Path PT01 = new Path();
                            PT01.Data = geometry;
                            PT01.Stroke = _AxesColor;
                            PT01.StrokeThickness = _AxeThickness - 1;

                            
                            this.Children.Add(TestLine);
                            this.Children.Add(PT01);
                        }
                        

                        CurrentLength += StepSize;
                    }
                }

                BaseLine.MouseEnter += HandleEnterBaseAxe;
                BaseLine.MouseLeave += HandleLeaveBaseAxe;
                BaseLine.MouseWheel += HandleMouseWheelBaseAxe;
                BaseLine.MouseDown += HandleMouseDown;
                BaseLine.MouseUp += HandleMouseUp;
                BaseLine.MouseMove += HandleMouseMove;

                BaseLine.ToolTip = obj.Name;
                BaseLine.Tag = obj;

                this.Children.Add(BaseLine);
                Index++;
                CurrentAngle += SplitBase;

            }

           

        }
        private void BuildDataPoints()
        {
            for(int n1 = 0; n1 < _DataLength; n1++)
            {
                Polygon polygon = new();
                polygon.Stroke = _AxesColor;
                polygon.StrokeThickness = 1;
                polygon.Fill = _PalleteData[n1];
                polygon.Opacity = 0.5;
                polygon.Effect = new System.Windows.Media.Effects.BlurEffect
                {
                    Radius = 3
                };

                int Index = 0;
                foreach (List<double> Listy in _Data)
                {
                    Point calculated = new Point(
                            _chartCenter.X - _AxesUnitVectors[Index].X * (Listy[n1] * _AxesStepSize[Index] / _AxesList[Index].Interval), 
                            _chartCenter.Y - _AxesUnitVectors[Index].Y * (Listy[n1] * _AxesStepSize[Index] / _AxesList[Index].Interval));
                    
                    polygon.Points.Add(calculated);

                    Path path = new Path();
                    EllipseGeometry ellipseGeometry = new EllipseGeometry();
                    path.Data = ellipseGeometry;
                    path.Stroke = _AxesColor;
                    path.Fill = Brushes.Red;
                    path.Opacity = 0.8;
                    ellipseGeometry.Center = new Point(calculated.X, calculated.Y);
                    ellipseGeometry.RadiusX = 5;
                    ellipseGeometry.RadiusY = 5;
                    path.ToolTip = Listy[n1].ToString();
                    path.Effect = new System.Windows.Media.Effects.BlurEffect
                    {
                        Radius = 3
                    };

                    this.Children.Add(path);

                    Index++;
                }

                this.Children.Add(polygon);
            }

        }
        #endregion

        #region Handles
        private void HandleEnterBaseAxe(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Line line = (Line)sender;
            line.StrokeThickness = 10;
        }

        private void HandleLeaveBaseAxe(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Line line = (Line)sender;
            line.StrokeThickness = _AxeThickness;
        }

        private void HandleMouseWheelBaseAxe(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Line line = (Line)sender;
            Axes axes = (Axes)line.Tag;

            if(e.Delta > 0)
            {
                axes.Interval = (double)axes.Interval * 1.10;
            }
            else
            {
                axes.Interval = (double)axes.Interval * 0.90;
            }

            UpdateImage();

        }

        private bool _mouseDown = false;
        private Point _mousePosition;
        private void HandleMouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _mouseDown = true;
            _mousePosition = e.GetPosition(this);
        }

        private void HandleMouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _mouseDown = false;
            UpdateImage();
        }

        private void HandleMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Line line = (Line)sender;
                Axes axes = (Axes)line.Tag;

                double toMove = axes.Interval*3;
                double Distance = Point.Subtract(_mousePosition, e.GetPosition(this)).Length;
                
                if(Distance > 0)
                {
                    axes.MinValue += toMove;
                    axes.MaxValue += toMove;
                    _mouseDown = false;
                    UpdateImage();
                }
                else if(Distance < 0)
                {
                    axes.MinValue -= toMove;
                    axes.MaxValue -= toMove;
                    _mouseDown = false;
                    UpdateImage();
                }

            }
        }

        #endregion

        #region override


        #endregion



    }
}
