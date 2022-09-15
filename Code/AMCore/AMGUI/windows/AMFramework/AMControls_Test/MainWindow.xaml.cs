﻿using System;
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
            
            AMControls.Charts.IAxes xaxis = new AMControls.Charts.LinearAxe("x Axis");
            xaxis.MaxValue = 0.000002;
            xaxis.MinValue = 1e-16;

            AMControls.Charts.IAxes yaxis = new AMControls.Charts.LinearAxe("y Axis");
            yaxis.MaxValue = 0.02;
            yaxis.MinValue = 1e-16;

            scat.Set_xAxis(xaxis);
            scat.Set_yAxis(yaxis);
            

            AMControls.Charts.IDataSeries dataSeries = new AMControls.Charts.ScatterBoxSeries() { ColorBox = Colors.Pink, ColorBoxBackground = Colors.Pink};
            dataSeries.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.000002, Y = 0.02, Label = "AL3TI_L_P0" });
            dataSeries.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.000002, Y = 0.0048, Label = "MG2SI_B_P0" });
            dataSeries.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.00000037, Y = 0.00093, Label = "MG2SI_B_P0" });
            dataSeries.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.000002, Y = 0.0000000004, Label = "MG2SI_B_P0" });
            scat.Add_series(dataSeries);

            AMControls.Charts.IDataSeries dataSeries2 = new AMControls.Charts.ScatterBoxSeries();
            dataSeries2.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.000002, Y = 0.02, Label = "AL3TI_L_P0" });
            dataSeries2.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.0000021 , Y = 0.0053, Label = "MG2SI_B_P0" });
            dataSeries2.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.00000067 , Y = 0.0035, Label = "MG2SI_B_P0" });
            dataSeries2.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.0000000004 , Y = 0.000002, Label = "MG2SI_B_P0" });
            scat.Add_series(dataSeries2);

            AMControls.Charts.IDataSeries dataSeries3 = new AMControls.Charts.ScatterSeries() { ColorSeries = Colors.Red, Label = "Nice Series"};
            for (int n1 = 0; n1 < 100; n1++) 
            {
                dataSeries3.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.001 * n1, Y = 0.01 * n1, Label = "Point " + n1 });
            }
            scat.Add_series(dataSeries3);

            Random random = new Random();
            for (int n1 = 0; n1 < 5; n1++) 
            {
                AMControls.Charts.IDataSeries dataSeriesG = new AMControls.Charts.ScatterBoxSeries();
                dataSeriesG.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.000002 * random.NextDouble(), Y = 0.02 * random.NextDouble(), Label = "AL3TI_L_P0" });
                dataSeriesG.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.0000021 * random.NextDouble(), Y = 0.0053 * random.NextDouble(), Label = "MG2SI_B_P0" });
                dataSeriesG.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.00000067 * random.NextDouble(), Y = 0.0035 * random.NextDouble(), Label = "MG2SI_B_P0" });
                dataSeriesG.DataPoints.Add(new AMControls.Charts.DataPoint() { X = 0.0000000004 * random.NextDouble(), Y = 0.000002 * random.NextDouble(), Label = "MG2SI_B_P0" });
                scat.Add_series(dataSeriesG);
                dataSeriesG.Label = "Series_" + n1;
                dataSeriesG.ColorSeries = Color.FromRgb((byte)random.Next(1,255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            scat.Adjust_axes_to_data();
        }
    }
}