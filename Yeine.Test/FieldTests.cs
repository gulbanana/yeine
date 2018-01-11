using System;
using Xunit;
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
        [InlineData(1, 1, ".", 0)]
        [InlineData(1, 1, "0", 1)]
        [InlineData(1, 1, "1", -1)]
        [InlineData(3, 3, "0,0,.,0,.,.,.,.,1", 2)]
        public void CalculateValue(int w, int h, string cells, int result)
        {
            var f = new Field(w, h, cells);

            Assert.Equal(result, f.CalculatePositionValue('0', '1'));
        }

        [Theory]
        [InlineData(1, 1, ".")]
        [InlineData(3, 3, ".,0,.,.,0,0,.,.,.")]
        public void ToString_Idempotent(int w, int h, string cells)
        {
            var f = new Field(w, h, cells);
            
            Assert.Equal(cells, f.ToString());
        }
    }
}