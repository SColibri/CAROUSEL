-- Item
Configuration = {API_path = "", External_API_path = "", Working_directory = "", Thermodynamic_database = "", Physical_database = "", Mobility_database = "", Max_thread_number = 1} --@Description Configuration object. \n System configurations and others can be found here

-- Constructor
function Configuration:new (o,API_path,External_API_path,Working_directory,Thermodynamic_database,Physical_database,Mobility_database,Max_thread_number) --@Description Creates a new Configuration object,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.API_path = API_path or ""
   self.External_API_path = External_API_path or ""
   self.Working_directory = Working_directory or ""
   self.Thermodynamic_database = Thermodynamic_database or ""
   self.Physical_database = Physical_database or ""
   self.Mobility_database = Mobility_database or ""
   self.Max_thread_number = Max_thread_number or 1
   self.AMName = "Configuration"

   o:load()

   return o
end

-- load
function Configuration:load () --@Description loads a configuration setup
   self.API_path = configuration_getAPI_path()
   self.External_API_path = configuration_getExternalAPI_path()
   self.Working_directory = configuration_get_working_directory()
   self.Thermodynamic_database = configuration_get_thermodynamic_database_path()
   self.Physical_database = configuration_get_physical_database_path()
   self.Mobility_database = configuration_get_mobility_database_path()
   self.Max_thread_number = configuration_get_max_thread_number()
end

-- save
function Configuration:save() --@Description saves a configuration setup
    configuration_setAPI_path(self.API_path)
	configuration_set_thermodynamic_database_path(self.Thermodynamic_database)
	configuration_set_physical_database_path(self.Physical_database)
	configuration_set_mobility_database_path(self.Mobility_database)
	configuration_set_max_thread_number(self.Max_thread_number)
	configuration_save()
end
