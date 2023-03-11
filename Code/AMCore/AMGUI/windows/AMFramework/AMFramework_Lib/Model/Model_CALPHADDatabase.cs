using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
{
    /// <summary>
    /// Model_CALPHADDatabase class that stores the path to the databases that were used for this project, paths should be relative to
    /// the working directory and avoid using absolute paths.
    /// </summary>
    public class Model_CALPHADDatabase : ModelAbstract
    {

        #region Fields
        /// <summary>
        /// Identifier
        /// </summary>
        private int _id = -1;

        /// <summary>
        /// Project identifier 
        /// </summary>
        private int _idProject = -1;

        /// <summary>
        /// Path to Thermodynamic database
        /// </summary>
        private string _thermodynamicDatabase = string.Empty;

        /// <summary>
        /// Path to mobility database
        /// </summary>
        private string _mobilityDatabase = string.Empty;

        /// <summary>
        /// Path to physical database
        /// </summary>
        private string _physicalDatabase = string.Empty;

        #endregion

        #region Properties
        /// <summary>
        /// Get/set Identifier
        /// </summary>
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

        /// <summary>
        /// Get/Set Project identifier 
        /// </summary>
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

        /// <summary>
        /// Path to Thermodynamic database
        /// </summary>
        [Order]
        public string ThermodynamicDatabase
        {
            get { return _thermodynamicDatabase; }
            set
            {
                _thermodynamicDatabase = value;
                OnPropertyChanged(nameof(ThermodynamicDatabase));
            }
        }

        /// <summary>
        /// Path to mobility database
        /// </summary>
        [Order]
        public string MobilityDatabase
        {
            get { return _mobilityDatabase; }
            set
            {
                _mobilityDatabase = value;
                OnPropertyChanged(nameof(MobilityDatabase));
            }
        }

        /// <summary>
        /// Path to physical database
        /// </summary>
        [Order]
        public string PhysicalDatabase
        {
            get { return _physicalDatabase; }
            set
            {
                _physicalDatabase = value;
                OnPropertyChanged(nameof(PhysicalDatabase));
            }
        }
        #endregion




        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_CALPHADDatabase>();
        }

        public override string Get_Table_Name()
        {
            return "CALPHADDatabase";
        }

        public override string Get_Scripting_ClassName()
        {
            return "CALPHADDatabase";
        }
        #endregion

    }
}
