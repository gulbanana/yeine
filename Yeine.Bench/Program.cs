using BenchmarkDotNet.Running;

namespace Yeine.Bench
{
    class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TurnCost>();
        }
    }
}