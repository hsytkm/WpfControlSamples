using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WpfControlSamples.ViewModels
{
    /// <inheritdoc cref="ICompositeColoredText"/>
    abstract record CompositeColoredTextBase : ICompositeColoredText
    {
        /// <summary>
        /// 継承レコードの ColoredText プロパティを返します。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ColoredText> GetColoredTextProperties();

        /// <summary>
        /// 検索の高速化用に 各ColoredText を連結した文字列を保持しておきます。
        /// IgnoreCase の仕様なので Lower です。
        /// </summary>
        private string ConcatLowerText
        {
            get
            {
                if (_concatLowerText is null)
                {
                    var sb = new StringBuilder();
                    foreach (var coloredText in GetColoredTextProperties())
                    {
                        sb.Append(coloredText.Text.ToLowerInvariant());
                        sb.Append(CompositeColoredText.Separator);      // ワードが密着すると意図通りに色付けされない
                    }
                    _concatLowerText = sb.ToString();
                }
                return _concatLowerText;
            }
        }
        private string? _concatLowerText;

        /// <summary>
        /// 引数の文字列にヒットする文字列に色を付けます</summary>
        /// </summary>
        /// <param name="words">色を付ける文字列</param>
        /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
        public bool ColorLetters(IReadOnlyCollection<string> words)
        {
            // words は Lower で通知される取り決め（高速化のため）
            var isHitAll = IsHitAllWords(ConcatLowerText, words);

            if (!isHitAll)
            {
                ClearColors();
            }
            else
            {
                // 文字列のヒット位置を更新
                foreach (var prop in GetColoredTextProperties())
                    prop.FilterWords(words);
            }
            return isHitAll;

            // 全ての検索文字列にヒットしかどうかを返します
            static bool IsHitAllWords(string source, IReadOnlyCollection<string> words)
            {
                foreach (var w in words)
                {
                    if (!source.Contains(w))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 文字列の色付けを解除します
        /// </summary>
        public void ClearColors()
        {
            foreach (var prop in GetColoredTextProperties())
                prop.Clear();
        }
    }
}