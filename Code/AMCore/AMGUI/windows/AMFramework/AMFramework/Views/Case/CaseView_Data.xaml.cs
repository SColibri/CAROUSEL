using System.Windows.Controls;

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for CaseView_Data.xaml
    /// </summary>
    public partial class CaseView_Data : UserControl
    {
        public CaseView_Data()
        {
            InitializeComponent();
        }

        public CaseView_Data(Controller.Controller_Cases cCase)
        {
            InitializeComponent();
            DataContext = cCase;
        }
    }
}
