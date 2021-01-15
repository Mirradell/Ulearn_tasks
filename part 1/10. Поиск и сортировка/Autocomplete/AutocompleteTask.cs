using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            int left = Math.Max(0, LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count));
            var res = new List<string>();

            for (int i = 0; i < Math.Min(phrases.Count - left, count + left); i++)
                if (phrases[i + left].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                   res.Add(phrases[i + left]);

            return res.ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            return RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 
                   LeftBorderTask.GetLeftBorderIndex(phrases,   prefix, -1, phrases.Count) - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        [TestCase(new string[0],                            "a",     2,  new string[0])]
        [TestCase(new[] { "aa" },                           "a",     2,  new[] { "aa"})]
        [TestCase(new[] { "aa", "ab" },                     "a",     2,  new[] { "aa", "ab" })]
        [TestCase(new[] { "aa", "ab", "ac" },               "a",     2,  new[] { "aa", "ab" })]
        [TestCase(new[] { "aa", "ab", "ac" },               "z",     2,  new string[0])]
        [TestCase(new[] { "aa", "ab", "ac", "bc" },         "a",     2,  new[] { "aa", "ab" })]
        [TestCase(new[] { "aa", "ab", "ac", "bc" },         "a",     2,  new[] { "aa", "ab" })]
        [TestCase(new[] { "aa", "ab", "ac" },               "z",     2,  new string[0])]
        [TestCase(new[] { "a", "b", "c", "c", "d", "e" },   "c",     10, new[] { "c", "c" })]
        [TestCase(new[] { "a", "b", "c", "c", "d", "e" },   "c",     1,  new[] { "c" })]
        [TestCase(new[] { "a", "bcdef" },                   "",      2,  new[] { "a", "bcdef" })]
        [TestCase(new[] { "a", "bcdef" },                   "b",     2,  new[] { "bcdef" })]
        [TestCase(new[] { "a", "bcdef" },                   "bc",    2,  new[] { "bcdef" })]
        [TestCase(new[] { "a", "bcdef" },                   "bcd",   2,  new[] { "bcdef" })]
        [TestCase(new[] { "a", "bcdef" },                   "bcde",  2,  new[] { "bcdef" })]
        [TestCase(new[] { "a", "bcdef" },                   "bcdef", 2,  new[] { "bcdef" })]
        public void TopByPrefix_IsEmpty_WhenNoPhrases(IReadOnlyList<string> phrases, string prefix, int count, string[] expectedCount)
        {
            var res = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            Assert.AreEqual(res.Length, expectedCount.Length);
            Assert.AreEqual(res, expectedCount);
        }

        [Test]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "a",  2)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "b",  3)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "c",  2)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "d",  0)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "6",  0)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "",   7)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "cb", 1)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "aa", 1)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "az", 0)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "bz", 0)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "cz", 0)]
        [TestCase(new[] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "z",  0)]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(IReadOnlyList<string> phrases, string prefix, int expectedCount)
        {
            int actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            Assert.AreEqual(actualCount, expectedCount);
        }

    }
}