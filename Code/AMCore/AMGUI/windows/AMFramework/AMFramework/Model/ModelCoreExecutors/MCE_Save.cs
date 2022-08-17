using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    public class MCE_Save:Model.ModelCoreCommunicationExecutor
    {
        public MCE_Save(ref Core.IAMCore_Comm comm,
                        ref Interfaces.Model_Interface ModelObject) : base(ref comm, ref ModelObject)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = _modelObject.Get_csv();

            if (_commandReference == null) { CoreOutput = "Error: DoAction, loading by id project is not available for this model!"; return; }
            CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
            _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("ID") == 0)?.SetValue(_modelObject, Convert.ToInt32(CoreOutput));
        }
        #endregion
    }
}
