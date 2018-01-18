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
        private readonly IStrategy p0;
        private readonly IStrategy p1;
        private readonly Random random;

        public ArenaEventLoop(int verbosity, IStrategy p0, IStrategy p1)
        {
            this.verbosity = verbosity;
            this.p0 = p0;
            this.p1 = p1;
            random = new Random();
        }

        public void Run(int games)
        {
            int p0Wins = 0;
            int p1Wins = 0;
            int draws = 0;

            for (var i = 0; i < games; i++)
            {
                var startingField = CreateRandomField(18, 16);
                
                var match1 = new Match(verbosity, startingField.Clone(), p0, p1);
                switch (match1.PlayGame())
                {
                    case GameResult.Player0Win: p0Wins++; break;
                    case GameResult.Player1Win: p1Wins++; break;
                    case GameResult.Draw: draws++; break;
                }
                
                var match2 = new Match(verbosity, startingField.Clone(), p1, p0);
                switch (match2.PlayGame())
                {
                    case GameResult.Player0Win: p1Wins++; break;
                    case GameResult.Player1Win: p0Wins++; break;
                    case GameResult.Draw: draws++; break;
                }
            }

            var played = games*2;
            Console.WriteLine($"{p0} vs {p1}, {played} games: {p0Wins}W / {p1Wins}L / {draws}D ({p0Wins*100/played}% / {p1Wins*100/played}% / {draws*100/played}%).");
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
    }
}