using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace yield
{

	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			//var window = new LinkedList<Tuple<int, DataPoint>>();
			var window = new List<Tuple<int, DataPoint>>();
			int counter = 1;
			foreach (var d in data)
			{
				var res = new DataPoint();
				res.OriginalY = d.OriginalY;
				res.X = d.X;

				if (window.Count > 0 && counter - window.First().Item1 >= windowWidth)
					window.RemoveAt(0);

				if (window.Count > 0 && window[window.Count - 1].Item2.OriginalY < d.OriginalY)
					window.RemoveAll(x => x.Item2.OriginalY < d.OriginalY);
/*				while (window.Count > 0 && window.Last().Item2.OriginalY < d.OriginalY)
				window.RemoveLast();

				if (window.Count > 0 && counter - window.First().Item1 >= windowWidth)
					window.RemoveFirst();
				//*/

				window.Add(new Tuple<int, DataPoint>(counter, d));

				res.MaxY = window.First().Item2.OriginalY;
				counter++;
				yield return res;
			}
		}
	}
}