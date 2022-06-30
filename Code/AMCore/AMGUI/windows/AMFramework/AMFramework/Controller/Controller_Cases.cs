using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_Cases : INotifyPropertyChanged
    {

        #region Cons_Des
        private Core.AMCore_Socket _AMCore_Socket;
        private Controller.Controller_DBS_Projects _ControllerProjects;
        private int _idProject = -1;
        private int _selectedIDCase = -1;

        public Controller_Cases(ref Core.AMCore_Socket socket,
                               Controller.Controller_DBS_Projects _project)
        {
            _AMCore_Socket = socket;
            _ControllerProjects = _project;
            _selectedPhases = new Controller_Selected_Phases(ref socket, this);
            _elementComposition = new(ref socket, this);
            _equilibriumPhaseFractions = new(ref socket, this);
        }

        public Controller_Cases()
        {

        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data
        private Model.Model_Case _selectedCase = new();

        List<Model.Model_Case> _cases = new List< Model.Model_Case >();

        public List<Model.Model_Case> Cases
        {
            get 
            {
                return _cases; 
            }
        }
        #endregion

        #region Flags
        private Model.Model_Case _selected_case;
        public Model.Model_Case SelectedCase 
        { 
            get { return _selected_case; } 
            set 
            {
                _selected_case = value;
                OnPropertyChanged("SelectedCase");
                OnPropertyChanged("SelectedPhases");
                OnPropertyChanged("ElementComposition");
            }
        }
        #endregion

        #region Methods
        public void refresh() 
        {
            load_data();
        }

        private string load_data() 
        {
            string Query = "database_table_custom_query SELECT * FROM \'Case\' WHERE IDProject = " + _ControllerProjects.SelectedProject.ID.ToString();
            string outy = _AMCore_Socket.send_receive(Query);
            List<string> rowItems = outy.Split("\n").ToList();
            _cases = new List<Model.Model_Case>();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 8)
                {
                    Model.Model_Case model = new Model.Model_Case();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDProject = Convert.ToInt32(columnItems[1]);
                    model.IDGroup = Convert.ToInt32(columnItems[2]);
                    model.Name = columnItems[3];
                    model.Script = columnItems[4];
                    model.Date = columnItems[5];
                    model.PosX = Convert.ToDouble(columnItems[6]);
                    model.PosY = Convert.ToDouble(columnItems[7]);
                    model.PosZ = Convert.ToDouble(columnItems[8]);
                    model.ElementComposition = new();
                    _cases.Add(model);
                }
            }

            _selectedPhases.fill_models_with_selectedPhases();
            _elementComposition.fill_models_with_composition();
            OnPropertyChanged("Cases");
            return outy;
        }

        public void update_phaseFractions(Model.Model_Case model) 
        {
            if (model is null) return;
            _equilibriumPhaseFractions.fill_model_phase_fraction(model);
        }

        public void Clear_phase_fractions(Model.Model_Case model)
        {
            _equilibriumPhaseFractions.clear_models_phaseFractions();
        }
        #endregion

        #region Controllers
        private Controller.Controller_Selected_Phases _selectedPhases;
        public List<Model.Model_SelectedPhases> SelectedPhases { get { return _selectedPhases.Phases; } }

        private Controller.Controller_ElementComposition _elementComposition;
        public List<Model.Model_ElementComposition> ElementComposition { get { return _elementComposition.Composition; } }

        private Controller.Controller_EquilibriumPhaseFraction _equilibriumPhaseFractions;
        public List<Model.Model_EquilibriumPhaseFraction> EquilibriumPhaseFraction { get { return _equilibriumPhaseFractions.Equilibrium; } }

        #endregion

        #region Commands
        #region run_equilibrium

        private ICommand _run_equilibrium;
        public ICommand Run_equilibrium
        {
            get
            {
                if (_run_equilibrium == null)
                {
                    _run_equilibrium = new RelayCommand(
                        param => this.Run_equilibrium_controll(),
                        param => this.Can_Run_equilibrium()
                    );
                }
                return _run_equilibrium;
            }
        }

        private void Run_equilibrium_controll()
        {

            refresh();
        }

        private bool Can_Run_equilibrium()
        {
            return true;
        }
        #endregion
        #endregion

    }
}
