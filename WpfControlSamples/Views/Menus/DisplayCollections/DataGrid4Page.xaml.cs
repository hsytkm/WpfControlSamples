using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus;

public partial class DataGrid4Page : ContentControl
{
    public DataGrid4Page()
    {
        DataContext = new DataGrid4ViewModel();
        InitializeComponent();
    }

#if false
    // 列ヘッダのクリックで全列を選択する対応(xamlのみでの対応版) https://stackoverflow.com/a/24119018
    private static void DataGridColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        var columnHeader = sender as DataGridColumnHeader;
        if (columnHeader?.FindVisualParent<DataGrid>() is { } dataGrid)
        {
            dataGrid.SelectedCells.Clear();
            dataGrid.Focus();

            foreach (var item in dataGrid.Items)
                dataGrid.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
        }
    }
#endif
}

class DataGrid4ViewModel : MyBindableBase
{
    public ValueArray2d<int> SourceArray2d
    {
        get => _sourceArray2d;
        private set => SetProperty(ref _sourceArray2d, value);
    }
    private ValueArray2d<int> _sourceArray2d = default!;

    public ICommand UpdateDataCommand => _updateDataCommand ??= new MyCommand(UpdateData);
    private ICommand _updateDataCommand = default!;

    public DataGrid4ViewModel()
    {
        var ary2d = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        SourceArray2d = new ValueArray2d<int>(ary2d);
    }

    private readonly Random _random = new();
    private void UpdateData()
    {
        var rows = _random.Next(1, 10);
        var columns = _random.Next(1, 10);
        var array = new int[columns * rows];

        var maxValue = 100;
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = _random.Next(maxValue);
        }

        SourceArray2d = new ValueArray2d<int>(columns, rows, array);
    }
}
