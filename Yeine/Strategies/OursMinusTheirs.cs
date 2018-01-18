using Yeine.State;

namespace Yeine.Strategies
{
    public class OursMinusTheirs : IMoveEvaluator
    {
        public int EvaluatePosition(Game state, Field position)
        {
            var ours = 0;
            var theirs = 0;

            position.EvaluateLivingCells(state.OurID, state.TheirID, out ours, out theirs);

            return ours - theirs;
        }

        public override string ToString() => "basic";
    }
}