
function split(str, delim, maxNb)
    if str == nil or delim == nil then
        return {}
    end

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

function trim(s)
   return (s:gsub("^%s*(.-)%s*$", "%1"))
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

function load_table_data(TableStore, ObjectType, csvTableData) --@Description Load table in csv format ( columns: ',' , Rows: '\n ') This function parses through the data and fills the objects. \n TableStore: Reference to table to store the data \n ObjectType: class type from which to call the new() function \n csvTableData: raw data in csv format  
    for i, Item in ipairs(csvTableData) do
        if string.len(Item) <= 1 then goto continue end
        local csvData = split(Item,",")
        TableStore[i] = ObjectType:new{}
        load_data(TableStore[i], csvData)
                
        ::continue::
    end
end

-- code snippet for deep copy from: http://lua-users.org/wiki/CopyTable, Author: not available, lua?
function table.deepcopy(orig, copies)
    copies = copies or {}
    local orig_type = type(orig)
    local copy
    if orig_type == 'table' then
        if copies[orig] then
            copy = copies[orig]
        else
            copy = {}
            copies[orig] = copy
            for orig_key, orig_value in next, orig, nil do
                copy[table.deepcopy(orig_key, copies)] = table.deepcopy(orig_value, copies)
            end
            setmetatable(copy, table.deepcopy(getmetatable(orig), copies))
        end
    else -- number, string, boolean, etc
        copy = orig
    end
    return copy
end

function write_to_file(filename, Content, writeMode)
  -- common modes:
  -- w: write and overwrite
  -- a: append

  assert(filename ~= nil, "AM_StringManipulators:write_to_file; filename was not given!")

  local Text = Content or "AM_StringManipulators:write_to_file;  Empty content"
  local wMode = writeMode or "w"
 
  file = assert(io.open(filename, wMode))
  file:write(Text)
  file:close()

  ::ENDFUNCTION::
end