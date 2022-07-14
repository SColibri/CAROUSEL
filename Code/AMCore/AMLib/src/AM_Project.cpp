#include "../include/AM_Project.h"

#pragma region Cons_Des
AM_Project::AM_Project(IAM_Database* database, AM_Config* configuration, int id): 
	_db(database), _configuration(configuration)
{
	_project = new DBS_Project(database, id);
	if (_project->load() != 0) _project->set_id(-1);

	load_singlePixel_Cases();
	load_DBS_selectedElements();
	load_DBS_CALPHAD();
	load_DBS_activePhases();
	load_DBS_activePhasesConfig();
	load_DBS_activePhases_composition();
}

AM_Project::AM_Project(IAM_Database* database, AM_Config* configuration, std::string projectName) :
	_db(database), _configuration(configuration)
{
	_project = new DBS_Project(database, -1);
	_project->load_ByName(projectName);

	if (_project->load() != 0) _project->set_id(-1);
	load_singlePixel_Cases();
	load_DBS_selectedElements();
	load_DBS_CALPHAD();
}

AM_Project::~AM_Project()
{
	if (_project != nullptr) delete _project;
	if (_tempPixel != nullptr) delete _tempPixel;
	if (_calphadDatabases != nullptr) delete _calphadDatabases;
	if (_activePhasesConfig != nullptr) delete _activePhasesConfig;
	clear_selectedElements();
	clear_singlePixel_cases();
	clear_activePhases();
}

#pragma endregion

#pragma region setters_getters
void AM_Project::set_project_name(std::string newName, std::string apiPath, std::string externalAPI_Path)
{
	_project->Name = newName;
	_project->APIName = std::filesystem::path(apiPath).filename().string();
	_project->External_APIName = std::filesystem::path(externalAPI_Path).filename().string();
	_project->save();

	_calphadDatabases->IDProject = _project->id();
	_calphadDatabases->Thermodynamic = std::filesystem::path(_configuration->get_ThermodynamicDatabase_path()).filename().string();
	_calphadDatabases->Physical = std::filesystem::path(_configuration->get_PhysicalDatabase_path()).filename().string();
	_calphadDatabases->Mobility = std::filesystem::path(_configuration->get_MobilityDatabase_path()).filename().string();
	_calphadDatabases->save();
}

const std::string& AM_Project::get_project_name()
{
	return _project->Name;
}

const std::string& AM_Project::get_project_APIName()
{
	return _project->APIName;
}

const std::string& AM_Project::get_project_external_APIName()
{
	return _project->External_APIName;
}

const int& AM_Project::get_project_ID()
{
	return _project->id();
}

std::string AM_Project::get_project_data_csv()
{
	return IAM_Database::csv_join_row(_project->get_input_vector(), IAM_Database::Delimiter);
}

std::vector<AM_pixel_parameters*>& AM_Project::get_singlePixel_Cases()
{
	return _singlePixel_cases;
}

std::string AM_Project::set_selected_elements_ByName(std::vector<std::string> newSelection)
{
	std::string outy{ "" };
	std::vector<int> IDelements;

	// check if elements exist
	for (auto& parm : newSelection)
	{
		string_manipulators::toCaps(parm);
		DBS_Element DBSFind(_db, -1);
		DBSFind.load_by_name(parm);

		// check if element is part of the database
		if (DBSFind.id() == -1)
		{
			std::string strBuild = "Error: The element " + parm + " was not found in the database! ";
			return strBuild;
		}

		// add non repreted elements!
		if (std::find(IDelements.begin(), IDelements.end(), DBSFind.id()) ==
			IDelements.end()) IDelements.push_back(DBSFind.id());

	}

	//Remove all data related to the project
	DBS_Project::remove_project_data(_db, get_project_ID());
	DBS_ActivePhases_ElementComposition::remove_project_data(_db, _project->id());

	//Add all selected elements
	for each (int IDElement in IDelements)
	{
		DBS_SelectedElements newElem(_db, -1);
		newElem.IDElement = IDElement;
		newElem.IDProject = get_project_ID();
		newElem.save();
		outy += "Element ID: " + std::to_string(IDElement) + " selected || ";
		
		DBS_ActivePhases_ElementComposition newComp(_db, -1);
		newComp.IDElement = IDElement;
		newComp.IDProject = _project->id();
		newComp.save();

	}

	load_DBS_selectedElements();
	load_singlePixel_Cases();

	return outy;
}

std::vector<std::string> AM_Project::get_selected_elements_ByName()
{
	std::vector<std::string> out;

	for (DBS_SelectedElements* var : _selectedElements)
	{
		DBS_Element tempElement(_db, var->IDElement);
		tempElement.load();
		out.push_back(tempElement.Name);
	}

	return out;
}

