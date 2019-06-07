using System;
using TerminalGame.Computers;

namespace TerminalGame
{
    public class Player
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public int Balance { get; private set; }
        public Computer PlayerComp { get; set; }
        public Computer ConnectedComp { get; set; }

        private static Player _instance;

        public static Player GetInstance()
        {
            if (_instance == null)
                _instance = new Player();
            return _instance;
        }

        private Player()
        {

        }

        public void CreateNewPlayer(string name, string password, int balance = 1000)
        {
            Name = name;
            Password = password;
            Balance = balance;
        }

        public void AddFunds(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative. To remove funds, use the SubtractFunds method instead.");
            if (Balance + value < Balance)
                Balance = int.MaxValue;
            else
                Balance += value;
        }
        public void SubtractFunds(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative. To add funds, use the AddFunds method instead.");
            if (Balance - value > Balance)
                Balance = int.MinValue;
            else
                Balance -= value;
        }
    }
}
