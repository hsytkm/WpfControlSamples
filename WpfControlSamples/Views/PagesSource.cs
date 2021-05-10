using System;
using System.Collections.Generic;
using WpfControlSamples.Views.Menus;

namespace WpfControlSamples.Views
{
    class PageTable
    {
        public string Title { get; }
        public IEnumerable<Type> Types { get; }
        public PageTable(string title, IEnumerable<Type> types) =>
            (Title, Types) = (title, types);
    }

    static class PagesSource
    {
        private static readonly IEnumerable<Type> _adornerPages = new[]
        {
            typeof(ResizableTextBoxPage),
            typeof(GhostAdornerPage),
            typeof(ResizeHandleAdornerPage),
        };

        private static readonly IEnumerable<Type> _animationPages = new[]
        {
            typeof(DoubleAnimationPage),
            typeof(ColorAnimationPage),
            typeof(PointAnimationPage),
            typeof(ThicknessAnimationPage),
            typeof(EasingAnimationPage),
            typeof(DoubleAnimationUsingKeyFramesPage),
            typeof(HideControlAnimation1Page),
            typeof(HideControlAnimation2Page),
            typeof(RotateAngleAnimationPage),
        };

        private static readonly IEnumerable<Type> _customControlPages = new[]
        {
            typeof(NumericUpDownPage),
        };

        private static readonly IEnumerable<Type> _dispatcherPages = new[]
        {
            typeof(DispatcherObjectPage),
            typeof(DispatcherTimerPage),
            typeof(DispatcherUnhandledExceptionPage),
            typeof(DispatcherCollectionPage),
        };

        private static readonly IEnumerable<Type> _displayCollection1Pages = new[]
        {
            typeof(ItemsControl1Page),
            typeof(TreeViewPage),
            typeof(ListBox1Page),
            typeof(ListBox2Page),
            typeof(ListBox3Page),
            typeof(ListBox4Page),
            typeof(ListBox5Page),
            typeof(ListBox6Page),
            typeof(ListBox7Page),
            typeof(ListBox8Page),
            typeof(ListBox9Page),
            typeof(ListBox10Page),
            typeof(ListView1Page),
            typeof(ListView2Page),
            typeof(ListView3Page),
            typeof(ListView4Page),
        };

        private static readonly IEnumerable<Type> _displayCollection2Pages = new[]
        {
            typeof(DataGrid1Page),
            typeof(DataGrid2Page),
            typeof(DataGrid3Page),
            typeof(DataGrid4Page),
            typeof(CollectionView1Page),
            typeof(CollectionView2Page),
            typeof(CollectionView3Page),
            typeof(CollectionView4Page),
        };

        private static readonly IEnumerable<Type> _editingTextPages = new[]
        {
            typeof(TextBox1Page),
            typeof(TextBox2Page),
            typeof(PasswordBoxPage),
            typeof(RichTextBoxPage),
            typeof(WatermarkPage),
        };

        private static readonly IEnumerable<Type> _graphicPages = new[]
        {
            typeof(LinePage),
            typeof(EllipsePage),
            typeof(PolygonPage),
            typeof(PathPage),
            typeof(CombinedGeometryPage),
            typeof(Brushes1Page),
            typeof(Brushes2Page),
            typeof(InkCanvasPage),
            typeof(Viewport3DPage),
            typeof(HsbColorPage),
            typeof(MovableRectangle1Page),
            typeof(MovableRectangle2Page),
            typeof(TrigonometricFunction1Page),
        };

        private static readonly IEnumerable<Type> _indicateActivityPages = new[]
        {
            typeof(ProgressBarPage),
        };

        private static readonly IEnumerable<Type> _informationPages = new[]
        {
            typeof(MenuPage),
            typeof(ToolBarPage),
            typeof(ContextMenuPage),
            typeof(PopupPage),
            typeof(ToolTipPage),
            typeof(ThumbPage),
            typeof(StatusBarPage),
        };

        private static readonly IEnumerable<Type> _initiateCommandPages = new[]
        {
            typeof(Button1Page),
            typeof(Button2Page),
            typeof(Button3Page),
            typeof(RepeatButtonPage),
            typeof(ToggleButton1Page),
        };

        private static readonly IEnumerable<Type> _interactionActionPages = new[]
        {
            typeof(CallMethodActionPage),
            typeof(InvokeCommandActionPage),
            typeof(ChangePropertyActionPage),
            typeof(ControlStoryboardActionPage),
            typeof(GoToStateActionPage),
            typeof(LaunchUriOrFileActionPage),
            typeof(PlaySoundActionPage),
            typeof(RemoveElementActionPage),
            typeof(RemoveItemInListBoxActionPage),
            typeof(CustomTriggerAction1Page),
            typeof(CustomTriggerAction2Page),
        };

        private static readonly IEnumerable<Type> _interactionBehaviorPages = new[]
        {
            typeof(BehaviorStylePage),
            typeof(DataStateBehaviorPage),
            typeof(MouseDragElementBehaviorPage),
            typeof(TranslateZoomRotateBehaviorPage),
            typeof(ConditionBehaviorPage),
            typeof(FluidMoveBehavior1Page),
            typeof(FluidMoveBehavior2Page),
            typeof(FluidMoveBehavior3Page),
            typeof(FluidMoveBehavior4Page),
            typeof(FluidMoveBehavior5Page),
            typeof(FluidMoveBehavior6Page),
        };

        private static readonly IEnumerable<Type> _interactionTriggerPages = new[]
        {
            typeof(BehaviorsEventTriggerPage),
            typeof(BehaviorsDataTriggerPage),
            typeof(KeyTriggerPage),
            typeof(TimerTriggerPage),
            typeof(PropertyChangedTriggerPage),
            typeof(CustomTriggerPage),
        };

