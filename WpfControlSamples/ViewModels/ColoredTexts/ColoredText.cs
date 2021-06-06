using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// 文字列を色付け位置込みで管理する
    /// </summary>
    public class ColoredText
    {
        private static readonly IReadOnlyCollection<Range> _empty = Array.Empty<Range>();

        /// <summary>表示テキスト</summary>
        public string Text { get; }

        /// <summary>色付け文字の位置管理</summary>
        public IReadOnlyCollection<Range> ColoredRanges { get; private set; }

        public ColoredText(string text) => (Text, ColoredRanges) = (text, _empty);

        /// <summary>文字列と検索語から色付け文字の位置を求めます</summary>
        /// <param name="sourceText">ヒットの対象文字列</param>
        /// <param name="words">検索語</param>
        /// <returns>色付け文字の位置</returns>
        private static IEnumerable<Range> FilterByWords(string sourceText, IReadOnlyCollection<string> words)
        {
            if (string.IsNullOrEmpty(sourceText)) yield break;

            // char毎に色付けフラグを作る (yield内ではSpan使えないのでArrayPool)
            var isColoredChar = ArrayPool<bool>.Shared.Rent(sourceText.Length);
            try
            {
                foreach (var word in words)
                {
                    var index = sourceText.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    if (index < 0) continue;

                    for (var i = index; i < index + word.Length; ++i)
                        isColoredChar[i] = true;
                }

                // 色付けフラグをRangeに変換（まずは start の頭出し）
                int startIndex = GetFirstTrueIndex(isColoredChar, 0), endIndex;
                while (startIndex < isColoredChar.Length)
                {
                    for (endIndex = startIndex + 1; endIndex < isColoredChar.Length; ++endIndex)
                    {
                        if (!isColoredChar[endIndex])   // true から false に変わった
                        {
                            yield return new Range(startIndex, endIndex);   // true の Range を返す
                            break;
                        }
                        else if (endIndex == isColoredChar.Length - 1)      // true のまま最終文字まで至った
                        {
                            yield return new Range(startIndex, endIndex + 1);
                            break;
                        }
                    }
                    startIndex = GetFirstTrueIndex(isColoredChar, endIndex);
                }
            }
            finally
            {
                ArrayPool<bool>.Shared.Return(isColoredChar, clearArray: true);
            }

            // bool[] から true の index を頭出し
            static int GetFirstTrueIndex(ReadOnlySpan<bool> flags, int startIndex)
            {
                for (var i = startIndex; i < flags.Length; ++i)
                {
                    if (flags[i]) return i;
                }
                return flags.Length;
            }
        }

        /// <summary>文字列と検索語から色付け文字の位置を更新します</summary>
        public void FilterWords(IReadOnlyCollection<string> words) => ColoredRanges = FilterByWords(Text, words).ToArray();

        public void Clear() => ColoredRanges = _empty;
    }
}
