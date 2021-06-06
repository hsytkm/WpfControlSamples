using System.Collections.Generic;
using System;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// 複数の ColoredText をまとめます。
    /// 派生クラスは View における DataGrid の Row になります。
    /// </summary>
    interface ICompositeColoredText
    {
        /// <summary>
        /// 引数の文字列にヒットする文字列に色を付けます</summary>
        /// </summary>
        /// <param name="words">色を付ける文字列</param>
        /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
        bool ColorLetters(IReadOnlyCollection<string> words);

        /// <summary>
        /// 文字列の色付けを解除します
        /// </summary>
        void ClearColors();
    }

    static class CompositeColoredText
    {
        /// <summary>
        /// 引数の検索語がTagにヒットするかを判定する Predicate を返します。
        /// CollectionView.Filter に設定します
        /// </summary>
        /// <param name="word">検索語</param>
        /// <returns>検索語のヒットを判定する Predicate</returns>
        public static Predicate<object> IsHitPredicate(string word)
        {
            // 空白で単語を分けて検索する（IgnoreCaseの仕様なので小文字化して渡す）
            var lowerWords = word.ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return lowerWords.Length > 0
            ? obj =>
            {
                var container = obj as ICompositeColoredText;

                return (container is not null)
                    ? container.ColorLetters(lowerWords)
                    : false;
            }
            : static obj =>
            {
                (obj as ICompositeColoredText)?.ClearColors();
                return true;
            };
        }
    }
}