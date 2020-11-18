using System;
using TerminalGame.Files;

namespace TerminalGame.Programs
{
    class Cat : Program
    {
        private static Cat _instance;

        public static Cat GetInstance()
        {
            if (_instance == null)
                _instance = new Cat();
            return _instance;
        }

        private Cat()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: cat");
                Kill();
                return;
            }
            if (World.World.GetInstance().Player.ConnectedComp.FileSystem.TryFindFile(_args[0], out File f))
            {
                Game.Terminal.WriteLine(f.ToString());
            }
            else
            {
                Game.Terminal.WriteLine($"cat: {_args[0]}: no such file or directory");
            }
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
