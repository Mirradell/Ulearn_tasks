using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZedGraph;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            var size = 16;
            while (size < 513)
            {
                classesTimes.Add(new ExperimentResult(size, benchmark.MeasureDurationInMs(new ClassArrayCreationTask(size), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult(size, benchmark.MeasureDurationInMs(new StructArrayCreationTask(size), repetitionsCount)));
                size *= 2;
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            var size = 16;
            while (size < 513)
            {
                classesTimes.Add(new ExperimentResult(size, benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(size), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult(size, benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(size), repetitionsCount)));
                size *= 2;
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}