using System.ComponentModel;

namespace AMFramework_Lib.Model
{
    /// <summary>
    /// Main class that handles the load and save implementation for all models, as also some
    /// other base functionalities.
    /// </summary>
    public abstract class ModelAbstract : Interfaces.Model_Interface
    {
        #region Reflection
        /// <summary>
        /// Extract all parameters marked as model parameters in order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameters<T>()
        {
            IOrderedEnumerable<System.Reflection.PropertyInfo> properties = from property in typeof(T).GetProperties()
                                                                            where Attribute.IsDefined(property, typeof(AMSystem.Attributes.OrderAttribute))
                                                                            orderby ((AMSystem.Attributes.OrderAttribute)property.GetCustomAttributes(typeof(AMSystem.Attributes.OrderAttribute), false).Single()).Order
                                                                            select property;

            return properties;
        }
        #endregion

        #region INotifyPropertyChanged_Interface
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region implementation_model_interface
        public string Get_csv(IOrderedEnumerable<System.Reflection.PropertyInfo> ParamInput)
        {
            string csv_out = "";
            int Index = 0;
            foreach (var item in ParamInput)
            {

                if (Index > 0) csv_out += ",";
                csv_out += item.GetValue(this)?.ToString()?.Replace(" ", "#");

                Index++;
            }

            return csv_out;
        }

        #endregion

        #region Model_interface
        /// <summary>
        /// Returns csv data string oof the model with marked parameters
        /// </summary>
        /// <returns></returns>
        public virtual string Get_csv()
        {
            return Get_csv(Get_parameter_list());
        }

        /// <summary>
        /// Loads string list into model marked parameters
        /// </summary>
        /// <param name="DataRaw"></param>
        /// <returns></returns>
        public virtual int Load_csv(List<string> DataRaw)
        {
            var parameterList = Get_parameter_list();

            try
            { Abstract_load(parameterList, DataRaw); }
            catch (Exceptions.DatabaseDataConversion_Exception e)
            { return 1; }

            return 0;
        }

        /// <summary>
        /// Returns the table name where the model data is stored
        /// </summary>
        /// <returns></returns>
        public abstract string Get_Table_Name();

        /// <summary>
        /// Returns the ordered parameters of the model
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// For scripting porpuses we can change the name of the model, and instead of using the tablename
        /// we can declare our own class name. For now, most of them have the same name as the table where
        /// the data is stored.
        /// </summary>
        /// <returns></returns>
        public abstract string Get_Scripting_ClassName();

        #endregion

        #region LoadData
        /// <summary>
        /// Function that loads data from string list. This function uses reflection to access all
        /// parameters and parameter order. Data has to be in specific order (from csv format).
        /// </summary>
        /// <param name="ParamInput"></param>
        /// <param name="rawData"></param>
        /// <exception cref="Exceptions.DatabaseDataConversion_Exception"></exception>
        protected void Abstract_load(IOrderedEnumerable<System.Reflection.PropertyInfo> ParamInput,
                                     List<string> rawData)
        {

            if (rawData.Count >= ParamInput.Count())
            {
                int Index = 0;
                foreach (var item in ParamInput)
                {

                    if (item.PropertyType.Equals(typeof(int)))
                    {
                        int tempValue = 0;
                        if (!int.TryParse(rawData[Index], out tempValue))
                        { throw new Exceptions.DatabaseDataConversion_Exception(item.Name + ", input from database: " + rawData[Index] + " is not convertible to int! "); }

                        item.SetValue(this, tempValue);
                    }
                    else if (item.PropertyType.Equals(typeof(double)))
                    {
                        double tempValue = 0;
                        if (!double.TryParse(rawData[Index], out tempValue))
                        { throw new Exceptions.DatabaseDataConversion_Exception(item.Name + ", input from database: " + rawData[Index] + " is not convertible to double! "); }

                        item.SetValue(this, tempValue);
                    }
                    else if (item.PropertyType.Equals(typeof(bool)))
                    {
                        bool tempValue = false;
                        if (!bool.TryParse(rawData[Index], out tempValue))
                        { throw new Exceptions.DatabaseDataConversion_Exception(item.Name + ", input from database: " + rawData[Index] + " is not convertible to bool! "); }

                        item.SetValue(this, tempValue);
                    }
                    else if (item.PropertyType.Equals(typeof(string)))
                    {
                        item.SetValue(this, rawData[Index]);
                    }

                    Index++;
                }
            }

        }

        #endregion

        #region parameters
        private bool _isSelected = false;
        /// <summary>
        /// Returns true if object is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private bool _isVisible = true;
        /// <summary>
        /// Returns true if object is visible
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _isActive = false;
        /// <summary>
        /// Return true if model is active or enabled
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        private bool _hasChanged = false;
        /// <summary>
        /// Returns true if content in the model has changed
        /// </summary>
        public bool HasChanged
        {
            get { return _hasChanged; }
            set
            {
                _hasChanged = value;
                OnPropertyChanged(nameof(HasChanged));
            }
        }
        #endregion

        #region Commands

        private static List<Interfaces.CoreCommand_Interface> _commandList = new();
        /// <summary>
        /// List of avavilable commands that this model handles. Uses implementations of the
        /// CoreCommand_interface, which is compatible with the ICommand used in wpf
        /// </summary>
        public static List<Interfaces.CoreCommand_Interface> CommandList { get { return _commandList; } }
        /// <summary>
        /// Add a new command for the current model.
        /// </summary>
        /// <param name="NewCommand"></param>
        public static void Add_command(Interfaces.CoreCommand_Interface NewCommand)
        {
            var item = _commandList.Find(e => e.Command_instruction.Equals(NewCommand.Command_instruction) == true);
            if (item != null) return;

            _commandList.Add(NewCommand);
        }
        /// <summary>
        /// If it exists, returns the command object specified by the enum Commands in the ModelCoreCommands class
        /// </summary>
        /// <param name="ModelObject"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static Interfaces.CoreCommand_Interface? Get_command<T>(object ModelObject)
        {
            var item = _commandList.Find(e => e.ObjectType.Equals(ModelObject.GetType()) & e.Executor_Type.Equals(typeof(T)));
            return item;
        }

        /// <summary>
        /// returns all avavilable commands for this class
        /// </summary>
        /// <returns></returns>
        public List<Interfaces.CoreCommand_Interface> Get_commands()
        {
            return _commandList.FindAll(e => e.ObjectType.Equals(this.GetType()));
        }
        #endregion
    }
}
