
#include <string>
#include <vector>
#include "Database_implementations/Data_stuctures/DBS_All_Structures_Header.h"
#include "Database_implementations/Data_stuctures/DBS_Project.h"
#include "Database_implementations/Data_stuctures/DBS_Case.h"
#include "Database_implementations/Data_stuctures/DBS_ElementComposition.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedPhases.h"
#include "Database_implementations/Data_stuctures/DBS_ScheilPhaseFraction.h"
#include "Database_implementations/Data_stuctures/DBS_CALPHADDatabase.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumConfiguration.h"
#include "Database_implementations/Data_stuctures/DBS_EquilibriumPhaseFractions.h"
#include "Database_implementations/Data_stuctures/DBS_SelectedElements.h"
#include "Database_implementations/Data_Controller.h"

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Block parameters
/// </summary>
class AM_pixel_parameters {

public:
	AM_pixel_parameters(IAM_Database* database, 
						DBS_Project* project, 
						int IDCase)
	{
		_db = database;
		_project = project;
		_case = new DBS_Case(database, IDCase);
		_case->IDProject = _project->id();
		_case->load();

		_CALPHAD_DB = new DBS_CALPHADDatabase(_db, -1);
		_scheilConfiguration = new DBS_ScheilConfiguration(_db, -1);
		_equilibriumConfiguration = new DBS_EquilibriumConfiguration(_db, -1);
		load_all();
	}

	AM_pixel_parameters(const AM_pixel_parameters& toCopy)
	{
		_db = toCopy._db;
		_project = toCopy._project;
		_case = new DBS_Case(*toCopy._case);

		_CALPHAD_DB = new DBS_CALPHADDatabase(*toCopy._CALPHAD_DB);
		_CALPHAD_DB->IDCase = _case->id();

		_scheilConfiguration = new DBS_ScheilConfiguration(*toCopy._scheilConfiguration);
		_scheilConfiguration->IDCase = _case->id();

		_equilibriumConfiguration = new DBS_EquilibriumConfiguration(*toCopy._equilibriumConfiguration);
		_equilibriumConfiguration->IDCase = _case->id();
		
		for(int n1 = 0; n1 < toCopy._elementComposition.size(); n1 ++)
		{
			_elementComposition.push_back(new DBS_ElementComposition(*toCopy._elementComposition[n1]));
			_elementComposition.back()->IDCase = _case->id();
		}

		for (int n1 = 0; n1 < toCopy._selectedPhases.size(); n1++)
		{
			_selectedPhases.push_back(new DBS_SelectedPhases(*toCopy._selectedPhases[n1]));
			_selectedPhases.back()->IDCase = _case->id();
		}


	}

	void load_all()
	{
		load_DBS_CALPHADDB(_case->id());
		load_DBS_ScheilConfig(_case->id());
		load_DBS_EquilibriumConfig(_case->id());
		load_DBS_EquilibriumPhaseFraction();
		load_DBS_ScheilPhaseFraction();
		load_DBS_elementComposition();
		load_DBS_selectedPhases();
	}

