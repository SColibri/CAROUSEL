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
                ChartingWindow.SpyderSeriesData tempSeries = new()
                {
                    SeriesName = _plotController.SpyderDataPlot[n1].Name
                };

                for (int n2 = 0; n2 < _plotController.SpyderDataPlot[n1].Values.Count; n2++)
                {
                    tempSeries.Data.Add(_plotController.SpyderDataPlot[n1].Values[n2][0]);
                    tempSeries.AxeName.Add(_plotController.SpyderDataPlot[n1].Phases[n2].Name);
                }

                MainChart.Add_series(tempSeries);
            }
        }

        private void update_spyderplot()
        {
            _plotController.Spyderplot_get_data();
            SpyderPlot.Plot.Clear();

            List<ChartingWindow.SpyderSeriesData> listSpyder = new();
            for (int n1 = 0; n1 < _plotController.SpyderDataPlot.Count; n1++)
            {
                ChartingWindow.SpyderSeriesData tempSeries = new()
                {
                    SeriesName = _plotController.SpyderDataPlot[n1].IDCase.ToString()
                };

                for (int n2 = 0; n2 < _plotController.SpyderDataPlot[n1].Values.Count; n2++)
                {
                    tempSeries.Data.Add(_plotController.SpyderDataPlot[n1].Values[n2][0]);
                    tempSeries.AxeName.Add(_plotController.SpyderDataPlot[n1].Phases[n2].Name);
                }

                listSpyder.Add(tempSeries);
            }

            if (listSpyder.Count == 0) return;
            List<string> AxesNames = listSpyder.SelectMany(e => e.AxeName).Select(fy => fy).Distinct().ToList();
            double[,] mainData = new double[listSpyder.Count, AxesNames.Count];
            double[] maxValues = { 100, 100, 3, 3, 3, 3, 3, 3, 3 };
            for (int i = 0; i < listSpyder.Count; i++) 
            {
                var dim0 = mainData.GetLength(0);
                var dim1 = mainData.GetLength(1);

                double[] tempValues = listSpyder[i].Data.ToArray();
                var tempArr = new double[dim0 + 1, dim1];
                Array.Copy(mainData, tempArr, AxesNames.Count);
                for (var n2 = 0; n2 < dim1; ++n2) tempArr[dim0, n2] = listSpyder[i].Data[n2]*100;
                mainData = tempArr;
            } 

            var RadarT = SpyderPlot.Plot.AddRadar(mainData, true, maxValues);
            RadarT.CategoryLabels = AxesNames.ToArray();
            RadarT.ShowAxisValues = true;
            
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            update_plot();
            update_spyderplot();
            MainChart.UpdateImage();
            //SpyderPlot.Refresh();

        }
    }
}
