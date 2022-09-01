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
