using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AMFramework_Lib.Core;
using Catel.Data;
using Catel.MVVM;
using Catel.Windows.Threading;
using SharpDX;

namespace AMFramework.Controller
{
    /// <summary>
    /// Callback controller that holds basic handles for basic API communication
    /// like error, messages, progress update and others.
    /// </summary>
    public class ControllerCallbacks : ViewModelBase, IDisposable
    {
        #region Fields
        /// <summary>
        /// API reference
        /// </summary>
        private IAMCore_Comm _comm;

        public enum MessageTypeEnum 
        { 
            None,
            Message,
            Error,
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ControllerCallbacks() 
        {
            MessageLog = new();
        }

        /// <summary>
        /// Default destructor
        /// </summary>
        ~ ControllerCallbacks()
        {
            // Remove Callbacks
            UnregisterCallBacks();
        }

        public void Dispose() 
        {
            // Remove Callbacks
            UnregisterCallBacks();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Register to API callbacks, implemented only for AMCore_libHandle
        /// </summary>
        public void RegisterCallbacks(IAMCore_Comm comm) 
        {
            // Remove callbacks is any to previous object
            UnregisterCallBacks();

            // change reference to current API
            _comm = comm;

            // Add handles to callbacks
            CallbackManager.MessageCallbackEvent += MessageHandle;
            CallbackManager.ErrorCallbackEvent += ErrorHandle;
            CallbackManager.ProgressUpdateCallbackEvent += UpdateProgressHandle;
            CallbackManager.ScriptFinishedCallbackEvent += ScriptEndHandle;

        }

        /// <summary>
        /// Unregister to callbacks
        /// </summary>
        private void UnregisterCallBacks() 
        {
            CallbackManager.MessageCallbackEvent -= MessageHandle;
            CallbackManager.ErrorCallbackEvent -= ErrorHandle;
            CallbackManager.ProgressUpdateCallbackEvent -= UpdateProgressHandle;
            CallbackManager.ScriptFinishedCallbackEvent -= ScriptEndHandle;
        }

        #endregion

        private void MessageHandle(object? sender, EventArgs e) 
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is string message)
                {
                    Message = message;
                    MessageLog.Add(new Tuple<string, MessageTypeEnum>(message, MessageTypeEnum.Message));
                }
            });

        }

        private void ErrorHandle(object? sender, EventArgs e)
        {
            if (sender is string message) 
            {
                ErrorMessage = message;
                MessageLog.Add(new Tuple<string, MessageTypeEnum>(message, MessageTypeEnum.Error));
            }
        }

        private void UpdateProgressHandle(object? sender, EventArgs e)
        {
            if (sender is Tuple<string, int> progress) { UpdateProgress = progress; }
        }

        private void ScriptEndHandle(object? sender, EventArgs e)
        {
            if (sender is string message) { ScriptEnd = message; }
        }


        public static readonly PropertyData MessageProperty = RegisterProperty("Message", typeof(string));
        public string Message 
        { 
            get => GetValue<string>(MessageProperty); 
            set => SetValue(MessageProperty, value); 
        }

        public static readonly PropertyData ErrorMessageProperty = RegisterProperty("ErrorMessage", typeof(string));
        public string ErrorMessage
        {
            get => GetValue<string>(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public static readonly PropertyData UpdateProgressProperty = RegisterProperty("UpdateProgress", typeof(Tuple<string, int>));
        public Tuple<string, int> UpdateProgress
        {
            get => GetValue<Tuple<string, int>>(UpdateProgressProperty);
            set => SetValue(UpdateProgressProperty, value);
        }

        public static readonly PropertyData ScriptEndProperty = RegisterProperty("ScriptEnd", typeof(string));
        public string ScriptEnd
        {
            get => GetValue<string>(ScriptEndProperty);
            set => SetValue(ScriptEndProperty, value);
        }

        public static readonly PropertyData MessageLogProperty = RegisterProperty("MessageLog", typeof(ObservableCollection<Tuple<string, MessageTypeEnum>>));
        public ObservableCollection<Tuple<string, MessageTypeEnum>> MessageLog
        {
            get => GetValue<ObservableCollection<Tuple<string, MessageTypeEnum>>>(MessageLogProperty);
            set => SetValue(MessageLogProperty, value);
        }
    }
}
