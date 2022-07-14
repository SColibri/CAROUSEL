using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_ActivePhasesConfiguration : Interfaces.Model_Interface
    {
        private int _id = -1;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        private int _idProject = -1;
        public int IDProject
        {
            get { return _idProject; }
            set
            {
                _idProject = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _startTemp = -1;
        public int StartTemp
        {
            get { return _startTemp; }
            set
            {
                _startTemp = value;
                OnPropertyChanged("StartTemp");
            }
        }

        private int _endTemp = -1;
        public int EndTemp
        {
            get { return _endTemp; }
            set
            {
                _endTemp = value;
                OnPropertyChanged("EndTemp");
            }
        }

        private int _stepSize = -1;
        public int StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged("StepSize");
            }
        }
        public string Get_csv()
        {
            string outy = ID + "," + IDProject + "," + StartTemp + "," + EndTemp + "," + StepSize;
            return outy;
        }

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
