using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace WpfControlSamples.ViewModels
{
    /// <summary>2次元配列と色付けルールをまとめて管理します</summary>
    /// <typeparam name="T">ValueType</typeparam>
    public record ColoredValueArray2d<T> : ValueArray2d<T> where T : struct, IComparable<T>
    {
        public ColoringRules<T> Rules { get; }

        public ColoredValueArray2d(int width, int height, IEnumerable<T> source, ColoringRules<T> rules)
            : base(width, height, source)
        {
            Rules = rules;
        }

        public ColoredValueArray2d(T[,] array2d, ColoringRules<T> rules)
            : base(array2d)
        {
            Rules = rules;
        }

        /// <summary>指定値 と 対応する色 のペアを取得します</summary>
        public (T value, Color color) GetValueColorPair(int x, int y)
        {
            var value = this[x, y];
            var color = Rules.GetColor(value);
            return (value, color);
        }
    }
}
