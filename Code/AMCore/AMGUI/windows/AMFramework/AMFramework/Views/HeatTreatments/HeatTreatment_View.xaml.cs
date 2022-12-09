using AMFramework_Lib.Core;
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
