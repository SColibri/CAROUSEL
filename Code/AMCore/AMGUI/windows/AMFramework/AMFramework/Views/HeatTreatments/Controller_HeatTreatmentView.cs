using AMControls.Charts;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMControls.Charts.Scatter;
using AMFramework.Controller;
using AMFramework.Core;
using AMFramework.Model;
using AMFramework.Structures;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AMFramework.Views.HeatTreatments
{
    public class Controller_HeatTreatmentView : Controller.ControllerAbstract
    {
        public Controller_HeatTreatmentView(IAMCore_Comm comm, Model.Model_Case caseModel) : base(comm) 
        {
            CaseModel = caseModel;
            HeatTreatmentPlot = new();

            IAxes xaxis = new LinearAxe("x Axis");
            xaxis.MaxValue = 500;
            xaxis.MinValue = 0;

            IAxes yaxis = new LinearAxe("y Axis");
            yaxis.MaxValue = 700;
            yaxis.MinValue = 0;

            HeatTreatmentPlot.Set_xAxis(xaxis);
            HeatTreatmentPlot.Set_yAxis(yaxis);
        }

        #region Properties
        private Model.Model_Case _caseModel;
        public Model.Model_Case CaseModel 
        {
            get { return _caseModel; }
            set 
            {
                _caseModel = value;
                OnPropertyChanged(nameof(CaseModel));
            }
        }

        private ModelController<Model.Model_HeatTreatment>? _selectedHeatTreatment;
        public ModelController<Model.Model_HeatTreatment>? SelectedHeatTreatment
        {
            get { return _selectedHeatTreatment; }
            set 
            {
                _selectedHeatTreatment = value;
                Update_Plot();
                OnPropertyChanged(nameof(SelectedHeatTreatment));
            }
        }
        #endregion

        #region Pages

        #region HeatTreatmentPlot

        private ScatterPlot _heatTreatmentPlot;
        /// <summary>
        /// This displays the heat treatment segments in a plot
        /// </summary>
        public ScatterPlot HeatTreatmentPlot 
        {
            get { return _heatTreatmentPlot; }
            set 
            {
                _heatTreatmentPlot = value;
                OnPropertyChanged(nameof(HeatTreatmentPlot));
            }
        }

        private void Update_Plot() 
        {
            HeatTreatmentPlot.Clear_Series();

            foreach (var item in CaseModel.HeatTreatments)
            {
                var rangeObjects = Plot_GetRangedObjects(item);
                if (rangeObjects.Item1.Count == 0) break;

                int lineNumber = rangeObjects.Item1[0].Values.Count;

                for (int i = 0; i < lineNumber; i++)
                {
                    IDataSeries dataSeries;
                    if (item == SelectedHeatTreatment)
                    {
                        dataSeries = new ScatterSeries() { ColorSeries = Colors.Red, Label = item.ModelObject.Name + "_" + i };
                    }
                    else
                    {
                        dataSeries = new ScatterSeries() { ColorSeries = Colors.DarkGray, Label = item.ModelObject.Name + "_" + i };
                    }

                    double currentTime = 0;
                    int index = -1;
                    foreach (var segments in rangeObjects.Item1)
                    {
                        // The first index belongs to the start temperature in heat treatment
                        if (index > -1) 
                        {
                            if (item.ModelObject.HeatTreatmentSegment[index].ModelObject.SelectedModeType == Model_HeatTreatmentSegment.ModeType.TIME_INTERVAL)
                            {
                                currentTime += rangeObjects.Item2[index].Values[i];
                            }
                            else
                            {
                                currentTime += segments.Values[i] / rangeObjects.Item2[index].Values[i];
                            }
                        }

                        dataSeries.Add_DataPoint(new DataPoint() { X = currentTime, Y = segments.Values[i] });

                        index += 1;
                    }

                    HeatTreatmentPlot.Add_series(dataSeries);
                }
              
            }

            

        }

        private List<Struct_AMRange> Plot_Get_TemperatureUniform_Rages(ModelController<Model_HeatTreatment> ht) 
        {
            List<Struct_AMRange> result = new();
            
            // Get all ranges for the first temperature
            Struct_AMRange startRangeObj = new();
            if (startRangeObj.Add(ht.ModelObject.TemplatedStartTemperature) == 1) return result;
            result.Add(startRangeObj);

            // Get all ranges from segments
            int MaxValue = startRangeObj.Values.Count;
            foreach (var segment in ht.ModelObject.HeatTreatmentSegment) 
            {
                Struct_AMRange rangeObj = new();
                result.Add(rangeObj);

                int actionResult = rangeObj.Add(segment.ModelObject.TemplatedEndTemperature);
                if (actionResult == 1) continue;
                if (MaxValue < rangeObj.Values.Count) MaxValue = rangeObj.Values.Count;
            }

            // Make uniform
            Plot_SetUniformRanges(MaxValue ,result);

            return result;
        }

        private Tuple<List<Struct_AMRange>, List<Struct_AMRange>> Plot_GetRangedObjects(ModelController<Model_HeatTreatment> ht) 
        {
            List<Struct_AMRange> result_Temperatures = Plot_Get_TemperatureUniform_Rages(ht);
            List<Struct_AMRange> result_Rates = new();

            if (result_Temperatures.Count == 0) return new(new(),new()); // oh la la, nice new new new functon xp really expressive
            int MaxValue = result_Temperatures[0].Values.Count;
            foreach (var segment in ht.ModelObject.HeatTreatmentSegment)
            {
                Struct_AMRange rangeObj = new();
                result_Rates.Add(rangeObj);

                int actionResult = rangeObj.Add(segment.ModelObject.TemplateValue);
                if (actionResult == 1) continue;
                
                if (MaxValue < rangeObj.Values.Count) MaxValue = rangeObj.Values.Count;                
            }
            Plot_SetUniformRanges(MaxValue, result_Rates);

            return new(result_Temperatures, result_Rates);
        }

        private void Plot_SetUniformRanges(int MaxValue, List<Struct_AMRange> doOnList) 
        {
            foreach (var item in doOnList)
            {
                if (item.Values.Count == 0) item.Values.Add(0);
                while (item.Values.Count < MaxValue)
                {
                    // Just add the last value and repeate it until we have a uniform set
                    item.Values.Add(item.Values[item.Values.Count - 1]);
                }
            }
        }


        #endregion

        #endregion

        #region Commands

        #region HeatTreatment
        #region Add_HT
        private ICommand _addHeatTreatment;
        public ICommand AddHeatTreatment
        {
            get
            {
                if (_addHeatTreatment == null)
                {
                    _addHeatTreatment = new RelayCommand(
                        param => this.AddHeatTreatment_Action(),
                        param => this.AddHeatTreatment_Check()
                    );
                }
                return _addHeatTreatment;
            }
        }

        private void AddHeatTreatment_Action()
        {
            List<ModelController<Model_HeatTreatment>> newList = new();

            foreach (var item in CaseModel.HeatTreatments)
            {
                newList.Add(item);
            }
            newList.Add(new(ref _comm));

            CaseModel.HeatTreatments = newList;
            SelectedHeatTreatment = newList.Last();
        }

        private bool AddHeatTreatment_Check()
        {
            return true;
        }
        #endregion

        #region Select_HT
        private ICommand _selectHeatTreatment;
        public ICommand SelectHeatTreatment
        {
            get
            {
                if (_selectHeatTreatment == null)
                {
                    _selectHeatTreatment = new RelayCommand(
                        param => this.SelectHeatTreatment_Action(param),
                        param => this.SelectHeatTreatment_Check()
                    );
                }
                return _selectHeatTreatment;
            }
        }

        private void SelectHeatTreatment_Action(object sender)
        {
            if (sender == null) return;
            if (!sender.GetType().Equals(typeof(ModelController<Model_HeatTreatment>))) return;

            ModelController<Model_HeatTreatment>? htController = sender as ModelController<Model_HeatTreatment>;
            if (htController == null) return; // Silly error where it says that this can be null

            foreach (var item in CaseModel.HeatTreatments)
            {
                item.ModelObject.IsSelected = false;
            }

            htController.ModelObject.IsSelected = true;
            SelectedHeatTreatment = htController;
        }

        private bool SelectHeatTreatment_Check()
        {
            return true;
        }
        #endregion

        #region remove_HT
        private ICommand _removeHeatTreatment;
        public ICommand RemoveHeatTreatment
        {
            get
            {
                if (_removeHeatTreatment == null)
                {
                    _removeHeatTreatment = new RelayCommand(
                        param => this.RemoveHeatTreatment_Action(param),
                        param => this.RemoveHeatTreatment_Check()
                    );
                }
                return _removeHeatTreatment;
            }
        }

        private void RemoveHeatTreatment_Action(object sender)
        {
            if (sender == null) return;
            if (!sender.GetType().Equals(typeof(ModelController<Model_HeatTreatment>))) return;

            ModelController<Model_HeatTreatment>? htController = sender as ModelController<Model_HeatTreatment>;
            if (htController == null) return; // Silly error where it says that this can be null

            List<ModelController<Model_HeatTreatment>> newList = new();

            foreach (var item in CaseModel.HeatTreatments)
            {
                if (item == htController) continue;
                newList.Add(item);
            }
            newList.Add(new(ref _comm));

            CaseModel.HeatTreatments = newList;
        }

        private bool RemoveHeatTreatment_Check()
        {
            return true;
        }
        #endregion
        #endregion

        #region Heat TreatmentSegments
        #region Add_Segment

        private ICommand _addHeatTreatmentSegment;
        public ICommand AddHeatTreatmentSegment
        {
            get
            {
                if (_addHeatTreatmentSegment == null)
                {
                    _addHeatTreatmentSegment = new RelayCommand(
                        param => this.AddHeatTreatmentSegment_Action(),
                        param => this.AddHeatTreatmentSegment_Check()
                    );
                }
                return _addHeatTreatmentSegment;
            }
        }

        private void AddHeatTreatmentSegment_Action()
        {
            if (SelectedHeatTreatment == null) 
            {
                Show_No_Project_Selected_Notification();
                return;
            }

            List<ModelController<Model_HeatTreatmentSegment>> newList = new();

            foreach (var item in SelectedHeatTreatment.ModelObject.HeatTreatmentSegment)
            {
                newList.Add(item);
            }
            newList.Add(new(ref _comm));
            newList.Last().ModelObject.PropertyChanged += HeatTreatmentSegments_PropertyChangedHandle;

            SelectedHeatTreatment.ModelObject.HeatTreatmentSegment = newList;
        }

        private bool AddHeatTreatmentSegment_Check()
        {
            return true;
        }

        private void HeatTreatmentSegments_PropertyChangedHandle(object? sender, PropertyChangedEventArgs e) 
        {
            Update_Plot();
        }

        #endregion

        #region Remove_Segment

        private ICommand _removeHeatTreatmentSegment;
        public ICommand RemoveHeatTreatmentSegment
        {
            get
            {
                if (_removeHeatTreatmentSegment == null)
                {
                    _removeHeatTreatmentSegment = new RelayCommand(
                        param => this.RemoveHeatTreatmentSegment_Action(param),
                        param => this.RemoveHeatTreatmentSegment_Check()
                    );
                }
                return _removeHeatTreatmentSegment;
            }
        }

        private void RemoveHeatTreatmentSegment_Action(object sender)
        {
            if (SelectedHeatTreatment == null) 
            {
                Show_No_Project_Selected_Notification();
                return;
            }

            if (sender == null) return;
            if (!sender.GetType().Equals(typeof(ModelController<Model_HeatTreatmentSegment>))) return;

            ModelController<Model_HeatTreatmentSegment>? hts = sender as ModelController<Model_HeatTreatmentSegment>;
            if (hts == null) return;
            hts.ModelObject.PropertyChanged -= HeatTreatmentSegments_PropertyChangedHandle;

            List<ModelController<Model_HeatTreatmentSegment>> newList = new();
            foreach (var item in SelectedHeatTreatment.ModelObject.HeatTreatmentSegment) 
            {
                if (item == hts) continue;
                newList.Add(item);
            }

            SelectedHeatTreatment.ModelObject.HeatTreatmentSegment = newList;
        }

        private bool RemoveHeatTreatmentSegment_Check()
        {
            return true;
        }
        #endregion
        #endregion

        #endregion


        #region Helper functions

        private void Show_No_Project_Selected_Notification() 
        {
            Controller_Global.MainControl?.Show_Notification("No ht selected", "Please select a heat treatment", FontAwesome.WPF.FontAwesomeIcon.Warning,
                                                                 System.Windows.Media.Brushes.Yellow, null, null);
        }
        #endregion


    }
}
