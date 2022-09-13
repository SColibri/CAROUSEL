-- Item
Precipitation_Calculations = {project = {}, dependentPhase = ""} --@Description

-- Constructor
function Precipitation_Calculations:setup (o,project, selectedPhases, dependentPhase, startComposition, endComposition, intervals, 
                                             scheilConfiguration, precipitationPhases, precipitateDomain, temperatureSegments, autoStart) --@Description
   o = o or {}

   assert(type(scheilConfiguration) == "table", "scheilConfiguration has to be a ScheilConfig object")
   assert(type(project) == "table", "project has to be a ScheilConfig object")

   setmetatable(o, self)
   self.__index = self
   self.project = project or error("project was not specified") --@TYPE Project
   self.selectedPhases = selectedPhases or error("please select phases to analyze")
   self.dependentPhase = dependentPhase or error("Please specify the dependent phase for the scheil calculations")
   self.startComposition = startComposition or error("Please specify the starting composition")
   self.endComposition = endComposition or startComposition
   self.intervals = intervals or 1
   self.scheilConfiguration = scheilConfiguration or error("Please specify the scheil configuration")
   self.precipitationPhases = precipitationPhases or error("Please specify a list of phases to precipitate")
   self.precipitateDomain = precipitateDomain or error("Please specify the precipitation domain to be used")
   self.temperatureSegments = temperatureSegments or error("Please define the temperature segments for the heat treatment")
   self.autoStart = autoStart or true

   assert(#startComposition == #endComposition, "Start and end compositions are not of the same array size, please check")

   if autoStart then
    self:create_objects()
   end

   return o
end


function Precipitation_Calculations:create_objects()
    
    local element = self.project:get_reference_element()
    for i = 1,intervals do
        
        local composition = {}
        local case = Case:new{Name = element.Name..}
    end

end

function Precipitation_Calculations:get_varying_element_number()
    
    local noElements = 0

    for i,Item in pairs(self.startComposition) do
        assert(self.endComposition[i] ~= nil, "Precipitation_Calculations:get_varying_element_number; end composition does not have the same elements, please check")
        if self.startComposition[i] ~= self.endComposition[i] then
            noElements = noElements + 1
        end
    end

    return noElements
end