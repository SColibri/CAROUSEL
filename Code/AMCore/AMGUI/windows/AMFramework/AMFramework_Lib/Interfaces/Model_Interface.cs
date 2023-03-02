using System.ComponentModel;

namespace AMFramework_Lib.Interfaces
{
    public interface Model_Interface : INotifyPropertyChanged
    {
        /// <summary>
        /// Obtains the parameter list used in the database corresponding to the model.
        /// The abstract class implements how to extract these parameters(objects) from the class
        /// in order to modify/consult them as specified by the csv sequence of the data as
        /// stored in the database.
        /// 
        /// For this to work, we need to mark the order of the parameters by implementing
        /// OrderAttribute in the model, this is done by putting [Order] above the public
        /// getter. With help of reflection you can get these marked parameters in the
        /// specified order (first to last only).
        /// 
        /// The Main advantage of this is that now the abstract class can handle the
        /// load csv into model, get csv format from model.
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<System.Reflection.PropertyInfo> Get_parameter_list();

        /// <summary>
        /// gets csv formated data of the model, this is used 
        /// for saving current model into the database
        /// </summary>
        /// <returns></returns>
        public string Get_csv();
        protected string Get_csv(IOrderedEnumerable<System.Reflection.PropertyInfo> ParamInput);

        /// <summary>
        /// load data into model using csv formatted data, if
        /// data is not of correct type or it does not have the correct
        /// dimension, it will return -1.
        /// </summary>
        /// <param name="csvData"></param>
        /// <returns></returns>
        public int Load_csv(List<string> DataRaw);

        /// <summary>
        /// returns a list of commands that are available for this class
        /// </summary>
        /// <returns></returns>
        public List<Interfaces.CoreCommand_Interface> Get_commands();

        /// <summary>
        /// Returns the name of the table whre data related to this object is stored
        /// </summary>
        /// <returns></returns>
        public string Get_Table_Name();

        /// <summary>
        /// This is the class name used in the scripting languague, e.g. from the projects table
        /// the project object is called Project
        /// </summary>
        /// <returns></returns>
        public string Get_Scripting_ClassName();
    }
}
