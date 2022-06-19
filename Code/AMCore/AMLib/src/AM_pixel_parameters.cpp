#include "../include/AM_pixel_parameters.h"

#pragma region constructor_destructor
AM_pixel_parameters::AM_pixel_parameters(IAM_Database* database,
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

AM_pixel_parameters::AM_pixel_parameters(const AM_pixel_parameters& toCopy)
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

	for (int n1 = 0; n1 < toCopy._elementComposition.size(); n1++)
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
#pragma endregion

#pragma region Object
void AM_pixel_parameters::save()
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

	for (int n1 = 0; n1 < _elementComposition.size(); n1++)
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

void AM_pixel_parameters::reload_element_compositions()
{
	for (auto* obj : _elementComposition) delete obj;
	_elementComposition.clear();

	//create all composition entries
	std::vector<std::string> projectElementsID = Data_Controller::csv_list_SelectedElements(_db, _project->id());
	for (std::string& IDstr : projectElementsID)
	{
		_elementComposition.push_back(new DBS_ElementComposition(_db, -1));
		_elementComposition.back()->IDElement = std::stoi(IDstr);
	}
}

int AM_pixel_parameters::create_new_pixel(IAM_Database* db, AM_Config* configuration, int projectID, std::string newName)
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
	for (std::string& IDstr : projectElementsID)
	{
		DBS_ElementComposition tempComp(db, -1);
		tempComp.IDCase = newCase.id();
		tempComp.IDElement = std::stoi(IDstr);
		tempComp.save();
	}

	//TODO: we can split all these commands into smaller functions
	return newCase.id();
}

#pragma region Object_methods
std::string AM_pixel_parameters::create_cases_vary_concentration(std::vector<int>& elementID,
	std::vector<double>& stepSize,
	std::vector<double>& steps,
	std::vector<double>& currentValues,
	int rIndex)
{
	// if vectors are not of the same size we abort the operation
	if (elementID.size() != stepSize.size() &&
		elementID.size() != steps.size())
	{
		return "vector size of input do not correspond!";
	}

	if (rIndex < elementID.size() - 1)
	{
		for (int n1 = 0; n1 < steps[rIndex]; n1++)
		{
			std::vector<double> newVector(currentValues.size());
			for (int n2 = 0; n2 < currentValues.size(); n2++)
			{
				newVector[n2] = currentValues[n2];
			}
			int index_EL = get_element_index(elementID[rIndex]);
			newVector[index_EL] = newVector[index_EL] + stepSize[rIndex] * n1;

			create_cases_vary_concentration(elementID, stepSize, steps, newVector, rIndex + 1);
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
			int index_EL = get_element_index(elementID[rIndex]);
			newVector[index_EL] = newVector[index_EL] + stepSize[rIndex] * n1;

			AM_pixel_parameters tempPixel(*this);
			for (int n2 = 0; n2 < _elementComposition.size(); n2++)
			{
				tempPixel.set_composition(_elementComposition[n2]->IDElement, newVector[n2]);
			}
			tempPixel.save();

		}
	}

	return "OK";
}

bool AM_pixel_parameters::check_if_phase_is_selected(int idPhase)
{
	// check if the phase was selected for this case, otherwise notify the user.
	auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(), [&idPhase](DBS_SelectedPhases* obj) {return obj->IDPhase == idPhase; });
	if (selPhaseIterator == _selectedPhases.end()) return false;
	return true;
}

int AM_pixel_parameters::get_element_index(int IDElement)
{
	auto componentIterator = find_if(_elementComposition.begin(), _elementComposition.end(), [&IDElement](DBS_ElementComposition* obj) {return obj->IDElement == IDElement; });
	if (componentIterator != _elementComposition.end())
	{
		auto index = std::distance(_elementComposition.begin(), componentIterator);
		return index;
	}

	return -1;
}


