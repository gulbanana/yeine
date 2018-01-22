using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private int p0Wins = 0;
        private int p1Wins = 0;
        private int draws = 0;

        public ArenaEventLoop(int verbosity, IStrategy p0, IStrategy p1)
        {
            this.verbosity = verbosity;
            this.p0 = p0;
            this.p1 = p1;
            random = new Random();
        }

        public (int w, int l, int d) PlayGames(int games)
        {
            var pairs = games / 2;

            Parallel.For(0, pairs, _ => PlayPair());

            return (p0Wins, p1Wins, draws);
        }

        private void PlayPair()
        {
            var startingField = CreateRandomField(18, 16);
                
            var match1 = new Match(verbosity, startingField.Clone(), p0, p1);
            switch (match1.PlayGame())
            {
                case GameResult.Player0Win: Interlocked.Increment(ref p0Wins); break;
                case GameResult.Player1Win: Interlocked.Increment(ref p1Wins); break;
                case GameResult.Draw: Interlocked.Increment(ref draws); break;
            }
            
            var match2 = new Match(verbosity, startingField.Clone(), p1, p0);
            switch (match2.PlayGame())
            {
                case GameResult.Player0Win: Interlocked.Increment(ref p1Wins); break;
                case GameResult.Player1Win: Interlocked.Increment(ref p0Wins); break;
                case GameResult.Draw: Interlocked.Increment(ref draws); break;
            }
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

            return new Field(18, 16, cells, new byte[2, 18, 16]);
        }
    }
}