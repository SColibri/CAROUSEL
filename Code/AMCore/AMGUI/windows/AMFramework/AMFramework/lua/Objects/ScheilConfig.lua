-- Project
ScheilConfig = {ID = -1, IDCase = -1, StartTemperature = 700, EndTemperature = 25, StepSize = 25, DependentPhase = 1, Min_Liquid_Fraction = 0.01} --@Description Case object. \n Each case contains all calculations and configurations for the ccurrent element composition

-- Constructor
function ScheilConfig:new (o, ID, IDCase, StartTemperature, EndTemperature, StepSize, DependentPhase, Min_Liquid_Fraction) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.StartTemperature = StartTemperature or 700
   self.EndTemperature = EndTemperature or 25
   self.StepSize = StepSize or 25
   self.DependentPhase = Pressure or 101325
   self.Min_Liquid_Fraction = Min_Liquid_Fraction or 0.01
   self.Columns = {"ID","IDCase","StartTemperature","EndTemperature","StepSize","DependentPhase","Min_Liquid_Fraction"}

   if o.ID > -1 then
    o:load()
   elseif o.IDCase > -1 then
    o:loadByCase()
   end

   return o
end

-- load
function ScheilConfig:load ()
   sqlData = split(spc_scheil_configuration_loadID(self.ID),",")
   load_data(self, sqlData)
end

function ScheilConfig:loadByCase ()
   sqlData = split(spc_scheil_configuration_load_caseID(self.IDCase),",")
   load_data(self, sqlData)
end

-- save
function ScheilConfig:save()
    local saveString = join(self, ",")
    spc_scheil_configuration_save(saveString)
end

-- Methods
function ScheilConfig:run()
    if self.IDCase > -1 then
        if ID == -1 then self:save() end

        local caseRef = Case:new{ID = self.IDCase}
        pixelcase_step_scheil_parallel(caseRef.IDProject, tostring(self.IDCase) .. '-' .. tostring(self.IDCase))

    end
end