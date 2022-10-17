using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Controllers
{
    public abstract class Controller_Abstract_Models<T>: IModelController where T : Model_Interface
    {
        protected Core.IAMCore_Comm _comm;
        public Controller_Abstract_Models(Core.IAMCore_Comm comm) 
        { 
            _comm = comm;
            _mCObject = new(ref comm);
        }

        public Controller_Abstract_Models(Core.IAMCore_Comm comm, ModelController<T> modelMC)
        {
            _comm = comm;
            _mCObject = modelMC;
        }


        private ModelController<T> _mCObject;
        public ModelController<T> MCObject
        {
            get { return _mCObject; }
            set
            {
                _mCObject = value;
                OnPropertyChanged("MCObject");
            }
        }

        public Model_Interface Model_Object { get { return MCObject.Model_Object; } }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
