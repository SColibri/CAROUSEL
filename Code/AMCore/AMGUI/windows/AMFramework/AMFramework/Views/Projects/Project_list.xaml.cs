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
    /// Interaction logic for Project_list.xaml
    /// </summary>
    public partial class Project_list : UserControl
    {
        public Project_list()
        {
            InitializeComponent();
        }

        public Project_list(Controller.Controller_DBS_Projects dbs)
        {
            InitializeComponent();
            dbs.DB_projects_reload();
            DataContext = dbs;
        }
    }
}
