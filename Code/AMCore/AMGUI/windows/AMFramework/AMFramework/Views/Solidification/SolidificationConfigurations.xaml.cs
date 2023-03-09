using AMFramework_Lib.Model;
using System.Windows.Controls;

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
