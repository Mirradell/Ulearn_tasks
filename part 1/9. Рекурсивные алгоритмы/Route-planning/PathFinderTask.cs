using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		static List<int> result = new List<int>();
		static double min = -1;

		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			result = new List<int>();
			result.Add(0);
			min = double.MaxValue;
			MakeTrivialPermutation(checkpoints, new List<int>() { 0 }, 1, 0);
			return result.ToArray();
		}

		private static void MakeTrivialPermutation(Point[] checkpoints, List<int> permutation, int position, double length)
		{
			if (position == checkpoints.Length && length < min)
			{
				min = length;
				var temp = new List<int>(permutation);
				result = temp;
				permutation = new List<int>();
			}
			else
				for (int i = 0; i < checkpoints.Length; i++)
                {
					var index = permutation.IndexOf(i);
					if (index == -1)
                    {
						Point last = new Point();
						if (permutation.Count > 0)
							last = checkpoints[permutation.Last()];

						if (permutation.Count > position)
							permutation[position] = i;
						else
							permutation.Add(i);
							
						if (last != null && length + PointExtensions.DistanceTo(last, checkpoints[i]) < min)
							MakeTrivialPermutation(checkpoints, permutation, position + 1, length + PointExtensions.DistanceTo(last, checkpoints[i]));
						
						permutation.Remove(permutation.Last());
                    }
                }
		}
	}
}