﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class DataGrid1Page : ContentControl
    {
        public DataGrid1Page()
        {
            InitializeComponent();
        }
    }

    class DataGrid1ViewModel : MyBindableBase
    {
        #region DataTable1
        public DataTable DataTableSource1 { get; } = GetDataTable(4, 4);

        private static DataTable GetDataTable(int rowLength, int columnLength)
        {
            var table = new DataTable();
            for (int c = 0; c < columnLength; c++)
            {
                table.Columns.Add($"C{c}");
            }
            for (int r = 0; r < rowLength; r++)
            {
                var row = table.NewRow();
                foreach (DataColumn col in table.Columns)
                {
                    row[col] = $"{col.ColumnName}-R{r}";
                }
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion

        #region DataTable2
        public int[,] Source2dArray { get; } = Get2dArray(3, 6);

        private static int[,] Get2dArray(int rowLength, int columnLength)
        {
            var data = new int[rowLength, columnLength];
            for (int r = 0; r < data.GetLength(0); r++)
            {
                for (int c = 0; c < data.GetLength(1); c++)
                {
                    data[r, c] = 1 + r * columnLength + c;
                }
            }
            return data;
        }
        #endregion

        public IReadOnlyList<DataGridProduct> Products { get; } = DataGridProduct.Products;
    }

    class DataGridProduct : MyBindableBase
    {
        public enum ProductMaker { Panasonic, Sony, Canon };

        // setterがないので Bland のみ View で書き換えられない
        public string Bland { get; }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _name;
        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
        private int _price;
        public bool HasStock
        {
            get => _hasStock;
            set => SetProperty(ref _hasStock, value);
        }
        private bool _hasStock;
        public ProductMaker Maker
        {
            get => _maker;
            set => SetProperty(ref _maker, value);
        }
        private ProductMaker _maker;
        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set => SetProperty(ref _releaseDate, value);
        }
        private DateTime _releaseDate;

        public DataGridProduct(string bland, string name, int price, bool hasStock, ProductMaker maker, DateTime releaseDate) =>
            (Bland, Name, Price, HasStock, Maker, ReleaseDate) = (bland, name, price, hasStock, maker, releaseDate);

        public static IReadOnlyList<DataGridProduct> Products = new DataGridProduct[]
        {
            new DataGridProduct("Lumix", "GH5", 5555, true, DataGridProduct.ProductMaker.Panasonic, new DateTime(2017,3,23)),
            new DataGridProduct("α", "α7", 777, false, DataGridProduct.ProductMaker.Sony, new DateTime(2013,11,15)),
            new DataGridProduct("EOS", "1DX", 10000, true, DataGridProduct.ProductMaker.Canon, new DateTime(2012,6,20)),
            new DataGridProduct("Kiss","X7", 3000, true, DataGridProduct.ProductMaker.Canon, new DateTime(2013,4,24)),
        };
    }

    class ProductMakerToComboBox
    {
        public string Label { get; set; }
        public DataGridProduct.ProductMaker Value { get; set; }
    }
}
