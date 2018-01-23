using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yeine.State;

namespace Yeine.Arena
{
    class ArenaEventLoop
    {
        private readonly IArena arena;
        private readonly int verbosity;
        private readonly IStrategy[] bots;

        public ArenaEventLoop(IArena arena, IStrategy[] bots, int verbosity)
        {
            this.arena = arena;
            this.bots = bots;
            this.verbosity = verbosity;
        }

        public void Run(int plannedGames)
        {
            // all vs all
            var pairs = new List<(int b1,int b2)>();
            for (var b1 = 0; b1 < bots.Length-1; b1++)
            {
                for (var b2 = b1+1; b2 < bots.Length; b2++)
                {
                    pairs.Add((b1, b2));
                }
            }

            var results = new (int w, int l, int d, int p)[bots.Length,bots.Length];

            // XXX doesn't seem to work in mingw
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                plannedGames = 2;
            };

            for (var i = 0; plannedGames == 0 || i < plannedGames; i += (plannedGames == 0 ? 0 : 2))
            {
                var startingField = Field.Random(18, 16);
                foreach (var pair in pairs)
                {
                    var p0 = bots[pair.b1];
                    var p1 = bots[pair.b2];

                    Action<MatchResult> OnCompleted(MatchResult preferred) => (MatchResult r) =>
                    {
                        results[pair.b1, pair.b2].p++;

                        if (r == MatchResult.Draw) 
                        { 
                            results[pair.b1, pair.b2].d++; 
                            results[pair.b2, pair.b1].d++; 
                        }
                        else if (r == preferred) 
                        { 
                            results[pair.b1, pair.b2].w++; 
                            results[pair.b2, pair.b1].w++; 
                        }
                        else 
                        {
                            results[pair.b1, pair.b2].l++; 
                            results[pair.b2, pair.b1].w++; 
                        }

                        // right now, this only works for one pair. maybe replace it with some sort of WriteResults which repeats the whole array
                        var (w, l, d, p) = results[pair.b1, pair.b2];
                        Console.Write("\x1b[1A"); // XXX detect ansi or con
                        Console.WriteLine($"{p0} vs {p1}, {p} games: {w}W / {l}L / {d}D ({w*100/p}% / {l*100/p}% / {d*100/p}%)");
                    };

                    var m1 = new Match(verbosity, startingField.Clone(), p0, p1);
                    arena.Submit(m1, OnCompleted(MatchResult.Player0Win));

                    var m2 = new Match(verbosity, startingField.Clone(), p1, p0);
                    arena.Submit(m2, OnCompleted(MatchResult.Player1Win));
                }
            }

            WriteCSV("wins", bots, results, (b1, b2) => results[b1, b2].w );
            WriteCSV("losses", bots, results, (b1, b2) => results[b1, b2].l);
            WriteCSV("draws", bots, results, (b1, b2) => results[b1, b2].d);
            WriteCSV("net", bots, results, (b1, b2) => results[b1, b2].w - results[b1, b2].l);
            WriteCSV("full", bots, results, (b1, b2) => results[b1, b2].w, (b1, b2) => results[b1, b2].l, (b1, b2) => results[b1, b2].d);
        }

        private void WriteCSV(string name, IStrategy[] bots, (int w, int l, int d, int p)[,] results, params Func<int, int, int>[] fs)
        {
            var csv = new StringBuilder();

            foreach (var f in fs)
            {
                csv.AppendLine("\"player0\"," + string.Join(",", bots.Select(s => $"\"vs {s}\"")));
                for (var b1 = 0; b1 < bots.Length; b1++)
                {   
                    var botResults = Enumerable.Range(0, bots.Length).Select(b2 => b1 == b2 ? "\"\"" : $"\"{f(b1, b2)}\"");
                    csv.AppendLine($"\"{bots[b1]}\"," + string.Join(",", botResults));
                }
            }

            File.WriteAllText($"arena-{bots.Length}x{results[0,1]}-{name}.csv", csv.ToString());
        }
    }
}