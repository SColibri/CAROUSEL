-- Item
EquilibriumConfig = {ID = -1, IDCase=-1, Temperature=700, StartTemperature = 700, EndTemperature = 25, TemperatureType = "C", StepSize = 25, Pressure = 101325} --@Description Equilibrium configuration object. \n configuration for step equilibrium is done using this object

-- Constructor
function EquilibriumConfig:new (o,ID, IDCase, Temperature, StartTemperature, EndTemperature, TemperatureType, StepSize, Pressure) --@Description
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.Temperature = Temperature or 700
   self.StartTemperature = StartTemperature or 700
   self.EndTemperature = EndTemperature or 25
   self.TemperatureType = TemperatureType or "C"
   self.StepSize = StepSize or 25
   self.Pressure = Pressure or 101325
   
   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function EquilibriumConfig:load ()
   sqlData = split(spc_equilibrium_configuration_load_caseID(self.IDCase))
   load_data(self, sqlData)
end

-- save
function EquilibriumConfig:save()
    local saveString = join(self, ",")
    spc_equilibrium_configuration_save(saveString)
end