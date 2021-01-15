using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			if (visits.Count() == 0)
				return 0.0;

			var bigrams = visits.OrderBy(x => x.UserId)
								.ThenBy(x => x.DateTime)
								.Bigrams()
								.Where(x => x.Item1.SlideType == slideType
										&&  x.Item1.UserId    == x.Item2.UserId
										&&  x.Item1.SlideId   != x.Item2.SlideId)
								.Select(x => (x.Item2.DateTime - x.Item1.DateTime).TotalMinutes)
								.Where(x => x >= 1 && x <= 120);

			if (bigrams.Count() == 0)//*/
				return 0.0;

			return bigrams.Median();
		}
	}
}