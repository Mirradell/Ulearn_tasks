using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Recognizer
{
	public class MyComparer : IComparer<KeyValuePair<double, int>>
	{
		public int Compare(KeyValuePair<double, int> p1, KeyValuePair<double, int> p2)
		{
			//if (p2.Value.CompareTo(p1.Value) == 0)
			//	return p2.Key.CompareTo(p1.Key);
			return p2.Key.CompareTo(p1.Key);
		}
	}
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] originalImage, double whitePixelsFraction)
		{
			int rows = originalImage.GetUpperBound(0) + 1;
			int columns = originalImage.Length / rows;
			var resultImage = new double[rows, columns];

			var pixelsDictionary = new Dictionary<double, int>();
			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					if (pixelsDictionary.ContainsKey(originalImage[x, y]))
						pixelsDictionary[originalImage[x, y]]++;
					else
						pixelsDictionary.Add(originalImage[x, y], 1);//*/
					

			
			int procent = (int)(whitePixelsFraction * rows * columns), pixelsCount = 0;
			var t = 255.0;
			var pixelsList = pixelsDictionary.ToList();
			pixelsList.Sort(new MyComparer());

			while (pixelsCount < procent && pixelsList.Count > 0)
            {
				var key = pixelsList.First().Key;
				pixelsCount += pixelsList.First().Value;
				t = key;
				pixelsList.RemoveAt(0);
            }

			if (pixelsList.Count > 0 && pixelsCount < procent)
				t = (int)pixelsList.First().Key;

			if (procent == rows * columns)
				t = -1;

			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					resultImage[x, y] = originalImage[x, y] >= t ? 1.0 : 0.0;
					
			return resultImage;
		}
	}
}

/*
 Image 1x1 (threshold=0)
{123} => {0}
Image 1x1 (threshold=1)
{123} => {1}
Image 1x2 (threshold=0.5)
{1,2} => {0,1}
Image 1x4 (threshold=0.5)
{1,2,2,3} => {0,1,1,1}
Image 3x3 (threshold=0.2)
{ {0.7	0.5	0.9}, {0.9, 0.9, 0.6}, {0, 0.1, 0.1} } => { {0, 0, 0}, {1, 1, 0}, {0, 0, 0} }
//*/