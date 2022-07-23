-- Item
Phase = {ID = -1, Name = ""} --@Description pahse object. \n phase information, this should be loaded from a database

-- Constructor
function Phase:new (o,ID,Name) --@Description Creates a new case,\n create new object by calling newVar = Case:new{ID = -1}, use ID = -1 when you want to create a new case item
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or ""
   self.Columns = {"ID","Name"}

   if o.ID > -1 or Name ~= "" then
    o:load()
   end

   return o
end

-- load
function Phase:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(phase_loadID(self.ID),",")
   else
    sqlData = split(phase_load_ByName(self.Name),",")
   end
   
   load_data(self, sqlData)
end

-- save
function Phase:save()
    local saveString = join(self, ",")
    phase_save(saveString)
end
