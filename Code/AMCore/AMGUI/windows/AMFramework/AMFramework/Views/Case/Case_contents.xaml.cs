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

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for Case_contents.xaml
    /// </summary>
    public partial class Case_contents : UserControl
    {
        public Case_contents()
        {
            InitializeComponent();
        }

        public Case_contents(ref Controller.Controller_Plot plotController)
        {
            InitializeComponent();
            DataContext = plotController;
            //plotController.refresh_used_Phases_inCases();

            //Components.Charting.LinePhaseChart.LinePhaseChart win01 = new Components.Charting.LinePhaseChart.LinePhaseChart(plotController);
            //win01.Height = 700;

            //Components.Charting.LinePhaseChart.SpiderPhaseChart win02 = new Components.Charting.LinePhaseChart.SpiderPhaseChart(plotController);
            //win02.Height = 700;

            //MainContent.Children.Add(win01);
            //MainContent.Children.Add(win02);

            //SelectedPhasesList.DataContext = plotController;

            
        }
    }
}
