using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Implementations;
using AMControls.Charts.Interfaces;
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
using System.Windows.Shapes;

namespace AMControls_Test.Charts
{
    /// <summary>
    /// Interaction logic for ParallaxExample.xaml
    /// </summary>
    public partial class ParallaxExample : Window
    {
        public ParallaxExample()
        {
            InitializeComponent();

            AMControls.Charts.Parallax.ParallaxPlot pChart = new();
            
            Random random = new Random();

            if(1 == 1) 
            {
                IDataSeries dataSeries1 = new ParallaxLineSeries();
                dataSeries1.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.02, Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries1.Add_DataPoint(new DataPoint() { X = 0.0000021, Y = 0.0053, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries1.Add_DataPoint(new DataPoint() { X = 0.00000067, Y = 0.0035, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries1.Add_DataPoint(new DataPoint() { X = 0.0000000004, Y = 0.000002, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                pChart.Add_series(dataSeries1);
                dataSeries1.Label = "Series_1";
                dataSeries1.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));

                IDataSeries dataSeries2 = new ParallaxLineSeries();
                dataSeries2.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.021, Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries2.Add_DataPoint(new DataPoint() { X = 0.0000021, Y = 0.0054, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries2.Add_DataPoint(new DataPoint() { X = 0.00000067, Y = 0.0045, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries2.Add_DataPoint(new DataPoint() { X = 0.0000000004, Y = 0.000012, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                pChart.Add_series(dataSeries2);
                dataSeries2.Label = "Series_2";
                dataSeries2.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));

                IDataSeries dataSeries3 = new ParallaxLineSeries();
                dataSeries3.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.019, Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries3.Add_DataPoint(new DataPoint() { X = 0.0000021, Y = 0.0052, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries3.Add_DataPoint(new DataPoint() { X = 0.00000067, Y = 0.0025, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeries3.Add_DataPoint(new DataPoint() { X = 0.0000000004, Y = 0.0000010, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                pChart.Add_series(dataSeries3);
                dataSeries3.Label = "Series_3";
                dataSeries3.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            for (int n1 = 0; n1 < 0; n1++)
            {
                IDataSeries dataSeriesG = new ParallaxLineSeries();
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.000002 * random.NextDouble(), Y = 0.02 * random.NextDouble(), Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.0000021 * random.NextDouble(), Y = 0.0053 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.00000067 * random.NextDouble(), Y = 0.0035 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.0000000004 * random.NextDouble(), Y = 0.000002 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                pChart.Add_series(dataSeriesG);
                dataSeriesG.Label = "Series_" + n1;
                dataSeriesG.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            this.Content = pChart;
        }
    }
}
