using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public class MyClass
    {
        public int Count;
        public List<string> Path = new List<string>();

        public MyClass(int count = 0)
        {
            Count = count;
        }

        public MyClass(int count, List<string> path, string add = "")
        {
            Count = count;
            Path = new List<string>(path);
            if (add != "")
                Path.Add(add);
        }

        public MyClass GetMax(MyClass item1, MyClass item2, string add1, string add2)
        {
            return item1.Count >= item2.Count ?
                        new MyClass(item1.Count, item1.Path) :
                        new MyClass(item2.Count, item2.Path);
        }
    }

    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return opt[first.Count, second.Count].Path;
        }

        private static MyClass[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var length1 = first.Count + 1;
            var length2 = second.Count + 1;
            //?
            var opt = new MyClass[length1, length2];

            //fill it
            for (var i = 0; i < length1; ++i)
                opt[i, 0] = new MyClass();

            // again
            for (var i = 1; i < length2; ++i)
                opt[0, i] = new MyClass();

            // fill inside
            for (var i = 1; i < length1; ++i)
                for (var j = 1; j < length2; ++j)
                {
                    // Math.Min(Math.Min(opt[i - 1, j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]) + 1
                    opt[i, j] = first[i - 1] == second[j - 1] ?
                                    new MyClass(opt[i - 1, j - 1].Count + 1, opt[i - 1, j - 1].Path, first[i - 1]) :
                                    new MyClass().GetMax(opt[i - 1, j], opt[i, j - 1], first[i - 1], second[j - 1]);
                    // opt[i, j] = 0;
                }

            return opt;
        }

        private static List<string> RestoreAnswer(MyClass[,] opt, List<string> first, List<string> second)
        {
            throw new NotImplementedException();
        }
    }
}