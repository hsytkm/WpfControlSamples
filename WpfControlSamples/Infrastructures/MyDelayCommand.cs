#nullable enable
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfControlSamples.Infrastructures
{
    class MyDelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool>? _canExecute;
        private readonly int _millisecondsDelay;

        private bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                ChangeCanExecute();
            }
        }
        private bool _isBusy;

        public event EventHandler? CanExecuteChanged;

        public MyDelayCommand(Action action, Func<bool>? canExecute = null, int millisecondsDelay = 1000)
            => (_action, _canExecute, _millisecondsDelay) = (action, canExecute, millisecondsDelay);

        public async void Execute(object? parameter)
        {
            try
            {
                IsBusy = true;
                var task = Task.Run(() => _action());
                await Task.WhenAll(task, Task.Delay(_millisecondsDelay));
            }
            finally { IsBusy = false; }
        }

        public bool CanExecute(object? parameter)
        {
            if (IsBusy) return false;
            return (_canExecute is null) || _canExecute();
        }

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    class MyDelayCommand<T> : ICommand
    {
        private readonly Action<T?> _action;
        private readonly Func<bool>? _canExecute;
        private readonly int _millisecondsDelay;

        private bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                ChangeCanExecute();
            }
        }
        private bool _isBusy;

        public event EventHandler? CanExecuteChanged;

        public MyDelayCommand(Action<T?> action, Func<bool>? canExecute = null, int millisecondsDelay = 1000)
            => (_action, _canExecute, _millisecondsDelay) = (action, canExecute, millisecondsDelay);

        public async void Execute(object? parameter)
        {
            try
            {
                IsBusy = true;
                var task = Task.Run(() => _action(parameter is not null ? (T)parameter : default));
                await Task.WhenAll(task, Task.Delay(_millisecondsDelay));
            }
            finally { IsBusy = false; }
        }

        public bool CanExecute(object? parameter)
        {
            if (IsBusy) return false;
            return (_canExecute is null) || _canExecute();
        }

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
