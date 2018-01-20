using System;
using System.Collections.Generic;
using System.Linq;
using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    /// <summary>Kill the cell which has the greatest positive impact on gamestate</summary>
    public class BestMove : IStrategy
    {
        private readonly int lookahead;
        private readonly int sacrificeOptions;
        private readonly bool evaluationShortcuts;

        public BestMove(int lookahead = 4, int sacrificeOptions = 5, bool evaluationShortcuts = true)
        {
            this.lookahead = lookahead;
            this.sacrificeOptions = sacrificeOptions;
            this.evaluationShortcuts = true;
        }

        public Move Act(Game state)
        {
            var passValue = Lookahead(state, _ => {});

            var bestKillValue = 0;
            var bestKillTarget = default(Point);
            var bestKillables = new List<CostedMove>();

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; y < state.Field.Height; y++)
                {
                    if (state.Field.Cells[x,y] != '.')
                    {
                        var killValue = Lookahead(state, f => f.Cells[x,y] = '.') - passValue;

                        if (killValue > bestKillValue)
                        {
                            bestKillValue = killValue;
                            bestKillTarget = new Point(x, y);
                        }

                        if (state.Field.Cells[x,y] == state.OurID)
                        {
                            bestKillables.Add(new CostedMove { Value = killValue, Target = new Point(x, y)});
                        }
                    }
                }
            }

            var bestBirthValue = 0;
            var bestBirthTarget = default(Point);
            var bestBirthSac1 = default(Point);
            var bestBirthSac2 = default(Point);
            var sacrifices = bestKillables.OrderByDescending(k => k.Value).Take(sacrificeOptions).Select(k => k.Target).ToArray();

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; y < state.Field.Height; y++)
                {
                    if (state.Field.Cells[x,y] == '.')
                    {
                        // potential birth target. try each combo of own-killables
                        for (var s1 = 0; s1 < sacrifices.Length - 1; s1++)
                        {
                            for (var s2 = s1+1; s2 < sacrifices.Length; s2++)
                            {
                                var birthValue = Lookahead(state, f =>
                                {
                                    f.Cells[x,y] = state.OurID;
                                    f.Cells[sacrifices[s1].X,sacrifices[s1].Y] = '.';
                                    f.Cells[sacrifices[s2].X,sacrifices[s2].Y] = '.';
                                }) - passValue;

                                if (birthValue > bestBirthValue)
                                {
                                    bestBirthValue = birthValue;
                                    bestBirthTarget = new Point(x, y);
                                    bestBirthSac1 = sacrifices[s1];
                                    bestBirthSac2 = sacrifices[s2];
                                }
                            }
                        }
                    }
                }
            }

            if (bestBirthValue > bestKillValue)
            {
                return Move.Birth(bestBirthTarget, bestBirthSac1, bestBirthSac2);
            }
            else if (bestKillValue > 0)
            {
                return Move.Kill(bestKillTarget);
            }
            else
            {
                return Move.Pass();
            }
        }

        private int Lookahead(Game state, Action<Field> f)
        {
            var position = state.Field.Clone();
            
            f(position);

            for (var i = 0; i < lookahead; i++)
            {
                position.Simulate();
            }

            return EvaluatePosition(state, position, evaluationShortcuts);
        }

        public static int EvaluatePosition(Game state, Field position, bool shortcuts)
        {
            var ours = 0;
            var theirs = 0;

            position.Evaluate(state.OurID, state.TheirID, out ours, out theirs);

            if (shortcuts)
            {
                if (ours == 0) return int.MinValue;
                if (theirs == 0) return int.MaxValue;
            }

            return ours - theirs;
        }

        public override string ToString() => $"{nameof(BestMove)}(look {lookahead}, sac {sacrificeOptions}, {PlusMinus(evaluationShortcuts)}s)";

        private char PlusMinus(bool b) => b ? '+' : '-';
    }
}