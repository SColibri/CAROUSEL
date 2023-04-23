using AMFramework.Components.ScriptingEditor;
using AMFramework_Lib.Controller;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_Scripting : ControllerAbstract
    {
        #region Fields
        private ObservableCollection<Scripting_ViewModel> _openScripts;
        private ICommand? _newScriptCommand;
        private ICommand? _saveScriptCommand;
        private ICommand? _showActiveScriptsCommand;
        private bool _showActiveScripts = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Controller_Scripting()
        {
            _openScripts = new();
        }
        #endregion


        #region Properties
        
        /// <summary>
        /// Available scripts that are open
        /// </summary>
        public ObservableCollection<Scripting_ViewModel> OpenScripts { get { return _openScripts; } }

        /// <summary>
        /// Get set 
        /// </summary>
        public bool ShowActiveScriptsList
        {
            get => _showActiveScripts;
            set 
            { 
                _showActiveScripts = value;
                OnPropertyChanged(nameof(ShowActiveScriptsList));
            }
        }

        #endregion

        #region Methods
        public void Close_Script(Scripting_ViewModel viewObject)
        {
            OpenScripts.Remove(viewObject);
        }

        public Scripting_ViewModel Create_NewScript()
        {
            OpenScripts.Add(new Scripting_ViewModel());
            return OpenScripts.Last();
        }
        #endregion

        #region Commands
        #region OpenScript

        private ICommand? _openScriptCommand;
        /// <summary>
        /// Command for opening a ne script file
        /// close command
        /// </summary>
        public ICommand? OpenScriptCommand
        {
            get
            {
                return _openScriptCommand ??= new RelayCommand(param => OpenScriptCommand_Action(param), param => OpenScriptCommand_Check());
            }
        }

        private void OpenScriptCommand_Action(object? cMW)
        {
            if (cMW == null) throw new Exception("OpenScript command: missing parameter");
            if (cMW is not Controller_Tabs tabController) throw new Exception("OpenScript Command: parameter has to be type of controller_tab");

            // create open file dialog
            System.Windows.Forms.OpenFileDialog ofd = new()
            {
                Filter = " lua script | *.lua",
                InitialDirectory = $"{Controller_Global.Configuration?.Working_Directory}\\Scripts",
                Multiselect = false
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
	            if (File.Exists(ofd.FileName))
	            {
		            // create view
		            Scripting_ViewModel scriptViewModel = Create_NewScript();
		            scriptViewModel.Load(ofd.FileName);

		            // create tab and add scripting view
		            tabController.Create_Tab(scriptViewModel.ScriptingEditor, scriptViewModel,
			            Path.GetFileName(scriptViewModel.Filename));

		            if (!Controller_Global.UserPreferences.RecentFiles.Contains(ofd.FileName))
		            {
			            Controller_Global.UserPreferences.RecentFiles.Add(ofd.FileName);
			            Controller_Global.UserPreferences.save();
					}

	            }
	            else
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?", "Script file", System.Windows.Forms.MessageBoxButtons.OK);
                }
            }
        }

        private bool OpenScriptCommand_Check()
        {
            return true;
        }
        #endregion
        #region NewScript

        
        /// <summary>
        /// Command for opening a ne script file
        /// close command
        /// </summary>
        public ICommand? NewScriptCommand
        {
            get
            {
                _newScriptCommand ??= new RelayCommand(param => NewScriptCommand_Action(param), param => NewScriptCommand_Check());
                return _newScriptCommand;
            }
        }

        private void NewScriptCommand_Action(object? cMW)
        {
            if (cMW == null) throw new Exception("OpenScript command: missing parameter");
            if (cMW is not Controller_Tabs tabController) throw new Exception("OpenScript Command: parameter has to be type of controller_tab");

            Scripting_ViewModel sScript = Create_NewScript();
            tabController.Create_Tab(sScript.ScriptingEditor, sScript, "New file");
        }

        private bool NewScriptCommand_Check()
        {
            return true;
        }
        #endregion
        #region SaveScriptCommand
        /// <summary>
        /// Command for opening a ne script file
        /// close command
        /// </summary>
        public ICommand? SaveScriptCommand
        {
            get
            {
                return _saveScriptCommand ??= new RelayCommand(param => SaveScriptCommand_Action(param), param => SaveScriptCommand_Check());
            }
        }

        private void SaveScriptCommand_Action(object? scriptingViewModel)
        {
            if (scriptingViewModel == null) return;
            if (scriptingViewModel is not Scripting_ViewModel sView) return;

            // Save and notify the user about the save status
            string title = "Scripting";
            string content = "Error saving"; // Worst case
            if (sView.Save())
            {
                content = "Your script was saved";
                SavedFileEvent?.Invoke(sView, EventArgs.Empty);
            }

            // notification
            Controller_Global.MainControl?.Show_Notification(title, content,
                                                            (int)FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle,
                                                            null, null, null);
        }

        private bool SaveScriptCommand_Check()
        {
            return true;
        }

        #endregion
        #region NewScript


        /// <summary>
        /// Command for opening a ne script file
        /// close command
        /// </summary>
        public ICommand? ShowActiveScriptsCommand
        {
            get
            {
                _showActiveScriptsCommand ??= new RelayCommand(param => ShowActiveScriptsAction(), param => ShowActiveScriptsCheck());
                return _showActiveScriptsCommand;
            }
        }

        /// <summary>
        /// Enable/Disable showing active script list
        /// </summary>
        private void ShowActiveScriptsAction()
        {
            ShowActiveScriptsList = !_showActiveScripts;
        }

        /// <summary>
        /// Check if showing active scripts is allowed
        /// </summary>
        /// <returns></returns>
        private bool ShowActiveScriptsCheck()
        {
            return true;
        }
        #endregion
        #endregion

        #region Events
        public event EventHandler SavedFileEvent;
        #endregion


    }
}
