-- Case
Case = {ID = -1, IDProject=-1, IDGroup=0, Name="NewCase", Script="", Date="", PosX=0, PosY=0, PosZ=0, EquilibriumConfiguration={}, ScheilConfiguration={}, EquilibriumPhaseFraction={}, ScheilConfigurationPhaseFraction={}} --@Description Case object. \n Each case contains all calculations and configurations for the ccurrent element composition

-- Constructor
function Case:new (o,ID, IDProject, IDGroup, Name, Script, Date, PosX, PosY, PosZ, SelectedPhases, EquilibriumConfiguration, ScheilConfiguration, EquilibriumPhaseFraction, ScheilConfigurationPhaseFraction) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   local o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDGroup = IDGroup or 0
   self.Name = Name or "Empty project"
   self.Script = Script or ""
   self.Date = Date or ""
   self.PosX = PosX or 0
   self.PosY = PosY or 0
   self.PosZ = PosZ or 0
   self.SelectedPhases = SelectedPhases or {}
   self.Columns = {"ID", "IDProject", "IDGroup", "Name", "Script", "Date", "PosX", "PosY", "PosZ"}
   self.EquilibriumConfiguration = EquilibriumConfig:new{}
   self.ScheilConfiguration = ScheilConfig:new{}
   self.equilibriumPhaseFraction = {}
   self.scheilConfigurationPhaseFraction = {}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function Case:load()
   -- Load CaseData
   local sqlData = split(spc_case_load_id(self.ID),",")
   load_data(self, sqlData)

   -- Load Selected Phases
   if self.ID > -1 then
    local sqlData = split(spc_case_load_id(self.ID),",")
   end

   -- Load Equilibrium Configuration
   if self.ID > -1 then
    local sqlData_equilibrium = split(spc_equilibrium_configuration_load_caseID(self.ID),",")
    self.EquilibriumConfiguration = EquilibriumConfig:new{}
    load_data(self.EquilibriumConfiguration, sqlData_equilibrium)
   end

   -- Load Scheil Configuration
   if self.ID > -1 then
    local sqlData_scheil = split(spc_scheil_configuration_load_caseID(self.ID),",")
    self.ScheilConfiguration = ScheilConfig:new{}
    load_data(self.ScheilConfiguration, sqlData_scheil)
   end

end

function Case:load_phase_fractions() --@Description This loads data for the phase diagram.

    -- equilibrium phase fractions
    local sqlData_equilibrium = split(spc_equilibrium_phasefraction_load_caseID(self.ID),"\n")
    for i,Item in ipairs(sqlData_equilibrium) do
        local sqlData_equilibrium_cells = split(Item,",")
        self.equilibriumPhaseFraction[i] = EquilibriumPhaseFraction:new{}
        load_data(self.equilibriumPhaseFraction[i], sqlData_equilibrium_cells)
    end

    -- scheil phase fractions
    local sqlData_scheil = split(spc_scheil_phasefraction_load_caseID(self.ID),"\n")
    for i,Item in ipairs(sqlData_scheil) do
        local sqlData_scheil_cells = split(Item,",")
        self.scheilConfigurationPhaseFraction[i] = ScheilPhaseFraction:new{}
        load_data(self.scheilConfigurationPhaseFraction[i], sqlData_scheil_cells)
    end

end

-- save
function Case:save()
    local saveString = join(self, ",")
    spc_case_save(saveString)
end
