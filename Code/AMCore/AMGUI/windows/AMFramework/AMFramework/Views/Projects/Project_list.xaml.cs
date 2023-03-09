using System.Windows.Controls;

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

        public Project_list(Controller.Controller_Project dbs)
        {
            InitializeComponent();
            dbs.Load_projectList();
            DataContext = dbs;
        }
    }
}
