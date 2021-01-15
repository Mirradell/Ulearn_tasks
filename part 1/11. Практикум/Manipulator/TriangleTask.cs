using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0)
                return double.NaN;

            var cos = (a * a + b * b - c * c) / (2.0 * a * b);
            if (cos < -1 || cos > 1)
                return double.NaN;
                
            return Math.Acos(cos);
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(6, 8, 10, Math.PI / 2)]
        [TestCase(0, 1, 10, double.NaN)]
        [TestCase(1, 1, -1, double.NaN)]
        [TestCase(3, 10, 1, double.NaN)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var actualAngle = TriangleTask.GetABAngle(a, b, c);
            Assert.AreEqual(actualAngle, expectedAngle, 1e-5);
        }
    }
}