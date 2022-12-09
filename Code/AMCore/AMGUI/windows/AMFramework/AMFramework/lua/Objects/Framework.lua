-- Item
Framework = {configuration = {}} --@Description Element object. \n Element information, this should be loaded from a database

-- Constructor
function Framework:new (o,configuration) --@Description Creates a new Element,\n 
   o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.configuration = configuration or {} --@TYPE Configuration
   o:load()

   return o
end

-- load
function Framework:load()
    self.configuration = Configuration:new{}

end

-- save
function Framework:save()
    self.configuration:save()

end
