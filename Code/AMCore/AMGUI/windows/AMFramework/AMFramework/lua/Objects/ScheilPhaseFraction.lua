-- Item
ScheilPhaseFraction = {ID = -1, IDCase = -1, IDPhase = -1, TypeComposition="weight", Temperature = 0, Value = 0} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function ScheilPhaseFraction:new (o,ID,IDCase,IDPhase,TypeComposition,Temperature,Value) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1
   self.IDPhase = IDPhase or 0
   self.TypeComposition = TypeComposition or "weight"
   self.Temperature = Temperature or 0
   self.Value = Value or 0
   self.Columns = {"ID","IDCase","IDPhase","TypeComposition","Temperature","Value"}
   o.phase = Phase:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function ScheilPhaseFraction:load ()
   local sqlData = split(spc_scheil_phasefraction_loadID(self.ID))
   load_data(self, sqlData)

   if self.ID > -1 then
    self.phase.ID = self.IDPhase
    self.phase.load()
   end
end

-- save
function ScheilPhaseFraction:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_scheil_phasefraction_save(saveString)) or -1
end

-- remove
function ScheilPhaseFraction:remove()
    spc_scheil_phasefraction_delete(self.ID)
end