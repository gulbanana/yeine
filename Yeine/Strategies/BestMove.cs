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
        private readonly double attackFactor;
        private readonly int deadzone;

        public BestMove(int lookahead = 4, int sacrificeOptions = 5, double attackFactor = 0.0, int deadzone = 0)
        {
            this.lookahead = lookahead;
            this.sacrificeOptions = sacrificeOptions;
            this.attackFactor = attackFactor;
            this.deadzone = deadzone;
        }

        public Move Act(Game state, Action<string> report)
        {
            var predictions = 0;

            var passValue = Predict(state, _ => {}); predictions++;

            var bestKillValue = 0;
            var bestKillTarget = default(Point);
            var bestKillables = new List<CostedMove>();

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; y < state.Field.Height; y++)
                {
                    if (state.Field.Cells[y,x] != '.')
                    {
                        var killValue = Predict(state, f => f.Cells[y,x] = '.') - passValue; predictions++;

                        if (killValue > bestKillValue)
                        {
                            bestKillValue = killValue;
                            bestKillTarget = new Point(x, y);
                        }

                        if (state.Field.Cells[y,x] == state.OurID)
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
                    if (state.Field.Cells[y,x] == '.')
                    {
                        // potential birth target. try each combo of own-killables
                        for (var s1 = 0; s1 < sacrifices.Length - 1; s1++)
                        {
                            for (var s2 = s1+1; s2 < sacrifices.Length; s2++)
                            {
                                var birthValue = Predict(state, f =>
                                {
                                    f.Cells[y,x] = state.OurID;
                                    f.Cells[sacrifices[s1].Y,sacrifices[s1].X] = '.';
                                    f.Cells[sacrifices[s2].Y,sacrifices[s2].X] = '.';
                                }) - passValue; predictions++;

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

            report($"executed {predictions} predictions");

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

        private int Predict(Game state, Action<Field> f)
        {
            var position = state.Field.Clone();
            
            f(position);

            for (var i = 0; i < lookahead; i++)
            {
                position.Simulate();
            }

            return EvaluatePosition(state, position, attackFactor, deadzone);
        }

        public static int EvaluatePosition(Game state, Field position, double attackFactor = 0.0, int deadzone = 0)
        {
            var ours = 0;
            var theirs = 0;

            position.Evaluate(state.OurID, state.TheirID, out ours, out theirs);

            // don't lose the game by mistake
            if (ours == 0) return int.MinValue; 

            // always win the game if possible
            if (theirs == 0) return int.MaxValue; 

            // if already ahead, prioritise destroying enemy cells 
            if ((ours - theirs) > deadzone)
            {
                var weight = (double)ours - (double)theirs;
                var weightedEnemyCells = (double)theirs * (1.0 + weight * attackFactor);
                theirs = (int)weightedEnemyCells;
            }

            return ours - theirs;
        }

        public override string ToString() => $"(agg {attackFactor}, gap {deadzone})";
    }
}