using Yeine.State;

namespace Yeine.Strategies
{
    public class RecogniseEnd : IMoveEvaluator
    {
        private readonly IMoveEvaluator inner;

        public RecogniseEnd(IMoveEvaluator inner)
        {
            this.inner = inner;
        }

        public int EvaluatePosition(Game state, Field position)
        {
            var us = state.OurID == '0' ? state.Player0 : state.Player1;
            var them = state.OurID == '0' ? state.Player1 : state.Player0;
            
            if (us.LivingCells == 0) 
            {
                return int.MinValue;
            }
            else if (them.LivingCells == 0) 
            {
                return int.MaxValue;
            }
            else
            {
                return inner.EvaluatePosition(state, position);
            }
        }

        public override string ToString() => $"shortcut";
    }
}