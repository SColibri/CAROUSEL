using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMControls.Charts;
using AMControls.Charts.Interfaces;
using AMControls.Charts.Implementations;
using System.Windows.Media.Animation;
using AMFramework.Components.Charting.Interfaces;
using AMFramework.Controller;
using AMControls.Charts.Implementations.DataSeries;
using System.ComponentModel;

namespace AMFramework.Views.Project_Map
{
    /// <summary>
    /// Interaction logic for Project_Map.xaml
    /// </summary>
    public partial class Project_Map : UserControl, INotifyPropertyChanged
    {
        private Controller_Plot _plotController;
        private List<IDataPlot> _dataPlots;
        private IAxes _xaxis;
        private IAxes _yaxis;
        private DataOptions _dataOption = DataOptions.Precipitation_Phase;
        private int _xDataOption = 2;
        private int _yDataOption = 0;

        public enum DataOptions 
        { 
            Precipitation_Phase
        }

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

        public void Refresh_Data() 
        {
            _dataPlots = _plotController.Get_HeatMap_GrainSize_vs_PhaseFraction_ObjectData();

            _dataOrigin.Clear();
            foreach (var item in _dataPlots)
            {
                if (!_dataOrigin.Contains(item.Name)) _dataOrigin.Add(item.Name);
            }
            OnPropertyChanged(nameof(DataOrigin));

            Plot_Data();
        }

        public void Plot_Data() 
        {

            Main_plot.Clear_Series();

            if (_dataPlots.Count == 0) return;
            SelectedDataOrigin = _dataPlots[0].Name;
            XDataColumn = _dataPlots[0].DataOptions[_xDataOption];
            YDataColumn = _dataPlots[0].DataOptions[_yDataOption];

            _xaxis.Name = _dataPlots[0].DataOptions[_xDataOption];
            _yaxis.Name = _dataPlots[0].DataOptions[_yDataOption];

            Random random = new();

            foreach (var dplot in _dataPlots)
            {
                ScatterBoxSeries sbs = new();
                dplot.X_Data_Option(_xDataOption);
                dplot.Y_Data_Option(_yDataOption);
                sbs.Label = dplot.SeriesName;
                sbs.DataPoints = dplot.DataPoints;
                if (sbs.DataPoints.Count == 0) continue;
                Main_plot.Add_series(sbs);
                sbs.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            Main_plot.Adjust_axes_to_data();

        }

        public void Select_Data() 
        { 
            
        }

        private void Main_plot_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                if(SearchControl.Visibility != Visibility.Visible) 
                {
                    SearchControl.Visibility = Visibility.Visible;
                }
                else 
                {
                    SearchControl.Visibility = Visibility.Collapsed;
                }
                
            }
            else if(e.Key == Key.T && Keyboard.IsKeyDown(Key.LeftCtrl)) 
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
                if(dResult.Count == 0) 
                {
                    MainWindow.notify.ShowBalloonTip(5000, "No results", "No available data was found with the given search criteria", System.Windows.Forms.ToolTipIcon.Info);
                }
            }
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
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
            Plot_Data();
        }
    }
}
