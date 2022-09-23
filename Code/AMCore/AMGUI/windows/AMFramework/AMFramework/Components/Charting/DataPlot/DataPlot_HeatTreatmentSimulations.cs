﻿using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.ContextMenu;
using AMFramework.Components.Charting.Interfaces;
using AMFramework.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Components.Charting.DataPlot
{
    internal class DataPlot_HeatTreatmentSimulations : IDataPlot
    {
        private string _name = "Heat treatment simulation";
        private List<string> _DataOptions = new() { "PhaseFraction", "NumberDensity", "MeanRadius" };
        private List<string> _ColumnNames = new();
        private string _tableViewName = "vd_HeatTreatment_Plot";
        private string _xDataName = "";
        private string _yDataName = "";
        private string _zDataName = "";
        private string _whereClause = "";
        private Core.IAMCore_Comm _socket;

        public DataPlot_HeatTreatmentSimulations(Core.IAMCore_Comm socket) 
        { 
            _socket = socket;
            Get_ColumnNames();
        }

        public string Name => _name;
        public List<string> DataOptions => _DataOptions;

        public List<IDataPoint> DataPoints 
        { 
            get 
            {
                return Get_datapoints();
            }
        }

        public string X_Data_Name => _xDataName;
        public string Y_Data_Name => _yDataName;
        public string Z_Data_Name => _zDataName;

        public void X_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _xDataName = _DataOptions[option];
        }

        public void Y_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _yDataName = _DataOptions[option];
        }

        public void Z_Data_Option(int option)
        {
            if (_DataOptions.Count <= option) return;
            _zDataName = _DataOptions[option];
        }

        public void Set_where_clause(string clause) 
        { 
            _whereClause = clause;
        }

        private void Get_ColumnNames() 
        {
            // select * from pragma_table_info('vd_HeatTreatment_Plot') as tblInfo
            string Query_T = "select name from pragma_table_info('" + _tableViewName + "') as tblInfo";
            string RawData_T = _socket.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();

            _ColumnNames.Clear();
            foreach (var item in RowData)
            {
                _ColumnNames.Add(item.Replace(",", "").Trim());
            }
        }

        private int Get_ColumnIndex(string cName) 
        {
            if (cName.Length == 0) return -1;

            int Index = 0;
            foreach (var item in _ColumnNames)
            {
                if(item.CompareTo(cName) == 0) { return Index; }
                Index++;
            }

            return -1;
        }

        private List<IDataPoint> Get_datapoints() 
        {
            List<IDataPoint> Result = new();

            int xIndex = Get_ColumnIndex(_xDataName);
            int yIndex = Get_ColumnIndex(_yDataName);
            int zIndex = Get_ColumnIndex(_zDataName);

            string Query_T = "SELECT * FROM vd_HeatTreatment_Plot";
            if (_whereClause.Length > 0) Query_T += " WHERE " + _whereClause + "";

            string RawData_T = _socket.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();
            if (RowData.Count == 0) return Result;

            foreach (var item in RowData)
            {
                IDataPoint tempObject = new DataPoint();
                List<string> cells = item.Split(",").ToList();
                if (cells.Count < 11) continue;

                if (xIndex > -1) { tempObject.X = Convert.ToDouble(cells[xIndex]); }
                if (yIndex > -1) { tempObject.Y = Convert.ToDouble(cells[yIndex]); }
                if (zIndex > -1) { tempObject.Z = Convert.ToDouble(cells[zIndex]); }

                tempObject.ContextMenu = new DataPoint_ProjectViewContextMenu(cells, cells[9]);
                tempObject.Label = cells[9];

                Result.Add(tempObject);
            }

            return Result;
        }

    }
}