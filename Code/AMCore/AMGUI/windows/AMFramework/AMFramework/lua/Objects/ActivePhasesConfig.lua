-- Item
ActivePhasesConfig = {ID = -1, IDProject = -1, StartTemp = 700, EndTemp = 25, StepSize = 25} --@Description Active phase configuration, config. used for solidification simulation using scheil or equilibrium

-- Constructor
function ActivePhasesConfig:new (o,ID,IDProject,StartTemp,EndTemp,StepSize) --@Description Creates a new active phase configuration object
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.StartTemp = StartTemp or 700
   self.EndTemp = EndTemp or 25
   self.StepSize = StepSize or 25
   self.Columns = {"ID","IDProject","StartTemp","EndTemp","StepSize"}
   self.AMName = "ActivePhasesConfig"

   if o.ID > -1 or o.IDProject > -1 then
    o:load()
   end

   return o
end

-- load
function ActivePhasesConfig:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(project_active_phases_configuration_loadID(self.ID),",")
   else
    sqlData = split(project_active_phases_configuration_load_IDProject(string.upper(self.IDProject)),",")
   end
   
   load_data(self, sqlData)
end

-- save
function ActivePhasesConfig:save() --@Description Saves an active phase object into the database, if ID = -1 it creates a new entry.
    local saveString = join(self, ",")
    self.ID = tonumber(project_active_phases_configuration_save(saveString)) or -1
end

-- remove
function ActivePhasesConfig:remove() --@Description Deletes the active phase entry
    project_active_phases_configuration_delete(self.ID)
end


-- Methods
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                               Determine Active pahses
-- .......................................................................................
function ActivePhasesConfig:run() --@Description Gets active phases and creates as many ActivePhases objects. This method uses the scheil method
    if self.IDProject == -1 then error("Active phases calculation is not possible because you have not selected a valiid project id") end
    get_active_phases(self.IDProject)
end
