using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var previousDataPoint = double.NaN;
			foreach(var d in data)
            {
				var dataPoint = new DataPoint();
				dataPoint.OriginalY = d.OriginalY;
				dataPoint.X = d.X;

				if (double.IsNaN(previousDataPoint))
				{
					d.ExpSmoothedY = d.OriginalY;
					previousDataPoint = d.OriginalY;
					yield return d;
				}
				else
				{
					dataPoint.ExpSmoothedY = alpha * dataPoint.OriginalY + (1 - alpha) * previousDataPoint;
					previousDataPoint = dataPoint.ExpSmoothedY;
					yield return dataPoint;
				}
			}
		}
	}
}