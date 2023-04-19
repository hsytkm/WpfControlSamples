using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfControlSamples.Extensions;

internal static class DataGridColumnHeaderStyles
{
    // 列ヘッダのクリックで全列を選択する対応  https://stackoverflow.com/a/24119018
    // DataGrid継承クラス内で Resource を設定するための Style です。
    // アプリ全体で1箇所蚤の対応であれば xaml+CodeBehind で対応すれば良いと思います。

    internal static Style SelectAllColumnsStyle()
    {
        static void DataGridColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not DataGridColumnHeader columnHeader)
                return;

            if (columnHeader?.FindVisualParent<DataGrid>() is not { } dataGrid)
                return;

            dataGrid.SelectedCells.Clear();
            dataGrid.Focus();

            foreach (var item in dataGrid.Items)
                dataGrid.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
        }

        var style = new Style(typeof(DataGridColumnHeader));
        style.Setters.Add(new EventSetter(DataGridColumnHeader.ClickEvent, new RoutedEventHandler(DataGridColumnHeader_Click)));
        return style;
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

    <!--  列ヘッダのクリックで全列を選択する対応(xamlのみでの対応版) https://stackoverflow.com/a/24119018  -->
    <DataGrid.Resources>
        <Style TargetType = "{x:Type DataGridColumnHeader}" >
            < EventSetter Event="Click" Handler="DataGridColumnHeader_Click" />
        </Style>
    </DataGrid.Resources>
#endif
}
