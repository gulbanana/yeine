using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    public class AlwaysPass : IStrategy
    {
        public Move Act(Game state, IEvaluator evaluator) => Move.Pass();

        public override string ToString() => nameof(AlwaysPass);
    }
}