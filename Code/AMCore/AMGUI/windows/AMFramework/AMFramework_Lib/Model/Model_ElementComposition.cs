using AMFramework_Lib.AMSystem.Attributes;

namespace AMFramework_Lib.Model
{
    public class Model_ElementComposition : ModelAbstract
    {

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

        private int _id_case = -1;
        [Order]
        public int IDCase
        {
            get { return _id_case; }
            set
            {
                _id_case = value;
                OnPropertyChanged(nameof(IDCase));
            }
        }

        private int _id_element = -1;
        [Order]
        public int IDElement
        {
            get { return _id_element; }
            set
            {
                _id_element = value;
                OnPropertyChanged(nameof(IDElement));
            }
        }

        private string _typeComposition = "";
        [Order]
        public string TypeComposition
        {
            get { return _typeComposition; }
            set
            {
                _typeComposition = value;
                OnPropertyChanged(nameof(TypeComposition));
            }
        }

        private double _value = -1;
        [Order]
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        #region Other
        private string _ElementName = "";
        public string ElementName
        {
            get { return _ElementName; }
            set
            {
                _ElementName = value;
                OnPropertyChanged(nameof(ElementName));
            }
        }

        private bool _isReferenceElement = false;
        public bool IsReferenceElement
        {
            get { return _isReferenceElement; }
            set
            {
                _isReferenceElement = value;
                OnPropertyChanged(nameof(IsReferenceElement));
            }
        }


        #endregion

        #region Additional
        private string _stringValue = "";
        /// <summary>
        /// String value is used for templated objects and ranged intervaled values
        /// </summary>
        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value;
                OnPropertyChanged(nameof(StringValue));
            }
        }
        #endregion


        #region Interfaces
        public override IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            return ModelAbstract.Get_parameters<Model_ElementComposition>();
        }

        public override string Get_Table_Name()
        {
            return "ElementComposition";
        }

        public override string Get_Scripting_ClassName()
        {
            return "ElementComposition";
        }
        #endregion
    }
}
