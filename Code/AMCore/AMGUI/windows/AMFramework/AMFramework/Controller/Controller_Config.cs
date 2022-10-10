using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;

namespace AMFramework.Controller
{
    /// <summary>
    /// Controller config points to the core implementation library using the interface IAMCore_Comm
    /// for sending commands. The config model contains paths to databases and to the working directory.
    /// 
    /// Note: The working directory is where the database will get created, thus modifiying the working directory will
    /// also create a new working database. 
    /// </summary>
    public class Controller_Config
    {

        #region Cons_Des
        /// <summary>
        /// 
        /// </summary>
        public Controller_Config()
        {
            load_model_data();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PathToLib">Path to library that implements the IAM_API interface</param>
            public Controller_Config(string PathToLib)
        {
            Controller_Global.ApiHandle = new Core.AMCore_libHandle(PathToLib);
            load_model_data();
        }

        /// <summary>
        /// Loads the model data
        /// </summary>
        private void load_model_data() 
        {
            if (Controller_Global.ApiHandle == null) return;
            datamodel.API_path = Controller_Global.ApiHandle.run_lua_command("configuration_getAPI_path", "");
            datamodel.External_API_path = Controller_Global.ApiHandle.run_lua_command("configuration_getExternalAPI_path", "");
            datamodel.Working_Directory = Controller_Global.ApiHandle.run_lua_command("configuration_get_working_directory", "");
            datamodel.Thermodynamic_database_path = Controller_Global.ApiHandle.run_lua_command("configuration_get_thermodynamic_database_path", "");
            datamodel.Physical_database_path = Controller_Global.ApiHandle.run_lua_command("configuration_get_physical_database_path", "");
            datamodel.Mobility_database_path = Controller_Global.ApiHandle.run_lua_command("configuration_get_mobility_database_path", "");
            datamodel.IsLoaded = true;
        }

        private void save_model_data() 
        {
            if (Controller_Global.ApiHandle == null) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "AM_API not linked!", "Please link the AM_API library", System.Windows.Forms.ToolTipIcon.Info);
                return;
            }
            bool dataSaved = true;

            string std01 = Controller_Global.ApiHandle.run_lua_command("configuration_setAPI_path " + datamodel.API_path, "");

            if (Controller_Global.ApiHandle.run_lua_command("configuration_setAPI_path " + datamodel.API_path,"").CompareTo("OK") != 0) dataSaved = false;
            if (Controller_Global.ApiHandle.run_lua_command("configuration_setExternalAPI_path " + datamodel.External_API_path, "").CompareTo("OK") != 0) dataSaved = false;
            if (Controller_Global.ApiHandle.run_lua_command("configuration_set_working_directory " + datamodel.Working_Directory, "").CompareTo("OK") != 0) dataSaved = false;
            if (Controller_Global.ApiHandle.run_lua_command("configuration_set_thermodynamic_database_path " + datamodel.Thermodynamic_database_path, "").CompareTo("OK") != 0) dataSaved = false;
            if (Controller_Global.ApiHandle.run_lua_command("configuration_set_physical_database_path " + datamodel.Physical_database_path, "").CompareTo("OK") != 0) dataSaved = false;
            if (Controller_Global.ApiHandle.run_lua_command("configuration_set_mobility_database_path " + datamodel.Mobility_database_path, "").CompareTo("OK") != 0) dataSaved = false;

