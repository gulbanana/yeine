using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Arena
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = args.SingleOrDefault(a => int.TryParse(a, out _)) ?? "0";
            var isVerbose = args.Any(a => a == "-v" || a == "--verbose");
            var isVeryVerbose = args.Any(a => a == "-vv" || a == "--veryVerbose");

            var bots = new IStrategy[]
            {
                new Strategies.BestMove(4, 5, 0.000, 0),
                new Strategies.BestMove(4, 5, 0.005, 10),
            };

            var eventLoop = new ArenaEventLoop(new MTArena(), bots, isVeryVerbose ? 2 : isVerbose ? 1 : 0);
            eventLoop.Run(int.Parse(count));
        }
    }
}