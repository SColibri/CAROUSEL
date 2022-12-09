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
    public class Model_ActivePhases : ModelAbstract
    {

        #region Constructor
        public Model_ActivePhases() 
        { 
        
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
                OnPropertyChanged(nameof(ID));
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
                OnPropertyChanged(nameof(IDProject));
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
                OnPropertyChanged(nameof(IDPhase));
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
                OnPropertyChanged(nameof(PhaseName));
            }
        }
        #endregion

        #region Implementation
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ActivePhases>();
        }

        public override string Get_Table_Name()
        {
            return "ActivePhases";
        }

        public override string Get_Scripting_ClassName()
        {
            return "ActivePhases";
        }
        #endregion
    }
}
