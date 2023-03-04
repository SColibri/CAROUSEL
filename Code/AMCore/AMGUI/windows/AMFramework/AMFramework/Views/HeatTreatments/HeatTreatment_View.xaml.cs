using System.Windows.Controls;

namespace AMFramework.Views.HeatTreatments
{
    /// <summary>
    /// Interaction logic for HeatTreatment_View.xaml
    /// </summary>
    public partial class HeatTreatment_View : UserControl
    {
        public HeatTreatment_View()
        {
            InitializeComponent();
        }

        public HeatTreatment_View(Controller_HeatTreatmentView controller)
        {
            InitializeComponent();
            DataContext = controller;
        }
    }
}
