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
        public bool Close()
        {
            return true;
        }

        public bool Save()
        {
            return true;
        }
        #endregion
    }
}
