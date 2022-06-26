using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AMFramework.Controller
{
    public class Controller_DBS_Projects : INotifyPropertyChanged
    {
        #region Socket
        private Core.AMCore_Socket _AMCore_Socket;
        public Controller_DBS_Projects(Core.AMCore_Socket socket) 
        {
            _AMCore_Socket = socket;
            controllerCases = new(ref socket, this);
            Controller_Selected_Elements = new(ref socket, this);
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Flags
        private bool _isSelected = false;
        public bool ISselected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                
                if(value == true) 
                {
                    SelectedCaseID = -1;
                }
                
                OnPropertyChanged("ISselected");
                OnPropertyChanged("ProjectVisibility");
            }
        }

        public Visibility ProjectVisibility 
        { 
            get 
            { 
                if (ISselected) { return Visibility.Visible; }
                else { return Visibility.Collapsed; }
            }
        }

        private int _selected_caseID = -1;
        public int SelectedCaseID { 
            get { return _selected_caseID; } 
            set 
            { 
                _selected_caseID = value;

                if(value > -1) 
                { 
                    _isSelected = false;
                }

                OnPropertyChanged("SelectedCaseID");
            } 
        }
        #endregion

        #region Methods

        private List<Model.Model_Projects> _DB_projects = new();
        public List<Model.Model_Projects> DB_projects
        {
            get
            {
                return _DB_projects;
            }
        }

        public string DB_projects_reload()
        {
            string outy = _AMCore_Socket.send_receive("dataController_csv 0");
            List<string> rowItems = outy.Split("\n").ToList();
            _DB_projects = new();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                

                if (columnItems.Count > 2) 
                {
                    Model.Model_Projects model = new Model.Model_Projects();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.Name = columnItems[1];
                    model.APIName = columnItems[2];

                    _DB_projects.Add(model);
                }
            }

            OnPropertyChanged("DB_projects");
            return outy;
        }

        public string DB_projects_create_new(string Name)
        {
            string buildString = "dataController_createProject " + Name;
            string outy = _AMCore_Socket.send_receive(buildString);
            DB_projects_reload();
            return outy;
        }

        private Model.Model_Projects _selectedProject;

        public Model.Model_Projects SelectedProject
        {
            get { return _selectedProject; }
            set 
            { 
                _selectedProject = value;
                controllerCases.refresh();
                Controller_Selected_Elements.refresh();
                OnPropertyChanged("SelectedProject");
                OnPropertyChanged("Cases");
                OnPropertyChanged("Elements");
            }
        }
        public void SelectProject(int ID) 
        {
            string outy = _AMCore_Socket.send_receive("project_loadID" + ID.ToString());
            if (outy.CompareTo("OK") != 0) return;
            SelectedProject = DataModel(ID);
        }

        public Model.Model_Projects DataModel(int ID) 
        { 
            Model.Model_Projects model = new Model.Model_Projects();

            if(ID == -1) return model;
            string outy = _AMCore_Socket.send_receive("project_getData ");
            fillModel(ref model, outy.Split(",").ToList());

            return model;
        }

        public int save_DataModel(Model.Model_Projects model) 
        {
            int result = 0;

            string csvFormat = model.get_csv();
            string outy = _AMCore_Socket.send_receive("dataController_saveProjectData " + csvFormat);
            if (outy.Contains("Error")) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "AMCore Error", outy, System.Windows.Forms.ToolTipIcon.Error);
                return 1; 
            }

            model.ID = Convert.ToInt32(outy);
            DB_projects_reload();
            MainWindow.notify.ShowBalloonTip(5000, "Project Saved", "Successful", System.Windows.Forms.ToolTipIcon.Info);
            return result;
        }

        private void fillModel(ref Model.Model_Projects model, List<string> dataIn) 
        {
            if (dataIn.Count < 3) return;
            model.ID = Convert.ToInt32(dataIn[0]);
            model.Name = dataIn[1];
            model.APIName = dataIn[2];
        }

        #endregion

        #region other_controllers
        private Controller.Controller_Cases controllerCases;
        public List<Model.Model_Case> Cases
        {
            get 
            { 
                return controllerCases.Cases;
            }
        }

        private Controller.Controller_Selected_Elements Controller_Selected_Elements;
        public List<Model.Model_SelectedElements> Elements
        {
            get { return Controller_Selected_Elements.Elements; }
        }
        #endregion

        #region Handles
        public void Select_project_Handle(object sender, EventArgs e)
        { 
            for(int n1 = 0; n1 < DB_projects.Count(); n1++) 
            {
                if (DB_projects[n1].IsSelected) 
                {
                    SelectedProject = DB_projects[n1];
                    break;
                }
            }
        
        }

        public void selected_treeview_item(object sender, RoutedPropertyChangedEventArgs<object> e) 
        {
   
            TreeView refTreeView = sender as TreeView;
            if (refTreeView.SelectedItem == null) return;

            TreeViewItem refTreeItem = refTreeView.SelectedItem as TreeViewItem;
            if (refTreeItem == null) return;
            if (refTreeItem.Tag == null) return;

            if (refTreeItem.Tag.ToString().ToUpper().CompareTo("PROJECT") == 0) 
            {
                ISselected = true;
            }
            else if (refTreeItem.Tag.ToString().ToUpper().Contains("CASESS")) 
            {
                StackPanel refStack = (StackPanel)refTreeItem.Header;
                Model.Model_Case? model = null;

                for (int n1 = 0; n1 < refStack.Children.Count ; n1++) 
                {
                    if (refStack.Children[n1].GetType().Equals(typeof(TextBlock))) 
                    {
                        if (((TextBlock)refStack.Children[n1]).Tag.GetType().Equals(typeof(Model.Model_Case))) 
                        {
                            model = ((Model.Model_Case)((TextBlock)refStack.Children[n1]).Tag);
                        }
                    }
                }

                if (model is null) return;

                controllerCases.SelectedCase = model;
            }
            else 
            {
                ISselected = false;
            }
            


        }
        #endregion
    }
}
