using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using System;
using System.Windows;
using System.Windows.Media;

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

            // Create axis
            IAxes axis01 = new LinearAxe("01 Axis");
            axis01.MaxValue = 0.02;
            axis01.MinValue = 1e-16;

            IAxes axis02 = new LinearAxe("02 Axis");
            axis02.MaxValue = 0.02;
            axis02.MinValue = 1e-16;

            IAxes axis03 = new LinearAxe("02 Axis");
            axis03.MaxValue = 0.02;
            axis03.MinValue = 1e-16;

            IAxes axis04 = new LinearAxe("02 Axis");
            axis04.MaxValue = 0.02;
            axis04.MinValue = 1e-16;

            IAxes axis05 = new LinearAxe("02 Axis");
            axis05.MaxValue = 0.02;
            axis05.MinValue = 1e-16;

            IAxes axis06 = new LinearAxe("02 Axis");
            axis06.MaxValue = 0.02;
            axis06.MinValue = 1e-16;

            IAxes axis07 = new LinearAxe("02 Axis");
            axis07.MaxValue = 0.02;
            axis07.MinValue = 1e-16;

            spyderFriend.Add_axis(axis01);
            spyderFriend.Add_axis(axis02);
            spyderFriend.Add_axis(axis03);
            spyderFriend.Add_axis(axis04);
            spyderFriend.Add_axis(axis05);
            spyderFriend.Add_axis(axis06);
            spyderFriend.Add_axis(axis07);

            // Add data
            Random random = new Random();
            for (int n1 = 0; n1 < 6; n1++)
            {
                IDataSeries dSeries = new SpyderSeries();
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_01", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_01" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_02", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_02" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_03", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_03" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_04", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_04" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_05", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_04" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_06", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_04" } });
                dSeries.Add_DataPoint(new DataPoint() { X = 0.01 * random.NextDouble(), Label = "Phase_07", ContextMenu = new DataPoint_ContextMenu_Text() { Title = "Phase_04" } });

                spyderFriend.Add_series(dSeries);
                dSeries.Label = "Series_" + n1;
                dSeries.ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            
        }
    }
}
