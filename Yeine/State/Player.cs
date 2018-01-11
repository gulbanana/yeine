namespace Yeine.State
{
    public class Player
    {
        public int ID { get; }
        public string Name { get; set; }
        public int LivingCells { get; set; }
        public string LastMove { get; set; }

        public Player(int id)
        {
            ID = id;
            Name = $"player{id}";
        }
    }
}
