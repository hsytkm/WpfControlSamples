using System;
using System.Windows;
using System.Windows.Media;

namespace WpfControlSamples.Extensions
{
    static class VisualTreeHelperExtension
    {
        /// <summary>
        /// Find the visual parent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(this DependencyObject sender) where T : DependencyObject
        {
            if (sender is null) return null;

            var parent = VisualTreeHelper.GetParent(sender);
            if (parent is T p) return p;

            return FindVisualParent<T>(parent);
        }
    }
}