#pragma once

#include "../../../AMLib/interfaces/IAM_lua_functions.h"

class API_lua_functions: public IAM_lua_functions
{
public:

	API_lua_functions(lua_State* state);

private:
	virtual void add_functions_to_lua(lua_State* state) override;

#pragma region lua_functions
	/// <summary>
	/// This is just an example of how you should bind a function in lua
	/// so I'll call this baby lua.
	/// </summary>
	/// <param name="state"></param>
	/// <returns>number of parameters it outputs</returns>
	static int bind_hello_world(lua_State* state);

#pragma endregion

};
