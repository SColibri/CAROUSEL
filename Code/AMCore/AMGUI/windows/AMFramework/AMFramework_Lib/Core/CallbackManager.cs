using AMFramework_Lib.Libs;
using System.Runtime.InteropServices;

namespace AMFramework_Lib.Core
{
    /// <summary>
    /// CallbackManager is a static class used for registering all the AMFramework callbacks
    /// </summary>
    public static class CallbackManager
    {

        #region Function Pointers
        /// <summary>
        /// function pointer to message callbacks
        /// </summary>
        private static AMFrameworkLib.MessageCallback? messageCallback;

        /// <summary>
        /// function pointer to error callbacks
        /// </summary>
        private static AMFrameworkLib.ErrorCallback? errorCallback;

        /// <summary>
        /// fucntion pointer to Script finished callbacks
        /// </summary>
        private static AMFrameworkLib.ScriptFinishedCallback? scriptFinishedCallback;

        /// <summary>
        /// function pointer progress updated callbacks
        /// </summary>
        private static AMFrameworkLib.ProgressUpdateCallback? progressUpdateCallback;
        #endregion

        #region Methods
        /// <summary>
        /// Registers AmFramework callbacks
        /// </summary>
        /// <param name="library"></param>
        /// <exception cref="Exception"></exception>
        public static void RegisterCallbacks(IntPtr library)
        {
            if (library != IntPtr.Zero)
            {
                try
                {
                    // Get pointers for registering callbacks
                    IntPtr[] callBacks =
                    {
                        Kernel32Lib.GetProcAddress(library, "RegisterMessageCallback"),
                        Kernel32Lib.GetProcAddress(library, "RegisterErrorCallback"),
                        Kernel32Lib.GetProcAddress(library, "RegisterScriptFinishedCallback"),
                        Kernel32Lib.GetProcAddress(library, "RegisterProgressUpdateCallback"),
                    };

                    // create new pointers for delegates
                    messageCallback = new(MessageCallbackEventHandle);
                    errorCallback = new(ErrorCallbackEventHandle);
                    scriptFinishedCallback = new(ScriptFinishedEventHandle);
                    progressUpdateCallback = new(ProgressUpdateEventHandle);

                    // Register function ponters
                    Marshal.GetDelegateForFunctionPointer<AMFrameworkLib.RegisterMessageCallback>(callBacks[0])(messageCallback);
                    Marshal.GetDelegateForFunctionPointer<AMFrameworkLib.RegisterErrorCallback>(callBacks[1])(errorCallback);
                    Marshal.GetDelegateForFunctionPointer<AMFrameworkLib.RegisterScriptFinishedCallback>(callBacks[2])(scriptFinishedCallback);
                    Marshal.GetDelegateForFunctionPointer<AMFrameworkLib.RegisterProgressUpdateCallback>(callBacks[3])(progressUpdateCallback);
                }
                catch (Exception e)
                {
                    throw new Exception($"Registering callbacks was not possible: {e.Message}");
                }
            }
        }
        #endregion

        #region Handles
        /// <summary>
        /// Message callback handle
        /// </summary>
        /// <param name="eventData"></param>
        private static void MessageCallbackEventHandle([MarshalAs(UnmanagedType.LPStr)] string eventData)
        {
            MessageCallbackEvent?.Invoke(eventData, EventArgs.Empty);
        }

        /// <summary>
        /// Error callback handle
        /// </summary>
        /// <param name="eventData"></param>
        private static void ErrorCallbackEventHandle([MarshalAs(UnmanagedType.LPStr)] string eventData)
        {
            ErrorCallbackEvent?.Invoke(eventData, EventArgs.Empty);
        }

        /// <summary>
        /// Script finished callback handle
        /// </summary>
        /// <param name="eventData"></param>
        private static void ScriptFinishedEventHandle([MarshalAs(UnmanagedType.LPStr)] string eventData)
        {
            ScriptFinishedCallbackEvent?.Invoke(eventData, EventArgs.Empty);
        }

        /// <summary>
        /// Progress update callback handle
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="eventProgress"></param>
        private static void ProgressUpdateEventHandle([MarshalAs(UnmanagedType.LPStr)] string eventData, int eventProgress)
        {
            ProgressUpdateCallbackEvent?.Invoke(new Tuple<string, int>(eventData.ToString(), eventProgress), EventArgs.Empty);
        }
        #endregion

        #region Events
        /// <summary>
        /// Expected sender is a string with the message from core implementation
        /// </summary>
        public static event EventHandler? MessageCallbackEvent;

        /// <summary>
        /// Expected sender is a string with error message
        /// </summary>
        public static event EventHandler? ErrorCallbackEvent;

        /// <summary>
        /// Expected sender is a string with script output
        /// </summary>
        public static event EventHandler? ScriptFinishedCallbackEvent;

        /// <summary>
        /// Expected sender is a Tuple<string, int>
        /// </summary>
        public static event EventHandler? ProgressUpdateCallbackEvent;
        #endregion
    }
}
