using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using AMFramework_Lib.Model;
using System.Windows;
using System.Windows.Input;

namespace AMFramework.Controller
{
    /// <summary>
    /// Active phases configuration data controller
    /// </summary>
    public class Controller_ActivePhasesConfiguration : ControllerAbstract
    {
        #region Field
        /// <summary>
        /// Configuration for active phases
        /// </summary>
        private ModelController<Model_ActivePhasesConfiguration>? _activePhasesConfiguration;

        /// <summary>
        /// Flag, returns true if active phases are being searched
        /// </summary>
        private bool _Searching_Active_Phases = false;


        private ICommand _Find_Active_Phases;

        #endregion
        
        #region Constructor

        public Controller_ActivePhasesConfiguration(ref IAMCore_Comm comm, int IDProject) : base(comm)
        {
            //_ProjectController = projectController;
            var tRef = ModelController<Model_ActivePhasesConfiguration>.LoadIDProject(ref _comm, IDProject);
            if (tRef.Count > 0) _activePhasesConfiguration = tRef[0];
        }

        public Controller_ActivePhasesConfiguration(ref IAMCore_Comm comm, ModelController<Model_ActivePhasesConfiguration> activePhaseConfig) : base(comm)
        {
            //_ProjectController = projectController;
            _activePhasesConfiguration = activePhaseConfig;
        }



        #endregion

        #region Properties

        /// <summary>
        /// get/set Configuration for obtaining the active phases
        /// </summary>
        public ModelController<Model_ActivePhasesConfiguration>? ActivePhasesConfiguration
        {
            get { return _activePhasesConfiguration; }
            set
            {
                _activePhasesConfiguration = value;
                OnPropertyChanged(nameof(ActivePhasesConfiguration));
            }
        }

        /// <summary>
        /// get/set flag for reporting working status
        /// </summary>
        public bool Searching_Active_Phases
        {
            get { return _Searching_Active_Phases; }
            set
            {
                _Searching_Active_Phases = value;
                OnPropertyChanged(nameof(Searching_Active_Phases));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Controller_Global.MainControl?.Show_loading(_Searching_Active_Phases);
                });
            }
        }

        #endregion

        #region Commands
        #region FindActivePhases
        /// <summary>
        /// Finds active phases using its current configuration
        /// </summary>
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

        /// <summary>
        /// Action to execute for finding the active phases
        /// </summary>
        private void Find_Active_Phases_Action()
        {
            if (Searching_Active_Phases) return;
            ActivePhasesConfiguration?.SaveAction?.DoAction();
            Controller_Global.MainControl?.Show_loading(true);
            Controller_Global.MainControl?.Set_Core_Output("Calculating active phases");

            Searching_Active_Phases = true;
            System.Threading.Thread TH01 = new(Method_Find_Active_Phases);
            TH01.Start();
        }

        /// <summary>
        /// Check if finding phases is allowed
        /// </summary>
        /// <returns></returns>
        private bool Can_Find_Active_Phases()
        {
            return true;
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Find active phases handle Async
        /// </summary>
        private void Method_Find_Active_Phases()
        {
            if (_activePhasesConfiguration == null) return;
            string outy = Controller_Global.ApiHandle.run_lua_command("get_active_phases", _activePhasesConfiguration.ModelObject.IDProject.ToString());
            Controller_Global.MainControl?.Set_Core_Output(outy);
            Searching_Active_Phases = false;
        }
        #endregion

        #region Flags

        #endregion

    }
}
