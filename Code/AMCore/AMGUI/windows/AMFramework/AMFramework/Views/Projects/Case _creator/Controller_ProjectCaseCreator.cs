﻿using AMFramework.Controller;
using AMFramework.Model;
using AMFramework.Model.Model_Controllers;
using AMFramework.Views.HeatTreatments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMFramework.Views.Projects.Other
{
    public class Controller_ProjectCaseCreator : Controller.ControllerAbstract
    {
        public Controller_ProjectCaseCreator(ref Core.IAMCore_Comm comm, ControllerM_Project projectController):base(comm) 
        {
            // Set project configuration on to new case template
            _projectController = projectController;
            CaseTemplate = new(_comm);
            CaseTemplate.MCObject.ModelObject.IDProject = _projectController.MCObject.ModelObject.ID;

            UpdateCaseTemplate_Elements();

            PhaseListPage = new Phase.PhaseList_View(comm);
            HeatTreatmentPage = new HeatTreatments.HeatTreatment_View(new Controller_HeatTreatmentView(comm, this.CaseTemplate.MCObject.ModelObject));
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

        #endregion

        #region Methods

        private void UpdateCaseTemplate_Elements() 
        {

            var refElement = _projectController.MCObject.ModelObject.SelectedElements.Find(e => e.ModelObject.ISReferenceElementBool);
            var eObj = ModelController<Model_Element>.LoadAll(ref _comm);
            List<ModelController<Model_ElementComposition >> compTable = new();
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
