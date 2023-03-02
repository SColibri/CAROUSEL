using AMFramework_Lib.Libs;
using System.Runtime.InteropServices;
using System.Text;

namespace AMFramework_Lib.Core
{
    /// <summary>
    /// Link to the AmFramework library instead of using IPC.
    /// </summary>
    public class AMCore_libHandle : IAMCore_Comm
    {
        #region Fields
        /// <summary>
        /// Pointer to library - AMFramework
        /// </summary>
        private IntPtr _library;

        /// <summary>
        /// Pointer to API object, used in combination 
        /// </summary>
        private IntPtr _api;

        /// <summary>
        /// 
        /// </summary>
        private AMFrameworkLib.API_run_lua_command _runLua;

        /// <summary>
        /// Returns true if API oject was loaded
        /// </summary>
        private bool _apiAvailable = false;

        #endregion


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathToLibrary"></param>
        public AMCore_libHandle(string pathToLibrary)
        {
            Link_to_library(pathToLibrary);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~AMCore_libHandle()
        {
            // free the library
            Free_library();
        }

        #endregion

        #region Load
        /// <summary>
        /// Load AMFramwork lubrary
        /// </summary>
        /// <param name="pathToLibrary"></param>
        private void Load_library(string pathToLibrary)
        {
            try
            {
                _library = Kernel32Lib.LoadLibrary(pathToLibrary);
            }
            catch (Exception)
            {
                _apiAvailable = false;
            }
        }

        /// <summary>
        /// Frees the library
        /// </summary>
        private void Free_library()
        {
            if (_library != IntPtr.Zero)
            {
                Kernel32Lib.FreeLibrary(_library);
            }
            _apiAvailable = false;
        }

        /// <summary>
        /// Get Pointer to API object
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void Load_api_controll()
        {
            try
            {
                IntPtr AddressPointer_api_object = Kernel32Lib.GetProcAddress(_library, "get_API_controll_default");

                AMFrameworkLib.get_API_controll_default apiObject = (AMFrameworkLib.get_API_controll_default)Marshal.GetDelegateForFunctionPointer(AddressPointer_api_object, typeof(AMFrameworkLib.get_API_controll_default));
                _api = apiObject();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get pointer to lua commands, this allows sending commands using lua syntax
        /// </summary>
        private void Load_run_lua_command_address_space()
        {
            IntPtr AddressPointer_run_lua = Kernel32Lib.GetProcAddress(_library, "API_run_lua_command");
            _runLua = (AMFrameworkLib.API_run_lua_command)Marshal.GetDelegateForFunctionPointer(AddressPointer_run_lua, typeof(AMFrameworkLib.API_run_lua_command));
        }

        #endregion

        /// <summary>
        /// Run lua command, base commands come from the library implementation and also form lua scripts that
        /// can be loaded using the run_script command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string run_lua_command(string command, string parameters)
        {
            // Check if API is availabe
            if (!_apiAvailable) return "Error: api not available!";

            // execute command
            string outCommand;
            try
            {
                outCommand = Marshal.PtrToStringAnsi(_runLua(_api, new StringBuilder(command), new StringBuilder(parameters))) ?? "";
            }
            catch (Exception e)
            {
                outCommand = $"Error: {e.Message}";
            }

            return outCommand;
        }

        /// <summary>
        /// Load new library using path
        /// </summary>
        /// <param name="apiPath"></param>
        public void update_path(string apiPath)
        {
            Link_to_library(apiPath);
        }

        /// <summary>
        /// Link to library 
        /// </summary>
        /// <param name="pathToLibrary"></param>
        /// <exception cref="Exception">Load library</exception>
        /// <exception cref="FileNotFoundException">pathToFile has to be valid</exception>
        private void Link_to_library(string pathToLibrary)
        {
            // Free library if already loaded
            Free_library();

            // Check for path type
            string fullPath = pathToLibrary;
            if (!System.IO.Path.IsPathRooted(pathToLibrary))
                fullPath = AppDomain.CurrentDomain.BaseDirectory + pathToLibrary;

            // Check if file exists
            if (!System.IO.File.Exists(fullPath))
            {
                throw new FileNotFoundException($"path to library was not found: {pathToLibrary}");
            }

            try
            {
                // Load library
                Load_library(fullPath);

                // Create new API control
                Load_api_controll();

                // Get command address space
                Load_run_lua_command_address_space();

                // Set callbacks
                CallbackManager.RegisterCallbacks(_library);

                // set to api available
                _apiAvailable = true;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured when loading the API: " + e.Message + " Path: " + fullPath);
            }
        }

    }
}
