using System;

namespace Yeine.State
{
    public struct Field
    {
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

        public Field Clone()
        {
            return new Field(Width, Height, (char[,])Cells.Clone());
        }

        public int CalculateValue(char us, char them)
        {
            var ours = 0;
            var theirs = 0;

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

            return ours - theirs;
        }
    }
}