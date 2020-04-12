using System;
using System.Collections.Generic;
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
    /// <summary>
    /// ProcessingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressBarPage : ContentControl
    {
        public ProgressBarPage()
        {
            InitializeComponent();

            DataContext = new ProgressBarViewModel();
        }
    }

    class ProgressBarViewModel : MyBindableBase
    {
        public int WaitSeconds { get; } = 6;   // 計測時間

        public double ProgressRatio
        {
            get => _progressRatio;
            private set => SetProperty(ref _progressRatio, value);
        }
        private double _progressRatio;

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                if (SetProperty(ref _isProcessing, value))
                {
                    // 処理実行中はコマンド開始を禁止する
                    (StartProcess as MyCommand).ChangeCanExecute();
                }
            }
        }
        private bool _isProcessing;

        public ICommand StartProcess => _startProcess ??
            (_startProcess = new MyCommand(async () =>
            {
                IsProcessing = true;
                ProgressRatio = 0;

                var sec = 0;
                do
                {
                    await Task.Delay(1000);
                    ProgressRatio = (++sec) / (double)WaitSeconds;
                }
                while (sec < WaitSeconds);

                IsProcessing = false;
            },
            () => !IsProcessing));
        private ICommand _startProcess;
    }
}