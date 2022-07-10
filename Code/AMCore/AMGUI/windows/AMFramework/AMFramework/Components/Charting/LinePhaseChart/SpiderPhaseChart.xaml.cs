using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AMFramework.Components.Charting.LinePhaseChart
{
    /// <summary>
    /// Interaction logic for SpiderPhaseChart.xaml
    /// </summary>
    public partial class SpiderPhaseChart : UserControl
    {
        private Controller.Controller_Plot _plotController;
        public SpiderPhaseChart()
        {
            InitializeComponent();
        }

        public SpiderPhaseChart(Controller.Controller_Plot plotController)
        {
            InitializeComponent();
            DataContext = plotController;
            _plotController = plotController;

            _plotController.PropertyChanged += handle_updated_plot;
        }

        #region Handleevents

        private void handle_updated_plot(object sender, PropertyChangedEventArgs e)
        {
            if (e is null) return;
            if (e.PropertyName.CompareTo("SpyderDataPlot") != 0) return;

            update_plot();
        }

        private void update_plot() 
        {
            if (_plotController.SpyderDataPlot.Count == 0) _plotController.Spyderplot_get_data();

            MainChart.ClearAll();
            
            for (int n1 = 0; n1 < _plotController.SpyderDataPlot.Count; n1++)
            {
                ChartingWindow.SpyderSeriesData tempSeries = new();
                tempSeries.SeriesName = _plotController.SpyderDataPlot[n1].IDCase.ToString();

                for (int n2 = 0; n2 < _plotController.SpyderDataPlot[n1].Values.Count; n2++)
                {
                    tempSeries.Data.Add(_plotController.SpyderDataPlot[n1].Values[n2][0]);
                    tempSeries.AxeName.Add(_plotController.SpyderDataPlot[n1].Phases[n2].Name);
                }

                MainChart.Add_series(tempSeries);
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            update_plot();
            MainChart.UpdateImage();
        }
    }
}