	void load_DBS_CALPHADDB(int IDCase)
	{
		if (_CALPHAD_DB == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
		DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if(DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_CALPHAD_DB->load(rowData);
		}
	}

	void load_DBS_ScheilConfig(int IDCase)
	{
		if (_scheilConfiguration == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_ScheilConfiguration());
		DTable.load_data(AMLIB::TN_ScheilConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if (DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_scheilConfiguration->load(rowData);
		}
	}

	void load_DBS_EquilibriumConfig(int IDCase)
	{
		if (_equilibriumConfiguration == nullptr) return;

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumConfiguration());
		DTable.load_data(AMLIB::TN_EquilibriumConfiguration().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

		if (DTable.row_count() > 0)
		{
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_equilibriumConfiguration->load(rowData);
		}
	}

	void load_DBS_EquilibriumPhaseFraction()
	{
		if (_equilibriumConfiguration == nullptr) return;
		_equilibriumPhaseFractions.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumPhaseFractions());
		DTable.load_data(AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] + " = \'" + std::to_string(_equilibriumConfiguration->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for(int n1 = 0 ; n1 < DTable.row_count(); n1 ++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				_equilibriumPhaseFractions.push_back(new DBS_EquilibriumPhaseFraction(_db, -1));
				_equilibriumPhaseFractions.back()->load(rowData);
			}
			std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
			_equilibriumConfiguration->load(rowData);
		}
	}

	void load_DBS_ScheilPhaseFraction()
	{
		if (_scheilConfiguration == nullptr) return;
		_scheilPhaseFractions.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumPhaseFractions());
		DTable.load_data(AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] + " = \'" + std::to_string(_scheilConfiguration->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				_scheilPhaseFractions.push_back(new DBS_ScheilPhaseFraction(_db, -1));
				_scheilPhaseFractions.back()->load(rowData);
			}
		}
	}

	void load_DBS_elementComposition()
	{
		// null pointer, in theory this will never happen
		if (_case == nullptr) return;
		// Template case
		if (_case->id() == -1) 
		{
			template_element_compositions();
			return;
		}

		// Existing case found, load data.
		_elementComposition.clear();
		AM_Database_Datatable DTable(_db, &AMLIB::TN_ElementComposition());
		DTable.load_data(AMLIB::TN_ElementComposition().columnNames[1] + " = \'" + std::to_string(_case->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				_elementComposition.push_back(new DBS_ElementComposition(_db,-1));
				_elementComposition.back()->load(rowData);
			}
		}
	}
	
	void template_element_compositions() 
	{
		for (auto* obj : _elementComposition) delete obj;
		_elementComposition.clear();

		//create all composition entries
		std::vector<std::string> projectElementsID = Data_Controller::csv_list_SelectedElements(_db, _project->id());
		for (std::string &IDstr : projectElementsID)
		{
			_elementComposition.push_back(new DBS_ElementComposition(_db, -1));
			_elementComposition.back()->IDElement = std::stoi(IDstr);
		}
	}

	void load_DBS_selectedPhases()
	{
		if (_case == nullptr) return;
		_selectedPhases.clear();

		AM_Database_Datatable DTable(_db, &AMLIB::TN_SelectedPhases());
		DTable.load_data(AMLIB::TN_SelectedPhases().columnNames[1] + " = \'" + std::to_string(_case->id()) + "\'");

		if (DTable.row_count() > 0)
		{
			for (int n1 = 0; n1 < DTable.row_count(); n1++)
			{
				std::vector<std::string> rowData = DTable.get_row_data(n1);
				_selectedPhases.push_back(new DBS_SelectedPhases(_db, -1));
				_selectedPhases.back()->load(rowData);
			}
		}
	}

	void save()
	{
		if (!_allowSave) return;
		_case->save();

		_CALPHAD_DB->IDCase = _case->id();
		_CALPHAD_DB->save();

		_scheilConfiguration->IDCase = _case->id();
		_scheilConfiguration->save();

		_equilibriumConfiguration->IDCase = _case->id();
		_equilibriumConfiguration->save();

		//TODO this would look nice if we declare a function as a template, for now, lets not optimize
		for (int n1 = 0; n1 < _equilibriumPhaseFractions.size(); n1++)
		{
			_equilibriumPhaseFractions[n1]->IDCase = _case->id();
			_equilibriumPhaseFractions[n1]->save();
		}

		for (int n1 = 0; n1 < _scheilPhaseFractions.size(); n1++)
		{
			_scheilPhaseFractions[n1]->IDCase = _case->id();
			_scheilPhaseFractions[n1]->save();
		}

		for(int n1 = 0; n1 < _elementComposition.size(); n1++)
		{
			_elementComposition[n1]->IDCase = _case->id();
			_elementComposition[n1]->save();
		}

		for (int n1 = 0; n1 < _selectedPhases.size(); n1++)
		{
			_selectedPhases[n1]->IDCase = _case->id();
			_selectedPhases[n1]->save();
		}

	}

	std::string csv_composition_ByName() 
	{
		std::string out{ "" };

		std::vector<std::vector<std::string>> entries;
		for (DBS_ElementComposition* comp : _elementComposition)
		{
			DBS_Element tempElement(_db, comp->IDElement);
			tempElement.load();

			entries.push_back(std::vector<std::string> { tempElement.Name, std::to_string(comp->Value) });
		}

		return IAM_Database::get_csv(entries);
	}

	int static create_new_pixel(IAM_Database* db, AM_Config* configuration, int projectID, std::string newName)
	{
		DBS_Case newCase(db, -1);
		newCase.IDProject = projectID;
		newCase.Name = newName;
		newCase.IDGroup = 0;
		newCase.save();

		DBS_CALPHADDatabase newCAL(db, -1);
		newCAL.IDCase = newCase.id();
		newCAL.Thermodynamic = std::filesystem::path(configuration->get_ThermodynamicDatabase_path()).filename().string();
		newCAL.Physical = std::filesystem::path(configuration->get_PhysicalDatabase_path()).filename().string();
		newCAL.Mobility = std::filesystem::path(configuration->get_MobilityDatabase_path()).filename().string();
		newCAL.save();

		// we use the default phase for the dependent phase in the Scheil
		// configuration,default = LIQUID.
		DBS_Phase tempPhase(db, -1);
		tempPhase.load_by_name("LIQUID");

		DBS_ScheilConfiguration newScheil(db, -1);
		newScheil.IDCase = newCase.id();
		newScheil.DependentPhase = tempPhase.id();
		newScheil.save();

		DBS_EquilibriumConfiguration newEquilib(db, -1);
		newEquilib.IDCase = newCase.id();
		newEquilib.save();

		//create all composition entries
		std::vector<std::string> projectElementsID = Data_Controller::csv_list_SelectedElements(db, projectID);
		for (std::string &IDstr : projectElementsID)
		{
			DBS_ElementComposition tempComp(db, -1);
			tempComp.IDCase = newCase.id();
			tempComp.IDElement = std::stoi(IDstr);
			tempComp.save();
		}

		//TODO: we can split all these commands into smaller functions
		return newCase.id();
	}

	std::string create_cases_vary_concentration(std::vector<int>& elementID, 
												std::vector<double>& stepSize, 
												std::vector<double>& steps,
												std::vector<double>& currentValues,
												int rIndex = 0) 
	{
		// if vectors are not of the same size we abort the operation
		if(elementID.size() != stepSize.size() &&
		   elementID.size() != steps.size())
		{return "vector size of input do not correspond!";}
		
		if(rIndex < elementID.size() - 1)
		{
			for(int n1 = 0; n1 < steps[rIndex]; n1++)
			{
				std::vector<double> newVector(currentValues.size());
				for (int n2 = 0; n2 < currentValues.size(); n2++)
				{
					newVector[n2] = currentValues[n2];
				}
				newVector[rIndex] = newVector[rIndex] + stepSize[rIndex]*n1;

				create_cases_vary_concentration(elementID, stepSize, steps, currentValues, rIndex + 1);
			}
		}
		else 
		{
			for (int n1 = 0; n1 < steps[rIndex]; n1++)
			{
				std::vector<double> newVector(currentValues.size());
				for (int n2 = 0; n2 < currentValues.size(); n2++)
				{
					newVector[n2] = currentValues[n2];
				}
				newVector[rIndex] = newVector[rIndex] + stepSize[rIndex] * n1;

				AM_pixel_parameters tempPixel(*this);
				for(int n2 = 0; n2 < elementID.size(); n2++)
				{
					tempPixel.set_composition(elementID[n2], newVector[n2]);
				}
				tempPixel.save();

			}
		}

		return "OK";
	}

