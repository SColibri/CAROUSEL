using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFRamework_Components.Charts
{
    internal class Axis
    {
        public string Name = "";
        public double Min = 0;
        public double Max = 0;
        public double Interval = 0;
        public bool IsVisible = true;
        public double Range 
        { 
            get 
            { 
                return Max - Min;
            } 
        }

    }
}
