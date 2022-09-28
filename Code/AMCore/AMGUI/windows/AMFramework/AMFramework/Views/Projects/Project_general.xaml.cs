using AMFramework.Model;
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
        private Controller.Controller_DBS_Projects _dbs;
        public Project_general()
        {
            InitializeComponent();
        }

        public Project_general(Controller.Controller_DBS_Projects dbs, int ID)
        {
            _dbs = dbs;
            DataContext = dbs.DataModel(ID);
            InitializeComponent();
            Pname.Focus();
        }

        public void save() 
        {
          _dbs.save_DataModel((Model.Model_Projects)DataContext);
        }

        public void saveClickHandle(object sender, EventArgs e) { save(); }
    }
}
