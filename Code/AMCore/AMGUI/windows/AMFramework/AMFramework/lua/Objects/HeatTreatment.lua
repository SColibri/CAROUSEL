-- Item
HeatTreatment = {ID = -1, Name = "", MaxTemperatureStep = 10, IDPrecipitationDomain = -1, StartTemperature = 100, TemperatureProfile = {}, SimulationData = {}} --@Description 

-- Constructor
function HeatTreatment:new (o,ID,Name,MaxTemperatureStep,IDPrecipitationDomain,StartTemperature,TemperatureProfile,SimulationData) --@Description 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or ""
   self.MaxTemperatureStep = MaxTemperatureStep or 10
   self.IDPrecipitationDomain = IDPrecipitationDomain or -1
   self.StartTemperature = StartTemperature or 100
   
   self.Columns = {"ID","Name","MaxTemperatureStep","IDPrecipitationDomain","StartTemperature"}
   self.TemperatureProfile = TemperatureProfile or {}
   self.SimulationData = SimulationData or {}
   
   if o.ID > -1 or Name ~= "" then
    o:load()
   end

   return o
end

-- load
function HeatTreatment:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_heat_treatment_load_id(self.ID),",")
   else
    sqlData = split(spc_heat_treatment_load_ByName(string.upper(self.Name)),",")
   end
   
   load_data(self, sqlData)
end

-- save
function HeatTreatment:save()
    self.Name = string.upper(self.Name)
    local saveString = join(self, ",")
    self.ID = tonumber(spc_heat_treatment_save(saveString)) or -1
end

-- remove
function HeatTreatment:remove()
    spc_heat_treatment_delete(self.ID)
end

--Methods
function HeatTreatment:load_temperature_profile()
  local sqlData = split(spc_heat_treatment_profile_load_IDHeatTreatment(self.ID),"\n")
  self.TemperatureProfile = {}
  load_table_data(self.TemperatureProfile, HeatTreatmentProfile, sqlData)
end

function HeatTreatment:load_simulation_data()
  local sqlData = split(spc_precipitation_simulation_data_HeatTreatmentID(self.ID),"\n")
  self.SimulationData = {}
  load_table_data(self.SimulationData, PrecipitateSimulationData, sqlData)
end