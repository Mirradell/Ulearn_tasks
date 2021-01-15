using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public class HelpClass
    {
        public int Count;
        public List<string> Path = new List<string>();

        public HelpClass(int count = 0)
        {
            Count = count;
        }

        public HelpClass(int count, List<string> path, string add = "")
        {
            Count = count;
            Path = new List<string>(path);
            if (add != "")
                Path.Add(add);
        }

        public HelpClass GetMax(HelpClass item1, HelpClass item2, string add1, string add2)
        {
            return item1.Count >= item2.Count ?
                        new HelpClass(item1.Count, item1.Path) :
                        new HelpClass(item2.Count, item2.Path);
        }
    }

    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return opt[first.Count, second.Count].Path;
        }

        private static HelpClass[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var length1 = first.Count + 1;
            var length2 = second.Count + 1;

            var opt = new HelpClass[length1, length2];

            for (var i = 0; i < length1; ++i)
                opt[i, 0] = new HelpClass();

            for (var i = 1; i < length2; ++i)
                opt[0, i] = new HelpClass();

            for (var i = 1; i < length1; ++i)
                for (var j = 1; j < length2; ++j)
                {
                    opt[i, j] = first[i - 1] == second[j - 1] ?
                                    new HelpClass(opt[i - 1, j - 1].Count + 1, opt[i - 1, j - 1].Path, first[i - 1]) :
                                    new HelpClass().GetMax(opt[i - 1, j], opt[i, j - 1], first[i - 1], second[j - 1]);
                }

            return opt;
        }

        private static List<string> RestoreAnswer(HelpClass[,] opt, List<string> first, List<string> second)
        {
            throw new NotImplementedException();
        }
    }
}