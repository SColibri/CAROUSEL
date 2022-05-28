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
using System.ComponentModel;

namespace AMFramework.Components.Windows
{
    /// <summary>
    /// Interaction logic for AM_popupWindow.xaml
    /// </summary>
    public partial class AM_popupWindow : UserControl, INotifyPropertyChanged
    {
        public AM_popupWindow() 
        { 
            InitializeComponent();
            DataContext = this;
        }

        private string _title = "";
        public string Title 
        { 
            get { return _title; } 
            set { 
                _title = value;
                OnPropertyChanged("Title");
            } 
        }

        public void add_button(Components.Button.AM_button toAdd) 
        {
            ButtonsPanel.Children.Add(toAdd);
        }

        #region events
        public event EventHandler PopupWindowClosed;
        #endregion
        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void AM_button_ClickButton(object sender, EventArgs e)
        {
            PopupWindowClosed?.Invoke(this, e);
        }
    }
}
