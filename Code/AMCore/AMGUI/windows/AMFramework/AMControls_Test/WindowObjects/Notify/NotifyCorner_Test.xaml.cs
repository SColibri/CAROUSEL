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

namespace AMControls_Test.WindowObjects.Notify
{
    /// <summary>
    /// Interaction logic for NotifyCorner_Test.xaml
    /// </summary>
    public partial class NotifyCorner_Test : Window
    {
        public NotifyCorner_Test()
        {
            InitializeComponent();
            DataContext = this;
        }

        private ICommand? _clickOnItem;

        public ICommand ClickOnItem
        {
            get
            {
                if (_clickOnItem == null)
                {
                    _clickOnItem = new AMControls.Implementations.Commands.RelayCommand(
                        param => this.ClickOnItem_Action(),
                        param => this.ClickOnItem_Check()
                    );
                }
                return _clickOnItem;
            }
        }

        private void ClickOnItem_Action()
        {
            animationThingy.Show();
        }

        private bool ClickOnItem_Check()
        {
            return true;
        }
    }
}
