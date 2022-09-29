﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AMControls.Custom.ProjectTreeView;
using System.Reflection;
using AutocompleteMenuNS;
using System.Windows.Media;
using System.Windows.Threading;
using AMFramework.Components.Button;
using AMFramework.Interfaces;
using AMFramework.Model;

namespace AMFramework.Controller
{
    public class Controller_DBS_Projects : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        public Controller_DBS_Projects(Core.IAMCore_Comm socket)
        {
            _AMCore_Socket = socket;
            controllerCases = new(ref socket, this);
            Controller_Selected_Elements = new(ref socket, this);
            Controller_Elements = new(ref socket, this);
            _ActivePhasesConfigurationController = new(ref socket, this);
            _ActivePhasesElementCompositionController = new(ref socket, this);
            _ActivePhasesController = new(ref socket, this);
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Flags
        public enum TABS
        {
            NONE,
            PROJECT,
            SELECTED_ELEMENTS,
            AVAILABLE_PHASES,
            SINGLE_PIXEL_CASES,
            CASEITEM
        }

        private TABS _selected_tab = TABS.NONE;
        public TABS SelectedTab 
        { 
            get { return _selected_tab; } 
            set 
            {
                _selected_tab = value;
                OnPropertyChanged("SelectedTab");
            }
        }

        private TabItem _tab_view = new();
        public TabItem TabView 
        { 
            get { return _tab_view;}
            set 
            {
                _tab_view = value;
                OnPropertyChanged("TabView");
            }
        }



        private bool _isSelected = false;
        public bool ISselected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectedTab = TABS.PROJECT;

