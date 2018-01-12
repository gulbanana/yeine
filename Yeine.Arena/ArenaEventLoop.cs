using System;
using System.Diagnostics;
using System.IO;
using Yeine.API;
using Yeine.State;

namespace Yeine.Arena
{
    public class ArenaEventLoop
    {
        private readonly Stopwatch stopwatch;
        private readonly IStrategy p0;
        private readonly IStrategy p1;
        private readonly bool verbose;

        public ArenaEventLoop(IStrategy player0, IStrategy player1, bool verbose)
        {
            p0 = player0;
            p1 = player1;
            this.verbose = verbose;
            stopwatch = new Stopwatch();
        }

        public void Run()
        {
            stopwatch.Restart();

            var games = CreateGamePair();
            
            Write($"==== {p0} vs {p1} ====");
            PlayGame(p0, games.s0a, p1, games.s1a);
            
            Write($"==== {p1} vs {p0} (mirror match) ====");
            PlayGame(p1, games.s0b, p0, games.s1b);
        }

        private void PlayGame(IStrategy p0, Game s0, IStrategy p1, Game s1)
        {
            var round = 1;
            var field = s0.Field.Clone();
            field.CalculateLivingCells('0', '1', out var c0, out var c1);

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

        // play from both sides to eliminate the first-turn factor
        private (Game s0a, Game s1a, Game s0b, Game s1b) CreateGamePair()
        {
            var s0a = new Game
            {
                OurName = "player0",
                OurID = '0',
                TheirID = '1'
            };

            var s0b = new Game
            {
                OurName = "player0",
                OurID = '0',
                TheirID = '1'
            };

            var s1a = new Game
            {
                 OurName = "player1",
                 OurID = '1',
                 TheirID = '0'
            };

            var s1b = new Game
            {
                 OurName = "player1",
                 OurID = '1',
                 TheirID = '0'
            };

            // XXX this should be random - it's a specific game picked from the leaderboard
            var startingCells = ".,.,.,.,0,.,0,.,0,.,0,0,0,.,.,.,0,.,.,0,.,0,.,.,.,.,.,.,.,.,0,.,0,.,.,.,.,.,.,0,.,0,.,0,0,0,0,.,.,0,0,0,.,.,.,.,.,.,.,0,.,.,0,.,.,.,0,.,.,.,.,0,.,.,.,0,.,.,.,.,.,.,0,.,.,0,.,.,.,0,.,.,0,.,0,.,0,.,.,.,0,.,.,.,.,0,.,.,.,.,0,.,.,0,.,.,.,0,.,.,.,.,.,0,.,.,.,.,.,.,.,.,.,0,.,.,.,.,.,.,.,0,0,.,.,1,1,.,.,.,.,.,.,.,1,.,.,.,.,.,.,.,.,.,1,.,.,.,.,.,1,.,.,.,1,.,.,1,.,.,.,.,1,.,.,.,.,1,.,.,.,1,.,1,.,1,.,.,1,.,.,.,1,.,.,1,.,.,.,.,.,.,1,.,.,.,1,.,.,.,.,1,.,.,.,1,.,.,1,.,.,.,.,.,.,.,1,1,1,.,.,1,1,1,1,.,1,.,1,.,.,.,.,.,.,1,.,1,.,.,.,.,.,.,.,.,1,.,1,.,.,1,.,.,.,1,1,1,.,1,.,1,.,1,.,.,.,.";

            s0a.ParseField(18, 16, startingCells);
            s0b.ParseField(18, 16, startingCells);
            s1a.ParseField(18, 16, startingCells);
            s1b.ParseField(18, 16, startingCells);

            return (s0a, s1a, s0b, s1b);
        }

        private void Write(string line)
        {
            Console.WriteLine($"@{stopwatch.ElapsedMilliseconds, 4}ms | {line}");
        }
    }
}