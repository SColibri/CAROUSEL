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

namespace AMFramework.Views.Elements
{
    /// <summary>
    /// Interaction logic for ElementsList.xaml
    /// </summary>
    public partial class ElementsList : UserControl
    {
        public ElementsList()
        {
            InitializeComponent();
        }

        public ElementsList(Controller.Controller_DBS_Projects projectController)
        {
            InitializeComponent();
            DataContext = projectController;
            projectController.load_database_available_elements();
            //System.Threading.Thread TH01 = new System.Threading.Thread(Load_Elements_Async);
            //TH01.Start();
        }

        private void Load_Elements_Async() 
        {
            ((Controller.Controller_DBS_Projects)DataContext).load_database_available_elements();
        }
    }
}
