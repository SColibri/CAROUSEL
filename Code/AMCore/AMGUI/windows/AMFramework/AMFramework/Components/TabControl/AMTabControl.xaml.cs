using AMFramework.Controller;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
