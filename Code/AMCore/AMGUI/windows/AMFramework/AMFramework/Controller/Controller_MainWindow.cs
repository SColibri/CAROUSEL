using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using AMFramework.Core;
using AMFramework.Interfaces;
using AMFramework.Model;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using AMFramework.Views.Projects;
using AMFramework.Views.Case;
using AMFramework.Views.Precipitation_Kinetics;
using AMFramework.AMSystem;
using Microsoft.Win32;

namespace AMFramework.Controller
{
    public class Controller_MainWindow : Controller_Abstract, IMainWindow
    {
        private MainWindow_ViewModel _AMView;        
        private Controller.Controller_AMCore _AMCore;
        private Controller.Controller_DBS_Projects _DBSProjects;
        private Controller.Controller_Config _Config;
        private Controller.Controller_Plot _Plot;

        private Core.IAMCore_Comm _coreSocket = new Core.AMCore_Socket();

        private Views.Projects.Project_contents _viewProjectContents;

        public Controller_MainWindow() 
        {
            AMSystem.UserPreferences? uPref = AMSystem.UserPreferences.load();
            
            if (uPref != null) Controller_Global.UserPreferences = uPref;

            if(Controller_Global.UserPreferences.IAM_API_PATH.Length == 0 && 1 == 2) 
            {    
                System.Windows.Forms.OpenFileDialog ofd = new();
                ofd.Filter = "Library dll | *.dll";
                ofd.Title = "please select an IAM_API library";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    Controller_Global.UserPreferences.IAM_API_PATH = ofd.FileName;
                    Controller_Global.UserPreferences.save();
                }
            }
            else 
            {
                _Config = new(Controller_Global.UserPreferences.IAM_API_PATH);
            }

            _Config = new();


            _coreSocket = Controller.Controller_Config.ApiHandle;

            AMSystem.AMFramework_startup.Start(ref _coreSocket);
            
            _AMCore = new(_coreSocket);
            _AMView = new();

            _DBSProjects = new(_coreSocket);
            _DBSProjects.PropertyChanged += Project_property_changed_handle;

            _Plot = new(ref _coreSocket, _DBSProjects);
            _AMCore.PropertyChanged += Core_output_changed_Handle;

            _viewProjectContents = new(ref _DBSProjects);

            reloadProjects();
        }

        private void Core_output_changed_Handle(object sender, PropertyChangedEventArgs e) 
        {
            if (e is null) return;
            if (e.PropertyName.CompareTo("CoreOutput") == 0) 
            {
                CoreOut = _AMCore.CoreOutput;
            }
        }

        #region getters
        public Controller.Controller_DBS_Projects get_project_controller() { return _DBSProjects; }

        public Views.Projects.Project_contents get_project_view_content()
        {
            return _viewProjectContents;
        }

        public Controller.Controller_Plot get_plot_Controller() { return _Plot; }

        public ref IAMCore_Comm Get_socket() 
        {
            return ref _coreSocket;
        }

        #endregion

        #region GUIElements
        #region Flags
        private bool _show_popup = false;
        public bool ShowPopup
        {
            get { return _show_popup; }
            set
            {
                _show_popup = value;
                OnPropertyChanged("ShowPopup");
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private bool _isComputing = false;
        public bool IsComputing
        {
            get { return _isComputing; }
            set
            {
                _isComputing = value;
                OnPropertyChanged("IsComputing");
            }
        }

        private bool _tabControl_Visible = true;
        public bool TabControlVisible 
        { 
            get { return _tabControl_Visible; } 
            set 
            { 
                _tabControl_Visible=value;
                OnPropertyChanged("TabControlVisible");
            }
        }

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged("SelectedTab");
            }
        }

        #endregion

        #region Control_Elements
        #region TabItems
        private List<TabItem> _tabItems = new();
        public List<TabItem> TabItems 
        { 
            get { return _tabItems; } 
            set 
            {
                _tabItems = value;
                OnPropertyChanged("TabItems");
            }
        }
        public void Add_Tab_Item(TabItem itemy) 
        {
            List<TabItem> nList = new();

            nList.Add(itemy);
            foreach (var item in TabItems)
            {
                nList.Add(item);
            }

            TabItems = nList;
            SelectedTab = itemy;
            OnPropertyChanged("TabItems");
        }
        public void Remove_tab_Item(TabItem itemy) 
        { 
            TabItems.Remove(itemy);
            OnPropertyChanged("TabItems");
        }