int AM_Project::set_reference_element(std::string ElementName)
{
	int Result{ 1 }; // Element has not been found

	DBS_Element tempElement(_db, -1);
	tempElement.load_by_name(ElementName);
	if (tempElement.id() == -1) return 2; // Selected element does not exist
	if (!element_is_contained(tempElement.id())) return 3; // element is not selected here

	for (DBS_SelectedElements* var : _selectedElements)
	{
		if (tempElement.id() == var->IDElement)
		{
			var->isReferenceElement = 1;
			Result = 0; // Element has been found
		}
		else
		{
			// reset all others, only one element can be a
			// reference element
			var->isReferenceElement = 0;
		}

		var->save();
	}

	return Result;
}

std::string AM_Project::get_reference_element_ByName()
{
	int refID = get_reference_element_ByID();
	if (refID > -1)
	{
		DBS_Element tempElement(_db, refID);
		tempElement.load();
		return tempElement.Name;
	}

	return "Not found!";
}

int AM_Project::get_reference_element_ByID()
{
	for (DBS_SelectedElements* selEl : _selectedElements)
	{
		if (selEl->isReferenceElement == 1)
		{
			DBS_Element tempElement(_db, selEl->IDElement);
			tempElement.load();

			return tempElement.id();
		}
	}

	return -1;
}

#pragma endregion

#pragma region Project_data
void AM_Project::clear_project_data()
{
	DBS_Project::remove_project_data(_db, _project->id());
	refresh_data();
}

void AM_Project::refresh_data()
{
	int tempId = _project->id();
	delete _project;
	_project = new DBS_Project(_db, tempId);
	_project->load(); // TODO: put this in a function

	load_singlePixel_Cases();
	load_DBS_selectedElements();
	load_DBS_CALPHAD();
}
#pragma endregion

#pragma region project_methods
void AM_Project::Edit_active_phase_configuration(int startTemp, int endTemp, int stepSize)
{
	if (_activePhasesConfig == nullptr) _activePhasesConfig = new DBS_ActivePhases_Configuration(_db, -1);
	_activePhasesConfig->IDProject = _project->id();
	_activePhasesConfig->StartTemp = startTemp;
	_activePhasesConfig->EndTemp = endTemp;
	_activePhasesConfig->StepSize = stepSize;
	_activePhasesConfig->save();
}

#pragma endregion

#pragma region Checks
bool AM_Project::project_is_valid()
{
	if (_project = nullptr) return false;
	if (_project->id() == -1) _project->save();
	if (_project->id() == -1) return false;

	return true;
}

bool AM_Project::element_is_contained(int IDElement)
{
	// find ID in loaded project
	auto SPC_it = std::find_if(_selectedElements.begin(), _selectedElements.end(),
		[&IDElement](DBS_SelectedElements* obj) {return obj->IDElement == IDElement; });

	if (SPC_it != _selectedElements.end()) return true;
	return false;
}
#pragma endregion

#pragma region Cases

AM_pixel_parameters* AM_Project::get_pixelCase(int IDCase)
{
	AM_pixel_parameters* out{ nullptr };

	//find in singlePixels
	auto SPC_it = std::find_if(_singlePixel_cases.begin(), _singlePixel_cases.end(), [&IDCase](AM_pixel_parameters* obj) {return obj->get_caseID() == IDCase; });
	if (SPC_it != _singlePixel_cases.end())
	{
		auto SPC_index = std::distance(_singlePixel_cases.begin(), SPC_it);
		out = _singlePixel_cases[SPC_index];
	}

	//TODO: add search pattern when adding objects and layers in the project
	if (out == nullptr)
	{
		// search on the object list
	}

	return out;
}

#pragma region SinglePixel_Cases
int AM_Project::new_singlePixel_Case(std::string newName)
{
	if (!project_is_valid()) return 1;

	int newIDCase = AM_pixel_parameters::create_new_pixel(_db, _configuration, _project->id(), newName);
	_singlePixel_cases.push_back(new AM_pixel_parameters(_db, _project, newIDCase));
	return 0;
}
#pragma endregion

#pragma region Template_Case
void AM_Project::create_case_template(const std::string& nameCase)
{
	if (_tempPixel != nullptr) delete _tempPixel;
	_tempPixel = new AM_pixel_parameters(_db, _project, -1);

	// Template cases are not saved because we have to specify what group the case belongs to
	// 0 - to single pixel case or > 0 if is an object.
	_tempPixel->set_AllowSave(false);
	_tempPixel->set_CaseName(nameCase);

}

AM_pixel_parameters* AM_Project::get_case_template()
{
	return _tempPixel;
}

int AM_Project::template_set_composition(std::string ElementName, double newValue)
{
	if (_tempPixel == nullptr) return 1;
	return _tempPixel->set_composition(ElementName, newValue);
}

