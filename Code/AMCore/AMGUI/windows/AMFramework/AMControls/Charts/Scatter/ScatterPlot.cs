using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using AMControls.Charts.Interfaces;

namespace AMControls.Charts.Scatter
{
    public class ScatterPlot:Canvas, IChart
    {
        private IAxes _xAxis;
        private IAxes _yAxis;
        private Color AxisColor = Colors.Black;
        private double AxisThickness = 1;
        private Color GridColor = Colors.Gray;
        private double GridThickness = 0.3;
        private int MajorTickSize = 10;
        private int MinorTickSize = 4;
        private System.Windows.Rect _chartArea = new();

        // Series
        private List<IDataSeries> _series = new();

        private FontFamily _axisLabelFontFamily = new("Lucida Sans");
        private int _axisLabelFontSize = 12;
        private System.Windows.FontStyle _axisLabelFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisLabelFontWeight = System.Windows.FontWeights.Bold; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisLabelFontStretch = System.Windows.FontStretches.Normal;
        private Color _axisLabelFontColor = Colors.Black;

        private FontFamily _axisTickFontFamily = new("Lucida Sans");
        private int _axisTickFontSize = 8;
        private System.Windows.FontStyle _axisTickFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _axisTickFontWeight = System.Windows.FontWeights.Light; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _axisTickFontStretch = System.Windows.FontStretches.Normal;
        private Color _axisTickFontColor = Colors.Black;

        private double _xMargin = 10;
        private double _yMargin = 10;

        // Axis
        private double _xAxisSpacing = 0;
        private double _xAxis_xLocation = 0;
        private double _yAxisSpacing = 0;
        private double _yAxis_yLocation = 0;


        // Legend
        private ILegend _legend = new Charts.Implementations.LegendBox();

        //Options
        private bool _showGrid = true;
        private double _mouseWheelDownFactor = 0.5;
        private double _mouseWheelUpFactor = 1.5;
        private double _translationFactor = 0.02;

        // Animations
        private double _axisAnimationTime = 350;
        private double _gridAnimationTime = 500;
        private bool[] _firstAnimation = {true, true, true, true};

        private ContextMenu dt = new();

        public ScatterPlot() 
        {
            Background = new SolidColorBrush(Colors.Transparent);
            this.Cursor = Cursors.Cross;


            MenuItem SaveImage = new();
            SaveImage.Header = "Save image";
            SaveImage.Click += SaveImage_ClickHandle;

            MenuItem PopWindow = new();
            PopWindow.Header = "Pop to new window";
            PopWindow.Click += New_window;

            dt.Items.Add(SaveImage);
            dt.Items.Add(PopWindow);

            ContextMenu = dt;

        }

        #region Setters
        public void Set_xAxis(IAxes value) 
        { 
            _xAxis = value;
            _xAxis.AxisOrientation = IAxes.Orientation.VERTICAL;
        }
        public void Set_yAxis(IAxes value) 
        { 
            _yAxis = value;
            _xAxis.AxisOrientation = IAxes.Orientation.HORIZONTAL;
        }

        public void Add_series(IDataSeries value) 
        {
            _series.Add(value);
            _series = _series.OrderBy(e => e.Index).ToList();
        }
        
        #endregion

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // Render axis
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
            Draw_HorizontalAxis(dc, _xAxis);
            Draw_VerticalAxis(dc, _yAxis);

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
            
            for (int i = 0; i < _series.Count; i++) 
            {
                _series[i].Draw(dc, this, _chartArea, _xAxisSpacing / _xAxis.Interval, _yAxisSpacing / _yAxis.Interval, _xAxis.MinValue, _yAxis.MinValue);
            }

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
            if (_mouseCaptured == true && MouseDown == false) 
            {
                Draw_ToolTip(dc);
            }

            _legend.Draw(dc, this, _chartArea, _series);
        }


        #region Drawing

