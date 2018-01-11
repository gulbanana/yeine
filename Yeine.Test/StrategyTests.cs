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

            var strat = new Starter();
            var move = strat.Act(state);

            Assert.NotEqual(MoveType.Pass, move.Command);
        }

        [Fact]
        public void Starter_Pass_IfForced()
        {
            state.ParseField(1, 1, ".");

            var strat = new Starter();
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
        public void KillBest_DoesntCrash()
        {
            var strat = new KillBest();
            var move = strat.Act(state);
        }
    }
}
