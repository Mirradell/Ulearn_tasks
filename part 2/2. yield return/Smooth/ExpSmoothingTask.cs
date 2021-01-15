using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var prev = double.NaN;
			foreach(var d in data)
            {
				var dp = new DataPoint();
				dp.OriginalY = d.OriginalY;
				dp.X = d.X;

				if (double.IsNaN(prev))
				{
					d.ExpSmoothedY = d.OriginalY;
					prev = d.OriginalY;
					yield return d;
				}
				else
				{
					dp.ExpSmoothedY = alpha * dp.OriginalY + (1 - alpha) * prev;
					prev = dp.ExpSmoothedY;
					yield return dp;
				}
			}
		}
	}
}