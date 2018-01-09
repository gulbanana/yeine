using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    public class AlwaysPass : IStrategy
    {
        public Move Act(Game state) => Move.Pass();
    }
}