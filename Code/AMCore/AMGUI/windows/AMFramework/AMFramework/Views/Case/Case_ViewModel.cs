using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMFramework_Lib.Interfaces;

namespace AMFramework.Views.Case
{
    public class Case_ViewModel : ViewModel_Interface
    {
        #region interface
        public bool close()
        {
            return true;
        }

        public bool save()
        {
            return true;
        }
        #endregion

        public Views.Case.Case_general get_item(Controller.Controller_Cases caseController)
        {
            return new Case_general(ref caseController);
        }
    }
}
