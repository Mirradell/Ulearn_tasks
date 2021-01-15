using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            int leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, left, right);

            for (int i = Math.Max(0, leftBorder); i < phrases.Count; i++)
            {
                if (prefix.StartsWith(phrases[i], StringComparison.OrdinalIgnoreCase))
                    continue;
                if (phrases[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (string.Compare(prefix, phrases[i], StringComparison.OrdinalIgnoreCase) < 0)
                    return i;
            }
            
            return phrases.Count;
        }
    }
}