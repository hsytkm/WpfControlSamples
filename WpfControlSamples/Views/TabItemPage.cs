using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views
{
    class TabItemPage : MyBindableBase
    {
        public Type PageType { get; }
        public string Name { get; }
        public Control? Content
        {
            get => _content;
            internal set => SetProperty(ref _content, value);
        }
        private Control? _content;

        internal TabItemPage(Type type, string name) => (PageType, Name) = (type, name);
        public TabItemPage(Type type)
            : this(type, type.ToString().Split('.').Last().Replace("Page", "")) { }
        public virtual void LoadContent()
        {
            if (Content is not null) ReleaseContent();
            Content = Activator.CreateInstance(PageType) as ContentControl;
        }
        public virtual void ReleaseContent()
        {
            if (Content is IDisposable d) d.Dispose();
            Content = null;
        }
    }

    class TabItemPageParent : TabItemPage
    {
        private readonly TabItemPage[] _children;

        internal TabItemPageParent(PageTable table)
            : base(typeof(PagesTabControl), table.Title)
        {
            _children = table.Types.Select(t => new TabItemPage(t)).ToArray();
        }
        public override void LoadContent()
        {
            var pagesTabControl = new PagesTabControl()
            {
                ItemsSource = _children,
                Background = Brushes.LightGray,
            };

            // 選択されたControlを読み込む（表示中のContentのみを読み込む）
            pagesTabControl.SelectionChanged += (_, e) =>
            {
                foreach (var item in e.RemovedItems)
                {
                    if (item is TabItemPage page)
                        page.ReleaseContent();

                }

                foreach (var item in e.AddedItems)
                {
                    if (item is TabItemPage page)
                        page.LoadContent();
                }
            };

            if (Content != null) ReleaseContent();
            Content = pagesTabControl;
        }
        public override void ReleaseContent()
        {
            foreach (var child in _children)
            {
                child.ReleaseContent();
            }
            Content = null;
        }
    }
}
