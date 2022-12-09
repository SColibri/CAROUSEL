using AMFramework.Controller;
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

namespace AMFramework.Components.TabControl
{
    /// <summary>
    /// Interaction logic for AMTabControl.xaml
    /// </summary>
    public partial class AMTabControl : UserControl
    {
        public AMTabControl()
        {
            InitializeComponent();
        }

        private void DataContextChangedEvent(object? sender, DependencyPropertyChangedEventArgs e) 
        {
            if (e.NewValue is Controller_Tabs tabController)
                ((Controller_Tabs)this.DataContext).PropertyChanged += DataContextPropertyChanged;

            ((Controller_Tabs)this.DataContext).DataHasChanged = true;
        }

        private void DataContextPropertyChanged(object? sender, PropertyChangedEventArgs e) 
        {
            if (sender is not Controller_Tabs cT) return;
            if (e.PropertyName?.CompareTo(nameof(Controller_Tabs.DataHasChanged)) == 0) 
            {
                if (cT.DataHasChanged) 
                {
                    MainControl.Items.Refresh();
                    cT.DataHasChanged = false; 
                }
            }
        }

    }
}
