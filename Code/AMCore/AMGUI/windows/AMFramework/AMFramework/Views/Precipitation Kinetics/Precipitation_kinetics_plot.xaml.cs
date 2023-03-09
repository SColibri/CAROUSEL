using AMFramework_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            if (ControllerPlot.HeatModel.PrecipitationDataOLD.Count == 0) return;

            double[] time_________ = new double[ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count];
            double[] temperature__ = new double[ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count];


            for (int n1 = 0; n1 < ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count; n1++)
            {
                time_________[n1] = ControllerPlot.HeatModel.HeatTreatmentProfileOLD[n1].Time/(60*60);
                temperature__[n1] = ControllerPlot.HeatModel.HeatTreatmentProfileOLD[n1].Temperature;
            }

            if (time_________.Length == 0) return;  // no data available?
            var handl_01 = plot01.Plot.AddScatterLines(time_________, temperature__, label: ControllerPlot.HeatModel.Name);

            List<int> distinctIDphase = ControllerPlot.HeatModel.PrecipitationDataOLD.Select(x => x.IDPrecipitationPhase).Distinct().ToList();

            List<double[]> listData02 = new();
            List<double[]> listData03 = new();
            List<double[]> listData04 = new();

            for (int n0 = 0; n0 < distinctIDphase.Count; n0++)
            {
                List<Model_PrecipitateSimulationData> SlectedMod = ControllerPlot.HeatModel.PrecipitationDataOLD.FindAll(x => x.IDPrecipitationPhase == distinctIDphase[n0]);
                double[] radius_______ = new double[ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count];
                double[] phasefraction = new double[ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count];
                double[] density______ = new double[ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count];

                for (int n1 = 0; n1 < ControllerPlot.HeatModel.HeatTreatmentProfileOLD.Count; n1++)
                {
                    radius_______[n1] = SlectedMod[n1].MeanRadius*1e+9;
                    phasefraction[n1] = SlectedMod[n1].PhaseFraction;
                    density______[n1] = SlectedMod[n1].NumberDensity;
                }

                listData02.Add(radius_______);
                listData03.Add(phasefraction);
                listData04.Add(density______);

                var handl_02 = plot02.Plot.AddScatterLines(time_________, ScottPlot.Tools.Log10(listData02[listData02.Count-1]), label: SlectedMod[0].PrecipitationName);
                var handl_03 = plot03.Plot.AddScatterLines(time_________, listData03[listData03.Count - 1], label: SlectedMod[0].PrecipitationName);
                var handl_04 = plot04.Plot.AddScatterLines(time_________, ScottPlot.Tools.Log10(listData04[listData04.Count - 1]), label: SlectedMod[0].PrecipitationName);
            }


            plot01.Plot.YLabel("Temperature (°C)");
            plot01.Plot.XLabel("Time (h)");
            plot01.Plot.Grid(true);

            plot02.Plot.YLabel("Mean Radius (nm)");
            plot02.Plot.XLabel("Time (h)");
            plot02.Plot.Grid(true);

            plot03.Plot.YLabel("Phase fraction (% weight)");
            plot03.Plot.XLabel("Time (h)");
            plot03.Plot.Grid(true);

            plot04.Plot.YLabel("Density (m^-3)");
            plot04.Plot.XLabel("Time (h)");
            plot04.Plot.Grid(true);

            plot01.Plot.Legend();
            plot02.Plot.Legend();
            plot03.Plot.Legend();
            plot04.Plot.Legend();

            plot01.Refresh();
            plot02.Refresh();
            plot03.Refresh();
            plot04.Refresh();


            // TEST CODE ONLY, REMOVE IF FORGOTTEN
            //getHM(ControllerPlot.Get_heat_map_phaseFraction_kinetics());
            //getHM(ControllerPlot.Get_heat_map_grainSize_kinetics());

        }


        private void getHM(List<Tuple<double, Tuple<string, string>>> HMdata)
        {
            Window Nw01 = new();
            Components.Charting.HeatMapFractions.HeatMap_PhaseFractions CompT = new();
            Nw01.Content = CompT;
            Nw01.Show();

            List<string> HeatTreatmentList = new();
            List<string> precipitationPhases = new();
            List<Tuple<List<double>, Tuple<string, List<string>>>> dataByHT = new();

            foreach (var item in HMdata)
            {
                if (HeatTreatmentList.Contains(item.Item2.Item2) == false)
                {
                    HeatTreatmentList.Add(item.Item2.Item2);

                    List<Tuple<double, Tuple<string, string>>> tempFind = HMdata.FindAll(e => e.Item2.Item2.CompareTo(item.Item2.Item2) == 0).ToList();
                    List<double> tempDouble = new();
                    List<string> tempNames = new();

                    foreach (var hT in tempFind)
                    {
                        if (precipitationPhases.Contains(hT.Item2.Item1) == false) precipitationPhases.Add(hT.Item2.Item1);
                        tempDouble.Add(hT.Item1);
                        tempNames.Add(hT.Item2.Item1);
                    }

                    dataByHT.Add(Tuple.Create(tempDouble, Tuple.Create(item.Item2.Item2, tempNames)));
                }

            }

            double[,] AllMap = new double[precipitationPhases.Count, HeatTreatmentList.Count];

            for (int n1 = 0; n1 < HeatTreatmentList.Count; n1++)
            {
                for (int n2 = 0; n2 < precipitationPhases.Count; n2++)
                {
                    Tuple<List<double>, Tuple<string, List<string>>>? mainItem = dataByHT.Find(e => e.Item2.Item1.CompareTo(HeatTreatmentList[n1]) == 0);
                    if (mainItem == null) continue;

                    int IndexPhase = mainItem.Item2.Item2.IndexOf(precipitationPhases[n2]);
                    if (IndexPhase == -1) continue;
                    AllMap[n2, n1] = mainItem.Item1[IndexPhase]*1e+9;
                }

            }

            CompT.refresh(AllMap);
        }
    }
}