        #region DrawAxes
        private void Draw_HorizontalAxis(DrawingContext dc, IAxes axisObject)
        {
            UpdateAxePosition_Values();
            if (axisObject != null)
            {
                // get axis width
                int axis_width = (int)(this.ActualWidth - _xAxis_xLocation - _yMargin);
                int axis_height = MajorTickSize;

                // Draw axis Label
                FormattedText labelFormat = new(axisObject.Name, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_axisLabelFontFamily, _axisLabelFontStyle, _axisLabelFontWeight, _axisLabelFontStretch),
                                                  _axisLabelFontSize, new SolidColorBrush(_axisLabelFontColor), VisualTreeHelper.GetDpi(this).PixelsPerDip);


                System.Windows.Point LabelStart = new(_xAxis_xLocation + (axis_width - labelFormat.Width)/2, this.ActualHeight - labelFormat.Height - _yMargin);
                dc.DrawText(labelFormat, LabelStart);

                // Set Bounds of axe
                axisObject.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point((int)(_xAxis_xLocation), (int)(this.ActualHeight - (_yAxis_yLocation))), new System.Drawing.Size(axis_width, axis_height));
                _chartArea.X = axisObject.Bounds.X;
                _chartArea.Width = axisObject.Bounds.Width;

                // Draw Line
                System.Windows.Point StartPoint = new(axisObject.Bounds.X, axisObject.Bounds.Y + axis_height / 2);
                System.Windows.Point EndPoint = new(axisObject.Bounds.X + axisObject.Bounds.Width, axisObject.Bounds.Y + axis_height/2);

                Pen Axe_pen = new(new SolidColorBrush(AxisColor), AxisThickness);
                Draw_AxisAnimation(dc, ref _firstAnimation[0], StartPoint, EndPoint);

                // Draw intervals
                double Sections = (axisObject.MaxValue - axisObject.MinValue) / axisObject.Interval;
                _xAxisSpacing = axis_width / Sections;

