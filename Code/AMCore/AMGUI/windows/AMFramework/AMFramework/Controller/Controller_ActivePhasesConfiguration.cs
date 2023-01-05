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
using System.Windows;

namespace AMFramework.Controller
{
    public class Controller_ActivePhasesConfiguration : ControllerAbstract
    {
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

        private ModelController<Model_ActivePhasesConfiguration>? _activePhasesConfiguration;
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

        private bool _Searching_Active_Phases = false;
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
            ActivePhasesConfiguration?.SaveAction?.DoAction();
            Controller_Global.MainControl?.Show_loading(true);
            Controller_Global.MainControl?.Set_Core_Output("Calculating active phases");

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
            if (_activePhasesConfiguration == null) return;
            string outy = _comm.run_lua_command("get_active_phases", _activePhasesConfiguration.ModelObject.IDProject.ToString());
            Controller_Global.MainControl?.Set_Core_Output(outy);
            Searching_Active_Phases = false;
            
        }
        #endregion

        #region Flags
        
        #endregion

    }
}
