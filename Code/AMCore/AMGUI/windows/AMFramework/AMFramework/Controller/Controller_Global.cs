using AMFramework.AMSystem;
using AMFramework.Core;
using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Controller
{
    public class Controller_Global : ControllerAbstract
    {

        /// <summary>
        /// Access to main window, used mainly for creating popups and interacting
        /// with the main window controller.
        /// </summary>
        public static IMainWindow? MainControl { get; set; }

        /// <summary>
        /// Communication channel to core implementation, uses lua scripting
        /// </summary>
        public static Core.IAMCore_Comm ApiHandle { get; set; } = new AMCore_Empty();

        /// <summary>
        /// User preferences top level
        /// </summary>
        public static UserPreferences UserPreferences { get; set; } = UserPreferences.load();

        /// <summary>
        /// AMcore configurations
        /// </summary>
        public static Controller.Controller_Config? Configuration { get; set; }
    }
}
