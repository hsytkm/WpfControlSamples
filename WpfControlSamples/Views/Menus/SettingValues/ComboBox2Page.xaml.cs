#nullable disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfControlSamples.Views.TypeConverters;

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
        #region Grouping
        /* ComboBox.ItemsSource に ListCollectionView を直接バインドしているが、
         * CollectionViewPage.xaml.cs のように CollectionViewSource.GetDefaultView() を使った方が
         * 疎結合でベターだと思う。
         */
        public ListCollectionView ItemsSource => _itemsSource ??= GetItemsSource();
        private ListCollectionView _itemsSource;

        class ComboBox2Data
        {
            public string Belonging { get; }
            public string CharactorName { get; }
            public ComboBox2Data(string b, string n) => (Belonging, CharactorName) = (b, n);
        }

        private ListCollectionView GetItemsSource()
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
            return view;
        }
        #endregion

        #region Dictionary
        public IDictionary<string, string> CharactorsDictionary { get; } =
            new Dictionary<string, string>()
            {
                { "Jotaro", "Star Platinum" },
                { "DIO", "World 21" },
                { "Joseph", "Hermit Purple" },
                { "Polnareff", "Silver Chariot" },
            };

        public string SelectedCharactorName
        {
            get => _selectedCharactorName;
            set => SetProperty(ref _selectedCharactorName, value);
        }
        private string _selectedCharactorName;
        #endregion

        #region Enum
        public JoJoStoryEnum SelectedStory
        {
            get => _selectedStory;
            set => SetProperty(ref _selectedStory, value);
        }
        private JoJoStoryEnum _selectedStory = JoJoStoryEnum.StardustCrusaders;
        #endregion

        public JoJoStoryEnumWithDescription SelectedValue4
        {
            get => _selectedValue4;
            set => SetProperty(ref _selectedValue4, value);
        }
        private JoJoStoryEnumWithDescription _selectedValue4;

        public JoJoStoryEnumWithDescription SelectedValue5
        {
            get => _selectedValue5;
            set => SetProperty(ref _selectedValue5, value);
        }
        private JoJoStoryEnumWithDescription _selectedValue5;
    }

    // ◆本当は ComboBox2ViewModelクラス の中に定義したかったけど、xamlからのTypeの参照実装が分からなかった…
    public enum JoJoStoryEnum
    {
        PhantomBlood,
        BattleTendency,
        StardustCrusaders,
        DiamondIsUnbreakable,
        GoldenWind,
        StoneOcean,
        SteelBallRun,
        JoJolion
    };

    // ◆本当は ComboBox2ViewModelクラス の中に定義したかったけど、xamlからのTypeの参照実装が分からなかった…
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum JoJoStoryEnumWithDescription
    {
        [Description("ファントムブラッド")]
        PhantomBlood = 1,
        [Description("戦闘潮流")]
        BattleTendency,
        [Description("スターダストクルセイダース")]
        StardustCrusaders,
        [Description("ダイヤモンドは砕けない")]
        DiamondIsUnbreakable,
        GoldenWind,
        StoneOcean,
        SteelBallRun,
        JoJolion
    };

    // https://stackoverflow.com/questions/4306743/wpf-data-binding-how-to-data-bind-an-enum-to-combo-box-using-xaml
    public class EnumDescriptionConverter : IValueConverter
    {
        public static readonly EnumDescriptionConverter Shared = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueText = value.ToString();
            if (value is Enum)
            {
                var fieldInfo = value.GetType().GetField(valueText);
                var attributes = fieldInfo?.GetCustomAttributes<DescriptionAttribute>(false);
                return attributes?.FirstOrDefault()?.Description ?? valueText;
            }
            return valueText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
