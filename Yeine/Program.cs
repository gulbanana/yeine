using System;
using System.IO;
using Yeine.Strategies;

namespace Yeine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var wideIn = new StreamReader(Console.OpenStandardInput(512)))
            {
                var eventLoop = new BotEventLoop(new StreamReader(Console.OpenStandardInput(512)), Console.Out, new Strategies.BestMove(5, canBirth: true));
                eventLoop.Run();
            }
        }
    }
}