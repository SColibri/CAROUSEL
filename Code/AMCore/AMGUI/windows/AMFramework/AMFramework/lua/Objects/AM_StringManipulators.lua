﻿
function split(str, delim, maxNb)
   -- Eliminate bad cases...
   if string.find(str, delim) == nil then
      return { str }
   end
   if maxNb == nil or maxNb < 1 then
      maxNb = 0    -- No limit
   end
   local result = {}
   local pat = "(.-)" .. delim .. "()"
   local nb = 0
   local lastPos
   for part, pos in string.gmatch(str, pat) do
      nb = nb + 1
      result[nb] = part
      lastPos = pos
      if nb == maxNb then
         break
      end
   end
   -- Handle the last field
   if nb ~= maxNb then
      result[nb + 1] = string.sub(str, lastPos)
   end
   return result
end
-- function from: http://lua-users.org/wiki/SplitJoin

function join(tableObject, delim)
    -- Get valid value objects from tableObject
    local resultArray = {}
    for i,Item in ipairs(tableObject.Columns) do
        resultArray[i] = tableObject[Item]
    end

    -- Concat string
    local result = table.concat(resultArray, delim)
    return result
end

function load_data(objectT, csvData)
    local index = 1
    for i , Item in ipairs(objectT.Columns) do
        if index > #csvData then break end
        if csvData[index] == nil then goto continue end

        if type(objectT[Item]) == "number"  then
            if tonumber(csvData[index]) ~= nil  then
                objectT[Item] = tonumber(csvData[index]);
            end
        else
            objectT[Item] = csvData[index]
        end

        ::continue::
        index = index + 1
    end
end