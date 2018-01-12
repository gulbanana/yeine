using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    /// <summary>Kill the cell which has the greatest positive impact on gamestate</summary>
    public class KillBest : IStrategy
    {
        public Move Act(Game state)
        {
            var basePosition = state.Field.Clone();
            basePosition.UpdatePosition();
            var baseValue = basePosition.CalculatePositionValue(state.OurID, state.TheirID);

            var bestValue = 0;
            var bestTarget = default(Point);

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; y < state.Field.Height; y++)
                {
                    if (state.Field.Cells[x,y] != '.')
                    {
                        var simField = state.Field.Clone();
                        
                        simField.Cells[x,y] = '.';
                        simField.UpdatePosition();

                        var simValue = simField.CalculatePositionValue(state.OurID, state.TheirID) - baseValue;

                        if (simValue > bestValue)
                        {
                            bestValue = simValue;
                            bestTarget = new Point(x, y);
                        }
                    }
                }
            }

            if (bestValue > 0)
            {
                return Move.Kill(bestTarget);
            }
            else
            {
                return Move.Pass();
            }
        }

        public override string ToString() => nameof(KillBest);
    }
}