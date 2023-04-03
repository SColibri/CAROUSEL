
ProjectBuilder =  {Name = "New Project", ProjectObject = {}, ConfigurationObject = {}}  --@Description Project builder object. \n This is a project wrapper that allows for easy project building

function ProjectBuilder:new(o, Name) --@Description Creates a new project builder,\n 
	local o = o or {}
	setmetatable(o, self)
	self.__index = self

	o.Name = Name or "New Project"
	o.ProjectObject = Project:new{Name = o.Name}
	o.ConfigurationObject = Configuration:new{}

	-- Get values from configuration
	o.Project.API = o.ConfigurationObject.API_path
	o.Project.ExternalAPI = o.ConfigurationObject.External_API_path

	return o
end

-- -------------------------------------------------------------------
--									SETTERS
-- -------------------------------------------------------------------
function ProjectBuilder:setThermodynamicDatabasePath(stringPath)
	self.ProjectObject.Databases.ThermodynamicDatabase = stringPath
	self.ConfigurationObject.Thermodynamic_database = stringPath
	initialize_core()
end

function ProjectBuilder:setMobilityDatabasePath(stringPath)
	self.ProjectObject.Databases.MobilityDatabase = stringPath
	self.ConfigurationObject.Mobility_database = stringPath
end

function ProjectBuilder:setPhysicalDatabasePath(stringPath)
	self.ProjectBuilder.Databases.PhysicalDatabase = stringPath
	self.ConfigurationObject.Physical_database = stringPath
end

function ProjectBuilder:setMaximumThreadNumber(maxNumber)
	self.ConfigurationObject.Max_thread_number = maxNumber
end

function ProjectBuilder:setReferencceElement(referenceElement)
	self.Project:set_reference_element(referenceElement)
end

-- -------------------------------------------------------------------
--								 METHODS
-- -------------------------------------------------------------------
function ProjectBuilder:save()
	self.Project:save()
	configuration_save()
end

function ProjectBuilder:selectElements(elementSelection)
	self.Project:select_elements(elementSelection)
end




