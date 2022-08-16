using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Interfaces
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
        /// Returns the lua command that saves the model and has
        /// as input a stirng in csv format. The model does not get saved if
        /// the input parameters do not have the same data size. When succesful
        /// it returns the ID of the element, otherwise -1.
        /// </summary>
        /// <returns></returns>
        public string Get_save_command();

        /// <summary>
        /// Returns the load command for loadByID that has input of integer value,
        /// corresponding to the ID of the element, this is, for a single item
        /// referenced by its ID. This command only loads one Item and returns it
        /// in a csv formatted way.
        /// </summary>
        /// <returns></returns>
        public string Get_load_command();

        /// <summary>
        /// How data should be searched for loading data into multiple models
        /// </summary>
        public enum SEARCH
        {
            BY_ID,
            BY_NAME,
            OTHER
        }
        /// <summary>
        /// Command that loads multiple entries of certain model type, this
        /// can be specified by parent ID, name or other. If not implemented
        /// it will retunr and empty string.
        /// </summary>
        /// <param name="findType"></param>
        /// <returns></returns>
        public string Get_load_command_table(SEARCH findType);

        /// <summary>
        /// Returns the delete command for the model that has as input the ID of the model.
        /// This command returns 'OK' when the command was succesful
        /// </summary>
        /// <returns></returns>
        public string Get_delete_command();

        /// <summary>
        /// Returns the name of the table whre data related to this object is stored
        /// </summary>
        /// <returns></returns>
        public string Get_Table_Name();
    }
}
