using AMFramework_Lib.Interfaces;
using System.ComponentModel;

namespace AMFramework_Lib.Model.Controllers
{
    public abstract class Controller_Abstract_Models<T> : IModelController where T : Model_Interface
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
                OnPropertyChanged(nameof(MCObject));
            }
        }

        public Model_Interface Model_Object { get { return MCObject.Model_Object; } }

        #region Static methods


        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
