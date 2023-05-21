using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMFramework.Components.Charting.DataPlot;
using AMFramework.Components.Charting.Interfaces;
using AMFramework_Lib.Core;
using Catel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AMFramework.Views.Project_Map
{
    /// <summary>
    /// Interaction logic for ProjectPhaseFractionView.xaml
    /// </summary>
    public partial class ProjectPhaseFractionView : UserControl
    {
        public ProjectPhaseFractionView()
        {
            InitializeComponent();
        }

        #region Fields
        /// <summary>
        /// Project ID
        /// </summary>
        private int _idProject = -1;

        /// <summary>
        /// Data to be used for plotting
        /// </summary>
        private IDataPlot _dataPlot;

        private Dictionary<int, IDataPoint[]> _sortedData = new();
        #endregion

        #region Properties
        /// <summary>
        /// Core communication
        /// </summary>
        public IAMCore_Comm Comm { get; set; }

        /// <summary>
        /// Project ID
        /// </summary>
        public int IDProject 
        {
            get => _idProject;
            set 
            { 
                _idProject = value;
                LoadData();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load data from database
        /// </summary>
        private void LoadData() 
        {
            if (Comm != null) 
            {
                _dataPlot = new DataPlot_ScheilSimulation(Comm);
                _dataPlot.X_Data_Option((int)DataPlot_ScheilSimulation.DataOptionEnum.PhaseFraction);
                _dataPlot.Y_Data_Option((int)DataPlot_ScheilSimulation.DataOptionEnum.solidification_temperature);
                _dataPlot.Z_Data_Option((int)DataPlot_ScheilSimulation.DataOptionEnum.IDCase);
                ((DataPlot_ScheilSimulation)_dataPlot).WhereClause = $" IDProject = {IDProject} ";
                _dataPlot.SeriesName = "All available data";

                RefreshPlot();
            }
        }

        public void RefreshPlot() 
        {
            UpdateSortedValues();
            SetSpyder();
        }

        private void SetSpyder()
        {
            string[] phases = GetActivePhaseList();
            int[] IDCases = GetCaseList();

            if (phases.Length == 0 || IDCases.Length == 0) return;

            Random random = new();
            SpyderSeries[] sbsArray = new SpyderSeries[IDCases.Length];

            for (int i = 0; i < IDCases.Length; i++)
            {
                sbsArray[i] = new();
                sbsArray[i].Label = $"Case: {IDCases[i]}";

                _sortedData[IDCases[i]].ForEach(e => { sbsArray[i].Add_DataPoint(e); });
                sbsArray[i].ColorSeries = Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255));
            }

            double maxValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Max() ?? 0;
            double minValue = sbsArray.SelectMany(e => e.DataPoints.Select(s => s.X))?.Min() ?? 0;

            spyderPlot.ClearAxis();
            for (int i = 0; i < phases.Length; i++)
            {
                double localMaxValue = _dataPlot.DataPoints.Where(e => e.Label == phases[i]).Select(e => e.X).Max();
                LinearAxe lAxe = new()
                {
                    Name = phases[i],
                    MinValue = minValue,
                    MaxValue = maxValue * 0.2 < localMaxValue ? maxValue : localMaxValue > 0 ? localMaxValue * 2 : maxValue,
                    Ticks = 5,
                };
                spyderPlot.Add_axis(lAxe);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                spyderPlot.Clear_Series();
                sbsArray.Where(e => e.DataPoints.Count > 0).ForEach(e => spyderPlot.Add_series(e));
                spyderPlot.InvalidateVisual();
            }));
        }

        private void UpdateSortedValues() 
        {
            string[] phases = GetActivePhaseList();
            int[] IDCases = GetCaseList();

            foreach (int idCase in IDCases)
            {
                IDataPoint[] phaseValues = new IDataPoint[phases.Length];
                IDataPoint[] pointsFound = GetPoints(idCase);

                int count = 0;
                foreach (string phase in phases)
                {
                    IDataPoint? phasePoint = pointsFound.FirstOrDefault(e => e.Label == phase);

                    if (phasePoint != null)
                    {
                        phaseValues[count] = phasePoint;
                    }
                    else
                    {
                        phaseValues[count] = new DataPoint()
                        {
                            X = 0,
                            Y = 0,
                            Z = idCase,
                        };
                    }

                    count++;
                }


                if (_sortedData.ContainsKey(idCase))
                {
                    _sortedData[idCase] = phaseValues;
                }
                else
                {
                    _sortedData.Add(idCase, phaseValues);
                }
            }
        }

        private string[] GetActivePhaseList() 
        {
            string[] result = new string[0];

            if (_dataPlot != null) 
            {
                result = _dataPlot.DataPoints.Select(e => e.Label).Distinct().ToArray();
            }

            return result;
        }

        private int[] GetCaseList()
        {
            int[] result = new int[0];

            if (_dataPlot != null)
            {
                result = _dataPlot.DataPoints.Select(e => (int)e.Z).Distinct().ToArray();
            }

            return result;
        }

        private IDataPoint[] GetPoints(int IDCase) 
        {
            IDataPoint[] result = new IDataPoint[0];

            if (_dataPlot != null)
            {
                result = _dataPlot.DataPoints.Where(e => (int)e.Z == IDCase).ToArray();
            }

            return result;
        }




        #endregion
    }
}
