using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    /// <summary>Kill the cell which has the greatest positive impact on gamestate</summary>
    public class KillBest : IStrategy
    {
        public Move Act(Game state)
        {
            var bestValue = 0;
            var bestTarget = default(Point);

            for (var x = 0; x < state.Field.Width; x++)
            {
                for (var y = 0; x < state.Field.Height; y++)
                {
                    if (state.Field.Cells[x,y] != '.')
                    {
                        var value = -1; // XXX clone and run sim
                        if (value > bestValue)
                        {
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
    }
}