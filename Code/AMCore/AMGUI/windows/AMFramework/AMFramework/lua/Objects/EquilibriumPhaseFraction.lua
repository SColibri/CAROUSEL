-- Item
EquilibriumPhaseFraction = {ID = -1, IDCase = -1, IDPhase = -1, Temperature = 0, Value = 0} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function EquilibriumPhaseFraction:new (o,ID,IDCase,IDPhase,Temperature,Value) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase
   self.IDPhase = IDPhase or 0
   self.Temperature = Temperature or 0
   self.Value = Value or 0
   self.Columns = {"ID","IDCase","IDPhase","Temperature","Value"}
   o.phase = Phase:new{}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function EquilibriumPhaseFraction:load ()
   local sqlData = split(spc_equilibrium_phasefraction_loadID(self.ID))
   load_data(self, sqlData)

   if self.ID > -1 then
    self.phase.ID = self.IDPhase
    self.phase.load()
   end
end

-- save
function EquilibriumPhaseFraction:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_equilibrium_phasefraction_save(saveString)) or -1
end

-- remove
function EquilibriumPhaseFraction:remove()
    spc_equilibrium_phasefraction_delete(self.ID)
end