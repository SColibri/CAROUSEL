-- Item
CALPHADDatabase = {ID = -1, IDProject = -1, ThermodynamicDatabase = "", MobilityDatabase = "", PhysicalDatabase = ""} --@Description CALPHAD database ojbject. Stores all paths for used databases

-- Constructor
function CALPHADDatabase:new (o,ID,IDProject,ThermodynamicDatabase,MobilityDatabase,PhysicalDatabase) --@Description Creates a CALPHADDatabase object,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDProject = IDProject or -1
   self.ThermodynamicDatabase = ThermodynamicDatabase or ""
   self.MobilityDatabase = MobilityDatabase or ""
   self.PhysicalDatabase = PhysicalDatabase or ""
   self.Columns = {"ID","IDProject","ThermodynamicDatabase","MobilityDatabase","PhysicalDatabase"}
   self.AMName = "Element"

   if o.ID > -1 or o.IDProject > -1 then
    o:load()
   end

   return o
end

-- load
function CALPHADDatabase:load () --@Description Loads data based on the ID or projectID, if the ID is -1 it will return an empty object
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(calphad_database_loadID(self.ID),",")
   elseif self.IDProject > -1 then
    sqlData = split(calphad_database_load_IDProject(self.IDProject),",")
   end
   
   load_data(self, sqlData)
end

-- save
function CALPHADDatabase:save() --@Description Saves an object into the database, if ID = -1 it creates a new entry. Returns -1 if failed to save
    self.Name = string.upper(self.Name)
    local saveString = join(self, ",")
    self.ID = tonumber(calphad_database_save(saveString)) or -1
end

-- remove
function CALPHADDatabase:remove() --@Description Deletes the object entry
    calphad_database_delete(self.ID)
end
