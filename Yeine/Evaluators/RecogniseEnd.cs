using Yeine.State;

namespace Yeine.Evaluators
{
    public class RecogniseEnd : IEvaluator
    {
        private readonly IEvaluator inner;

        public RecogniseEnd(IEvaluator inner)
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

        public override string ToString() => $"{nameof(RecogniseEnd)}";
    }
}