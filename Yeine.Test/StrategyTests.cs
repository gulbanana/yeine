using System;
using System.IO;
using System.Linq;
using Xunit;
using Yeine.API;
using Yeine.State;
using Yeine.Strategies;

namespace Yeine.Test
{
    public class StrategyTests
    {
        private readonly Game state;

        public StrategyTests()
        {
            state = new Game();
        }

        [Fact]
        public void Starter_KillOrBirth_WhenAllowed()
        {
            state.ParseField(2, 2, "0,0,1,.");

            var strat = new RandomMoves();
            var move = strat.Act(state);

            Assert.NotEqual(MoveType.Pass, move.Command);
        }

        [Fact]
        public void Starter_Pass_IfForced()
        {
            state.ParseField(1, 1, ".");

            var strat = new RandomMoves();
            var move = strat.Act(state);

            Assert.Equal(MoveType.Pass, move.Command);
        }

        [Fact]
        public void AlwaysPass_AlwaysPasses()
        {
            var strat = new AlwaysPass();
            var move = strat.Act(state);

            Assert.Equal(MoveType.Pass, move.Command);
        }

        [Fact]
        public void BestMove_DoesntCrash()
        {
            var strat = new BestMove(1, false);
            var move = strat.Act(state);
        }
    }
}
