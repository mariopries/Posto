// <copyright file="DelegateCommandT.cs" company="Nish Sivakumar">
// Copyright (c) Nish Sivakumar. All rights reserved.
// </copyright>

namespace NS.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Initializes a new instance of the DelegateCommand<T> class.
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> executeMethod;

        private readonly Func<T, bool> canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecuteMethod != null ? this.canExecuteMethod((T)parameter) : true;
        }

        public void Execute(object parameter)
        {
            if (this.executeMethod != null)
            {
                this.executeMethod((T)parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
