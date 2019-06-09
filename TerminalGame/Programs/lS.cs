using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Programs
{
    class Ls : Program
    {
        private static Ls _instance;

        public static Ls GetInstance()
        {
            if (_instance == null)
                _instance = new Ls();
            return _instance;
        }

        private Ls()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            Game.Terminal.WriteLine(Player.GetInstance().ConnectedComp.FileSystem.CurrentDir.ListChildren());
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
