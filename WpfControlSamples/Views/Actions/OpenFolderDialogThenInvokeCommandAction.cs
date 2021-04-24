#nullable disable
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using WpfControlSamples.Views.Controls;

namespace WpfControlSamples.Views.Actions
{
    class OpenFolderDialogThenInvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title), typeof(string), typeof(OpenFolderDialogThenInvokeCommandAction),
                new FrameworkPropertyMetadata("Open Folder"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command), typeof(ICommand), typeof(OpenFolderDialogThenInvokeCommandAction));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private readonly object Parameter = null;   // NotImprement

        protected override void Invoke(object parameter)
        {
            if (!(Window.GetWindow(AssociatedObject) is Window window)) return;

            var browser = new FolderBrowserDialog(Title);
            var result = browser.ShowDialog(window);
            var selectedPath = (result == FolderBrowserDialog.Result.OK) ? browser.SelectedPath ?? null : null;

            if (!string.IsNullOrEmpty(selectedPath) && Command.CanExecute(Parameter))
                Command.Execute(selectedPath);
        }
    }
}
