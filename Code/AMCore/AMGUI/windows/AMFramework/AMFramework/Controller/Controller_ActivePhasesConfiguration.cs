using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_ActivePhasesConfiguration : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _ProjectController;
        public Controller_ActivePhasesConfiguration(ref Core.IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _ProjectController = projectController;
        }
        #endregion

        #region ActivePhases

        private Model.Model_ActivePhasesConfiguration _APConfiguration = new();
        public Model.Model_ActivePhasesConfiguration APConfiguration
        { 
            get { return _APConfiguration; }
            set 
            { 
                _APConfiguration = value;
                OnPropertyChanged("APConfiguration");
            }
        }
        public void Refresh() 
        { 
            // TODO: load AP config

        }

        public static Model.Model_ActivePhasesConfiguration Get_model(Core.IAMCore_Comm comm,int IDProject) 
        { 
            Model.Model_ActivePhasesConfiguration model = null;

            string Query = "SELECT ActivePhases_Configuration.* FROM ActivePhases_Configuration WHERE IDProject=" + IDProject;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            if (rowItems.Count == 0) 
            {
                Model.Model_ActivePhasesConfiguration NewModel = new();
                NewModel.IDProject = IDProject;
                
            }

            return model;
        }

        private static void FillModel(List<string> DataRaw) 
        {
            Model.Model_ActivePhasesConfiguration model = new Model.Model_ActivePhasesConfiguration();

        }

        public static void Save(Model.Model_ActivePhasesConfiguration model) 
        { 
            
        }
        #endregion

        #region Commands
        #region FindActivePhases

        private ICommand _Find_Active_Phases;
        public ICommand Find_Active_Phases
        {
            get
            {
                if (_Find_Active_Phases == null)
                {
                    _Find_Active_Phases = new RelayCommand(
                        param => this.Find_Active_Phases_Action(),
                        param => this.Can_Find_Active_Phases()
                    );
                }
                return _Find_Active_Phases;
            }
        }

        private void Find_Active_Phases_Action()
        {
            if(Searching_Active_Phases) return;

            Searching_Active_Phases = true;
            System.Threading.Thread TH01 = new(Method_Find_Active_Phases);
            TH01.Start();
        }

        private bool Can_Find_Active_Phases()
        {
            return true;
        }
        #endregion
        #endregion

        #region Methods
        private void Method_Find_Active_Phases() 
        {
            string outy = _AMCore_Socket.run_lua_command("get_active_phases",_ProjectController.SelectedProject.ID.ToString());
            Searching_Active_Phases = false;
            _ProjectController.Refresh_ActivePhases();
        }
        #endregion

        #region Flags
        private bool _Searching_Active_Phases = false;
        public bool Searching_Active_Phases 
        {
            get { return _Searching_Active_Phases; } 
            set 
            {
                _Searching_Active_Phases = value;
                OnPropertyChanged("Searching_Active_Phases");
            }
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
