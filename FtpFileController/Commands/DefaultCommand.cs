using System;
using System.Windows.Input;

namespace FtpFileController.Commands {
    public class DefaultCommand : ICommand {
        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteLambda;

        public DefaultCommand(Action<object> executeAction, Func<object, bool> canExecuteLambda = null) {
            _executeAction = executeAction;
            _canExecuteLambda = canExecuteLambda;
        }

        public bool CanExecute(object parameter) {
            return _canExecuteLambda?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter) => _executeAction(parameter);
    }
}