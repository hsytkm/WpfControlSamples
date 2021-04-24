#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /// <summary>
    /// ObservableCollection の HasItem をチェックするように Count で判定したが、
    /// もっとスマートな実装がありそうな（ってか本プロジェクト内にある）気がする…
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    class IntegerToBooleanConverter : GenericValueConverter<int, bool>
    {
        public override bool Convert(int i, object parameter, CultureInfo culture) => i != 0;

        public override int ConvertBack(bool b, object parameter, CultureInfo culture) => b ? 1 : 0;
    }
}
