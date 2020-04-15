using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MessageBoxPage : ContentControl
    {
        public MessageBoxPage()
        {
            InitializeComponent();

            DataContext = new MessageBoxViewModel();
        }
    }

    class MessageBoxViewModel : MyBindableBase
    {
        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message;

        #region MessageBoxButton
        public ICommand OKCommand => _oKCommand ??
            (_oKCommand = new MyCommand<string>(text =>
                ShowMessageBox(text, MessageBoxButton.OK)));
        private ICommand _oKCommand;

        public ICommand OKCancelCommand => _oKCancelCommand ??
            (_oKCancelCommand = new MyCommand<string>(text =>
                ShowMessageBox(text, MessageBoxButton.OKCancel)));
        private ICommand _oKCancelCommand;

        public ICommand YesNoCommand => _yesNoCommand ??
            (_yesNoCommand = new MyCommand<string>(text =>
                ShowMessageBox(text, MessageBoxButton.YesNo)));
        private ICommand _yesNoCommand;

        public ICommand YesNoCancelCommand => _yesNoCancelCommand ??
            (_yesNoCancelCommand = new MyCommand<string>(text =>
                ShowMessageBox(text, MessageBoxButton.YesNoCancel)));
        private ICommand _yesNoCancelCommand;
        #endregion

        public IList<MessageBoxImage> MessageBoxImages { get; } =
            MyArrayExtension.GetEnums<MessageBoxImage>().Distinct().ToList();

        public MessageBoxImage SelectedImages
        {
            get => _selectedImages;
            set
            {
                if (SetProperty(ref _selectedImages, value))
                {
                    var res = MessageBox.Show("MessageBoxImage", "MessageBox.Show", MessageBoxButton.OK, _selectedImages);
                    UpdateResultMessage(res);
                }
            }
        }
        private MessageBoxImage _selectedImages;

        private void ShowMessageBox(string text, MessageBoxButton button)
        {
            var res = MessageBox.Show(text, "MessageBox.Show", button);
            UpdateResultMessage(res);
        }

        private void UpdateResultMessage(MessageBoxResult result)
        {
            Message = result.ToString();
        }
    }
}