using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalGame.Programs
{
    class Mkdir : Program
    {
        private static Mkdir _instance;

        public static Mkdir GetInstance()
        {
            if (_instance == null)
                _instance = new Mkdir();
            return _instance;
        }

        private Mkdir()
        {

        }

        protected override void Run()
        {
            if(_args.Length < 1)
            {
                Game.Terminal.WriteLine("mkdir: missing operand");
                Kill();
                return;
            }
            if(_args.Length > 1)
            {
                Game.Terminal.WriteLine("mkdir: too many operands");
                Kill();
                return;
            }

            World.World.GetInstance().Player.ConnectedComp.FileSystem.CurrentDir.AddFile(new Files.File(_args[0]));
            Kill();
            return;
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {

        }
    }
}
