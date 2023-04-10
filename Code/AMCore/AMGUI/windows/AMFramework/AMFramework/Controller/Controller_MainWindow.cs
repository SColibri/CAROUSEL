using AMControls.Custom.ProjectTreeView;
using AMFramework.Components.ScriptingEditor;
using AMFramework.Components.Windows;
using AMFramework.Views.Case;
using AMFramework.Views.Precipitation_Kinetics;
using AMFramework.Views.Projects;
using AMFramework_Lib.AMSystem;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;
using AMFramework_Lib.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AMFramework.Controller
{
	/// <summary>
	/// Controller_MainWindow logic used as data context for the Main window control
	/// </summary>
	public class Controller_MainWindow : ControllerAbstract, IMainWindow
	{
		#region Fields
		private bool _apiLoaded = false;

		//------------------------------------------------------------------------------------
		//                                      CONTROLLERS
		//------------------------------------------------------------------------------------
		private readonly Controller_AMCore _AMCore;
		private readonly Controller_Config _configurationController;
		private readonly Controller_Plot _plotController;
		private readonly Controller_Project _projectController;
		private readonly Controller_Tabs _dataContextTabs;
		private readonly Controller_Scripting _dataContextScripting;
		#endregion

		#region Properties
		/// <summary>
		/// Get/Set API loaded. Returns true if an API library has been loaded.
		/// </summary>
		public bool APILoaded
		{
			get => _apiLoaded;
			set
			{
				_apiLoaded = value;
				OnPropertyChanged(nameof(APILoaded));
			}
		}

		/// <summary>
		/// Gets user preferences
		/// </summary>
		public UserPreferences Preferences => Controller_Global.UserPreferences;

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public Controller_MainWindow()
		{
			UserPreferences? uPref = UserPreferences.load();

			Controller_Config cConfig = new(Controller_Global.UserPreferences.IAM_API_PATH);
			Controller_Global.Configuration = cConfig.datamodel;

			_configurationController = cConfig;

			_comm = Controller.Controller_Config.ApiHandle;

			AMFramework_startup.Start(ref _comm);

			_AMCore = new Controller_AMCore(_comm);

			_projectController = new(_comm);
			_projectController.Load_projectList();

			_plotController = new(ref _comm, _projectController);
			_AMCore.PropertyChanged += Core_output_changed_Handle;

			NotificationObject = new();

			// Add tab controller used as datacontext
			_dataContextTabs = new();
			_dataContextTabs.TabClosed += ClosedTab_Handle;
			_dataContextScripting = new();
			_dataContextScripting.SavedFileEvent += SavedFileEvent_Handle;

		}
		#endregion

		#region Methods

		#endregion



		private void Core_output_changed_Handle(object sender, PropertyChangedEventArgs e)
		{
			if (e is null) return;
			if (e.PropertyName.CompareTo("CoreOutput") == 0)
			{
				CoreOut = _AMCore.CoreOutput;
			}
		}

		#region getters

		public Controller_Plot get_plot_Controller() { return _plotController; }

		public ref IAMCore_Comm Get_socket()
		{
			return ref _comm;
		}

		#endregion

		#region GUIElements

		#region TreeViewItem
		public TV_TopView_controller TreeViewController { get => _projectController.Controller_xDataTreeView.DTV_Controller; }
		#endregion

		#endregion

		#region Open_Views_Tabs
		public void Show_Project_PlotView(Model_Projects modelObject)
		{
			DataContext_Tabs.Create_Tab(new Views.Project_Map.Project_Map(get_plot_Controller()), new Project_ViewModel(), "Project view");
		}

		public void Show_Project_EditView(Model_Projects modelObject)
		{
			// _projectController.Load_project(modelObject.ID);
			DataContext_Tabs.Create_Tab(new Views.Projects.ProjectView_Data(_projectController), new Project_ViewModel(), "Project");
		}

		public void Show_Case_PlotView(Model_Case modelObject)
		{
			DataContext_Tabs.Create_Tab(new Views.Case.plotViews.Case_SpyderChart(_comm, _projectController), new Case_ViewModel(), "Case plot");
		}

		public void Show_Case_EditWindow(Model_Case modelObject)
		{
			Controller.Controller_Cases tController = _projectController.CaseController;
			tController.Set_SelectedCase(modelObject);

			DataContext_Tabs.Create_Tab(new Views.Case.CaseView_Data(tController), new Case_ViewModel(), "Case item");
		}

		public void Show_HeatTreatment_PlotView(Model_HeatTreatment modelObject)
		{
			get_plot_Controller().HeatModel = modelObject;

			DataContext_Tabs.Create_Tab(new Precipitation_kinetics_plot(get_plot_Controller()), new PrecipitationKinetics_ViewModel(), "Heat treatment");
		}

		public void Show_HeatTreatment_EditWindow(Model_HeatTreatment modelObject)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Core
		private string _coreOut = "";
		/// <summary>
		/// Set/get core output text
		/// </summary>
		public string CoreOut
		{
			get { return _coreOut; }
			set
			{
				_coreOut = value;
				OnPropertyChanged(nameof(CoreOut));
			}
		}

		/// <summary>
		/// Update core output on GUI
		/// </summary>
		/// <param name="outputString"></param>
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
				DataContext = _configurationController // new Controller.Controller_Config(_coreSocket);
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

		/// <summary>
		/// Project content
		/// </summary>
		public Controller_Project ProjectController => _projectController;

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
			if (e?.PropertyName?.CompareTo("IsWorking") == 0)
			{
				OnPropertyChanged(nameof(IsLoading));
			}
		}

		#region Controls
		public Components.Windows.AM_popupWindow popupProject(int ID)
		{
			// set new project object as selected
			_projectController.SelectedProject = new(_comm);

			// Create new view
			Views.Projects.Project_general Pg = new(_projectController);
			Components.Windows.AM_popupWindow Pw = new() { Title = "New project" };
			Pw.ContentPage.Children.Add(Pg);

			// Add window buttons
			Components.Button.AM_button nbutt = new()
			{
				IconName = "Save",
				Margin = new System.Windows.Thickness(3),
				CornerRadius = "20",
				GradientTransition = "DodgerBlue"
			};

			// Add button handles
			nbutt.Command = _projectController.SaveProject;
			nbutt.ClickButton += Pw.AM_Close_Window_Event;

			Pw.add_button(nbutt);
			return Pw;
		}

		public Components.Windows.AM_popupWindow popupProjectList(int ID)
		{
			Views.Projects.Project_list Pg = new(_projectController);
			Components.Windows.AM_popupWindow Pw = new() { Title = "Open" };
			Pw.ContentPage.Children.Add(Pg);

			Components.Button.AM_button nbutt = new()
			{
				IconName = FontAwesome.WPF.FontAwesomeIcon.Upload.ToString(),
				Margin = new System.Windows.Thickness(3),
				CornerRadius = "20",
				GradientTransition = "DodgerBlue"
			};
			nbutt.Command = _projectController.SelectProject;
			nbutt.ClickButton += Pw.AM_Close_Window_Event;

			Pw.add_button(nbutt);
			return Pw;
		}
		#endregion

		#endregion

		#region Popup
		private AM_popupWindow? _popupWindow = new();
		/// <summary>
		/// Popup window
		/// </summary>
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
			PopupVisibility = true;

			PopupWindow = pWindow;
		}

		/// <summary>
		/// Handle that closes the popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Close_popup(object? sender, EventArgs e)
		{
			PopupWindow = null;
			PopupVisibility = false;
		}

		private bool _popupVisibility = false;
		/// <summary>
		/// set/get shows or hides the popup control
		/// </summary>
		public bool PopupVisibility
		{
			get { return _popupVisibility; }
			set
			{
				_popupVisibility = value;

				// When popup is visible we hide the tabcontrol
				// only because Scintilla remains as topmost
				// object
				if (value) DataContext_Tabs.TabControlVisible = false;
				else DataContext_Tabs.TabControlVisible = true;

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
		/// <summary>
		/// Show notification as a small popupbox that appears on the right bottom corner 
		/// </summary>
		/// <param name="Title">Notification title description</param>
		/// <param name="Content">Notification description</param>
		/// <param name="IconType">(int)FontAwesome.WPF.FontAwesomeIcon.InfoCircle</param>
		/// <param name="IconForeground">Struct_Color</param>
		/// <param name="ContentBackground">Struct_Color</param>
		/// <param name="TitleBackground">Struct_Color</param>
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
		/// <summary>
		/// set/get loading flag, if true, shows the loading screen
		/// </summary>
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;

				// Set tabcontrol visibility
				if (_isLoading) DataContext_Tabs.TabControlVisible = false;
				else DataContext_Tabs.TabControlVisible = true;

				OnPropertyChanged(nameof(IsLoading));
			}
		}
		/// <summary>
		/// Shows loading screen
		/// </summary>
		/// <param name="showLoading"></param>
		public void Show_loading(bool showLoading)
		{
			// Using dispatcher for thread safe invoke
			Application.Current.Dispatcher.Invoke(() => { IsLoading = showLoading; });
		}
		#endregion

		#region AdditionalInformation
		// -----------------------------------------------------------
		// Additional information displays information from the
		// scripting editor, such as class definitions or methods.
		// This can be enabled also outside the scripting editor
		// and be used in any place where needed to specify additional
		// information about an object
		// -----------------------------------------------------------

		private string _titleAdditionalInformation = "";
		/// <summary>
		/// Set/get the title description
		/// </summary>
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
		/// <summary>
		/// set/get content for additional information window
		/// </summary>
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
		/// <summary>
		/// set/get flag used to indicate if window is expanded (visible or not)
		/// </summary>
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

		#region Controllers
		/// <summary>
		/// Handles all the tab controls
		/// </summary>
		public Controller_Tabs DataContext_Tabs => _dataContextTabs;


		/// <summary>
		/// Handles all file scripting
		/// </summary>
		public Controller_Scripting DataContext_Scripting => _dataContextScripting;


		/// <summary>
		/// Callback controller
		/// </summary>
		public ControllerCallbacks DataContext_Callbacks => _configurationController.Callbacks;

		/// <summary>
		/// Configuration controller
		/// </summary>
		public Controller_Config DataContext_Config => _configurationController;
		#endregion

		#region Commands
		#region CancelCoreCommand

		private ICommand? _cancelCoreCommand;
		/// <summary>
		/// Command for opening a ne script file
		/// close command
		/// </summary>
		public ICommand? CancelCoreCommand
		{
			get
			{
				return _cancelCoreCommand ??= new RelayCommand(param => CancelCoreCommand_Action(), param => CancelCoreCommand_Check());
			}
		}

		private void CancelCoreCommand_Action()
		{
			Cancel_Script();
		}

		private bool CancelCoreCommand_Check()
		{
			return true;
		}

		/// <summary>
		/// Call cancel operation on core
		/// </summary>
		public void Cancel_Script()
		{
			Controller_Global.MainControl?.Set_Core_Output("Cancelling operation.. ");
			Controller_Global.ApiHandle.run_lua_command("core_cancel_operation ", "");
		}
		#endregion
		#region ClosePopupCommand

		private ICommand? _closePopupCommand;
		/// <summary>
		/// Command closes a popup object
		/// close command
		/// </summary>
		public ICommand? ClosePopupCommand
		{
			get
			{
				return _closePopupCommand ??= new RelayCommand(param => ClosePopupCommand_Action(), param => ClosePopupCommand_Check());
			}
		}

		private void ClosePopupCommand_Action()
		{
			PopupVisibility = false;
		}

		private bool ClosePopupCommand_Check()
		{
			return true;
		}

		#endregion

		#region OpenFile

		private ICommand? _openFileCommand;
		/// <summary>
		/// Command for opening a script file
		/// close command
		/// </summary>
		public ICommand? OpenFileCommand
		{
			get
			{
				return _openFileCommand ??= new RelayCommand(param => OpenFileCommand_Action(param), param => OpenFileCommand_Check());
			}
		}

		private void OpenFileCommand_Action(object? param)
		{
			// for now it just opens a script
			if (param is string path && File.Exists(path))
			{
				if (DataContext_Scripting.OpenScripts.FirstOrDefault(e => e.Filename == path) == null)
				{
					// create view
					Scripting_ViewModel scriptViewModel = DataContext_Scripting.Create_NewScript();
					scriptViewModel.Load(path);

					// create tab and add scripting view
					DataContext_Tabs.Create_Tab(scriptViewModel.ScriptingEditor, scriptViewModel, path);
				}
			}
		}

		private bool OpenFileCommand_Check()
		{
			return true;
		}

		#endregion
		#endregion

		#region Handles
		/// <summary>
		/// After closing the tab we want to notify all objects that need to be notified
		/// for now, this just checks for the scripting controller
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClosedTab_Handle(object? sender, EventArgs e)
		{
			if (sender == null) return;
			if (sender is not TabItem tabObject) return;
			if (tabObject.Tag is not Scripting_ViewModel scriptViewModel) return;

			_dataContextScripting.Close_Script(scriptViewModel);
		}

		/// <summary>
		/// When a scripting file is saved, it updates the filename in the GUI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SavedFileEvent_Handle(object? sender, EventArgs e)
		{
			if (sender == null) return;
			if (sender is not Scripting_ViewModel sViewModel) return;

			TabItem tabby = DataContext_Tabs.TabItems.First(e => e.Tag == sViewModel);
			if (tabby == null) return;

			tabby.Header = sViewModel.Filename;
		}

		#endregion

	}
}
