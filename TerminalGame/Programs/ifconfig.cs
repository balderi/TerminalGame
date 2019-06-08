using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Programs
{
    class Ifconfig : Program
    {

        private static Ifconfig _instance;

        public static Ifconfig GetInstance()
        {
            if (_instance == null)
                _instance = new Ifconfig();
            return _instance;
        }

        private Ifconfig()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: ifconfig");
                Kill();
                return;
            }
            Game.Terminal.WriteLine(Player.GetInstance().ConnectedComp.IP);
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
