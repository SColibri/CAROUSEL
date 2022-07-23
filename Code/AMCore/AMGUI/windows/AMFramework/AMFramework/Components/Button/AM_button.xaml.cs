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
using FontAwesome.WPF;

namespace AMFramework.Components.Button
{
    /// <summary>
    /// Interaction logic for AM_button.xaml
    /// </summary>
    public partial class AM_button : UserControl, INotifyPropertyChanged
    {
        private string _iconName = "Close";
        public  string IconName 
        { 
            get { return _iconName; } 
            set 
            { 
                _iconName = value;
                OnPropertyChanged("IconName");
            } 
        }

        private string _foregroundIcon = "Black";
        public string ForegroundIcon 
        { 
            get { return _foregroundIcon; } 
            set 
            {
                _foregroundIcon = value;
                OnPropertyChanged("ForegroundIcon");
            }
        }

        private string _gradientColor_2 = "Silver";
        public string GradientColor_2
        {
            get { return _gradientColor_2; }
            set
            {
                _gradientColor_2 = value;
                OnPropertyChanged("GradientColor_2");
            }
        }

        private string _gradientColor_1 = "White";
        public string GradientColor_1
        {
            get { return _gradientColor_1; }
            set
            {
                _gradientColor_1 = value;
                OnPropertyChanged("GradientColor_1");
            }
        }

        private string _gradientTransition = "DarkRed";
        public string GradientTransition
        {
            get { return _gradientTransition; }
            set
            {
                _gradientTransition = value;
                OnPropertyChanged("GradientTransition");
            }
        }

        private string _cornerRadius = "10";

        public string CornerRadius
        {
            get { return _cornerRadius; }
            set 
            { 
                _cornerRadius = value;
                OnPropertyChanged("CornerRadius");
            }
        }

        private object _modelTag = null;

        public object ModelTag 
        { 
            get { return _modelTag; } 
            set 
            { 
                _modelTag = value;
                OnPropertyChanged("ModelTag");
            }
        }



        public AM_button()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region events
        public event EventHandler ClickButton;
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickButton?.Invoke(this, new EventArgs());
        }
        #endregion


    }
}
