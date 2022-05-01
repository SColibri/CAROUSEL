using System.Collections.Generic;
using System.Windows;
using ScintillaNET;

namespace AMFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Framey.Navigate(new Views.Config.Configuration());

            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });
            Chartingy.Add_Axe(new Components.Charting.Axes("Test") { MaxValue = 10 });

            List<List<double>> dataRadar = new();
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());
            dataRadar.Add(new List<double>());

            dataRadar[0].Add(5);
            dataRadar[1].Add(1);
            dataRadar[2].Add(3);
            dataRadar[3].Add(8);
            dataRadar[4].Add(4);
            dataRadar[5].Add(7);
            dataRadar[6].Add(9);
            dataRadar[7].Add(10);

            dataRadar[0].Add(2);
            dataRadar[1].Add(9);
            dataRadar[2].Add(8);
            dataRadar[3].Add(5);
            dataRadar[4].Add(1);
            dataRadar[5].Add(5);
            dataRadar[6].Add(2);
            dataRadar[7].Add(6);

            Chartingy.Data = dataRadar;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Chartingy.UpdateImage();
        }
    }
}
