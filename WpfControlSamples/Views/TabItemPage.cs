using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlSamples.Views
{
    public class TabItemPage
    {
        public Type PageType { get; }
        public string Name { get; }
        public Control Content { get; internal set; }
        internal TabItemPage(Type type, string name) => (PageType, Name) = (type, name);
        public TabItemPage(Type type)
            : this(type, type.ToString().Split('.').Last().Replace("Page", "")) { }
        public virtual void LoadContent()
        {
            if (Content != null) ReleaseContent();
            Content = (ContentControl)Activator.CreateInstance(PageType);

            //Debug.WriteLine($"LoadContent() : {PageType}");
        }
        public virtual void ReleaseContent()
        {
            if (Content is IDisposable d) d.Dispose();
            Content = null;

            //Debug.WriteLine($"ReleaseContent() : {PageType}");
        }
    }

    public class TabItemPageParent : TabItemPage
    {
        public readonly TabItemPage[] _children;
        internal TabItemPageParent(PageTable table)
            : base(typeof(PagesTabControl), table.Title)
        {
            _children = table.Types.Select(t => new TabItemPage(t)).ToArray();
        }
        public override void LoadContent()
        {
            // ◆Table内の全Pageを読んでるのでメモリ面でイマイチ。直す
            foreach (var child in _children)
            {
                child.LoadContent();
            }

            if (Content != null) ReleaseContent();
            Content = new PagesTabControl()
            {
                ItemsSource = _children,
                Background = Brushes.LightGray,
            };
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
