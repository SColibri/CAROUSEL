-- Item
Element = {ID = -1, Name = ""} --@Description Element object. \n Element name and ID, do not create elements yourself, let them be extracted from the database

-- Constructor
function Element:new (o,ID,Name) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or ""
   self.Columns = {"ID","Name"}
   self.AMName = "Element"

   if o.ID > -1 or o.Name ~= "" then
    o:load()
   end

   return o
end

-- load
function Element:load () --@Description Loads data based on the ID, if the ID is -1 it will return an empty object
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(element_loadID(self.ID),",")
   else
    sqlData = split(element_load_ByName(string.upper(self.Name)),",")
   end
   
   load_data(self, sqlData)
end

-- save
function Element:save() --@Description Saves an object into the database, if ID = -1 it creates a new entry.
    self.Name = string.upper(self.Name)
    local saveString = join(self, ",")
    self.ID = tonumber(element_save(saveString)) or -1
end

-- remove
function Element:remove() --@Description Deletes the object entry
    element_delete(self.ID)
end