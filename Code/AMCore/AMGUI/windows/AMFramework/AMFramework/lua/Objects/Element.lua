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

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function Element:load ()
   sqlData = split(element_loadID(self.ID))
   load_data(self, sqlData)
end

-- save
function Element:save()
    local saveString = join(self, ",")
    element_save(saveString)
end