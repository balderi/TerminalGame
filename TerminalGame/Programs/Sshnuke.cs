using System;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Sshnuke : Program
    {
        private readonly string[] _textToWrite;
        private int _counter;
        private Computer _cComp;

        private static Sshnuke _instance;

        public static Sshnuke GetIsntance()
        {
            if (_instance == null)
                _instance = new Sshnuke();
            return _instance;
        }

        private Sshnuke()
        {
            _textToWrite = new string[]
                {
                    ".",
                    ".",
                    ".",
                    " successful.",
                    "\nAttempting to exploit SSHv1 CRC32 ",
                    ".",
                    ".",
                    ".",
                    " successful.",
                    "\nResetting root password to \"password\".",
                    "\nSystem open: Access level <9>"
                };
        }

        protected override void Run()
        {
            _cComp = World.World.GetInstance().Player.ConnectedComp;
            _counter = 0;
            _cComp.PerformIllegalAction();
            Game.Terminal.WriteLine($"Connectiong to  {_cComp.IP}:ssh ");
            _timer.Interval = 1000;
            _timer.Start();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            if (_counter == 3 && !_cComp.CheckPortOpen(22))
            {
                Game.Terminal.WriteLine("sshnuke: error: port 22 not open");
                Kill();
                return;
            }
            if (_counter == 9)
            {
                _timer.Interval = 500;
                _cComp.RootPassword = "password"; // TODO: Change password of root user whenever that is implemented...
                _cComp.PlayerHasRoot = true;
            }
            if (_counter == _textToWrite.Length)
            {
                Kill();
                return;
            }

            Game.Terminal.Write(_textToWrite[_counter++]);
        }
    }
}
