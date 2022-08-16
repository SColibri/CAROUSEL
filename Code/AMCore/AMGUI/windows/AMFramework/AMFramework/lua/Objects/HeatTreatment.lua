-- Item
HeatTreatment = {ID = -1, IDCase = -1, Name = "", MaxTemperatureStep = 10, IDPrecipitationDomain = -1, StartTemperature = 100, TemperatureProfile = {}, SimulationData = {}, Segments = {} } --@Description 

-- Constructor
function HeatTreatment:new (o,ID,IDCase,Name,MaxTemperatureStep,IDPrecipitationDomain,StartTemperature,TemperatureProfile,SimulationData, Segments) --@Description 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.Name = Name or ""
   self.MaxTemperatureStep = MaxTemperatureStep or 10
   self.IDPrecipitationDomain = IDPrecipitationDomain or -1
   self.StartTemperature = StartTemperature or 100
   
   self.Columns = {"ID","IDCase","Name","MaxTemperatureStep","IDPrecipitationDomain","StartTemperature"}
   self.TemperatureProfile = TemperatureProfile or {}
   self.SimulationData = SimulationData or {}
   self.Segments = Segments or {}
   
   if o.ID > -1 or string.len(o.Name) > 1 then
    o:load()
   end

   return o
end

-- load
function HeatTreatment:load()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_heat_treatment_load_id(self.ID),",")
   elseif string.len(self.Name) > 0 then
    sqlData = split(spc_heat_treatment_load_ByName(self.Name),",")
   end
   
   load_data(self, sqlData)
   
   if self.ID > -1 then
      --load all segments
      self.Segments = {}
      local sqlDataSegments = split(spc_heat_treatment_segment_load_IDHeatTreatment(self.ID),"\n")
      load_table_data(self.Segments, HeatTreatmentSegment, sqlDataSegments)
   end
end

-- save
function HeatTreatment:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_heat_treatment_save(saveString)) or -1
end

-- remove
function HeatTreatment:remove()
    spc_heat_treatment_delete(self.ID)
end

--Methods
function HeatTreatment:clear_all()
    for i,Item in ipairs(self.TemperatureProfile) do
      Item.remove()
    end
    self.TemperatureProfile = {}
    
    for i,Item in ipairs(self.SimulationData) do
      Item.remove()
    end
    self.SimulationData = {}
    
    for i,Item in ipairs(self.Segments) do
      Item.remove()
    end
    self.Segments = {}
end

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

function HeatTreatment:run_kinetic_simulation()
  local caseRef = Case:new{ID = self.IDCase}
  pixelcase_calculate_heat_treatment(caseRef.IDProject, caseRef.ID.."-"..caseRef.ID, self.Name)
end