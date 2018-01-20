using BenchmarkDotNet.Attributes;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Bench
{
    public class EvaluationOptions
    {
        private readonly Game state;
        [Params(true, false)] public bool Shortcuts { get; set; }

        public EvaluationOptions()
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
        }

        [Benchmark] public int Evaluate() => BestMove.EvaluatePosition(state, state.Field, Shortcuts);
    }
}