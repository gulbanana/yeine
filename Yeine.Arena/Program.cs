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

            var eventLoop = new ArenaEventLoop(new Strategies.AlwaysPass(), new Strategies.KillBest(), isVerbose);
            eventLoop.Run(int.Parse(count));
        }
    }
}