﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow_ViewModel viewModel = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Controller.Controller_MainWindow();
            Controller_Global.MainControl = (IMainWindow)DataContext;

            databaseView.DataContext = ((Controller.Controller_MainWindow)DataContext).get_project_controller();
            databaseView.Navigator.SelectedItemChanged += ((Controller.Controller_MainWindow)DataContext).Selected_treeview_item_Handle;
            databaseView.Navigator.SelectedItemChanged += Treeview_selectionChanged_Handle;
            databaseView.click_heat_treatment += Select_kinetic_precipitation;

            TV_Menu_Controller tvmController = new();
            tvmController.Main_Nodes.Add(new TV_TopView(((Controller.Controller_MainWindow)DataContext).get_project_controller().DTV_Controller));
            tvc.DataContext = tvmController;

            ((Controller.Controller_MainWindow)DataContext).get_project_controller().PropertyChanged += OnProperty_changed_project;

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
            Process me = Process.GetCurrentProcess();
            var listProcesses = Process.GetProcessesByName("mcc");
            foreach (var process in listProcesses)
            {
                process.Kill();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Chartingy.UpdateImage();
        }

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
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MainTabControl.SelectedItem != null)
            {
                if (((ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).close())
                {
                    ((Border)sender).Triggers.Clear();
                    ((TabItem)MainTabControl.SelectedItem).Visibility = Visibility.Collapsed;

                    if(((TabItem)MainTabControl.SelectedItem).Tag.GetType().Equals(typeof(ViewModel_Interface))) 
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

        private void RibbonMenuItem_Click_save_luaFile(object sender, RoutedEventArgs e)
        {
            if(MainTabControl.SelectedItem != null)
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

        private void RibbonMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //ControllerAMCore.CoreOutput = Sock.send_receive("initialize_core");
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
            
            if (controllerMain.Projects.Count == 0) 
            {
                notify.ShowBalloonTip(5000,"No open project","Please select a project",System.Windows.Forms.ToolTipIcon.Info);
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
                if(File.Exists(ofd.FileName) == true) 
                {

                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Add_Tab_Item(controllerMain.scriptView_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                    MainTabControl.Items.Refresh();
                }
                else 
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?","Script file",System.Windows.Forms.MessageBoxButtons.OK);
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

        private void ClosePopup(object? sender, System.EventArgs e)
        { 
            Popup.Visibility=Visibility.Collapsed;
            MainTabControl.Visibility = Visibility.Visible;
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

        #region Handles
        #region Treeview
        private void Treeview_selectionChanged_Handle(object sender, EventArgs e)
        {
            MainTabControl.Items.Refresh();

            if (MainTabControl.Items.Count == 0) return;
            MainTabControl.SelectedItem = MainTabControl.Items[MainTabControl.Items.Count - 1];
        }

        private void Treeview_selected_heatTreatment(object sender, EventArgs e)
        {



            if (MainTabControl.Items.Count == 0) return;
            MainTabControl.SelectedItem = MainTabControl.Items[MainTabControl.Items.Count - 1];
        }

        #endregion
        #region project
        private void OnProperty_changed_project(object? sender, PropertyChangedEventArgs propertyName)
        {
            if (sender == null) return;
            if (propertyName.PropertyName == null) return;

            Controller.Controller_DBS_Projects controllerReference = (Controller.Controller_DBS_Projects)sender;
            if (propertyName.PropertyName.ToUpper().CompareTo("SELECTEDTAB") == 0)
            {
                OnProperty_IsSelected(controllerReference);
            }
        }

        private void OnProperty_IsSelected(Controller.Controller_DBS_Projects controllerReference) 
        {
            // Do nothing, no project has been selected
            if (controllerReference.SelectedProject == null) return;

            remove_all_treeview_tabs();
            switch (controllerReference.SelectedTab)
            {
                case Controller.Controller_DBS_Projects.TABS.NONE:
                    break;
                case Controller.Controller_DBS_Projects.TABS.PROJECT:

                    break;
                case Controller.Controller_DBS_Projects.TABS.SELECTED_ELEMENTS:
                    break;
                case Controller.Controller_DBS_Projects.TABS.AVAILABLE_PHASES:
                    break;
                case Controller.Controller_DBS_Projects.TABS.SINGLE_PIXEL_CASES:
                    break;
                case Controller.Controller_DBS_Projects.TABS.CASEITEM:
                    break;
                default:
                    break;
            }

            // Remove tab if not selected
            if (controllerReference.ISselected == false) 
            {
                Remove_project_tab();
            }
            else 
            {
                // Add tab if it is selected
                Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                controllerMain.Add_Tab_Item(viewModel.get_project_tab(controllerMain.get_project_controller()));
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                MainTabControl.Items.Refresh();
            }

            if(controllerReference.SelectedCaseWindow == false) 
            {
                Remove_CasePlot_Tab();
            }
            else 
            {
                Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                controllerMain.Add_Tab_Item(viewModel.get_case_list_tab(controllerMain.get_plot_Controller()));
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                MainTabControl.Items.Refresh();
            }

            if (controllerReference.SelectedCaseID == false)
            {
                Remove_caseID_tab();
            }
            else
            {
                Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                controllerMain.Add_Tab_Item(viewModel.get_case_itemTab(controllerMain.get_project_controller().ControllerCases));
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                MainTabControl.Items.Refresh();
            }


            // Remove all other tabs
            Remove_cases_tab();
        }

        private void remove_all_treeview_tabs() 
        {
            Remove_project_tab();
            Remove_CasePlot_Tab();
            Remove_caseID_tab();
            Remove_cases_tab();
        }
        private void Remove_CasePlot_Tab()
        {
            for (int n1 = 0; n1 < MainTabControl.Items.Count; n1++)
            {
                if (((TabItem)MainTabControl.Items[n1]).Tag.GetType().Equals(typeof(Views.Case.Case_ViewModel)))
                {
                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Remove_tab_Item((TabItem)MainTabControl.Items[n1]);
                    MainTabControl.Items.Refresh();
                    break;
                }
            }
        }
        private void Remove_project_tab() 
        {
            for (int n1 = 0; n1 < MainTabControl.Items.Count; n1++)
            {
                if (((TabItem)MainTabControl.Items[n1]).Tag.GetType().Equals(typeof(Views.Projects.Project_ViewModel)))
                {
                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Remove_tab_Item((TabItem)MainTabControl.Items[n1]);
                    MainTabControl.Items.Refresh();
                    break;
                }
            }
        }

        private void Remove_caseID_tab()
        {
            for (int n1 = 0; n1 < MainTabControl.Items.Count; n1++)
            {
                if (((TabItem)MainTabControl.Items[n1]).Tag.GetType().Equals(typeof(Views.Projects.Project_ViewModel)))
                {
                    Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);
                    controllerMain.Remove_tab_Item((TabItem)MainTabControl.Items[n1]);
                    MainTabControl.Items.Refresh();
                    break;
                }
            }
        }

        private void Remove_cases_tab() 
        { 
        
        }

        private void Select_kinetic_precipitation(object? sender, EventArgs e)
        {
            if (sender is null) return;
            if (!sender.GetType().Equals(typeof(Model_HeatTreatment))) return;
            Model_HeatTreatment refModel = (Model_HeatTreatment)sender;

            // clear all tabs and reload selection
            Controller.Controller_MainWindow controllerMain = ((Controller.Controller_MainWindow)DataContext);

            for (int n1 = 0; n1 < MainTabControl.Items.Count; n1++)
            {
                if (((TabItem)MainTabControl.Items[n1]).Tag.GetType().Equals(typeof(Views.Precipitation_Kinetics.Precipitation_kinetics_plot)))
                {
                    controllerMain.Remove_tab_Item((TabItem)MainTabControl.Items[n1]);
                    MainTabControl.Items.Refresh();
                    break;
                }
            }

            Controller.Controller_Plot refPlot = controllerMain.get_plot_Controller();
            refPlot.HeatModel = refModel;
            controllerMain.Add_Tab_Item(viewModel.get_kinetic_precipitation_itemTab(refPlot));
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            MainTabControl.Items.Refresh();
        }

        #endregion


        #endregion

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
    }
}
