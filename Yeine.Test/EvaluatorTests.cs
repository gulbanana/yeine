using System;
using Xunit;
using Yeine.Evaluators;
using Yeine.State;

namespace Yeine.Test
{
    public class EvaluatorTests
    {
        [Theory]
        [InlineData(1, 1, ".", 0)]
        [InlineData(1, 1, "0", 1)]
        [InlineData(1, 1, "1", -1)]
        [InlineData(3, 3, "0,0,.,0,.,.,.,.,1", 2)]
        public void OursMinusTheirs(int w, int h, string cells, int result)
        {
            var f = new Field(w, h, cells);
            var g = new Game();

            Assert.Equal(result, new OursMinusTheirs().EvaluatePosition(g, f));
        }
    }
}