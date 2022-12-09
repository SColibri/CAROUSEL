using AMFramework_Lib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMFramework.Views.Tables
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl, INotifyPropertyChanged
    {
        private IAMCore_Comm _socket;
        public TableView(IAMCore_Comm socket)
        {
            InitializeComponent();
            _socket = socket;
            _tableNames = new();
            Load_table_names();

            DataContext = this;
        }

        private List<string> _tableNames;
        public List<string> TableNames { get { return _tableNames; } }
        private void Load_table_names()
        {
            _tableNames.Clear();

            string Query_T = "SELECT name FROM sqlite_master where type='view' AND sql LIKE \"%vd_%\";";
            string RawData_T = _socket.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();
            if (RowData.Count == 0) return;

            foreach (string row in RowData)
            {
                if (row.Length <= 1) continue;
                _tableNames.Add(row.Replace(",", "").Trim());
            }

            OnPropertyChanged(nameof(TableNames));
        }

        private string _selectedTable = "";
        private List<string> _columnNames = new();
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null) return;
            _selectedTable = ((ComboBox)sender).SelectedItem.ToString();
            Fill_Table();
        }

        private DataTable _tableData = new();
        public DataView DView 
        { 
            get { return _tableData.DefaultView; } 
        }

        public DataTable TableData
        {
            get { return _tableData; } 
        }
        private void Fill_Table() 
        {
            Get_ColumnNames();
            //MainGrid.Columns.Clear();
            _tableData.Columns.Clear();

            foreach (var item in _columnNames)
            {
                if (item.Length <= 1) continue;
                _tableData.Columns.Add(new DataColumn(item));
                //MainGrid.Columns.Add(new DataGridTextColumn() { Header = item });
            }

            string Query_T = "SELECT * FROM " + _selectedTable;
            string RawData_T = _socket.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Trim('\n').Split("\n").ToList();
            if (RowData.Count == 0) return;

            foreach (string row in RowData)
            {
                List<string> Cells = row.Trim(',').Split(",").ToList();
                if (Cells.Count < _tableData.Columns.Count) continue;
                DataRow dR = _tableData.NewRow();
                dR.ItemArray = Cells.ToArray();

                _tableData.Rows.Add(dR);
            }

           
            OnPropertyChanged(nameof(DView));
        }

        private void Get_ColumnNames()
        {
            _columnNames.Clear();
            // select * from pragma_table_info('vd_HeatTreatment_Plot') as tblInfo
            string Query_T = "select name from pragma_table_info('" + _selectedTable + "') as tblInfo";
            string RawData_T = _socket.run_lua_command("database_table_custom_query", Query_T);
            List<string> RowData = RawData_T.Split("\n").ToList();

            _columnNames.Clear();
            foreach (var item in RowData)
            {
                _columnNames.Add(item.Replace(",", "").Trim());
            }
        }

        #region INotifyPropertyChanged_Interface
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        
    }
}
