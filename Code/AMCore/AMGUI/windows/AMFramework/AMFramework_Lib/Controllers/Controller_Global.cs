using AMFramework_Lib.AMSystem;
using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Model;

namespace AMFramework_Lib.Controller
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
        // public static Controller.Controller_Config? Configuration { get; set; }
        public static Model_configuration? Configuration { get; set; }
    }
}
