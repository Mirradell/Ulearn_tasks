using System;
using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Manipulation
{
    public static class ManipulatorTask
    { 
        public static double AngleToNorm(double angle)
        {
            double res = angle;
            while (res > Math.PI)
                res -= 2 * Math.PI;
            while (res < -1 * Math.PI)
                res += 2 * Math.PI;
            return res;
        }
        
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            alpha = AngleToNorm(alpha);
            // Найдите координаты сустава Wrist по координатам (x, y) и углу alpha — они понадобятся в следующих шагах.
            var wristY = Manipulator.Palm * Math.Sin(alpha);
            var wristX = Math.Sqrt(Manipulator.Palm * Manipulator.Palm - wristY * wristY);

            if (alpha <= Math.PI)
                wristY += y;
            else
                wristY = y - wristY;

            if (alpha >= Math.PI / 2.0 && alpha <= 3 * Math.PI / 2.0) //угол альфа от 90 до 270
                wristX += x;
            else
                wristX = x - wristX;

            // Найдите угол elbow с помощью метода GetABAngle в треугольнике(shoulder, elbow, wrist), в котором все три стороны теперь известны.
            var distShoulderWrist = Math.Sqrt(wristX * wristX + wristY * wristY);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, distShoulderWrist);

            // Найдите угол shoulder, как сумму двух углов:
            var shouder = TriangleTask.GetABAngle(distShoulderWrist, Manipulator.UpperArm, Manipulator.Forearm);
            shouder += Math.Atan(wristY / wristX);

            // Проще всего найти угол wrist. Сделать это можно по формуле wrist = –alpha – shoulder – elbow.
            var wrist = -1 * alpha - shouder - elbow;
            if (double.IsNaN(elbow) || double.IsNaN(shouder) || double.IsNaN(wrist))
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shouder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        [TestCase(180, 150, 0)]
        //[TestCase(-116.806269661433d, -172.850264571537d, 60.8476643789342d)]
        [TestCase(60.0f, -270.0f, 0)]
        [TestCase(60.0f, 270.0f, 0)]
        [TestCase(0, 330.0f, -1.5707963267949d)]
        [TestCase(90, 0, 0)]
        public void TestMoveManipulatorTo(double x, double y, double alpha)
        {
            var joints = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
            if (double.IsNaN(joints[0]))
                Assert.AreEqual(x, 0.0, 1e-5);
            else
            {
                var actualResult = AnglesToCoordinatesTask.GetJointPositions(joints[0], joints[1], joints[2]);
                Assert.AreEqual(x, actualResult[actualResult.Length - 1].X, 1e-5, "x");
                Assert.AreEqual(y, actualResult[actualResult.Length - 1].Y, 1e-5, "y");
            }
        }

        [Test]
        [TestCase(Math.PI, Math.PI)]
        [TestCase(3 * Math.PI, Math.PI)]
        [TestCase(-1 * Math.PI, -1 * Math.PI)]
        [TestCase(11 * Math.PI, Math.PI)]
        [TestCase(10 * Math.PI, 0)]
        [TestCase(Math.PI * 5 / 2, Math.PI / 2)]
        public void TestAngleToNorm(double angle, double expectedResult)
        {
            var actualResult = ManipulatorTask.AngleToNorm(angle);
            Assert.AreEqual(expectedResult, actualResult, 1e-5, expectedResult + " != " + actualResult);
        }
    }
}