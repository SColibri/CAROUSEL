using System.Runtime.InteropServices;
using System.Text;

namespace AMFramework_Lib.Libs
{
    /// <summary>
    /// AMFrameworkLib list of all access points that the framework has.
    /// </summary>
    public static class AMFrameworkLib
    {
        /// <summary>
        /// Create an API object and returns a pointer.
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr get_API_controll_default();

        /// <summary>
        /// Run a command using the pointer to the API object and the command parameters. Commands are listed
        /// in a text file inside the Core application (c++).
        /// </summary>
        /// <param name="API_pointer"></param>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate IntPtr API_run_lua_command(IntPtr API_pointer, StringBuilder command, StringBuilder parameters);

        // ------------------------------------------------------------------------------------------------------------
        //                                                  CALLBACKS
        // ------------------------------------------------------------------------------------------------------------

        #region Fuction Delegates

        /// <summary>
        /// Message callback delegate
        /// </summary>
        /// <param name="command"></param>
        public delegate void MessageCallback([MarshalAs(UnmanagedType.LPStr)] string command);

        /// <summary>
        /// Error callback delegate
        /// </summary>
        /// <param name="errorText"></param>
        public delegate void ErrorCallback([MarshalAs(UnmanagedType.LPStr)] string errorText);

        /// <summary>
        /// Script finished delegate
        /// </summary>
        /// <param name="scriptMessage"></param>
        public delegate void ScriptFinishedCallback([MarshalAs(UnmanagedType.LPStr)] string scriptMessage);

        /// <summary>
        /// Progress update delegate
        /// </summary>
        /// <param name="progressText">Progress text</param>
        /// <param name="progress">0 - 100 value that represents the progress in percentage</param>
        public delegate void ProgressUpdateCallback([MarshalAs(UnmanagedType.LPStr)] string progressText, int progress);

        #endregion

        #region Callbacks
        /// <summary>
        /// Message callbacks, messages from the frameworks that should be prompted to the user
        /// </summary>
        /// <param name="message"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RegisterMessageCallback(MessageCallback message);


        /// <summary>
        /// Error callbacks, handled error notifications.
        /// </summary>
        /// <param name="message"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RegisterErrorCallback(ErrorCallback message);


        /// <summary>
        /// Script finished callback, notifies when a script has completed running and returns the output
        /// </summary>
        /// <param name="message"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RegisterScriptFinishedCallback(ScriptFinishedCallback message);

        /// <summary>
        /// Progress update callback, reports the progress of a task
        /// </summary>
        /// <param name="message"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RegisterProgressUpdateCallback(ProgressUpdateCallback message);
        #endregion



    }
}
