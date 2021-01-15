using System;
using System.Drawing;
using Microsoft.Win32;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        private static PointF EndPoint(double angle, double length)
        {
            return new PointF((float)(length * Math.Cos(angle)), (float)(length * Math.Sin(angle)));
        }

        private static PointF SumPoint(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        private static PointF MinPoint(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }
        private static PointF ToNewCoordinate(PointF point, double angle)
        {
            return new PointF((float)(point.X * Math.Cos(angle) + point.Y * Math.Sin(angle)),
                              (float)(point.Y * Math.Cos(angle) - point.X * Math.Sin(angle)));
        }

        private static PointF ToOldCoordinate(PointF point, double angle)
        {
            return new PointF((float)(point.X * Math.Cos(angle) - point.Y * Math.Sin(angle)),
                              (float)(point.X * Math.Sin(angle) + point.Y * Math.Cos(angle)));
        }

        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var upperArm = EndPoint(shoulder, Manipulator.UpperArm);
            var foreArm  = EndPoint(elbow, Manipulator.Forearm);
            var palmArm  = EndPoint(wrist, Manipulator.Palm);

            palmArm = SumPoint(foreArm, ToOldCoordinate(palmArm, Math.PI + elbow));
            foreArm = SumPoint(upperArm, ToOldCoordinate(foreArm, Math.PI + shoulder));
            palmArm = SumPoint(upperArm, ToOldCoordinate(palmArm, Math.PI + shoulder));

            return new PointF[]
            {
                upperArm,
                foreArm,
                palmArm
            };
        }

        public static double Dist(PointF p1, PointF p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [Test]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI * 2, Math.PI,     Math.PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(0,           Math.PI,     Math.PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(0, 0, 0, 90.0, 0)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}