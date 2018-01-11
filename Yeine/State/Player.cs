namespace Yeine.State
{
    public class Player
    {
        public readonly int ID;
        public string Name;
        public int LivingCells;
        public string LastMove;

        public Player(int id)
        {
            ID = id;
            Name = $"player{id}";
        }
    }
}
