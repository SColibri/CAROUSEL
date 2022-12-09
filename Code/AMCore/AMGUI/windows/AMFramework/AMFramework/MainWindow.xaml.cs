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
using AMFramework_Lib.Model.Model_Controllers;
using AMFramework.Views.Case;
using AMFramework.Views.Case.plotViews;

namespace AMFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml, Entry point
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Entry point for the application
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Controller.Controller_MainWindow();

            // ------------------------------------------
            //          Initialize components
            // --------------------------------sh----------

            // add Main control reference to global controller
            Controller_Global.MainControl = (IMainWindow)DataContext;

            // Create new Datagrid view control
            TV_Menu_Controller tvmController = new();
            tvmController.Main_Nodes.Add(new TV_TopView(((Controller.Controller_MainWindow)DataContext).TreeViewController));
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

        #region RibbonMenu_Handles

        // ------------------------------------------------------------------
        // NOTE: These handles where just a fast implementation for testing and 
        // a better practice is to use the MVVM model, and create commands on the 
        // Main window controller. This also applies for some of the methods included
        // in this file.
        // ------------------------------------------------------------------
       

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);

            if (controllerMain.Projects.Count == 0)
            {
                notify.ShowBalloonTip(5000, "No open project", "Please select a project", System.Windows.Forms.ToolTipIcon.Info);
                return;
            }

            //controllerMain.Add_Tab_Item(viewModel.Get_projectMap_plot(controllerMain.get_plot_Controller()));
            //MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            //MainTabControl.Items.Refresh();
        }


        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            //MainTabControl.Visibility = Visibility.Collapsed;
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupConfigurations();
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }

        #endregion

        #endregion

        #region Methods
        private void ClosePopup(object? sender, System.EventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            //MainTabControl.Visibility = Visibility.Visible;
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
