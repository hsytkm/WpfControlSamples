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
        public static T FindVisualParent<T>(this DependencyObject sender)
            where T : DependencyObject
        {
            if (sender is null) return null;

            var parent = VisualTreeHelper.GetParent(sender);
            if (parent is T p) return p;

            return FindVisualParent<T>(parent);
        }

        /// <summary>
        /// 指定コントロールの以下の指定コントロールを取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool TryGetChildControl<T>(this DependencyObject d,  out T child)
            where T : DependencyObject
        {
            if (d is T control1)
            {
                child = control1;
                return true;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                if (TryGetChildControl<T>(VisualTreeHelper.GetChild(d, i), out var control2))
                {
                    child = control2;
                    return true;
                }
            }

            child = default;
            return false;
        }

    }
}