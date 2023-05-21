using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMControls.Charts.Spyder;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AMFramework.Controller
{
    public class Controller_SpyderPlot : AMFramework_Lib.Controller.ControllerAbstract
    {
        #region Fields
        private Controller_Project _projectController;
        private List<IDataSeries> _dataSeries = new();
        private Dictionary<int, int> _dictPhaseIndex = new(); // <IDPhase, IDIndex in PhaseList>
        private Random _random = new();
        #endregion

        #region Constructor
        public Controller_SpyderPlot(IAMCore_Comm comm, Controller_Project projectController) : base(comm)
        {
            _projectController = projectController;

            // Get list of phases
            if (_projectController.SelectedProject != null)
                PhaseList = ControllerM_Phase.UniquePhasesByIDProject(_comm, _projectController.SelectedProject.MCObject.ModelObject.ID);

            CreatePlot();
        }
        #endregion

        #region Properties
        private List<ControllerM_Phase> _phaseList = new();
        /// <summary>
        /// List of unique phases available in all case elements
        /// </summary>
        public List<ControllerM_Phase> PhaseList
        {
            get => _phaseList;
            set
            {
                _phaseList = value;

                _dictPhaseIndex = new();
                int index = 0;
                foreach (var item in value)
                {
                    _dictPhaseIndex.Add(item.MCObject.ModelObject.ID, index);
                    index++;
                }

                OnPropertyChanged(nameof(PhaseList));
            }
        }

        private bool _updatingFlag = false;
        /// <summary>
        /// Flag used for showing/hiding the loading GUI element
        /// </summary>
        public bool UpdatingFlag
        {
            get => _updatingFlag;
            set
            {
                _updatingFlag = value;
                OnPropertyChanged(nameof(UpdatingFlag));
            }
        }

        #endregion

        #region UI
        private object? _spyderPlot;
        /// <summary>
        /// Spyderplot user interface boxed inside an object type
        /// </summary>
        public object? SpyderPlot
        {
            get => _spyderPlot;
            set
            {
                _spyderPlot = value;
                OnPropertyChanged(nameof(SpyderPlot));
            }
        }
        #endregion

        #region Methods
        public void UpdatePlot()
        {
            if (_projectController.SelectedProject == null) return; //TODO: notify gui

            UpdatingFlag = true;
            _dataSeries.Clear();
            if (_projectController.SelectedProject.MCObject.ModelObject.Cases.Count > 0)
                _projectController.SelectedProject.MCObject.ModelObject.Cases[0].ModelObject.IsSelected = true; ;

            // Start Work
            Thread TH01 = new(LoadData_Async);
            TH01.Priority = ThreadPriority.Normal;
            TH01.Start();


        }

        private void CreatePlot()
        {
            SpyderPlot SpyderMain = new();

            // Add all axis
            foreach (var item in PhaseList)
            {
                IAxes axeObject = new LinearAxe() { Name = item.MCObject.ModelObject.Name, MinValue = 1, MaxValue = 10 };
                SpyderMain.Add_axis(axeObject);
            }

            // Add Series to plot
            foreach (var item in _dataSeries)
            {
                SpyderMain.Add_series(item);
            }

            SpyderPlot = SpyderMain;
        }

        private void LoadData_Async()
        {
            if (_projectController.SelectedProject == null) return;

            List<ModelController<Model_Case>> selectedCases = _projectController.SelectedProject.MCObject.ModelObject.Cases.FindAll(e => e.ModelObject.IsSelected == true);

            // Load all simulation data
            foreach (var item in selectedCases)
            {
                item.ModelObject.ScheilPhaseFractions = ControllerM_Case.LoadData_ScheilSolidificationSimulation(_comm, item);
            }

            List<ModelController<Model_Case>> SelectedL = _projectController.SelectedProject.MCObject.ModelObject.Cases.FindAll(e => e.ModelObject.IsSelected == true);
            foreach (var item in SelectedL)
            {
                // Create new dataseries
                IDataSeries series = new SpyderSeries();
                series.ColorSeries = Color.FromRgb((byte)_random.Next(1, 255), (byte)_random.Next(1, 255), (byte)_random.Next(1, 255));

                // Set to 0 all data series
                foreach (var phaseModel in PhaseList)
                {
                    series.Add_DataPoint(new DataPoint() { X = 0, Label = phaseModel.MCObject.ModelObject.Name, ContextMenu = new DataPoint_ContextMenu_Text() { Title = "No Title" } });
                }

                Add_DataPoints(series, item);

                // Add to list
                _dataSeries.Add(series);
            }

            // Update GUI
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { CreatePlot(); }));
        }

        private void Add_DataPoints(IDataSeries series, ModelController<Model_Case> caseModel)
        {
            double solidificationTemp = caseModel.ModelObject.ScheilPhaseFractions.Min(e => e.ModelObject.Temperature);

            List<ModelController<Model_ScheilPhaseFraction>> scheilPhaseFractions = caseModel.ModelObject.ScheilPhaseFractions.FindAll(e => e.ModelObject.Temperature == solidificationTemp &&
                                                                                                                                 e.ModelObject.IDCase == caseModel.ModelObject.ID).ToList();
            // set data points 
            foreach (var item in scheilPhaseFractions)
            {
                int index;
                if (_dictPhaseIndex.TryGetValue(1, out index))
                    series.DataPoints[index].X = item.ModelObject.Value;
            }
        }

        #endregion

        #region Commands
        #region Save

        private ICommand? _updatePlotCommand;
        public ICommand? UpdatePlotCommand
        {
            get
            {
                if (_updatePlotCommand == null)
                {
                    _updatePlotCommand = new RelayCommand(
                        param => this.UpdatePlot_Action(),
                        param => this.UpdatePlot_Check()
                    );
                }
                return _updatePlotCommand;
            }
        }

        private void UpdatePlot_Action()
        {
            UpdatePlot();
        }

        private bool UpdatePlot_Check()
        {
            return false;
        }
        #endregion
        #endregion
    }
}
