-- Item
ElementComposition = {ID = -1,IDCase = -1, IDElement=-1, TypeComposition="weight", value=0, element={} } --@Description Element object. \n Element information, this should be loaded from a database

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

   -- initialize element item on object
   if o.IDElement > -1 then
    o.element = Element:new{ID = o.IDElement}
   end

   return o
end

function ElementComposition:copy (obj) --@Description deep copy of object on to current object, it does not copy the ID\n 
   error(1 == 2, "Deprecated function, this deep copies are done using table.deepcopy implementation")
   assert(obj ~= nil, "ElementComposition:copy  Error; nil object as parameter")

   self.ID = -1
   self.IDCase = obj.IDCase
   self.IDElement = obj.IDElement
   self.TypeComposition = obj.TypeComposition
   self.value = obj.value or 0
   self.Columns = {"ID","IDCase","IDElement","TypeComposition","value"}
   self.element = Element:new{}
end

-- load
function ElementComposition:load ()
   local sqlData = split(spc_elementcomposition_load_id(self.ID))
   load_data(self, sqlData)

   self.element = Element:new{ID = self.IDElement}
end

-- save
function ElementComposition:save()
    assert(self.IDElement ~= -1, "Wrong id element for: "..self.element.Name.." and value: "..self.value)
    assert(type(self.value) ~= "table", "ElementComposition:save; invalid table value, composition ranges cannot be saved directly")
    if self.IDElement == -1 then error("wrong id element") end

    local saveString = join(self, ",")    
    self.ID = tonumber(spc_elementcomposition_save(saveString)) or -1

    if self.ID == -1 then error("Element composition was not saved!") end
end

-- remove
function ElementComposition:remove()
    spc_elementcomposition_delete(self.ID)
end