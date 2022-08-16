using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Precipitation_Kinetics
{
    internal class PrecipitationKinetics_ViewModel : Interfaces.ViewModel_Interface
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

        public Views.Precipitation_Kinetics.Precipitation_kinetics_plot get_plot(Controller.Controller_Plot plotController)
        {
            return new Precipitation_kinetics_plot(plotController);
        }
    }
}
