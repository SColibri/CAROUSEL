using AMFramework_Lib.Core;
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
using AMFramework_Lib.Model.Model_Controllers;
using System.Threading;
using AMFramework_Lib.Model;
using System.ComponentModel;

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

            Create_DataContext(comm);
        }


        /// <summary>
        /// Shows phases from CALPHAD database and sets as selected all the phases selected in the case level, deprecate?
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        public PhaseList_View(IAMCore_Comm comm, int IDCase)
        {
            InitializeComponent();

            Create_DataContext(comm);
        }

        /// <summary>
        /// Creates a controller phase object and uses it as the datacontext. To retrieve selection use the Get_selection
        /// Method included in this view or call the controller's function
        /// </summary>
        /// <param name="comm"></param>
        private void Create_DataContext(IAMCore_Comm comm) 
        {
            var dC = new Controller_Phase(comm);
            DataContext = dC;

            Thread TH01 = new(dC.LoadFromDatabase);
            TH01.Start();
        }

        /// <summary>
        /// Returns all selected phases
        /// </summary>
        /// <returns></returns>
        public List<ModelController<Model_Phase>> Get_Selection() 
        {
            var selectedList = ((Controller_Phase)DataContext).Get_Selected();
            List<ModelController<Model_Phase>> result = new();

            // extract the modelcontroller
            foreach (var item in selectedList)
            {
                result.Add(item.MCObject);
            }

            return result;
        }




    }
}