int AM_Project::template_set_composition(int ElementID, double newValue)
{
	if (_tempPixel == nullptr) return 1;
	return _tempPixel->set_composition(ElementID, newValue);
}

std::string AM_Project::template_get_composition(int ElementID, double newValue)
{
	if (_tempPixel == nullptr) return "No template selected";
	return _tempPixel->csv_composition_ByName();
}

std::string AM_Project::create_cases_vary_concentration(std::vector<int>& elementID,
	std::vector<double>& stepSize,
	std::vector<double>& steps,
	std::vector<double>& currentValues)
{
	if (_tempPixel == nullptr) return "No template selected";
	std::string out{""};
	out = _tempPixel->create_cases_vary_concentration(elementID, stepSize, steps, currentValues);
	load_singlePixel_Cases();
	return out;
}
#pragma endregion

#pragma endregion

#pragma region Loaders
void AM_Project::load_singlePixel_Cases()
{
	clear_singlePixel_cases();
	AM_Database_Datatable CaseTables(_db, &AMLIB::TN_Case());
	CaseTables.load_data(AMLIB::TN_Case().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\' AND " +
		AMLIB::TN_Case().columnNames[2] + " = \'0\'");

	if (CaseTables.row_count() > 0)
	{
		for (int n1 = 0; n1 < CaseTables.row_count(); n1++)
		{
			_singlePixel_cases.push_back(new AM_pixel_parameters(_db, _project, std::stoi(CaseTables(0, n1))));
		}
	}
}
void AM_Project::load_DBS_selectedElements()
{
	if (_project == nullptr) return;
	clear_selectedElements();
	
	AM_Database_Datatable DTable(_db, &AMLIB::TN_SelectedElements());
	DTable.load_data(AMLIB::TN_SelectedElements().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		for (int n1 = 0; n1 < DTable.row_count(); n1++)
		{
			std::vector<std::string> rowData = DTable.get_row_data(n1);
			_selectedElements.push_back(new DBS_SelectedElements(_db, -1));
			_selectedElements.back()->load(rowData);
		}
	}
}
void AM_Project::load_DBS_CALPHAD()
{
	if (_project == nullptr) return;

	AM_Database_Datatable DTable(_db, &AMLIB::TN_CALPHADDatabase());
	DTable.load_data(AMLIB::TN_CALPHADDatabase().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		if (_calphadDatabases != nullptr) delete _calphadDatabases;
		std::vector<std::string> rowData = DTable.get_row_data(0);
		_calphadDatabases = new DBS_CALPHADDatabase(_db, -1);
		_calphadDatabases->load(rowData);
		return;
	}

	_calphadDatabases = new DBS_CALPHADDatabase(_db, -1);
}
void AM_Project::load_DBS_activePhasesConfig()
{
	if (_project == nullptr) return;

	AM_Database_Datatable DTable(_db, &AMLIB::TN_ActivePhases_Configuration());
	DTable.load_data(AMLIB::TN_ActivePhases_Configuration().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		if (_activePhasesConfig != nullptr) delete _activePhasesConfig;
		std::vector<std::string> rowData = DTable.get_row_data(0);
		_activePhasesConfig = new DBS_ActivePhases_Configuration(_db, -1);
		_activePhasesConfig->load(rowData);
		return;
	}

	_activePhasesConfig = new DBS_ActivePhases_Configuration(_db, -1);
}
void AM_Project::load_DBS_activePhases()
{
	if (_project == nullptr) return;
	clear_activePhases();

	AM_Database_Datatable DTable(_db, &AMLIB::TN_ActivePhases());
	DTable.load_data(AMLIB::TN_ActivePhases().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		for (int n1 = 0; n1 < DTable.row_count(); n1++)
		{
			std::vector<std::string> rowData = DTable.get_row_data(n1);
			_ativePhases.push_back(new DBS_ActivePhases(_db, -1));
			_ativePhases.back()->load(rowData);
		}
	}
}
void AM_Project::load_DBS_activePhases_composition()
{
	if (_project == nullptr) return;
	clear_activePhases_elementCompositons();

	AM_Database_Datatable DTable(_db, &AMLIB::TN_ActivePhases_ElementComposition());
	DTable.load_data(AMLIB::TN_ActivePhases_ElementComposition().columnNames[1] + " = \'" + std::to_string(_project->id()) + "\'");

	if (DTable.row_count() > 0)
	{
		for (int n1 = 0; n1 < DTable.row_count(); n1++)
		{
			std::vector<std::string> rowData = DTable.get_row_data(n1);
			_ativePhases_composition.push_back(new DBS_ActivePhases_ElementComposition(_db, -1));
			_ativePhases_composition.back()->load(rowData);
		}
	}
}

#pragma endregion
