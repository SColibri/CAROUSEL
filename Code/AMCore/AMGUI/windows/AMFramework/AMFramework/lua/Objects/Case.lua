-- Case
Case = {ID = -1, IDProject=-1, IDGroup=0, Name="NewCase", Script="", Date="", PosX=0, PosY=0, PosZ=0, EquilibriumConfiguration=EquilibriumConfig:new{}, ScheilConfiguration={}, equilibriumPhaseFraction={}, scheilPhaseFraction={}, selectedPhases={}, elementComposition={}, precipitationPhases = {}, precipitationDomain = {}, heatTreatment = {} } --@Description Case object. \n Each case contains all calculations and configurations for the ccurrent element composition

-- Constructor
function Case:new (o,ID, IDProject, IDGroup, Name, Script, Date, PosX, PosY, PosZ, SelectedPhases, EquilibriumConfiguration, ScheilConfiguration, equilibriumPhaseFraction, scheilPhaseFraction, selectedPhases, elementComposition, precipitationPhases, precipitationDomain, heatTreatment) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   local o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDGroup = IDGroup or 0
   self.Name = Name or "Empty project"
   self.Script = Script or ""
   self.Date = Date or os.date("%x %X")
   self.PosX = PosX or 0
   self.PosY = PosY or 0
   self.PosZ = PosZ or 0
   self.SelectedPhases = SelectedPhases or {}
   self.Columns = {"ID", "IDProject", "IDGroup", "Name", "Script", "Date", "PosX", "PosY", "PosZ"}

   self.EquilibriumConfiguration = EquilibriumConfiguration or EquilibriumConfig:new{}
   self.ScheilConfiguration = ScheilConfiguration or  ScheilConfig:new{}
   self.equilibriumPhaseFraction = equilibriumPhaseFraction or  {} 
   self.scheilPhaseFraction = scheilPhaseFraction or  {}
   self.selectedPhases = selectedPhases or {}
   self.elementComposition = elementComposition or {}
   self.precipitationPhases = precipitationPhases or {}
   self.precipitationDomain = precipitationDomain or {}
   self.heatTreatment = heatTreatment or {}

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

   if self.ID == -1 then goto continue end

   -- Load Selected Phases
   local sqlData = split(spc_case_load_id(self.ID),",")

   -- Load Equilibrium Configuration
   local sqlData_equilibrium = split(spc_equilibrium_configuration_load_caseID(self.ID),",")
   self.EquilibriumConfiguration = EquilibriumConfig:new{}
   load_data(self.EquilibriumConfiguration, sqlData_equilibrium)

   -- Load Scheil Configuration
   local sqlData_scheil = split(spc_scheil_configuration_load_caseID(self.ID),",")
   self.ScheilConfiguration = ScheilConfig:new{}
   load_data(self.ScheilConfiguration, sqlData_scheil)

   -- Load Element composition
    local sqlData_eComp = split(spc_elementcomposition_load_id_case(self.ID),"\n")
    self.elementComposition = {}
    for i,Item in ipairs(sqlData_eComp) do
        local sqlData_eComp_cells = split(Item,",")
        self.elementComposition[i] = ElementComposition:new{}
        load_data(self.elementComposition[i], sqlData_eComp)
    end

    -- load precipitation phases
    local sqlData_PP = split(spc_precipitation_phase_load_caseID(self.ID),"\n")
    self.precipitationPhases = {}
    load_table_data(self.precipitationPhases, PrecipitationPhase, sqlData_PP)

    -- load precipitation domains
    local sqlData_PD = split(spc_precipitation_domain_load_caseID(self.ID),"\n")
    self.precipitationDomain = {}
    load_table_data(self.precipitationDomain, PrecipitationDomain, sqlData_PD)
    
    -- load heat treatments
    local sqlData_HT = split(spc_heat_treatment_load_IDCase(self.ID),"\n")
    self.heatTreatment = {}
    load_table_data(self.heatTreatment, HeatTreatment, sqlData_HT)

    ::continue::
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
        self.scheilPhaseFraction[i] = ScheilPhaseFraction:new{}
        load_data(self.scheilPhaseFraction[i], sqlData_scheil_cells)
    end

