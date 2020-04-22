using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace WpfControlSamples.Views.Triggers
{
    class OpenFileDialogAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title), typeof(string), typeof(OpenFileDialogAction),
                new FrameworkPropertyMetadata("Open File"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                nameof(Filter), typeof(string), typeof(OpenFileDialogAction),
                new FrameworkPropertyMetadata("All Files(*.*)|*.*"));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty SelectedFilePathProperty =
            DependencyProperty.Register(
                nameof(SelectedFilePath), typeof(string), typeof(OpenFileDialogAction),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedFilePath
        {
            get => (string)GetValue(SelectedFilePathProperty);
            set => SetValue(SelectedFilePathProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Title = this.Title,
                Filter = this.Filter
            };

            var ret = dialog.ShowDialog();
            SelectedFilePath = (ret.HasValue && ret.Value) ? dialog.FileName : null;
        }
    }
}