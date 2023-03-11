using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using System.ComponentModel;

namespace AMFramework_Lib.Model
{
    /// <summary>
    /// Model controller is the default way to manage a model object (Load, save, other)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelController<T> : IModelController where T : Model_Interface
    {
        private IAMCore_Comm _coreCommunication;
        private Model_Interface _model;
        public ModelController(ref IAMCore_Comm comm)
        {
            _coreCommunication = comm;

            // Create instance of template using the activator class
            var refT = Activator.CreateInstance(typeof(T));

            if (refT != null)
            {
                _model = (T)refT;
                ModelObject = (T)_model;
            }
            else throw new Exception("ModelController: Instance was not created because it was not found!");

        }

        public ModelController(ref IAMCore_Comm comm, T modely)
        {
            _coreCommunication = comm;
            _model = modely;
            ModelObject = (T)_model;
        }

        /// <summary>
        /// Returns the data model object of generic type T
        /// </summary>
        public T ModelObject
        {
            get
            {
                return (T)_model;
            }
            set
            {
                _model = value;

                // Add default commands to current data model
                SaveAction = new(ref _coreCommunication, ref _model);
                DeleteAction = new(ref _coreCommunication, ref _model);
                LoadByIDAction = new(ref _coreCommunication, ref _model);
                LoadByNameAction = new(ref _coreCommunication, ref _model);
            }
        }

        // Interface implementation
        public Model_Interface Model_Object
        {
            get
            {
                return _model;
            }
        }

        // Default Model actions 
        public ModelCoreExecutors.MCE_Save? SaveAction { get; set; }
        public ModelCoreExecutors.MCE_Delete? DeleteAction { get; set; }
        public ModelCoreExecutors.MCE_LoadByID? LoadByIDAction { get; set; }
        public ModelCoreExecutors.MCE_LoadByName? LoadByNameAction { get; set; }

        #region LoadMany_Model_Controllers
        //  Default list loading

        /// <summary>
        /// Returns a list of model controllers ALL
        /// </summary>
        /// <returns></returns>
        public static List<ModelController<T>> LoadAll(ref Core.IAMCore_Comm comm)
        {
            var refT = Activator.CreateInstance(typeof(T));
            if (refT == null) return new();

            Model_Interface tempRef = (Model_Interface)refT;

            ModelCoreExecutors.MCE_LoadALL tempMCE = new(ref comm, ref tempRef);
            tempMCE.DoAction();

            return ExtractMCE(comm, tempMCE);
        }

        /// <summary>
        /// Returns a list of model controllers IDProject, this only applies for models that have a IDProject property
        /// </summary>
        /// <returns></returns>
        public static List<ModelController<T>> LoadIDProject(ref Core.IAMCore_Comm comm, int IDproject)
        {
            var refT = Activator.CreateInstance(typeof(T));
            if (refT == null) return new();

            Model_Interface tempRef = (Model_Interface)refT;

            if (tempRef == null) return new List<ModelController<T>>();
            tempRef.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDProject") == 0)?.SetValue(tempRef, Convert.ToInt32(IDproject));

            ModelCoreExecutors.MCE_LoadByIDProject tempMCE = new(ref comm, ref tempRef);
            tempMCE.DoAction();

            return ExtractMCE(comm, tempMCE);
        }

        /// <summary>
        /// Returns a list of model controllers IDCase. Note: not all models load by IDCase
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="IDCase"></param>
        /// <returns></returns>
        public static List<ModelController<T>> LoadIDCase(ref Core.IAMCore_Comm comm, int IDCase)
        {
            var refT = Activator.CreateInstance(typeof(T));
            if (refT == null) return new();

            Model_Interface tempRef = (Model_Interface)refT;

            if (tempRef == null) return new List<ModelController<T>>();
            tempRef.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDCase") == 0)?.SetValue(tempRef, Convert.ToInt32(IDCase));

            ModelCoreExecutors.MCE_LoadByIDCase tempMCE = new(ref comm, ref tempRef);
            tempMCE.DoAction();

            return ExtractMCE(comm, tempMCE);
        }

        /// <summary>
        /// Returns a list of model controllers based on a SQL query command
        /// </summary>
        /// <returns></returns>
        public static List<ModelController<T>> LoadByQuery(ref Core.IAMCore_Comm comm, string query)
        {
            var refT = Activator.CreateInstance(typeof(T));
            if (refT == null) return new();

            Model_Interface tempRef = (Model_Interface)refT;

            if (tempRef == null) return new List<ModelController<T>>();
            ModelCoreExecutors.MCE_LoadByQuery tempMCE = new(ref comm, ref tempRef, query);
            tempMCE.DoAction();

            return ExtractMCE(comm, tempMCE);
        }

        /// <summary>
        /// From the MCE object list, we create a list of model controllers for those model objects
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="MCEObject"></param>
        /// <returns></returns>
        private static List<ModelController<T>> ExtractMCE(Core.IAMCore_Comm comm, ModelCoreCommunicationExecutor MCEObject)
        {
            List<ModelController<T>> listObjects = new();

            foreach (var item in MCEObject.ModelObjects)
            {
                listObjects.Add(new(ref comm, (T)item));
            }

            return listObjects;

        }
        #endregion

        #region INotifyPropertyChanged_Interface
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
