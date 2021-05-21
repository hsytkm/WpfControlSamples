using System;
using System.Collections;
using System.Collections.Generic;

namespace WpfControlSamples.ViewModels
{
    /// <summary>2次元配列を管理します</summary>
    /// <typeparam name="T">ValueType</typeparam>
    public record ValueArray2d<T> : IEnumerable<T> where T : struct
    {
        public int Width { get; }
        public int Height { get; }

        public int Columns => Width;
        public int Rows => Height;

        protected T[] _source;
        public ReadOnlySpan<T> GetSpan() => _source.AsSpan();

        public ValueArray2d(int width, int height, IEnumerable<T> source)
        {
            (Width, Height) = (width, height);
            _source = new T[Width * Height];

            int n = 0;
            foreach (var data in source)
            {
                if (n >= _source.Length) throw new IndexOutOfRangeException();
                _source[n++] = data;
            }
        }

        public ValueArray2d(T[,] array2d)
        {
            (Width, Height) = (array2d.GetLength(1), array2d.GetLength(0));
            _source = new T[Width * Height];

            int n = 0;
            foreach (var data in array2d)
            {
                if (n >= _source.Length) throw new IndexOutOfRangeException();
                _source[n++] = data;
            }
        }

        public ref T this[int x, int y]
        {
            get
            {
                if (IsOutOfRangeX(x)) throw new ArgumentOutOfRangeException(nameof(x));
                if (IsOutOfRangeY(y)) throw new ArgumentOutOfRangeException(nameof(y));
                return ref _source[Width * y + x];
            }
        }

        protected bool IsOutOfRangeX(int x) => x < 0 || Width <= x;
        protected bool IsOutOfRangeY(int y) => y < 0 || Height <= y;

        /// <summary>指定行の要素を全て列挙します</summary>
        public ReadOnlySpan<T> GetRowSpan(int y)
        {
            if (IsOutOfRangeY(y)) throw new ArgumentOutOfRangeException(nameof(y));
            return _source.AsSpan(Width * y, Width);
        }

#if false
        /// <summary>指定行の要素を全て列挙します</summary>
        public IReadOnlyList<IReadOnlyList<T>> GetRowsList()
        {
            var list = new IReadOnlyList<T>[Height];
            for (int y = 0; y < Height; y++)
            {
                list[y] = GetRowSpan(y).ToArray();
            }
            return list;
        }
#endif

        public IEnumerator<T> GetEnumerator() { foreach (T data in _source) yield return data; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