int AM_pixel_parameters::get_element_index(std::string ElementName)
{
	DBS_Element tempElement(_db, -1);
	tempElement.load_by_name(ElementName);
	if (tempElement.id() == -1) return -1;

	return get_element_index(tempElement.id());
}

int AM_pixel_parameters::get_reference_elementID()
{
	size_t IDElement = Data_Controller::get_reference_element_ByID(_db, _project->id());
	return IDElement;
}

void AM_pixel_parameters::reset_equilibrium()
{
	if (_equilibriumConfiguration == nullptr) return;
	DBS_EquilibriumConfiguration::remove_equilibrium_data(_db, _case->id());
	_equilibriumPhaseFractions.clear();

}

void AM_pixel_parameters::reset_scheil()
{
	if (_scheilConfiguration == nullptr) return;
	DBS_ScheilConfiguration::remove_scheil_data(_db, _case->id());
	_scheilPhaseFractions.clear();

}

#pragma endregion

#pragma region Object_getters_setters
void AM_pixel_parameters::set_AllowSave(bool allowsave)
{
	_allowSave = allowsave;
}

const int& AM_pixel_parameters::get_caseID()
{
	return _case->id();
}

const std::string& AM_pixel_parameters::get_CaseName()
{
	return _case->Name;
}

void AM_pixel_parameters::set_CaseName(std::string newName)
{
	_case->Name = newName;
}

std::vector<std::string> AM_pixel_parameters::get_composition_string()
{
	std::vector<std::string> out;

	for (DBS_ElementComposition*& var : _elementComposition)
	{
		out.push_back(std::to_string(var->Value));
	}

	return out;
}

std::vector<double> AM_pixel_parameters::get_composition_double()
{
	std::vector<double> out;

	for (DBS_ElementComposition*& var : _elementComposition)
	{
		out.push_back(var->Value);
	}

	return out;
}

int AM_pixel_parameters::set_composition(int IDElement, double newValue)
{
	auto elementIterator = find_if(_elementComposition.begin(), _elementComposition.end(),
		[&IDElement](const DBS_ElementComposition* obj) {return obj->IDElement == IDElement; });

	if (elementIterator != _elementComposition.end())
	{
		auto index = std::distance(_elementComposition.begin(), elementIterator);
		_elementComposition[index]->Value = newValue;
		update_referenceComposition();
		return 0;
	}

	return 1;
}

int AM_pixel_parameters::set_composition(std::string ElementName, double newValue)
{
	DBS_Element tempElement(_db, -1);
	tempElement.load_by_name(ElementName);
	if (tempElement.id() == -1) return 1;
	if (ElementName.compare("VA") == 0) newValue = 0.0;
	update_referenceComposition();

	return set_composition(tempElement.id(), newValue);
}

void AM_pixel_parameters::update_referenceComposition()
{
	AM_Database_Datatable dT(_db, &AMLIB::TN_SelectedElements());
	dT.load_data("IDProject = " + std::to_string(_project->id()) + " AND " + 
				 "isReferenceElement = 1 ");

	if (dT.row_count() == 0) return; // no elements set as reference
	int IDElement = std::stoi(dT(2, 0));
	auto selIterator = find_if(_elementComposition.begin(), _elementComposition.end(), 
		[&IDElement](DBS_ElementComposition* obj) {return obj->IDElement == IDElement;});

	if (selIterator == _elementComposition.end()) return; // no element found, error?
	auto index = std::distance(_elementComposition.begin(), selIterator);
	double totalSum{ 0 };

	// sum up all values from the table
	for(int n1 = 0 ; n1 < _elementComposition.size(); n1 ++)
	{
		if (n1 != index) { totalSum += _elementComposition[n1]->Value; }
	}

	_elementComposition[index]->Value = 100 - totalSum; //TODO: this is only for weight percentage
}

bool AM_pixel_parameters::check_composition() 
{
	double sumTot{ 0 };

	for (int n1 = 0; n1 < _elementComposition.size(); n1++)
	{
		sumTot += _elementComposition[n1]->Value;
	}
	if (sumTot == 100) return true;

	return false;
}

