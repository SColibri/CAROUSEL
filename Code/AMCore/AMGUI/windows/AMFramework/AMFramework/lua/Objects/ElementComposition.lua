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
   self.TypeComposition = TypeComposition or "weight"
   self.value = value or 0
   self.Columns = {"ID","IDCase","IDElement","TypeComposition","value"}
   self.element = Element:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ElementComposition:load ()
   local sqlData = split(spc_elementcomposition_load_id(self.ID))
   load_data(self, sqlData)

   self.element = Element:new{self.IDElement}
end

-- save
function ElementComposition:save()
    if self.IDElement == -1 then error("wrong id element") end

    local saveString = join(self, ",")    
    self.ID = tonumber(spc_elementcomposition_save(saveString)) or -1

    if self.ID == -1 then error("Element composition was not saved!") end
end

-- remove
function ElementComposition:remove()
    spc_elementcomposition_delete(self.ID)
end