-- Item
PrecipitationPhase = {ID = -1, IDCase = -1, IDPhase = -1, NumberSizeClasses = -1, Name = "", NucleationSites = "none", IDPrecipitationDomain = -1, CalcType = "normal", MinRadius = 0.000001, MeanRadius = 0.000002, MaxRadius = 0.000003, StdDev = 0.05, PrecipitateDistribution = "", PhaseName = ""  } --@Description Active phases element

-- Constructor
function PrecipitationPhase:new (o,ID,IDCase,IDPhase,NumberSizeClasses,Name,NucleationSites,IDPrecipitationDomain,CalcType,MinRadius,MeanRadius,MaxRadius,StdDev,PrecipitateDistribution) --@Description Creates a new precipitation phase
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.IDCase = IDCase or -1 
   self.IDPhase = IDPhase or -1
   self.NumberSizeClasses = NumberSizeClasses or -1 
   self.Name = Name or "" 
   self.NucleationSites = NucleationSites or "none" 
   self.IDPrecipitationDomain = IDPrecipitationDomain or -1 
   self.CalcType = CalcType or "normal" 
   self.MinRadius = MinRadius or 0.000001 
   self.MeanRadius = MeanRadius or 0.000002 
   self.MaxRadius = MaxRadius or 0.000003 
   self.StdDev = StdDev or 0.05 
   self.PrecipitateDistribution = PrecipitateDistribution or "_" 
   self.Columns = {"ID","IDCase","IDPhase","NumberSizeClasses","Name","NucleationSites","IDPrecipitationDomain","CalcType","MinRadius","MeanRadius","MaxRadius","StdDev","PrecipitateDistribution"}

   self.PhaseName = ""
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
   self:load_PhaseName()
end

function PrecipitationPhase:load_PhaseName()
    if self.IDPhase > -1 then
        local phasey = Phase:new{ID = self.IDPhase}
        self.IDPhase = phasey.ID
    end
end

-- save
function PrecipitationPhase:save()
    assert(self.IDCase > -1, "PrecipitationPhase:save; IDCase was not specified on for this object")
    assert(self.IDPhase > -1, "PrecipitationPhase:save; IDPhase was not specified on for this object")

    local saveString = join(self, ",")
    self.ID = tonumber(spc_precipitation_phase_save(saveString)) or -1
end

-- remove
function PrecipitationPhase:remove()
    spc_precipitation_phase_delete(self.ID)
end

-- Methods
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
-- |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

-- .......................................................................................
--                                            Phase
-- .......................................................................................

-- Set phase
function PrecipitationPhase:set_phase(phaseItem)
    assert(type(phaseItem) == "table", "PrecipitationPhase:set_phase; parameter is not a phase object")
    assert(phaseItem.ID ~= nil, "PrecipitationPhase:set_phase; parameter is not a phase object")
    assert(phaseItem.ID > -1, "PrecipitationPhase:set_phase; Current object has an ID = -1, this means that it is not stored in the database, please reload the CALPHAD dataabase")
    
    self.IDPhase = phaseItem.ID
end

-- Calculate precipitation distribution using scheil method
function PrecipitationPhase:calculate_precipitation_distribution_scheil()
    assert(self.ID > -1, "PrecipitationPhase:calculate_precipitation_distribution_scheil; Precipitation phase has to be saved before executing")
    assert(self.IDCase > -1, "PrecipitationPhase:calculate_precipitation_distribution_scheil; this object is not related to any case object")

    local casey = Case:new{ID = self.IDCase}
    assert(casey.ID > -1, "PrecipitationPhase:calculate_precipitation_distribution_scheil; case ID not found!")

    pixelcase_calculate_precipitate_distribution(casey.IDProject, casey.ID.."-"..casey.ID)
end
