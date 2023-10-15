using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlSamples.Views.Menus
{
    public partial class SystemColors2Page : ContentControl
    {
        public SystemColors2Page()
        {
            InitializeComponent();
            DataContext = SystemColorName.GetAllItems();
        }
    }

    public sealed record SystemColorName(string Name, SolidColorBrush Brush)
    {
        public static IReadOnlyList<SystemColorName> GetAllItems() =>
            typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .Select(x => (name: $"SystemColors.{x.Name}", brush: x.GetValue(null) as SolidColorBrush))
                    .Where(x => x.brush is not null)
                    .Select(x => new SystemColorName(x.name, x.brush!))
                    .ToArray();
    }
}
