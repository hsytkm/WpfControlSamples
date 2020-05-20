using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Windows;

// Interaction Triggers in Style in ResourceDictionary WPF
// https://stackoverflow.com/questions/22321966/interaction-triggers-in-style-in-resourcedictionary-wpf
namespace WpfControlSamples.Views.Resources
{
    /// <summary>
    /// <see cref="FrameworkTemplate"/> for InteractivityElements instance
    /// <remarks>Subclassed for forward compatibility, perhaps one day <see cref="FrameworkTemplate"/> </remarks>
    /// <remarks>will not be partially internal</remarks>
    /// </summary>
    class InteractivityTemplate : DataTemplate { }

    /// <summary>
    /// Holder for interactivity entries
    /// </summary>
    class InteractivityItems : FrameworkElement
    {
        /// <summary>
        /// Storage for triggers
        /// </summary>
        public new List<Microsoft.Xaml.Behaviors.TriggerBase> Triggers =>
            _triggers ?? (_triggers = new List<Microsoft.Xaml.Behaviors.TriggerBase>());
        private List<Microsoft.Xaml.Behaviors.TriggerBase> _triggers;

        /// <summary>
        /// Storage for Behaviors
        /// </summary>
        public List<Behavior> Behaviors => _behaviors ?? (_behaviors = new List<Behavior>());
        private List<Behavior> _behaviors;

        #region Template attached property

        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.RegisterAttached(
                "Template", typeof(InteractivityTemplate), typeof(InteractivityItems),
                new PropertyMetadata(default(InteractivityTemplate), OnTemplateChanged));

        public static InteractivityTemplate GetTemplate(DependencyObject obj)
            => (InteractivityTemplate)obj.GetValue(TemplateProperty);

        public static void SetTemplate(DependencyObject obj, InteractivityTemplate value)
            => obj.SetValue(TemplateProperty, value);

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dt = (InteractivityTemplate)e.NewValue;
            dt.Seal();

            var ih = (InteractivityItems)dt.LoadContent();
            var bc = Interaction.GetBehaviors(d);
            var tc = Interaction.GetTriggers(d);

            foreach (var behavior in ih.Behaviors)
                bc.Add(behavior);

            foreach (var trigger in ih.Triggers)
                tc.Add(trigger);
        }

        #endregion
    }
}
