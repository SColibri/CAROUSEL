using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Model;
using AMFramework_Lib.Core;
using AMFramework_Lib.Controller;

namespace AMFramework.Controller
{
    public class Controller_ActivePhasesElementComposition : ControllerAbstract
    {
        #region Socket
        private IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _ProjectController;
        public Controller_ActivePhasesElementComposition(ref IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _ProjectController = projectController;
        }
        #endregion

        #region Composition
        private List<Model_ActivePhasesElementComposition> _composition = new();
        public List<Model_ActivePhasesElementComposition> Composition { get { return _composition; } }

        public void refresh() 
        { 
            // Todo load all components and values
        }
        #endregion

    }
}
