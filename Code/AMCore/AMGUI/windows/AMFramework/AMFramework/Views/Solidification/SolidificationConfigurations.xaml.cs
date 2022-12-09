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

namespace AMFramework.Views.Solidification
{
    /// <summary>
    /// Interaction logic for SolidificationConfigurations.xaml
    /// </summary>
    public partial class SolidificationConfigurations : UserControl
    {
        public SolidificationConfigurations()
        {
            InitializeComponent();
        }

        public SolidificationConfigurations(Model_Case caseModel)
        {
            InitializeComponent();
            DataContext = new SolidificationConfigurations_ViewModel(caseModel);
        }

    }
}
