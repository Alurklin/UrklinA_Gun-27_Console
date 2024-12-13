namespace CasinoGame.Profiles
{
    public class PlayerProfile
    {
        public string Name { get; set; }
        public int Money { get; set; }

        public PlayerProfile(string name, int initialMoney)
        {
            Name = name;
            Money = initialMoney;
        }
    }
}
