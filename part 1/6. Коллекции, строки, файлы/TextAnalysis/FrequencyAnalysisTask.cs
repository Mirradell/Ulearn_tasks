using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var frequency = new SortedDictionary<string, int>();
            foreach (var sentence in text)
            {
                for (int i = 1; i < sentence.Count; i++) // 2gramma
                {
                    var gramma = sentence[i - 1] + " " + sentence[i];
                    if (frequency.ContainsKey(gramma))
                        frequency[gramma]++;
                    else
                        frequency.Add(gramma, 1);
                }

                for (int i = 2; i < sentence.Count; i++) // 3gramma
                {
                    var gramma = sentence[i - 2] + " " + sentence[i - 1] + " " + sentence[i];
                    if (frequency.ContainsKey(gramma))
                        frequency[gramma]++;
                    else
                        frequency.Add(gramma, 1);
                }
            }


            var result = new Dictionary<string, string>();
            foreach (var gramma in frequency)
            {
                var str = gramma.Key.Split(' ');
                var begin = str[0];
                if (str.Length > 2)
                    begin += " " + str[1];
                var end = str[str.Length - 1];

                if (!result.ContainsKey(begin))
                    result.Add(begin, end);
                else
                {
                    var newGramma = begin + " " + result[begin];
                    if (frequency[newGramma] < gramma.Value ||
                        frequency[newGramma] == gramma.Value && string.CompareOrdinal(end, result[begin]) < 0)
                        result[begin] = end;
                }
            }
            return result;
        }
    }
}