        public void Remove_ByTagType(Type objType) 
        {
            TabItems.RemoveAll(e => e.Tag.GetType().Equals(objType));
            OnPropertyChanged("TabItems");
        }

        #endregion
        #endregion

        #endregion

        #region IMainWindow
        public void Show_Project_PlotView(Model_Projects modelObject)
        {
            Remove_ByTagType(typeof(Project_ViewModel));
            TabItem tabContainer = Create_Tab(new Views.Project_Map.Project_Map(get_plot_Controller()), new Project_ViewModel(), "Project view");
            Add_Tab_Item(tabContainer);
        }

        public void Show_Project_EditView(Model_Projects modelObject)
        {
            Remove_ByTagType(typeof(Project_ViewModel));
            TabItem tabContainer = Create_Tab(new Views.Projects.Project_contents(ref _DBSProjects), new Project_ViewModel(), "Project");
            Add_Tab_Item(tabContainer);
        }

        public void Show_Case_PlotView(Model_Case modelObject)
        {
            Remove_ByTagType(typeof(Case_ViewModel));
            Controller.Controller_Cases tController = _DBSProjects.ControllerCases;
            tController.SelectedCase = modelObject;
            TabItem tabContainer = Create_Tab(new Views.Case.Case_contents(ref _Plot), new Case_ViewModel(), "Case plot");
            Add_Tab_Item(tabContainer);
        }

        public void Show_Case_EditWindow(Model_Case modelObject)
        {
            Remove_ByTagType(typeof(Case_ViewModel));
            Controller.Controller_Cases tController = _DBSProjects.ControllerCases;
            tController.SelectedCase = modelObject;
            TabItem tabContainer = Create_Tab(new Views.Case.Case_general(ref tController), new Case_ViewModel(), "Case item");
            Add_Tab_Item(tabContainer);
        }

        public void Show_HeatTreatment_PlotView(Model_HeatTreatment modelObject)
        {
            Remove_ByTagType(typeof(PrecipitationKinetics_ViewModel));
            Controller.Controller_Cases tController = _DBSProjects.ControllerCases;
            get_plot_Controller().HeatModel = modelObject;
            TabItem tabContainer = Create_Tab(new Views.Precipitation_Kinetics.Precipitation_kinetics_plot(get_plot_Controller()), new PrecipitationKinetics_ViewModel(), "Heat treatment");
            Add_Tab_Item(tabContainer);
        }

        public void Show_HeatTreatment_EditWindow(Model_HeatTreatment modelObject)
        {
            Remove_ByTagType(modelObject.GetType());

            throw new NotImplementedException();
        }

        
        private TabItem Create_Tab(object itemView, object modelObject, string tabTitle) 
        {
            TabItem result = new TabItem();

            string headerTitle = tabTitle;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = get_TabHeader(tabTitle, ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            result.Content = itemView;
            result.Tag = modelObject;

            return result;
        }

        public Grid get_TabHeader(string TabTitle, Uri uriImage)
        {
            Grid grid = new Grid();
            ColumnDefinition CDef_01 = new ColumnDefinition();
            CDef_01.Width = new GridLength(25);
            ColumnDefinition CDef_02 = new ColumnDefinition();
            CDef_01.Width = new GridLength(1, GridUnitType.Star);

            grid.ColumnDefinitions.Add(CDef_01);
            grid.ColumnDefinitions.Add(CDef_02);

            Image image = new Image();
            if (uriImage != null)
            {
                ImageSource imS = new BitmapImage(uriImage);
                image.Source = imS;
            }

            TextBlock textBlock = new TextBlock();
            textBlock.FontWeight = FontWeights.DemiBold;
            textBlock.Text = TabTitle;

            Grid.SetColumn(image, 0);
            Grid.SetColumn(textBlock, 0);
            grid.Children.Add(textBlock);
            grid.Children.Add(image);

            return grid;
        }
        #endregion

        #region Core
        private string _coreOut = "";
        public string CoreOut { 
            get { return _coreOut; } 
            set 
            {
                _coreOut = value;
                OnPropertyChanged("CoreOut");
            } 
        }

        #endregion

        #region Configurations
        public Components.Windows.AM_popupWindow popupConfigurations()
        {
            Views.Config.Configuration Pg = new();
            Pg.DataContext = _Config; // new Controller.Controller_Config(_coreSocket);

            Components.Windows.AM_popupWindow Pw = new() { Title = "Configurations" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Save.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += ((Controller.Controller_Config)Pg.DataContext).saveClickHandle;

            Pw.add_button(nbutt);
            return Pw;
        }
        #endregion

        #region Projects

        #region Data
        private List<Model.Model_Projects> _projects = new();
        public List<Model.Model_Projects> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged("Projects");
            }
        }
        #endregion

