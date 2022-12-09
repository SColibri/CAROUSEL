using AMFramework.Views.Case;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Solidification
{
    public class SolidificationConfigurations_ViewModel : ControllerAbstract
    {

        #region Constructor
        public SolidificationConfigurations_ViewModel(Model_Case caseObject) 
        {
            _caseTemplate = caseObject;
        }

        #endregion

        #region Properties
        private Model_Case _caseTemplate;
        public Model_Case CaseTemplate
        {
            get { return _caseTemplate; }
            set
            {
                _caseTemplate = value;
                OnPropertyChanged(nameof(CaseTemplate));
            }
        }

        #region Solidification_configuration
        private double _startTemperature = 700;
            public double StartTemperature
            {
                get { return _startTemperature; }
                set
                {
                    _startTemperature = value;
                    OnPropertyChanged(nameof(StartTemperature));
                    if (CaseTemplate.ScheilConfiguration != null)
                        CaseTemplate.ScheilConfiguration.ModelObject.StartTemperature = StartTemperature;
                    
                    if (CaseTemplate.EquilibriumConfiguration != null)
                        CaseTemplate.EquilibriumConfiguration.ModelObject.StartTemperature = StartTemperature;
                }
            }


            private double _endTemperature = 450;
            public double EndTemperature
            {
                get { return _endTemperature; }
                set
                {
                    _endTemperature = value;
                    OnPropertyChanged(nameof(EndTemperature));

                    if (CaseTemplate.ScheilConfiguration != null)
                        CaseTemplate.ScheilConfiguration.ModelObject.EndTemperature = EndTemperature;

                    if (CaseTemplate.EquilibriumConfiguration != null)
                        CaseTemplate.EquilibriumConfiguration.ModelObject.EndTemperature = EndTemperature;

                }
            }


            private double _stepSize = 1;
            public double StepSize
            {
                get { return _stepSize; }
                set
                {
                    _stepSize = value;
                    OnPropertyChanged(nameof(StepSize));

                    if (CaseTemplate.ScheilConfiguration != null)
                        CaseTemplate.ScheilConfiguration.ModelObject.StepSize = StepSize;

                    if (CaseTemplate.EquilibriumConfiguration != null)
                        CaseTemplate.EquilibriumConfiguration.ModelObject.StepSize = StepSize;
                }
            }

            private double _minLiquidFraction = 1;
            public double MinLiquidFraction
            {
                get { return _minLiquidFraction; }
                set
                {
                    _minLiquidFraction = value;
                    OnPropertyChanged(nameof(MinLiquidFraction));

                    if(CaseTemplate.ScheilConfiguration != null)
                        CaseTemplate.ScheilConfiguration.ModelObject.MinLiquidFraction = MinLiquidFraction;
                }
            }
            #endregion

        #endregion
    }
}
