using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_Config
    {

        #region Cons_Des
        private Core.AMCore_Socket _AMCore_Socket;
        public Controller_Config(Core.AMCore_Socket socket)
        {
            _AMCore_Socket = socket;
            load_model_data();
        }
        public Controller_Config()
        {

        }

        private void load_model_data() 
        {
            datamodel.API_path = _AMCore_Socket.send_receive("configuration_getAPI_path");
            datamodel.External_API_path = _AMCore_Socket.send_receive("configuration_getExternalAPI_path");
            datamodel.Working_Directory = _AMCore_Socket.send_receive("configuration_get_working_directory");
            datamodel.Thermodynamic_database_path = _AMCore_Socket.send_receive("configuration_get_thermodynamic_database_path");
            datamodel.Physical_database_path = _AMCore_Socket.send_receive("configuration_get_physical_database_path");
            datamodel.Mobility_database_path = _AMCore_Socket.send_receive("configuration_get_mobility_database_path");
        }

        private void save_model_data() 
        {
            bool dataSaved = true;

            if(_AMCore_Socket.send_receive("configuration_setAPI_path " + datamodel.API_path).CompareTo("OK") != 0) dataSaved = false;
            if (_AMCore_Socket.send_receive("configuration_setExternalAPI_path " + datamodel.External_API_path).CompareTo("OK") != 0) dataSaved = false;
            if (_AMCore_Socket.send_receive("configuration_set_working_directory " + datamodel.Working_Directory).CompareTo("OK") != 0) dataSaved = false;
            if (_AMCore_Socket.send_receive("configuration_set_thermodynamic_database_path " + datamodel.Thermodynamic_database_path).CompareTo("OK") != 0) dataSaved = false;
            if (_AMCore_Socket.send_receive("configuration_set_physical_database_path " + datamodel.Physical_database_path).CompareTo("OK") != 0) dataSaved = false;
            if (_AMCore_Socket.send_receive("configuration_set_mobility_database_path " + datamodel.Mobility_database_path).CompareTo("OK") != 0) dataSaved = false;

            if(dataSaved == false) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "Something went wrong, configuration was not saved", "Error", System.Windows.Forms.ToolTipIcon.Error);
                return;
            }
            MainWindow.notify.ShowBalloonTip(5000, "Configuration was saved", "Success", System.Windows.Forms.ToolTipIcon.Info);

        }

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
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "API library|*.dll";
            OFD.Title = "Select folder where external API can be found";
            OFD.CheckFileExists = true;

            if (OFD.ShowDialog() == true)
            {
                datamodel.External_API_path = OFD.FileName;
            }
        }

        private bool Can_Change_externalAPI_path()
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
