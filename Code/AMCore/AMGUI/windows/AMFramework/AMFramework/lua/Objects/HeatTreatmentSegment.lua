-- Item
HeatTreatmentSegment = {ID = -1, stepIndex = -1, IDHeatTreatment = -1, IDPrecipitationDomain = -1, EndTemperature = 0.0, TemperatureGradient = 0.0, Duration = 0.0} --@Description pahse object. \n phase information, this should be loaded from a database

-- Constructor
function HeatTreatmentSegment:new (o,ID,stepIndex,IDHeatTreatment,IDPrecipitationDomain,EndTemperature,TemperatureGradient,Duration) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.stepIndex = stepIndex or 1
   self.IDHeatTreatment = IDHeatTreatment or -1
   self.IDPrecipitationDomain = IDPrecipitationDomain or -1
   self.EndTemperature = EndTemperature  or 0.0
   self.TemperatureGradient = TemperatureGradient or 0.0
   self.Duration = Duration or 0.0
   
   self.Columns = {"ID","stepIndex","IDHeatTreatment","IDPrecipitationDomain","EndTemperature","TemperatureGradient","Duration"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function HeatTreatmentSegment:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_heat_treatment_segment_load_id(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function HeatTreatmentSegment:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_heat_treatment_segment_save(saveString)) or -1
end

-- remove
function HeatTreatmentSegment:remove()
    spc_heat_treatment_segment_delete(self.ID)
end