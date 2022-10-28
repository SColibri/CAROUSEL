using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;
using AMFramework_Lib.AMSystem;
using AMFramework_Lib.Structures;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using AMFramework.Views.Projects;
using AMFramework.Views.Case;
using AMFramework.Views.Precipitation_Kinetics;
using AMFramework.Components.Windows;

namespace AMFramework.Controller
{
    public class Controller_MainWindow : ControllerAbstract, IMainWindow
    {
        private readonly MainWindow_ViewModel _AMView;        
        private readonly Controller.Controller_AMCore _AMCore;
        private Controller.Controller_DBS_Projects _DBSProjects;
        private readonly Controller.Controller_Config _Config;
        private Controller.Controller_Plot _Plot;

        private readonly Controller.Controller_Project _Project;

        private IAMCore_Comm _coreSocket = new AMCore_Socket();

        private readonly Views.Projects.Project_contents _viewProjectContents;

        public Controller_MainWindow() 
        {
            UserPreferences? uPref = UserPreferences.load();
            
            Controller_Config cConfig = new(Controller_Global.UserPreferences.IAM_API_PATH);
            Controller_Global.Configuration = cConfig.datamodel;

            _Config = cConfig;

            _coreSocket = Controller.Controller_Config.ApiHandle;

            AMFramework_startup.Start(ref _coreSocket);
            
            _AMCore = new(_coreSocket);
            _AMView = new();

            _DBSProjects = new(_coreSocket);
            _DBSProjects.PropertyChanged += Project_property_changed_handle;

            _Plot = new(ref _coreSocket, _DBSProjects);
            _AMCore.PropertyChanged += Core_output_changed_Handle;

            _viewProjectContents = new(ref _DBSProjects);
            _Project = new(_coreSocket);
            _Project.Load_projectList();
            NotificationObject = new();

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
                OnPropertyChanged(nameof(ShowPopup));
            }
        }

        

        private bool _isComputing = false;
        public bool IsComputing
        {
            get { return _isComputing; }
            set
            {
                _isComputing = value;
                OnPropertyChanged(nameof(IsComputing));
            }
        }

