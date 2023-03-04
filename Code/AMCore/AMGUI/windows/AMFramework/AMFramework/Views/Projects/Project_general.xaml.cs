using System.Windows.Controls;

namespace AMFramework.Views.Projects
{
    /// <summary>
    /// Interaction logic for Project_general.xaml
    /// </summary>
    public partial class Project_general : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Project_general()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbs"></param>
        public Project_general(Controller.Controller_Project dbs)
        {
            DataContext = dbs;
            InitializeComponent();
            Pname.Focus();
        }
    }
}
