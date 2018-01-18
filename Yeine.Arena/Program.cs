using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                new Strategies.BestMove(2, 2),
                new Strategies.BestMove(3, 5),
            };

            var evaluators = new IEvaluator[]
            {
                new Evaluators.OursMinusTheirs()
            };

            var bots = (from s in strategies from e in evaluators select new Bot(s, e)).ToArray();
            var pairs = new List<(Bot b1, Bot b2)>();
            for (var b1 = 0; b1 < bots.Length-1; b1++)
            {
                for (var b2 = b1+1; b2 < bots.Length; b2++)
                {
                    pairs.Add((bots[b1], bots[b2]));
                }
            }

            Parallel.ForEach(pairs, pair =>
            {
                var eventLoop = new ArenaEventLoop(isVeryVerbose ? 2 : isVerbose ? 1 : 0, pair.b1, pair.b2);
                eventLoop.Run(int.Parse(count));
            });
        }
    }
}