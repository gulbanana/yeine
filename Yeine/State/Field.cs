using System;
using System.Text;
using Yeine.API;

namespace Yeine.State
{
    public struct Field
    {
        private static int updates;
        private static int evaluations;

        public readonly int Width;
        public readonly int Height;
        public readonly char[,] Cells;

        public Field(int width, int height, char[,] cells)
        {
            Width = width;
            Height = height;
            Cells = cells;
        }

        public Field(int width, int height, string input) 
        {
            Width = width;
            Height = height;
            Cells = new char[width, height];
            
            int p0;
            int p1;
            Parse(input, out p0, out p1);
        }

        public Field(int width, int height, string input, out int p0, out int p1)
        {
            Width = width;
            Height = height;
            Cells = new char[width, height];
            
            Parse(input, out p0, out p1);
        }

        private void Parse(string input, out int p0, out int p1)
        {
            var x = 0;
            var y = 0;

            var p0cells = 0;
            var p1cells = 1;
            foreach (string cell in input.Split(','))
            {
                var cellData = cell[0];
                Cells[x,y] = cellData;

                switch (cellData)
                {
                    case '0':
                        p0cells++;
                        break;

                    case '1':
                        p1cells++;
                        break;
                }

                if (++x == Width)
                {
                    x = 0;
                    y++;
                }
            }

            p0 = p0cells;
            p1 = p1cells;
        }

        public void ProcessCommand(Move move, char us)
        {
            switch (move.Command)
            {
                case MoveType.Kill:
                    var kTarget = move.Arguments[0];
                    Cells[kTarget.X, kTarget.Y] = '.';
                    break;

                case MoveType.Birth:
                    var bTarget = move.Arguments[0];
                    Cells[bTarget.X, bTarget.Y] = us;
                    var sac1 = move.Arguments[1];
                    var sac2 = move.Arguments[2];
                    Cells[sac1.X, sac1.Y] = '.';
                    Cells[sac2.X, sac2.Y] = '.';
                    break;
            }
        }

        public Field Clone()
        {
            return new Field(Width, Height, (char[,])Cells.Clone());
        }

        public void EvaluateLivingCells(char us, char them, out int ours, out int theirs)
        {
            evaluations++;

            ours = 0;
            theirs = 0;

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Cells[x,y] == us)
                    {
                        ours++;
                    }
                    else if (Cells[x,y] == them)
                    {
                        theirs++;
                    }
                }
            }
        }

        public void UpdatePosition()
        {
            updates++;

            var neighbours = new int[Width, Height];

            // pass 1: count neighbours
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Cells[x,y] != '.')
                    {
                        if (x > 0 && y > 0) neighbours[x-1, y-1]++;
                        if (y > 0) neighbours[x, y-1]++;
                        if (x < Width-1 && y > 0) neighbours[x+1, y-1]++;

                        if (x > 0) neighbours[x-1, y]++;
                        if (x < Width-1) neighbours[x+1, y]++;

                        if (x > 0 && y < Height-1) neighbours[x-1, y+1]++;
                        if (y < Height-1) neighbours[x, y+1]++;
                        if (x < Width-1 && y < Height-1) neighbours[x+1, y+1]++;
                    }
                }
            }

            // pass 2: life and death
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Cells[x,y] == '.')
                    {
                        if (neighbours[x,y] == 3)
                        {
                            Cells[x,y] = GetPrevalentTeam(x, y);
                        }
                    }
                    else // living cell
                    {
                        if (neighbours[x,y] < 2 || neighbours[x,y] > 3)
                        {
                            Cells[x,y] = '.';
                        }
                    }
                }
            }
        }
        
        // XXX sucks that this is separate from the main pass calculations
        private char GetPrevalentTeam(int w, int h)
        {
            var zero = 0;
            var one = 0;

            for (var x = w-1; x <= w+1; x++)
            {
                for (var y = h-1; y <= h+1; y++)
                {
                    if (x < 0 || y < 0 || x == Width || y == Height) 
                    {
                        continue;
                    }
                    else if (Cells[x,y] == '0')
                    {
                        zero++;
                    }
                    else if (Cells[x,y] == '1')
                    {
                        one++;
                    }
                }
            }

            return (zero > one) ? '0' : '1';
        }

        // inefficient, but it's only used in tests
        public override string ToString()
        {
            var builder = new StringBuilder();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (builder.Length != 0)
                    {
                        builder.Append(",");
                    }
                    builder.Append(Cells[x,y]);
                }
            }

            return builder.ToString();
        }

        public static string ReportStats()
        {
            return $"{updates} updates, {evaluations} evaluations";
        }
    }
}