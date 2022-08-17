using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    internal class MCE_Delete:Model.ModelCoreCommunicationExecutor
    {
        public MCE_Delete(ref Core.IAMCore_Comm comm,
                        ref Interfaces.Model_Interface ModelObject) : base(ref comm, ref ModelObject)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            //TODO: add checks of neccesary
            Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
            CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
            _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.SetValue(_modelObject, -1);
        }
        #endregion
    }
}
