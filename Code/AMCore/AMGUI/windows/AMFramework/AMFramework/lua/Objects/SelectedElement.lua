-- Item
SelectedElement = {ID = -1, IDProject = -1, IDElement = -1, IsReferenceElement = 0} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function SelectedElement:new (o,ID,IDProject,IDElement,IsReferenceElement) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.IDElement = IDElement or -1
   self.IsReferenceElement = IsReferenceElement or 0
   self.Columns = {"ID","IDProject","IDElement","IsReferenceElement"}
   self.Name = ""

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function SelectedElement:load ()
   local sqlData = split(spc_selectedelement_load_id(self.ID))
   load_data(self, sqlData)

   local tempRef = Element:new{ID = self.IDElement}
   self.Name = tempRef.Name
end

-- save
function SelectedElement:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_selectedelement_save(saveString)) or -1
end

-- remove
function SelectedElement:remove()
    spc_selectedelement_delete(self.ID)
end
