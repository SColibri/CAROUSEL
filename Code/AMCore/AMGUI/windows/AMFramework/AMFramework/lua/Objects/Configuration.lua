-- Item
Configuration = {API_path = "", External_API_path = "", Working_directory = "", Thermodynamic_database = "", Physical_database = "", Mobility_database = ""} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function Configuration:new (o,API_path,External_API_path,Working_directory,Thermodynamic_database,Physical_database,Mobility_database) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.API_path = API_path or ""
   self.External_API_path = External_API_path or ""
   self.Working_directory = Working_directory or ""
   self.Thermodynamic_database = Thermodynamic_database or ""
   self.Physical_database = Physical_database or ""
   self.Mobility_database = Mobility_database or ""

   o:load()

   return o
end

-- load
function Configuration:load ()
   self.API_path = configuration_getAPI_path()
   self.External_API_path = configuration_getExternalAPI_path()
   self.Working_directory = configuration_get_working_directory()
   self.Thermodynamic_database = configuration_get_thermodynamic_database_path()
   self.Physical_database = configuration_get_physical_database_path()
   self.Mobility_database = configuration_get_mobility_database_path()
end

-- save
function Configuration:save()
   self.API_path = configuration_setAPI_path()
   self.External_API_path = configuration_setExternalAPI_path()
   self.Working_directory = configuration_set_working_directory()
   self.Thermodynamic_database = configuration_set_thermodynamic_database_path()
   self.Physical_database = configuration_set_physical_database_path()
   self.Mobility_database = configuration_set_mobility_database_path()
end
