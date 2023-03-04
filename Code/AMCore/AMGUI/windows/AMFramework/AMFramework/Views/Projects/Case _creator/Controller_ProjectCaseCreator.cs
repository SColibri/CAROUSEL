using AMFramework.Controller;
using AMFramework.Views.HeatTreatments;
using AMFramework.Views.Phase;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework_Lib.Scripting.LUA;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMFramework.Views.Projects.Other
{
    public class Controller_ProjectCaseCreator : ControllerAbstract
    {
        public Controller_ProjectCaseCreator(ref IAMCore_Comm comm, ControllerM_Project projectController) : base(comm)
        {
            // Set project configuration on to new case template
            _projectController = projectController;
            CaseTemplate = new(_comm);
            CaseTemplate.MCObject.ModelObject.IDProject = _projectController.MCObject.ModelObject.ID;

            UpdateCaseTemplate_Elements();

            PhaseListPage = new Phase.PhaseList_View(comm);
            HeatTreatmentPage = new HeatTreatments.HeatTreatment_View(new Controller_HeatTreatmentView(comm, this.CaseTemplate.MCObject.ModelObject));
            ScriptSectionPrecipitation = new Components.ScriptingEditor.Scripting_editor() { DataContext = new Components.ScriptingEditor.Scripting_ViewModel() };
            ScriptPrecipitationText = "require\"AMFramework.lua\"\n\r";

            CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.StartTemperature = StartTemperature;
            CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.StartTemperature = StartTemperature;
            CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.EndTemperature = EndTemperature;
            CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.EndTemperature = EndTemperature;
            CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.StepSize = StepSize;
            CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.StepSize = StepSize;
            CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.MinLiquidFraction = MinLiquidFraction;
        }


        #region Models
        private ControllerM_Project _projectController;
        public ControllerM_Project ProjectController
        {
            get { return _projectController; }
            set
            {
                _projectController = value;
                OnPropertyChanged(nameof(ProjectController));
            }
        }


        private ControllerM_Case _caseTemplate;
        public ControllerM_Case CaseTemplate
        {
            get { return _caseTemplate; }
            set
            {
                _caseTemplate = value;
                OnPropertyChanged(nameof(CaseTemplate));
            }
        }
        #endregion

        #region Views

        private object? _phaseListPage;
        public object? PhaseListPage
        {
            get { return _phaseListPage; }
            set
            {
                _phaseListPage = value;
                OnPropertyChanged(nameof(PhaseListPage));
            }
        }

        private object? _heatTreatmentPage;
        public object? HeatTreatmentPage
        {
            get { return _heatTreatmentPage; }
            set
            {
                _heatTreatmentPage = value;
                OnPropertyChanged(nameof(HeatTreatmentPage));
            }
        }

        private object? _scriptSectionPrecipitation;
        public object? ScriptSectionPrecipitation
        {
            get { return _scriptSectionPrecipitation; }
            set
            {
                _scriptSectionPrecipitation = value;
                OnPropertyChanged(nameof(ScriptSectionPrecipitation));
            }
        }


        #endregion

        #region Parameters
        private ControllerM_Phase? _selectedMinLiquidFractionPhase;
        public ControllerM_Phase? SelectedMinLiquiedFractionPhase
        {
            get { return _selectedMinLiquidFractionPhase; }
            set
            {
                _selectedMinLiquidFractionPhase = value;
                OnPropertyChanged(nameof(SelectedMinLiquiedFractionPhase));

                if (value == null)
                {
                    CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.DependentPhase = -1;
                }
                else
                {
                    // TODO:Make safe
                    int ID = _selectedMinLiquidFractionPhase.MCObject.ModelObject.ID;
                    CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.DependentPhase = ID;
                }

            }
        }


        #region Solidification_configuration
        private double _startTemperature = 700;
        public double StartTemperature
        {
            get { return _startTemperature; }
            set
            {
                _startTemperature = value;
                OnPropertyChanged(nameof(StartTemperature));

                CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.StartTemperature = StartTemperature;
                CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.StartTemperature = StartTemperature;
            }
        }


        private double _endTemperature = 450;
        public double EndTemperature
        {
            get { return _endTemperature; }
            set
            {
                _endTemperature = value;
                OnPropertyChanged(nameof(EndTemperature));

                CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.EndTemperature = EndTemperature;
                CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.EndTemperature = EndTemperature;

            }
        }


        private double _stepSize = 1;
        public double StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged(nameof(StepSize));


                CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.StepSize = StepSize;
                CaseTemplate.MCObject.ModelObject.EquilibriumConfiguration.ModelObject.StepSize = StepSize;
            }
        }

        private double _minLiquidFraction = 1;
        public double MinLiquidFraction
        {
            get { return _minLiquidFraction; }
            set
            {
                _minLiquidFraction = value;
                OnPropertyChanged(nameof(MinLiquidFraction));

                // TODO: Make safe
                CaseTemplate.MCObject.ModelObject.ScheilConfiguration.ModelObject.MinLiquidFraction = MinLiquidFraction;
            }
        }

        private string _scriptPrecipitationText = "";
        public string ScriptPrecipitationText
        {
            get { return _scriptPrecipitationText; }
            set
            {
                _scriptPrecipitationText = value;

                ((Components.ScriptingEditor.Scripting_editor)ScriptSectionPrecipitation).Scripting.Text = value;
                OnPropertyChanged(nameof(ScriptPrecipitationText));
            }
        }
        #endregion

        #endregion

        #region Commands

        private ICommand _removeMinLiquidSelection;
        public ICommand RemoveMinLiquidSelection
        {
            get
            {
                if (_removeMinLiquidSelection == null)
                {
                    _removeMinLiquidSelection = new RelayCommand(
                        param => this.RemoveMinLiquidSelection_Action(),
                        param => this.RemoveMinLiquidSelection_Check()
                    );
                }
                return _removeMinLiquidSelection;
            }
        }

        private void RemoveMinLiquidSelection_Action()
        {
            SelectedMinLiquiedFractionPhase = null;
        }

        private bool RemoveMinLiquidSelection_Check()
        {
            return true;
        }

        #region PrecipitationDomains
        #region Add_PrecipitationDomain
        private ICommand _addPrecipitationDomain;
        public ICommand AddPrecipitationDomain
        {
            get
            {
                if (_addPrecipitationDomain == null)
                {
                    _addPrecipitationDomain = new RelayCommand(
                        param => this.AddPrecipitationDomain_Acction(),
                        param => this.AddPrecipitationDomain_Check()
                    );
                }
                return _addPrecipitationDomain;
            }
        }

        private void AddPrecipitationDomain_Acction()
        {
            List<ModelController<Model_PrecipitationDomain>> newList = new();

            foreach (var item in CaseTemplate.MCObject.ModelObject.PrecipitationDomains)
            {
                newList.Add(item);
            }
            newList.Add(new(ref _comm));

            CaseTemplate.MCObject.ModelObject.PrecipitationDomains = newList;
        }

        private bool AddPrecipitationDomain_Check()
        {
            return true;
        }

        #endregion

        #region Remove_PrecipitationDomain
        private ICommand _removePrecipitationDomain;
        public ICommand RemovePrecipitationDomain
        {
            get
            {
                if (_removePrecipitationDomain == null)
                {
                    _removePrecipitationDomain = new RelayCommand(
                        param => this.RemovePrecipitationDomain_Acction(param),
                        param => this.RemovePrecipitationDomain_Check()
                    );
                }
                return _removePrecipitationDomain;
            }
        }

        private void RemovePrecipitationDomain_Acction(object? sender)
        {
            if (sender == null) return;
            if (!sender.GetType().Equals(typeof(ModelController<Model_PrecipitationDomain>))) return;

            ModelController<Model_PrecipitationDomain>? refP = sender as ModelController<Model_PrecipitationDomain>; ;
            if (refP == null) return;

            List<ModelController<Model_PrecipitationDomain>> newList = new();

            foreach (var item in CaseTemplate.MCObject.ModelObject.PrecipitationDomains)
            {
                if (item == refP) continue;
                newList.Add(item);
            }

            CaseTemplate.MCObject.ModelObject.PrecipitationDomains = newList;
        }

        private bool RemovePrecipitationDomain_Check()
        {
            return true;
        }

        #endregion

        #endregion

        #region Actions
        private ICommand _createCases;
        public ICommand CreateCases
        {
            get
            {
                if (_createCases == null)
                {
                    _createCases = new RelayCommand(
                        param => this.CreateCases_Action(),
                        param => this.CreateCases_Check()
                    );
                }
                return _createCases;
            }
        }

        private void CreateCases_Action()
        {

            CreateCases_UpdatePhases();
            Scripting_LUA_ProjectCaseCreator pcc = new(_projectController.MCObject.ModelObject, _caseTemplate.MCObject.ModelObject);

            foreach (var item in CaseTemplate.MCObject.ModelObject.SelectedPhases)
            {
                if (CaseTemplate.MCObject.ModelObject.PrecipitationPhases.Find(e => e.ModelObject.IDPhase == item.ModelObject.IDPhase) != null) continue;

                Model_PrecipitationPhase P0Set_Model = new()
                {
                    IDPhase = item.ModelObject.IDPhase,
                    Name = item.ModelObject.PhaseName + "_P0"
                };

                CaseTemplate.MCObject.ModelObject.PrecipitationPhases.Add(new(ref _comm, P0Set_Model));
                Scripting_LUA_Template<Model_PrecipitationPhase> P0Set = new(P0Set_Model);

            }

            string precipitatePhasesSection = "";
            foreach (var item in CaseTemplate.MCObject.ModelObject.PrecipitationPhases)
            {
                Scripting_LUA_Template<Model_PrecipitationPhase> P0Set = new(item.ModelObject);

                precipitatePhasesSection += P0Set.Create_Object();
            }

            ScriptPrecipitationText = precipitatePhasesSection;

            Window nWind = new();
            Components.ScriptingEditor.Scripting_editor sEdit = new() { DataContext= new Components.ScriptingEditor.Scripting_ViewModel() };
            sEdit.Scripting.Text = pcc.Create_Object();
            nWind.Content = sEdit;
            nWind.Show();

            // MessageBox.Show(pcc.ScriptText(),"Hello");
        }

        private bool CreateCases_Check()
        {
            return true;
        }

        private void CreateCases_UpdatePhases()
        {
            if (PhaseListPage == null) return;
            _caseTemplate.MCObject.ModelObject.SelectedPhases.Clear();
            Controller_Phase? cp = ((PhaseList_View)PhaseListPage).DataContext as Controller_Phase;

            if (cp == null) return;
            List<ControllerM_Phase> cmp = cp.Get_Selected();
            foreach (var item in cmp)
            {
                ModelController<Model_SelectedPhases> tempRef = new(ref _comm);
                tempRef.ModelObject.IDPhase = item.MCObject.ModelObject.ID;
                tempRef.ModelObject.PhaseName = item.MCObject.ModelObject.Name;

                _caseTemplate.MCObject.ModelObject.SelectedPhases.Add(tempRef);
            }
        }

        private void CreateCases_UpdateHeatTreatments()
        {

        }
        #endregion

        #endregion

        #region Methods

        private void UpdateCaseTemplate_Elements()
        {

            var refElement = _projectController.MCObject.ModelObject.SelectedElements.Find(e => e.ModelObject.ISReferenceElementBool);
            var eObj = ModelController<Model_Element>.LoadAll(ref _comm);
            List<ModelController<Model_ElementComposition>> compTable = new();
            foreach (var item in _projectController.MCObject.ModelObject.SelectedElements)
            {
                var refEObj = eObj.Find(e => e.ModelObject.ID == item.ModelObject.IDElement);
                if (refEObj == null) continue;

                Model_ElementComposition refComp = new()
                {
                    IDElement = refEObj.ModelObject.ID,
                    ElementName = refEObj.ModelObject.Name
                };

                compTable.Add(new(ref _comm, refComp));

                if (refElement == null) continue;
                if (refEObj.ModelObject.ID == refElement.ModelObject.IDElement) refComp.IsReferenceElement = true;
            }
            CaseTemplate.MCObject.ModelObject.ElementComposition = compTable;
        }


        #endregion


    }
}
