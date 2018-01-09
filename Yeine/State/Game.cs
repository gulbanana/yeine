using System.Collections.Generic;

namespace Yeine.State
{
    public class Game
    {
        public int MaxTimebank { get; set; }
        public int TimePerMove { get; set; }
        public int MaxRounds { get; set; }
        public int RoundNumber { get; set; }
        public int Timebank { get; set; }
        public string MyName { get; set; }

        public Dictionary<string, Player> Players { get; }
        public Field Field { get; }

        public Game()
        {
            Field = new Field();
            Players = new Dictionary<string, Player>();
        }
    }
}