﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.ModelCoreExecutors
{
    internal class MCE_LoadByQuery:Model.ModelCoreCommunicationExecutor
    {
        private string _query;
        public MCE_LoadByQuery(ref Core.IAMCore_Comm comm,
                    ref Interfaces.Model_Interface ModelObject,
                    int CommandType,
                    string query) : base(ref comm, ref ModelObject, CommandType)
        { _query = query; }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = _query;
            CoreOutput = _coreCommunication.run_lua_command("database_table_custom_query ", Command_parameters);
            Create_ModelObjects(CoreOutput);
        }
        #endregion
    }
}
