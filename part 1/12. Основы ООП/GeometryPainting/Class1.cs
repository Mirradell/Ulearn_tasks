using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;
        public Vector() { }

        public Vector(double x, double y) 
        {
            X = x;
            Y = y;
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector v2)
        {
            return new Vector(X + v2.X, Y + v2.Y);
        }

        public bool Belongs(Segment s)
        {
            return Geometry.IsVectorInSegment(this, s);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector v1)
        {
            return Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        //Добавьте метод Geometry.GetLength, вычисляющий длину сегмента,
        public static double GetLength(Segment s)
        {
            return GetLength(Add(s.End, new Vector(-1 * s.Begin.X, -1 * s.Begin.Y)));
        }

        //метод Geometry.IsVectorInSegment(Vector, Segment), проверяющий, что задаваемая вектором точка лежит в отрезке.
        public static bool IsVectorInSegment(Vector v, Segment s)
        {
            double a = s.End.Y - s.Begin.Y,
                   b = s.Begin.X - s.End.X,
                   c = s.Begin.Y * s.End.X - s.Begin.X * s.End.Y;
            if (Math.Abs(a * v.X + b * v.Y + c) == 0)
                return Dist(v, s.Begin) + Dist(v, s.End) == GetLength(s);
            return false;
        }
        private static double Dist(Vector v1, Vector v2)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }
    }

    // Создайте класс Segment, представляющий отрезок прямой. Концы его отрезков должны задаваться двумя публичными полями: Begin и End типа Vector.
    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public Segment() { }

        public Segment(Vector begin, Vector end)
        {
            Begin = begin;
            End = end;
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector v)
        {
            return Geometry.IsVectorInSegment(v, this);
        }
    }
}
