using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    internal class MCE_LoadALL:Model.ModelCoreCommunicationExecutor
    {
        public MCE_LoadALL(ref Core.IAMCore_Comm comm,
                        ref Interfaces.Model_Interface ModelObject,
                        int CommandType) : base(ref comm, ref ModelObject, CommandType)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = " SELECT * FROM " + _modelObject.Get_Table_Name();
            CoreOutput = _coreCommunication.run_lua_command("database_table_custom_query ", Command_parameters);
            Create_ModelObjects(CoreOutput);
        }
        #endregion

    }
}
