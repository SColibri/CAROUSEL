# AMFramework

## Matcalc API for AMFramework

This is an implementation of [IAM_API.h](../AMLib/interfaces/IAM_API.h) that uses Matcalc for all the CALPHAD calculations.

## Loading the dynamic library

Since the core implementation has LUA embedded you can request data or call functions using scripting commands, for this we first need to store a pointer to the implementation and also one to the function that handles the commands:

- Pointer to API object
 ```__declspec(dllexport) AM_API_Matcalc* get_API_Controll(AM_Config* configuration)```

- Pointer to function
  ``` __declspec(dllexport) char const* API_run_lua_command(AM_API_Matcalc* API_pointer, char* command, char* parameters) ```

  Loading these two library objects, you will be able to execute commands and retreive data from the database, for more information on lua functions please refer to the documentation. You can also find an updated list of functions in the build folder (after cmake).