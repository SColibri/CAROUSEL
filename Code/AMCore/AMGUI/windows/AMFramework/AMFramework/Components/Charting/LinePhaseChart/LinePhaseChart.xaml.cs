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
using System.ComponentModel;
using InteractiveDataDisplay.WPF;

namespace AMFramework.Components.Charting.LinePhaseChart
{
    /// <summary>
    /// Interaction logic for LinePhaseChart.xaml
    /// </summary>
    public partial class LinePhaseChart : UserControl, INotifyPropertyChanged
    {
        private Controller.Controller_Plot _plotController; 
        public LinePhaseChart()
        {
            InitializeComponent();
        }

        public LinePhaseChart(Controller.Controller_Plot plotController)
        {
            InitializeComponent();
            DataContext = plotController;
            _plotController = plotController;

            _plotController.PropertyChanged += handle_updated_plot;
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Handleevents

        private void handle_updated_plot(object sender, PropertyChangedEventArgs e) 
        {
            if (e is null) return;
            if(e.PropertyName.CompareTo("LineGraphs") == 0) 
            {
                ListPlot.Plot.Clear();
                for (int n1 = 0; n1 < _plotController.LineGraphs.Count; n1++)
                {
                    ListGraph.Children.Add(_plotController.LineGraphs[n1]);
                }
            }
        }

        #endregion

        /*
         
         <plot:Chart BottomTitle="Temperature" LeftTitle="Phase fraction" Visibility="Visible">
                <plot:Chart.Title>
                    <TextBlock HorizontalAlignment="Center" FontSize="18" Margin="0,5,0,5">Line graph legend sample</TextBlock>
                </plot:Chart.Title>
                <plot:Chart.LegendContent>
                    <plot:LegendItemsPanel>
                        <plot:LegendItemsPanel.Resources>
                            <DataTemplate x:Key="InteractiveDataDisplay.WPF.LineGraph">
                                <StackPanel Orientation="Horizontal">
                                    <CheckBox/>
                                    <Line Width="15" Height="15" X1="0" Y1="0" X2="15" Y2="15" Stroke="{Binding Path=Stroke}" StrokeThickness="2"/>
                                    <TextBlock Margin="5,0,0,0" Text="{Binding Path=Description}"/>
                                </StackPanel>
                            </DataTemplate>
                        </plot:LegendItemsPanel.Resources>
                    </plot:LegendItemsPanel>
                </plot:Chart.LegendContent>
                <Grid x:Name="ListGraph">
                    <ItemsControl ItemsSource="{Binding Path=LineGraphs, UpdateSourceTrigger=PropertyChanged}"/>
                </Grid>
             </plot:Chart>
         */


    }
}
