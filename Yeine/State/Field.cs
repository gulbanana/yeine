using System.Collections.Generic;
using Yeine.API;

namespace Yeine.State
{
    public class Field
    {
        public string MyID { get; set; }
        public string OpponentID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private string[,] Cells;

        public void ParseFromString(string input)
        {
            Cells = new string[Width, Height];
            int x = 0;
            int y = 0;

            foreach (string cell in input.Split(','))
            {
                Cells[x,y] = cell;

                if (++x == Width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public Dictionary<string, List<Point>> GetCellMapping()
        {
            var cellMap = new Dictionary<string, List<Point>>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    string cell = Cells[x,y];

                    if (!cellMap.ContainsKey(cell))
                    {
                        cellMap.Add(cell, new List<Point>());
                    }

                    cellMap[cell].Add(new Point(x, y));
                }
            }

            return cellMap;
        }
    }
}