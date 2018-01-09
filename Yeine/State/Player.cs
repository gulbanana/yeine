namespace Yeine.State
{
    public class Player
    {
        public string Name { get; private set; }
        public int LivingCells { get; set; }
        public string previousMove { get; set; }

        public Player(string playerName)
        {
            Name = playerName;
        }
    }
}
