using System;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public class Runner
    {
        private readonly TextReader input;
        private readonly TextWriter output;
        private readonly Strategy strategy;
        private readonly Game state;
        private readonly Parser parser;

        public Runner(TextReader input, TextWriter output, Strategy strategy)
        {
            this.input = input;
            this.output = output;
            this.strategy = strategy;
            this.state = new Game();
            this.parser = new Parser(state);
        }

        public void Run()
        {
            var line = default(string);

            while ((line = input.ReadLine()) != null)
            {
                var parts = line.Split(' ');

                switch (parts[0])
                {
                    case "settings":
                        parser.Settings(parts[1], parts[2]);
                        break;

                    case "update":
                        if (parts[1].Equals("game"))
                        {
                            parser.UpdateGame(parts[2], parts[3]);
                        }
                        else
                        {
                            parser.UpdatePlayer(parts[1], parts[2], parts[3]);
                        }
                        break;

                    case "action":
                        if (parts[1].Equals("move"))
                        {
                            var move = strategy.DoMove(state);
                            output.WriteLine(move?.ToString() ?? MoveType.Pass.ToString());
                        }
                        break;

                    default:
                        Console.Error.WriteLine("Unknown command");
                        break;
                }
            }
        }
    }
}