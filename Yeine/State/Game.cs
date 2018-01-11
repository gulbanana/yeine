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
        public string MyName { get; set; } = "player0";
        public string MyID { get; set; } = "0";
        public string TheirID { get; set; }  = "1";

        // field state
        public int FieldWidth { get; set; } = 0;
        public int FieldHeight { get; set; } = 0;
        public string[,] Cells { get; private set; } = new string[0,0];

        // current round state
        public Player Player0 { get; }
        public Player Player1 { get; }
        public int RoundNumber { get; set; }
        public int CurrentTimebank { get; set; } = 10000;

        public Game()
        {
            Player0 = new Player(0);
            Player1 = new Player(1);
        }

        public void Log(string text)
        {
            Console.Error.WriteLine($"{MyName} T{RoundNumber}: {text}");
        }

        public void ParseField(int width, int height, string input)
        {
            FieldWidth = width;
            FieldHeight = height;
            ParseField(input);
        }

        public void ParseField(string input)
        {
            Cells = new string[FieldWidth, FieldHeight];
            var x = 0;
            var y = 0;

            Player0.LivingCells = 0;
            Player1.LivingCells = 0;
            foreach (string cell in input.Split(','))
            {
                Cells[x,y] = cell;

                switch (cell)
                {
                    case "0":
                        Player0.LivingCells++;
                        break;

                    case "1":
                        Player1.LivingCells++;
                        break;
                }

                if (++x == FieldWidth)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
}