end

-- save
function Case:save()
    local saveString = join(self, ",")
    local saveOut = spc_case_save(saveString)

    if tonumber(saveOut) ~= nil then
        self.ID = tonumber(saveOut) or -1

        self.EquilibriumConfiguration.IDCase = self.ID
        self.EquilibriumConfiguration:save()

        self.ScheilConfiguration.IDCase = self.ID
        self.ScheilConfiguration:save()

        for i,Item in ipairs(self.equilibriumPhaseFraction) do
            self.equilibriumPhaseFraction[i].IDCase = self.ID
            self.equilibriumPhaseFraction[i]:save()
        end

        for i,Item in ipairs(self.scheilPhaseFraction) do
            self.scheilPhaseFraction[i].IDCase = self.ID
            self.scheilPhaseFraction[i]:save()
        end

        for i,Item in ipairs(self.selectedPhases) do
            self.selectedPhases[i].IDCase = self.ID
            self.selectedPhases[i]:save()
        end
        
        for i,Item in ipairs(self.elementComposition) do
            self.elementComposition[i].IDCase = self.ID
            self.elementComposition[i]:save()
        end
        
        for i,Item in ipairs(self.precipitationDomain) do
            self.precipitationDomain[i].IDCase = self.ID
            self.precipitationDomain[i]:save()
        end
        
        for i,Item in ipairs(self.precipitationPhases) do
            self.precipitationPhases[i].IDCase = self.ID
            self.precipitationPhases[i]:save()
        end
        
        for i,Item in ipairs(self.heatTreatment) do
            self.heatTreatment[i].IDCase = self.ID
            self.heatTreatment[i]:save()
        end
        
    end
end

-- Methods
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                                       Selected phases
-- .......................................................................................

function Case:select_phases(In)
    local Etable = split(In," ")

    -- Save before adding selected phases
    if self.ID == -1 then self:save() end
    if #Etable > 0 then
        self:clear_selected_phases()
        self.selectedPhases = {}
        for i,Item in ipairs(Etable) do
            
            local phasey = Phase:new{Name = Item}

            if phasey.ID == -1 then
                get_phaseNames()
                phasey = Phase:new{Name = Item}
            end

            if phasey.ID == -1 then
                error("select_phases: The phase \'" .. Item .. "\' is not contained in the database! :(")
            end
            
            self.selectedPhases[i] = SelectedPhase:new{IDPhase = phasey.ID, IDCase = self.ID}
            self.selectedPhases[i]:save()

        end
    end
end

function Case:clear_selected_phases()
    for i,Item in ipairs(self.selectedPhases) do
        self.selectedPhases[i]:remove()
    end
end

-- .......................................................................................
--                                Equilibrium/Scheil Phase fractions
-- .......................................................................................

function Case:clear_equilibriumPhaseFractions()
    for i,Item in ipairs(self.equilibriumPhaseFraction) do
        self.equilibriumPhaseFraction[i]:remove()
    end
end

function Case:clear_scheilPhaseFraction()
    for i,Item in ipairs(self.scheilPhaseFraction) do
        self.scheilPhaseFraction[i]:remove()
    end
end

-- .......................................................................................
--                                       Element composition
-- .......................................................................................

-- Set composition
function Case:set_composition(In)
    local Etable = split(In," ")

    if #self.elementComposition ~= #Etable then
        self:clear_elementComposition()

        if #self.elementComposition == 0 then 
            error("Case:set_composition -> project has no elements or has not been saved!")
        end
    end

    if #Etable > 0 then
        for i,Item in ipairs(Etable) do
            if tonumber(Item) ~= nil then
                self.elementComposition[i].value = tonumber(Item)
                self.elementComposition[i]:save()
            else
                local STable = split(Etable[i],"=")
                if #STable > 1 then 
                    local tempRef = self:find_composition_ByName(STable[1])
                    tempRef.value = tonumber(STable[2])
                    tempRef:save()
                end
            end
        end
    end
end