#pragma region getters
	const int& get_caseID()
	{
		return _case->id();
	}

	const std::string& get_CaseName() 
	{
		return _case->Name;
	}
	void set_CaseName(std::string newName)
	{
		_case->Name = newName;
	}

	void set_AllowSave(bool allowsave) 
	{
		_allowSave = allowsave;
	}

#pragma region Equilibrium
	DBS_EquilibriumConfiguration* get_EquilibriumConfiguration()
	{
		return _equilibriumConfiguration; //TODO remove this
	}

	void set_equilibrium_config_startTemperature(double newvalue) 
	{
		_equilibriumConfiguration->StartTemperature = newvalue;
	}

	void set_equilibrium_config_endTemperature(double newvalue)
	{
		_equilibriumConfiguration->EndTemperature = newvalue;
	}

	void set_equilibrium_config_stepSize(double newvalue)
	{
		//TODO implement missing step size into the database
	}
#pragma endregion

#pragma region Scheil
	DBS_ScheilConfiguration* get_ScheilConfiguration()
	{
		return _scheilConfiguration; //TODO remove this
	}

	void set_scheil_config_startTemperature(double newvalue)
	{
		_scheilConfiguration->StartTemperature = newvalue;
	}
	double get_scheil_config_startTemperature() { return _scheilConfiguration->StartTemperature; }

	void set_scheil_config_endTemperature(double newvalue)
	{
		_scheilConfiguration->EndTemperature = newvalue;
	}
	double get_scheil_config_endTemperature() { return _scheilConfiguration->EndTemperature; }


	void set_scheil_config_stepSize(double newvalue)
	{
		_scheilConfiguration->StepSize = newvalue;
	}
	double get_scheil_config_StepSize() { return _scheilConfiguration->StepSize; }


	bool check_if_phase_is_selected(int idPhase)
	{
		// check if the phase was selected for this case, otherwise notify the user.
		auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(), [&idPhase](DBS_SelectedPhases* obj) {return obj->id() == idPhase; });
		if (selPhaseIterator == _selectedPhases.end()) return false;
		return true;
	}

	int set_scheil_config_dependentPhase(int newvalue)
	{
		// check if the phase was selected for this case, otherwise notify the user.
		if (!check_if_phase_is_selected(newvalue)) return 1;

		// return if phase is selected
		_scheilConfiguration->DependentPhase = newvalue;
		return 0;
	}
	int set_scheil_config_dependentPhase(std::string phaseName)
	{
		// find the phase in our database
		DBS_Phase tempPhase(_db, -1);
		tempPhase.load_by_name(phaseName);
		if (tempPhase.id() == -1) return 1; // phase does not exist

		// check if the phase was selected for this case, otherwise notify the user.
		if (!check_if_phase_is_selected(tempPhase.id())) return 1;

		// If all is well, we set the dependent phase
		_scheilConfiguration->DependentPhase = tempPhase.id();
		return 0;
	}
	int get_scheil_config_dependentPhaseID() { return _scheilConfiguration->DependentPhase; }
	std::string get_scheil_config_dependentPhaseName() 
	{ 
		DBS_Phase tempPhase(_db, _scheilConfiguration->DependentPhase);
		tempPhase.load();
		return tempPhase.Name;
	}

	void set_scheil_config_minimumLiquidFraction(double newvalue)
	{
		_scheilConfiguration->minimumLiquidFraction = newvalue;
	}
	double get_scheil_config_minimumLiquidFraction() { return _scheilConfiguration->minimumLiquidFraction; }
