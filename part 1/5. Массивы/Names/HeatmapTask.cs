using System;
using ZedGraph;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            double[,] res = new double[31, 12];

            foreach (var name in names)
                if (name.BirthDate.Day != 1)
                    res[name.BirthDate.Day - 1, name.BirthDate.Month - 1]++;
            
            string[] xAxis = new string[30];

            for (int i = 1; i < xAxis.Length; i++)
                xAxis[i] = (i + 1).ToString();
            
            string[] yAxis = new string[12];
            
            for (int i = 0; i < yAxis.Length; i++)
                yAxis[i] = (i + 1).ToString();

            return new HeatmapData("Тепловая карта имен", res, xAxis, yAxis);
        }
    }
}