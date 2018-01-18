using System;
using System.IO;
using System.Linq;
using Yeine.Strategies;

namespace Yeine.Arena
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = args.SingleOrDefault(a => int.TryParse(a, out _)) ?? "1";
            var isVerbose = args.Any(a => a == "-v" || a == "--verbose");
            var isVeryVerbose = args.Any(a => a == "-vv" || a == "--veryVerbose");

            var strategies = new IStrategy[]
            {
                new Strategies.BestMove(3),
                new Strategies.BestMove(5)
            };

            var evaluators = new IEvaluator[]
            {
                new Evaluators.OursMinusTheirs()
            };

            for (var i = 0; i < strategies.Length; i++)
            {
                for (var j = 0; j < strategies.Length; j++)
                {
                    var strat1 = strategies[i];
                    var strat2 = strategies[j];
                    foreach (var eval1 in evaluators)
                    {
                        foreach (var eval2 in evaluators)
                        {
                            var eventLoop = new ArenaEventLoop(
                                isVeryVerbose ? 2 : isVerbose ? 1 : 0,
                                new Bot(strat1, eval1),
                                new Bot(strat2, eval2));

                            eventLoop.Run(int.Parse(count));
                        }
                    }
                }
            }
        }
    }
}