#pragma endregion

	

	std::vector<std::string> get_composition_string()
	{
		std::vector<std::string> out;

		for (DBS_ElementComposition* &var : _elementComposition)
		{
			out.push_back(std::to_string(var->Value));
		}

		return out;
	}

	std::vector<double> get_composition_double()
	{
		std::vector<double> out;

		for (DBS_ElementComposition* &var : _elementComposition)
		{
			out.push_back(var->Value);
		}

		return out;
	}

	int set_composition(int IDElement, double newValue)
	{
		auto elementIterator = find_if(_elementComposition.begin(), _elementComposition.end(),
			[&IDElement](const DBS_ElementComposition* obj) {return obj->IDElement == IDElement; });

			if(elementIterator != _elementComposition.end())
			{
				auto index = std::distance(_elementComposition.begin(), elementIterator);
				_elementComposition[index]->Value = newValue;
				return 0;
			}

			return 1;
	}

	int set_composition(std::string ElementName, double newValue)
	{
		DBS_Element tempElement(_db, -1);
		tempElement.load_by_name(ElementName);
		if (tempElement.id() == -1) return 1;

		return set_composition(tempElement.id(), newValue);
	}

	std::vector<std::string> get_selected_phases_ByName()
	{
		std::vector<std::string> out;
		
		for(int n1 = 0; n1 < _selectedPhases.size(); n1++)
		{
			DBS_Phase tempPhase(_db, _selectedPhases[n1]->IDPhase);
			tempPhase.load();

			out.push_back(tempPhase.Name);
		}

		return out;
	}

	std::vector<int> get_selected_phases_ByID()
	{
		std::vector<int> out;

		for (int n1 = 0; n1 < _selectedPhases.size(); n1++)
		{
			out.push_back(_selectedPhases[n1]->IDPhase);
		}

		return out;
	}

	int add_selectedPhase(std::string phaseName)
	{
		// check if phase exists
		DBS_Phase tempPhase(_db, -1);
		tempPhase.load_by_name(phaseName);
		if (tempPhase.id() == -1) return 1;

		// check if phase is already selected
		auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(), 
			[&tempPhase](DBS_SelectedPhases* obj)
			{
				return obj->id() == tempPhase.id();
			}
		);
		if (selPhaseIterator != _selectedPhases.end()) return 2;

		// add phase to list
		_selectedPhases.push_back(new DBS_SelectedPhases(_db, -1));
		_selectedPhases.back()->IDCase = _case->id();
		_selectedPhases.back()->IDPhase = tempPhase.id();

		return 0;
	}

	int remove_selectedPhase(std::string phaseName) 
	{
		// get phase id
		DBS_Phase tempPhase(_db, -1);
		tempPhase.load_by_name(phaseName);
		if (tempPhase.id() == -1) return 1;

		// check if phase is already selected
		auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(),
			[&tempPhase](DBS_SelectedPhases* obj)
			{
				return obj->id() == tempPhase.id();
			}
		);
		
		if (selPhaseIterator != _selectedPhases.end())
		{
			auto index = std::distance(_selectedPhases.begin(), selPhaseIterator);
			_selectedPhases[index]->remove();
			_selectedPhases.erase(_selectedPhases.begin() + index);

			return 0;
		}


		return 1;
	}

	int select_phases(std::vector<std::string> phasesSelect)
	{
		remove_all_seleccted_phase();
		for(auto &namePhase : phasesSelect)
		{
			string_manipulators::toCaps(namePhase);
			if (add_selectedPhase(namePhase) != 0) return 1;
		}

		return 0;
	}

	void remove_all_seleccted_phase() 
	{
		if (_selectedPhases.empty()) return;
		for (int n1 = 0; n1 < _selectedPhases.size(); n1++)
		{
			_selectedPhases[n1]->remove();
			delete _selectedPhases[n1];
		}
		_selectedPhases.clear();
	}
#pragma endregion

#pragma region Methods
	void reset_equilibrium()
	{
		//if there is no setup for 
		if (_equilibriumConfiguration == nullptr) return;
		DBS_EquilibriumConfiguration::remove_equilibrium_data(_db,_case->id());
		_equilibriumPhaseFractions.clear();

	}

	void reset_scheil()
	{
		if (_scheilConfiguration == nullptr) return;
		DBS_ScheilConfiguration::remove_scheil_data(_db, _case->id());
		_scheilPhaseFractions.clear();
		
	}

#pragma endregion
private:
	//Flags
	bool _allowSave{ true };

	//Models
	IAM_Database* _db;
	DBS_Project* _project;
	DBS_CALPHADDatabase* _CALPHAD_DB;
	DBS_Case* _case;
	DBS_ScheilConfiguration* _scheilConfiguration;
	DBS_EquilibriumConfiguration* _equilibriumConfiguration;

	std::vector<DBS_EquilibriumPhaseFraction*> _equilibriumPhaseFractions;
	std::vector<DBS_ScheilPhaseFraction*> _scheilPhaseFractions;
	std::vector<DBS_ElementComposition*> _elementComposition;
	std::vector<DBS_SelectedPhases*> _selectedPhases;



};
/** @}*/