using Yeine.State;

namespace Yeine.Evaluators
{
    public class OursMinusTheirs : IEvaluator
    {
        public int EvaluatePosition(Game state, Field position)
        {
            var ours = 0;
            var theirs = 0;

            position.EvaluateLivingCells(state.OurID, state.TheirID, out ours, out theirs);

            return ours - theirs;
        }

        public override string ToString() => nameof(OursMinusTheirs);
    }
}