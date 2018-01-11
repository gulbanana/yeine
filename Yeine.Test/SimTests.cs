using System;
using Xunit;
using Yeine.State;

namespace Yeine.Test
{
    public class SimTests
    {
        [Fact]
        public void EmptyField()
        {
            var f = new Field(3, 3, ".,.,.,.,.,.,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,.,.,.,.,.,.,.,.", f.ToString());
        }

        [Fact]
        public void OneCellDies()
        {
            var f = new Field(3, 3, ".,.,.,.,0,.,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,.,.,.,.,.,.,.,.", f.ToString());
        }

        [Fact]
        public void TwoCellsDie()
        {
            var f = new Field(3, 3, ".,.,.,.,0,0,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,.,.,.,.,.,.,.,.", f.ToString());
        }

        [Fact]
        public void ThreeCellsGrow()
        {
            var f = new Field(3, 3, ".,0,.,.,0,0,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,0,0,.,0,0,.,.,.", f.ToString());
        }

        [Fact]
        public void FourCellsSurvive()
        {
            var f = new Field(3, 3, ".,0,0,.,0,0,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,0,0,.,0,0,.,.,.", f.ToString());
        }

        [Fact]
        public void GrowColour_MinorityThem()
        {
            var f = new Field(3, 3, ".,1,.,.,0,0,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,1,0,.,0,0,.,.,.", f.ToString());
        }

        [Fact]
        public void GrowColour_MajorityThem()
        {
            var f = new Field(3, 3, ".,0,.,.,1,1,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,0,1,.,1,1,.,.,.", f.ToString());
        }

        [Fact]
        public void GrowColour_EntirelyThem()
        {
            var f = new Field(3, 3, ".,1,.,.,1,1,.,.,.");

            f.UpdatePosition();

            Assert.Equal(".,1,1,.,1,1,.,.,.", f.ToString());
        }
    }
}