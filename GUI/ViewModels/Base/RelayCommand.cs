using System;
using System.Windows.Input;



namespace VDM.GUI.ViewModels.Base
{
    /// <summary>
    /// Represents a bindable command between a View and a ViewModel.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the command.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;

        /// <summary>
        /// Initializes a bindable command instance in a ViewModel.
        /// </summary>
        /// <param name="execute">The function to call when the command fires.</param>
        /// <param name="canExecute">The function or property to call to check whether the command can fire.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute), $"Cannot instantiate new {nameof(RelayCommand<T>)} since method to execute is null.");

            _execute = execute;
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}