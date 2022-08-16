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
    public class Model_ActivePhasesConfiguration : ModelAbstract
    {
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

        private int _startTemp = -1;
        [Order]
        public int StartTemp
        {
            get { return _startTemp; }
            set
            {
                _startTemp = value;
                OnPropertyChanged("StartTemp");
            }
        }

        private int _endTemp = -1;
        [Order]
        public int EndTemp
        {
            get { return _endTemp; }
            set
            {
                _endTemp = value;
                OnPropertyChanged("EndTemp");
            }
        }

        private int _stepSize = -1;
        [Order]
        public int StepSize
        {
            get { return _stepSize; }
            set
            {
                _stepSize = value;
                OnPropertyChanged("StepSize");
            }
        }


        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ActivePhasesConfiguration>();
        }

        public override string Get_save_command()
        {
            return "project_active_phases_configuration_save";
        }

        public override string Get_load_command()
        {
            return "project_active_phases_configuration_loadID";
        }

        public override string Get_load_command_table(Model_Interface.SEARCH findType)
        {
            return "project_active_phases_configuration_load_IDProject";
        }

        public override string Get_delete_command()
        {
            return "project_active_phases_configuration_delete";
        }

        public override string Get_Table_Name()
        {
            return "ActivePhases_Configuration";
        }
        #endregion
    }
}
