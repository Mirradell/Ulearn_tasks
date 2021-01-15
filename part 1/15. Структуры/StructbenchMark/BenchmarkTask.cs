using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.

            var time = new Stopwatch();
            int count = repetitionCount;
            task.Run();
            
            while (count > 0)
            {
                time.Start();
                task.Run();
                time.Stop();
                count--;
            }

            return time.ElapsedMilliseconds / (double)repetitionCount;
		}
	}

    public class CreateString : ITask
    {
        public void Run()
        {
            new string('a', 1000);
        }
    }

    public class CreateBuilder : ITask
    {
        public void Run()
        {
            var s = new StringBuilder();
            for (int i = 0; i < 1000; i++)
                s.Append('a');
            s.ToString();
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var allTime = new Benchmark();
            int repeat = 1000;

            var stringTime  = allTime.MeasureDurationInMs(new CreateString(),  repeat);
            var builderTime = allTime.MeasureDurationInMs(new CreateBuilder(), repeat);
            Assert.Less(stringTime, builderTime);
        }
    }
}