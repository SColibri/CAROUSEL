﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// implementation from  https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern

namespace AMFramework.Controller
{
    internal class RelayCommand : ICommand
    {
        #region Fields 
        readonly Action<object?> _execute;
        readonly Predicate<object?>? _canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action<object?> execute) : this(execute, null) { }
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute; _canExecute = canExecute;
        }
        #endregion // Constructors 
        #region ICommand Members 
        //[DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object? parameter) { _execute(parameter); }
        #endregion // ICommand Members 
    }
}
