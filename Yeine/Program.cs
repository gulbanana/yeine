using System;
using System.IO;
using Yeine.Strategies;

namespace Yeine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(Console.OpenStandardInput(512)))
            {
                Runner game = new Runner(reader, Console.Out, new Strategies.AlwaysPass());
                game.Run();
            }
        }
    }
}