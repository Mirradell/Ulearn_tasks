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
            //?
            var opt = new double[length1, length2];

            //fill it
            for (var i = 1; i < length1; ++i)
                opt[i, 0] = i;

            // again
            for (var i = 1; i < length2; ++i)
                opt[0, i] = i;

            // fill inside
            for (var i = 1; i < length1; ++i)
                for (var j = 1; j < length2; ++j)
                {
                    // Math.Min(Math.Min(opt[i - 1, j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]) + 1
                    opt[i, j] = first[i - 1] == second[j - 1] ? 
                                    opt[i - 1, j - 1] : 
                                    Math.Min(Math.Min(opt[i - 1, j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]),
                                                      opt[i - 1, j] + 1), 
                                                      opt[i, j - 1] + 1);
                    // opt[i, j] = 0;
                }

            return opt[length1 - 1, length2 - 1];
        }


        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var res = new List<ComparisonResult>();

            // res.Add();
            for (int i = 0; i < documents.Count - 1; i++)
                for (int j = i + 1; j < documents.Count; j++)
                {
                    // TODO &!&&!&
                    double distance = LevenshteinDistance(documents[i], documents[j]);
                    //distance += Math.Abs(documents[i].Count - documents[j].Count);
                    res.Add(new ComparisonResult(documents[i], documents[j], distance));
                }

            //res.Add(new ComparisonResult(documents[0], documents[1], TokenDistanceCalculator.GetTokenDistance(documents[0][0], documents[1][0])));
            return res;
        }
    }
}