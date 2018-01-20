using BenchmarkDotNet.Attributes;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Bench
{
    public class TurnCost
    {
        private readonly Game state;

        private readonly IStrategy bm12 = new BestMove(1, 2); // no lookahead, minimal sac count
        private readonly IStrategy bm34 = new BestMove(3, 4); // older config, should be faster
        private readonly IStrategy bm45 = new BestMove(4, 5); // current (20/01) optimal config
        private readonly IStrategy bm46 = new BestMove(4, 6); // unmeasurably-better(?) config

        public TurnCost()
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

        [Benchmark] public void BestMove_12() => bm12.Act(state);
        [Benchmark] public void BestMove_34() => bm34.Act(state);
        [Benchmark] public void BestMove_45() => bm45.Act(state);
        [Benchmark] public void BestMove_46() => bm46.Act(state);
    }
}