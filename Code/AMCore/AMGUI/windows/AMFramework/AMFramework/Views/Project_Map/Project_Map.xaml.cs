using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.Interfaces;
using AMFramework.Controller;
using AMFramework_Lib.Controller;
using Catel.Collections;
using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AMFramework.Views.Project_Map
{
    /// <summary>
    /// Interaction logic for Project_Map.xaml
    /// </summary>
    public partial class Project_Map : UserControl, INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// Controller with methods that retrieves data from AMCore
        /// </summary>
        private Controller_Plot _plotController;

        /// <summary>
        /// List of structure like containers used for storing data
        /// </summary>
        private List<IDataPlot> _dataPlots;

        /// <summary>
        /// Scatter, Spyder and parallax plot x-axis data source
        /// </summary>
        private int _xDataOption = 2;

        /// <summary>
        /// scatter plot y-axis data source
        /// </summary>
        private int _yDataOption = 0;
        
        /// <summary>
        /// Data sorted by key 
        /// </summary>
        private Dictionary<string, List<IDataPoint>> _sortedData = new();
        private string[] _htNames = new string[0];
        private string[] _phaseNames = new string[0];

        // -------------------------------------
        // Deprecated variables TODO
        private DataOptions _dataOption = DataOptions.Precipitation_Phase;

        /// <summary>
        /// Used for scatter plot // TODO: Obsolete, this should not be handled here
        /// </summary>
        private IAxes _xaxis;

        /// <summary>
        /// Used for scatter plot // TODO: Obsolete, this should not be handled here
        /// </summary>
        private IAxes _yaxis;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plotController"></param>
        public Project_Map(Controller.Controller_Plot plotController)
        {
            InitializeComponent();
            _plotController = plotController;

            _xaxis = new LinearAxe("New x axis");
            _yaxis = new LinearAxe("New y axis");

            Main_plot.Set_xAxis(_xaxis);
            Main_plot.Set_yAxis(_yaxis);
            Main_plot.SelectionChanged += Selection_Changed_Handle;
            Main_plot.ContextMenuClicked += ContextMenuClicked;

            this.DataContext = this;
            Refresh_Data();
        }
        #endregion
        public enum DataOptions
        {
            Precipitation_Phase
        }


        #region Handle
        private void OnChartMouseMove_Handle(object? sender, MouseEventArgs e)
        {
            if (sender == null) return;
            Point mPos = e.GetPosition(this);
            IDataPoint dP = ((IChart)sender).Get_Position(mPos.X, mPos.Y);

            if (dP.X_draw > -1 && dP.Y_draw > -1)
            {
                Tooltip.Visibility = Visibility.Visible;
                XLocation.Text = dP.X.ToString("E3");
                YLocation.Text = dP.Y.ToString("E3");

                Canvas.SetLeft(Tooltip, dP.X_draw);
                Canvas.SetTop(Tooltip, dP.Y_draw);
            }
            else
            {
                Tooltip.Visibility = Visibility.Collapsed;
            }
        }

        private void Selection_Changed_Handle(object? sender, EventArgs e)
        {
            SelectedPoints = Main_plot.Get_Selected_DataPoints();
        }
        #endregion

        private void ContextMenuClicked(object? sender, EventArgs e)
        {
            if (sender == null) return;
            if (!(sender is IDataPoint)) return;

            IDataPoint dP = (IDataPoint)sender;
            if (!(dP.Tag is List<string>)) return;

            List<string> lData = (List<string>)dP.Tag;
            int IDProject = Convert.ToInt16(lData[7]);
            int IDCase = Convert.ToInt16(lData[8]);
            int IDHT = Convert.ToInt16(lData[2]);

            _plotController.Find_DataPoint(IDProject, IDCase, IDHT);
        }

        private List<IDataPoint> _selectePoints = new();
        public List<IDataPoint> SelectedPoints
        {
            get { return _selectePoints; }
            set
            {
                _selectePoints = value;
                OnPropertyChanged(nameof(SelectedPoints));
            }
        }

        private List<string> _dataOrigin = new();
        public List<string> DataOrigin
        {
            get { return _dataOrigin; }
            set
            {
                _dataOrigin = value;
                OnPropertyChanged(nameof(DataOrigin));
            }
        }

        private List<string> _columnNames = new();
        public List<string> ColumnNames
        {
            get { return _columnNames; }
            set
            {
                _columnNames = value;
                OnPropertyChanged(nameof(ColumnNames));
            }
        }

        private string _selectedDataOrigin = "";
        public string SelectedDataOrigin
        {
            get { return _selectedDataOrigin; }
            set
            {
                _selectedDataOrigin = value;
                OnPropertyChanged(nameof(SelectedDataOrigin));

                IDataPlot? dTemp = _dataPlots.Find(e => e.Name.CompareTo(_selectedDataOrigin) == 0);
                if (dTemp == null) return;
                ColumnNames = dTemp.DataOptions;
            }
        }

        private string _xDataColumn = "";
        public string XDataColumn
        {
            get { return _xDataColumn; }
            set
            {
                _xDataColumn = value;
                OnPropertyChanged(nameof(XDataColumn));

                IDataPlot? dTemp = _dataPlots.Find(e => e.Name.CompareTo(_selectedDataOrigin) == 0);
                if (dTemp == null) return;
                _xDataOption = dTemp.DataOptions.IndexOf(_xDataColumn);
            }
        }

        private string _yDataColumn = "";
        public string YDataColumn
        {
            get { return _yDataColumn; }
            set
            {
                _yDataColumn = value;
                OnPropertyChanged(nameof(YDataColumn));

                IDataPlot? dTemp = _dataPlots.Find(e => e.Name.CompareTo(_selectedDataOrigin) == 0);
                if (dTemp == null) return;
                _yDataOption = dTemp.DataOptions.IndexOf(_yDataColumn);
            }
        }

        private bool _showToolTip = false;
        public bool ShowToolTip
        {
            get { return _showToolTip; }
            set
            {
                _showToolTip = value;
                OnPropertyChanged(nameof(ShowToolTip));

                if (value)
                {
                    Main_plot.ChartAreaMouseMove += OnChartMouseMove_Handle;
                }
                else
                {
                    Main_plot.ChartAreaMouseMove -= OnChartMouseMove_Handle;
                }
            }
        }

        #region Interfaces
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Sorts data in two ways, by phase name, by heat treatment
        /// </summary>
        private void UpdateSortedData() 
        {
            // Get heat treatment items
            _htNames = _dataPlots
                .SelectMany(e => e.DataPoints)
                .Select(e => e.Label)
                .Distinct()
                .ToArray();

            // Get all phase names
            _phaseNames = _dataPlots.Select(e => e.SeriesName).ToArray();

            // Get data sorted by Phase
            foreach (var item in _dataPlots)
            {
                if (_sortedData.ContainsKey(item.SeriesName))
                {
                    _sortedData[item.SeriesName] = item.DataPoints;
                }
                else 
                {
                    _sortedData.Add(item.SeriesName, item.DataPoints);
                }
            }

            // Get data sorted by Heat treatment
            List<Tuple<string, double>> sortedByValue = new();
            foreach (var item in _phaseNames)
            {
                sortedByValue.Add(new Tuple<string,double>(item, 0));
            }

            // Sort by value
            foreach (var item in _htNames)
            {
                for (int i = 0; i < _phaseNames.Length; i++)
                {
                    IDataPoint? phaseValue = _dataPlots
                        .Where(e => e.SeriesName == _phaseNames[i])
                        .SelectMany(e => e.DataPoints)
                        .Where(e => e.Label == item)
                        .FirstOrDefault();

                    sortedByValue[i] = new Tuple<string, double>(sortedByValue[i].Item1, sortedByValue[i].Item2 + (phaseValue?.X ?? 0) );
                }
            }

            var customComparer = Comparer<Tuple<string, double>>.Create((x, y) => x.Item2.CompareTo(y.Item2));

            sortedByValue.Sort(customComparer);
            _phaseNames = sortedByValue.Select(x => x.Item1).ToArray();

            foreach (var item in _htNames)
            {
                List<IDataPoint> listy = new();

                for (int i = 0; i < _phaseNames.Length; i++)
                {
                    IDataPoint? phaseValue = _dataPlots
                        .Where(e => e.SeriesName == _phaseNames[i])
                        .SelectMany(e => e.DataPoints)
                        .Where(e => e.Label == item)
                        .FirstOrDefault();

                    listy.Add(phaseValue);
                }

                if (_sortedData.ContainsKey(item))
                {
                    _sortedData[item] = listy;
                }
                else
                {
                    _sortedData.Add(item, listy);
                }
            }


        }

        public void Refresh_Data()
        {
            _dataPlots = _plotController.Get_HeatMap_GrainSize_vs_PhaseFraction_ObjectData();
            UpdateSortedData();

            _dataOrigin.Clear();
            foreach (var item in _dataPlots)
            {
                if (!_dataOrigin.Contains(item.Name)) _dataOrigin.Add(item.Name);
            }
            OnPropertyChanged(nameof(DataOrigin));

            Controller_Global.MainControl?.Show_loading(true);

            Thread TH01 = new(Plot_Data);
            TH01.Priority = ThreadPriority.Highest;
            TH01.Start();

            // Plot_Data();
        }

        public void Plot_Data()
        {
            if (_dataPlots.Count == 0) return;

            SelectedDataOrigin = _dataPlots[0].Name;
            XDataColumn = _dataPlots[0].DataOptions[_xDataOption];
            YDataColumn = _dataPlots[0].DataOptions[_yDataOption];

            _xaxis.Name = XDataColumn;
            _yaxis.Name = YDataColumn;

            Random random = new();

            // set data  options
            foreach (var dplot in _dataPlots)
            {
                dplot.X_Data_Option(_xDataOption);
                dplot.Y_Data_Option(_yDataOption);
            }

            UpdateSortedData();

            SetSpyder3();
            //SetParallax2();
            ScatterBoxSeries[] sbsArray = new ScatterBoxSeries[_dataPlots.Count];
            for (int i = 0; i < _dataPlots.Count; i++)
            {
                sbsArray[i] = new();
                sbsArray[i].Label = _dataPlots[i].SeriesName;

                AMControls.Charts.Interfaces.IDataPoint[] tempVals = new AMControls.Charts.Interfaces.IDataPoint[_dataPlots[i].DataPoints.Count];
                _dataPlots[i].DataPoints.CopyTo(tempVals, 0);
                sbsArray[i].DataPoints = tempVals.ToList();
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Main_plot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => Main_plot.Add_series(e));
                Main_plot.Adjust_axes_to_data();
                Controller_Global.MainControl?.Show_loading(false);
            }));
        }

        private void SetSpyder()
        {
            Random random = new();
            SpyderSeries[] sbsArray = new SpyderSeries[_dataPlots.Count];

            for (int i = 0; i < _dataPlots.Count; i++)
            {
                sbsArray[i] = new();
                sbsArray[i].Label = _dataPlots[i].SeriesName;

                _dataPlots[i].DataPoints.ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            int axisNumber = sbsArray.Max(e => e.DataPoints.Count);
            string[] axisNames = sbsArray.Where(e => e.DataPoints.Count == axisNumber).SelectMany(e => e.DataPoints.Select(s => s.Label)).ToArray();
            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Min() ?? 0;

            spyderPlot.ClearAxis();
            for (int i = 0; i < axisNumber; i++)
            {
                LinearAxe lAxe = new()
                {
                    Name = axisNames[i],
                    MinValue = minValue,
                    MaxValue = maxValue,
                    Ticks = 10,
                };
                spyderPlot.Add_axis(lAxe);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                spyderPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => spyderPlot.Add_series(e));
                spyderPlot.InvalidateVisual();
            }));

        }

        private void SetSpyder2()
        {

            string[] uniqueHT = _dataPlots
                .SelectMany(e => e.DataPoints)
                .Select(e => e.Label)
                .Distinct()
                .ToArray();

            string[] phaseNames = _dataPlots.Select(e => e.SeriesName).ToArray();

            Random random = new();
            SpyderSeries[] sbsArray = new SpyderSeries[uniqueHT.Length];

            Dictionary<string, List<IDataPoint>> seriesData = new();
            foreach (var item in uniqueHT)
            {
                List<IDataPoint> listy = new();

                foreach (var phase in phaseNames)
                {
                    IDataPoint? phaseValue = _dataPlots
                        .Where(e => e.SeriesName == phase)
                        .SelectMany(e => e.DataPoints)
                        .Where(e => e.Label == item)
                        .FirstOrDefault();

                    listy.Add(phaseValue);
                }

                seriesData.Add(item, listy);
            }

            for (int i = 0; i < uniqueHT.Length; i++)
            {

                sbsArray[i] = new();
                sbsArray[i].Label = uniqueHT[i];

                List<IDataPoint> testy = seriesData[uniqueHT[i]];
                for (int j = 0; j < phaseNames.Length; j++)
                {
                    testy[j].Label = phaseNames[j];
                }

                seriesData[uniqueHT[i]].ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            int axisNumber = sbsArray.Max(e => e.DataPoints.Count);
            string[] axisNames = sbsArray.Where(e => e.DataPoints.Count == axisNumber).SelectMany(e => e.DataPoints.Select(s => s.Label)).ToArray();
            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Min() ?? 0;

            spyderPlot.ClearAxis();
            for (int i = 0; i < axisNumber; i++)
            {
                LinearAxe lAxe = new()
                {
                    Name = axisNames[i],
                    MinValue = minValue,
                    MaxValue = maxValue,
                    Ticks = 10,
                };
                spyderPlot.Add_axis(lAxe);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                spyderPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => spyderPlot.Add_series(e));
                spyderPlot.InvalidateVisual();
            }));
        }

        private void SetSpyder3()
        {
            Random random = new();
            SpyderSeries[] sbsArray = new SpyderSeries[_htNames.Length];

            for (int i = 0; i < _htNames.Length; i++)
            {

                sbsArray[i] = new();
                sbsArray[i].Label = _htNames[i];

                List<IDataPoint> testy = _sortedData[_htNames[i]];
                for (int j = 0; j < _phaseNames.Length; j++)
                {
                    if (testy[j] == null) testy[j] = new DataPoint();
                    testy[j].Label = _phaseNames[j];
                }

                _sortedData[_htNames[i]].ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Min() ?? 0;

            spyderPlot.ClearAxis();
            for (int i = 0; i < _phaseNames.Length; i++)
            {
                double localMaxValue = _sortedData[_phaseNames[i]].Select(e => e.X).Max();
                LinearAxe lAxe = new()
                {
                    Name = _phaseNames[i],
                    MinValue = minValue,
                    MaxValue = maxValue * 0.2 < localMaxValue ? maxValue : localMaxValue > 0 ? localMaxValue * 2 : maxValue,
                    Ticks = 5,
                };
                spyderPlot.Add_axis(lAxe);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                spyderPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => spyderPlot.Add_series(e));
                spyderPlot.InvalidateVisual();
            }));
        }


        // TODO: Example on data set for parallax, this is just a fast implementation, refactor please :')
        private void SetParallax()
        {

            string[] uniqueHT = _dataPlots
                .SelectMany(e => e.DataPoints)
                .Select(e => e.Label)
                .Distinct()
                .ToArray();

            string[] phaseNames = _dataPlots.Select(e => e.SeriesName).ToArray();

            Random random = new();
            ParallaxLineSeries[] sbsArray = new ParallaxLineSeries[uniqueHT.Length];

            Dictionary<string, List<IDataPoint>> seriesData = new();
            foreach (var item in uniqueHT)
            {
                List<IDataPoint> listy = new();

                foreach (var phase in phaseNames)
                {
                    IDataPoint? phaseValue = _dataPlots
                        .Where(e => e.SeriesName == phase)
                        .SelectMany(e => e.DataPoints)
                        .Where(e => e.Label == item)
                        .FirstOrDefault();

                    listy.Add(phaseValue);
                }

                seriesData.Add(item, listy);
            }

            for (int i = 0; i < uniqueHT.Length; i++)
            {

                sbsArray[i] = new();
                sbsArray[i].Label = uniqueHT[i];

                List<IDataPoint> testy = seriesData[uniqueHT[i]];
                for (int j = 0; j < phaseNames.Length; j++)
                {
                    testy[j].Label = phaseNames[j];
                }

                seriesData[uniqueHT[i]].ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            int axisNumber = sbsArray.Max(e => e.DataPoints.Count);
            string[] axisNames = sbsArray.Where(e => e.DataPoints.Count == axisNumber).SelectMany(e => e.DataPoints.Select(s => s.Label)).ToArray();
            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Min() ?? 0;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                parallaxPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => parallaxPlot.Add_series(e));
                parallaxPlot.InvalidateVisual();
            }));
        }

        private void SetParallax2()
        {
            Random random = new();
            ParallaxLineSeries[] sbsArray = new ParallaxLineSeries[_htNames.Length];

            for (int i = 0; i < _htNames.Length; i++)
            {

                sbsArray[i] = new();
                sbsArray[i].Label = _htNames[i];

                List<IDataPoint> testy = _sortedData[_htNames[i]];
                for (int j = 0; j < _phaseNames.Length; j++)
                {
                    testy[j].Label = _phaseNames[j];
                }

                _sortedData[_htNames[i]].ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            int axisNumber = sbsArray.Max(e => e.DataPoints.Count);
            string[] axisNames = sbsArray.Where(e => e.DataPoints.Count == axisNumber).SelectMany(e => e.DataPoints.Select(s => s.Label)).ToArray();
            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.Y))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.Y))?.Min() ?? 0;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                parallaxPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => parallaxPlot.Add_series(e));
                parallaxPlot.InvalidateVisual();
            }));
        }

        public void Select_Data()
        {

        }

        private void Main_plot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (SearchControl.Visibility != Visibility.Visible)
                {
                    SearchControl.Visibility = Visibility.Visible;
                }
                else
                {
                    SearchControl.Visibility = Visibility.Collapsed;
                }

            }
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (ToolBorder.Visibility != Visibility.Visible)
                {
                    ToolBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Main_plot_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (SearchControl.Visibility != Visibility.Visible)
                {
                    SearchControl.Visibility = Visibility.Visible;

                    CubicEase ease = new() { EasingMode = EasingMode.EaseOut };
                    DoubleAnimation sTextAnim = new(1, new Duration(TimeSpan.FromMilliseconds(900)))
                    {
                        From = 0,
                        EasingFunction = ease
                    };

                    stringSearch.BeginAnimation(Border.OpacityProperty, sTextAnim);

                    stringSearch.Select(stringSearch.Text.Length, 0);
                    stringSearch.Focus();
                }
                else
                {
                    SearchControl.Visibility = Visibility.Collapsed;
                }
            }
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (ToolBorder.Visibility != Visibility.Visible)
                {
                    ToolBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Main_plot_MouseEnter(object sender, MouseEventArgs e)
        {
            Main_plot.Focus();
        }

        private void AM_button_ClickButton(object? sender, EventArgs e)
        {
            if (SearchByRegion.IsChecked == true)
            {

            }
            else
            {
                List<IDataPoint> dResult = Main_plot.Search(stringSearch.Text);
                if (dResult.Count == 0)
                {
                    MainWindow.notify.ShowBalloonTip(5000, "No results", "No available data was found with the given search criteria", System.Windows.Forms.ToolTipIcon.Info);
                }
            }
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AM_button_ClickButton(null, EventArgs.Empty);
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

            ExpanderColumn.MinWidth = 0;

            Storyboard storyboard = new();

            Duration duration = new(TimeSpan.FromMilliseconds(500));
            CubicEase ease = new() { EasingMode = EasingMode.EaseOut };

            DoubleAnimation animation = new()
            {
                EasingFunction = ease,
                Duration = duration
            };
            animation.Completed += Animation_expand_completed;
            storyboard.Children.Add(animation);
            animation.From = ExpanderColumn.ActualWidth;
            animation.To = 200;
            Storyboard.SetTarget(animation, ExpanderColumn);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

            storyboard.Begin();
        }

        private void Animation_expand_completed(object? sender, EventArgs e)
        {
            ExpanderColumn.MinWidth = 200;
            ExpanderColumn.MaxWidth = 500;
        }
        private void Animation_contract_completed(object? sender, EventArgs e)
        {
            ExpanderColumn.MinWidth = 0;
            ExpanderColumn.MaxWidth = 50;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderColumn.MinWidth = 0;

            Storyboard storyboard = new();

            Duration duration = new(TimeSpan.FromMilliseconds(500));
            CubicEase ease = new() { EasingMode = EasingMode.EaseOut };

            DoubleAnimation animation = new()
            {
                EasingFunction = ease,
                Duration = duration
            };
            animation.Completed += Animation_contract_completed;
            storyboard.Children.Add(animation);
            animation.From = ExpanderColumn.ActualWidth;
            animation.To = 50;
            Storyboard.SetTarget(animation, ExpanderColumn);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

            storyboard.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread TH01 = new(Plot_Data);
            TH01.Priority = ThreadPriority.Highest;
            TH01.Start();
        }

        // TODO: temporal solution for showing different charts
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (spyderPlot.Visibility == Visibility.Visible)
            {
                Main_plot.Visibility = Visibility.Collapsed;
                parallaxPlot.Visibility = Visibility.Visible;
                spyderPlot.Visibility = Visibility.Collapsed;
                yAxisData.IsEnabled = false;
                
            }
            else if (parallaxPlot.Visibility == Visibility.Visible)
            {
                Main_plot.Visibility = Visibility.Visible;
                parallaxPlot.Visibility = Visibility.Collapsed;
                spyderPlot.Visibility = Visibility.Collapsed;
                yAxisData.IsEnabled = true;
            }
            else
            {
                Main_plot.Visibility = Visibility.Collapsed;
                spyderPlot.Visibility = Visibility.Visible;
                parallaxPlot.Visibility = Visibility.Collapsed;
                yAxisData.IsEnabled = false;
            }


        }
    }
}
