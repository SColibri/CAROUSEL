-- Item
PrecipitationPhase = {ID = -1, IDCase = -1, NumberSizeClasses = -1, Name = "", NucleationSites = "none", IDPrecipitationDomain = -1, CalcType = "normal", MinRadius = 0.000001, MeanRadius = 0.000002, MaxRadius = 0.000003, StdDev = 0.05, PrecipitateDistribution = ""  } --@Description Active phases element

-- Constructor
function PrecipitationPhase:new (o,ID,IDCase,NumberSizeClasses,Name,NucleationSites,IDPrecipitationDomain,CalcType,MinRadius,MeanRadius,MaxRadius,StdDev,PrecipitateDistribution) --@Description Creates a new precipitation phase
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1 
   self.NumberSizeClasses = NumberSizeClasses or -1 
   self.Name = Name or "" 
   self.NucleationSites = NucleationSites or "none" 
   self.IDPrecipitationDomain = IDPrecipitationDomain or -1 
   self.CalcType = CalcType or "normal" 
   self.MinRadius = MinRadius or 0.000001 
   self.MeanRadius = MeanRadius or 0.000002 
   self.MaxRadius = MaxRadius or 0.000003 
   self.StdDev = StdDev or 0.05 
   self.PrecipitateDistribution = PrecipitateDistribution or "" 
   self.Columns = {"ID","IDCase","NumberSizeClasses","Name","NucleationSites","IDPrecipitationDomain","CalcType","MinRadius","MeanRadius","MaxRadius","StdDev","PrecipitateDistribution"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function PrecipitationPhase:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_precipitation_phase_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function PrecipitationPhase:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_precipitation_phase_save(saveString)) or -1
end

-- remove
function PrecipitationPhase:remove()
    spc_precipitation_phase_delete(self.ID)
end