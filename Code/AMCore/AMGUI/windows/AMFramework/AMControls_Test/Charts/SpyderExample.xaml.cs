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
using AMControls.Charts;
using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;

namespace AMControls_Test.Charts
{
    /// <summary>
    /// Interaction logic for SpyderExample.xaml
    /// </summary>
    public partial class SpyderExample : Window
    {
        public SpyderExample()
        {
            InitializeComponent();

            AMControls.Charts.Spyder.SpyderPlot sp = new();

            IAxes T01 = new LinearAxe() { Name = "Axis_1", MinValue = 1, MaxValue = 10 };
            IAxes T02 = new LinearAxe() { Name = "Axis_2", MinValue = 1, MaxValue = 10 };
            IAxes T03 = new LinearAxe() { Name = "Axis_3", MinValue = 1, MaxValue = 10 };
            IAxes T04 = new LinearAxe() { Name = "Axis_4", MinValue = 1, MaxValue = 10 };
            IAxes T05 = new LinearAxe() { Name = "Axis_5", MinValue = 1, MaxValue = 10 };
            IAxes T06 = new LinearAxe() { Name = "Axis_6", MinValue = 1, MaxValue = 10 };

            sp.Add_axis(T01); 
            sp.Add_axis(T02); 
            sp.Add_axis(T03);
            sp.Add_axis(T04);
            sp.Add_axis(T05);
            sp.Add_axis(T06);

            Random random = new Random();
            for (int n1 = 0; n1 < 5; n1++)
            {
                IDataSeries nSeries = new SpyderSeries();
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_1", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_2", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_3", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_4", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_5", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.Add_DataPoint(new DataPoint() { X = random.Next(1, 10), Label = "Axis_6", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                nSeries.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
                sp.Add_series(nSeries);
            }

            this.Content = sp;
        }
    }
}
