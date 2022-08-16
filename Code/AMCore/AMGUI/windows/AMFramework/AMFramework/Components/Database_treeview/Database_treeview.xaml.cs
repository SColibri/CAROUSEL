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

namespace AMFramework.Components.Database_treeview
{
    /// <summary>
    /// Interaction logic for Database_treeview.xaml
    /// </summary>
    public partial class Database_treeview : UserControl
    {
        public Database_treeview()
        {
            InitializeComponent();
        }

        public Database_treeview(ref Controller.Controller_DBS_Projects amCore)
        {
            DataContext = amCore;
        }


        public event EventHandler click_heat_treatment;
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Model.Model_HeatTreatment heatTreatment = (Model.Model_HeatTreatment)(((Border)sender).Tag);
            click_heat_treatment?.Invoke(heatTreatment, new EventArgs());
        }
    }
}
