using System;
using System.Collections;
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

namespace AMControls.Custom.ProjectTreeView
{
    /// <summary>
    /// Interaction logic for TV_TopView.xaml
    /// </summary>
    public partial class TV_TopView : UserControl, INotifyPropertyChanged
    {
        public TV_TopView() 
        {
            InitializeComponent();
        }
        public TV_TopView(TV_TopView_controller tController)
        {
            InitializeComponent();
            DataContext = tController;
        }


        #region interface
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TV_TopView_controller tref = (TV_TopView_controller)this.DataContext;

            if (tref.IsSelected) 
            {
                tref.IsExpanded = false;
                tref.IsSelected = false;
            }
            else 
            {
                tref.IsExpanded = true;
                tref.IsSelected = true;
            }

        }
    }
}
