using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using AMFramework_Lib.Model;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;

namespace AMFramework.Controller
{
    public class Controller_ActivePhasesConfiguration : ControllerAbstract
    {
        #region Socket
        private Controller_DBS_Projects _ProjectController;

        [Obsolete("Constructor type is obsolete, we should avoid the usage of the prototype Controller_DBS_project")]
        /// <summary>
        /// Thid function will be deprecated for the new restructured method
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="projectController"></param>
        public Controller_ActivePhasesConfiguration(ref IAMCore_Comm comm, Controller_DBS_Projects projectController) : base(comm)
        {
            _ProjectController = projectController;
        }

        #region New_implementation

        public Controller_ActivePhasesConfiguration(ref IAMCore_Comm comm, int IDProject) : base(comm)
        {
            //_ProjectController = projectController;
            var tRef = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref _comm, IDProject);
            if(tRef.Count > 0) _activePhasesConfiguration = tRef[0];
        }

        public Controller_ActivePhasesConfiguration(ref IAMCore_Comm comm, ModelController<Model_ActivePhasesConfiguration> activePhaseConfig) : base(comm)
        {
            //_ProjectController = projectController;
            _activePhasesConfiguration = activePhaseConfig;
        }

        private ModelController<Model_ActivePhasesConfiguration>? _activePhasesConfiguration;
        public ModelController<Model_ActivePhasesConfiguration>? ActivePhasesConfiguration
        {
            get { return _activePhasesConfiguration; }
            set
            {
                _activePhasesConfiguration = value;
                OnPropertyChanged(nameof(ActivePhasesConfiguration));
            }
        }
        #endregion

        #endregion

        #region ActivePhases

        private Model_ActivePhasesConfiguration _APConfiguration = new();
        public Model_ActivePhasesConfiguration APConfiguration
        { 
            get { return _APConfiguration; }
            set 
            { 
                _APConfiguration = value;
                OnPropertyChanged(nameof(APConfiguration));
            }
        }

        public static Model_ActivePhasesConfiguration Get_model(IAMCore_Comm comm,int IDProject) 
        { 
            Model_ActivePhasesConfiguration model = null;

            string Query = "SELECT ActivePhases_Configuration.* FROM ActivePhases_Configuration WHERE IDProject=" + IDProject;
            string outCommand = comm.run_lua_command("database_table_custom_query", Query);
            List<string> rowItems = outCommand.Split("\n").ToList();

            if (rowItems.Count == 0) 
            {
                Model_ActivePhasesConfiguration NewModel = new()
                {
                    IDProject = IDProject
                };

            }

            return model;
        }

        private static void FillModel(List<string> DataRaw) 
        {
            Model_ActivePhasesConfiguration model = new();

        }

        public static void Save(Model_ActivePhasesConfiguration model) 
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
        [Obsolete("use Method_Find_Active_Phases instead")]
        private void Method_Find_Active_Phases_OLD() 
        {
            string outy = _comm.run_lua_command("get_active_phases",_ProjectController.SelectedProject.ID.ToString());
            Searching_Active_Phases = false;
            _ProjectController.Refresh_ActivePhases();
        }

        private void Method_Find_Active_Phases()
        {
            if (_activePhasesConfiguration == null) return;
            string outy = _comm.run_lua_command("get_active_phases", _activePhasesConfiguration.ModelObject.IDProject.ToString());
            Searching_Active_Phases = false;
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
                OnPropertyChanged(nameof(Searching_Active_Phases));
            }
        }
        #endregion

    }
}
