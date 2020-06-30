using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(object), typeof(DataTable))]
    class Array2dToDataTableConverter : GenericValueConverter<object, DataTable>
    {
        public override DataTable Convert(object value, object parameter, CultureInfo culture)
        {
            // 二次元配列 なら DataTable に変換する
            var type = value.GetType();
            if (type.IsArray && type.GetArrayRank() == 2)
            {
                var array = (Array)value;
                var rowLength = array.GetLength(0);
                var columnLength = array.GetLength(1);

                var table = new DataTable();

                for (int c = 0; c < columnLength; c++)
                {
                    table.Columns.Add("C" + c.ToString());
                }
                for (int r = 0; r < rowLength; r++)
                {
                    var row = table.NewRow();
                    for (int c = 0; c < columnLength; c++)
                    {
                        var col = table.Columns[c];
                        row[col] = array.GetValue(r, c);
                    }
                    table.Rows.Add(row);
                }
                return table;
            }

            throw new NotSupportedException(value.ToString());
        }

        public override object ConvertBack(DataTable dataTable, object parameter, CultureInfo culture) => default;
    }
}