                for (int n1 = 1; n1 < Sections; n1++)
                {
                    bool _doAnimation = _firstAnimation[2];
                    System.Windows.Point sPoint = new(axisObject.Bounds.X + _xAxisSpacing * n1, axisObject.Bounds.Y);
                    System.Windows.Point ePoint = new(axisObject.Bounds.X + _xAxisSpacing * n1, axisObject.Bounds.Y + axisObject.Bounds.Height);

                    dc.DrawLine(Axe_pen, sPoint, ePoint);

                    string valueString = (axisObject.MinValue + n1 * axisObject.Interval).ToString(axisObject.IntervalNotation());
                    FormattedText txtFormat = new(valueString, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_axisTickFontFamily, _axisTickFontStyle, _axisTickFontWeight, _axisTickFontStretch),
                                                  _axisTickFontSize, new SolidColorBrush(_axisTickFontColor), VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    System.Windows.Point TextStart = new(sPoint.X - txtFormat.Width / 2, ePoint.Y);
                    dc.DrawText(txtFormat, TextStart);

                    if (_showGrid) 
                    {
                        System.Windows.Point sGPoint = new(sPoint.X, sPoint.Y);
                        System.Windows.Point eGPoint = new(sPoint.X, _yMargin);

                        Draw_GridAnimation(dc, ref _doAnimation, sGPoint, eGPoint);
                    }
                }
                if (_firstAnimation[2]) _firstAnimation[2] = !_firstAnimation[2];
                
            }
        }

        private void Draw_VerticalAxis(DrawingContext dc, IAxes axisObject)
        {
            UpdateAxePosition_Values();
            if (axisObject != null)
            {
                // get axis width
                int axis_height = (int)(this.ActualHeight - _yAxis_yLocation - _yMargin/2);
                int axis_width = MajorTickSize;

                // Draw axis Label
                FormattedText labelFormat = new(axisObject.Name, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_axisLabelFontFamily, _axisLabelFontStyle, _axisLabelFontWeight, _axisLabelFontStretch),
                                                  _axisLabelFontSize, new SolidColorBrush(_axisLabelFontColor), VisualTreeHelper.GetDpi(this).PixelsPerDip);


                System.Windows.Point LabelStart = new(_xMargin, labelFormat.Width + (axis_height - labelFormat.Width) /2);

                RotateTransform labelTR = new RotateTransform(-90, LabelStart.X, LabelStart.Y);
                dc.PushTransform(labelTR);
                dc.DrawText(labelFormat, LabelStart);
                dc.Pop();

                // Set Bounds of axe
                axisObject.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point((int)(_xAxis_xLocation), (int)(_yMargin)), new System.Drawing.Size(axis_width, axis_height));
                _chartArea.Y = axisObject.Bounds.Y;
                _chartArea.Height = axisObject.Bounds.Height;

                // Draw Line
                System.Windows.Point StartPoint = new(axisObject.Bounds.X + axisObject.Bounds.Width - MajorTickSize, axisObject.Bounds.Y);
                System.Windows.Point EndPoint = new(axisObject.Bounds.X + axisObject.Bounds.Width - MajorTickSize, axisObject.Bounds.Y + axisObject.Bounds.Height);

                Pen Axe_pen = new(new SolidColorBrush(AxisColor), AxisThickness);
                Draw_AxisAnimation(dc, ref _firstAnimation[1], EndPoint, StartPoint);

                // Draw intervals
                double Sections = (axisObject.MaxValue - axisObject.MinValue) / axisObject.Interval;
                _yAxisSpacing = axis_height / Sections;

                for (int n1 = 1; n1 < Sections; n1++)
                {
                    bool _doAnimation = _firstAnimation[3];
                    System.Windows.Point sPoint = new(StartPoint.X - MajorTickSize/2, axisObject.Bounds.Y + _yAxisSpacing * (Sections - n1));
                    System.Windows.Point ePoint = new(StartPoint.X + MajorTickSize, axisObject.Bounds.Y + _yAxisSpacing * (Sections - n1)) ;

                    dc.DrawLine(Axe_pen, sPoint, ePoint);

                    string valueString = (axisObject.MinValue + n1 * axisObject.Interval).ToString(axisObject.IntervalNotation());
                    FormattedText txtFormat = new(valueString, System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_axisTickFontFamily, _axisTickFontStyle, _axisTickFontWeight, _axisTickFontStretch),
                                                  _axisTickFontSize, new SolidColorBrush(_axisTickFontColor), VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    System.Windows.Point TextStart = new(sPoint.X - txtFormat.Width, sPoint.Y - (txtFormat.Height - AxisThickness)/2);
                    dc.DrawText(txtFormat, TextStart);

                    if (_showGrid)
                    {
                        System.Windows.Point sGPoint = new(sPoint.X, sPoint.Y);
                        System.Windows.Point eGPoint = new(this.ActualWidth - _xMargin, sPoint.Y);

                        Draw_GridAnimation(dc,ref _doAnimation, sGPoint, eGPoint);
                    }
                }
                if (_firstAnimation[3]) _firstAnimation[3] = !_firstAnimation[3];


            }
        }

        #region Animations
        private void Draw_AxisAnimation(DrawingContext dc , ref bool doAnimation,
                                        System.Windows.Point sPoint, System.Windows.Point ePoint) 
        {

            Pen Axe_pen = new(new SolidColorBrush(AxisColor), AxisThickness);
            Pen Grid_pen = new(new SolidColorBrush(GridColor), GridThickness);

            if (doAnimation)
            {
                PointAnimationBase pab = (PointAnimationBase)new PointAnimation(sPoint, new Duration(TimeSpan.FromMilliseconds(_axisAnimationTime)));
                PointAnimationBase pas = (PointAnimationBase)new PointAnimation(ePoint, new Duration(TimeSpan.FromMilliseconds(_axisAnimationTime)));
                System.Windows.Media.Animation.AnimationClock p1Animation = pab.CreateClock();
                System.Windows.Media.Animation.AnimationClock p2Animation = pas.CreateClock();

                dc.DrawLine(Axe_pen, sPoint, p1Animation, sPoint, p2Animation);

                doAnimation = !doAnimation;
            }
            else 
            {
                dc.DrawLine(Axe_pen, sPoint, ePoint);
            }
            
        }

        private void Draw_GridAnimation(DrawingContext dc, ref bool doAnimation,
                                       System.Windows.Point sPoint, System.Windows.Point ePoint)
        {

            Pen Axe_pen = new(new SolidColorBrush(AxisColor), AxisThickness);
            Pen Grid_pen = new(new SolidColorBrush(GridColor), GridThickness);

            if (doAnimation)
            {
                PointAnimationBase pab = (PointAnimationBase)new PointAnimation(sPoint, new Duration(TimeSpan.FromMilliseconds(_gridAnimationTime)));
                PointAnimationBase pas = (PointAnimationBase)new PointAnimation(ePoint, new Duration(TimeSpan.FromMilliseconds(_gridAnimationTime)));
                System.Windows.Media.Animation.AnimationClock p1Animation = pab.CreateClock();
                System.Windows.Media.Animation.AnimationClock p2Animation = pas.CreateClock();

                dc.DrawLine(Grid_pen, sPoint, p1Animation, sPoint, p2Animation);

                doAnimation = !doAnimation;
            }
            else
            {
                dc.DrawLine(Grid_pen, sPoint, ePoint);
            }

        }

        private void Draw_DataPoint_HoverAnimation() 
        {
            
        }

        #endregion
        #endregion

        #region Tooltip
        private void Draw_ToolTip(DrawingContext dc) 
        {
            if (_xAxis == null || _yAxis == null) return;

            

            System.Windows.Rect Box = new(Tooltip_position.X, Tooltip_position.Y, 10, 10);
            dc.DrawRectangle(new SolidColorBrush(Colors.White), new Pen(new SolidColorBrush(Colors.Silver), 1), Box);

        }

        private void UpdateAxePosition_Values() 
        {
            FormattedText labelFormat = new("Sample Text", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_axisLabelFontFamily, _axisLabelFontStyle, _axisLabelFontWeight, _axisLabelFontStretch),
                                                  _axisLabelFontSize, new SolidColorBrush(_axisLabelFontColor), VisualTreeHelper.GetDpi(this).PixelsPerDip);

            _xAxis_xLocation = _xMargin * 3 + 50;
            _yAxis_yLocation = _yMargin * 3 + labelFormat.Height;
        }
        #endregion

        

        #endregion

        #region Methods
        public void Save_Image(string filename) 
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.RenderSize.Width,
                                     (int)this.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(this);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)this.ActualWidth, (int)this.ActualHeight));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = System.IO.File.OpenWrite(filename))
            {
                pngEncoder.Save(fs);
            }
        }

        public void Adjust_axes_to_data() 
        {
            if (_series.Count == 0) return;
            if (_xAxis == null || _yAxis == null) return;

            double xMin = _series.Min(e => e.DataPoints.Min(e => e.X));
            double xMax = _series.Max(e => e.DataPoints.Max(e => e.X));
            double yMin = _series.Min(e => e.DataPoints.Min(e => e.Y));
            double yMax = _series.Max(e => e.DataPoints.Max(e => e.Y));

            _xAxis.MinValue = xMin;
            _yAxis.MinValue = yMin;
            _xAxis.MaxValue = xMax;
            _yAxis.MaxValue = yMax;

            InvalidateVisual();
        }

        #endregion

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            Point mouseLocation = e.GetPosition(this);

            if (e.Delta > 0)
            {
                ZoomIn(mouseLocation.X, mouseLocation.Y);
            }
            else 
            {
                ZoomOut(mouseLocation.X, mouseLocation.Y);
            }
        }

        private Point Translate_StartPosition = new();
        private bool MouseDown = false;
        private double _xMinValue = 0;
        private double _xMaxValue = 0;
        private double _yMinValue = 0;
        private double _yMaxValue = 0;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            
            if(_xAxis != null && _yAxis != null) 
            {
                Translate_StartPosition = e.GetPosition(this);
                MouseDown = true;
                _mouseCaptured = false;

                _xMinValue = _xAxis.MinValue;
                _xMaxValue = _xAxis.MaxValue;
                _yMinValue = _yAxis.MinValue;
                _yMaxValue = _yAxis.MaxValue;
            }
        }


        private Point Tooltip_position = new();
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point mouseLocation = e.GetPosition(this);
            if (MouseDown) 
            {
                Translate(Translate_StartPosition.X - mouseLocation.X, -Translate_StartPosition.Y + mouseLocation.Y);
            }
            else 
            {
                bool doInvalidate = false;
                //Tooltip_position = e.GetPosition(this);
                //InvalidateVisual();

                // check if over a data point
                for (int i = 0; i < _series.Count; i++) 
                {
                    bool tempRes = _series[i].Check_MouseHover(mouseLocation.X, mouseLocation.Y);
                    if (tempRes) doInvalidate = true;
                }

                if (doInvalidate) InvalidateVisual();
            }
        }

        private bool _mouseCaptured = false;
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _mouseCaptured = true;


        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseCaptured = false;
            MouseDown = false;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            MouseDown = false;
            _mouseCaptured = true;

            bool toRemove = true;
            while (toRemove) 
            {
                toRemove = false;
                foreach (MenuItem item in dt.Items)
                {
                    if(item.Header.ToString().Contains("HT") || item.Header.ToString().Contains("Show all series")) 
                    {
                        dt.Items.Remove(item);
                        toRemove = true;
                        break;
                    }
                }
            }
            

            Point mousePos =  e.GetPosition(this);

            bool hiddenSeries = false;
            foreach (var item in _series)
            {
                if (!item.IsVisible) 
                {
                    hiddenSeries = true;
                    continue;
                }

                item.CheckHit(mousePos.X, mousePos.Y);

                if (item.IsSelected) 
                {
                    MenuItem Mitem = new() { Header = "HT_" + item.Label + ": Hide"};
                    Mitem.Tag = item;
                    Mitem.Click += Handle_HideSeries;

                    dt.Items.Add(Mitem);
                    Bring_series_to_front(item);
                    _series = _series.OrderBy(e => e.Index).ToList();
                }
            }
            

            if (hiddenSeries) 
            {
                MenuItem Mitem = new() { Header = "Show all series" };
                Mitem.Click += Handle_showAllSeries;

                dt.Items.Add(Mitem);
            }

            // Legend
            IDataSeries dtTemp = _legend.Check_series_Hit(mousePos.X, mousePos.Y);

            // Zoom in series
            if (dtTemp != null) 
            {
                _xAxis.MinValue = dtTemp.DataPoints.Min(e => e.X);
                _xAxis.MaxValue = dtTemp.DataPoints.Max(e => e.X);
                _yAxis.MinValue = dtTemp.DataPoints.Min(e => e.Y);
                _yAxis.MaxValue = dtTemp.DataPoints.Max(e => e.Y);
                Bring_series_to_front(dtTemp);
                dtTemp.IsSelected = true;
                _series = _series.OrderBy(e => e.Index).ToList();
            }

            InvalidateVisual();
        }

        private void Handle_HideSeries(object sender, RoutedEventArgs e) 
        {
            IDataSeries series = (IDataSeries)((MenuItem)sender).Tag;

            series.IsVisible = false;
            InvalidateVisual();
        }

        private void Handle_showAllSeries(object sender, RoutedEventArgs e) 
        {
            foreach (var item in _series)
            {
                item.IsVisible = true;
            }

            InvalidateVisual();
        }

        

        private void ZoomIn(double x, double y) 
        {
            if (_xAxis != null && _yAxis != null)
            {
                double xInterval = (_xAxis.MaxValue - _xAxis.MinValue)*0.5;
                double yInterval = (_yAxis.MaxValue - _yAxis.MinValue) * 0.5;

                _xAxis.MinValue += (xInterval) / 2 ;
                _xAxis.MaxValue -= (xInterval) / 2 ;
                _yAxis.MinValue += (yInterval) / 2 ;
                _yAxis.MaxValue -= (yInterval) / 2 ;

                this.InvalidateVisual();
            }
        }

        private void ZoomOut(double x, double y) 
        {
            if (_xAxis != null && _yAxis != null)
            {
                double xInterval = (_xAxis.MaxValue - _xAxis.MinValue) * 0.5;
                double yInterval = (_yAxis.MaxValue - _yAxis.MinValue) * 0.5;

                _xAxis.MinValue -= (xInterval) / 2;
                _xAxis.MaxValue += (xInterval) / 2;
                _yAxis.MinValue -= (yInterval) / 2;
                _yAxis.MaxValue += (yInterval) / 2;

                this.InvalidateVisual();
            }
        }

        private void Translate(double x, double y) 
        {
            if(_xAxis != null && _yAxis != null) 
            {
                _xAxis.MinValue = _xMinValue + x * _translationFactor * _xAxis.Interval;
                _xAxis.MaxValue = _xMaxValue + x * _translationFactor * _xAxis.Interval;
                _yAxis.MinValue = _yMinValue + y * _translationFactor * _yAxis.Interval;
                _yAxis.MaxValue = _yMaxValue + y * _translationFactor * _yAxis.Interval;

                this.InvalidateVisual();
            }
        }

        private void SaveImage_ClickHandle(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = "plot";
            saveFileDialog.DefaultExt = ".jpg";
            saveFileDialog.Filter = "Image (.jpg)|*.jpg";

            // Show save file dialog box
            Nullable<bool> DialogResult = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (DialogResult == true)
            {
                // Save document
                string filename = saveFileDialog.FileName;
                Save_Image(filename);
            }
        }

        private void Bring_series_to_front(IDataSeries seriesObject) 
        {
            int index = 0;
            foreach (var item in _series)
            {
                item.Index = index++;
            }

            seriesObject.Index = _series.Count + 1;
        }

        private void New_window(object sender, RoutedEventArgs e) 
        {
            ScatterPlot sP = new();
            sP.Set_xAxis(_xAxis);
            sP.Set_yAxis(_yAxis);

            foreach (var item in _series)
            {
                sP.Add_series(item);
            }

            Window win = new();
            win.Content = sP;
            win.Show();
        }
    }
}
