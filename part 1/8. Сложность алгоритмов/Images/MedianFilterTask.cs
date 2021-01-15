using System.Collections.Generic;
using System;
using System.Linq;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		/* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
		public static double Mediana(int fromX, int fromY, int toX, int toY, double[,] originalImage, int rows, int columns)
        {
			List<double> medianList = new List<double>();
			for (int i = Math.Max(0, fromX); i <= Math.Min(rows - 1, toX); i++)
				for (int j = Math.Max(0, fromY); j <= Math.Min(columns - 1, toY); j++)
					medianList.Add(originalImage[i, j]);

			medianList.Sort();
			if (medianList.Count % 2 != 0)
				return medianList[(medianList.Count - 1) / 2];

			return (medianList[(medianList.Count - 1) / 2] + medianList[medianList.Count / 2]) / 2.0;
        }

		public static double[,] MedianFilter(double[,] originalImage)
		{
			int rows = originalImage.GetUpperBound(0) + 1;
			int columns = originalImage.Length / rows;
			var resultImage = new double[rows, columns];
			
			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					resultImage[x, y] = Mediana(x - 1, y - 1, x + 1, y + 1, originalImage, rows, columns);
			
			return resultImage;
		}
	}
}