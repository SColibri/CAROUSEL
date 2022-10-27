using AMFramework.AMSystem;
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
using System.Windows.Shapes;

namespace AMFramework.N_GUI
{
    /// <summary>
    /// Interaction logic for WindowNewTest.xaml
    /// </summary>
    public partial class WindowNewTest : Window
    {
        public WindowNewTest()
        {
            InitializeComponent();
            DataContext = new Controller_NorasMainWindow();
        }
    }
}
