using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    public class MCE_LoadByIDProject : Model.ModelCoreCommunicationExecutor
    {
        public MCE_LoadByIDProject(ref Core.IAMCore_Comm comm,
                        ref Interfaces.Model_Interface ModelObject,
                        Type ExecutorType) : base(ref comm, ref ModelObject, ExecutorType)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDProject") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
            CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
            Create_ModelObjects(CoreOutput);
        }
        #endregion
    }
}