std::vector<std::string> AM_pixel_parameters::get_selected_phases_ByName()
{
	std::vector<std::string> out;

	for (int n1 = 0; n1 < _selectedPhases.size(); n1++)
	{
		DBS_Phase tempPhase(_db, _selectedPhases[n1]->IDPhase);
		tempPhase.load();

		out.push_back(tempPhase.Name);
	}

	return out;
}

std::vector<int> AM_pixel_parameters::get_selected_phases_ByID()
{
	std::vector<int> out;

	for (int n1 = 0; n1 < _selectedPhases.size(); n1++)
	{
		out.push_back(_selectedPhases[n1]->IDPhase);
	}

	return out;
}

int AM_pixel_parameters::add_selectedPhase(std::string phaseName)
{
	// check if phase exists
	DBS_Phase tempPhase(_db, -1);
	tempPhase.load_by_name(phaseName);
	if (tempPhase.id() == -1) return 1;

	// check if phase is already selected
	auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(),
		[&tempPhase](DBS_SelectedPhases* obj)
		{
			return obj->IDPhase == tempPhase.id();
		}
	);
	if (selPhaseIterator != _selectedPhases.end()) return 2;

	// add phase to list
	_selectedPhases.push_back(new DBS_SelectedPhases(_db, -1));
	_selectedPhases.back()->IDCase = _case->id();
	_selectedPhases.back()->IDPhase = tempPhase.id();

	return 0;
}

