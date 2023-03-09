using AMFramework_Lib.Model;
using System.Windows.Controls;

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for Case_newitem.xaml
    /// </summary>
    public partial class Case_newitem : UserControl
    {
        public Case_newitem()
        {
            InitializeComponent();
        }

        public Case_newitem(Model_Case caseModel)
        {
            InitializeComponent();
            DataContext = caseModel;
        }
    }
}
