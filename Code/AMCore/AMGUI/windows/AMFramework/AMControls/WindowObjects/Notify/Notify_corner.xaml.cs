using AMControls.Implementations.Commands;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AMControls.WindowObjects.Notify
{
    /// <summary>
    /// Interaction logic for Notify_corner.xaml
    /// 
    /// 
    /// </summary>
    public partial class Notify_corner : UserControl, INotifyPropertyChanged
    {
        public Notify_corner()
        {
            InitializeComponent();
            DataContext = this;
            _showTime.Elapsed += Show_timer_elapsedHandle;
        }

        #region Parameters
        private Color _titleBackground = Colors.LightBlue;
        public Color TitleBackground
        {
            get { return _titleBackground; }
            set
            {
                _titleBackground = value;
                OnPropertyChanged(nameof(TitleBackground));
            }
        }

        private Brush _contentBackground = Brushes.Black;
        public Brush ContentBackground
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

        private Brush _iconForeground = Brushes.Red;
        public Brush IconForeground
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

        private bool _showAnimation = false;
        public bool ShowAnimation
        {
            get { return _showAnimation; }
            set
            {
                _showAnimation = value;
                OnPropertyChanged(nameof(ShowAnimation));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Timer used for showing the popup in a defined time interval.
        /// Animations wil start when modifying the ShowAnoímation parameter
        /// </summary>
        private Timer _showTime = new();

        /// <summary>
        /// Show popup for a defined time interval
        /// </summary>
        /// <param name="interval"></param>
        public void Show(double interval = 5000)
        {
            this.Visibility = Visibility.Visible;
            _showTime.Stop();
            _showTime.Interval  = interval;
            _showTime.Start();

            ShowAnimation = true;
        }

        /// <summary>
        /// This handle changes the ShowAnimation parameter to false so it can finalize the animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_timer_elapsedHandle(object? sender, ElapsedEventArgs e)
        {
            ShowAnimation = false;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Visibility = Visibility.Collapsed;
            }));

            if (sender != null)
                ((Timer)sender).Stop();
        }

        #endregion

        #region Commands
        private ICommand _clickOnItem;

        /// <summary>
        /// Click on item invokes the clicked event, this happens when clicked on
        /// the top border object
        /// </summary>
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

        /// <summary>
        /// Command handle for when command is executed. Invokes the clicked event.
        /// </summary>
        private void ClickOnItem_Action()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Click on item is possible by default, insert conditions if needed
        /// </summary>
        /// <returns></returns>
        private bool ClickOnItem_Check()
        {
            return true;
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
        /// <summary>
        /// Eventhandler for click action
        /// </summary>
        public event EventHandler? Clicked;

        #endregion
    }
}
