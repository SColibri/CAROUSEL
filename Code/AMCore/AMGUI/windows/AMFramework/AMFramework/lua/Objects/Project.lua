-- Project
Project = {ID = -1, Name = "No name", API = "", ExternalAPI = "" , Columns ={} , selectedElements = {}, cases = {}} --@Description Project object. \n Project controlls all lower level objects

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
   self.cases = cases or {}

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
        local sqlDataRow_Cases = split(spc_case_load_project_id(self.ID),"\n")
        load_table_data(self.cases, Case, sqlDataRow_Cases)
               
        -- Load Selected Elements
        local sqlDataRow_Elements = split(spc_selectedelement_load_id_project(self.ID),"\n")
        load_table_data(self.selectedElements, SelectedElement, sqlDataRow_Elements)
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
        self.ID = tonumber(saveOut)

        for i,Item in ipairs(self.cases) do
            self.cases[i].IDProject = self.ID
            self.cases[i]:save()
        end
    end
end

-- Methods
function Project:clear()
    if self.ID == -1 then goto continue end
    project_remove_dependentData(self.ID)
    ::continue::
end

function Project:select_elements(In)
    local Etable = split(In," ")
    if #Etable > 0 then
        self:clear_selected_elements()
        self.selectedElements = {}
        for i,Item in ipairs(Etable) do
            
            local elemy = Element:new{Name = Item}
            if elemy.ID == -1 then
                error("select_elements: The element \'" .. Item .. "\' is not contained in the database! :(")
            end
            
            self.selectedElements[i] = SelectedElement:new{IDElement = elemy.ID, IDProject = self.ID}
            self.selectedElements[i]:save()
            ::continue::
        end
    end
end

function Project:clear_selected_elements()
    for i,Item in ipairs(self.selectedElements) do
        self.selectedElements[i]:remove()
    end
end

