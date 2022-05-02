using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScintillaNET;

namespace AMFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow_ViewModel viewModel = new();
        public MainWindow()
        {
            InitializeComponent();
            Framey.Navigate(new Views.Config.Configuration());



            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Chartingy.UpdateImage();
        }

        #region HandlesRibbon
        private void RibbonMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.Items.Add(viewModel.get_new_lua_script());
        }

        #endregion

        #region HandlesTemplate
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MainTabControl.SelectedItem != null)
            {
                if (((Interfaces.ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).close())
                {
                    ((Border)sender).Triggers.Clear();
                    ((TabItem)MainTabControl.SelectedItem).Visibility = Visibility.Collapsed;
                    MainTabControl.Items.RemoveAt(MainTabControl.SelectedIndex);
                }
            }    
        }
        #endregion

        private void RibbonMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if(MainTabControl.SelectedItem != null)
            {
                ((Interfaces.ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).save();
            }
        }
    }
}
