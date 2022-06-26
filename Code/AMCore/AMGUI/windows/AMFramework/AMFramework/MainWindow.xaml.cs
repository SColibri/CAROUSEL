using System.Collections.Generic;
using System.ComponentModel;
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

            databaseView.DataContext = ((Controller.Controller_MainWindow)DataContext).get_project_controller();
            databaseView.Navigator.SelectedItemChanged += ((Controller.Controller_MainWindow)DataContext).get_project_controller().selected_treeview_item;

            ((Controller.Controller_MainWindow)DataContext).get_project_controller().PropertyChanged += OnProperty_changed_project;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Chartingy.UpdateImage();
        }

        #region HandlesRibbon
        private void RibbonMenuItem_Click_new_luaFile(object sender, RoutedEventArgs e)
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

        private void RibbonMenuItem_Click_save_luaFile(object sender, RoutedEventArgs e)
        {
            if(MainTabControl.SelectedItem != null)
            {
                ((Interfaces.ViewModel_Interface)((TabItem)MainTabControl.SelectedItem).Tag).save();
            }
        }
        private void RibbonMenuItem_Click_LoadLuaFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = " lua script | *.lua";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(ofd.FileName) == true)
                {
                    MainTabControl.Items.Add(((Controller.Controller_MainWindow)DataContext).scriptView_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
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
            MainTabControl.Items.Add(viewModel.get_new_plot());
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            
        }

        private void RibbonMenuItem_Click_loadApi(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = false;

            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            { 
                if(File.Exists(ofd.FileName) == true) 
                {
                    MainTabControl.Items.Add(((Controller.Controller_MainWindow)DataContext).scriptView_new_lua_script(ofd.FileName));
                    MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
                }
                else 
                {
                    System.Windows.Forms.MessageBox.Show("Ups, where did the file go?","Script file",System.Windows.Forms.MessageBoxButtons.OK);
                }
            }

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

        #region Handles
        #region project
        private void OnProperty_changed_project(object sender, PropertyChangedEventArgs propertyName)
        {
            if (propertyName.PropertyName == null) return;

            Controller.Controller_DBS_Projects controllerReference = (Controller.Controller_DBS_Projects)sender;
            if (propertyName.PropertyName.ToUpper().CompareTo("ISSELECTED") == 0)
            {
                OnProperty_IsSelected(controllerReference);
            }
        }

        private void OnProperty_IsSelected(Controller.Controller_DBS_Projects controllerReference) 
        {
            // Do nothing, no project has been selected
            if (controllerReference.SelectedProject == null) return;

            // Remove tab if not selected
            if (controllerReference.ISselected == false) 
            {
                Remove_project_tab();
                return;
            }

            // Add tab if it is selected
            MainTabControl.Items.Add(viewModel.get_project_tab(((Controller.Controller_MainWindow)DataContext).get_project_controller()));
            MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;

            // Remove all other tabs
            Remove_cases_tab();
        }

        private void Remove_project_tab() 
        {
            for (int n1 = 0; n1 < MainTabControl.Items.Count; n1++)
            {
                if (((TabItem)MainTabControl.Items[n1]).Tag.GetType().Equals(typeof(Views.Projects.Project_ViewModel)))
                {
                    MainTabControl.Items.Remove(MainTabControl.Items[n1]);
                    break;
                }
            }
        }

        private void Remove_cases_tab() 
        { 
        
        }

        #endregion


        #endregion

        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Visible;

            Controller.Controller_MainWindow controller_MainWindow = (Controller.Controller_MainWindow)DataContext;
            Components.Windows.AM_popupWindow Pw = controller_MainWindow.popupConfigurations();
            Pw.PopupWindowClosed += ClosePopup;

            PopupFrame.Navigate(Pw);
        }
    }
}
