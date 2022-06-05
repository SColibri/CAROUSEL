﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace AMFramework.Controller
{
    public class Controller_MainWindow : INotifyPropertyChanged
    {
        
        private Controller.Controller_AMCore _AMCore;
        private Controller.Controller_DBS_Projects _DBSProjects;
        private Core.AMCore_Socket _coreSocket = new Core.AMCore_Socket();
        public Controller_MainWindow() 
        {
            _coreSocket.init();
            _AMCore = new(_coreSocket);
            _DBSProjects = new(_coreSocket);
            _AMCore.PropertyChanged += Core_output_changed_Handle;

            reloadProjects();
        }

        private void Core_output_changed_Handle(object sender, PropertyChangedEventArgs e) 
        {
            if (e is null) return;
            if (e.PropertyName.CompareTo("CoreOutput") == 0) 
            {
                CoreOut = _AMCore.CoreOutput;
            }
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Core
        private string _coreOut = "";
        public string CoreOut { 
            get { return _coreOut; } 
            set 
            {
                _coreOut = value;
                OnPropertyChanged("CoreOut");
            } 
        }

        #endregion

        #region Projects
        private List<Model.Model_Projects> _projects = new();
        public List<Model.Model_Projects> Projects { 
            get => _projects; 
            set 
            {
                _projects = value;
                OnPropertyChanged("Projects");
            } 
        }
        public void reloadProjects() 
        {
            CoreOut = _DBSProjects.DB_projects_reload();
            Projects = _DBSProjects.DB_projects;
        }
        public void createProject(string Name) 
        { 
            CoreOut = _DBSProjects.DB_projects_create_new(Name);
        }

        public Components.Windows.AM_popupWindow popupProject(int ID) 
        {
            Views.Projects.Project_general Pg = new(_DBSProjects, ID);
            Components.Windows.AM_popupWindow Pw = new() { Title = "New project" };
            Pw.ContentPage.Children.Add(Pg);
            
            Components.Button.AM_button nbutt = new() 
            { 
                IconName = "Save",
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += Pg.saveClickHandle;

            Pw.add_button(nbutt);
            return Pw;
        }

        public Components.Windows.AM_popupWindow popupProjectList(int ID)
        {
            Views.Projects.Project_list Pg = new(_DBSProjects);
            Components.Windows.AM_popupWindow Pw = new() { Title = "Open" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Upload.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            //nbutt.ClickButton += Pg.saveClickHandle;

            Pw.add_button(nbutt);
            return Pw;
        }
        #endregion

        #region Scripting
        private MainWindow_ViewModel _scriptModel = new();
        public MainWindow_ViewModel ScriptView => _scriptModel;
        public List<RibbonMenuItem> OpenScripts
        {
            get 
            {   
                List<RibbonMenuItem> menu = new List<RibbonMenuItem>();
                foreach (Components.Scripting.Scripting_ViewModel item in _scriptModel.OpenScripts)
                {
                    RibbonMenuItem itemy = new()
                    {
                        Header = item.Filename,
                        Tag = item.Filename
                    };

                    itemy.Click += run_script;
                    menu.Add(itemy);
                }
                return menu; 
            }
        }

        private void run_script(object sender, EventArgs e) 
        {
            RibbonMenuItem itemy = (RibbonMenuItem)sender;
            _AMCore.Run_command("run_lua_script " + itemy.Tag);
        }

        public System.Windows.Controls.TabItem scriptView_new_lua_script(string filename = "") 
        {
            System.Windows.Controls.TabItem Tabby = ScriptView.get_new_lua_script(filename);
            OnPropertyChanged("OpenScripts");
            return Tabby;
        }
        #endregion

        #region Plotting
        public TabItem get_new_plot(string plotName = "")
        {
            TabItem result = ScriptView.get_new_plot();
            return result;
        }

        #endregion

    }
}