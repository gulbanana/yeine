using System.Text;
using System.Collections.Generic;

namespace Yeine.API
{
    public class Move
    {
        private readonly MoveType command;
        private readonly Point[] arguments;

        private Move(MoveType type, params Point[] points) 
        {
            this.command = type;
            this.arguments = points;
        }

        public static Move Pass() => new Move(MoveType.Pass);

        public static Move Kill(Point target) => new Move(MoveType.Kill, target);

        public static Move Birth(Point target, Point parent1, Point parent2) => new Move(MoveType.Birth, target, parent1, parent2);

        public override string ToString() 
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(command.ToString());
            
            for (var i = 0; i < arguments.Length; i++)
            {
                builder.Append(" ");
                builder.Append(arguments[i].ToString());
            }

            return builder.ToString();
        }
    }
}
