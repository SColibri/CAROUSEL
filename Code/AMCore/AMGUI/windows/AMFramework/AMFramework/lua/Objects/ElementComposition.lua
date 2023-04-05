-- Item
ElementComposition = {ID = -1,IDCase = -1, IDElement=-1, TypeComposition="weight", value=0, element={} } --@Description ElementComposition object. \n Item that holds the value in weight percentage of an element

-- Constructor
function ElementComposition:new (o,ID,IDCase,IDElement,TypeComposition,value) --@Description Creates a new ElementComposition,\n 
   local o = o or {}
   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.IDElement = IDElement or -1
   self.TypeComposition = TypeComposition or "weight"
   self.value = value or 0
   self.Columns = {"ID","IDCase","IDElement","TypeComposition","value"}
   o.element = Element:new{}

   if o.ID > -1 then
    o:load()
   end

   -- initialize element item on object
   if o.IDElement > -1 then
    o.element = Element:new{ID = o.IDElement}
   end

   return o
end

-- load
function ElementComposition:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = split(spc_elementcomposition_load_id(self.ID))
   load_data(self, sqlData)

   self.element = Element:new{ID = self.IDElement}
end

-- save
function ElementComposition:save() --@Description Saves an object into the database, if ID = -1 it creates a new entry.
    assert(self.IDElement ~= -1, "Wrong id element for: "..self.element.Name.." and value: "..self.value)
    assert(type(self.value) ~= "table", "ElementComposition:save; invalid table value, composition ranges cannot be saved directly")
    if self.IDElement == -1 then error("wrong id element") end

    local saveString = join(self, ",")    
    self.ID = tonumber(spc_elementcomposition_save(saveString)) or -1

    if self.ID == -1 then error("Element composition was not saved!") end
end

-- remove
function ElementComposition:remove() --@Description Deletes the object entry
    spc_elementcomposition_delete(self.ID)
end