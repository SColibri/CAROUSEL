﻿local reallib
local fakelib
local machine
local extrasymbols = {}

local args = {...}
local errmsg
if args[1] then reallib = args[1] end
if args[2] then fakelib = args[2] end
if args[3] then machine = args[3] end
for i=4,#args do
	table.insert(extrasymbols, args[i])
end

if #args<3 then errmsg = "not enough parameters" end
if not ({X86=true})[machine] then errmsg = "'"..machine.."' is not a recognized machine architecture" end
if errmsg then
	if errmsg then io.stderr:write("Error: "..errmsg.."\n") end
	io.stderr:write[[
Usage: mkforwardlib-gcc <reallib> <fakelib> <machine> [<extrasymbols>...]

    reallib         The name of the existing dll that you want to use.
    fakelib         The name of the fake dll that you want to create and that
                    will call the reallib instead.
    machine         The hardware architecture of your windows version among:
                    X86
    extrasymbols    Additionnal symbols to export beyond standard Lua symbols
                    (if you have a very custom Lua dll).

Example: mkforwardlib lua52 lua5.2 X86
]]
	-- :TODO: add support for AMD64, IA64, AM33, ARM, CEE, EBC, M32R, MIPS, MIPS16, MIPSFPU, MIPSFPU16, MIPSR41XX, SH4, SH5, THUMB
	os.exit(0)
end

local symbols = {
        'luaL_addlstring',
        'luaL_addstring',
        'luaL_addvalue',
        'luaL_argerror',
        'luaL_buffinit',
        'luaL_buffinitsize',
        'luaL_callmeta',
        'luaL_checkany',
        'luaL_checkinteger',
        'luaL_checklstring',
        'luaL_checknumber',
        'luaL_checkoption',
        'luaL_checkstack',
        'luaL_checktype',
        'luaL_checkudata',
        'luaL_checkunsigned',
        'luaL_checkversion_',
        'luaL_error',
        'luaL_execresult',
        'luaL_fileresult',
        'luaL_getmetafield',
        'luaL_getsubtable',
        'luaL_gsub',
        'luaL_len',
        'luaL_loadbufferx',
        'luaL_loadfilex',
        'luaL_loadstring',
        'luaL_newmetatable',
        'luaL_newstate',
        'luaL_openlib',
        'luaL_openlibs',
        'luaL_optinteger',
        'luaL_optlstring',
        'luaL_optnumber',
        'luaL_optunsigned',
        'luaL_prepbuffsize',
        'luaL_pushmodule',
        'luaL_pushresult',
        'luaL_pushresultsize',
        'luaL_ref',
        'luaL_requiref',
        'luaL_setfuncs',
        'luaL_setmetatable',
        'luaL_testudata',
        'luaL_tolstring',
        'luaL_traceback',
        'luaL_unref',
        'luaL_where',
        'lua_absindex',
        'lua_arith',
        'lua_atpanic',
        'lua_callk',
        'lua_checkstack',
        'lua_close',
        'lua_compare',
        'lua_concat',
        'lua_copy',
        'lua_createtable',
        'lua_dump',
        'lua_error',
        'lua_gc',
        'lua_getallocf',
        'lua_getctx',
        'lua_getfield',
        'lua_getglobal',
        'lua_gethook',
        'lua_gethookcount',
        'lua_gethookmask',
        'lua_getinfo',
        'lua_getlocal',
        'lua_getmetatable',
        'lua_getstack',
        'lua_gettable',
        'lua_gettop',
        'lua_getupvalue',
        'lua_getuservalue',
        'lua_insert',
        'lua_iscfunction',
        'lua_isnumber',
        'lua_isstring',
        'lua_isuserdata',
        'lua_len',
        'lua_load',
        'lua_newstate',
        'lua_newthread',
        'lua_newuserdata',
        'lua_next',
        'lua_pcallk',
        'lua_pushboolean',
        'lua_pushcclosure',
        'lua_pushfstring',
        'lua_pushinteger',
        'lua_pushlightuserdata',
        'lua_pushlstring',
        'lua_pushnil',
        'lua_pushnumber',
        'lua_pushstring',
        'lua_pushthread',
        'lua_pushunsigned',
        'lua_pushvalue',
        'lua_pushvfstring',
        'lua_rawequal',
        'lua_rawget',
        'lua_rawgeti',
        'lua_rawgetp',
        'lua_rawlen',
        'lua_rawset',
        'lua_rawseti',
        'lua_rawsetp',
        'lua_remove',
        'lua_replace',
        'lua_resume',
        'lua_setallocf',
        'lua_setfield',
        'lua_setglobal',
        'lua_sethook',
        'lua_setlocal',
        'lua_setmetatable',
        'lua_settable',
        'lua_settop',
        'lua_setupvalue',
        'lua_setuservalue',
        'lua_status',
        'lua_toboolean',
        'lua_tocfunction',
        'lua_tointegerx',
        'lua_tolstring',
        'lua_tonumberx',
        'lua_topointer',
        'lua_tothread',
        'lua_tounsignedx',
        'lua_touserdata',
        'lua_type',
        'lua_typename',
        'lua_upvalueid',
        'lua_upvaluejoin',
        'lua_version',
        'lua_xmove',
        'lua_yieldk',
--      'luaopen_base',
--      'luaopen_bit32',
--      'luaopen_coroutine',
--      'luaopen_debug',
--      'luaopen_io',
--      'luaopen_math',
--      'luaopen_os',
--      'luaopen_package',
--      'luaopen_string',
--      'luaopen_table',
}

local def = io.open(fakelib..".def", "wb")
def:write("EXPORTS\n")
if reallib:find("%.") then
	reallib = '"'..reallib..'"'
end
for _,symbol in ipairs(symbols) do
--	def:write(""..reallib.."."..symbol.."="..reallib.."."..symbol.."\n")
	def:write(""..symbol.."="..reallib.."."..symbol.."\n")
end
for _,symbol in ipairs(extrasymbols) do
--	def:write(""..reallib.."."..symbol.."="..reallib.."."..symbol.."\n")
	def:write(""..symbol.."="..reallib.."."..symbol.."\n")
end
def:close()

os.execute('"'..table.concat({
	'gcc',
--	" -shared", -- this option seem to be incompatible with -nostartfiles
	" -o "..fakelib..".dll",
	" -nostartfiles",
	" "..fakelib..".def",
})..'"')

os.execute('"'..table.concat({
	'strip',
	" --strip-unneeded",
	" "..fakelib..".dll",
})..'"')
