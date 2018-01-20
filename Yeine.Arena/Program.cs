using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeine.Strategies;

namespace Yeine.Arena
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = args.SingleOrDefault(a => int.TryParse(a, out _)) ?? "2";
            var isVerbose = args.Any(a => a == "-v" || a == "--verbose");
            var isVeryVerbose = args.Any(a => a == "-vv" || a == "--veryVerbose");

            var bots = new IStrategy[]
            {
                new Strategies.BestMove(3, 4),
                new Strategies.BestMove(4, 5),
            };

            var pairs = new List<(int b1,int b2)>();
            for (var b1 = 0; b1 < bots.Length-1; b1++)
            {
                for (var b2 = b1+1; b2 < bots.Length; b2++)
                {
                    pairs.Add((b1, b2));
                }
            }

            var results = new (int w, int l, int d)[bots.Length,bots.Length];

            var games = int.Parse(count);
            foreach (var pair in pairs)
            {
                var p0 = bots[pair.b1];
                var p1 = bots[pair.b2];
                var eventLoop = new ArenaEventLoop(isVeryVerbose ? 2 : isVerbose ? 1 : 0, p0, p1);
                var (w, l, d) = eventLoop.PlayGames(games);
                
                Console.WriteLine($"{p0} vs {p1}, {games} games: {w}W / {l}L / {d}D ({w*100/games}% / {l*100/games}% / {d*100/games}%).");

                results[pair.b1, pair.b2] = (w, l, d);
                results[pair.b2, pair.b1] = (l, w, d);
            }

            WriteCSV("wins", bots, results, games, (b1, b2) => results[b1, b2].w );
            WriteCSV("losses", bots, results, games, (b1, b2) => results[b1, b2].l);
            WriteCSV("draws", bots, results, games, (b1, b2) => results[b1, b2].d);
            WriteCSV("net", bots, results, games, (b1, b2) => results[b1, b2].w - results[b1, b2].l);
        }

        private static void WriteCSV(string name, IStrategy[] bots, (int w, int l, int d)[,] results, int total, Func<int, int, int> transitive)
        {
            var csv = new StringBuilder();

            csv.AppendLine("\"player0\"," + string.Join(",", bots.Select(s => $"\"vs {s}\"")));

            for (var b1 = 0; b1 < bots.Length; b1++)
            {
                var botResults = Enumerable.Range(0, bots.Length).Select(b2 => b1 == b2 ? "\"0\"" : $"\"{results[b1, b2].w - results[b1, b2].l}\"");
                csv.AppendLine($"\"{bots[b1]}\"," + string.Join(",", botResults));
            }

            File.WriteAllText($"arena-{DateTime.Now}-{name}.csv", csv.ToString());
        }
    }
}