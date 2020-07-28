using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    class OpenFileDialogThenInvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title), typeof(string), typeof(OpenFileDialogThenInvokeCommandAction),
                new FrameworkPropertyMetadata("Open File"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                nameof(Filter), typeof(string), typeof(OpenFileDialogThenInvokeCommandAction),
                new FrameworkPropertyMetadata("All Files(*.*)|*.*"));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command), typeof(ICommand), typeof(OpenFileDialogThenInvokeCommandAction));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Title = this.Title,
                Filter = this.Filter
            };
            var ret = dialog.ShowDialog();
            var selectedPath = (ret.HasValue && ret.Value) ? dialog.FileName : null;

            if (selectedPath != null && Command.CanExecute(selectedPath))
                Command.Execute(selectedPath);
        }
    }
}