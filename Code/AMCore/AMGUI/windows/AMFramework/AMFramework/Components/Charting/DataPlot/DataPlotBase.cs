using AMControls.Charts.Implementations;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.ContextMenu;
using AMFramework.Components.Charting.Interfaces;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting.DataPlot
{
    internal abstract class DataPlotBase : IDataPlot
    {
        #region Fields
        /// <summary>
        /// Communication object
        /// </summary>
        protected IAMCore_Comm _comm;

        /// <summary>
        /// Table from which data should be extracted
        /// </summary>
        protected string _tableViewName = string.Empty;

        /// <summary>
        /// Data columns, name of columns in database
        /// </summary>
        protected List<string> _ColumnNames = new();

        #endregion

        #region Properties
        /// <summary>
        /// Table or view from which data should be extracted
        /// </summary>
        protected abstract string TableViewName {get; set;}

        /// <summary>
        /// Column name from table where data should be used as label
        /// </summary>
        protected abstract string LabelColumnName { get; set; }

        /// <summary>
        /// SQL query used for filtering
        /// </summary>
        public string WhereClause { get; set; } = string.Empty;

        /// <summary>
        /// Column name from table where data should be used as label
        /// </summary>
        protected List<List<string>> CellData { get; set; } = new();

        /// <summary>
        /// Returns data points
        /// </summary>
        /// <returns></returns>
        public List<IDataPoint> GetDatapoints()
        {

            if (CellData.Count == 0)
            {
                LoadData();
            }

            return GetDataFromExisiting();
        }

        // IDataPlot Interface

        /// <summary>
        /// 
        /// </summary>
        public abstract string Name { get; }
        public abstract string SeriesName { get; set; }
        public abstract List<string> DataOptions { get; }
        public List<IDataPoint> DataPoints
        {
            get
            {
                return GetDatapoints();
            }
        }
        public abstract string X_Data_Name { get; }
        public abstract string Y_Data_Name { get; }
        public abstract string Z_Data_Name { get; }

        public abstract void X_Data_Option(int option);
        public abstract void Y_Data_Option(int option);
        public abstract void Z_Data_Option(int option);
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comm">core communication</param>
        public DataPlotBase(IAMCore_Comm comm)
        {
            _comm = comm;
            Get_ColumnNames();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads data into _cellData
        /// </summary>
        protected abstract void LoadData();

        /// <summary>
        /// Get column names
        /// </summary>
        protected void Get_ColumnNames()
        {
            string Query_T = "select name from pragma_table_info('" + TableViewName + "') as tblInfo";
            string RawData_T = _comm.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();

            _ColumnNames.Clear();
            foreach (var item in RowData)
            {
                _ColumnNames.Add(item.Replace(",", "").Trim());
            }
        }

        /// <summary>
        /// Get index of column using column name
        /// </summary>
        /// <param name="cName"></param>
        /// <returns></returns>
        protected int Get_ColumnIndex(string cName)
        {
            if (cName.Length == 0) return -1;

            int Index = 0;
            foreach (var item in _ColumnNames)
            {
                if (string.Equals(cName, item, StringComparison.OrdinalIgnoreCase)) { return Index; }
                Index++;
            }

            return -1;
        }

        protected List<IDataPoint> GetDataFromExisiting()
        {
            List<IDataPoint> Result = new();

            int xIndex = Get_ColumnIndex(X_Data_Name);
            int yIndex = Get_ColumnIndex(Y_Data_Name);
            int zIndex = Get_ColumnIndex(Z_Data_Name);
            int labelIndex = Get_ColumnIndex(LabelColumnName);

            // Check for invalid label index
            if (labelIndex == -1) 
            {
                Controller_Global.MainControl?.Show_Notification("Invalid index", $"DataPlotBase: Label index [{labelIndex}] is not valid, please check if table structure contains the column [{LabelColumnName}]",
                    (int)FontAwesome.WPF.FontAwesomeIcon.Warning, null, null, null);
                return Result;
            }

            foreach (List<string> cells in CellData)
            {
                if (cells.Count <= Math.Max(Math.Max(xIndex, yIndex), zIndex)) continue;

                IDataPoint tempObject = new DataPoint();

                if (xIndex > -1) { tempObject.X = Convert.ToDouble(cells[xIndex]); }
                if (yIndex > -1) { tempObject.Y = Convert.ToDouble(cells[yIndex]); }
                if (zIndex > -1) { tempObject.Z = Convert.ToDouble(cells[zIndex]); }

                tempObject.ContextMenu = new DataPoint_ProjectViewContextMenu(cells, cells[labelIndex]);
                tempObject.Label = cells[labelIndex];
                tempObject.Tag = cells;

                Result.Add(tempObject);
            }

            return Result;
        }
        #endregion
    }
}