        private static readonly IEnumerable<Type> _itemTemplatePages = new[]
        {
            typeof(DataTemplatePage),
            typeof(VisualStateManagerPage),
            typeof(ItemTemplateSelectorPage),
        };

        private static readonly IEnumerable<Type> _layoutMultiplePages = new[]
        {
            typeof(GridPage),
            typeof(GridSplitterPage),
            typeof(UniformGridPage),
            typeof(GroupBoxPage),
            typeof(StackPanelPage),
            typeof(DockPanelPage),
            typeof(WrapPanelPage),
            typeof(CanvasPage),
            typeof(TabControl1Page),
            typeof(TabControl2Page),
            typeof(TabControl3Page),
        };

        private static readonly IEnumerable<Type> _layoutSinglePages = new[]
        {
            typeof(ContentControlPage),
            typeof(BorderPage),
            typeof(ScrollViewerPage),
            typeof(ExpanderPage),
            typeof(ViewboxPage),
            typeof(BulletDecoratorPage),
            typeof(NavigationServicePage),
        };

        private static readonly IEnumerable<Type> _presentationPages = new[]
        {
            typeof(LabelPage),
            typeof(TextBlockPage),
            typeof(TextBlockLogPage),
            typeof(BulletinTextPage),
            typeof(ImagePage),
            typeof(ImageThumbnailPage),
            typeof(ImageZoomPage),
            typeof(ImageClickPage),
            typeof(MediaElementPage),
            typeof(WebBrowserPage),
            typeof(EmbeddedFontPage),
        };

        private static readonly IEnumerable<Type> _settingValuePages = new[]
        {
            typeof(SliderPage),
            typeof(ComboBox1Page),
            typeof(ComboBox2Page),
            typeof(ComboBox3Page),
            typeof(CheckBoxPage),
            typeof(RadioButton1Page),
            typeof(RadioButton2Page),
            typeof(CalendarPage),
        };

        private static readonly IEnumerable<Type> _systemPages = new[]
        {
            //typeof(BlankPage),
            //typeof(BlankPage),
            typeof(SystemTopPage),
            typeof(EnvironmentPage),
            typeof(EnvDirectoryPage),
            typeof(AssemblyPage),
            typeof(FileVerInfoPage),
            typeof(PropetriesResourcesPage),
            typeof(PropetriesSettingsPage),
        };

        private static readonly IEnumerable<Type> _triggerPages = new[]
        {
            typeof(EventTriggerPage),
            typeof(DataTriggerPage),
            typeof(MultiDataTriggerPage),
            typeof(TriggerPropertyPage),
            typeof(MultiTriggerPage),
        };

        private static readonly IEnumerable<Type> _uIElementPages = new[]
        {
            typeof(TransformsPage),
            typeof(EffectsPage),
            typeof(ClipPage),
            typeof(OpacityPage),
            typeof(VisualBrushPage),
            typeof(SaveToImagePage),
        };

        private static readonly IEnumerable<Type> _windowsDeskopPages = new[]
        {
            typeof(SystemColorsPage),
            typeof(MouseCursorPage),
            typeof(MessageBoxPage),
            typeof(ShowWindowPage),
            typeof(ApplicationCommandsPage),
            typeof(FileDialogPage),
            typeof(FileDropPage),
            typeof(FolderDialogPage),
            typeof(FullScreenPage),
        };

        private static readonly IEnumerable<Type> _xamlFunctionPages = new[]
        {
            typeof(NamePage),
            typeof(StaticResourcePage),
            typeof(DynamicResourcePage),
            typeof(MarkupPage),
            typeof(TypeConverterPage),
            typeof(ValueConverterPage),
            typeof(BindingSourcePage),
            typeof(MultiBinding1Page),
            typeof(MultiBinding2Page),
            typeof(MarkupExtensionPage),
            typeof(ObjectDataProviderPage),
            typeof(RoutedEventPage),
            typeof(DependencyPropertyPage),
            typeof(AttachedPropertyPage),
            typeof(AttachedEventPage),
            typeof(DynamicXamlLoadPage),
            typeof(ParseXamlPage),
        };

        public static IEnumerable<PageTable> AllPageTables { get; } = new[]
        {
            new PageTable("Systems", _systemPages),
            new PageTable("XamlFunctions", _xamlFunctionPages),
            new PageTable("WindowsDeskop", _windowsDeskopPages),
            new PageTable("Dispatchers", _dispatcherPages),
            new PageTable("Triggers", _triggerPages),
            new PageTable("InteractionBehaviors", _interactionBehaviorPages),
            new PageTable("InteractionTriggers", _interactionTriggerPages),
            new PageTable("InteractionActions", _interactionActionPages),
            new PageTable("Animations", _animationPages),
            new PageTable("Graphics", _graphicPages),
            new PageTable("UIElements", _uIElementPages),
            new PageTable("Informations", _informationPages),
            new PageTable("LayoutSingle", _layoutSinglePages),
            new PageTable("LayoutMultiple", _layoutMultiplePages),
            new PageTable("Presentation", _presentationPages),
            new PageTable("InitiateCommands", _initiateCommandPages),
            new PageTable("SettingValues", _settingValuePages),
            new PageTable("EditingText", _editingTextPages),
            new PageTable("IndicateActivity", _indicateActivityPages),
            new PageTable("DisplayCollections1", _displayCollection1Pages),
            new PageTable("DisplayCollections2", _displayCollection2Pages),
            new PageTable("ItemTemplate", _itemTemplatePages),
            new PageTable("Adorners", _adornerPages),
            new PageTable("CustomControls", _customControlPages),
        };
    }
}
