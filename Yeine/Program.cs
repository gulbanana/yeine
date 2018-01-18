using System;
using System.IO;

namespace Yeine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var wideIn = new StreamReader(Console.OpenStandardInput(512)))
            {
                var eventLoop = new BotEventLoop(
                    new StreamReader(Console.OpenStandardInput(512)), 
                    Console.Out, 
                    new Strategies.BestMove(2, 2),
                    new Evaluators.RecogniseEnd(new Evaluators.OursMinusTheirs()));
                    
                eventLoop.Run();
            }
        }
    }
}