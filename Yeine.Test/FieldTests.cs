using System;
using Xunit;
using Yeine.API;
using Yeine.State;

namespace Yeine.Test
{
    public class FieldTests
    {
        [Theory]
        [InlineData(1, 1, ".")]
        [InlineData(1, 2, ".,.")]
        [InlineData(2, 1, ".,.")]
        [InlineData(2, 2, ".,.,.,.")]
        public void Parse(int w, int h, string cells)
        {
            var f = new Field(w, h, cells);

            Assert.Equal(w, f.Width);
            Assert.Equal(h, f.Height);

            var doesNotThrow = f.Cells[w-1, h-1];
            Assert.Throws<IndexOutOfRangeException>(() => f.Cells[w, h-1]);
            Assert.Throws<IndexOutOfRangeException>(() => f.Cells[w-1, h]);
        }

        [Theory]
        [InlineData(1, 1, ".")]
        [InlineData(5, 3, "0,0,0,0,0,.,.,.,.,.,1,1,1,1,1")]
        public void Clone(int w, int h, string cells)
        {
            var f1 = new Field(w, h, cells);
            var f2 = f1.Clone();

            Assert.Equal(f1.Width, f2.Width);
            Assert.Equal(f1.Height, f2.Height);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    Assert.Equal(f1.Cells[x,y], f2.Cells[x,y]);
                }
            }
        }

        [Theory]
        [InlineData(1, 1, ".")]
        [InlineData(3, 3, ".,0,.,.,0,0,.,.,.")]
        public void ToString_Idempotent(int w, int h, string cells)
        {
            var f = new Field(w, h, cells);
            
            Assert.Equal(cells, f.ToString());
        }

        [Fact]
        public void CountStartingCells()
        {
            var f = new Field(18, 16, ".,0,0,0,.,.,.,.,.,0,.,.,.,0,.,.,.,.,0,.,.,.,.,0,0,.,.,.,.,.,.,0,0,.,.,0,.,0,.,0,0,.,.,0,.,0,0,.,.,.,.,.,0,.,.,0,.,.,.,.,.,.,.,0,.,.,.,0,0,.,.,.,0,0,0,.,.,.,.,0,0,.,.,0,.,.,.,.,.,.,.,.,0,.,.,.,.,.,.,0,.,.,.,.,.,.,0,.,0,.,0,.,.,.,.,0,0,.,.,0,0,0,.,.,.,.,.,0,.,.,.,0,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,1,.,.,.,1,.,.,.,.,.,1,1,1,.,.,1,1,.,.,.,.,1,.,1,.,1,.,.,.,.,.,.,1,.,.,.,.,.,.,1,.,.,.,.,.,.,.,.,1,.,.,1,1,.,.,.,.,1,1,1,.,.,.,1,1,.,.,.,1,.,.,.,.,.,.,.,1,.,.,1,.,.,.,.,.,1,1,.,1,.,.,1,1,.,1,.,1,.,.,1,1,.,.,.,.,.,.,1,1,.,.,.,.,1,.,.,.,.,1,.,.,.,1,.,.,.,.,.,1,1,1,.");
            
            f.EvaluateLivingCells('0', '1', out var us, out var them);

            Assert.Equal(40, us);
            Assert.Equal(40, them);
        }

        [Fact]
        public void ProcessBirth()
        {
            var f = new Field(18, 16, ".,0,0,0,.,.,.,.,.,0,.,.,.,0,.,.,.,.,0,.,.,.,.,0,0,.,.,.,.,.,.,0,0,.,.,0,.,0,.,0,0,.,.,0,.,0,0,.,.,.,.,.,0,.,.,0,.,.,.,.,.,.,.,0,.,.,.,0,0,.,.,.,0,0,0,.,.,.,.,0,0,.,.,0,.,.,.,.,.,.,.,.,0,.,.,.,.,.,.,0,.,.,.,.,.,.,0,.,0,.,0,.,.,.,.,0,0,.,.,0,0,0,.,.,.,.,.,0,.,.,.,0,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,1,.,.,.,1,.,.,.,.,.,1,1,1,.,.,1,1,.,.,.,.,1,.,1,.,1,.,.,.,.,.,.,1,.,.,.,.,.,.,1,.,.,.,.,.,.,.,.,1,.,.,1,1,.,.,.,.,1,1,1,.,.,.,1,1,.,.,.,1,.,.,.,.,.,.,.,1,.,.,1,.,.,.,.,.,1,1,.,1,.,.,1,1,.,1,.,1,.,.,1,1,.,.,.,.,.,.,1,1,.,.,.,.,1,.,.,.,.,1,.,.,.,1,.,.,.,.,.,1,1,1,.");
            
            Assert.Equal('.', f.Cells[6,7]);
            Assert.Equal('0', f.Cells[3,0]);
            Assert.Equal('0', f.Cells[9,0]);

            f.ProcessCommand(Move.Birth(new Point(6,7), new Point(3,0), new Point(9,0)), '0');

            Assert.Equal('0', f.Cells[6,7]);
            Assert.Equal('.', f.Cells[3,0]);
            Assert.Equal('.', f.Cells[9,0]);
        }
    }
}