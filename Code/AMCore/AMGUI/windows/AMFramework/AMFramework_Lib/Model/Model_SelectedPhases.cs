using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AMFramework_Lib.Interfaces;
using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
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
                OnPropertyChanged(nameof(ID));
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
                OnPropertyChanged(nameof(IDCase));
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
                OnPropertyChanged(nameof(IDPhase));
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
                OnPropertyChanged(nameof(PhaseName));
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
                OnPropertyChanged(nameof(IsDependentPhase));
            }
        }
        #endregion
        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_SelectedPhases>();
        }
        public override string Get_Table_Name()
        {
            return "SelectedPhases";
        }

        public override string Get_Scripting_ClassName()
        {
            return "SelectedPhase";
        }
        #endregion
    }
}
