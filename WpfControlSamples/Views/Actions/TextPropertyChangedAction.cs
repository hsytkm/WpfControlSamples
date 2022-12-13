using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace WpfControlSamples.Views.Actions;

public sealed class TextPropertyChangedAction : TriggerAction<TextBox>
{
    protected override void Invoke(object parameter)
    {
        // Binding を取得して UpdateSource で変更通知を発行します
        var bindingExpression = BindingOperations.GetBindingExpression(AssociatedObject, TextBox.TextProperty);
        bindingExpression?.UpdateSource();
    }
}
