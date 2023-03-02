namespace AMFramework_Lib.Model.ModelCoreExecutors
{
    public class MCE_LoadByIDCase : Model.ModelCoreCommunicationExecutor
    {
        public MCE_LoadByIDCase(ref Core.IAMCore_Comm comm,
                                ref Interfaces.Model_Interface ModelObject) : base(ref comm, ref ModelObject)
        { }

        #region Implementation Abstract class
        public override void DoAction()
        {
            Command_parameters = _modelObject.Get_parameter_list().ToList().Find(e => e.Name.CompareTo("IDCase") == 0)?.GetValue(_modelObject)?.ToString() ?? "";

            if (_commandReference == null) { CoreOutput = "Error: DoAction, loading by id project is not available for this model!"; return; }
            CoreOutput = _coreCommunication.run_lua_command(_commandReference.Command_instruction, Command_parameters);
            Create_ModelObjects(CoreOutput);
        }
        #endregion
    }
}
