using BenchmarkDotNet.Running;

namespace Yeine.Bench
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<TurnComponents>();
            BenchmarkRunner.Run<ParameterComparison>();
        }
    }
}