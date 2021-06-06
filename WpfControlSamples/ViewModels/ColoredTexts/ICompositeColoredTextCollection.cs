using System;
using System.Collections.Immutable;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// ICompositeColoredText のコレクションを保持します。
    /// 派生クラスは View における DataGrid の ItemsSource になります。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ICompositeColoredTextCollection<T> where T : ICompositeColoredText
    {
        /// <summary>
        /// ICompositeColoredText のコレクションです。
        /// DataGrid の ItemsSource になります。
        /// </summary>
        IImmutableList<T> Collection { get; }
    }
}
