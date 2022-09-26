using AMControls.Charts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting.Interfaces
{
    public interface IDataPlot
    {
        public string Name { get; }
        public string SeriesName { get; set; }   

        public List<string> DataOptions { get; }
        public List<IDataPoint> DataPoints { get; }

        public void X_Data_Option(int option);
        public void Y_Data_Option(int option);
        public void Z_Data_Option(int option);

        public string X_Data_Name { get; }
        public string Y_Data_Name { get; }
        public string Z_Data_Name { get; }
    }
}
