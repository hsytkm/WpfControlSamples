using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus;

public partial class ContextMenu2Page : UserControl
{
    public ContextMenu2Page()
    {
        DataContext = new ContextMenu2ViewModel();
        InitializeComponent();
    }
}

class ContextMenu2ViewModel : MyBindableBase
{
    public IReadOnlyList<NameValueKey> Items { get; } = NameValueKey.Data;

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
    private string _message = "This text is displayed on ContextMenu.";
}
