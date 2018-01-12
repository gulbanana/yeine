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
            var eventLoop = new ArenaEventLoop(new Strategies.AlwaysPass(), new Strategies.KillBest(), verbose: args.Any(a => a == "-v" || a == "--verbose"));
            eventLoop.Run();
        }
    }
}