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
                Runner game = new Runner(wideIn, Console.Out, new Strategies.AlwaysPass());
                game.Run();
            }
        }
    }
}