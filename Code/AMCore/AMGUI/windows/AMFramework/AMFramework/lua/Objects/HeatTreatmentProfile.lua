-- Item
HeatTreatmentProfile = {ID = -1, IDHeatTreatment = -1, Time = 0.0, Temperature = 0} --@Description pahse object. \n phase information, this should be loaded from a database

-- Constructor
function HeatTreatmentProfile:new (o,ID,IDHeatTreatment,Time,Temperature) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDHeatTreatment = IDHeatTreatment or -1
   self.Time = Time or -1
   self.Temperature = Temperature or -1
  
   
   self.Columns = {"ID","IDHeatTreatment","Time","Temperature"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function HeatTreatmentProfile:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_heat_treatment_profile_load_id(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function HeatTreatmentProfile:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_heat_treatment_profile_save(saveString)) or -1
end

-- remove
function HeatTreatmentProfile:remove()
    spc_heat_treatment_profile_delete(self.ID)
end