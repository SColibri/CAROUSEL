using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_Cases : INotifyPropertyChanged
    {

        #region Cons_Des
        private Core.AMCore_Socket _AMCore_Socket;
        private int _idProject = -1;
        private int _selectedIDCase = -1;
        public Controller_Cases(Core.AMCore_Socket socket, 
                                int IDProject,
                                int IDCase)
        {
            _AMCore_Socket = socket;
            _idProject = IDProject;
            _selectedIDCase = IDCase;
            load_data();
        }
        public Controller_Cases()
        {

        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data
        private Model.Model_Case _selectedCase = new();

        List<Model.Model_Case> _cases = new();

        public List<Model.Model_Case> Cases
        {
            get { return _cases; }
            set
            {
                _cases = value;
                OnPropertyChanged("Cases");
            }
        }
        #endregion

        #region Methods

        private string load_data() 
        {
            string outy = _AMCore_Socket.send_receive("dataController_csv 1");
            List<string> rowItems = outy.Split("\n").ToList();
            _cases = new();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 8)
                {
                    Model.Model_Case model = new Model.Model_Case();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDProject = Convert.ToInt32(columnItems[1]);
                    model.IDGroup = Convert.ToInt32(columnItems[2]);
                    model.Name = columnItems[3];
                    model.Script = columnItems[4];
                    model.Date = columnItems[5];
                    model.PosX = Convert.ToInt32(columnItems[6]);
                    model.PosY = Convert.ToInt32(columnItems[7]);
                    model.PosZ = Convert.ToInt32(columnItems[8]);

                    _cases.Add(model);
                }
            }

            OnPropertyChanged("Cases");
            return outy;
        }

        #endregion

    }
}
