using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private bool _isSelected = false;
        public bool ISselected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                if (value == true)
                {
                    SelectedCaseID = false;
                    _selected_case_window = false;
                }

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

                if (value == true)
                {
                    ISselected = false;
                    SelectedCaseWindow = false;
                }

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
                controllerCases.refresh();
                Controller_Selected_Elements.refresh();

                OnPropertyChanged("SelectedProject");
                OnPropertyChanged("Cases");
                OnPropertyChanged("Elements");
                load_database_available_phases();
            }
        }
        public void SelectProject(int ID)
        {
            string outy = _AMCore_Socket.run_lua_command("project_loadID" + ID.ToString(), "");
            if (outy.CompareTo("OK") != 0) return;
            SelectedProject = DataModel(ID);
            load_database_available_phases();
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

            string csvFormat = model.get_csv();
            string outy = _AMCore_Socket.run_lua_command("dataController_saveProjectData " + csvFormat, "");
            if (outy.Contains("Error"))
            {
                MainWindow.notify.ShowBalloonTip(5000, "AMCore Error", outy, System.Windows.Forms.ToolTipIcon.Error);
                return 1;
            }

            model.ID = Convert.ToInt32(outy);
            DB_projects_reload();
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

        private List<List<Model.Model_Case>> _cases_BySelectedPhases;
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
        private Controller.Controller_Element Controller_Elements;
        public List<Model.Model_SelectedElements> Elements
        {
            get { return Controller_Selected_Elements.Elements; }
        }

        private List<Model.Model_Element> _available_Elements = new();
        public List<Model.Model_Element> AvailableElements { get { return _available_Elements; } }

        public void load_database_available_elements() 
        {
            _available_Elements = Controller_Elements.get_available_elements_in_database();
            OnPropertyChanged("AvailableElements");
        }
        #endregion

        #region Plot

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
            else if (refTree.Tag.ToString().ToUpper().Contains("CASELIST"))
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
