using System;
using System.Diagnostics;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public class BotEventLoop
    {
        private readonly TextReader input;
        private readonly TextWriter output;
        private readonly IStrategy strategy;
        private readonly Game state;
        private readonly Parser parser;
        private readonly Stopwatch timer;

        public BotEventLoop(TextReader input, TextWriter output, IStrategy strategy)
        {
            this.input = input;
            this.output = output;
            this.strategy = strategy;
            this.state = new Game();
            this.parser = new Parser(state);
            this.timer = new Stopwatch();
        }

        public void Run()
        {
            var line = default(string);

            while ((line = input.ReadLine()) != null)
            {
                var timeout = parser.Command(line);
                if (timeout.HasValue)
                {                  
                    state.CurrentTimebank = timeout.Value;

                    state.Log($"begin turn, {timeout}ms available");
                    timer.Restart();

                    var move = strategy.Act(state);

                    timer.Stop();
                    state.Log($"end turn, {timer.ElapsedMilliseconds}ms used");

                    output.WriteLine(move);
                }
            }
        }
    }
}