using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    public class MCE_LoadByName:Model.ModelCoreCommunicationExecutor
    {
        public MCE_LoadByName(ref Core.IAMCore_Comm comm,
                       ref Interfaces.Model_Interface ModelObject) : base(ref comm, ref ModelObject)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("Name") == 0)?.GetValue(_modelObject)?.ToString() ?? "";
            CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
            _modelObject.Load_csv(CoreOutput.Split(",").ToList());
        }
        #endregion

    }
}
