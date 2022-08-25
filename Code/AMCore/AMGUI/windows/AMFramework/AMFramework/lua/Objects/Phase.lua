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
    self.ID = tonumber(phase_save(saveString)) or -1
end

-- Methods
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                                            Phase
-- .......................................................................................

function Phase:get_name_fromID(IDPhase)
    assert(type(IDPhase) == "number", "Phase:get_name_fromID; IDPhase has to be an integer")
    
    local phasey = Phase:new{ID = IDPhase}
    assert(phasey.ID == IDPhase, "Phase:get_name_fromID; Looks like this phase does not exist in the database")

    return phasey.Name
end

function Phase:get_name_fromName(PhaseName)
    assert(type(PhaseName) == "string", "Phase:get_name_fromName; Name has to be string")
    
    local phasey = Phase:new{Name = PhaseName}
    assert(phasey.ID == -1, "Phase:get_name_fromName; Looks like this phase does not exist in the database")

    return phasey.Name
end
