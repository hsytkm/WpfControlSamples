using System;
using System.Collections.Generic;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListView3Page : ContentControl
    {
        public ListView3Page()
        {
            InitializeComponent();
            DataContext = new ListView3ViewModel();
        }
    }

    class ListView3ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public ICommand SelectedItemsCopyCommand =>
            _selectedItemsCopyCommand ??= new MyCommand<object>(param =>
            {
                var items = ((System.Collections.IList)param).OfType<ColorListViewItem>();
                var builder = new StringBuilder();

                // Viewの選択順に並んでいるので元コレクションの並びにソート
                var sortedItems = items.OrderBy(x => SourceColors.IndexOf(x));
                foreach (var item in sortedItems)
                {
                    // View / ViewModel のそれぞれで勝手に index を管理している（VMで一元管理した方が良い）
                    var index = SourceColors.IndexOf(item) + 1;
                    builder.AppendLine($"{index}\t{item.Name}\t{item.ColorLevel}");
                }
                Clipboard.SetText(builder.ToString());
            });
        private ICommand _selectedItemsCopyCommand;

        public ListView3ViewModel() { }
    }
}
