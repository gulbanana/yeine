using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Yeine.Test
{
    public class IntegrationTest
    {
        private const string script = 
@"settings timebank 10000
settings time_per_move 100
settings player_names player0,player1
settings your_bot player0
settings your_botid 0
settings field_width 18
settings field_height 16
settings max_rounds 100
update game round 0
update game field .,.,.,1,.,0,.,.,.,.,.,0,.,1,.,.,1,.,.,.,.,.,.,.,0,.,.,1,1,.,.,.,1,1,.,.,0,.,.,.,0,.,.,0,1,0,1,.,.,.,1,.,.,.,1,.,.,.,.,1,.,0,.,.,1,.,0,.,1,.,1,1,.,1,.,1,.,.,0,1,.,0,.,0,.,0,.,.,.,1,1,.,.,.,.,0,.,.,1,.,.,.,.,.,0,.,0,1,0,.,.,.,.,.,.,1,.,1,0,.,1,0,.,.,.,.,.,0,.,.,.,.,.,.,1,.,.,.,.,.,1,.,0,1,0,1,.,0,.,.,.,.,.,0,.,.,.,.,.,.,1,.,.,.,.,.,1,0,.,1,0,.,0,.,.,.,.,.,.,1,0,1,.,1,.,.,.,.,.,0,.,.,1,.,.,.,.,0,0,.,.,.,1,.,1,.,1,.,0,1,.,.,0,.,0,.,0,0,.,0,.,1,.,0,.,.,1,.,0,.,.,.,.,0,.,.,.,0,.,.,.,0,1,0,1,.,.,1,.,.,.,1,.,.,0,0,.,.,.,0,0,.,.,1,.,.,.,.,.,.,.,0,.,.,0,.,1,.,.,.,.,.,1,.,0,.,.,.
update player0 living_cells 50
update player1 living_cells 50
action move 10000";

        [Fact]
        public void OneMove()
        {
            var reader = new StringReader(script);
            var writer = new StringWriter();
            Runner runner = new Runner(reader, writer, new Strategies.AlwaysPass());
            runner.Run();

            Assert.Equal("Pass" + Environment.NewLine, writer.ToString());
        }
    }
}
