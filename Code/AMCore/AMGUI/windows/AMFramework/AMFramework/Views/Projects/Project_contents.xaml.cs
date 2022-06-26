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
    /// Interaction logic for Project_contents.xaml
    /// </summary>
    public partial class Project_contents : UserControl
    {
        public Project_contents()
        {
            InitializeComponent();
        }

        public Project_contents(ref Controller.Controller_DBS_Projects pController)
        {
            InitializeComponent();
            DataContext = pController;
        }
    }
}
