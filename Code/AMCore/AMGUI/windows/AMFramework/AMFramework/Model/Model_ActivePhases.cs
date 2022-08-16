using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework.Interfaces;
using AMFramework.AMSystem.Attributes;

namespace AMFramework.Model
{
    public class Model_ActivePhases : ModelAbstract
    {

        #region Constructor
        public Model_ActivePhases() 
        { 
        
        }

        private void Add_allCommands(Core.IAMCore_Comm comm) 
        {
            Type thisType = this.GetType();

            ModelCoreCommand MCC = new(ref comm);
            MCC.ObjectType = this.GetType();
            MCC.Command_instruction = "project_active_phases_save";
        }

        #endregion

        private int _id = -1;
        [Order]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        private int _idProject = -1;
        [Order]
        public int IDProject
        {
            get { return _idProject; }
            set
            {
                _idProject = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _idPhase = -1;
        [Order]
        public int IDPhase
        {
            get { return _idPhase; }
            set
            {
                _idPhase = value;
                OnPropertyChanged("IDPhase");
            }
        }

        #region Other
        private string _phaseName = "";
        public string PhaseName 
        { 
            get { return _phaseName; } 
            set 
            {
                _phaseName = value;
                OnPropertyChanged("PhaseName");
            }
        }
        #endregion

        #region Implementation
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ActivePhases>();
        }
   
        public override string Get_save_command()
        {
            return "project_active_phases_save";
        }

        public override string Get_load_command()
        {
            return "project_active_phases_loadID";
        }

        public override string Get_load_command_table(Model_Interface.SEARCH findType)
        {
            return "project_active_phases_load_IDProject";
        }

        public override string Get_delete_command()
        {
            return "project_active_phases_delete";
        }

        public override string Get_Table_Name()
        {
            return "ActivePhases";
        }
        #endregion
    }
}
