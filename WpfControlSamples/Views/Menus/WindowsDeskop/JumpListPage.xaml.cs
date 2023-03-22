using System.Windows.Controls;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus;

public partial class JumpListPage : UserControl
{
    public JumpListPage()
    {
        DataContext = new JumpListViewModel();
        InitializeComponent();
    }
}

internal sealed class JumpListViewModel : MyBindableBase
{
    public string SelectedFolderPath
    {
        get => _selectedFolderPath;
        set
        {
            if (SetProperty(ref _selectedFolderPath, value))
                JumpListManager.AddToRecentDirectory(value);
        }
    }
    private string _selectedFolderPath = "";
}
