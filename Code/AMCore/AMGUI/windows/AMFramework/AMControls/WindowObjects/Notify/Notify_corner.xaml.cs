using AMControls.Implementations.Commands;
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

namespace AMControls.WindowObjects.Notify
{
    /// <summary>
    /// Interaction logic for Notify_corner.xaml
    /// </summary>
    public partial class Notify_corner : UserControl, INotifyPropertyChanged
    {
        public Notify_corner()
        {
            InitializeComponent();
        }

        #region Parameters
        private Color _titleBackground = Colors.SteelBlue;
        public Color TitleBackground
        {
            get { return _titleBackground; }
            set 
            {
                _titleBackground = value;
                OnPropertyChanged(nameof(TitleBackground));
            }
        }

        private Color _contentBackground = Colors.Black;
        public Color ContentBackground 
        {
            get { return _contentBackground; }
            set 
            {
                _contentBackground = value;
                OnPropertyChanged(nameof(ContentBackground));
            }
        }

        private string _title = "New item";
        public string Title 
        {
            get { return _title; }
            set 
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private FontAwesome.WPF.FontAwesomeIcon _icon = FontAwesome.WPF.FontAwesomeIcon.None;
        public FontAwesome.WPF.FontAwesomeIcon Icon 
        {
            get { return _icon; }
            set 
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        private Color _iconForeground = Colors.Red;
        public Color IconForeground
        {
            get { return _iconForeground; }
            set
            {
                _iconForeground = value;
                OnPropertyChanged(nameof(IconForeground));
            }
        }

        private string _text = "";
        public string Text 
        {
            get { return _text; }
            set 
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private object? _contentTag;
        public object? ContentTag
        {
            get { return _contentTag; }
            set 
            {
                _contentTag = value;
                OnPropertyChanged(nameof(ContentTag));
            }
        }
        #endregion

        #region Commands
        private ICommand _clickOnItem;

        public ICommand ClickOnItem
        {
            get
            {
                if (_clickOnItem == null)
                {
                    _clickOnItem = new RelayCommand(
                        param => this.ClickOnItem_Action(),
                        param => this.ClickOnItem_Check()
                    );
                }
                return _clickOnItem;
            }
        }

        private void ClickOnItem_Action() 
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private bool ClickOnItem_Check() 
        {
            return false;
        }

        #endregion

        #region Interface
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Events
        public event EventHandler? Clicked;

        #endregion
    }
}
