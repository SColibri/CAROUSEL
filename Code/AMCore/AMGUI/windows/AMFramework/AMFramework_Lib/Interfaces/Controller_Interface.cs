using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Interfaces
{
    internal interface Controller_Interface: INotifyPropertyChanged
    {
        public void Refresh();

    }
}
