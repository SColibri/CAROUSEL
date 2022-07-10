using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    internal class Controller_Element : INotifyPropertyChanged
    {
        #region Socket
        private Core.IAMCore_Comm _AMCore_Socket;
        private Controller_DBS_Projects _projectController;
        public Controller_Element(ref Core.IAMCore_Comm socket, Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _projectController = projectController;
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
        public void Refresh() 
        {
            _elements = get_elements_list();
            OnPropertyChanged("Elements");
        }
        #endregion

        #region Data
        private List<Model.Model_Element> _elements = new();
        public List<Model.Model_Element> Elements { get { return _elements; } } 
        public List<Model.Model_Element> get_elements_from_project(int IDProject)
        {
            List<Model.Model_Element> composition = new();
            string Query = "database_table_custom_query SELECT Element.ID as IDE, Element.Name, SelectedElements.* FROM SelectedElements INNER JOIN Element ON Element.ID=SelectedElements.IDElement WHERE IDProject = " + IDProject;
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        public List<Model.Model_Element> get_elements_list()
        {
            List<Model.Model_Element> composition = new();
            string Query = "database_table_custom_query SELECT Element.* FROM Element";
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }

        public List<Model.Model_Element> get_available_elements_in_database()
        {
            // get available element names from database
            List<Model.Model_Element> composition = new();
            string Query = "matcalc_database_elementNames";
            string outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> elementList = outCommand.Split("\n").ToList();

            // get related ID's from database by given name, missing ID's are ignored
            if (elementList.Count == 0) return composition;
            Query = "database_table_custom_query SELECT * FROM Element WHERE Name = '" + elementList[0] + "' " ;
            for (int i = 0; i < elementList.Count; i++) 
            {
                Query += " OR Name = '" + elementList[i].Replace("\r","") + "' "; 
            }
            outCommand = _AMCore_Socket.run_lua_command(Query,"");
            List<string> rowItems = outCommand.Split("\n").ToList();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();
                if (columnItems.Count < 2) continue;
                composition.Add(fillModel(columnItems));
            }

            return composition;
        }
        private Model.Model_Element fillModel(List<string> DataRaw)
        {
            if (DataRaw.Count < 2) throw new Exception("Error: Element RawData is wrong");

            Model.Model_Element modely = new();
            modely.ID = Convert.ToInt32(DataRaw[0]);
            modely.Name = DataRaw[1];

            return modely;
        }
        #endregion
    }
}
