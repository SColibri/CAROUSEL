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
using AMControls;
using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Interfaces;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls_Test.Custom;
using AMControls_Test.Charts;
using AMControls_Test.WindowObjects.Notify;
using AMControls.Custom.Scripting;

namespace AMControls_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IAxes xaxis = new LinearAxe("x Axis");
            xaxis.MaxValue = 0.000002;
            xaxis.MinValue = 1e-16;

            IAxes yaxis = new LinearAxe("y Axis");
            yaxis.MaxValue = 0.02;
            yaxis.MinValue = 1e-16;

            scat.Set_xAxis(xaxis);
            scat.Set_yAxis(yaxis);
            

            IDataSeries dataSeries = new ScatterBoxSeries() { ColorBox = Colors.Pink, ColorBoxBackground = Colors.Pink};
            dataSeries.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.02, Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.0048, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries.Add_DataPoint(new DataPoint() { X = 0.00000037, Y = 0.00093, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.0000000004, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            scat.Add_series(dataSeries);

            IDataSeries dataSeries2 = new ScatterBoxSeries();
            dataSeries2.Add_DataPoint(new DataPoint() { X = 0.000002, Y = 0.02, Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries2.Add_DataPoint(new DataPoint() { X = 0.0000021 , Y = 0.0053, Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries2.Add_DataPoint(new DataPoint() { X = 0.00000067 , Y = 0.0035, Label = "MG2SI_B_P0" , ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            dataSeries2.Add_DataPoint(new DataPoint() { X = 0.0000000004 , Y = 0.000002, Label = "MG2SI_B_P0" , ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
            scat.Add_series(dataSeries2);

            IDataSeries dataSeries3 = new ScatterSeries() { ColorSeries = Colors.Red, Label = "Nice Series"};
            for (int n1 = 0; n1 < 100; n1++) 
            {
                dataSeries3.Add_DataPoint(new DataPoint() { X = 0.001 * n1, Y = 0.01 * n1, Label = "Point " + n1, ContextMenu = new DataPoint_ContextMenu_Text() });
            }
            scat.Add_series(dataSeries3);

            Random random = new Random();
            for (int n1 = 0; n1 < 505; n1++) 
            {
                IDataSeries dataSeriesG = new ScatterBoxSeries();
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.000002 * random.NextDouble(), Y = 0.02 * random.NextDouble(), Label = "AL3TI_L_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.0000021 * random.NextDouble(), Y = 0.0053 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.00000067 * random.NextDouble(), Y = 0.0035 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                dataSeriesG.Add_DataPoint(new DataPoint() { X = 0.0000000004 * random.NextDouble(), Y = 0.000002 * random.NextDouble(), Label = "MG2SI_B_P0", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "AL3TI_L_P0" } });
                scat.Add_series(dataSeriesG);
                dataSeriesG.Label = "Series_" + n1;
                dataSeriesG.ColorSeries = Color.FromRgb((byte)random.Next(1,255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            scat.Adjust_axes_to_data();

            scat.ChartAreaMouseMove += OnChartMouseMove_Handle;

            ParallaxExample pExample = new();
            pExample.Show();

            SpyderExample spyderExample = new SpyderExample();
            spyderExample.Show();

            NotifyCorner_Test notyExample = new();
            notyExample.Show();

            ScriptingView scView = new();
            scView.Show();
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
    }
}
