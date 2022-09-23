-- Project
Project = {ID = -1, Name = "No name", API = "", ExternalAPI = "" , Columns ={} , selectedElements = {}, cases = {}, ActivePhases = {}, ActivePhasesConfig = {}, ActivePhasesElementComposition = {}} --@Description Project object. \n Project controlls all lower level objects

-- Constructor
function Project:new (o,ID,Name,API,ExternalAPI,selectedElements,cases) --@Description Creates a new project,\n create new object by calling newVar = Project:new{Name = value} or Project:new({},ID,Name).
   local o = o or {}
   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or "Empty project"
   self.API = API or ""
   self.ExternalAPI = ExternalAPI or ""

   self.Columns = {"ID","Name","API","ExternalAPI"}
   self.selectedElements = selectedElements or {}
   self.cases = cases or {} --@TYPE Case

   --Active Phases
   self.ActivePhases = {} --@TYPE ActivePhases
   self.ActivePhasesConfig = {}
   self.ActivePhasesElementComposition = {}

   if o.ID > -1 or o.Name ~= "Empty project" then
    o:load()
   end

   return o
end

-- Load
function Project:load ()
   
   local sqlData_project
   if self.ID > -1 then
    sqlData_project = split(project_loadID(self.ID),",")
   else
    sqlData_project = split(project_load_data(self.Name),",")
   end

   local tempName = self.Name
   load_data(self, sqlData_project)
   
    -- Load Project data
    if self.ID > -1 then

        -- Load Cases
        self.cases = {}
        local sqlDataRow_Cases = split(spc_case_load_project_id(self.ID),"\n")
        load_table_data(self.cases, Case, sqlDataRow_Cases)
               
        -- Load Selected Elements
        self.selectedElements = {}
        local sqlDataRow_Elements = split(spc_selectedelement_load_id_project(self.ID),"\n")
        load_table_data(self.selectedElements, SelectedElement, sqlDataRow_Elements)

        -- Load Active phases
        self.ActivePhases = {}
        self.ActivePhasesElementComposition = {}
        self.ActivePhasesConfig = ActivePhasesConfig:new{IDProject = self.ID}

        local sqlDataRow_AP = split(project_active_phases_load_IDProject(self.ID),"\n")
        local sqlDataRow_APEC = split(project_active_phases_element_composition_load_IDProject(self.ID),"\n")

        load_table_data(self.ActivePhases, ActivePhases, sqlDataRow_AP)
        load_table_data(self.ActivePhasesElementComposition, ActivePhasesElementComposition, sqlDataRow_APEC)

    else
        if string.len(tempName) > 0 then
         self.Name = tempName
         self.ID = project_new(self.Name)
        end
    end

end

-- Save
function Project:save()
    local saveString = join(self, ",")
    local saveOut = project_save(saveString)

    if tonumber(saveOut) ~= nil then
        self.ID = tonumber(saveOut) or -1

        for i,Item in ipairs(self.cases) do
            self.cases[i].IDProject = self.ID
            self.cases[i]:save()
        end

        self.ActivePhasesConfig.IDProject = self.ID
        self.ActivePhasesConfig:save()
    end
end

-- Methods
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                                   Self
-- .......................................................................................

-- clear all related data
function Project:clear()
    if self.ID == -1 then goto continue end
    project_remove_dependentData(self.ID)

    self.selectedElements = {}
    self.cases = {}
    
    ::continue::
end

-- .......................................................................................
--                                   Elements
-- .......................................................................................

-- select active elements
function Project:select_elements(In)
    local Etable = split(In," ")
    if #Etable > 0 then
        self:clear_selected_elements()
        self.selectedElements = {}
        for i,Item in ipairs(Etable) do
            
            local elemy = Element:new{Name = Item}

            if elemy.ID == -1 then
                get_elementNames()
                elemy = Element:new{Name = Item}
            end

            if elemy.ID == -1 then
                error("select_elements: The element \'" .. Item .. "\' is not contained in the database! :(")
            end
            
            self.selectedElements[i] = SelectedElement:new{IDElement = elemy.ID, IDProject = self.ID}
            self.selectedElements[i]:save()
            ::continue::
        end

        -- update cases will remove calculation data
        self:update_cases()

        self:update_active_phases()
    end
end

-- remove all selected elements
function Project:clear_selected_elements()
    for i,Item in ipairs(self.selectedElements) do
        self.selectedElements[i]:remove()
    end
end

-- set reference element
function Project:set_reference_element(refElement)
    local indexNum = self:get_element_index(refElement)
    
    if indexNum > -1 then
        for i,Item in ipairs(self.selectedElements) do
            Item.IsReferenceElement = 0
            Item:save()
        end

        self.selectedElements[indexNum].IsReferenceElement = 1
        self.selectedElements[indexNum]:save()
    end
end

-- return element index in object list
function Project:get_element_index(elementName)
    if #self.selectedElements > 0 then
        
        for i,Item in ipairs(self.selectedElements) do
            if string.len(self.selectedElements[i].Name) == 0 then self.selectedElements[i]:load() end
            if string.upper(self.selectedElements[i].Name) == string.upper(elementName) then
                return i
            end
        end

    else
        msgbox("There are no selected elements")
        error("The element "..elementName.." is not selected, go to project definition and add the element")
    end

    return -1
