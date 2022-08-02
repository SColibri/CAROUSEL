-- Item
ActivePhases = {ID = -1, IDProject = -1, IDPhase = -1} --@Description Active phases element

-- Constructor
function ActivePhases:new (o,ID,IDProject,IDPhase) --@Description Creates a new active phase object used in project
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDPhase = IDPhase or -1
   self.Columns = {"ID","IDProject","IDPhase"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ActivePhases:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(project_active_phases_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function ActivePhases:save()
    local saveString = join(self, ",")
    self.ID = tonumber(project_active_phases_save(saveString)) or -1
end

-- remove
function ActivePhases:remove()
    project_active_phases_delete(self.ID)
end
