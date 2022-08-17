using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AMFramework.Model
{
    /// <summary>
    /// Model controller is the default way to manage a model object (Load, save, other)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ModelController<T> where T : Interfaces.Model_Interface
    {
        private Core.IAMCore_Comm _coreCommunication;
        private Interfaces.Model_Interface _model;
        public ModelController(ref Core.IAMCore_Comm comm) 
        { 
            _coreCommunication = comm;
            ModelObject = (T)Activator.CreateInstance(typeof(T));
        }

        public ModelController(ref Core.IAMCore_Comm comm, T modely)
        {
            _coreCommunication = comm;
            ModelObject = modely;
        }

        public T ModelObject 
        { 
            get { return (T)_model; } 
            set 
            { 
                _model = (Interfaces.Model_Interface)value;

                SaveAction = new(ref _coreCommunication, ref _model);
                DeleteAction = new(ref _coreCommunication, ref _model);
                LoadByIDAction = new(ref _coreCommunication, ref _model);
                LoadByNameAction = new(ref _coreCommunication, ref _model);
            }
        }

        // Default Model actions 
        public Model.ModelCoreExecutors.MCE_Save SaveAction { get; set; }
        public Model.ModelCoreExecutors.MCE_Delete DeleteAction { get; set; }
        public Model.ModelCoreExecutors.MCE_LoadByID LoadByIDAction { get; set; }
        public Model.ModelCoreExecutors.MCE_LoadByName LoadByNameAction { get; set; }

        #region LoadMany_Model_Controllers
        //  Default list loading

        /// <summary>
        /// Returns a list of model controllers ALL
        /// </summary>
        /// <returns></returns>
        public static List<ModelController<T>> LoadAll(ref Core.IAMCore_Comm comm) 
        {
            Interfaces.Model_Interface tempRef = (Interfaces.Model_Interface)Activator.CreateInstance(typeof(T));
            Model.ModelCoreExecutors.MCE_LoadALL tempMCE = new(ref comm, ref tempRef);
            tempMCE.DoAction();

            return ExtractMCE(comm, tempMCE);
        }

        /// <summary>
        /// Returns a list of model controllers IDProject
        /// </summary>
        /// <returns></returns>
        public static List<ModelController<T>> LoadIDProject(ref Core.IAMCore_Comm comm, int IDproject)
        {
            Interfaces.Model_Interface tempRef = (Interfaces.Model_Interface)Activator.CreateInstance(typeof(T));

            if (tempRef == null) return new List<ModelController<T>>();
            tempRef.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDProject") == 0)?.SetValue(tempRef, Convert.ToInt32(IDproject));

            Model.ModelCoreExecutors.MCE_LoadByIDProject tempMCE = new(ref comm, ref tempRef);
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


    }
}