        private void Project_property_changed_handle(object sender, PropertyChangedEventArgs e) 
        {
            if (e is null) return;
            if (e.PropertyName is null) return;
            if(e?.PropertyName?.CompareTo("IsWorking") == 0) 
            {
                OnPropertyChanged("IsLoading");
            }
        }
        #region Methods
        public void reloadProjects()
        {
            CoreOut = _DBSProjects.DB_projects_reload();
            Projects = _DBSProjects.DB_projects;
        }
        public void createProject(string Name)
        {
            CoreOut = _DBSProjects.DB_projects_create_new(Name);
        }

        #endregion

        #region Controls
        public Components.Windows.AM_popupWindow popupProject(int ID)
        {
            Views.Projects.Project_general Pg = new(_DBSProjects, ID);
            Components.Windows.AM_popupWindow Pw = new() { Title = "New project" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = "Save",
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += Pg.saveClickHandle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

        public Components.Windows.AM_popupWindow popupProjectList(int ID)
        {
            Views.Projects.Project_list Pg = new(_DBSProjects);
            Components.Windows.AM_popupWindow Pw = new() { Title = "Open" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Upload.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += _DBSProjects.Select_project_Handle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

        public System.Windows.Controls.TabItem projectView_Tab()
        {
            Binding myBinding = new Binding("VisibilityProperty");
            myBinding.Source = _DBSProjects.ProjectVisibility;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            System.Windows.Controls.TabItem Tabby = new();
            Tabby.SetBinding(UIElement.VisibilityProperty, myBinding);
            Tabby.Content = new Views.Projects.Project_contents(ref _DBSProjects);

            OnPropertyChanged("OpenScripts");
            return Tabby;
        }
        #endregion

        #region Commands
        #region _new_project

        private ICommand _new_project;
        public ICommand New_project
        {
            get
            {
                if (_new_project == null)
                {
                    _new_project = new RelayCommand(
                        param => this.New_project_controll(),
                        param => this.Can_Change_new_project()
                    );
                }
                return _new_project;
            }
        }

        private void New_project_controll()
        {

        }

        private bool Can_Change_new_project()
        {
            return true;
        }
        #endregion
        #endregion

        #endregion

        #region Scripting
        private MainWindow_ViewModel _scriptModel = new();
        public MainWindow_ViewModel ScriptView => _scriptModel;
        public List<RibbonMenuItem> OpenScripts
        {
            get 
            {   
                List<RibbonMenuItem> menu = new List<RibbonMenuItem>();
                foreach (Components.Scripting.Scripting_ViewModel item in _scriptModel.OpenScripts)
                {
                    RibbonMenuItem itemy = new()
                    {
                        Header = item.Filename,
                        Tag = item.Filename
                    };

                    itemy.Click += run_script;
                    menu.Add(itemy);
                }
                return menu; 
            }
        }

        private void run_script(object sender, EventArgs e) 
        {
            RibbonMenuItem itemy = (RibbonMenuItem)sender;
            CurrentRunningScript = (string)itemy.Tag;

            Components.Scripting.Scripting_ViewModel? modelScript = _scriptModel.OpenScripts.Find(e => e.Filename.CompareTo(CurrentRunningScript) == 0);
            if (modelScript != null) modelScript.save();

            ScriptRunning = true;
            TabControlVisible = false;
            System.Threading.Thread TH01 = new(run_script_async);
            TH01.Start();

            System.Threading.Thread TH02 = new(Check_for_core_output_script);
            TH02.Priority = System.Threading.ThreadPriority.Lowest;
            TH02.Start();
        }

        private void run_script_async() 
        {
            _AMCore.Run_command("run_lua_script " + CurrentRunningScript);
            TabControlVisible = true;
            ScriptRunning = false;
        }

        private void Check_for_core_output_script() 
        { 
            while (_scriptRunning) 
            {
                //RunningScriptOutput = _AMCore.Run_command("core_buffer ");
                System.Threading.Thread.Sleep(100);
                break;
            }
        }

        public System.Windows.Controls.TabItem scriptView_new_lua_script(string filename = "") 
        {
            System.Windows.Controls.TabItem Tabby = ScriptView.get_new_lua_script(filename);
            OnPropertyChanged("OpenScripts");
            return Tabby;
        }

        private bool _scriptRunning = false;
        public bool ScriptRunning
        {
            get { return _scriptRunning; }
            set 
            { 
                _scriptRunning = value;
                OnPropertyChanged("ScriptRunning");
            }
        }

        public string CurrentRunningScript { get; set; } = "Running script";
        
        private string _runningScriptOutput = "";
        public string RunningScriptOutput 
        { 
            get { return _runningScriptOutput; }
            set 
            {
                _runningScriptOutput = value;
                OnPropertyChanged("RunningScriptOutput");
            } 
        }

        public void Cancel_Script() 
        {
            CurrentRunningScript = "Cancelling....";
            _AMCore.Run_command("core_cancel_operation ");
        }
        #endregion

        #region Plotting
        public TabItem get_new_plot(string plotName = "")
        {
            TabItem result = ScriptView.get_new_plot();
            return result;
        }

        #endregion

        #region TreeView
        private TabItem _treeview_selected_tab;

        #region Handles
        /// <summary>
        /// Handles treeview selection in main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Selected_treeview_item_Handle(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //Remove old tab
            if (_treeview_selected_tab != null) Remove_tab_Item(_treeview_selected_tab);
            GC.Collect();

            // Check parameters
            TreeView? refTreeView = sender as TreeView;
            if (refTreeView == null) return;
            if (refTreeView.SelectedItem == null) return;

            // get selected item
            object refTreeItem = refTreeView.SelectedItem;
            if (refTreeItem == null) return;

            // check if object is a treeview or a model
            if (refTreeItem.GetType().Equals(typeof(TreeViewItem)))
            {
                Selected_treeview_item_ByString(refTreeItem as TreeViewItem);
            }
            else
            {
                Selected_treeview_item_ByModel(refTreeItem);
            }

            if (_treeview_selected_tab == null) return;
            if (!TabItems.Contains(_treeview_selected_tab))
                Add_Tab_Item(_treeview_selected_tab);
        }

        public void Selected_kinetick_precipitation_heatT_Handle(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!sender.GetType().Equals(typeof(Model.Model_HeatTreatment))) return;
            Model.Model_HeatTreatment tempRef = (Model.Model_HeatTreatment)sender;
            Controller.Controller_PrecipitateSimulationData.fill_heatTreatment_model(ref _coreSocket, tempRef);

            //Remove old tab
            if (_treeview_selected_tab != null) Remove_tab_Item(_treeview_selected_tab);
            GC.Collect();

            _treeview_selected_tab = _AMView.get_case_itemTab(_DBSProjects.ControllerCases);
        }

        /// <summary>
        /// Adds tab item w.r.t the header text of the treeview item
        /// </summary>
        /// <param name="refItem"></param>
        private void Selected_treeview_item_ByString(TreeViewItem refItem)
        {
            // check if tag has a object type
            if (refItem.Tag == null) return;

            // If tag is of another type than string switch to ByModel function
            if (!refItem.Tag.GetType().Equals(typeof(string))) 
            {
                Selected_treeview_item_ByModel(refItem.Tag);
                return;
            }

            // get tab item from value set
            string selectedHeader = (string)refItem.Tag;
            if (selectedHeader.ToUpper().CompareTo("PROJECT") == 0)
            {
                _treeview_selected_tab = _AMView.get_project_tab(_DBSProjects);
            }
            else if (selectedHeader.ToUpper().Contains("SINGLE"))
            {
                if (get_plot_Controller().SelectedProject == null) return;
                _treeview_selected_tab = _AMView.get_case_list_tab(get_plot_Controller());
            }
            else if (selectedHeader.ToUpper().Contains("CASEITEM"))
            {
                _treeview_selected_tab = _AMView.get_case_itemTab(_DBSProjects.ControllerCases);
            }
        }

        /// <summary>
        /// Adds tab item w.r.t the model
        /// </summary>
        /// <param name="refItem"></param>
        private void Selected_treeview_item_ByModel(object refItem)
        {
            if (refItem.GetType().Equals(typeof(Model.Model_Case)))
            {
                _DBSProjects.ControllerCases.SelectedCase = (Model.Model_Case)refItem;
                _treeview_selected_tab = _AMView.get_case_itemTab(_DBSProjects.ControllerCases);
            }
            else if (refItem.GetType().Equals(typeof(Controller_DBS_Projects)))
            {
                _treeview_selected_tab = _AMView.get_project_tab((Controller_DBS_Projects)refItem);
            }
        }

       

        #endregion
        #endregion


        #region Commands

        #endregion

    }
}
