-- Item
Element = {ID = -1, Name = ""} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function Element:new (o,ID,Name) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or ""
   self.Columns = {"ID","Name"}

   if o.ID > -1 or o.Name ~= "" then
    o:load()
   end

   return o
end

-- load
function Element:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(element_loadID(self.ID),",")
   else
    sqlData = split(element_load_ByName(string.upper(self.Name)),",")
   end
   
   load_data(self, sqlData)
end

-- save
function Element:save()
    self.Name = string.upper(self.Name)
    local saveString = join(self, ",")
    self.ID = tonumber(element_save(saveString)) or -1
end

-- remove
function Element:remove()
    element_delete(self.ID)
end