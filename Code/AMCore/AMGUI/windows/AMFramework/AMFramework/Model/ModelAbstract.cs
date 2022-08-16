using AMFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model
{
    public abstract class ModelAbstract : Interfaces.Model_Interface
    {
        #region Reflection
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
        public event PropertyChangedEventHandler PropertyChanged;

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
        public virtual string Get_csv() 
        {
            return Get_csv(Get_parameter_list());
        }
        public virtual int Load_csv(List<string> DataRaw) 
        {
            var parameterList = Get_parameter_list();

            try
            { Abstract_load(parameterList, DataRaw); }
            catch (Exceptions.DatabaseDataConversion_Exception e)
            { return 1; }

            return 0;
        }
        public abstract string Get_save_command();
        public abstract string Get_load_command();
        public abstract string Get_load_command_table(Interfaces.Model_Interface.SEARCH findType);
        public abstract string Get_delete_command();
        public abstract string Get_Table_Name();
        public virtual IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list() 
        { 
            throw new NotImplementedException();
        }


        #endregion

        #region LoadData
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
                        if (!int.TryParse(rawData[Index],out tempValue)) 
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
                        if (!bool.TryParse(rawData[Index],out tempValue)) 
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
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
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
        public static Interfaces.CoreCommand_Interface? Get_command(object ModelObject, ModelCoreCommand.Commands commands) 
        {
            var item = _commandList.Find(e => e.ObjectType.Equals(ModelObject.GetType()) & e.Command_Type == (int)commands);
            var TVar = Add_command;
            Core.IAMCore_Comm TSock = new Core.AMCore_libHandle(""); ;
            TVar(new ModelCoreCommand(ref TSock));

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
