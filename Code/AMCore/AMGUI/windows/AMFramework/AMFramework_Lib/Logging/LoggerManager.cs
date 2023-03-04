using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;

namespace AMFramework_Lib.Logging
{
    /// <summary>
    /// LoggerManager is a wrapper class for NLog logging
    /// </summary>
    public static class LoggerManager
    {
        /// <summary>
        /// NLog object
        /// </summary>
        private static Logger _logger;

        /// <summary>
        /// Setup logger and create new instance of NLog logger
        /// </summary>
        public static void Setup() 
        {
            LogManager.Configuration = new XmlLoggingConfiguration("Logging/NLog.config");
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Log trace
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message) 
        { 
            _logger?.Trace(message);
        }

        /// <summary>
        /// Log debug
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            _logger?.Debug(message);
        }

        /// <summary>
        /// Log Info
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            _logger?.Info(message);
        }

        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            _logger?.Warn(message);
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            _logger?.Error(message);
        }

        /// <summary>
        /// Log fatal exception
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            _logger?.Fatal(message);
        }
    }
}
