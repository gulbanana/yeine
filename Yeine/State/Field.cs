using System;
using System.Collections.Generic;
using System.Text;
using Yeine.API;

namespace Yeine.State
{
    public struct Field
    {
        private static readonly List<Point>[,] affectedPoints;

        static Field()
        {
            affectedPoints = new List<Point>[18,16];

            for (var x = 0; x < 18; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    var ps = new List<Point>();

                    var notLeft = x > 0;
                    var notRight = x < 18 - 1;

                    if (notLeft) ps.Add(new Point(x-1, y));
                    if (notRight) ps.Add(new Point(x+1, y));

                    if (y > 0)
                    {
                        ps.Add(new Point(x, y-1));
                        if (notLeft) ps.Add(new Point(x-1, y-1));
                        if (notRight) ps.Add(new Point(x+1, y-1));
                    }

                    if (y < 16-1)
                    {
                        ps.Add(new Point(x, y+1));
                        if (notLeft) ps.Add(new Point(x-1, y+1));
                        if (notRight) ps.Add(new Point(x+1, y+1));
                    }

                    affectedPoints[x,y] = ps;
                }
            }
        }

        public readonly int Width;
        public readonly int Height;
        public readonly char[,] Cells;
        private readonly byte[,,] neighbours;

        public Field(int width, int height, char[,] cells, byte[,,] neighbours)
        {
            Width = width;
            Height = height;
            Cells = cells;
            this.neighbours = neighbours;
        }

        public Field(int width, int height, string input) 
        {
            Width = width;
            Height = height;
            Cells = new char[width, height];
            neighbours = new byte[2, width, height];

            int p0;
            int p1;
            Parse(input, out p0, out p1);
        }

        public Field(int width, int height, string input, out int p0, out int p1)
        {
            Width = width;
            Height = height;
            Cells = new char[width, height];
            neighbours = new byte[2, width, height];
            
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
            return new Field(Width, Height, (char[,])Cells.Clone(), neighbours);
        }

        public void Evaluate(char us, char them, out int ours, out int theirs)
        {
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

        public void Simulate()
        {
            unsafe 
            {
                fixed (byte* bs = neighbours)
                {
                    long* ls = (long*)bs;
                    for (var i = 0; i < neighbours.Length / 8; i++)
                    {
                        ls[i] = 0;
                    }
                }
            } 
            
            // pass 1: count neighbours
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Cells[x,y] != '.')
                    {
                        var p = Cells[x,y] - '0';
                        foreach (var affected in affectedPoints[x,y])
                        {
                            neighbours[p, affected.X, affected.Y]++;
                        }
                    }
                }
            }

            // pass 2: life and death
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var totalNeighbours = neighbours[0,x,y] + neighbours[1,x,y];
                    if (Cells[x,y] == '.')
                    {                        
                        if (totalNeighbours == 3)
                        {
                            Cells[x,y] = neighbours[0,x,y] > neighbours[1,x,y] ? '0' : '1';
                        }
                    }
                    else // living cell
                    {
                        if (totalNeighbours < 2 || totalNeighbours > 3)
                        {
                            Cells[x,y] = '.';
                        }
                    }
                }
            }
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
    }
}