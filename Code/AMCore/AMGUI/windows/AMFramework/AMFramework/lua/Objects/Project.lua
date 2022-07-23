-- Project
Project = {ID = -1, Name = "No name", API = "", ExternalAPI = "", SelectedElements = {}, Cases = {}, Columns ={} } --@Description Project object. \n Project controlls all lower level objects

-- Constructor
function Project:new (o,ID,Name,API,ExternalAPI,SelectedElements,Cases) --@Description Creates a new project,\n create new object by calling newVar = Project:new{Name = value} or Project:new({},ID,Name).
   local o = o or {}
   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or "Empty project"
   self.API = API or ""
   self.ExternalAPI = ExternalAPI or ""
   self.SelectedElements = SelectedElements or {}
   self.Cases = Cases or {}
   self.Columns = {"ID","Name","API","ExternalAPI"}

   if o.ID > -1 then
    o:load()
   elseif o.Name ~= "Empty project" then
    o:load()
   end

   return o
end

-- Derived class method printArea
function Project:load ()
   
   local sqlData_project
   if self.ID > -1 then
    sqlData_project = split(project_load_data(self.ID),",")
   else
    sqlData_project = split(project_load_data(self.Name),",")
   end
   load_data(self, sqlData_project)
   
    -- Load Project data
    if type(tonumber(sqlData_project[1])) == "number" then
        
        -- Load Cases
         local sqlDataRow_Cases = split(spc_case_load_project_id(self.ID),"\n")
         for i, Item in ipairs(sqlDataRow_Cases) do
            sqlData_Cases = split(Item,",")
            self.Cases[i] = Case:new{}
            load_data(self.Cases[i], sqlData_Cases)
         end

         -- Load Selected Elements

    else
        self.ID = project_new(self.Name)
    end

    
end

function Project:save()
    local saveString = join(self, ",")
    project_save(saveString)
end

