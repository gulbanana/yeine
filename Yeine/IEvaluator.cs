using Yeine.API;
using Yeine.State;

namespace Yeine
{
    public interface IEvaluator
    {
        int EvaluatePosition(Game state, Field position);
    }
}