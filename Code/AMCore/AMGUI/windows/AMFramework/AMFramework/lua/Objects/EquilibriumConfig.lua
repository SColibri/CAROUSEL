-- Item
EquilibriumConfig = {ID = -1, IDCase=-1, Temperature=700, StartTemperature = 700, EndTemperature = 25, TemperatureType = "C", StepSize = 25, Pressure = 101325} --@Description Equilibrium configuration object. \n configuration for equilibrium solidification simulations

-- Constructor
function EquilibriumConfig:new (o,ID, IDCase, Temperature, StartTemperature, EndTemperature, TemperatureType, StepSize, Pressure) --@Description new Equilibrium configuration object
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
   self.Columns = {"ID","IDCase","Temperature","StartTemperature","EndTemperature","TemperatureType","StepSize", "Pressure"}
   
   if o.ID > -1 then
    o:load()
   elseif o.IDCase > -1 then
    o:loadByCase()
   end

   return o
end

-- load
function EquilibriumConfig:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = split(spc_equilibrium_configuration_loadID(self.ID))
   load_data(self, sqlData)
end

function EquilibriumConfig:loadByCase () --@Description Loads configuration from ID of case object specified with IDCase
   local sqlData = split(spc_equilibrium_configuration_load_caseID(self.IDCase),",")
   load_data(self, sqlData)
end

-- save
function EquilibriumConfig:save() --@Description Saves an object into the database, if ID = -1 it creates a new entry.
    -- If IDCase is not defined we do not save this configuration
    if self.IDCase == -1 then goto continue end

    -- Check if a configuration has been defined for this case
    if self.ID == -1 then
        local tempE = EquilibriumConfig:new{IDCase=self.IDCase}
        if tempE.ID > -1 then self.ID = tempE.ID end
    end

    -- Save data
    local saveString = join(self, ",")
    self.ID = tonumber(spc_equilibrium_configuration_save(saveString)) or -1

    ::continue::
end

-- Methods
function EquilibriumConfig:run() --@Description Run equilibrium solidification simulation
    if self.IDCase > -1 then
        if self.ID == -1 then self:save() end

        local caseRef = Case:new{ID = self.IDCase}
        pixelcase_step_equilibrium_parallel(caseRef.IDProject, tostring(self.IDCase) .. '-' .. tostring(self.IDCase))
        
    end
end