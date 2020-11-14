using System;
using TerminalGame.Utils;

namespace TerminalGame.Programs
{
    class Disconnect : Program
    {

        private static Disconnect _instance;

        public static Disconnect GetInstance()
        {
            if (_instance == null)
                _instance = new Disconnect();
            return _instance;
        }

        private Disconnect()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: disconnect");
                Kill();
                return;
            }
            if(World.World.GetInstance().Player.ConnectedComp == World.World.GetInstance().Player.PlayerComp)
            {
                Game.Terminal.WriteLine("Cannot disconnect from gateway");
                Kill();
                return;
            }
            World.World.GetInstance().Player.PlayerComp.Connect();
            Game.Terminal.WriteLine("Disconnected");
            Kill();
            MusicManager.GetInstance().ChangeSong("gameBgm", 0.5f);
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
