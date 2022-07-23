-- Item
ElementComposition = {ID = -1,IDCase = -1, IDElement=-1, TypeComposition="weight", value=0 } --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function ElementComposition:new (o,ID,IDCase,IDElement,TypeComposition,value) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.IDElement = IDElement or -1
   self.TypeComposition = TypeComposition
   self.value = value or 0
   self.Columns{"ID","IDCase","IDElement","TypeComposition","value"}
   self.element = Element:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ElementComposition:load ()
   sqlData = split(spc_elementcomposition_load_id(self.ID))
   load_data(self, sqlData)

   self.element = Element:new{self.IDElement}
end

-- save
function ElementComposition:save()
    local saveString = join(self, ",")
    spc_elementcomposition_save(saveString)
end

-- remove
function ElementComposition:remove()
    spc_elementcomposition_Delete(self.ID)
end