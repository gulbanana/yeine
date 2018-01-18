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

            var strategies = new Func<IMoveEvaluator, IStrategy>[]
            {
                e => new Strategies.BestMove(e, 2, 2),
                e => new Strategies.BestMove(e, 3, 2),
                e => new Strategies.BestMove(e, 4, 2),
                e => new Strategies.BestMove(e, 5, 2),
            };

            var evaluators = new IMoveEvaluator[]
            {
                new Strategies.OursMinusTheirs(),
            };

            var bots = (from s in strategies from e in evaluators select s(e)).ToArray();
            var pairs = new List<(IStrategy b1, IStrategy b2)>();
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