using TerminalGame.Computers;

namespace TerminalGame
{
    class Player
    {
        //private string name;
        
        private static Player instance;

        public Computer ConnectedComputer { get; set; }
        public Computer PlayersComputer { get; set; }

        private Player() { }

        public static Player GetInstance()
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }
    }
}
