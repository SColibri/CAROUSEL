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

namespace AMFramework.Views.Project_Map
{
    /// <summary>
    /// Interaction logic for Project_Map.xaml
    /// </summary>
    public partial class Project_Map : UserControl
    {
        private Controller.Controller_Plot _plotController;
        public Project_Map(Controller.Controller_Plot plotController)
        {
            InitializeComponent();
            _plotController = plotController;

            IAxes xaxis = new LinearAxe("Mean radius (M)");
            IAxes yaxis = new LinearAxe("Phase fraction");

            Main_plot.Set_xAxis(xaxis);
            Main_plot.Set_yAxis(yaxis);

            Refresh();
        }

        public void Refresh() 
        {
            Random random = new Random();
            List<IDataSeries> DList = _plotController.Get_HeatMap_GrainSize_vs_PhaseFraction();

            foreach (var item in DList)
            {
                Main_plot.Add_series(item);
                item.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            Main_plot.Adjust_axes_to_data();
        }
    }
}
