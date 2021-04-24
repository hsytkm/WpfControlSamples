using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfControlSamples.Infrastructures
{
    class MyAsyncCommand : ICommand
    {
        private readonly Func<Task> _asyncAction;
        private readonly Func<bool>? _canExecute;

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

        public MyAsyncCommand(Func<Task> asyncAction, Func<bool>? canExecute = null)
            => (_asyncAction, _canExecute) = (asyncAction, canExecute);

        public async void Execute(object? parameter)
        {
            try
            {
                IsBusy = true;
                await _asyncAction();
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

    class MyAsyncCommand<T> : ICommand
    {
        private readonly Func<T?, Task> _asyncAction;
        private readonly Func<bool>? _canExecute;
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

        public MyAsyncCommand(Func<T?, Task> asyncAction, Func<bool>? canExecute = null)
            => (_asyncAction, _canExecute) = (asyncAction, canExecute);

        public async void Execute(object? parameter)
        {
            try
            {
                IsBusy = true;
                await _asyncAction(parameter is not null ? (T)parameter : default);
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
