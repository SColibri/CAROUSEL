using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Interfaces
{
    internal interface Model_Interface : INotifyPropertyChanged
    {
        public string Get_csv();
    }
}
