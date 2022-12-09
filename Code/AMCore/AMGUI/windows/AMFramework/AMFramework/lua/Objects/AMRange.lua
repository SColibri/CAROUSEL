-- AMRange
AMRange = {Items = {}} --@Description AMRange object. \n This object interprets text as range or array values

-- Constructor
function AMRange:new (o,Items) --@Description Creates a new AMRange,\n 
   local o = o or {}

   setmetatable(o, self)
   self.__index = self
   self.AMName = "AMRange"
   o.Items = Items or {}
   
   return o
end

function AMRange:add_range (rangeObject) --@Description Adds a range to current list, multiple options {Numeric List/Array, Number, String (Start-End:Step Size) Eg:(1-2:1)}

	-- Add values to table
	if type(rangeObject) == "table" then
		for index,item in ipairs(rangeObject) do
			assert(tonumber(item) ~= nil, "AMRange:add_range Error || value in table is not numeric; "..item)
			table.insert(self.Items, tonumber(item))
		end
	elseif type(rangeObject) == "number" then
		table.insert(self.Items, rangeObject)
	elseif type(rangeObject) == "string" then
		if string.find(rangeObject,'-') then
			
			local fValue = split(rangeObject, '-')
			if #fValue ~= 2 then error("AMRange:add_range Error Missing parameter in string range. Error in: "..rangeObject) end

			local sValue = split(fValue[2], ':')
			if #sValue ~= 2 then error("AMRange:add_range Error Missing stepsize in string range. Error in: "..rangeObject) end

			local startValue = tonumber(fValue[1])
			local endValue = tonumber(sValue[1])
			local stepSize = tonumber(sValue[2])

			if startValue ~= nil and endValue ~= nil and stepSize ~=nil then
				for n1 = startValue, endValue, stepSize do
					table.insert(self.Items, n1)
				end
			else
				error("AMRange:add_range Error invalid non-numeric parameter in: "..rangeObject)
			end

		else
			error("AMRange:add_range Error invalid string format, string has to specified as eg. start-end:stepsize for example 1-2:1, Error in: "..rangeObject)
		end
	else
		error("AMRange:add_range Error invalid input argument, this method only allows the following types; Table, number and string. Error in: "..rangeObject)
	end

	
end


