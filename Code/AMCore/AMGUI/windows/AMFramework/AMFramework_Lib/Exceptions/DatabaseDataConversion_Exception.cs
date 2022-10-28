using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Exceptions
{
    public class DatabaseDataConversion_Exception:SystemException
    {
        public DatabaseDataConversion_Exception() 
        { 
        }

        public DatabaseDataConversion_Exception(string? message):base(message)
        {
        }

        public DatabaseDataConversion_Exception(string? message, Exception e):base(message, e)
        {
        }

        private string _databaseError = "";
        public string DatabaseError { get { return _databaseError; } set { _databaseError = value; } }
    }
}
