using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace WpfControlSamples.Views.AttachedProperties;

// [Style内でBehaviorやTriggerを設定したい - かずきのBlog@hatena](https://blog.okazuki.jp/entry/20110507/1304738683)
// [WPFのBehaviorをStyleで使う方法 - かずきのBlog@hatena](https://blog.okazuki.jp/entry/2016/07/19/192918)

/// <summary>
/// Style内で Trigger を設定します
/// </summary>
public sealed class StyleTriggerCollection : FreezableCollection<Microsoft.Xaml.Behaviors.TriggerBase>
{
    public static readonly DependencyProperty StyleTriggersProperty =
        DependencyProperty.RegisterAttached("StyleTriggers", typeof(StyleTriggerCollection), typeof(StyleTriggerCollection),
            new PropertyMetadata((sender, e) =>
            {
                if (e.OldValue == e.NewValue)
                    return;

                if (e.NewValue is not StyleTriggerCollection styleTriggerCollection)
                    return;

                var triggers = Interaction.GetTriggers(sender);
                triggers.Clear();

                foreach (var styleTrigger in styleTriggerCollection)
                {
                    var trigger = (Microsoft.Xaml.Behaviors.TriggerBase)styleTrigger.Clone();

                    foreach (var action in styleTrigger.Actions)
                        trigger.Actions.Add((Microsoft.Xaml.Behaviors.TriggerAction)action.Clone());

                    triggers.Add(trigger);
                }
            }));

    public static StyleTriggerCollection GetStyleTriggers(DependencyObject obj) =>
        (StyleTriggerCollection)obj.GetValue(StyleTriggersProperty);

    public static void SetStyleTriggers(DependencyObject obj, StyleTriggerCollection value) =>
        obj.SetValue(StyleTriggersProperty, value);

    protected override Freezable CreateInstanceCore() => new StyleTriggerCollection();
}
