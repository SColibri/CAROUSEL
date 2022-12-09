using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMFramework_Lib.Interfaces;

namespace AMFramework.Views.Precipitation_Kinetics
{
    internal class PrecipitationKinetics_ViewModel : ViewModel_Interface
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

        public Views.Precipitation_Kinetics.Precipitation_kinetics_plot get_plot(Controller.Controller_Plot plotController)
        {
            return new Precipitation_kinetics_plot(plotController);
        }
    }
}
