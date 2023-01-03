using AMFramework.Controller;
using AMFramework.Views.ActivePhases;
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

namespace AMFramework.Views.Projects
{
    /// <summary>
    /// Interaction logic for ProjectView_Data.xaml
    /// </summary>
    public partial class ProjectView_Data : UserControl
    {
        public ProjectView_Data(Controller_Project projectController)
        {
            InitializeComponent();
            DataContext = projectController;
        }
    }
}
