using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.DataPlot;
using AMFramework.Components.Charting.Interfaces;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AMFramework.Controller
{
    public class Controller_Plot : ControllerAbstract
    {

        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_Project _projectController;
        public Controller_Plot(ref IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            _AMCore_Socket = comm;
            _projectController = projectController;

            if (_projectController.SelectedProject != null)
                SelectedProject = _projectController.SelectedProject.MCObject.ModelObject;
        }

        #endregion

        public ObservableCollection<ModelController<Model_Phase>> Used_Phases_inCases => _projectController.UsedPhases;
        public Model_Projects SelectedProject { get; }

        #region Line_graphs
        private List<LineGraph> _lineGraphs = new();
        public List<LineGraph> LineGraphs { get { return _lineGraphs; } }

        private List<Tuple<List<double>, List<double>>> _dataPlot = new();
        public List<Tuple<List<double>, List<double>>> DataPlot { get { return _dataPlot; } }

        public List<Tuple<int, int>> LineGraphID = new();

        /// <summary>
        /// Create a new equilibrium line plot, specify ID and phase
        /// </summary>
        /// <param name="IDCase"></param>
        /// <param name="IDPhase"></param>
        public void Add_Equilibrium_graph(int IDCase, int IDPhase)
        {
            // check if it is already contained before adding
            if (LineGraphID.Any(m => m.Item1 == IDCase && m.Item2 == IDPhase)) return;

            // find model from project controller
            ModelController<Model_Case>? model = _projectController.CaseController.Cases.Find(e => e.ModelObject.ID == IDCase);
            if (model is null) return;

            // load Data
            //_projectController.Case_load_equilibrium_phase_fraction(model);
            List<ModelController<Model_EquilibriumPhaseFraction>> modelList = model.ModelObject.EquilibriumPhaseFractions.FindAll(e => e.ModelObject.IDPhase == IDPhase);
            if (modelList.Count == 0) return;

            //get data points
            ModelController<Model_SelectedPhases> phaseSelected = model.ModelObject.SelectedPhases.Find(e => e.ModelObject.IDCase == IDCase && e.ModelObject.IDPhase == IDPhase) ?? new(ref _comm);
            List<double> tempAxis = modelList.Select(e => e.ModelObject.Temperature).ToList();
            List<double> phaseFraction = modelList.Select(e => e.ModelObject.Value).ToList();

            for (int n1 = 0; n1 < phaseFraction.Count; n1++)
            {
                if (phaseFraction[n1] == 0) continue;
                //phaseFraction[n1] = Math.Abs(Math.Log10(phaseFraction[n1]));
            }

            DataPlot.Add(new(tempAxis, phaseFraction));

            Application.Current.Dispatcher.Invoke(delegate
            {
                // create new line plots
                var lg = new LineGraph
                {
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 1 * 10, 0)),
                    Description = String.Format("Data series {0}", phaseSelected.ModelObject.PhaseName),
                    StrokeThickness = 2
                };
                lg.ToolTip = lg.Description;

                // Add ID's into registered list and send onUpdate
                LineGraphID.Add(new(IDCase, IDPhase));
                LineGraphs.Add(lg);
                OnPropertyChanged(nameof(LineGraphs));
                OnPropertyChanged(nameof(DataPlot));
                lg.Plot(tempAxis, phaseFraction);
            });

        }

        public void LineGraph_clear()
        {
            LineGraphID.Clear();
            LineGraphs.Clear();
            DataPlot.Clear();
            //_projectController.Case_clear_phase_fraction_data();
            OnPropertyChanged(nameof(LineGraphs));
        }

        public void refresh_used_Phases_inCases()
        {
            //_projectController.refresh_used_Phases_inCases();
        }

        #endregion

        #region Flags
        private bool _is_loading = false;
        public bool IsLoading
        {
            get { return _is_loading; }
            set
            {
                _is_loading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        #endregion

        #region Handles
        private ICommand _plot_linear_phases;
        public ICommand Plot_linear_phases
        {
            get
            {
                if (_plot_linear_phases == null)
                {
                    _plot_linear_phases = new RelayCommand(
                        param => this.Search_API_path_controll(),
                        param => this.Can_Change_API_path()
                    );
                }
                return _plot_linear_phases;
            }
        }

        private void Search_API_path_controll()
        {
            if (IsLoading) return;
            IsLoading = true;

            LineGraph_clear();

            System.Threading.Thread TH01 = new(Plot_linear_phases_async);
            TH01.Start();
        }

        private void Plot_linear_phases_async()
        {
            List<ModelController<Model_Case>> selCases = _projectController.CaseController.Cases.FindAll(e => e.ModelObject.IsSelected == true);
            List<ModelController<Model_Phase>> selPhases = Used_Phases_inCases.ToList().FindAll(e => e.ModelObject.IsSelected == true);

            for (int n1 = 0; n1 < selCases.Count; n1++)
            {
                for (int n2 = 0; n2 < selPhases.Count; n2++)
                {
                    Add_Equilibrium_graph(selCases[n1].ModelObject.ID, selPhases[n2].ModelObject.ID);
                }
            }

            OnPropertyChanged(nameof(LineGraphs));
            IsLoading = false;
        }

        private bool Can_Change_API_path()
        {
            return true;
        }
        #endregion

        private Model_HeatTreatment _heatModel = new();
        public Model_HeatTreatment HeatModel
        {
            get { return _heatModel; }
            set
            {
                _heatModel = value;
                _heatModel.HeatTreatmentProfileOLD.Clear();
                _heatModel.PrecipitationDataOLD.Clear();
                Controller_HeatTreatmentProfile.fill_heatTreatment_model(ref _AMCore_Socket, _heatModel);
                Controller_PrecipitateSimulationData.fill_heatTreatment_model(ref _AMCore_Socket, _heatModel);
            }
        }

        public List<Tuple<double, Tuple<string, string>>> Get_heat_map_phaseFraction_kinetics()
        {
            // This function was only built for test purposes, we should optimize and refactor this part of code after testing
            List<Tuple<double, Tuple<string, string>>> result = new();
            List<double> phaseFractions = new();

            string Query = "SELECT a.ID, a.IDHeatTreatment, a.IDPrecipitationPhase, a.PhaseFraction, PrecipitationPhase.Name, HeatTreatment.Name AS htName FROM (SELECT ID, IDHeatTreatment, IDPrecipitationPhase, PhaseFraction FROM PrecipitateSimulationData ORDER BY IDHeatTreatment, ID DESC) AS a INNER JOIN PrecipitationPhase ON PrecipitationPhase.ID = a.IDPrecipitationPhase INNER JOIN HeatTreatment ON HeatTreatment.ID = a.IDHeatTreatment GROUP BY IDHeatTreatment, IDPrecipitationPhase";
            string RawData = _AMCore_Socket.run_lua_command("database_table_custom_query", Query);

            List<string> RowData = RawData.Split("\n").ToList();
            if (RowData.Count == 0) return result;

            foreach (var row in RowData)
            {
                List<string> cell = row.Split(",").ToList();
                if (cell.Count < 6) continue;

                double phaseFraction = 0;
                if (!double.TryParse(cell[3], out phaseFraction)) continue;

                Tuple<double, Tuple<string, string>> tempItem = Tuple.Create(phaseFraction, Tuple.Create(cell[4], cell[5]));
                result.Add(tempItem);
            }

            return result;
        }

        public List<Tuple<double, Tuple<string, string>>> Get_heat_map_grainSize_kinetics()
        {
            // This function was only built for test purposes, we should optimize and refactor this part of code after testing
            List<Tuple<double, Tuple<string, string>>> result = new();
            List<double> phaseFractions = new();

            string Query = "SELECT a.ID, a.IDHeatTreatment, a.IDPrecipitationPhase, a.MeanRadius, a.PhaseFraction, PrecipitationPhase.Name, HeatTreatment.Name AS htName FROM (SELECT ID, IDHeatTreatment, IDPrecipitationPhase, MeanRadius, PhaseFraction FROM PrecipitateSimulationData ORDER BY IDHeatTreatment, ID DESC) AS a INNER JOIN PrecipitationPhase ON PrecipitationPhase.ID = a.IDPrecipitationPhase INNER JOIN HeatTreatment ON HeatTreatment.ID = a.IDHeatTreatment GROUP BY IDHeatTreatment, IDPrecipitationPhase";
            string RawData = _AMCore_Socket.run_lua_command("database_table_custom_query", Query);

            List<string> RowData = RawData.Split("\n").ToList();
            if (RowData.Count == 0) return result;

            foreach (var row in RowData)
            {
                List<string> cell = row.Split(",").ToList();
                if (cell.Count < 6) continue;

                double phaseFraction = 0;
                if (!double.TryParse(cell[3], out phaseFraction)) continue;

                Tuple<double, Tuple<string, string>> tempItem = Tuple.Create(phaseFraction, Tuple.Create(cell[4], cell[5]));
                result.Add(tempItem);
            }

            return result;
        }


        public List<IDataSeries> Get_HeatMap_GrainSize_vs_PhaseFraction()
        {
            List<IDataSeries> Result = new();

            string Query_T = "SELECT DISTINCT Name FROM PrecipitationPhase;";
            string RawData_T = _AMCore_Socket.run_lua_command("database_table_custom_query", Query_T);

            List<string> RowData = RawData_T.Split("\n").ToList();
            if (RowData.Count == 0) return Result;

            foreach (string row in RowData)
            {
                if (row.Length == 0) continue;

                ScatterBoxSeries sbs = new();
                DataPlot_HeatTreatmentSimulations dplot = new(_AMCore_Socket);
                dplot.X_Data_Option(2);
                dplot.Y_Data_Option(0);
                dplot.Set_where_clause("PrecipitationPhase = \"" + row.Replace(",", "").Trim() + "\"");
                sbs.Label = row.Replace(",", "").Trim();

                sbs.DataPoints = dplot.DataPoints;
                if (sbs.DataPoints.Count == 0) continue;
                Result.Add(sbs);
            }

            return Result;
        }

        public List<IDataPlot> Get_HeatMap_GrainSize_vs_PhaseFraction_ObjectData()
        {
            List<IDataPlot> Result = new();

            string Query_T = "SELECT DISTINCT Name FROM PrecipitationPhase;";
            string RawData_T = _AMCore_Socket.run_lua_command("database_table_custom_query", Query_T);

            List<string> RowData = RawData_T.Split("\n").ToList();
            if (RowData.Count == 0) return Result;

            foreach (string row in RowData)
            {
                if (row.Length == 0) continue;

                IDataPlot dplot = new DataPlot_HeatTreatmentSimulations(_AMCore_Socket);
                dplot.X_Data_Option(2);
                dplot.Y_Data_Option(0);
                ((DataPlot_HeatTreatmentSimulations)dplot).Set_where_clause("PrecipitationPhase = \"" + row.Replace(",", "").Trim() + "\"");
                dplot.SeriesName = row.Replace(",", "").Trim();

                Result.Add(dplot);
            }

            return Result;
        }

        public void Find_DataPoint(int IDProject, int IDCase, int IDHT)
        {
            //_projectController.TreeIDCase = IDCase;
            //_projectController.TreeIDHeatTreatment = IDHT;

            //if (_projectController.SelectedProject == null) 
            //{
            //    _projectController.DB_projects_reload();
            //}

            //foreach (var item in _projectController.DB_projects)
            //{
            //    if (item.ID == IDProject)
            //    {
            //        _projectController.SelectedProject = item;
            //    }
            //}
        }
    }
}
