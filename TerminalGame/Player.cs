using System;
using System.Runtime.Serialization;
using TerminalGame.Computers;

namespace TerminalGame
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int Balance { get; set; }
        [DataMember]
        public Computer PlayerComp { get; set; }
        [DataMember]
        public Computer ConnectedComp { get; set; }

        //public Player()
        //{

        //}

        private static Player _instance;

        public static Player GetInstance()
        {
            if (_instance == null)
                _instance = new Player();
            return _instance;
        }

        private Player()
        {
            //if (PlayerComp == null)
            //    PlayerComp = World.World.GetInstance().Computers.Find(c => c.Name == "localhost");
            //if (ConnectedComp == null)
            //    PlayerComp = PlayerComp;
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
