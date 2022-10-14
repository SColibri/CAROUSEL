-- Item
ActivePhasesElementComposition = {ID = -1, IDProject = -1, IDElement = -1, Value = 0} --@Description Active phases element composition, this class is used for specifying all element compositions used for the simulation on determining the active pahases

-- Constructor
function ActivePhasesElementComposition:new (o,ID,IDProject,IDElement,Value) --@Description Creates a new active phase element composition object used in project
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDElement = IDElement or -1
   self.Value = Value or 0.0000
   
   self.Columns = {"ID","IDProject","IDElement","Value"}
   self.AMName = "ActivePhasesElementComposition"

   o.element = Element:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ActivePhasesElementComposition:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(project_active_phases_element_composition_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
   self.element = Element:new{self.IDElement}
end

-- save
function ActivePhasesElementComposition:save() --@Description Saves an active phase object into the database, if ID = -1 it creates a new entry.
    local saveString = join(self, ",")
    self.ID = tonumber(project_active_phases_element_composition_save(saveString)) or -1
end

-- remove
function ActivePhasesElementComposition:remove() --@Description Deletes the active phase entry
    project_active_phases_element_composition_delete(self.ID)
end
