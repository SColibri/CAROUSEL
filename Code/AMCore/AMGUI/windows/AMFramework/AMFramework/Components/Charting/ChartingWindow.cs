using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace AMFramework.Components.Charting
{
    [Obsolete("Use AMControls instead")]
    public class ChartingWindow:Canvas
    {

        private int _DataLength = 0;
        private Point _chartCenter = new();
        private List<Point> _AxesUnitVectors = new();
        private List<double> _AxesStepSize = new();

        private List<Axes> _AxesList = new();
        public List<Axes> AxesList { get { return _AxesList; } }

        public struct SpyderSeriesData
        {
            public string SeriesName;
            public List<double> Data;
            public List<string> AxeName;
            public object Tag;
            public Polygon polygonObj;
            public SpyderSeriesData() 
            {
                SeriesName = "Empty";
                Data = new List<double>();
                AxeName = new List<string>();
                Tag = "Empty";
                polygonObj = new Polygon();
            }
        }

        private List<SpyderSeriesData> _series = new();
        public List<SpyderSeriesData> Series { get { return _series; } }

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

        private SolidColorBrush _AxesColor = Brushes.DimGray;
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

        private List<SolidColorBrush> _PalleteData = new() 
        { 
            Brushes.YellowGreen,
            Brushes.LightBlue,
            Brushes.LightGreen,
            Brushes.LightPink,
            Brushes.LightCyan,
            Brushes.LightSeaGreen,
            Brushes.LightGray
        };

        public enum ChartingType
        {
            RADAR2D,
            RADAR3D
        }

        public ChartingWindow()
        {
            this.Background = new SolidColorBrush(Colors.White);
            Rectangle Background = new();
            this.Children.Add(Background);
            Background.Fill = Brushes.White;
            this.SizeChanged += HandleTab;
            this.VisualEdgeMode = EdgeMode.Aliased;
            SnapsToDevicePixels = true;
           
        }

        #region getters_setters
        public void Add_series(SpyderSeriesData NewSeries) 
        {
            _series.Add(NewSeries);
            Update_axes();
        }
        #endregion

        #region Methods
        private void Update_axes() 
        {
            _AxesList.Clear();
            List<string> AxesNames = _series.SelectMany(e => e.AxeName).Select(fy => fy).Distinct().ToList();
            List<double> maxValues = new();

            // find max value in series
            for (int n1 = 0; n1 < AxesNames.Count; n1++)
            {
                maxValues.Add(0.000001);
                for (int n2 = 0; n2 < _series.Count; n2++)
                {
                    int indexPhase = _series[n2].AxeName.FindIndex(e => e.CompareTo(AxesNames[n1]) == 0);
                    if (indexPhase == -1) continue;
                    if(maxValues[n1] < _series[n2].Data[indexPhase]) 
                    {
                        maxValues[n1] = _series[n2].Data[indexPhase];
                    }
                }
            }

            int IndexTemp = 0;
            foreach (string axy in AxesNames)
            {

                Axes tempAxe = new(axy)
                {
                    MinValue = 0,
                    MaxValue = maxValues[IndexTemp]
                };
                tempAxe.Interval = tempAxe.MaxValue/3;

                _AxesList.Add(tempAxe);
                IndexTemp += 1;
            }
        }

        public void Add_Axe(Axes NewAxe)
        {
            _AxesList.Add(NewAxe);
            UpdateImage();
        }

        public void Clear_axes()
        {
            _AxesList.Clear();
        }

        public void ClearAll() 
        {
            _AxesList.Clear();
            _Data.Clear();
            Series.Clear();
        }

        public void UpdateImage()
        {
            this.Children.Clear();

            //BuildBaseAxe();
            
            //BuildDataPoints();
            BuildBaseAxe();
            BuidDataPoints_usingStructure();
            BuildBaseAxe();
            Build_legend();
            Build_axis_legend();
        }



        #endregion

        #region ImageBuild
        private void BuildBaseAxe()
        {
            List<Axes> VisList = _AxesList.FindAll(e => e.IsVisible == true);
            if (VisList.Count == 0) return;
            
            double SplitBase = 360 / (VisList.Count);
            Int16 Index = 0;
            double CurrentAngle = 45;
            double CenterX = this.ActualWidth  / 2;
            double CenterY = this.ActualHeight / 2;
            double TextMargin = 70;
            _chartCenter.X = CenterX;
            _chartCenter.Y = CenterY;

            Path path = new();
            EllipseGeometry ellipseGeometry = new();
            path.Data = ellipseGeometry;
            path.Stroke = _AxesColor;
            path.Fill = _AxesColor;
            path.Opacity = 0.05;
            ellipseGeometry.Center = new Point(CenterX, CenterY);
            ellipseGeometry.RadiusX = CenterX - TextMargin;
            ellipseGeometry.RadiusY = CenterY - TextMargin;

            this.Children.Add(path);

            _AxesUnitVectors = new();
            _AxesStepSize = new();
            foreach (Axes obj in VisList) 
            {
                if (!obj.IsVisible) continue;

                double angle = -Math.PI * CurrentAngle / 180.0;
                Line BaseLine = new()
                {
                    SnapsToDevicePixels = true,
                    StrokeThickness = _AxeThickness
                };
                BaseLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                BaseLine.Stroke = _AxesColor;
                BaseLine.Effect = new System.Windows.Media.Effects.BlurEffect
                {
                    Radius = 0
                };

                BaseLine.X1 = CenterX;
                BaseLine.Y1 = CenterY;

                BaseLine.X2 = BaseLine.X1 + Math.Cos(angle) * (BaseLine.X1-TextMargin);
                BaseLine.Y2 = BaseLine.Y1 + Math.Sin(angle) * (BaseLine.Y1-TextMargin);

                int SpacingLines = (int)((obj.MaxValue-obj.MinValue)/obj.Interval);
                

                if (SpacingLines > 0)
                {
                    Point point_difference = (Point)Point.Subtract(
                                            new Point(CenterX, CenterY),
                                            new Point(BaseLine.X2, BaseLine.Y2));

                    double Line_length = Math.Abs(Point.Subtract(
                                            new Point(CenterX, CenterY), 
                                            new Point(BaseLine.X2, BaseLine.Y2)).Length);

                    Point unit_vector = new(point_difference.X/ Line_length, 
                                                  point_difference.Y/ Line_length);
                    _AxesUnitVectors.Add(unit_vector);

                    Point perpendicular_vector = new(-unit_vector.Y, unit_vector.X);

                    
                    double LinePerp_length = Math.Abs(Point.Subtract(
                                            new Point(unit_vector.X, unit_vector.Y),
                                            new Point(perpendicular_vector.X, perpendicular_vector.Y)).Length);

                    Point unitPerp_vector = new(perpendicular_vector.X / LinePerp_length,
                                                  perpendicular_vector.Y / LinePerp_length);

                    //double TestPerp = perpendicular_vector.X * unit_vector.X + perpendicular_vector.Y* unit_vector.Y;
                    double StepSize = Line_length / SpacingLines;
                    double CurrentLength = StepSize;

                    _AxesStepSize.Add(StepSize);
                    for (int n1 = 1; n1 < SpacingLines; n1++)
                    {
                        Point point_toDistance = new(CenterX - unit_vector.X * CurrentLength, 
                                                           CenterY - unit_vector.Y * CurrentLength);
                        double X_Calc = point_toDistance.X;
                        double Y_Calc = point_toDistance.Y;

                        if(Double.IsNaN(X_Calc) != true)
                        {
                            Line TestLine = new()
                            {
                                Stroke = _AxesColor,
                                Fill = _AxesColor,
                                StrokeThickness = 0.1,//_AxeThickness - 1;



                                X1 = X_Calc,
                                Y1 = Y_Calc,

                                X2 = X_Calc - perpendicular_vector.X * 7,
                                Y2 = Y_Calc - perpendicular_vector.Y * 7
                            };

                            double valuePosition = (obj.MinValue) + obj.Interval * n1;
                            FormattedText formattedText = new(valuePosition.ToString("0.0E0"),
                                        CultureInfo.GetCultureInfo("en-us"),
                                        FlowDirection.RightToLeft,
                                        new Typeface(
                                        new FontFamily("Arial"),
                                        FontStyles.Normal,
                                        FontWeights.Normal,
                                        FontStretches.Condensed),
                                        15, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                            Geometry geometry = formattedText.BuildGeometry(new Point(TestLine.X2-12, TestLine.Y2-12));
                            Path PT01 = new()
                            {
                                Data = geometry,
                                Stroke = _AxesColor,
                                Fill = _AxesColor,
                                StrokeThickness = 0.1//_AxeThickness - 1;
                            };

                            this.Children.Add(TestLine);
                            this.Children.Add(PT01);

                            if(SpacingLines-1 == n1) 
                            {
                                double vector_mult = unitPerp_vector.X * unit_vector.X;
                                double vector_mult2 = unitPerp_vector.Y * unit_vector.Y;
                                double angle_text = Math.Acos((unitPerp_vector.Y))*(180/Math.PI);

                                FormattedText formattedAxisName = new(obj.Name,
                                        CultureInfo.GetCultureInfo("en-us"),
                                        FlowDirection.RightToLeft,
                                        new Typeface(
                                        new FontFamily("Arial"),
                                        FontStyles.Normal,
                                        FontWeights.Normal,
                                        FontStretches.Normal),
                                        12, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                Geometry geometryAxisName = formattedAxisName.BuildGeometry(new Point(BaseLine.X2, BaseLine.Y2));

                                double textOffset = 60;
                                geometryAxisName.Transform = new TranslateTransform(geometryAxisName.Bounds.Width / 2 - unit_vector.X * textOffset, -unit_vector.Y * textOffset);


                                Path PT02 = new()
                                {
                                    Data = geometryAxisName,
                                    Stroke = _AxesColor,
                                    Fill = _AxesColor,
                                    StrokeThickness = 0.1//_AxeThickness - 1;
                                };

                                this.Children.Add(PT02);
                            }
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
                BaseLine.SnapsToDevicePixels = true;

                this.Children.Add(BaseLine);
                Index++;
                CurrentAngle += SplitBase;

            }

           

        }
        private void BuildDataPoints()
        {
            for(int n1 = 0; n1 < _DataLength; n1++)
            {
                Polygon polygon = new()
                {
                    Stroke = _AxesColor,
                    StrokeThickness = 1,
                    Fill = _PalleteData[n1],
                    Opacity = 0.5,
                    Effect = new System.Windows.Media.Effects.BlurEffect
                    {
                        Radius = 3
                    }
                };

                int Index = 0;
                foreach (List<double> Listy in _Data)
                {
                    Point calculated = new(
                            _chartCenter.X - _AxesUnitVectors[Index].X * (Listy[n1] * _AxesStepSize[Index] / _AxesList[Index].Interval), 
                            _chartCenter.Y - _AxesUnitVectors[Index].Y * (Listy[n1] * _AxesStepSize[Index] / _AxesList[Index].Interval));
                    
                    polygon.Points.Add(calculated);

                    Path path = new();
                    EllipseGeometry ellipseGeometry = new();
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

        private void BuidDataPoints_usingStructure() 
        {
            int Index_pallette = 0;
            for (int n1 = 0; n1 < _series.Count; n1++)
            {
                Polygon polygon = new()
                {
                    Stroke = _AxesColor,
                    StrokeThickness = 1,
                    Fill = _PalleteData[Index_pallette],
                    Opacity = 0.5,
                    Effect = new System.Windows.Media.Effects.BlurEffect
                    {
                        Radius = 3
                    }
                };

                Index_pallette += 1;
                if(Index_pallette > _PalleteData.Count - 1) { Index_pallette = 0; }

                int Index = 0;
                int IndexVisible = 0;
                foreach (double Listy in _series[n1].Data)
                {
                    if (!_AxesList[Index].IsVisible) { Index++; continue; }
                    Point calculated = new(
                            _chartCenter.X - _AxesUnitVectors[IndexVisible].X * (Listy * _AxesStepSize[IndexVisible] / _AxesList[Index].Interval),
                            _chartCenter.Y - _AxesUnitVectors[IndexVisible].Y * (Listy * _AxesStepSize[IndexVisible] / _AxesList[Index].Interval));

                    polygon.Points.Add(calculated);

                    Path path = new();
                    EllipseGeometry ellipseGeometry = new();
                    path.Data = ellipseGeometry;
                    path.Stroke = _AxesColor;
                    path.Fill = Brushes.Red;
                    path.Opacity = 0.8;
                    ellipseGeometry.Center = new Point(calculated.X, calculated.Y);
                    ellipseGeometry.RadiusX = 5;
                    ellipseGeometry.RadiusY = 5;
                    path.ToolTip = Listy.ToString();
                    path.Effect = new System.Windows.Media.Effects.BlurEffect
                    {
                        Radius = 3
                    };
                    
                    this.Children.Add(path);

                    Index++; IndexVisible++;
                }

                this.Children.Add(polygon);
                SpyderSeriesData seriesData = Series[0];
                seriesData.polygonObj = polygon;
                //Series[n1] = seriesData;
            }

        }

        private void Build_legend() 
        {
            Border LegendBorder = new()
            {
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                CornerRadius = new CornerRadius(5),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    ShadowDepth = 1,
                    Opacity = 1,
                    BlurRadius = 1
                }
            };

            Expander LegendExpander = new()
            {
                Header = "Series",
                Margin = new Thickness(5)
            };
            LegendBorder.Child = LegendExpander;

            StackPanel LegendStackPanel = new();
            int Index = 0;
            foreach (SpyderSeriesData item in Series)
            {
                Grid seriesGrid = new();
                seriesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30)});
                seriesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                seriesGrid.Margin = new Thickness(3);

                Border seriesColor = new()
                {
                    Background = _PalleteData[Index],
                    CornerRadius = new CornerRadius(2),
                    Margin = new Thickness(2)
                };
                seriesColor.SetValue(Grid.ColumnProperty, 0);
                seriesColor.Tag = Index;
                seriesColor.MouseUp += HandleSelectSeries;

                TextBlock textBlock = new()
                {
                    Text = item.SeriesName
                };
                textBlock.SetValue(Grid.ColumnProperty, 1);

                seriesGrid.Children.Add(seriesColor);
                seriesGrid.Children.Add(textBlock);

                LegendStackPanel.Children.Add(seriesGrid);
                Index += 1;

                if(Index >= _PalleteData.Count) 
                {
                    Index = 0;
                }
            }
            LegendExpander.Content = LegendStackPanel;

            this.Children.Add(LegendBorder);
            
        }
        private void Build_axis_legend() 
        {
            Border LegendBorder = new()
            {
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                CornerRadius = new CornerRadius(5),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    ShadowDepth = 1,
                    Opacity = 1,
                    BlurRadius = 1
                },
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(80, 0, 0, 0)
            };

            Expander LegendExpander = new()
            {
                Header = "Axes",
                Margin = new Thickness(5)
            };
            LegendBorder.Child = LegendExpander;

            StackPanel LegendStackPanel = new();
            int Index = 0;
            foreach (Axes item in AxesList)
            {
                Grid seriesGrid = new();
                seriesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                seriesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                seriesGrid.Margin = new Thickness(3);

                Border seriesColor = new();
                if(item.IsVisible) seriesColor.Background = new SolidColorBrush(Colors.Black);
                else seriesColor.Background = new SolidColorBrush(Colors.White);
                seriesColor.BorderBrush = new SolidColorBrush(Colors.Silver);
                seriesColor.BorderThickness = new Thickness(1);
                seriesColor.CornerRadius = new CornerRadius(2);
                seriesColor.Margin = new Thickness(2);
                seriesColor.SetValue(Grid.ColumnProperty, 0);
                seriesColor.Tag = Index;
                seriesColor.MouseUp += HandleShowHideAxe;

                TextBlock textBlock = new()
                {
                    Text = item.Name
                };
                textBlock.SetValue(Grid.ColumnProperty, 1);

                seriesGrid.Children.Add(seriesColor);
                seriesGrid.Children.Add(textBlock);

                LegendStackPanel.Children.Add(seriesGrid);
                Index += 1;
            }
            LegendExpander.Content = LegendStackPanel;

            this.Children.Add(LegendBorder);
        }
        private void Show_series(int Index) 
        {
            foreach (SpyderSeriesData item in Series)
            {
                item.polygonObj.Visibility = Visibility.Collapsed;
            }

            SpyderSeriesData itemy = Series[Index];
            itemy.polygonObj.Visibility = Visibility.Visible;
        }

        private void Show_all_series()
        {
            foreach (SpyderSeriesData item in Series)
            {
                item.polygonObj.Visibility = Visibility.Visible;
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

        private void HandleTab(object sender, SizeChangedEventArgs e)
        {
            this.UpdateImage();
        }

        private void HandleSelectSeries(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Show_series((int)((Border)sender).Tag);
        }

        private void HandleShowHideAxe(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Axes tempAxe = _AxesList[(int)((Border)sender).Tag];
            tempAxe.IsVisible = !tempAxe.IsVisible;
            Show_all_series();
            UpdateImage();
        }
        #endregion

        #region override




        #endregion



    }
}
