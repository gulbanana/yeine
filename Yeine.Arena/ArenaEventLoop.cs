using System;
using System.Diagnostics;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine.Arena
{
    class ArenaEventLoop
    {        
        private readonly int verbosity;
        private readonly Bot p0;
        private readonly Bot p1;

        private readonly Random random;
        private readonly Stopwatch stopwatch;

        public ArenaEventLoop(int verbosity, Bot p0, Bot p1)
        {
            this.verbosity = verbosity;
            this.p0 = p0;
            this.p1 = p1;

            random = new Random();
            stopwatch = new Stopwatch();
        }

        public void Run(int games)
        {
            stopwatch.Restart();

            int p0Wins = 0;
            int p1Wins = 0;
            int draws = 0;

            for (var i = 0; i < games; i++)
            {
                var startingField = CreateRandomField(18, 16);
                
                if (verbosity >= 1) Write($"==== {p0} vs {p1} ====");
                switch (PlayGame(startingField.Clone(), p0, p1))
                {
                    case Result.Player0Win: p0Wins++; break;
                    case Result.Player1Win: p1Wins++; break;
                    case Result.Draw: draws++; break;
                }
                
                if (verbosity >= 1) Write($"==== {p1} vs {p0} (mirror match) ====");
                switch (PlayGame(startingField.Clone(), p1, p0))
                {
                    case Result.Player0Win: p1Wins++; break;
                    case Result.Player1Win: p0Wins++; break;
                    case Result.Draw: draws++; break;
                }
            }

            stopwatch.Stop();

            var played = games*2;
            Console.WriteLine($"{played} games played between '{p0}' and '{p1}'. Win/Lose/Draw {p0Wins}/{p1Wins}/{draws} ({p0Wins*100/played}%/{p1Wins*100/played}%/{draws*100/played}%).");
        }

        private Field CreateRandomField(int w, int h)
        {
            var cells = new char[w,h];
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    cells[x,y] = '.';
                }
            }

            for (var i = 0; i < 40;)
            {
                var x = random.Next(0, w-1);
                var y = random.Next(0, (h/2)-1);

                if (cells[x,y] == '.')
                {
                    cells[x,y] = '0';
                    cells[x,h-1-y] = '1';
                    i++;
                }
            }

            return new Field(18, 16, cells);
        }

        private Result PlayGame(Field field, Bot p0, Bot p1)
        {
            if (verbosity >= 2) Write(field.ToString());
            field.EvaluateLivingCells('0', '1', out var c0, out var c1);

            var s0 = new Game { OurName = "player0", OurID = '0', TheirID = '1' };
            var s1 = new Game { OurName = "player1", OurID = '1', TheirID = '0' };
            var round = 1;

            bool PlayTurn(IStrategy strat, IEvaluator eval, Game state)
            {
                state.ParseField(field.Width, field.Height, field.ToString());
                var m = strat.Act(state, eval);
                field.ProcessCommand(m, state.OurID);
                field.UpdatePosition();
                var v = eval.EvaluatePosition(state, field);
                if (verbosity >= 2) Write($"Round {round}, player0 {m}, {(v>0 ? "+" : "")}{v}");

                field.EvaluateLivingCells('0', '1', out c0, out c1);
                return c0 == 0 || c1 ==  0;
            }

            while (round <= 100)
            {
                if (PlayTurn(p0.Strategy, p0.Evaluator, s0)) break;
                if (PlayTurn(p1.Strategy, p1.Evaluator, s1)) break;
                round++;
            }

            var isDraw = round > 100;
            
            if (verbosity >= 1)
            {
                var result = isDraw ? "DRAW" : (c0 > c1 ? p0 : p1).ToString() + " WIN";
                Write($"{result} in {round-1} rounds - player0 {c0}, player1 {c1}");
            }

            return isDraw ? Result.Draw : c0 > c1 ? Result.Player0Win : Result.Player1Win;
        }

        private void Write(string line)
        {
            Console.WriteLine($"@{stopwatch.ElapsedMilliseconds, 4}ms | {line}");
        }
    }
}