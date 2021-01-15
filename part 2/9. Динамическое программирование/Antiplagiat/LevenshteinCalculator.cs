using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public static double LevenshteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var length1 = first.Count + 1;
            var length2 = second.Count + 1;

            var opt = new double[length1, length2];

            for (var i = 1; i < length1; ++i)
                opt[i, 0] = i;

            for (var i = 1; i < length2; ++i)
                opt[0, i] = i;

            for (var i = 1; i < length1; ++i)
                for (var j = 1; j < length2; ++j)
                {
                    opt[i, j] = first[i - 1] == second[j - 1] ? 
                                    opt[i - 1, j - 1] : 
                                    Math.Min(Math.Min(opt[i - 1, j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]),
                                                      opt[i - 1, j] + 1), 
                                                      opt[i, j - 1] + 1);
                }

            return opt[length1 - 1, length2 - 1];
        }


        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var res = new List<ComparisonResult>();

            for (int i = 0; i < documents.Count - 1; i++)
                for (int j = i + 1; j < documents.Count; j++)
                {
                    double distance = LevenshteinDistance(documents[i], documents[j]);
                    res.Add(new ComparisonResult(documents[i], documents[j], distance));
                }

            return res;
        }
    }
}