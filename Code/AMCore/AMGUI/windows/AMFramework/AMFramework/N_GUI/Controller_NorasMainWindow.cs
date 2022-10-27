using AMFramework.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.N_GUI
{
    internal class Controller_NorasMainWindow : Controller.ControllerAbstract
    {
        // This is the constructor
        public Controller_NorasMainWindow() 
        {
            // Load Core communication and user preferences
            AMSystem.UserPreferences? uPref = AMSystem.UserPreferences.load();
            Controller_Global.Configuration ??= new(Controller_Global.UserPreferences.IAM_API_PATH);

            // TODO: Add static page


        }

        /// <summary>
        /// This is your page content used in Main window
        /// </summary>
        private object? _pageContent;
        
        /// <summary>
        /// Public property with getter and setter
        /// </summary>
        public object? PageContent 
        {
            get { return _pageContent; }
            set 
            { 
                _pageContent = value;

                // If this is not set, when the content of the page changes
                // the view does not update!

                OnPropertyChanged(nameof(PageContent));
            }
        }

        // TODO: Add commands



    }
}
