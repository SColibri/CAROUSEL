using AMControls.Charts.Implementations;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.ContextMenu;
using AMFramework.Components.Charting.Interfaces;
using AMFramework_Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting.DataPlot
{
    internal class DataPlot_ScheilSimulation : DataPlotBase
    {
        public enum DataOptionEnum
        {
            PhaseFraction,
            solidification_temperature,
            IDCase
        }

        #region Fields
        /// <summary>
        /// Data options
        /// </summary>
        private List<string> _DataOptions = new() { "PhaseFraction", "solidification_temperature", "IDCase" };

        /// <summary>
        /// 
        /// </summary>
        private string _xDataName = "solidification_temperature";
        
        /// <summary>
        /// 
        /// </summary>
        private string _yDataName = "PhaseFraction";
        
        /// <summary>
        /// 
        /// </summary>
        private string _zDataName = "";

        #endregion

        #region Properties
        /// <summary>
        /// DataPlotBase implementation
        /// Table or view from which data should be extracted
        /// </summary>
        protected override string TableViewName { get; set; } = "v_scheil_solidification";

        /// <summary>
        /// DataPlotBase implementation
        /// Column name from table where data should be used as label
        /// </summary>
        protected override string LabelColumnName { get; set; } = "phaseName";
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="comm"></param>
        public DataPlot_ScheilSimulation(IAMCore_Comm comm) : base(comm)
        { 
            // Empty
        }
        #endregion
        public override string Name => "Scheil simulations";

        public override string SeriesName { get; set; } = "Empty";

        public override List<string> DataOptions => _DataOptions;

        public override string X_Data_Name => _xDataName;

        public override string Y_Data_Name => _yDataName;

        public override string Z_Data_Name => _zDataName;

        public override void X_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _xDataName = _DataOptions[option];
        }

        public override void Y_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _yDataName = _DataOptions[option];
        }

        public override void Z_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _zDataName = _DataOptions[option];
        }

        #region Methods
        /// <summary>
        /// Load data from database
        /// </summary>
        protected override void LoadData()
        {
            string Query_T = $"SELECT * FROM {TableViewName}";
            if (WhereClause.Length > 0) Query_T += " WHERE " + WhereClause + "";

            string RawData_T = _comm.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();
            if (RowData.Count == 0) return;

            foreach (var item in RowData)
            {
                List<string> cells = item.Trim().Split(",").ToList();
                CellData.Add(cells);
            }

        }
        #endregion

        
    }
}
