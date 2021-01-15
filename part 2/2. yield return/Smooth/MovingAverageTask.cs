using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			int counter = 0;
			var queue = new Queue<double>();
			var sum = 0.0;
			foreach(var d in data)
            {
				var res = new DataPoint();
				res.X = d.X;
				res.OriginalY = d.OriginalY;
				queue.Enqueue(d.OriginalY);
				sum += d.OriginalY;

				if (counter < windowWidth)
					counter++;
				else
				{
					sum -= queue.Peek();
					queue.Dequeue();
				}

				res.AvgSmoothedY = sum / counter;
				yield return res;
            }
		}
	}
}