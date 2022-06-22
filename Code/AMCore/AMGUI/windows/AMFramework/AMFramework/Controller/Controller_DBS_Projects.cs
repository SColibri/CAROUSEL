using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_DBS_Projects : INotifyPropertyChanged
    {
        #region Socket
        private Core.AMCore_Socket _AMCore_Socket;
        public Controller_DBS_Projects(Core.AMCore_Socket socket) 
        {
            _AMCore_Socket = socket;
            controllerCases = new(socket, -1, -1);
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                controllerCases = new(_AMCore_Socket, _selectedProject.ID, -1);
                OnPropertyChanged("SelectedProject");
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
        public Controller.Controller_Cases controllerCases;
        #endregion
    }
}
