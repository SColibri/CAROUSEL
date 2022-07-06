using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Case
{
    public class Case_ViewModel : Interfaces.ViewModel_Interface
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

        public Views.Case.Case_contents get_content(Controller.Controller_Plot plotController)
        {
            return new Case_contents(ref plotController);
        }

        public Views.Case.Case_general get_item(Controller.Controller_Cases caseController)
        {
            return new Case_general(ref caseController);
        }
    }
}
