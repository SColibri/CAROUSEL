-- Item
PrecipitateSimulationData = {ID = -1, IDPrecipitationPhase = -1, IDHeatTreatment = -1, Time = 0.0, PhaseFraction = 0, NumberDensity = 0, MeanRadius = 0.0} --@Description pahse object. \n phase information, this should be loaded from a database

-- Constructor
function PrecipitateSimulationData:new (o,ID,IDPrecipitationPhase,IDHeatTreatment,Time,PhaseFraction,NumberDensity,MeanRadius) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDPrecipitationPhase = IDPrecipitationPhase or -1
   self.IDHeatTreatment = IDHeatTreatment or -1
   self.Time = Time or 0.0
   self.PhaseFraction = PhaseFraction or 0.0
   self.NumberDensity = NumberDensity or 0.0
   self.MeanRadius = MeanRadius or 0.0
   
   self.Columns = {"ID","IDPrecipitationPhase","IDHeatTreatment","Time","PhaseFraction","NumberDensity","MeanRadius"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function PrecipitateSimulationData:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_precipitation_simulation_data_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function PrecipitateSimulationData:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_precipitation_simulation_data_save(saveString)) or -1
end

-- remove
function PrecipitateSimulationData:remove()
    spc_precipitation_simulation_data_delete(self.ID)
end