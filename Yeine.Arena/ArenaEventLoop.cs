using System;
using System.Diagnostics;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine.Arena
{
    public class ArenaEventLoop
    {        
        private readonly IStrategy p0;
        private readonly IStrategy p1;
        private readonly bool verbose;
        
        private readonly Random random;
        private readonly Stopwatch stopwatch;

        public ArenaEventLoop(IStrategy player0, IStrategy player1, bool verbose)
        {
            p0 = player0;
            p1 = player1;
            this.verbose = verbose;

            random = new Random();
            stopwatch = new Stopwatch();
        }

        public void Run(int games)
        {
            stopwatch.Restart();

            for (var i = 0; i < games; i++)
            {
                var startingField = CreateRandomField(18, 16);
                
                Write($"==== {p0} vs {p1} ====");
                PlayGame(startingField.Clone(), p0, p1);
                
                Write($"==== {p1} vs {p0} (mirror match) ====");
                PlayGame(startingField.Clone(), p1, p0);
            }

            stopwatch.Stop();
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
                    i++;
                }
            }

            for (var i = 0; i < 40; i++)
            {
                var x = random.Next(0, w-1);
                var y = random.Next(0, (h/2)-1) + h/2;

                if (cells[x,y] == '.')
                {
                    cells[x,y] = '1';
                    i++;
                }
            }

            return new Field(18, 16, cells);
        }

        private void PlayGame(Field field, IStrategy p0, IStrategy p1)
        {
            field.CalculateLivingCells('0', '1', out var c0, out var c1);

            if (verbose) Write(field.ToString());

            var s0 = new Game
            {
                OurName = "player0",
                OurID = '0',
                TheirID = '1'
            };

            var s1 = new Game
            {
                 OurName = "player1",
                 OurID = '1',
                 TheirID = '0'
            };

            var round = 1;
            while (round <= 100)
            {
                s0.ParseField(field.Width, field.Height, field.ToString());
                var m0 = p0.Act(s0);
                field.ProcessCommand(m0);
                field.UpdatePosition();
                var v0 = field.CalculatePositionValue('0', '1');
                if (verbose) Write($"Round {round}, player0 {m0}, {(v0>0 ? "+" : "")}{v0}");

                field.CalculateLivingCells('0', '1', out c0, out c1);
                if (!(c0 > 0 && c1 > 0))
                {
                    break;
                }

                s1.ParseField(field.Width, field.Height, field.ToString());
                var m1 = p1.Act(s1);
                field.ProcessCommand(m1);
                field.UpdatePosition();
                var v1 = field.CalculatePositionValue('0', '1');
                if (verbose) Write($"Round {round}, player1 {m1}, {(v1>0 ? "+" : "")}{v1}");

                field.CalculateLivingCells('0', '1', out c0, out c1);
                if (!(c0 > 0 && c1 > 0))
                {
                    break;
                }

                round++;
            }

            var result = (round > 100) ? "DRAW" : 
                         (c0 > c1) ? p0.ToString() + " WIN" : 
                         p1.ToString() + " WIN";

            Write($"{result} in {round-1} rounds - player0 {c0}, player1 {c1}");
        }

        private void Write(string line)
        {
            Console.WriteLine($"@{stopwatch.ElapsedMilliseconds, 4}ms | {line}");
        }
    }
}