using System.Windows;
using System.Windows.Controls;

namespace AMFramework.Components.Charting.HeatMapFractions
{
    /// <summary>
    /// Interaction logic for HeatMap_PhaseFractions.xaml
    /// </summary>
    public partial class HeatMap_PhaseFractions : UserControl
    {
        public HeatMap_PhaseFractions()
        {
            InitializeComponent();
        }

        public void refresh(double[,] dataHeatMap)
        {
            var HMPlot = MainPlot.Plot.AddHeatmap(dataHeatMap, ScottPlot.Drawing.Colormap.Turbo);
            MainPlot.Plot.AddColorbar(HMPlot);

            MainPlot.Refresh();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainPlot.Refresh();
        }
    }
}