            if(dataSaved == false) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "Something went wrong, configuration was not saved", "Error", System.Windows.Forms.ToolTipIcon.Error);
                return;
            }
            MainWindow.notify.ShowBalloonTip(5000, "Configuration was saved", "Success", System.Windows.Forms.ToolTipIcon.Info);

            if(datamodel.API_path.CompareTo(Controller_Global.UserPreferences.IAM_API_PATH) != 0) 
            {
                Controller_Global.UserPreferences.IAM_API_PATH = datamodel.API_path;
                Controller_Global.UserPreferences.save();
            }
            
        }

        #endregion

        #region getters
        public static Core.IAMCore_Comm? ApiHandle { get { return Controller_Global.ApiHandle; } }
        #endregion

        #region Model

        public Model.Model_configuration datamodel { get; set; } = new Model.Model_configuration();

        #endregion

        #region Logic

        #region APIPath_Framework

        private ICommand _Search_API_Path;
        public ICommand Search_API_Path
        {
            get
            {
                if (_Search_API_Path == null)
                {
                    _Search_API_Path = new RelayCommand(
                        param => this.Search_API_path_controll(),
                        param => this.Can_Change_API_path()
                    );
                }

                return _Search_API_Path;
            }
        }

        private void Search_API_path_controll()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "AM_library|*.dll";
            OFD.Title = "Select the AM_Framework API library";
            OFD.CheckFileExists = true;

            if (OFD.ShowDialog() == true)
            {
                datamodel.IsLoaded = true;
                datamodel.API_path = OFD.FileName;
            }
        }

        private bool Can_Change_API_path()
        {
            return true;
        }
        #endregion

        #region external_APIPath_Framework

        private ICommand _Search_externalAPI_Path;
        public ICommand Search_externalAPI_Path
        {
            get
            {
                if (_Search_externalAPI_Path == null)
                {
                    _Search_externalAPI_Path = new RelayCommand(
                        param => this.Search_externalAPI_path_controll(),
                        param => this.Can_Change_externalAPI_path()
                    );
                }
                return _Search_externalAPI_Path;
            }
        }

        private void Search_externalAPI_path_controll()
        {
            System.Windows.Forms.FolderBrowserDialog OFD = new System.Windows.Forms.FolderBrowserDialog();
            OFD.Description = "external API directory";
            OFD.UseDescriptionForTitle = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                datamodel.IsLoaded = true;
                datamodel.External_API_path = OFD.SelectedPath;
            }
        }

        private bool Can_Change_externalAPI_path()
        {
            return true;
        }
        #endregion

        #region external_Working_directory

        private ICommand _Search_workingDirectory_Path;
        public ICommand Search_workingDirectory_Path
        {
            get
            {
                if (_Search_workingDirectory_Path == null)
                {
                    _Search_workingDirectory_Path = new RelayCommand(
                        param => this.Search_workingDirectory_path_controll(),
                        param => this.Can_Change_externalAPI_path()
                    );
                }
                return _Search_workingDirectory_Path;
            }
        }

        private void Search_workingDirectory_path_controll()
        {
            System.Windows.Forms.FolderBrowserDialog OFD = new System.Windows.Forms.FolderBrowserDialog();
            OFD.Description = "working directory";
            OFD.UseDescriptionForTitle = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                datamodel.Working_Directory = OFD.SelectedPath;
            }
        }

        private bool Can_Change_workingDirectory_path()
        {
            return true;
        }
        #endregion

        #region external_thermodynamic_database

        private ICommand _Search_thermodynamic_database_Path;
        public ICommand Search_thermodynamic_database_Path
        {
            get
            {
                if (_Search_thermodynamic_database_Path == null)
                {
                    _Search_thermodynamic_database_Path = new RelayCommand(
                        param => this.Search_thermodynamic_database_path_controll(),
                        param => this.Can_Change_thermodynamic_database_path()
                    );
                }
                return _Search_thermodynamic_database_Path;
            }
        }

        private void Search_thermodynamic_database_path_controll()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "thermodynamic database|*.tdb";
            OFD.Title = "Select a thermodynamic database";
            OFD.CheckFileExists = true;

            if (OFD.ShowDialog() == true)
            {
                datamodel.Thermodynamic_database_path = OFD.FileName;
            }
        }

        private bool Can_Change_thermodynamic_database_path()
        {
            return true;
        }
        #endregion

        #region external_Physical_database

        private ICommand _search_physical_database_Path;
        public ICommand Search_physical_database_Path
        {
            get
            {
                if (_search_physical_database_Path == null)
                {
                    _search_physical_database_Path = new RelayCommand(
                        param => this.Search_physical_database_path_controll(),
                        param => this.Can_Change_physical_database_path()
                    );
                }
                return _search_physical_database_Path;
            }
        }

        private void Search_physical_database_path_controll()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "physical database|*.pdb";
            OFD.Title = "Select a physical database";
            OFD.CheckFileExists = true;

            if (OFD.ShowDialog() == true)
            {
                datamodel.Physical_database_path = OFD.FileName;
            }
        }

        private bool Can_Change_physical_database_path()
        {
            return true;
        }
        #endregion

        #region external_Mobility_database

        private ICommand _search_Mobility_database_Path;
        public ICommand Search_Mobility_database_Path
        {
            get
            {
                if (_search_Mobility_database_Path == null)
                {
                    _search_Mobility_database_Path = new RelayCommand(
                        param => this.Search_Mobility_database_path_controll(),
                        param => this.Can_Change_Mobility_database_path()
                    );
                }
                return _search_Mobility_database_Path;
            }
        }

        private void Search_Mobility_database_path_controll()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "physical database|*.ddb";
            OFD.Title = "Select a physical database";
            OFD.CheckFileExists = true;

            if (OFD.ShowDialog() == true)
            {
                datamodel.Mobility_database_path = OFD.FileName;
            }
        }

        private bool Can_Change_Mobility_database_path()
        {
            return true;
        }
        #endregion

        #endregion

        #region Handles
        public void saveClickHandle(object sender, EventArgs e) 
        {
            save_model_data();
        }
        #endregion


    }
}
