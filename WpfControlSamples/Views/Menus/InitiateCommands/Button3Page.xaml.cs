#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class Button3Page : ContentControl
    {
        public Button3Page()
        {
            InitializeComponent();
            DataContext = new Button3ViewModel();
        }
    }

    class Button3ViewModel : MyBindableBase
    {
        public string Result
        {
            get => _result;
            private set => SetProperty(ref _result, value);
        }
        private string _result;

        public ICommand HeavyActionCommand => _heavyActionCommand ??=
            new MyAsyncCommand(async () =>
            {
                await Task.Delay(2000);
                Result = "Finish HeavyAction";
            });
        private ICommand _heavyActionCommand;

        public ICommand HeavyFuncCommand => _heavyFuncCommand ??=
            new MyAsyncCommand<int>(async parameter =>
            {
                var result = await Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    return parameter * 2;
                });
                Result = $"HeavyCalclation : {parameter} * 2 = {result}";
            });
        private ICommand _heavyFuncCommand;

        public ICommand DelayActionCommand => _delayActionCommand ??=
            new MyDelayCommand(() => Result = "Finish DelayAction", millisecondsDelay: 2000);
        private ICommand _delayActionCommand;

        public ICommand DelayFuncCommand => _delayFuncCommand ??=
            new MyDelayCommand<int>(
                parameter => Result = $"Finish DelayFunc => {parameter} * 2 = {parameter * 2}",
                millisecondsDelay: 2000);
        private ICommand _delayFuncCommand;

    }
}
