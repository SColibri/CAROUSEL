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

namespace AMFramework.Views.Phase
{
    /// <summary>
    /// Interaction logic for Phase_List.xaml
    /// </summary>
    public partial class Phase_List : UserControl
    {
        public Phase_List()
        {
            InitializeComponent();
        }

        public Phase_List(Controller.Controller_DBS_Projects projectController)
        {
            InitializeComponent();
            DataContext = projectController;
            projectController.get_phase_selection_from_current_case();
        }
    }
}
