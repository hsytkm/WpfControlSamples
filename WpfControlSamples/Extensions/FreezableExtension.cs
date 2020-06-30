using System;
using System.Windows;

namespace WpfControlSamples.Extensions
{
    static class FreezableExtension
    {
        public static T WithFreeze<T>(this T obj) where T : Freezable
        {
            obj.Freeze();
            return obj;
        }

    }
}
