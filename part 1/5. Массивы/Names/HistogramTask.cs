using System;
using System.Linq;
using System.Security.Cryptography;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            double[] hist = new double[31];

            foreach (var name in names)
                if (name.Name == name)
                    hist[name.BirthDate.Day - 1]++;

            string[] xAxis = new string[31];
            
            for (int i = 0; i < xAxis.Length; i++)
                xAxis[i] = (i + 1).ToString();

            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), xAxis, hist);
        }
    }
}