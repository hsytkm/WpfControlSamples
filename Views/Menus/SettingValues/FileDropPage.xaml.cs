using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class FileDropPage : ContentControl
    {
        public FileDropPage()
        {
            InitializeComponent();
        }
    }

    class FileDropViewModel : MyBindableBase
    {
        public IList<string> DroppedPaths
        {
            get => _droppedPaths;
            set
            {
                if (SetProperty(ref _droppedPaths, value))
                {
                    DroppedPathsText = (value is null) ? null : ToNumberedText(value);
                }

                static string ToNumberedText(IList<string> paths)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < paths.Count; ++i)
                        sb.AppendLine($"{i} : " + paths[i]);

                    return sb.ToString();
                }
            }
        }
        private IList<string> _droppedPaths;

        public ICommand ClearFilePathCommand => _clearFilePathCommand ??
            (_clearFilePathCommand = new MyCommand(
                () => DroppedPaths = null,
                () => DroppedPaths != null && DroppedPaths.Any()));
        private ICommand _clearFilePathCommand;

        public string DroppedPathsText
        {
            get => _droppedPathsText;
            set
            {
                if (SetProperty(ref _droppedPathsText, value))
                {
                    (ClearFilePathCommand as MyCommand).ChangeCanExecute();
                }
            }
        }
        private string _droppedPathsText;
    }
}