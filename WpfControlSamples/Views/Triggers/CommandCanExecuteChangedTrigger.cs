using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Triggers
{
    /*  Triggerの自作：Command.CanExecute の変化でトリガ発火するが、Event.Remove が怪しい…
     *
     *  ReferenceSource
     *    https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/windows/Controls/Primitives/ButtonBase.cs
     */
    class CommandCanExecuteChangedTrigger : TriggerBase<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(CommandCanExecuteChangedTrigger),
                new FrameworkPropertyMetadata(null, (sender, e) => ((CommandCanExecuteChangedTrigger)sender).OnCommandChanged(e)));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private readonly object Parameter = null;   // NotImprement

        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //}

        //protected override void OnDetaching()
        //{
        //    base.OnDetaching();
        //}

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ICommand oldCommand)
                UnhookCommand(oldCommand);

            if (e.NewValue is ICommand newCommand)
                HookCommand(newCommand);
        }

        private void UnhookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
            UpdateCanExecute();
        }

        private void HookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
            UpdateCanExecute();
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        private void UpdateCanExecute()
        {
            var canExecute = Command.CanExecute(Parameter);
            InvokeActions(canExecute);
        }
    }
}
