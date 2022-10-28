using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Windows;

namespace WpfControlSamples.Views.AttachedProperties;

// [WPFのBehaviorをStyleで使う方法 - かずきのBlog@hatena](https://blog.okazuki.jp/entry/2016/07/19/192918)

/// <summary>
/// Style内で Behavior を設定します
/// </summary>
public sealed class StyleBehaviorCollection : FreezableCollection<Behavior>
{
    public static readonly DependencyProperty StyleBehaviorsProperty =
        DependencyProperty.RegisterAttached("StyleBehaviors", typeof(StyleBehaviorCollection), typeof(StyleBehaviorCollection),
            new PropertyMetadata((sender, e) =>
            {
                if (e.OldValue == e.NewValue)
                    return;

                if (e.NewValue is not StyleBehaviorCollection behaviorCollection)
                    return;

                var behaviors = Interaction.GetBehaviors(sender);
                behaviors.Clear();

                // Behaviorのインスタンスは複数クラスにアタッチできないのでCloneして渡す
                foreach (var behavior in behaviorCollection.Select(x => (Behavior)x.Clone()))
                    behaviors.Add(behavior);
            }));

    public static StyleBehaviorCollection GetStyleBehaviors(DependencyObject obj) =>
        (StyleBehaviorCollection)obj.GetValue(StyleBehaviorsProperty);

    public static void SetStyleBehaviors(DependencyObject obj, StyleBehaviorCollection value) =>
        obj.SetValue(StyleBehaviorsProperty, value);

    protected override Freezable CreateInstanceCore() => new StyleBehaviorCollection();
}
