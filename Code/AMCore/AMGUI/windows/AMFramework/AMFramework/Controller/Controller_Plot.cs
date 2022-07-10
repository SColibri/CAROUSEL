using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using InteractiveDataDisplay.WPF;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace AMFramework.Controller
{
    public class Controller_Plot : INotifyPropertyChanged
    {

        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _projectController;
        public Controller_Plot(ref Core.IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _projectController = projectController;
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public List<Model.Model_Phase> Used_Phases_inCases { get { return _projectController.Used_Phases_inCases; } }
        public Model.Model_Projects SelectedProject { get { return _projectController.SelectedProject; } }
        public List<Model.Model_Case> Cases { get { return _projectController.Cases; } }

        #region Line_graphs
        private List<LineGraph> _lineGraphs = new List<LineGraph>();
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
            Model.Model_Case? model = _projectController.Cases.Find(e => e.ID == IDCase);
            if (model is null) return;

            // load Data
            _projectController.Case_load_equilibrium_phase_fraction(model);
            List<Model.Model_EquilibriumPhaseFraction> modelList = model.EquilibriumPhaseFractions.FindAll(e => e.IDPhase == IDPhase);
            if (modelList.Count == 0) return;

            //get data points
            Model.Model_SelectedPhases phaseSelected = model.SelectedPhases.Find(e => e.IDCase == IDCase && e.IDPhase == IDPhase) ?? new();
            List<double> tempAxis = modelList.Select(e => e.Temperature).ToList();
            List<double> phaseFraction = modelList.Select(e => e.Value).ToList();

            for (int n1 = 0; n1 < phaseFraction.Count; n1++)
            {
                if (phaseFraction[n1] == 0) continue;
                //phaseFraction[n1] = Math.Abs(Math.Log10(phaseFraction[n1]));
            }

            DataPlot.Add(new(tempAxis, phaseFraction));

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                // create new line plots
                var lg = new LineGraph();
                lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(1 * 10), 0));
                lg.Description = String.Format("Data series {0}", phaseSelected.PhaseName);
                lg.StrokeThickness = 2;

                // Add ID's into registered list and send onUpdate
                LineGraphID.Add(new(IDCase, IDPhase));
                LineGraphs.Add(lg);
                OnPropertyChanged("LineGraphs");
                OnPropertyChanged("DataPlot");
                lg.Plot(tempAxis, phaseFraction);
            });

        }

        public void LineGraph_clear()
        {
            LineGraphID.Clear();
            LineGraphs.Clear();
            DataPlot.Clear();
            _projectController.Case_clear_phase_fraction_data();
            OnPropertyChanged("LineGraphs");
        }

        public void refresh_used_Phases_inCases() 
        {
            _projectController.refresh_used_Phases_inCases();
        }

        #endregion

        #region spyderChart
        public struct SpyderDataStructure
        {
            public int IDCase;
            public List<Model.Model_Phase> Phases;
            public List<List<double>> Values;

            public SpyderDataStructure() 
            {
                IDCase = -1;
                Phases = new List<Model.Model_Phase>();
                Values = new List<List<double>>();
            }
        }

        private List<SpyderDataStructure> _spyderDataPlot = new();
        public List<SpyderDataStructure> SpyderDataPlot { get { return _spyderDataPlot; } }
        public List<int> SpyderGraphID = new();
        public void Add_equilibrium_spyder_phase(int IDCase, double Temperature)
        {
            // check if it is already contained before adding
            if (SpyderGraphID.Any(m => m == IDCase)) return;

            // find model from project controller
            Model.Model_Case? model = _projectController.Cases.Find(e => e.ID == IDCase);
            if (model is null) return;

            // load Data
            _projectController.Case_load_equilibrium_phase_fraction(model);
            double solidificationTemp = model.EquilibriumPhaseFractions.Min(e => e.Temperature);

            // get data points
            SpyderDataStructure dataPlotItem = new();
            dataPlotItem.IDCase = IDCase;
            dataPlotItem.Phases = Used_Phases_inCases;
            foreach (Model.Model_Phase phaseModel in Used_Phases_inCases)
            {
                List<Model.Model_EquilibriumPhaseFraction> modelList = model.EquilibriumPhaseFractions.FindAll(e => e.IDPhase == phaseModel.ID && e.Temperature == solidificationTemp);
                if (modelList.Count == 0) continue;

                Tuple<Model.Model_Phase, List<double>> tempItem = new(phaseModel, new());
                dataPlotItem.Values.Add(modelList.Select(e => e.Value).ToList());
            }
            _spyderDataPlot.Add(dataPlotItem);
        }

        public void Spyderplot_get_data() 
        {
            if (IsLoading) return;
            IsLoading = true;

            System.Threading.Thread TH01 = new(Spyderplot_get_data_async);
            TH01.Start();
        }

        public void Spyderplot_get_data_async() 
        {
            List<Model.Model_Case> selCases = _projectController.Cases.FindAll(e => e.IsSelected == true);
            // List<Model.Model_Phase> selPhases = Used_Phases_inCases.FindAll(e => e.IsSelected == true);

            for (int n1 = 0; n1 < selCases.Count; n1++)
            {
                Add_equilibrium_spyder_phase(selCases[n1].ID, 0);
            }
            IsLoading = false;
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
                OnPropertyChanged("IsLoading");
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
            List<Model.Model_Case> selCases = _projectController.Cases.FindAll(e => e.IsSelected == true);
            List<Model.Model_Phase> selPhases = Used_Phases_inCases.FindAll(e => e.IsSelected == true);

            for (int n1 = 0; n1 < selCases.Count; n1++)
            {
                for (int n2 = 0; n2 < selPhases.Count; n2++)
                {
                    Add_Equilibrium_graph(selCases[n1].ID, selPhases[n2].ID);
                }
            }

            OnPropertyChanged("LineGraphs");
            IsLoading = false;
        }

        private bool Can_Change_API_path()
        {
            return true;
        }
        #endregion



    }
}