        private bool _tabControl_Visible = true;
        public bool TabControlVisible 
        { 
            get { return _tabControl_Visible; } 
            set 
            { 
                _tabControl_Visible=value;
                OnPropertyChanged(nameof(TabControlVisible));
            }
        }

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
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
                OnPropertyChanged(nameof(TabItems));
            }
        }
        public void Add_Tab_Item(TabItem itemy) 
        {
            List<TabItem> nList = new() { itemy };

            foreach (var item in TabItems)
            {
                nList.Add(item);
            }

            TabItems = nList;
            SelectedTab = itemy;
            OnPropertyChanged(nameof(TabItems));
        }
        public void Remove_tab_Item(TabItem itemy) 
        { 
            TabItems.Remove(itemy);
            OnPropertyChanged(nameof(TabItems));
        }

        public void Remove_ByTagType(Type objType) 
        {
            TabItems.RemoveAll(e => e.Tag.GetType().Equals(objType));
            OnPropertyChanged(nameof(TabItems));
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
            _Project.Load_project(_DBSProjects.SelectedProject.ID);

            Remove_ByTagType(typeof(Project_ViewModel));
            TabItem tabContainer = Create_Tab(new Views.Projects.ProjectView_Data(_Project), new Project_ViewModel(), "Project");
            Add_Tab_Item(tabContainer);
            //Remove_ByTagType(typeof(Project_ViewModel));
            //TabItem tabContainer = Create_Tab(new Views.Projects.Project_contents(ref _DBSProjects), new Project_ViewModel(), "Project");
            //Add_Tab_Item(tabContainer);
        }

        public void Show_Case_PlotView(Model_Case modelObject)
        {
            Remove_ByTagType(typeof(Case_ViewModel));
            Controller.Controller_Cases tController = _DBSProjects.ControllerCases;
            tController.SelectedCaseOLD = modelObject;
            TabItem tabContainer = Create_Tab(new Views.Case.Case_contents(ref _Plot), new Case_ViewModel(), "Case plot");
            Add_Tab_Item(tabContainer);
        }

        public void Show_Case_EditWindow(Model_Case modelObject)
        {
            Remove_ByTagType(typeof(Case_ViewModel));
            Controller.Controller_Cases tController = _DBSProjects.ControllerCases;
            tController.SelectedCaseOLD = modelObject;
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
            TabItem result = new();

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
            Grid grid = new();
            ColumnDefinition CDef_01 = new()
            {
                Width = new GridLength(25)
            };
            ColumnDefinition CDef_02 = new();
            CDef_01.Width = new GridLength(1, GridUnitType.Star);

            grid.ColumnDefinitions.Add(CDef_01);
            grid.ColumnDefinitions.Add(CDef_02);

            Image image = new();
            if (uriImage != null)
            {
                ImageSource imS = new BitmapImage(uriImage);
                image.Source = imS;
            }

            TextBlock textBlock = new()
            {
                FontWeight = FontWeights.DemiBold,
                Text = TabTitle
            };

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
                OnPropertyChanged(nameof(CoreOut));
            } 
        }
        public void Set_Core_Output(string outputString)
        {
            CoreOut = outputString;
        }

        #endregion

        #region Configurations
        public Components.Windows.AM_popupWindow popupConfigurations()
        {
            Views.Config.Configuration Pg = new()
            {
                DataContext = _Config // new Controller.Controller_Config(_coreSocket);
            };

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
        private List<Model_Projects> _projects = new();
        public List<Model_Projects> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }
        #endregion

        private void Project_property_changed_handle(object sender, PropertyChangedEventArgs e) 
        {
            if (e is null) return;
            if (e.PropertyName is null) return;
            if(e?.PropertyName?.CompareTo("IsWorking") == 0) 
            {
                OnPropertyChanged(nameof(IsLoading));
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
            Show_Notification("Project","Project has been created");
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
            Binding myBinding = new("VisibilityProperty")
            {
                Source = _DBSProjects.ProjectVisibility,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            System.Windows.Controls.TabItem Tabby = new();
            Tabby.SetBinding(UIElement.VisibilityProperty, myBinding);
            Tabby.Content = new Views.Projects.Project_contents(ref _DBSProjects);

            OnPropertyChanged(nameof(OpenScripts));
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
        private readonly MainWindow_ViewModel _scriptModel = new();
        public MainWindow_ViewModel ScriptView => _scriptModel;
        public List<RibbonMenuItem> OpenScripts
        {
            get 
            {   
                List<RibbonMenuItem> menu = new();
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
            modelScript?.save();

            ScriptRunning = true;
            TabControlVisible = false;
            System.Threading.Thread TH01 = new(run_script_async);
            TH01.Start();

            System.Threading.Thread TH02 = new(Check_for_core_output_script)
            {
                Priority = System.Threading.ThreadPriority.Lowest
            };
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
            OnPropertyChanged(nameof(OpenScripts));
            return Tabby;
        }

        private bool _scriptRunning = false;
        public bool ScriptRunning
        {
            get { return _scriptRunning; }
            set 
            { 
                _scriptRunning = value;
                OnPropertyChanged(nameof(ScriptRunning));
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
                OnPropertyChanged(nameof(RunningScriptOutput));
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
            if (sender is not TreeView refTreeView) return;
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

        // kinetic is not the same as kinetick xp
        public void Selected_kinetick_precipitation_heatT_Handle(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!sender.GetType().Equals(typeof(Model_HeatTreatment))) return;
            Model_HeatTreatment tempRef = (Model_HeatTreatment)sender;
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
            if (refItem.GetType().Equals(typeof(Model_Case)))
            {
                _DBSProjects.ControllerCases.SelectedCaseOLD = (Model_Case)refItem;
                _treeview_selected_tab = _AMView.get_case_itemTab(_DBSProjects.ControllerCases);
            }
            else if (refItem.GetType().Equals(typeof(Controller_DBS_Projects)))
            {
                _treeview_selected_tab = _AMView.get_project_tab((Controller_DBS_Projects)refItem);
            }
        }






        #endregion
        #endregion

        #region Popup
        private AM_popupWindow? _popupWindow = new();
        public AM_popupWindow? PopupWindow
        {
            get { return _popupWindow; }
            set
            {
                _popupWindow = value;
                OnPropertyChanged(nameof(PopupWindow));
            }
        }

        /// <summary>
        /// Implementation that shows a popup object of type AM_popupwindow
        /// </summary>
        /// <param name="pObject"></param>
        /// <exception cref="Exception"></exception>
        public void Show_Popup(object pObject)
        {
            if (!pObject.GetType().Equals(typeof(AM_popupWindow)))
                throw new Exception("Show_Popup: WPF application, objects have to be of type AMFramework_popupWindows and current type is " + pObject.GetType().Name);

            AM_popupWindow? pWindow = pObject as AM_popupWindow;
            if (pWindow == null) return;

            pWindow.PopupWindowClosed += Close_popup;
            TabControlVisible = false;
            PopupVisibility = true;
            
            PopupWindow = pWindow;
        }

        private void Close_popup(object? sender, EventArgs e) 
        {
            PopupWindow = null;
            PopupVisibility = false;
            TabControlVisible = true;
        }

        private bool _popupVisibility = false;
        public bool PopupVisibility 
        { 
            get { return _popupVisibility; }
            set 
            {
                _popupVisibility = value;
                OnPropertyChanged(nameof(PopupVisibility));
            }
        }

        #endregion

        #region Notification
        private AMControls.WindowObjects.Notify.Notify_corner _notificationObject;
        /// <summary>
        /// Using Notification object we can notify anything to the user as a small comment box
        /// </summary>
        public AMControls.WindowObjects.Notify.Notify_corner NotificationObject 
        { 
            get { return _notificationObject; }
            set 
            {
                _notificationObject = value;
                
                // set default size max/min sizes
                _notificationObject.MaxHeight = 300;
                _notificationObject.MaxWidth = 500;
                _notificationObject.Visibility = Visibility.Collapsed;

                OnPropertyChanged(nameof(NotificationObject));
            }
        }
        public void Show_Notification(string Title, string Content, int IconType = (int)FontAwesome.WPF.FontAwesomeIcon.InfoCircle,
                                      Struct_Color? IconForeground = null, Struct_Color? ContentBackground = null, Struct_Color? TitleBackground = null)
        {
            // Set notification parameters
            NotificationObject.Title = Title;
            NotificationObject.Text = Content;
            NotificationObject.Icon = (FontAwesome.WPF.FontAwesomeIcon)IconType;

            //If Brushes are null, set default values
            if (IconForeground != null) 
            {
                Color nColor = Color.FromArgb((byte)IconForeground.A, (byte)IconForeground.R, (byte)IconForeground.G, (byte)IconForeground.B);
                NotificationObject.IconForeground = new SolidColorBrush(nColor); 
            }
            else NotificationObject.IconForeground = Brushes.White;

            if (ContentBackground != null)
            {
                Color nColor = Color.FromArgb((byte)ContentBackground.A, (byte)ContentBackground.R, (byte)ContentBackground.G, (byte)ContentBackground.B);
                NotificationObject.ContentBackground = new SolidColorBrush(nColor);
            }
            else NotificationObject.ContentBackground = Brushes.Black;

            if (TitleBackground != null)
            {
                Color nColor = Color.FromArgb((byte)TitleBackground.A, (byte)TitleBackground.R, (byte)TitleBackground.G, (byte)TitleBackground.B);
                NotificationObject.TitleBackground = nColor;
            }
            else NotificationObject.TitleBackground = Colors.LightBlue;

            // show notification (Maybe add the time interavl into the interface as parameter)
            NotificationObject.Show(5000);
        }
        #endregion

        #region LoadingInformationDisplay
        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public void Show_loading(bool showLoading)
        {
            IsLoading = showLoading;
        }
        #endregion

        #region AdditionalInformation
        private string _titleAdditionalInformation = "";
        public string TitleAdditionalInformation 
        { 
            get { return _titleAdditionalInformation; }
            set 
            {
                _titleAdditionalInformation = value;
                OnPropertyChanged(nameof(TitleAdditionalInformation));
                AdditionalInformationIsExpanded = true;
            }
        }

        private string _contentAdditionalInformation = "";
        public string ContentAdditionalInformation 
        { 
            get { return _contentAdditionalInformation; }
            set 
            {
                _contentAdditionalInformation = value;
                OnPropertyChanged(nameof(ContentAdditionalInformation));
                AdditionalInformationIsExpanded = true;
            }
        }

        private bool _additionalInformationIsExpanded = false;
        public bool AdditionalInformationIsExpanded 
        { 
            get { return _additionalInformationIsExpanded; }
            set 
            {
                _additionalInformationIsExpanded = value;
                OnPropertyChanged(nameof(AdditionalInformationIsExpanded));
            }
        }
        #endregion

        #region Commands

        #endregion

    }
}