end

-- return element index that is set as referencce element
function Project:get_reference_element_index()
    if #self.selectedElements > 0 then
        
        for i,Item in ipairs(self.selectedElements) do
            if self.selectedElements[i].IsReferenceElement == 1 then
                return i
            end
        end

    else
        msgbox("There are no selected elements")
    end

    return -1
end

-- returns the reference element
function Project:get_reference_element()
    local refIndex = self:get_reference_element_index()

    if refIndex > -1 then
        return self.selectedElements[refIndex]
    end

    return nil
end

-- .......................................................................................
--                                   Cases
-- .......................................................................................

-- add case
function Project:add_case(caseObject)
    table.insert(self.cases, caseObject)
    caseObject.IDProject = self.ID
    caseObject:clear_elementComposition(self)
    self:save()
end

-- not sure if we ever use this, TODO: check for removal
function Project:update_cases()

    for i,Item in ipairs(self.cases) do
        self.cases[i]:clear_elementComposition(self)
        self.cases[i]:clear_equilibriumPhaseFractions()
        self.cases[i]:clear_scheilPhaseFraction()
        self.cases[i]:clear_precipitationPhases()
        self.cases[i]:clear_precipitationDomains()
    end

end

-- .......................................................................................
--                                   Active phases
-- .......................................................................................

-- get active phases based on current configuration
function Project:update_active_phases()
    
    -- Remove all previously calculated active phases
    for i,Item in ipairs(self.ActivePhases) do
        self.ActivePhases[i]:remove()
    end
    self.ActivePhases = {}

    -- Remove previous Element compositions
    self:update_active_phases_element_composition()

end

-- Active phases element composition configuration update objects to hold the ccorrect ID's
function Project:update_active_phases_element_composition()
    for i,Item in ipairs(self.ActivePhasesElementComposition) do
        self.ActivePhasesElementComposition[i]:remove()
    end
    self.ActivePhasesElementComposition = {}
    
    assert(self.ID ~= -1,"update_active_phases: Project is not saved")
    for i,Item in ipairs(self.selectedElements) do
       self.ActivePhasesElementComposition[i] = ActivePhasesElementComposition:new{IDProject = self.ID, IDElement = Item.IDElement}
       self.ActivePhasesElementComposition[i].IDProject = self.ID
       self.ActivePhasesElementComposition[i]:save()
    end
end

--  Active phases set composition for calculation
function Project:active_phases_set_composition(In)
    local Etable = split(In," ")

    if #self.ActivePhasesElementComposition < #Etable then
        self.update_active_phases_element_composition()

        if #self.ActivePhasesElementComposition == 0 then 
            error("active_phases_set_composition:set_composition -> project has no elements or has not been saved!")
        elseif #self.ActivePhasesElementComposition < #Etable then
            error("active_phases_set_composition:set_composition -> The project does not contain so many selected elements!")
        end
    end

    if #Etable > 0 then
        for i,Item in ipairs(Etable) do
            if tonumber(Item) ~= nil then
                self.ActivePhasesElementComposition[i].Value = tonumber(Item)
                self.ActivePhasesElementComposition[i]:save()
            else
                local STable = split(Etable[i],"=")
                if #STable > 1 then 
                    local tempRef = self:find_composition_ByName(STable[1])
                    tempRef.Value = tonumber(STable[2])
                    tempRef:save()
                end
            end
        end
    end

    self:composition_setReference()
end

-- Active phases find composition by element name
function Project:find_composition_ByName(nameObj)
    for i,Item in ipairs(self.ActivePhasesElementComposition) do
        if self.ActivePhasesElementComposition[i].element.Name == nameObj then
            return self.ActivePhasesElementComposition[i]
        end
    end

    return nil
end

-- Active phases set reference element
function Project:composition_setReference() --@Description Uses the reference element to sum all compositions up to 100% (weight percentage)
    local IDReferenceElement = -1

    for i,Item in ipairs(self.selectedElements) do
        if self.selectedElements[i].IsReferenceElement == 1 then
            IDReferenceElement = self.selectedElements[i].IDElement
        end
    end

    local TotSum = 0
    local referenceElement = {}

    for i,Item in ipairs(self.ActivePhasesElementComposition) do
        if self.ActivePhasesElementComposition[i].IDElement ~= IDReferenceElement then
            TotSum = TotSum + self.ActivePhasesElementComposition[i].Value
        else
            referenceElement = self.ActivePhasesElementComposition[i]
        end
    end

    if referenceElement.Value ~= nil then
        referenceElement.Value = 100 - TotSum
        referenceElement:save()
    end

end

-- Calculations
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                                   Active phases
-- .......................................................................................

-- Calculation get active phases
function Project:get_active_phases() --@Description Using scheil method returns a list of active phases
    if self.ID == -1 then error("get_active_phases: Project does not exist! save the project before doing any calculations!") end
    get_active_phases(self.ID)
end

-- Calculation heat treatments
function Project:get_heat_treatments() --@Description Runs heat treatment calculations for all cases that have heat treatments
    if self.ID == -1 then error("get_heat_treatments: Project does not exist! save the project before doing any calculations!") end
    project_calculate_heat_treatments(self.ID)
end
