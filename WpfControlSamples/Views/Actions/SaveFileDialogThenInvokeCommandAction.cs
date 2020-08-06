using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    class SaveFileDialogThenInvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title), typeof(string), typeof(SaveFileDialogThenInvokeCommandAction),
                new FrameworkPropertyMetadata("Save File"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                nameof(Filter), typeof(string), typeof(SaveFileDialogThenInvokeCommandAction),
                new FrameworkPropertyMetadata("All Files(*.*)|*.*"));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command), typeof(ICommand), typeof(SaveFileDialogThenInvokeCommandAction));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private readonly object Parameter = null;   // NotImprement

        protected override void Invoke(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                Title = this.Title,
                Filter = this.Filter
            };
            var ret = dialog.ShowDialog();
            var selectedPath = (ret.HasValue && ret.Value) ? dialog.FileName : null;

            if (!string.IsNullOrEmpty(selectedPath) && Command.CanExecute(Parameter))
                Command.Execute(selectedPath);
        }
    }
}