using System.Collections.Generic;
using System.IO;
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
            DataContext = new Controller.Controller_MainWindow();
            databaseView.DataContext = DataContext;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Chartingy.UpdateImage();
        }

        #region HandlesRibbon
        private void RibbonMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.Items.Add(viewModel.get_new_lua_script());
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;

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

                    if(((TabItem)MainTabControl.SelectedItem).Tag.GetType().Equals(typeof(Interfaces.ViewModel_Interface))) 
                    {
                        ((Interfaces.ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).close();
                    }

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

        private void RibbonMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //ControllerAMCore.CoreOutput = Sock.send_receive("initialize_core");
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.Items.Add(viewModel.get_new_plot());
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            
        }

        private void RibbonMenuItem_Click_3()
        {

        }

        private void RibbonMenuItem_Click_4()
        {

        }

        private void RibbonMenuItem_Click_loadApi(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = false;

            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            { 
                if(File.Exists(ofd.FileName) == true) 
                {
                    MainTabControl.Items.Add(viewModel.get_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                }
                else 
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?","Script file",System.Windows.Forms.MessageBoxButtons.OK);
                }
            }

        }

        private void RibbonButton_Click_newProject(object sender, RoutedEventArgs e)
        {

        }

        private void RibbonButton_Click_1(object sender)
        {

        }

        private void RibbonButton_Click_NewProject(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupProject(-1);
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupProjectList(-1);
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }

        private void ClosePopup(object sender, System.EventArgs e)
        { 
            Popup.Visibility=Visibility.Collapsed;
        }

        private void RibbonButton_Click_RunScript(object sender, RoutedEventArgs e)
        {
           // if(((TabItem)MainTabControl.SelectedItem).Content.GetType().Equals(typeof()))
        }

        #region GLOBAL
        public static System.Windows.Forms.NotifyIcon notify = new()
        {
            Icon = new System.Drawing.Icon("Resources/Icons/Logo.ico"),
            Visible = true
        };

        #endregion

        
    }
}