-- Composition by name
function Case:find_composition_ByName(nameObj)
    for i,Item in ipairs(self.elementComposition) do
        if self.elementComposition[i].element.Name == nameObj then
            return self.elementComposition[i]
        end
    end

    return nil
end

-- Clear all element compositions
function Case:clear_elementComposition(projectObject)
    for i,Item in ipairs(self.elementComposition) do
        self.elementComposition[i]:remove()
    end

    self.elementComposition = {}
    if self.IDProject > -1 then
        local Pobject = projectObject or Project:new{ID = self.IDproject}
        for i,Item in ipairs(Pobject.selectedElements) do 
            self.elementComposition[i] = ElementComposition:new{IDCase = self.ID, 
                                                                IDElement = Pobject.selectedElements[i].IDElement}

        end
    end
end

-- .......................................................................................
--                                      Precipitation Phases
-- .......................................................................................
function Case:clear_precipitationPhases()
    for i,Item in ipairs(self.precipitationPhases) do
        self.precipitationPhases[i]:remove()
    end
    self.precipitationPhases = {}
end

function Case:add_precipitation_phase(pPhase) 
    -- check before applying
    assert(type(pPhase) == "table", "add_precipitation_phase only accepts as input an object of PrecipitationPhase")
    assert(#pPhase.Name > 0, "Precipitation phase does not have a name")

    -- core before saving checks if phase exists, and if so it only updates the entry
    self.precipitationPhases[#self.precipitationPhases + 1] = pPhase
    self.precipitationPhases[#self.precipitationPhases].ID = -1
    self.precipitationPhases[#self.precipitationPhases].IDCase = self.ID
    self.precipitationPhases[#self.precipitationPhases]:save()

    -- reload Data
    self:load()
end

-- .......................................................................................
--                                      Precipitation Domains
-- .......................................................................................
function Case:clear_precipitationDomains()
    for i,Item in ipairs(self.precipitationDomain) do
        self.precipitationDomain[i]:remove()
    end
    self.precipitationDomain = {}
end

function Case:add_precipitation_domain(pDomain)
    -- check before applying
    assert(type(pDomain) == "table", "add_precipitation_domain only accepts as input an object of PrecipitationDomain")
    assert(#pDomain.Name > 0, "Precipitation domain does not have a name")

    -- core before saving checks if phase exists, and if so it only updates the entry
    self.precipitationDomain[#self.precipitationDomain + 1] = pDomain
    self.precipitationDomain[#self.precipitationDomain].ID = -1
    self.precipitationDomain[#self.precipitationDomain].IDCase = self.ID
    self.precipitationDomain[#self.precipitationDomain]:save()

    -- reload Data
    self:load()
end

-- .......................................................................................
--                                       Heat treatment
-- .......................................................................................

function Case:clear_heat_treatments()
    for i,Item in ipairs(self.heatTreatment) do
        self.heatTreatment[i]:remove()
    end
    self.heatTreatment = {}
end

function Case:add_heat_treatment(hTreatment)
    -- check before applying
    assert(type(hTreatment) == "table", "add_heat_treatment only accepts as input an object of HeatTreatment")
    assert(#hTreatment.Name > 0, "Heat treatment does not have a name")

     -- core before saving checks if phase exists, and if so it only updates the entry
    self.heatTreatment[#self.heatTreatment + 1] = hTreatment
    self.heatTreatment[#self.heatTreatment].ID = -1
    self.heatTreatment[#self.heatTreatment].IDCase = self.ID
    self.heatTreatment[#self.heatTreatment]:save()

    -- reload
    self:load()
end


-- Calculations
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                             Calculate precipitate distribution
-- .......................................................................................

-- Single case calculation (for parallel implementation check project level/object)
function Case:calculate_heat_treatment(HeatTreatment_Name)
    -- Check input
    assert(type(HeatTreatment_Name) == "string", "calculate_precipitate_distribution requires a string as an input")

    -- find specific heat treatment
    for i,Item in ipairs(self.heatTreatment) do
        if Item.Name == HeatTreatment_Name then
            Item:run_kinetic_simulation()
            goto ENDCALC
        end
    end

    ::ENDCALC::

    -- reload
    self:load()
end