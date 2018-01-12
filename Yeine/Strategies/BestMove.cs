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
        private readonly bool canBirth;
        private readonly List<CostedMove> ownKillables;
        
        public BestMove(int lookahead, bool canBirth)
        {
            this.lookahead = lookahead;
            this.canBirth = canBirth;
            ownKillables = new List<CostedMove>();
        }

        public Move Act(Game state)
        {
            var basePosition = state.Field.Clone();
            for (var i = 0; i < lookahead; i++) basePosition.UpdatePosition();
            var baseValue = basePosition.CalculatePositionValue(state.OurID, state.TheirID);

            var bestKillValue = 0;
            var bestKillTarget = default(Point);
            ownKillables.Clear();

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; y < state.Field.Height; y++)
                {
                    if (state.Field.Cells[x,y] != '.')
                    {
                        var simField = state.Field.Clone();                        
                        simField.Cells[x,y] = '.';

                        for (var i = 0; i < lookahead; i++) 
                        {
                            simField.UpdatePosition();
                        }
                        var simValue = simField.CalculatePositionValue(state.OurID, state.TheirID) - baseValue;

                        if (simValue > bestKillValue)
                        {
                            bestKillValue = simValue;
                            bestKillTarget = new Point(x, y);
                        }

                        if (state.Field.Cells[x,y] == state.OurID)
                        {
                            ownKillables.Add(new CostedMove { Value = simValue, Target = new Point(x, y)});
                        }
                    }
                }
            }

            var bestBirthValue = 0;
            var bestBirthTarget = default(Point);
            var bestBirthSac1 = default(Point);
            var bestBirthSac2 = default(Point);

            if (canBirth)
            {
                var bestKillables = ownKillables.OrderByDescending(k => k.Value).Select(k => k.Target).Take(4).ToArray();

                for (var x = 0; x < state.Field.Width; x++)
                {
                    for (var y = 0; y < state.Field.Height; y++)
                    {
                        if (state.Field.Cells[x,y] == '.')
                        {
                            // potential birth target. try each combo of own-killables
                            for (var s1 = 0; s1 < bestKillables.Length - 1; s1++)
                            {
                                for (var s2 = s1+1; s2 < bestKillables.Length; s2++)
                                {
                                    var simField = state.Field.Clone();

                                    simField.Cells[x,y] = state.OurID;
                                    simField.Cells[bestKillables[s1].X,bestKillables[s1].Y] = '.';
                                    simField.Cells[bestKillables[s2].X,bestKillables[s2].Y] = '.';

                                    for (var i = 0; i < lookahead; i++) simField.UpdatePosition();

                                    var simValue = simField.CalculatePositionValue(state.OurID, state.TheirID) - baseValue;

                                    if (simValue > bestBirthValue)
                                    {
                                        bestBirthValue = simValue;
                                        bestBirthTarget = new Point(x, y);
                                        bestBirthSac1 = bestKillables[s1];
                                        bestBirthSac2 = bestKillables[s2];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //state.Log(Field.ReportStats());

            if (canBirth && bestBirthValue > bestKillValue)
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

        public override string ToString() => $"{nameof(BestMove)}(lookahead: {lookahead}, canBirth: {canBirth})";
    }
}