int AM_pixel_parameters::remove_selectedPhase(std::string phaseName)
{
	// get phase id
	DBS_Phase tempPhase(_db, -1);
	tempPhase.load_by_name(phaseName);
	if (tempPhase.id() == -1) return 1;

	// check if phase is already selected
	auto selPhaseIterator = find_if(_selectedPhases.begin(), _selectedPhases.end(),
		[&tempPhase](DBS_SelectedPhases* obj)
		{
			return obj->IDPhase == tempPhase.id();
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

int AM_pixel_parameters::select_phases(std::vector<std::string> phasesSelect)
{
	remove_all_seleccted_phase();
	for (auto& namePhase : phasesSelect)
	{
		string_manipulators::toCaps(namePhase);
		if (add_selectedPhase(namePhase) != 0) return 1;
	}

	return 0;
}

void AM_pixel_parameters::remove_all_seleccted_phase()
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

#pragma endregion

#pragma region Data
void AM_pixel_parameters::load_all()
{
	load_DBS_CALPHADDB(_case->id());
	load_DBS_ScheilConfig(_case->id());
	load_DBS_EquilibriumConfig(_case->id());
	load_DBS_EquilibriumPhaseFraction();
	load_DBS_ScheilPhaseFraction();
	load_DBS_elementComposition();
	load_DBS_selectedPhases();
}

void AM_pixel_parameters::load_DBS_CALPHADDB(int IDCase)
{
	if (_CALPHAD_DB == nullptr) return;

	AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
	DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(IDCase) + "\'");

	if (DTable.row_count() > 0)
	{
		std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
		_CALPHAD_DB->load(rowData);
	}
}

void AM_pixel_parameters::load_DBS_ScheilConfig(int IDCase)
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

void AM_pixel_parameters::load_DBS_EquilibriumConfig(int IDCase)
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

void AM_pixel_parameters::load_DBS_EquilibriumPhaseFraction()
{
	if (_equilibriumConfiguration == nullptr) return;
	_equilibriumPhaseFractions.clear();

	AM_Database_Datatable DTable(_db, &AMLIB::TN_EquilibriumPhaseFractions());
	DTable.load_data(AMLIB::TN_EquilibriumPhaseFractions().columnNames[1] + " = \'" + std::to_string(_equilibriumConfiguration->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		for (int n1 = 0; n1 < DTable.row_count(); n1++)
		{
			std::vector<std::string> rowData = DTable.get_row_data(n1);
			_equilibriumPhaseFractions.push_back(new DBS_EquilibriumPhaseFraction(_db, -1));
			_equilibriumPhaseFractions.back()->load(rowData);
		}
		std::vector<std::string> rowData = DTable.get_row_data(0); // we only expect one value to match for this, all others will be ignored
		_equilibriumConfiguration->load(rowData);
	}
}

void AM_pixel_parameters::load_DBS_ScheilPhaseFraction()
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

void AM_pixel_parameters::load_DBS_elementComposition()
{
	// null pointer, in theory this will never happen
	if (_case == nullptr) return;
	// Template case
	if (_case->id() == -1)
	{
		reload_element_compositions();
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
			_elementComposition.push_back(new DBS_ElementComposition(_db, -1));
			_elementComposition.back()->load(rowData);
		}
	}
}

void AM_pixel_parameters::load_DBS_selectedPhases()
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

#pragma region csv
std::string AM_pixel_parameters::csv_composition_ByName()
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

#pragma endregion

#pragma endregion

#pragma region external_objects


#pragma region Equilibrium
void AM_pixel_parameters::set_equilibrium_config_startTemperature(double newvalue)
{
	_equilibriumConfiguration->StartTemperature = newvalue;
}

double AM_pixel_parameters::get_equilibrium_config_startTemperature()
{
	return _equilibriumConfiguration->StartTemperature;
}

void AM_pixel_parameters::set_equilibrium_config_endTemperature(double newvalue)
{
	_equilibriumConfiguration->EndTemperature = newvalue;
}

double AM_pixel_parameters::get_equilibrium_config_endTemperature()
{
	return _equilibriumConfiguration->EndTemperature;
}

void AM_pixel_parameters::set_equilibrium_config_stepSize(double newvalue)
{
	_equilibriumConfiguration->StepSize = newvalue;
}

double AM_pixel_parameters::get_equilibrium_config_stepSize()
{
	return _equilibriumConfiguration->StepSize;
}
#pragma endregion

#pragma region Scheil

void AM_pixel_parameters::set_scheil_config_startTemperature(double newvalue)
{
	_scheilConfiguration->StartTemperature = newvalue;
}
double AM_pixel_parameters::get_scheil_config_startTemperature() { return _scheilConfiguration->StartTemperature; }

void AM_pixel_parameters::set_scheil_config_endTemperature(double newvalue)
{
	_scheilConfiguration->EndTemperature = newvalue;
}
double AM_pixel_parameters::get_scheil_config_endTemperature() { return _scheilConfiguration->EndTemperature; }


void AM_pixel_parameters::set_scheil_config_stepSize(double newvalue)
{
	_scheilConfiguration->StepSize = newvalue;
}
double AM_pixel_parameters::get_scheil_config_StepSize() { return _scheilConfiguration->StepSize; }

int AM_pixel_parameters::set_scheil_config_dependentPhase(int newvalue)
{
	// check if the phase was selected for this case, otherwise notify the user.
	if (!check_if_phase_is_selected(newvalue)) return 1;

	// return if phase is selected
	_scheilConfiguration->DependentPhase = newvalue;
	return 0;
}
int AM_pixel_parameters::set_scheil_config_dependentPhase(std::string phaseName)
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
int AM_pixel_parameters::get_scheil_config_dependentPhaseID() { return _scheilConfiguration->DependentPhase; }
std::string AM_pixel_parameters::get_scheil_config_dependentPhaseName()
{
	DBS_Phase tempPhase(_db, _scheilConfiguration->DependentPhase);
	tempPhase.load();
	return tempPhase.Name;
}

void AM_pixel_parameters::set_scheil_config_minimumLiquidFraction(double newvalue)
{
	_scheilConfiguration->minimumLiquidFraction = newvalue;
}
double AM_pixel_parameters::get_scheil_config_minimumLiquidFraction() { return _scheilConfiguration->minimumLiquidFraction; }

#pragma endregion


#pragma endregion


