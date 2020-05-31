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
    public partial class ComboBox2Page : ContentControl
    {
        public ComboBox2Page()
        {
            InitializeComponent();

            DataContext = new ComboBox2ViewModel();
        }
    }

    class ComboBox2ViewModel : MyBindableBase
    {
        /* ComboBox.ItemsSource に ListCollectionView を直接バインドしているが、
         * CollectionViewPage.xaml.cs のように CollectionViewSource.GetDefaultView() を使った方が
         * 疎結合でベターだと思う。
         */
        public ListCollectionView ItemsSource { get; }

        public IDictionary<string, string> Charactors { get; } =
            new Dictionary<string, string>()
            {
                { "承太郎", "スタープラチナ" },
                { "DIO", "ザ・ワールド" },
                { "ジョセフ", "ハーミットパープル" },
                { "ポルナレフ", "シルバーチャリオッツ" },
            };

        public string SelectedCharactorName
        {
            get => _selectedCharactorName;
            set => SetProperty(ref _selectedCharactorName, value);
        }
        private string _selectedCharactorName;

        public ComboBox2ViewModel()
        {
            // 主キーの登場順にグルーピングされるっぽい
            var itemsSource = new List<ComboBox2Data>
            {
                new ComboBox2Data("スタンド使い", "承太郎"),
                new ComboBox2Data("波紋使い", "リサリサ"),
                new ComboBox2Data("柱の男", "ワムウ"),
                new ComboBox2Data("柱の男", "カーズ"),
                new ComboBox2Data("柱の男", "エシディシ"),
                new ComboBox2Data("吸血鬼", "DIO"),
                new ComboBox2Data("スタンド使い", "DIO"),
                new ComboBox2Data("スタンド使い", "イギー"),
                new ComboBox2Data("波紋使い", "ツェペリ"),
                new ComboBox2Data("スタンド使い", "ポルナレフ"),
                new ComboBox2Data("波紋使い", "ストレイツォ"),
                new ComboBox2Data("吸血鬼", "ストレイツォ"),
            };
            var view = new ListCollectionView(itemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ComboBox2Data.Belonging)));

            ItemsSource = view;
        }
    }

    class ComboBox2Data
    {
        public string Belonging { get; }
        public string CharactorName { get; }
        public ComboBox2Data(string b, string n) =>
            (Belonging, CharactorName) = (b, n);
    }
}