using System.Windows.Controls;
using System.Windows.Input;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus;

public partial class MarkupCommandPage : UserControl
{
    public MarkupCommandPage()
    {
        DataContext = new MarkupCommandViewModel();
        InitializeComponent();
    }
}

class MarkupCommandViewModel : MyBindableBase
{
    public int Counter
    {
        get => _counter;
        private set => SetProperty(ref _counter, value);
    }
    private int _counter;

    public ICommand CountUp3Command => _countUp3Command ??= new MyCommand(() => Counter += 3);
    private ICommand? _countUp3Command;

    // CommandParameter には、対象イベントハンドラの EventArgs が渡ります
    public ICommand CountDown2Command => _countDown2Command ??= new MyCommand<object?>(args => Counter -= 2);
    private ICommand? _countDown2Command;
}
