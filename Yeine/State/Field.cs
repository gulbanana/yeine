using System;
using System.Text;
using Yeine.API;

namespace Yeine.State
{
    public struct Field
    {
        public readonly int Width;
        public readonly int Height;
        private readonly int length;
        public readonly char[,] Cells;
        private readonly byte[,,] neighbours;

        public Field(int width, int height, char[,] cells, byte[,,] neighbours)
        {
            Width = width;
            Height = height;
            length = width * height;
            Cells = cells;
            this.neighbours = neighbours;
        }

        public Field(int width, int height, string input) 
        {
            Width = width;
            Height = height;
            length = width * height;
            Cells = new char[height, width];
            neighbours = new byte[2, height, width];

            int p0;
            int p1;
            Parse(input, out p0, out p1);
        }

        public Field(int width, int height, string input, out int p0, out int p1)
        {
            Width = width;
            Height = height;
            length = width * height;
            Cells = new char[height, width];
            neighbours = new byte[2, height, width];
            
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
                Cells[y,x] = cellData;

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
                    Cells[kTarget.Y, kTarget.X] = '.';
                    break;

                case MoveType.Birth:
                    var bTarget = move.Arguments[0];
                    Cells[bTarget.Y, bTarget.X] = us;
                    var sac1 = move.Arguments[1];
                    var sac2 = move.Arguments[2];
                    Cells[sac1.Y, sac1.X] = '.';
                    Cells[sac2.Y, sac2.X] = '.';
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

            unsafe 
            {
                fixed (char* pCells = Cells)
                {
                    for (var i = 0; i < length; i++)
                    {
                        if (pCells[i] == us)
                        {
                            ours++;
                        }
                        else if (pCells[i] == them)
                        {
                            theirs++;
                        }
                    }
                }
            }
        }

        public void Simulate()
        {
            unsafe 
            {
                fixed (byte* pNeighbours = neighbours)
                fixed (char* pCells = Cells)
                {
                    long* ls = (long*)pNeighbours;
                    for (var i = 0; i < neighbours.Length / 8; i++)
                    {
                        ls[i] = 0;
                    }
            
                    // pass 1: count neighbours
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            if (pCells[y*Width + x] != '.')
                            {
                                var ixPlayer = (pCells[y*Width + x] - '0') * length;

                                var notLeft = x > 0;
                                var notRight = x < Width - 1;

                                if (notLeft) pNeighbours[ixPlayer + y*Width + x-1]++;
                                if (notRight) pNeighbours[ixPlayer + y*Width + x+1]++;

                                if (y > 0)
                                {
                                    pNeighbours[ixPlayer + (y-1)*Width + x]++;
                                    if (notLeft) pNeighbours[ixPlayer + (y-1)*Width + x-1]++;
                                    if (notRight) pNeighbours[ixPlayer + (y-1)*Width + x+1]++;
                                }

                                if (y < Height-1)
                                {
                                    pNeighbours[ixPlayer + (y+1)*Width + x]++;
                                    if (notLeft) pNeighbours[ixPlayer + (y+1)*Width + x-1]++;
                                    if (notRight) pNeighbours[ixPlayer + (y+1)*Width + x+1]++;
                                }
                            }
                        }
                    }

                    // pass 2: life and death
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            var totalNeighbours = pNeighbours[y*Width + x] + pNeighbours[length + y*Width + x];
                            if (pCells[y*Width + x] == '.')
                            {                        
                                if (totalNeighbours == 3)
                                {
                                    pCells[y*Width + x] = pNeighbours[y*Width + x] > pNeighbours[length + y*Width + x] ? '0' : '1';
                                }
                            }
                            else // living cell
                            {
                                if (totalNeighbours < 2 || totalNeighbours > 3)
                                {
                                    pCells[y*Width + x] = '.';
                                }
                            }
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
                    builder.Append(Cells[y,x]);
                }
            }

            return builder.ToString();
        }
    }
}