                OnPropertyChanged("ISselected");
            }
        }

        public Visibility ProjectVisibility
        {
            get
            {
                if (ISselected) { return Visibility.Visible; }
                else { return Visibility.Collapsed; }
            }
        }

        private bool _selected_caseID = false;
        public bool SelectedCaseID {
            get { return _selected_caseID; }
            set
            {
                _selected_caseID = value;
                SelectedTab = TABS.CASEITEM;

                OnPropertyChanged("SelectedCaseID");
            }
        }

        private bool _selected_case_window = false;
        public bool SelectedCaseWindow
        {
            get { return _selected_case_window; }
            set
            {
                _selected_case_window = value;
                if (value)
                {
                    ISselected = false;
                    SelectedCaseID = false;
                }

                OnPropertyChanged("SelectedCaseWindow");
            }
        }

        private bool _isWorking = false;
        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged("IsWorking");
            }
        }

        private bool _loading_project = false;
        public bool Loading_project
        {
            get { return _loading_project; }
            set 
            {
                _loading_project = value;
                OnPropertyChanged("Loading_project");
            }
        }

        private int _treeIDCase = -1;
        public int TreeIDCase
        {
            get { return _treeIDCase; }
            set 
            {
                _treeIDCase = value;
            }
        }

        private int _treeIDHeatTreatment = -1;
        public int TreeIDHeatTreatment
        {
            get { return _treeIDHeatTreatment; }
            set
            {
                _treeIDHeatTreatment = value;
            }
        }

        private List<int> _selectionTree = new();
        public List<int> SelectionTree 
        { 
            get { return _selectionTree; }
            set 
            {
                _selectionTree = value;
                OnPropertyChanged("SelectionTree");
            }
        }

        #endregion

        #region Methods

        private List<Model.Model_Projects> _DB_projects = new();
        public List<Model.Model_Projects> DB_projects
        {
            get
            {
                return _DB_projects;
            }
        }

        public string DB_projects_reload()
        {
            string outy = Controller.Controller_Config.ApiHandle.run_lua_command("dataController_csv", "0");
            List<string> rowItems = outy.Split("\n").ToList();
            _DB_projects = new();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 2)
                {
                    Model.Model_Projects model = new Model.Model_Projects();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.Name = columnItems[1];
                    model.APIName = columnItems[2];

                    _DB_projects.Add(model);
                }
            }

            OnPropertyChanged("DB_projects");
            return outy;
        }

        public string DB_projects_create_new(string Name)
        {
            string buildString = "dataController_createProject " + Name;
            string outy = _AMCore_Socket.run_lua_command(buildString, "");
            DB_projects_reload();
            return outy;
        }

        private Model.Model_Projects _selectedProject;

        public Model.Model_Projects SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                SelectProject(_selectedProject.ID);
            }
        }
        public void SelectProject(int ID)
        {
            if (_loading_project) return;

            Loading_project = true;
            System.Threading.Thread TH01 = new System.Threading.Thread(Load_project_async);
            TH01.Priority = System.Threading.ThreadPriority.Highest;
            TH01.Start(ID);
        }

        private void Load_project_async(object ID) 
        {
            if (!ID.GetType().Equals(typeof(int))) 
            {
                Loading_project = false;
                MainWindow.notify.ShowBalloonTip(5000, "Internal error","Error: wrong input type for Load_project_async", System.Windows.Forms.ToolTipIcon.Error);
                return;
            }

            _AMCore_Socket.run_lua_command("project_loadID ", ((int)ID).ToString());
            controllerCases.refresh();
            Controller_Selected_Elements.refresh();
            _ActivePhasesController.Refresh();
            _selectedElements = Controller_Selected_Elements.Elements;

            load_database_available_phases();
            Sort_Cases_BySelectedPhases();
            OnPropertyChanged("SelectedProject");
            OnPropertyChanged("Cases");
            OnPropertyChanged(nameof(SelectedElements));

            if(_treeIDCase > -1 && _treeIDHeatTreatment > -1) 
            {
                Model.Model_Case? CaseID = Cases.Find(e => e.ID == _treeIDCase);
                if(CaseID != null) 
                {
                    Model.Model_HeatTreatment? Htreat = CaseID.HeatTreatments.Find(e => e.ID == _treeIDHeatTreatment);
                    if(Htreat != null) 
                    {
                        Htreat.IsSelected = true;
                    }
                }
                
            }

            Loading_project = false;
            Application.Current.Dispatcher.Invoke(new Action(Refresh_DTV));
        }

        public Model.Model_Projects DataModel(int ID)
        {
            Model.Model_Projects model = new Model.Model_Projects();

            if (ID == -1) return model;
            string outy = _AMCore_Socket.run_lua_command("project_getData ", "");
            fillModel(ref model, outy.Split(",").ToList());

            return model;
        }

        public int save_DataModel(Model.Model_Projects model)
        {
            int result = 0;

            string outy = _AMCore_Socket.run_lua_command("project_new " + model.Name, "");
            if (outy.Contains("Error"))
            {
                MainWindow.notify.ShowBalloonTip(5000, "AMCore Error", outy, System.Windows.Forms.ToolTipIcon.Error);
                return 1;
            }

            model.ID = Convert.ToInt32(outy);
            SelectedProject = DataModel(model.ID);
            MainWindow.notify.ShowBalloonTip(5000, "Project Saved", "Successful", System.Windows.Forms.ToolTipIcon.Info);
            return result;
        }

        private void fillModel(ref Model.Model_Projects model, List<string> dataIn)
        {
            if (dataIn.Count < 3) return;
            model.ID = Convert.ToInt32(dataIn[0]);
            model.Name = dataIn[1];
            model.APIName = dataIn[2];
        }

        private void Update_project() 
        {
            SelectProject(_selectedProject.ID);
        }

        #endregion

        #region Cases
        private Controller.Controller_Cases controllerCases;
        public Controller.Controller_Cases ControllerCases { get { return controllerCases; } }
        public List<Model.Model_Case> Cases
        {
            get
            {
                return controllerCases.Cases;
            }
        }

        private List<List<Model.Model_Case>> _cases_BySelectedPhases = new();
        public List<List<Model.Model_Case>> Cases_BySelectedPhases
        {
            get { return _cases_BySelectedPhases; }
            set
            {
                _cases_BySelectedPhases = value;
                OnPropertyChanged("Cases_BySelectedPhases");
            }
        }

        private void Sort_Cases_BySelectedPhases()
        {
            _cases_BySelectedPhases.Clear();

            if (Cases.Count == 0) return;
            _cases_BySelectedPhases.Add(new());
            _cases_BySelectedPhases[0].Add(Cases[0]);

            foreach (Model.Model_Case caseItem in Cases.Skip(1))
            {
                for (int n1 = 0; n1 < _cases_BySelectedPhases.Count; n1++)
                {
                    if (!_cases_BySelectedPhases[n1][0].SelectedPhases.Except(caseItem.SelectedPhases).Any()) 
                    {
                        _cases_BySelectedPhases[n1].Add(caseItem);
                        break;
                    }

                    if(n1 == _cases_BySelectedPhases.Count - 1) 
                    {
                        _cases_BySelectedPhases.Add(new());
                        _cases_BySelectedPhases[n1+1].Add(caseItem);
                    }
                }
            }
            OnPropertyChanged("Cases_BySelectedPhases");
        }

        public void Case_load_equilibrium_phase_fraction(Model.Model_Case model) 
        {
            controllerCases.update_phaseFractions(model);
        }

        public void Case_clear_phase_fraction_data() 
        {
            //controllerCases.Clear_phase_fractions();
        }

        private List<Model.Model_Phase> _used_Phases_inCases;
        public List<Model.Model_Phase> Used_Phases_inCases
        { get { return _used_Phases_inCases; } }

        public void refresh_used_Phases_inCases() 
        {
            IsWorking = true;

            if (_selectedProject is null) return;
            List<int> CaseList = Cases.Select(c => c.ID).ToList();
            _used_Phases_inCases = Controller.Controller_Phase.get_unique_phases_from_caseList(ref _AMCore_Socket, _selectedProject.ID);

            //IsWorking = false;
            OnPropertyChanged("Used_Phases_inCases");
        }

        #endregion

        #region Phases
        private List<Model.Model_Phase> _available_Phase = new();
        public List<Model.Model_Phase> AvailablePhase { get { return _available_Phase; } }

        public void load_database_available_phases()
        {
            _available_Phase = Controller_Phase.get_available_phases_in_database(ref _AMCore_Socket);
            OnPropertyChanged("AvailablePhase");
        }

        public void get_phase_selection_from_current_case() 
        {
            foreach (Model.Model_Phase item in AvailablePhase)
            {
                item.IsSelected = false;
            }

            if (controllerCases.SelectedCase is null) return;
            foreach (Model.Model_SelectedPhases item in controllerCases.SelectedCase.SelectedPhases)
            {
                Model.Model_Phase? tempFindPhase = AvailablePhase.Find(e => e.ID == item.IDPhase);
                if (tempFindPhase is null) continue;
                tempFindPhase.IsSelected = true;
            }

        }

        public void set_phase_selection_to_current_case()
        {
            if (controllerCases.SelectedCase is null) return;

            controllerCases.SelectedCase.Clear_selectedPhases();
            foreach (Model.Model_Phase item in AvailablePhase)
            {
                if (item.IsSelected == false) continue;
                Model.Model_SelectedPhases tempSelPhase = new();

                tempSelPhase.IDPhase = item.ID;
                tempSelPhase.IDCase = controllerCases.SelectedCase.ID;
                tempSelPhase.PhaseName = item.Name;
                controllerCases.SelectedCase.Add_selectedPhases(tempSelPhase);
            }

        }

        public void set_phase_selection_ifActive() 
        {
            if (_ActivePhasesController.ActivePhases.Count == 0) return;

            foreach (var item in AvailablePhase)
            {
                Model.Model_ActivePhases tempRef = _ActivePhasesController.ActivePhases.Find(e => e.IDPhase == item.ID);
                if (tempRef is null) 
                {
                    item.IsActive = false;
                    continue;
                }
                else
                { 
                    item.IsActive = true;
                }
            }
        }

        #region Commands
        #region Search_phases
        private string _search_phase_text = "";

        public string search_phase_text
        {
            get { return _search_phase_text; }
            set 
            { 
                _search_phase_text = value;

                foreach (Model.Model_Phase item in AvailablePhase)
                { 
                    if(item.Name.ToUpper().Contains(_search_phase_text.ToUpper())) item.IsVisible = true;
                    else item.IsVisible = false;
                }

                OnPropertyChanged("search_phase_text");
                OnPropertyChanged("AvailablePhase");
            }
        }
        #endregion
        #endregion
        #endregion

        #region Elements
        private Controller.Controller_Selected_Elements Controller_Selected_Elements;
        public Controller.Controller_Selected_Elements controller_Selected_Elements
        {
            get { return Controller_Selected_Elements; }
        }

        private Controller.Controller_Element Controller_Elements;

        private List<Model.Model_SelectedElements> _selectedElements = new();
        public List<Model.Model_SelectedElements> SelectedElements
        {
            get { return _selectedElements; }
        }

        private List<Model.Model_Element> _available_Elements = new();
        public List<Model.Model_Element> AvailableElements { get { return _available_Elements; } }

        public void load_database_available_elements() 
        {
            ElementsIsLoading = true;
            _available_Elements = Controller_Elements.get_available_elements_in_database();

            foreach (Model.Model_SelectedElements Elementy in SelectedElements)
            {
                Model.Model_Element SelElement = AvailableElements.Find(e => e.ID == Elementy.IDElement);
                
                if (SelElement == null) continue;
                SelElement.IsSelected = true;

                if (Elementy.ISReferenceElementBool) SelElement.IsReferenceElement = true;
            }

            ElementsIsLoading = false;
            OnPropertyChanged("AvailableElements");
        }

        private bool _elementsIsLoading = false;
        public bool ElementsIsLoading 
        { 
            get { return _elementsIsLoading; } 
            set 
            { 
                _elementsIsLoading = value;
                OnPropertyChanged("_elementsIsLoading");
            } 
        }

        private bool _elementsIsSaving = false;
        public bool ElementsIsSaving
        {
            get { return _elementsIsSaving; }
            set
            {
                _elementsIsSaving = value;
                OnPropertyChanged("ElementsIsSaving");
            }
        }
        public void Save_elementSelection_Handle(object sender, EventArgs e) 
        {
            List<Model.Model_Element> ElementListy = AvailableElements.FindAll(e => e.IsSelected == true);

            if(ElementListy.Count == 0) 
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one element!");
                return;
            }

            // check if user actually changed something before deleting cases
            if (ElementListy.Count == SelectedElements.Count) 
            {
                bool somethingChanged = false;
                foreach (Model.Model_Element Elementy in ElementListy)
                {
                    int Index = SelectedElements.FindIndex(e => e.IDElement == Elementy.ID);
                    if(Index == -1) 
                    {
                        somethingChanged = true;
                        break;
                    }
                }

                if (!somethingChanged) 
                {
                    MainWindow.notify.ShowBalloonTip(5000, "Element selection", "No change has been made", System.Windows.Forms.ToolTipIcon.Warning);
                    return;
                }
            }

            // proceed with selection
            string selectedElements = ElementListy[0].Name;
            foreach (Model.Model_Element Elementy in ElementListy.Skip(1))
            {
                selectedElements += "||" + Elementy.Name;
            }

            string commOut = _AMCore_Socket.run_lua_command("project_selectElements ", selectedElements);
            if (commOut.Contains("Error")) { MainWindow.notify.ShowBalloonTip(5000, "Error project", "Elements could not be selected", System.Windows.Forms.ToolTipIcon.Error); }

            // reference element
            Model.Model_Element refElement = ElementListy.Find(e => e.IsReferenceElement == true);
            if (refElement != null)
            {
                commOut = _AMCore_Socket.run_lua_command("project_setReferenceElement ", refElement.Name);
                if (commOut.Contains("Error")) { MainWindow.notify.ShowBalloonTip(5000, "Error project", "Reference element was not set", System.Windows.Forms.ToolTipIcon.Error); }
            }

            Update_project();
            OnPropertyChanged("SelectedElements");
            ElementsIsSaving = false;
        }
        #endregion

        #region ActivePhases
        private Controller.Controller_ActivePhasesConfiguration _ActivePhasesConfigurationController;
        public Controller.Controller_ActivePhasesConfiguration ActivePhasesConfigurationController 
        { 
            get { return _ActivePhasesConfigurationController; } 
        }

        private Controller.Controller_ActivePhases _ActivePhasesController;
        public Controller.Controller_ActivePhases ActivePhasesController
        {
            get { return _ActivePhasesController; }
        }

        private Controller.Controller_ActivePhasesElementComposition _ActivePhasesElementCompositionController;
        public Controller.Controller_ActivePhasesElementComposition ActivePhasesElementCompositionController
        {
            get { return _ActivePhasesElementCompositionController; }
        }

        public void Refresh_ActivePhases() 
        {
            _ActivePhasesController.Refresh();
        }
        #endregion

        #region Plot

        #endregion

        #region FTR
        private TV_TopView_controller _dtv_Controller = new();
        public TV_TopView_controller DTV_Controller
        {
            get { return _dtv_Controller; }
            set 
            {
                _dtv_Controller = value;
                OnPropertyChanged("DTV_Controller");
            }
        }

        private void Refresh_DTV() 
        {

            _dtv_Controller.Title = SelectedProject.Name;
            _dtv_Controller.ID = SelectedProject.ID;

            List<object> listy = new();
            
            WrapPanel ToolPanel = new();
            ToolPanel.Orientation = Orientation.Horizontal;
            ToolPanel.FlowDirection = FlowDirection.RightToLeft;
            ToolPanel.Margin = new Thickness(3,3,3,3);

            AM_button plotAccess = new Components.Button.AM_button()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = SelectedProject
            };
            plotAccess.ClickButton += OnMouseClick_Plot_Handle;
            ToolPanel.Children.Add(plotAccess);

            AM_button editAccess = new Components.Button.AM_button()
            {
                IconName = "Edit",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = SelectedProject
            };
            editAccess.ClickButton += OnMouseClick_Edit_Handle;
            ToolPanel.Children.Add(editAccess);

            listy.Add(ToolPanel);

            _dtv_Controller.Clear_Items();
            listy.Add(new TV_TopView(dtv_Add_elements()));
            listy.Add(new TV_TopView(dtv_Add_activePhases()));
            listy.Add(new TV_TopView(dtv_Add_singlePixelCases()));
            listy.Add(new TV_TopView(dtv_Add_object()));
            _dtv_Controller.Items = listy;

            OnPropertyChanged("DTV_Controller");
        }

        private void OnMouseClick_Plot_Handle(object? sender, EventArgs e) 
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Projects) return;
            Model_Projects mRef = (Model_Projects)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Project_PlotView(mRef);
        }

        private void OnMouseClick_Edit_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Projects) return;
            Model_Projects mRef = (Model_Projects)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Project_EditView(mRef);
        }

        private TV_TopView_controller dtv_Add_elements() 
        {
            TV_TopView_controller TC_proj = new();
            TC_proj.Title = "Selected Elements";
            TC_proj.IconObject = FontAwesome.WPF.FontAwesomeIcon.Slack;

            WrapPanel sPanel = new();
            sPanel.Orientation = Orientation.Horizontal;

            foreach (var item in SelectedElements)
            {
                sPanel.Children.Add(dtv_ElementFormat(item.ElementName));
            }

            TC_proj.Items.Add(sPanel);

            return TC_proj;
        }

        private TV_TopView_controller dtv_Add_activePhases()
        {
            TV_TopView_controller TC_proj = new();
            TC_proj.Title = "Active phases";
            TC_proj.IconObject = FontAwesome.WPF.FontAwesomeIcon.Clipboard;

            return TC_proj;
        }

        private TV_TopView_controller dtv_Add_singlePixelCases()
        {
            TV_TopView_controller TC_proj = new();
            TC_proj.Title = "Single pixel cases";
            TC_proj.IconObject = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;

            WrapPanel ToolPanel = new();
            ToolPanel.Orientation = Orientation.Horizontal;
            ToolPanel.FlowDirection = FlowDirection.RightToLeft;
            ToolPanel.Margin = new Thickness(3, 3, 3, 3);

            AM_button plotAccess = new Components.Button.AM_button()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = new Model_Case()
            };
            plotAccess.ClickButton += OnMouseClick_CasePixel_View_Handle;
            ToolPanel.Children.Add(plotAccess);
            TC_proj.Items.Add(ToolPanel);

            foreach (var item in Cases)
            {
                TC_proj.Items.Add(new TV_TopView(dtv_Add_CaseSingle(item)));
            }

            return TC_proj;
        }

        private void OnMouseClick_CasePixel_View_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Case) return;
            Model_Case mRef = (Model_Case)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Case_PlotView(mRef);
        }

        private TV_TopView_controller dtv_Add_CaseSingle(Model.Model_Case casey)
        {
            TV_TopView_controller TC_proj = new();
            TC_proj.Title = "Case " + casey.ID;
            TC_proj.IconObject = FontAwesome.WPF.FontAwesomeIcon.EllipsisH;

            WrapPanel ToolPanel = new();
            ToolPanel.Orientation = Orientation.Horizontal;
            ToolPanel.FlowDirection = FlowDirection.RightToLeft;
            ToolPanel.Margin = new Thickness(3, 3, 3, 3);

            AM_button plotAccess = new Components.Button.AM_button()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = casey
            };
            //plotAccess.ClickButton += OnMouseClick_Plot_Handle;
            ToolPanel.Children.Add(plotAccess);

            AM_button editAccess = new Components.Button.AM_button()
            {
                IconName = "Edit",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = casey
            };
            editAccess.ClickButton += OnMouseClick_Case_Edit_Handle;
            ToolPanel.Children.Add(editAccess);
            TC_proj.Items.Add(ToolPanel);

            // Add case composition
            TV_TopView_controller TC_Composition = new();
            TC_Composition.Title = "Element Composition";
            TC_Composition.IconObject = FontAwesome.WPF.FontAwesomeIcon.PuzzlePiece;

            WrapPanel sPanel = new();
            sPanel.Orientation = Orientation.Horizontal;

            foreach (var item in casey.ElementComposition)
            {
                sPanel.Children.Add(dtv_ElementFormat(item.ElementName + " : " + item.Value));
            }
            TC_Composition.Items.Add(sPanel);

            TC_proj.Items.Add(new TV_TopView(TC_Composition));

            // Add Case Precipitationn kinetics
            TC_proj.Items.Add(new TV_TopView(dtv_Add_Precipitation_kinetics(casey)));


            return TC_proj;
        }

        private void OnMouseClick_Case_Edit_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Case) return;
            Model_Case mRef = (Model_Case)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Case_EditWindow(mRef);
        }

        private TV_TopView_controller dtv_Add_Precipitation_kinetics(Model.Model_Case casey) 
        {
            TV_TopView_controller TC_Kinetics = new();
            TC_Kinetics.Title = "Precipitation kinetics";
            TC_Kinetics.IconObject = FontAwesome.WPF.FontAwesomeIcon.SnowflakeOutline;

            // Heat treatments
            TV_TopView_controller TC_HT = new();
            TC_HT.Title = "Heat treatments";
            TC_HT.IconObject = FontAwesome.WPF.FontAwesomeIcon.Thermometer;

            foreach (var item in casey.HeatTreatments)
            {
                TV_TopView_controller TC_HT_Item = new();
                TC_HT_Item.Title = item.ID + " : " + item.Name;
                TC_HT_Item.IconObject = FontAwesome.WPF.FontAwesomeIcon.None;

                WrapPanel ToolPanel = new();
                ToolPanel.Orientation = Orientation.Horizontal;
                ToolPanel.FlowDirection = FlowDirection.RightToLeft;
                ToolPanel.Margin = new Thickness(3, 3, 3, 3);

                AM_button plotAccess = new Components.Button.AM_button()
                {
                    IconName = "AreaChart",
                    Width = 20,
                    Height = 20,
                    GradientColor_2 = "White",
                    ForegroundIcon = "DodgerBlue",
                    GradientTransition = "SteelBlue",
                    Margin = new Thickness(2, 2, 2, 2),
                    CornerRadius = "2",
                    ModelTag = item
                };
                plotAccess.ClickButton += OnMouseClick_HeatTreatment_View_Handle;
                ToolPanel.Children.Add(plotAccess);
                TC_HT_Item.Items.Add(ToolPanel);

                TC_HT.Items.Add(new TV_TopView(TC_HT_Item));
            }

            TC_Kinetics.Items.Add(new TV_TopView(TC_HT));

            // Precipitation phases
            TV_TopView_controller TC_PR = new();
            TC_PR.Title = "Precipitation phases";
            TC_PR.IconObject = FontAwesome.WPF.FontAwesomeIcon.Circle;

            foreach (var item in casey.PrecipitationPhases)
            {
                TV_TopView_controller TC_PR_Item = new();
                TC_PR_Item.Title = item.Name;
                TC_PR_Item.IconObject = FontAwesome.WPF.FontAwesomeIcon.None;

                TC_PR.Items.Add(new TV_TopView(TC_PR_Item));
            }

            TC_Kinetics.Items.Add(new TV_TopView(TC_PR));

            // Precipitation Domain
            TV_TopView_controller TC_DO = new();
            TC_DO.Title = "Precipitation domain";
            TC_DO.IconObject = FontAwesome.WPF.FontAwesomeIcon.DotCircleOutline;

            foreach (var item in casey.PrecipitationDomains)
            {
                TV_TopView_controller TC_DO_Item = new();
                TC_DO_Item.Title = item.Name;
                TC_DO_Item.IconObject = FontAwesome.WPF.FontAwesomeIcon.None;

                TC_DO.Items.Add(new TV_TopView(TC_DO_Item));
            }

            TC_Kinetics.Items.Add(new TV_TopView(TC_DO));

            return TC_Kinetics;
        }

        private void OnMouseClick_HeatTreatment_View_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_HeatTreatment) return;
            Model_HeatTreatment mRef = (Model_HeatTreatment)sRef.ModelTag;

            Controller_Global.MainControl?.Show_HeatTreatment_PlotView(mRef);
        }

        private TV_TopView_controller dtv_Add_object()
        {
            TV_TopView_controller TC_proj = new();
            TC_proj.Title = "Object";
            TC_proj.IconObject = FontAwesome.WPF.FontAwesomeIcon.Cube;

            return TC_proj;
        }

        private Border dtv_ElementFormat(string content) 
        {
            Border Belement = new();
            Belement.Background = new SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
            Belement.BorderBrush = new SolidColorBrush(System.Windows.Media.Colors.Silver);

            TextBlock tBlock = new();
            tBlock.Text = content;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Margin = new Thickness(5, 5, 5, 5);
            Belement.Child = tBlock;

            return Belement;

        }
        #endregion

        #region other_controllers

        #endregion

        #region Handles
        public void Select_project_Handle(object sender, EventArgs e)
        { 
            for(int n1 = 0; n1 < DB_projects.Count(); n1++) 
            {
                if (DB_projects[n1].IsSelected) 
                {
                    SelectedProject = DB_projects[n1];

                    break;
                }
            }
        
        }

        public void selected_treeview_item(object sender, RoutedPropertyChangedEventArgs<object> e) 
        {
   
            TreeView refTreeView = sender as TreeView;
            if (refTreeView.SelectedItem == null) return;

            object refTreeItem = refTreeView.SelectedItem;
            if (refTreeItem == null) return;

            if (selected_treeview_item_tagString(refTreeItem)) return;
            if (selected_treeview_item_tagType(refTreeItem)) return;

        }

        private bool selected_treeview_item_tagString(object refTreeItem) 
        {
            //Check if Tag is of type string
            if (!refTreeItem.GetType().Equals(typeof(TreeViewItem))) return false;

            TreeViewItem refTree = refTreeItem as TreeViewItem;
            if (refTree.Tag == null) return false;
            if (!refTree.Tag.GetType().Equals(typeof(string))) return false;

            if (refTree.Tag.ToString().ToUpper().CompareTo("PROJECT") == 0)
            {
                ISselected = true;
            }
            else if (refTree.Tag.ToString().ToUpper().Contains("SINGLE"))
            {
                SelectedCaseWindow = true;
            }
            else if (refTree.Tag.ToString().ToUpper().Contains("CASE"))
            {
                StackPanel refStack = (StackPanel)refTree.Header;
                Model.Model_Case? model = null;

                for (int n1 = 0; n1 < refStack.Children.Count; n1++)
                {
                    if (refStack.Children[n1].GetType().Equals(typeof(TextBlock)))
                    {
                        if (((TextBlock)refStack.Children[n1]).Tag.GetType().Equals(typeof(Model.Model_Case)))
                        {
                            model = ((Model.Model_Case)((TextBlock)refStack.Children[n1]).Tag);
                        }
                    }
                }

                if (model is null) return false;
                controllerCases.SelectedCase = model;
            }
            else
            {
                ISselected = false;
            }

            return true;
        }

        private bool selected_treeview_item_tagType(object refTreeItem)
        {
            //Check if Tag is of type string
            if (refTreeItem.GetType().Equals(typeof(Model.Model_Case))) 
            {
                controllerCases.SelectedCase = (Model.Model_Case)refTreeItem;
                SelectedCaseID = true;
                return true;
            }

            return false;
        }
        #endregion
    }
}
