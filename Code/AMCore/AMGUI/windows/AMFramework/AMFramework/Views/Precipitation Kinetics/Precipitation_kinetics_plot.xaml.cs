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

namespace AMFramework.Views.Precipitation_Kinetics
{
    /// <summary>
    /// Interaction logic for Precipitation_kinetics_plot.xaml
    /// </summary>
    public partial class Precipitation_kinetics_plot : UserControl
    {
        private Controller.Controller_Plot ControllerPlot;
        public Precipitation_kinetics_plot()
        {
            InitializeComponent();
        }

        public Precipitation_kinetics_plot(Controller.Controller_Plot cPlot)
        {
            InitializeComponent();
            ControllerPlot = cPlot;
            DataContext = ControllerPlot;
            updatePlots();
        }

        public void updatePlots() 
        {
            if (ControllerPlot is null) return;
            if (ControllerPlot.HeatModel.ID == -1) return;
            if (ControllerPlot.HeatModel.PrecipitationData.Count == 0) return;

            double[] time_________ = new double[ControllerPlot.HeatModel.HeatTreatmentProfile.Count];
            double[] temperature__ = new double[ControllerPlot.HeatModel.HeatTreatmentProfile.Count];
            

            for(int n1 = 0; n1 < ControllerPlot.HeatModel.HeatTreatmentProfile.Count; n1++) 
            {
                time_________[n1] = ControllerPlot.HeatModel.HeatTreatmentProfile[n1].Time;
                temperature__[n1] = ControllerPlot.HeatModel.HeatTreatmentProfile[n1].Temperature;
            }

            if (time_________.Length == 0) return;
            var handl_01 = plot01.Plot.AddScatterLines(time_________, temperature__);

            List<int> distinctIDphase = ControllerPlot.HeatModel.PrecipitationData.Select(x => x.IDPrecipitationPhase).Distinct().ToList();

            List<double[]> listData02 = new();
            List<double[]> listData03 = new();
            List<double[]> listData04 = new();

            for (int n0 = 0; n0 < distinctIDphase.Count; n0++) 
            {
                List<Model.Model_PrecipitateSimulationData> SlectedMod = ControllerPlot.HeatModel.PrecipitationData.FindAll(x => x.IDPrecipitationPhase == distinctIDphase[n0]);
                double[] radius_______ = new double[ControllerPlot.HeatModel.HeatTreatmentProfile.Count];
                double[] phasefraction = new double[ControllerPlot.HeatModel.HeatTreatmentProfile.Count];
                double[] density______ = new double[ControllerPlot.HeatModel.HeatTreatmentProfile.Count];

                for (int n1 = 0; n1 < ControllerPlot.HeatModel.HeatTreatmentProfile.Count; n1++)
                {
                    radius_______[n1] = SlectedMod[n1].MeanRadius*1e+9;
                    phasefraction[n1] = SlectedMod[n1].PhaseFraction;
                    density______[n1] = SlectedMod[n1].NumberDensity;
                }

                listData02.Add(radius_______);
                listData03.Add(phasefraction);
                listData04.Add(density______);

                var handl_02 = plot02.Plot.AddScatterLines(time_________, listData02[listData02.Count-1], label: SlectedMod[0].PrecipitationName);
                var handl_03 = plot03.Plot.AddScatterLines(time_________, listData03[listData03.Count - 1], label: SlectedMod[0].PrecipitationName);
                var handl_04 = plot04.Plot.AddScatterLines(time_________, ScottPlot.Tools.Log10(listData04[listData04.Count - 1]), label: SlectedMod[0].PrecipitationName);
            }
            

            plot01.Plot.YLabel("Temperature (°C)");
            plot01.Plot.XLabel("Time (s)");
            plot01.Plot.Grid(true);

            plot02.Plot.YLabel("Mean Radius (nm)");
            plot02.Plot.XLabel("Time (s)");
            plot02.Plot.Grid(true);

            plot03.Plot.YLabel("Phase fraction (% weight)");
            plot03.Plot.XLabel("Time (s)");
            plot03.Plot.Grid(true);

            plot04.Plot.YLabel("Density (m^-3)");
            plot04.Plot.XLabel("Time (s)");
            plot04.Plot.Grid(true);

            plot01.Plot.Legend();
            plot02.Plot.Legend();
            plot03.Plot.Legend();
            plot04.Plot.Legend();

            plot01.Refresh();
            plot02.Refresh();
            plot03.Refresh();
            plot04.Refresh();
        }
    }
}
