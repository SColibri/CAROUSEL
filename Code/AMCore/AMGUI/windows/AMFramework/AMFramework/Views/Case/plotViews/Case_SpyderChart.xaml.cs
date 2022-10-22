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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMFramework.Views.Case.plotViews
{
    /// <summary>
    /// Interaction logic for Case_SpyderChart.xaml
    /// </summary>
    public partial class Case_SpyderChart : UserControl
    {
        public Case_SpyderChart()
        {
            InitializeComponent();

            IAxes T01 = new LinearAxe() { Name = "Axis_1", MinValue = 1, MaxValue = 10 };
            IAxes T02 = new LinearAxe() { Name = "Axis_2", MinValue = 1, MaxValue = 10 };
            IAxes T03 = new LinearAxe() { Name = "Axis_3", MinValue = 1, MaxValue = 10 };
            IAxes T04 = new LinearAxe() { Name = "Axis_4", MinValue = 1, MaxValue = 10 };
            IAxes T05 = new LinearAxe() { Name = "Axis_5", MinValue = 1, MaxValue = 10 };
            IAxes T06 = new LinearAxe() { Name = "Axis_6", MinValue = 1, MaxValue = 10 };

            SpyderMain.Add_axis(T01);
            SpyderMain.Add_axis(T02);
            SpyderMain.Add_axis(T03);
            SpyderMain.Add_axis(T04);
            SpyderMain.Add_axis(T05);
            SpyderMain.Add_axis(T06);

            Random random = new();
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
                SpyderMain.Add_series(nSeries);
            }

        }
    }
}
