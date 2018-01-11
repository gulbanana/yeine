using System;
using System.Collections.Generic;

namespace Yeine.State
{
    public class Game
    {
        // fixed settings - defaults as a convenience for tests
        public int MaxTimebank { get; set; } = 10000;
        public int TimePerMove { get; set; } = 100;
        public int MaxRounds { get; set; } = 100;
        public string OurName { get; set; } = "player0";
        public char OurID { get; set; } = '0';
        public char TheirID { get; set; }  = '1';

        // current game state
        public Field Field { get; private set; }
        public Player Player0 { get; }
        public Player Player1 { get; }
        public int RoundNumber { get; set; }
        public int CurrentTimebank { get; set; } = 10000;

        public Game()
        {
            Field = new Field(0, 0, new char[0,0]);
            Player0 = new Player(0);
            Player1 = new Player(1);
        }

        public void Log(string text)
        {
            Console.Error.WriteLine($"{OurName} T{RoundNumber}: {text}");
        }

        public void ParseField(int width, int height, string input)
        {
            Field = new Field(width, height, input, out Player0.LivingCells, out Player1.LivingCells);
        }
    }
}