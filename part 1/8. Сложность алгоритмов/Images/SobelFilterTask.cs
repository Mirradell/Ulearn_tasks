using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] Transponent(double[,] originalImage)
        {
            var width = originalImage.GetLength(0);
            var height = originalImage.GetLength(1);
            var resultImage = new double[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    resultImage[x, y] = originalImage[y, x];

            return resultImage;
        }

        public static double Sobel(int x, int y, double[,] originalImage, int rows, int columns, double[,] sx, double[,] sy, int step)
        {
            double resX = 0.0;
            double resY = 0.0;
            int width = sx.GetUpperBound(0) + 1;
            int height = sx.Length / width;
            
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    resX += sx[i, j] * originalImage[x + i - step, y + j - step];
                    resY += sy[i, j] * originalImage[x + i - step, y + j - step];
                }
            return Math.Sqrt(resX * resX + resY * resY);
        }

        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            int width = g.GetUpperBound(0) + 1;
            int height = g.Length / width;

            int widthSX = (sx.GetUpperBound(0) + 1) / 2;

            var resultImage = new double[width, height];
            var sy = Transponent(sx);
            if (width == 1 && height == 1)
                resultImage[0, 0] = Math.Abs(sx[0, 0] * g[0, 0] * Math.Sqrt(2));
            else
                for (int x = widthSX; x < width - widthSX; x++)
                    for (int y = widthSX; y < height - widthSX; y++)
                    {
                        resultImage[x, y] = Sobel(x, y, g, width, height, sx, sy, widthSX);
                    }
            return resultImage;
        }
    }
}