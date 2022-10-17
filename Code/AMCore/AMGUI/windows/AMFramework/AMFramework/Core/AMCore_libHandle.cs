using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows;

namespace AMFramework.Core
{
    /// <summary>
    /// Link to the AmFramework library instead of using IPC.
    /// </summary>
    internal class AMCore_libHandle: IAMCore_Comm
    {

        private IntPtr _library; // AMFramework
        private IntPtr _api; // IAM_API pointer - implementation
        private API_run_lua_command run_lua; // run lua command address space

        #region Constructor
        public AMCore_libHandle(string pathToLibrary)
        {
            Link_to_library(pathToLibrary);
        }

        ~AMCore_libHandle() 
        {
            Free_library();
        }

        private void Free_library() 
        {
            if (_library != IntPtr.Zero)
            {
                FreeLibrary(_library);
            }
            _api_available = false;
        }

        #region Load
        private void Load_library(string pathToLibrary) 
        {
            _library = LoadLibrary(pathToLibrary);

            // Uff user32 on windows 7 causes module load error 126 win32
            //if (Marshal.GetLastWin32Error() != 0) 
            //{
                
            //    Win32Exception ex = new Win32Exception();
            //    string report = "Linking to the library was not possible: \n" + 
            //                    "Module Name: " + ex.TargetSite?.Module.Name + "\n" +
            //                    "Target site: " + ex.TargetSite?.Name + "\n" +
            //                    "Internal error: " + ex.InnerException?.Message + "\n" +
            //                    "Error Message: " + ex.Message;
            //    System.Windows.Forms.MessageBox.Show(report);
            //}
        }

        private void Load_api_controll() 
        {
            try
            {
                IntPtr AddressPointer_api_object = GetProcAddress(_library, "get_API_controll_default");

                get_API_controll_default apiObject = (get_API_controll_default)Marshal.GetDelegateForFunctionPointer(AddressPointer_api_object, typeof(get_API_controll_default));
                _api = apiObject();

                // User32.dll causes loading error
                //if (Marshal.GetLastWin32Error() == 0)
                //{
                //    get_API_controll_default apiObject = (get_API_controll_default)Marshal.GetDelegateForFunctionPointer(AddressPointer_api_object, typeof(get_API_controll_default));
                //    _api = apiObject();
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Library does not contain the correct function address \'get_API_controll_default\' that implements IAM_API");
                //}
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("An error occured when loading the API: " + e.Message);
                throw new Exception(e.Message);
            }
            
                
        }

        private void Load_run_lua_command_address_space() 
        {
            IntPtr AddressPointer_run_lua = GetProcAddress(_library, "API_run_lua_command");
            run_lua = (API_run_lua_command)Marshal.GetDelegateForFunctionPointer(AddressPointer_run_lua, typeof(API_run_lua_command));
        }

        #endregion

        #endregion

        #region Flags
        private bool _api_available = false; // Flag used to specify if api is loaded

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
            if (!_api_available) return "Error: api not available!";
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


        public void update_path(string apiPath)
        {
            Link_to_library(apiPath);
        }

        private void Link_to_library(string pathToLibrary)
        {
            Free_library();
            string fullPath = pathToLibrary;
            if(!System.IO.Path.IsPathRooted(pathToLibrary)) 
                fullPath = AppDomain.CurrentDomain.BaseDirectory + pathToLibrary;

            if (!System.IO.File.Exists(fullPath)) return;
            try
            {
                Load_library(fullPath);
                Load_api_controll();
                Load_run_lua_command_address_space();
                _api_available = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("An error occured when loading the API: " + e.Message + " Path: " + fullPath);
                throw;
            }
        }

        
        #region Library
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
        #endregion
    }
}
