using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AMControls.Custom.ProjectTreeView;
using AMFramework.Controller;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Model;
using ScintillaNET;

namespace AMFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml, Entry point
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow_ViewModel viewModel = new();

        /// <summary>
        /// Entry point for the application
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Controller.Controller_MainWindow();

            // ------------------------------------------
            //          Initialize components
            // ------------------------------------------

            // add Main control reference to global controller
            Controller_Global.MainControl = (IMainWindow)DataContext;

            // Create new Datagrid view control
            TV_Menu_Controller tvmController = new();
            tvmController.Main_Nodes.Add(new TV_TopView(((Controller.Controller_MainWindow)DataContext).get_project_controller().DTV_Controller));
            tvc.DataContext = tvmController;

            // When closed add the closing handle, this closes all additional
            // windows.
            this.Closing += Closing_window;
        }

        /// <summary>
        /// This will close all mcc children, however, if the application crashes, this will not be closed properly either.
        /// note also that we are closing all mcc named processes, so if we want multiple windows we don't know what will happen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closing_window(object? sender, CancelEventArgs e)
        {
            // Core implementation leaves some matcalc processes open, here
            // we ensure that they get closed when gui application is closed (not crashed)
            Process me = Process.GetCurrentProcess();
            var listProcesses = Process.GetProcessesByName("mcc");
            foreach (var process in listProcesses)
            {
                process.Kill();
            }
        }

        #region Handles

        #region Window_Handles
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Chartingy.UpdateImage();
        }

        #endregion

        #region HandlesRibbon
        private void RibbonMenuItem_Click_new_luaFile(object sender, RoutedEventArgs e)
        {
            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
            controllerMain.Add_Tab_Item(viewModel.get_new_lua_script());
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            MainTabControl.Items.Refresh();
        }

        #endregion

        #region HandlesTemplate
        /// <summary>
        /// Close tab Icon handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MainTabControl.SelectedItem != null)
            {
                if (((ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).close())
                {
                    ((Border)sender).Triggers.Clear();
                    ((TabItem)MainTabControl.SelectedItem).Visibility = Visibility.Collapsed;

                    if (((TabItem)MainTabControl.SelectedItem).Tag.GetType().Equals(typeof(ViewModel_Interface)))
                    {
                        ((ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).close();
                    }

                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Remove_tab_Item((TabItem)MainTabControl.SelectedItem);
                    MainTabControl.Items.Refresh();
                }
            }
        }
        #endregion

        #region RibbonMenu_Handles

        // ------------------------------------------------------------------
        // NOTE: These handles where just a fast implementation for testing and 
        // a better practice is to use the MVVM model, and create commands on the 
        // Main window controller. This also applies for some of the methods included
        // in this file.
        // ------------------------------------------------------------------
        
        private void RibbonMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //ControllerAMCore.CoreOutput = Sock.send_receive("initialize_core");
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);

            if (controllerMain.Projects.Count == 0)
            {
                notify.ShowBalloonTip(5000, "No open project", "Please select a project", System.Windows.Forms.ToolTipIcon.Info);
                return;
            }

            controllerMain.Add_Tab_Item(viewModel.Get_projectMap_plot(controllerMain.get_plot_Controller()));
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            MainTabControl.Items.Refresh();
        }

        private void RibbonButton_TableView_Click(object sender, RoutedEventArgs e)
        {
            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);

            controllerMain.Add_Tab_Item(viewModel.Get_DataGridTable(controllerMain.Get_socket()));
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            MainTabControl.Items.Refresh();
        }

        private void RibbonMenuItem_Click_loadApi(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new()
            {
                Multiselect = false
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(ofd.FileName) == true)
                {

                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Add_Tab_Item(controllerMain.scriptView_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                    MainTabControl.Items.Refresh();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?", "Script file", System.Windows.Forms.MessageBoxButtons.OK);
                }
            }

        }

        private void RibbonButton_Click_NewProject(object sender, RoutedEventArgs e)
        {
            //MainTabControl.Visibility = Visibility.Collapsed;
            //Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupProject(-1);
            //Pw.PopupWindowClosed += ClosePopup;

            controller_MainWindow.Show_Popup(Pw);
            //PopupFrame.Navigate(Pw);
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            MainTabControl.Visibility = Visibility.Collapsed;
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupProjectList(-1);
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }

        private void RibbonMenuItem_Click_save_luaFile(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem != null)
            {
                ((ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).save();
            }
        }
        private void RibbonMenuItem_Click_LoadLuaFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new()
            {
                Filter = " lua script | *.lua",
                InitialDirectory = Controller_Global.Configuration?.Working_Directory,
                Multiselect = false
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(ofd.FileName) == true)
                {
                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Add_Tab_Item(controllerMain.scriptView_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                    MainTabControl.Items.Refresh();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?", "Script file", System.Windows.Forms.MessageBoxButtons.OK);
                }
            }
        }

        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            MainTabControl.Visibility = Visibility.Collapsed;
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupConfigurations();
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }

        private void RibbonButton_Click_3(object sender, RoutedEventArgs e)
        {
            ((Controller.Controller_MainWindow)DataContext).Cancel_Script();
        }

        #endregion

        #endregion

        #region Methods
        private void ClosePopup(object? sender, System.EventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            MainTabControl.Visibility = Visibility.Visible;
        }
        #endregion









        #region Static methods
        [Obsolete("Deprecate prefer using controller global notify")]
        public static System.Windows.Forms.NotifyIcon notify = new()
        {
            Icon = new System.Drawing.Icon("Resources/Icons/Logo.ico"),
            Visible = true
        };

        #endregion

        
    }
}
