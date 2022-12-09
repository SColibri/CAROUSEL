using AMFramework_Lib.Model;
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
