require"AM_StringManipulators"

-- Project
ScheilConfig = {ID = -1, IDCase = -1, StartTemperature = 700, EndTemperature = 25, TemperatureType = "C", StepSize = 25, DependentPhase = 1, Min_Liquid_Fraction=0.01} --@Description Case object. \n Each case contains all calculations and configurations for the ccurrent element composition

-- Constructor
function ScheilConfig:new (ID, IDCase, StartTemperature, EndTemperature, TemperatureType, StepSize, DependentPhase, Min_Liquid_Fraction) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
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
   self.Columns = {"ID","StartTemperature","EndTemperature","StepSize","TemperatureType","DependentPhase","Min_Liquid_Fraction"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ScheilConfig:load ()
   sqlData = split(spc_scheil_configuration_load_caseID(self.CaseID),",")
   load_data(self, sqlData)
end

-- save
function ScheilConfig:save()
    local saveString = join(self, ",")
    spc_scheil_configuration_save(saveString)
end