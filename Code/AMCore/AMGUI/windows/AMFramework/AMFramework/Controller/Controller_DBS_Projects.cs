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

        private List<string> _DB_projects = new();
        public List<string> DB_projects
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
                if (columnItems.Count > 1) _DB_projects.Add(columnItems[1]);
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

        public Model.Model_Projects DataModel(int ID) 
        { 
            Model.Model_Projects model = new Model.Model_Projects();

            if(ID == -1) return model;
            string outy = _AMCore_Socket.send_receive("dataController_getProjectData " + ID);
            fillModel(ref model, outy.Split(",").ToList());

            return model;
        }

        public int save_DataModel(Model.Model_Projects model) 
        {
            int result = 1;

            //dataController_saveProjectData
            string csvFormat = model.get_csv();
            string outy = _AMCore_Socket.send_receive("dataController_saveProjectData " + csvFormat);

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
    }
}
