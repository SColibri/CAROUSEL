-- Item
SelectedPhase = {ID = -1, IDCase = -1, IDPhase = -1, phase = {}} --@Description

-- Constructor
function SelectedPhase:new (o,ID,IDCase,IDPhase,phase) --@Description Creates a new Selected Phase,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.IDPhase = IDPhase or -1
   self.Columns = {"ID","IDCase","IDPhase"}
   self.phase = phase or Phase:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function SelectedPhase:load ()
   sqlData = split(spc_selectedphase_load_id(self.IDCase))
   load_data(self, sqlData)

   if self.ID > -1 then
    self.phase.ID = self.IDPhase
    self.phase:load()
   end
end

-- save
function SelectedPhase:save()
    local saveString = join(self, ",")
    spc_selectedphase_save(saveString)
end

-- remove
function SelectedPhase:remove()
    spc_selectedphase_delete(self.ID)
end