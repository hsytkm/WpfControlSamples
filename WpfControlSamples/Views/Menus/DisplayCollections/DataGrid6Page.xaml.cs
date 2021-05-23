using System;
using System.Collections;
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
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid6Page : ContentControl
    {
        public DataGrid6Page()
        {
            DataContext = new DataGrid6ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid6ViewModel : MyBindableBase
    {
        public int RowColLength
        {
            get => _rowColLength;
            set => SetProperty(ref _rowColLength, value);
        }
        private int _rowColLength = 256;

        public ValueArray2d<int> SourceArray2d
        {
            get => _sourceArray2d;
            private set => SetProperty(ref _sourceArray2d, value);
        }
        private ValueArray2d<int> _sourceArray2d = default!;

        public ICommand UpdateDataCommand => _updateDataCommand ??= new MyCommand(UpdateData);
        private ICommand _updateDataCommand = default!;

        private void UpdateData()
        {
            var array2d = new int[RowColLength, RowColLength];
            for (int i = 0; i < array2d.GetLength(0); i++)
            {
                for (int j = 0; j < array2d.GetLength(1); j++)
                    array2d[i, j] = (i * 1000) + j;
            }
            SourceArray2d = new ValueArray2d<int>(array2d);
        }
    }
}
