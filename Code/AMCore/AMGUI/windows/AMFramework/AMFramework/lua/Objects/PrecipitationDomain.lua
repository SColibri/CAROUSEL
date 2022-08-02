-- Item
PrecipitationDomain = {ID = -1,Name = "",IDPhase=-1,InitialGrainDiameter=0.000005,EquilibriumDiDe=100000000000,VacancyEvolutionModel="",ConsiderExVa=0,ExcessVacancyEfficiency=0.0} --@Description Active phases element

-- Constructor
function PrecipitationDomain:new (o,ID,IDCase,Name,IDPhase,InitialGrainDiameter,EquilibriumDiDe,VacancyEvolutionModel,ConsiderExVa,ExcessVacancyEfficiency) --@Description Creates a new active phase object used in project
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.ID = ID or -1
   self.Name = Name or ""
   self.IDPhase = IDPhase or -1
   self.InitialGrainDiameter = InitialGrainDiameter or 0.000005
   self.EquilibriumDiDe = EquilibriumDiDe or 100000000000
   self.VacancyEvolutionModel = VacancyEvolutionModel or ""
   self.ConsiderExVa = ConsiderExVa or 0
   self.ExcessVacancyEfficiency = ExcessVacancyEfficiency or 0.0
   self.Columns = {"ID","Name","IDPhase","InitialGrainDiameter","EquilibriumDiDe","VacancyEvolutionModel","ConsiderExVa","ExcessVacancyEfficiency"}

   if o.ID > -1 then
    o:load()
   end

   return o
end

-- load
function PrecipitationDomain:load ()
   local sqlData = {}

   if self.ID > -1 then
    sqlData = split(spc_precipitation_domain_loadID(self.ID),",")
   end
   
   load_data(self, sqlData)
end

-- save
function PrecipitationDomain:save()
    local saveString = join(self, ",")
    self.ID = tonumber(spc_precipitation_domain_save(saveString)) or -1
end

-- remove
function PrecipitationDomain:remove()
    spc_precipitation_domain_delete(self.ID)
end
