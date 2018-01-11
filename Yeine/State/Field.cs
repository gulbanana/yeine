namespace Yeine.State
{
    public struct Field
    {
        public int Width { get; }
        public int Height { get; }
        public char[,] Cells { get; }

        public Field(int width, int height, char[,] cells)
        {
            Width = width;
            Height = height;
            Cells = cells;
        }

        public Field Clone()
        {
            return new Field(Width, Height, (char[,])Cells.Clone());
        }
    }
}