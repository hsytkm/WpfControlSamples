using System;
using System.Collections.Generic;
using System.Linq;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views
{
    class HomeMenuItem
    {
        // 下記ページを参考に区分
        // https://docs.microsoft.com/ja-jp/dotnet/framework/wpf/controls/control-library
        public enum PageType
        {
            //Systems,
            //BottomTab,
            //XamlFunctions,
            //UIFunctions,
            //Effects,
            //LayoutSingle,
            //LayoutMultiple,
            //Presentation,
            //InitiateCommands,
            //SettingValues,
            //EditingText,
            IndicateActivity,
            //DisplayCollections,
            //ItemTemplate,
            //Triggers,
            //StateTriggers,
            //MessagingCenter,
            //About,
        }

        /// <summary>各メニューに対応するページ辞書</summary>
        public static IDictionary<PageType, Type> PagesMap { get; } = new Dictionary<PageType, Type>()
        {
            //[PageType.Systems] = typeof(SystemsMenuPage),
            //[PageType.BottomTab] = typeof(BottomTabPage),
            //[PageType.XamlFunctions] = typeof(XamlFunctionsMenuPage),
            //[PageType.UIFunctions] = typeof(UIFunctionsMenuPage),
            //[PageType.Effects] = typeof(EffectsMenuPage),
            //[PageType.LayoutSingle] = typeof(LayoutSingleMenuPage),
            //[PageType.LayoutMultiple] = typeof(LayoutMultipleMenuPage),
            //[PageType.Presentation] = typeof(PresentationMenuPage),
            //[PageType.InitiateCommands] = typeof(InitiateCommandsMenuPage),
            //[PageType.SettingValues] = typeof(SettingValuesMenuPage),
            //[PageType.EditingText] = typeof(EditingTextMenuPage),
            //[PageType.IndicateActivity] = typeof(IndicateActivityMenuPage),
            //[PageType.DisplayCollections] = typeof(DisplayCollectionsMenuPage),
            //[PageType.ItemTemplate] = typeof(ItemTemplateMenuPage),
            //[PageType.Triggers] = typeof(TriggersMenuPage),
            //[PageType.StateTriggers] = typeof(StateTriggersMenuPage),
            //[PageType.MessagingCenter] = typeof(MessagingCenterPage),
            //[PageType.About] = typeof(AboutPage),
        };

        public PageType Type { get; }

        public string Title { get; }

        public HomeMenuItem(PageType type, string title) => (Type, Title) = (type, title);

        public static IList<HomeMenuItem> AllItems { get; } =
            MyArrayExtension.GetEnums<PageType>()
                .Select(t => new HomeMenuItem(t, Enum.GetName(typeof(PageType), t))).ToList();
    }
}
