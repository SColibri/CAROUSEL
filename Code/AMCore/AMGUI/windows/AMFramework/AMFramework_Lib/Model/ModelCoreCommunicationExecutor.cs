using AMFramework_Lib.Core;
using AMFramework_Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework_Lib.Model
{
    public abstract class ModelCoreCommunicationExecutor : Interfaces.CoreCommandExecutor_Interface
    {
        #region Constructor
        protected readonly Core.IAMCore_Comm _coreCommunication;
        protected readonly Interfaces.Model_Interface _modelObject;
        public ModelCoreCommunicationExecutor(ref Core.IAMCore_Comm comm, 
                                              ref Interfaces.Model_Interface ModelObject)
        {
            _modelObjects = new();
            _coreCommunication = comm;
            _modelObject = ModelObject;
            _commandReference = _modelObject.Get_commands().Find(e => e.Executor_Type.Equals(this.GetType()));

        }
        #endregion

        #region Implementation

        public object? Tag { get; set; }
        public string Command_parameters { get; set; } = "";
        public bool IsEnabled { get; set; } = true;
        public IAMCore_Comm CoreCommunication => _coreCommunication;
        public string CoreOutput { get; set; } = "";

        #region Action
        /// <summary>
        /// Check if we don't have null objects before using them. Maybe consider moving this function 
        /// into the implementetions itself.
        /// </summary>
        /// <returns></returns>
        public bool CheckBeforeAction()
        {
            if (!IsEnabled) return false;
            if (_commandReference == null) return false;
            if (_commandReference.Command_instruction.Length <= 1) return false;

            return true;
        }

        /// <summary>
        /// Check if the output is correct.
        /// </summary>
        /// <returns></returns>
        protected bool CheckOutput() 
        {
            if (CoreOutput.Contains("Error")) return false;
            return true;
        }
        /// <summary>
        /// DoActions are implemented using explicit implementations for each case e.g. save
        /// delete, load all, etc. Adding more commands can be done just by adding  a new class.
        /// </summary>
        public abstract void DoAction();
        #endregion
        #endregion

        #region Methods & parameters
        /// <summary>
        /// List of available commands in _modelObject
        /// </summary>
        protected readonly Interfaces.CoreCommand_Interface? _commandReference;

        protected List<Interfaces.Model_Interface> _modelObjects;
        public List<Interfaces.Model_Interface> ModelObjects { get { return _modelObjects ?? (_modelObjects = new List<Interfaces.Model_Interface>()); } }
        public string LoadQuery { get; set; } = "";

        /// <summary>
        /// creates objects of type defined by _modelObject using the Activator function in C# (reflection)
        /// </summary>
        /// <param name="csvTableData"></param>
        protected void Create_ModelObjects(string csvTableData) 
        {
            // clear old data
            _modelObjects.Clear();

            // get row entries
            List<string> rowItems = csvTableData.Split("\n").ToList();
            if (rowItems.Count == 0) return;

            // load data to model
            foreach (var item in rowItems)
            {
                // get values
                List<string> cellItems = item.Split(",").ToList();

                // Create new object of type _modelObject, check for null.
                Interfaces.Model_Interface? tempModel = (Model_Interface)Activator.CreateInstance(_modelObject.GetType());
                if (tempModel == null || cellItems.Count < 2) continue;
                
                // load into newly created model and add to
                tempModel.Load_csv(cellItems);
                _modelObjects.Add(tempModel);
            }
        }

        #endregion
    }
}
