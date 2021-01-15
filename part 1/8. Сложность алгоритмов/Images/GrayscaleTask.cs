using System;

namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double[,] ToGrayscale(Pixel[,] originalImage)
		{
			int rows = originalImage.GetUpperBound(0) + 1;
			int columns = originalImage.Length / rows;
			var resultImage = new double[rows, columns];

			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					resultImage[x, y] = Math.Min(255, (0.299 * originalImage[x, y].R + 0.587 * originalImage[x, y].G + 0.114 * originalImage[x, y].B) / 255.0);

			return resultImage;
		}
	}
}