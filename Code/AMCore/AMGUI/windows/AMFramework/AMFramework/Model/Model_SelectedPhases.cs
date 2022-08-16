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
    public class Model_SelectedPhases : ModelAbstract
    {
        private int _ID = -1;
        [Order]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        private int _IDCase = -1;
        [Order]
        public int IDCase
        {
            get { return _IDCase; }
            set
            {
                _IDCase = value;
                OnPropertyChanged("IDCase");
            }
        }

        private int _IDPhase = -1;
        [Order]
        public int IDPhase
        {
            get { return _IDPhase; }
            set
            {
                _IDPhase = value;
                OnPropertyChanged("IDPhase");
            }
        }

        private string _PhaseName = "";
        [Order]
        public string PhaseName
        {
            get { return _PhaseName; }
            set
            {
                _PhaseName = value;
                OnPropertyChanged("PhaseName");
            }
        }

        #region Other_properties
        
        private bool _isDependentPhase = false;
        public bool IsDependentPhase
        {
            get { return _isDependentPhase; }
            set
            {
                _isDependentPhase = value;
                OnPropertyChanged("IsDependentPhase");
            }
        }
        #endregion
        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_SelectedPhases>();
        }

        public override string Get_save_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_load_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_load_command_table(Model_Interface.SEARCH findType)
        {
            throw new NotImplementedException();
        }

        public override string Get_delete_command()
        {
            throw new NotImplementedException();
        }

        public override string Get_Table_Name()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
