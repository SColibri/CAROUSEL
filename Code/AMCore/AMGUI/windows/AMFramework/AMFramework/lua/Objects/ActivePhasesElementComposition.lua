-- Item
ActivePhasesElementComposition = {ID = -1, IDProject = -1, IDElement = -1, Value = 0} --@Description Active phases element

-- Constructor
function ActivePhasesElementComposition:new (o,ID,IDProject,IDElement,Value) --@Description Creates a new active phase object used in project
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDElement = IDElement or -1
   self.Value = Value or 0.0000
   
   self.Columns = {"ID","IDProject","IDElement","Value"}
   self.element = Element:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ActivePhasesElementComposition:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(project_active_phases_element_composition_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
   self.element = Element:new{self.IDElement}
end

-- save
function ActivePhasesElementComposition:save()
    local saveString = join(self, ",")
    self.ID = tonumber(project_active_phases_element_composition_save(saveString)) or -1
end

-- remove
function ActivePhasesElementComposition:remove()
    project_active_phases_element_composition_delete(self.ID)
end
