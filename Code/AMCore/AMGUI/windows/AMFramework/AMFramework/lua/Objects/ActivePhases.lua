-- Item
ActivePhases = {ID = -1, IDProject = -1, IDPhase = -1} --@Description Active phases: Phases that are formed based on the active phases configuration (e.i. scheil or equilibrium)

-- Constructor
function ActivePhases:new (o,ID,IDProject,IDPhase) --@Description Creates a new active phase object, this object belongs to the project level
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDPhase = IDPhase or -1
   self.Columns = {"ID","IDProject","IDPhase"}
   self.AMName = "ActivePhases"

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ActivePhases:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(project_active_phases_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function ActivePhases:save() --@Description Saves an active phase object into the database, if ID = -1 it creates a new entry.
    local saveString = join(self, ",")
    self.ID = tonumber(project_active_phases_save(saveString)) or -1
end

-- remove
function ActivePhases:remove() --@Description Deletes the active phase entry
    project_active_phases_delete(self.ID)
end
