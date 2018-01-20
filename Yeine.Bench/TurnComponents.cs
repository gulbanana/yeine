using BenchmarkDotNet.Attributes;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Bench
{
    public class TurnComponents
    {
        private readonly Game state;
        private Field oscillatingField;

        public TurnComponents()
        {
            state = new Game();
            state.ParseField(18, 16, 
                ".,0,0,.,.,.,.,.,.,.,.,.,.,0,.,.,.,.," +
                "0,.,.,.,.,0,0,.,.,.,.,.,.,0,0,.,.,0," + 
                ".,0,.,0,0,.,.,0,.,0,0,.,.,.,.,.,0,.," + 
                ".,0,.,.,.,.,.,.,.,0,.,.,.,0,0,.,.,.," + 
                "0,0,0,.,.,.,.,0,0,.,.,0,.,.,.,.,.,.," + 
                ".,.,0,.,.,.,.,.,.,0,.,.,.,.,.,.,0,.," + 
                "0,.,0,.,.,.,.,0,0,.,.,0,0,0,.,.,.,.," + 
                ".,0,.,.,.,0,0,.,.,.,.,.,.,.,.,.,.,.," + 
                ".,.,.,.,.,.,.,.,.,.,.,.,1,.,.,.,1,.," + 
                ".,.,.,.,1,1,1,.,.,1,1,.,.,.,.,1,.,1," +
                ".,1,.,.,.,.,.,.,1,.,.,.,.,.,.,1,.,.," +
                ".,.,.,.,.,.,1,.,.,1,1,.,.,.,.,1,1,1," +
                ".,.,.,1,1,.,.,.,1,.,.,.,.,.,.,.,1,.," +
                ".,1,.,.,.,.,.,1,1,.,1,.,.,1,1,.,1,.," +
                "1,.,.,1,1,.,.,.,.,.,.,1,1,.,.,.,.,1," + 
                ".,.,.,.,1,.,.,.,1,.,.,.,.,.,1,1,1,.");

            oscillatingField = state.Field.Clone();
        }

        [Benchmark] 
        public Field Clone()
        {
            return state.Field.Clone();
        }

        [Benchmark] 
        public Field Sim()
        {
            oscillatingField.Simulate();
            return oscillatingField;
        }

        [Benchmark] 
        public int Eval()
        {
            return BestMove.EvaluatePosition(state, state.Field, true);
        }

        [Benchmark] 
        public int Lookahead1()
        {
            var clonedField = state.Field.Clone();
            clonedField.Simulate();
            return BestMove.EvaluatePosition(state, clonedField, true);
        }

        [Benchmark] 
        public int Lookahead4()
        {
            var clonedField = state.Field.Clone();
            clonedField.Simulate();
            clonedField.Simulate();
            clonedField.Simulate();
            clonedField.Simulate();
            return BestMove.EvaluatePosition(state, clonedField, true);
        }
    }
}