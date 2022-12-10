using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions;

// https://sourcechord.hatenablog.com/entry/2014/12/08/030947
// https://github.com/sourcechord/MarkupExtensionsForEvents/blob/master/MarkupExtensionsForEvents/InvokeCommandExtension.cs

[MarkupExtensionReturnType(typeof(EventHandler))]
public sealed class InvokeCommandExtension : MarkupExtension
{
    /// <summary>
    /// イベント発生時に呼び出すコマンドのパスを取得または設定します
    /// </summary>
    public PropertyPath Path { get; set; } = default!;

    private BindingTargetObject? _targetObject;

    public InvokeCommandExtension(PropertyPath bindingCommandPath)
    {
        Path = bindingCommandPath;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget pvt)
            return null;

        Type? type = pvt.TargetProperty switch
        {
            EventInfo ei => ei.EventHandlerType,
            MethodInfo mi => mi.GetParameters()[1].ParameterType,
            _ => null
        };
        if (type is null)
            return null;

        if (pvt.TargetObject is FrameworkElement element)
        {
            _targetObject = new BindingTargetObject();
            SetBinding(element.DataContext);
            element.DataContextChanged += (_, e) => SetBinding(e.NewValue);
        }
        else if (pvt.TargetObject is FrameworkContentElement contentElement)
        {
            _targetObject = new BindingTargetObject();
            SetBinding(contentElement.DataContext);
            contentElement.DataContextChanged += (_, e) => SetBinding(e.NewValue);
        }
        else
        {
            return null;
        }

        // ここで、イベントハンドラを作成し、マークアップ拡張の結果として返します
        if (this.GetType().GetMethod(nameof(PrivateHandlerGeneric), BindingFlags.NonPublic | BindingFlags.Instance) is not MethodInfo nonGenericMethod)
            return null;

        if (type.GetMethod("Invoke")?.GetParameters()[1].ParameterType is not Type argType)
            return null;

        // CommandParameter には、対象イベントハンドラの EventArgs が渡ります
        var genericMethod = nonGenericMethod.MakeGenericMethod(argType);
        return Delegate.CreateDelegate(type, this, genericMethod);
    }

    private void SetBinding(object dataContext)
    {
        var binding = new Binding()
        {
            Source = dataContext,
            Path = Path,
        };
        _ = BindingOperations.SetBinding(_targetObject, BindingTargetObject.TargetValueProperty, binding);
    }

    private void PrivateHandlerGeneric<T>(object sender, T e)
    {
        if (_targetObject?.TargetValue is ICommand command)
        {
            if (command.CanExecute(e))
                command.Execute(e);
        }
    }
}

/// <summary>
/// 実行対象のコマンドをバインディングターゲットとして保持するために使用するクラス
/// InvokeCommandExtension クラス内で使用します
/// </summary>
internal class BindingTargetObject : DependencyObject
{
    // Using a DependencyProperty as the backing store for TargetValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TargetValueProperty =
        DependencyProperty.Register(nameof(TargetValue), typeof(object), typeof(BindingTargetObject));

    public object? TargetValue
    {
        get => (object?)GetValue(TargetValueProperty);
        set => SetValue(TargetValueProperty, value);
    }
}
