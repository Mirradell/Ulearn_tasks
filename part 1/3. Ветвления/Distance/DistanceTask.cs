using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
        //расстояние между двумя точками
        private static double Dist(double ax, double ay, double bx, double by)
        {
            return Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));
        }
        //расстояние от точки до  прямой
        private static double Dist(double a, double b, double c, double x, double y)
        {
            return Math.Abs(a * x + b * y + c) / Math.Sqrt(a * a + b * b);
        }

		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
            if (ax == bx && ay == by && ax == x && ay == y)
                return 0;

            double dist    = Dist(by - ay, ax - bx, ay * bx - ax * by, x, y),
                   dist_a  = Dist(ax, ay, x, y), 
                   dist_b  = Dist(bx, by, x, y), 
                   dist_ab = Dist(ax, ay, bx, by);

                        
            if (dist < 0.00001) // лежит на прямой
                if (Math.Abs(dist_ab - dist_a - dist_b) < 0.00001)
                    return dist;
                else
                    return Math.Min(dist_a, dist_b);

            if (Math.Abs(Math.Sqrt(dist_a * dist_a - dist * dist) + Math.Sqrt(dist_b * dist_b - dist * dist) - dist_ab) < 0.00001)
                return dist;

            return Math.Min(dist_a, dist_b);
        }
	}
}