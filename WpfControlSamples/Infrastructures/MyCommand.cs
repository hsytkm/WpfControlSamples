using System;
using System.Windows.Input;

namespace WpfControlSamples.Infrastructures
{
    class MyCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public MyCommand(Action execute, Func<bool>? canExecute = null)
            => (_action, _canExecute) = (execute, canExecute);

        public void Execute(object? parameter) => _action();

        public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    class MyCommand<T> : ICommand
    {
        private readonly Action<T?> _action;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public MyCommand(Action<T?> execute, Func<bool>? canExecute = null)
            => (_action, _canExecute) = (execute, canExecute);

        public void Execute(object? parameter) => _action(parameter is not null ? (T)parameter : default);

        public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
