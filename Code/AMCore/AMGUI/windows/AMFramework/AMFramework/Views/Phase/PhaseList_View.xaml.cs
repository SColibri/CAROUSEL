using AMFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMFramework.Controller;
using AMFramework.Model.Model_Controllers;
using System.Threading;

namespace AMFramework.Views.Phase
{
    /// <summary>
    /// Interaction logic for PhaseList_View.xaml
    /// </summary>
    public partial class PhaseList_View : UserControl
    {
        /// <summary>
        /// Default constructor, loads phases from current database 
        /// </summary>
        /// <param name="comm"></param>
        public PhaseList_View(IAMCore_Comm comm)
        {
            InitializeComponent();

            // Create default datacontext
            var dC = new Controller_Phase(comm);
            DataContext = dC;

            Thread TH01 = new(dC.LoadFromDatabase);
            TH01.Start();
        }


        /// <summary>
        /// Shows phases from CALPHAD database and sets as selected all the phases selected in the case level
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        public PhaseList_View(IAMCore_Comm comm, int IDCase)
        {
            InitializeComponent();

            var dC = new Controller_Phase(comm);
            DataContext = dC;

            Thread TH01 = new(dC.LoadFromDatabase);
            TH01.Start();
        }



    }
}
