#nullable disable
using System;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class TreeViewPage : ContentControl
    {
        public TreeViewPage()
        {
            InitializeComponent();

            DataContext = new TreeViewViewModel();
        }
    }

    class TreeViewViewModel : MyBindableBase
    {
        public IList<TreeColors> Colors { get; }

        public TreeColors SelectedColors
        {
            get => _selectedColors;
            set => SetProperty(ref _selectedColors, value);
        }
        private TreeColors _selectedColors;

        public TreeViewViewModel()
        {
            var sourceChild = Models.SampleData.WpfColors.Select(x => new TreeColors(x.Name)).ToList();

            var source1 = sourceChild.Where(x => x.Name.Contains("Light")).ToList();
            var source2 = sourceChild.Where(x => x.Name.Contains("Blue")).ToList();

            Colors = new List<TreeColors>()
            {
                new TreeColors("Light", new List<TreeColors>()
                {
                    GetColors(source1, "Gray"),
                    GetColors(source1, "Green"),
                    GetColors(source1, "Blue"),
                }),
                new TreeColors("Blue", new List<TreeColors>()
                {
                    GetColors(source2, "Light"),
                    GetColors(source2, "Dark"),
                    GetColors(source2, "Medium"),
                })
            };

            static TreeColors GetColors(IList<TreeColors> source, string name)
            {
                var colors = source.Where(x => x.Name.Contains(name));
                return new TreeColors(name, colors.ToList());
            }
        }
    }

    class TreeColors
    {
        public string Name { get; }

        public IList<TreeColors> Child { get; }

        private static readonly IList<TreeColors> _emptyChild = Enumerable.Empty<TreeColors>().ToList();

        public TreeColors(string name)
            : this(name, _emptyChild) { }

        public TreeColors(string name, IList<TreeColors> child) =>
            (Name, Child) = (name, child);

        public override string ToString() => Name;
    }
}
