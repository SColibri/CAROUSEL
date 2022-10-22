using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AMFramework.Components.Button
{
    /// <summary>
    /// Interaction logic for ConfirmButton.xaml
    /// </summary>
    public partial class ConfirmButton : UserControl, INotifyPropertyChanged
    {
        public ConfirmButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Interfaces
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string _text = "Accept";
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                CurrentText = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private string _warningText = "Warning message";
        public string WarningText
        {
            get { return _warningText; }
            set
            {
                _warningText = value;
                OnPropertyChanged(nameof(WarningText));
            }
        }

        private string _currentText = "Accept";
        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                _currentText = value;
                OnPropertyChanged(nameof(CurrentText));
            }
        }

        private string _cornerRadius = "15";
        public string CornerRadius 
        { 
            get { return _cornerRadius; } 
            set 
            { 
                _cornerRadius = value;
                OnPropertyChanged(nameof(CornerRadius));
            }
        }

        private string _gradientColor_2 = "Silver";
        public string GradientColor_2
        {
            get { return _gradientColor_2; }
            set
            {
                _gradientColor_2 = value;
                OnPropertyChanged(nameof(GradientColor_2));
            }
        }

        private string _gradientColor_1 = "White";
        public string GradientColor_1
        {
            get { return _gradientColor_1; }
            set
            {
                _gradientColor_1 = value;
                OnPropertyChanged(nameof(GradientColor_1));
            }
        }

        private string _gradientTransition = "DarkRed";
        public string GradientTransition
        {
            get { return _gradientTransition; }
            set
            {
                _gradientTransition = value;
                OnPropertyChanged(nameof(GradientTransition));
            }
        }

        private string _gradientMouseEnter = "AliceBlue";
        public string GradientMouseEnter
        {
            get { return _gradientMouseEnter; }
            set
            {
                _gradientMouseEnter = value;
                OnPropertyChanged(nameof(GradientMouseEnter));
            }
        }

        private string _foreColor = "DarkRed";
        public string ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        private bool _Background1_isVisible = true;
        public bool Background1_isVisible
        {
            get { return _Background1_isVisible; }
            set
            {
                _Background1_isVisible = value;
                OnPropertyChanged(nameof(Background1_isVisible));
            }
        }

        private bool _Background2_isVisible = false;
        public bool Background2_isVisible
        {
            get { return _Background2_isVisible; }
            set
            {
                _Background2_isVisible = value;
                OnPropertyChanged(nameof(Background2_isVisible));
            }
        }

        #region Handles

        DateTime MouseDownTime = DateTime.UtcNow;
        DateTime MouseUpTime = DateTime.UtcNow;
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentText = Text;
            MouseUpTime = DateTime.UtcNow;

            TimeSpan timeDiff = MouseUpTime - MouseDownTime;
            if (Convert.ToInt32(timeDiff.TotalSeconds) > 2.5) 
            {
                ClickButton?.Invoke(this, new EventArgs());
            }

            Background2_isVisible = false;
            Background1_isVisible = true;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentText = _warningText;
            MouseDownTime = DateTime.UtcNow;

            Background2_isVisible = true;
            Background1_isVisible = false;
        }

        #endregion

        #region Events
        public event EventHandler ClickButton;
        #endregion

    }
}
