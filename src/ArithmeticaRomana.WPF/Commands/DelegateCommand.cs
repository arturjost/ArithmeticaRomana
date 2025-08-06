using System.Windows.Input;

namespace ArithmeticaRomana.WPF.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public DelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(ConvertParameter(parameter));
        }

        public void Execute(object? parameter)
        {
            _execute(ConvertParameter(parameter));
        }

        private T? ConvertParameter(object? parameter)
        {
            if (parameter is T validParameter)
            {
                return validParameter;
            }
            return default;
        }
    }
}
