using TerminalGame.Computers;

namespace TerminalGame
{
    class Player
    {
        private static Player _instance;

        public Computer ConnectedComputer { get; set; }
        public Computer PlayersComputer { get; set; }

        public string Name { get; private set; }
        public string Password { get; private set; }
        public int Balance { get; private set; }

        private Player() { }

        public static Player GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Player();
            }
            return _instance;
        }

        public void CreateNewPlayer(string name, string password)
        {
            Name = name;
            Password = password;
            Balance = 1000;
        }

        public void LoadPlayer(string name, string password, int balance)
        {
            Name = name;
            Password = password;
            Balance = balance;
        }

        public void AddMoney(int amount)
        {
            if (Balance > 0 && Balance + amount < 0)
                Balance = int.MaxValue;
            else
                Balance += amount;
        }

        public void SubMoney(int amount)
        {
            if (Balance < 0 && Balance - amount > 0)
                Balance = int.MinValue;
            else
                Balance -= amount;
        }
    }
}
