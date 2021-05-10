using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WpfControlSamples.ViewModels
{
    /// <summary>2次元配列と色付けルールをまとめて管理します</summary>
    /// <typeparam name="T">ValueType</typeparam>
    public record ColoringRules<T> where T : struct, IComparable<T>
    {
        /// <summary>色付けルールです。先頭の要素から順に判定されます。</summary>
        public IReadOnlyList<ColoringRule<T>> Rules { get; }

        /// <summary>色付けの判定順序（デフォは昇順）</summary>
        public bool IsDescendingOrder { get; }

        public ColoringRules(IEnumerable<ColoringRule<T>> coloringRules, bool isDescendingOrder = false)
        {
            IsDescendingOrder = isDescendingOrder;
            Rules = (coloringRules is IReadOnlyList<ColoringRule<T>> list) ? list : coloringRules.ToArray();
        }

        public Color GetColor(T value)
        {
            var compareOperator = IsDescendingOrder
                ? ColoringRule<T>.CompareOperator.GreaterThanOrEqualTo
                : ColoringRule<T>.CompareOperator.LessThanOrEqualTo;

            for (int i = 0; i < Rules.Count - 1; i++)
            {
                if (Rules[i].TryGetColor(value, compareOperator, out var color))
                    return color;
            }
            return Rules[^1].Color;
        }
    }

    /// <summary>値の色付けルールです</summary>
    public record ColoringRule<T> where T : struct, IComparable<T>
    {
        public enum CompareOperator
        {
            Equal, LessThan, LessThanOrEqualTo, GreaterThan, GreaterThanOrEqualTo,
        }

        public T Threshold { get; init; } = default;
        public Color Color { get; init; } = Colors.Black;

        public ColoringRule() { }
        public ColoringRule(T threshold, Color color) => (Threshold, Color) = (threshold, color);

        public bool TryGetColor(T value, CompareOperator compareOperator, out Color color)
        {
            // value(this) は Threshold(other) よりも小さい -> 負数
            var compare = value.CompareTo(Threshold);
            bool isHit = compareOperator switch
            {
                CompareOperator.Equal => compare == 0,
                CompareOperator.LessThan => compare < 0,
                CompareOperator.LessThanOrEqualTo => compare < 0 || compare == 0,
                CompareOperator.GreaterThan => compare > 0,
                CompareOperator.GreaterThanOrEqualTo => compare > 0 || compare == 0,
                _ => throw new NotImplementedException()
            };

            if (isHit) color = Color;
            return isHit;
        }
    }
}
