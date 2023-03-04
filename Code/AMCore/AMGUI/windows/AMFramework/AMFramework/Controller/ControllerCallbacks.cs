using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AMControls.Custom;
using AMControls.ExtensionMethods;
using AMControls.Interfaces;
using AMFramework_Lib.Core;
using Catel.Collections;
using Catel.Data;
using Catel.IO;
using Catel.MVVM;
using Catel.Windows.Threading;
using SharpDX;
using AMFramework_Lib.Logging;

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
        /// Text that be searcched in objects
        /// </summary>
        private string _searchText = "";

        /// <summary>
        /// Max number of messages to be stored in memory
        /// </summary>
        private int _maxMessages = 500;
        
        /// <summary>
        /// Amount of logs to be saved and discarded from memory
        /// each time it reaches the maxMessage limit
        /// </summary>
        private int _storeBatchSize = 400;

        /// <summary>
        /// Callback message type
        /// </summary>
        public enum MessageTypeEnum 
        { 
            None,
            Message,
            Error,
            Script,
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ControllerCallbacks() 
        {
            MessageLog = new();
            SearchMessages = new Command<string>(SearchMessagesAction);
            RemoveMessages = new Command<ISelectable?>(RemoveMessagesAction);
        }

        /// <summary>
        /// Default destructor
        /// </summary>
        ~ ControllerCallbacks()
        {
            Dispose(false);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Text to be searched in the message logs
        /// </summary>
        public string SearchText 
        {
            get => _searchText;
            set 
            {
                _searchText = value;
                SearchMessages.Execute(value);
            }
        }


        public static readonly PropertyData MessageProperty = RegisterProperty("Message", typeof(string));
        /// <summary>
        /// Last message property
        /// </summary>
        public string Message
        {
            get => GetValue<string>(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly PropertyData ErrorMessageProperty = RegisterProperty("ErrorMessage", typeof(string));
        /// <summary>
        /// Last error message
        /// </summary>
        public string ErrorMessage
        {
            get => GetValue<string>(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public static readonly PropertyData UpdateProgressProperty = RegisterProperty("UpdateProgress", typeof(Tuple<string, int>));
        /// <summary>
        /// Get/set Update progress data
        /// </summary>
        public Tuple<string, int> UpdateProgress
        {
            get => GetValue<Tuple<string, int>>(UpdateProgressProperty);
            set => SetValue(UpdateProgressProperty, value);
        }

        public static readonly PropertyData ScriptEndProperty = RegisterProperty("ScriptEnd", typeof(string));
        /// <summary>
        /// Get/set las script message
        /// </summary>
        public string ScriptEnd
        {
            get => GetValue<string>(ScriptEndProperty);
            set => SetValue(ScriptEndProperty, value);
        }

        public static readonly PropertyData MessageLogProperty = RegisterProperty("MessageLog", typeof(ObservableCollection<SelectableRow>));
        /// <summary>
        /// Message collection, stores all messages 
        /// </summary>
        public ObservableCollection<SelectableRow> MessageLog
        {
            get => GetValue<ObservableCollection<SelectableRow>>(MessageLogProperty);
            set 
            {
                SetValue(MessageLogProperty, value);

                // If message logs are too big, save to file
                if (MessageLog.Count > _maxMessages) SaveLog();
            }
        }
        #endregion

        #region IDispose
        private bool _disposed = false;

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!_disposed) 
            {
                if (disposing)
                {
                    // No managed resources to remove
                }

                // Save logs
                SaveLog(true);

                // Remove unmanaged
                UnregisterCallBacks();
                _disposed = true;
            }
        }

        #endregion

        #region Commands
        public Command<string> SearchMessages { get; private set; }

        /// <summary>
        /// Search in messages action
        /// </summary>
        /// <param name="parameter"></param>
        private void SearchMessagesAction(string parameter) 
        {
            MessageLog.ForEach(e => e.IsVisible = true);
            MessageLog.Where(e => !e.Search(parameter)).ForEach(e => e.IsVisible = false);
        }

        public Command<ISelectable?> RemoveMessages { get; private set; }

        /// <summary>
        /// Remove Messages action
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveMessagesAction(ISelectable? parameter)
        {
            // ContextMenu access, make sure that IsSelected flag is set
            if (parameter != null) parameter.IsSelected = true;
            
            // Get all selected items and remove
            SelectableRow[] vary = MessageLog.Where(e => e.IsSelected).ToArray();

            foreach (SelectableRow item in vary)
            {
                MessageLog.Remove(item);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Register to API callbacks, implemented only for AMCore_libHandle
        /// </summary>
        public void RegisterCallbacks() 
        {
            // Remove callbacks is any to previous object
            UnregisterCallBacks();

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

        /// <summary>
        /// Save the log file
        /// </summary>
        private void SaveLog(bool saveAll = false) 
        {
            // Check if directory exists
            if (!Directory.Exists("MessageLogs"))
            {
                Directory.CreateDirectory("MessageLogs");
            }

            try
            {
                // create new file
                string filename = $"MessageLogs/MessageLogs_{DateTime.Now.ToString("dd-MM-yyyy")}.log";
                StreamWriter logFile = File.AppendText(filename);

                // Append
                int lastIndex = saveAll ? 0 : Math.Max(MessageLog.Count - _storeBatchSize, 0);
                for (int i = MessageLog.Count - 1; i >= lastIndex; i--)
                {
                    logFile.WriteLine("");
                    logFile.WriteLine($"Entry: {MessageLog[i].TimeStamp} - Type: {MessageLog[i].Icon.ToString()}");
                    logFile.WriteLine($"{MessageLog[i].Text}");
                }

                // Remove 
                MessageLog.RemoveLast(_storeBatchSize);
            }
            catch (Exception e)
            {
                // Do nothing for now
                throw;
            }
            
        }

        #endregion

        #region Handles


        /// <summary>
        /// Message callback handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageHandle(object? sender, EventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is string message)
                {
                    Message = message;
                    MessageLog.Add(new SelectableRow() { Text = message, Icon = MessageTypeEnum.Message, AllowsMultiSelect = true });
                }
            });

        }

        /// <summary>
        /// Error message callback handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorHandle(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is string message)
                {
                    Message = message;
                    MessageLog.Add(new SelectableRow() { Text = message, Icon = MessageTypeEnum.Error, AllowsMultiSelect = true });
                    LoggerManager.Error(message);
                }
            });
        }

        /// <summary>
        /// Update progress callback handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateProgressHandle(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is Tuple<string, int> progress) { UpdateProgress = progress; }
            });
        }

        /// <summary>
        /// Script finished callback handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptEndHandle(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is string message)
                {
                    ScriptEnd = message;
                    MessageLog.Add(new SelectableRow() { Text = message, Icon = MessageTypeEnum.Script, AllowsMultiSelect = true });
                    LoggerManager.Info(message);
                }
            });
        }

        #endregion



    }
}
