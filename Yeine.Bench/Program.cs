using BenchmarkDotNet.Running;

namespace Yeine.Bench
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ParameterComparison>();
            BenchmarkRunner.Run<TurnComponents>();
            BenchmarkRunner.Run<EvaluationOptions>();
        }
    }
}