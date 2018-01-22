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
            var move = strat.Act(state, _ => {});

            Assert.NotEqual(MoveType.Pass, move.Command);
        }

        [Fact]
        public void Starter_Pass_IfForced()
        {
            state.ParseField(1, 1, ".");

            var strat = new RandomMoves();
            var move = strat.Act(state, _ => {});

            Assert.Equal(MoveType.Pass, move.Command);
        }

        [Fact]
        public void AlwaysPass_AlwaysPasses()
        {
            var strat = new AlwaysPass();
            var move = strat.Act(state, _ => {});

            Assert.Equal(MoveType.Pass, move.Command);
        }

        [Fact]
        public void BestMove_DoesntCrash()
        {
            state.ParseField(1, 1, ".");

            var strat = new BestMove(1, 4);
            var move = strat.Act(state, _ => {});
        }

        [Theory]
        [InlineData(2, 1, "0,1", 0)]
        [InlineData(1, 1, "0", int.MaxValue)]
        [InlineData(1, 1, "1", int.MinValue)]
        [InlineData(3, 1, "0,0,1", 1)]
        [InlineData(3, 1, "0,1,1", -1)]
        [InlineData(3, 3, "0,0,.,0,.,.,.,.,1", 2)]
        public void BestMove_CellDifferenceCount(int w, int h, string cells, int result)
        {
            var f = new Field(w, h, cells);
            var g = new Game();

            Assert.Equal(result, BestMove.EvaluatePosition(g, f));
        }
    }
}
