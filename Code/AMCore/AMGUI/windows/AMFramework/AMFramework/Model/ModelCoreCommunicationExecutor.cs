using AMFramework.Core;
using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Model
{
    public abstract class ModelCoreCommunicationExecutor : Interfaces.CoreCommandExecutor_Interface
    {
        #region Constructor
        protected readonly Core.IAMCore_Comm _coreCommunication;
        protected readonly Interfaces.Model_Interface _modelObject;
        public ModelCoreCommunicationExecutor(ref Core.IAMCore_Comm comm, 
                                              ref Interfaces.Model_Interface ModelObject, 
                                              int CommandType)
        {
            _coreCommunication = comm;
            _modelObject = ModelObject;
            _commandType = (Commands)CommandType;
            _commandReference = _modelObject.Get_commands().Find(e => e.Command_Type == CommandType);
        }

        public enum Commands
        {
            NONE,
            SAVE_ID,
            DELTE_ID,
            LOAD_ID,
            LOAD_BYNAME,
            LOAD_IDPROJECT,
            LOAD_ALL,
            LOAD_BYQUERY
        }
        #endregion

        #region Implementation

        public object? Tag { get; set; }
        public string Command_parameters { get; set; } = "";
        public bool IsEnabled { get; set; } = true;
        public IAMCore_Comm CoreCommunication => _coreCommunication;
        public string CoreOutput { get; set; } = "";

        #region Command
        private ICommand? _coreCommand;
        /// <summary>
        /// Icommand that runs the currtent stored command
        /// </summary>
        public ICommand CoreCommand
        {
            get
            {
                if (_coreCommand == null)
                {
                    _coreCommand = new Controller.RelayCommand(
                        param => this.DoAction(),
                        param => this.CheckBeforeAction()
                    );
                }

                return _coreCommand;
            }
        }

        protected bool CheckBeforeAction()
        {
            if (!IsEnabled) return false;
            if (_commandReference == null) return false;
            if (_commandReference.Command_instruction.Length <= 1) return false;

            return true;
        }

        protected bool CheckOutput() 
        {
            if (CoreOutput.Contains("Error")) return false;
            return true;
        }

        public abstract void DoAction()
        {
            if (!CheckBeforeAction()) return;

            // we leave the switch statement a
            switch (_commandType)
            {
                case Commands.SAVE_ID:
                    Command_parameters = _modelObject.Get_csv();
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.SetValue(_modelObject, Convert.ToInt64(CoreOutput));
                    break;
                case Commands.DELTE_ID:
                    Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.SetValue(_modelObject, Convert.ToInt64(CoreOutput));
                    break;
                case Commands.LOAD_ID:
                    Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    _modelObject.Load_csv(CoreOutput.Split(",").ToList());
                    break;
                case Commands.LOAD_BYNAME:
                    Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("Name") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    _modelObject.Load_csv(CoreOutput.Split(",").ToList());
                    break;
                case Commands.LOAD_IDPROJECT:
                    Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDProject") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    Create_ModelObjects(CoreOutput);
                    break;
                case Commands.LOAD_ALL:
                    throw new NotImplementedException("Load all is not yet implemented");
                    Command_parameters = "";
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    Create_ModelObjects(CoreOutput);
                    break;
                case Commands.LOAD_BYQUERY:
                    if (LoadQuery.Length == 0) return;
                    Command_parameters = LoadQuery;
                    CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
                    Create_ModelObjects(CoreOutput);
                    break;
                default:
                    throw new Exception("Error: Model core command executor DoAction returns a non-valid selected option!");
            }

        }
        #endregion
        #endregion

        #region Methods & parameters
        /// <summary>
        /// List of available commands in _modelObject
        /// </summary>
        protected readonly Interfaces.CoreCommand_Interface? _commandReference;
        protected Commands _commandType = Commands.NONE;

        protected List<Interfaces.Model_Interface> _modelObjects;
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
                if (tempModel == null) continue;
                
                // load into newly created model and add to
                tempModel.Load_csv(cellItems);
                _modelObjects.Add(tempModel);
            }
        }

        #endregion
    }
}
