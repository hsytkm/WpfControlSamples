using Microsoft.Xaml.Behaviors;
using System.Windows;
using WpfControlSamples.Views.Controls;

namespace WpfControlSamples.Views.Actions
{
    class OpenFolderDialogAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title), typeof(string), typeof(OpenFolderDialogAction),
                new FrameworkPropertyMetadata("Open Folder"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty SelectedFolderPathProperty =
            DependencyProperty.Register(
                nameof(SelectedFolderPath), typeof(string), typeof(OpenFolderDialogAction),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedFolderPath
        {
            get => (string)GetValue(SelectedFolderPathProperty);
            set => SetValue(SelectedFolderPathProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (Window.GetWindow(AssociatedObject) is not Window window) return;

            var browser = new FolderBrowserDialog(Title);
            var result = browser.ShowDialog(window);

            SelectedFolderPath = (result is FolderBrowserDialog.Result.OK) ? browser.SelectedPath : "";
        }
    }
}
