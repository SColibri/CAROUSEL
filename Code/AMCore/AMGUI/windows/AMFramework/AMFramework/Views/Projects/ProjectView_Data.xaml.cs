using AMFramework.Controller;
using System.Windows.Controls;

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
