using Yeine.API;
using Yeine.State;

namespace Yeine.Strategies
{
    public interface IMoveEvaluator
    {
        int EvaluatePosition(Game state, Field position);
    }
}