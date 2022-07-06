using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Dynamic;
using System.Runtime.InteropServices;


namespace AMFramework.AMsystem
{
    /// <summary>
    /// Link to the library instead of using IPC. This can make it faster for data access.
    /// </summary>
    public class AMFramework_library_APIHandle
    {
        #region kernel32
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDLLDirectory(string libDirectoryPath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr LoadLibrary(string pathToDLL);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        #endregion

        #region AMFramework_library
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr get_API_controll_default();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate IntPtr API_run_lua_command(IntPtr API_pointer, StringBuilder command, StringBuilder parameters);
        #endregion

        API_run_lua_command run_lua;

        private IntPtr _library;
        private IntPtr _api;
        public AMFramework_library_APIHandle(string pathToLibrary) 
        {
            try
            {
                _library = LoadLibrary(pathToLibrary);
                int errorThingy_before = Marshal.GetLastWin32Error();
                IntPtr AddressPointer_api_object = GetProcAddress(_library, "get_API_controll_default");
                int errorThingy = Marshal.GetLastWin32Error();
                get_API_controll_default apiObject = (get_API_controll_default)Marshal.GetDelegateForFunctionPointer(AddressPointer_api_object, typeof(get_API_controll_default));
                _api = apiObject();
            }
            catch (Exception e)
            {
                global::System.Windows.Forms.MessageBox.Show("An error occured when loading the API: " + e.Message);
                throw;
            }
            
        }

        public string run_lua_command(string command, string parameters) 
        {
            IntPtr AddressPointer_run_lua = GetProcAddress(_library, "API_run_lua_command");
            API_run_lua_command apiObject = (API_run_lua_command)Marshal.GetDelegateForFunctionPointer(AddressPointer_run_lua, typeof(API_run_lua_command));
            
            string outCommand;

            try
            {
                outCommand = Marshal.PtrToStringAnsi(apiObject(_api, new StringBuilder(command), new StringBuilder(parameters)));
            }
            catch (Exception e)
            {
                outCommand = "Error: " + e.Message;
            }

            return outCommand.ToString();
        }
    }
}
