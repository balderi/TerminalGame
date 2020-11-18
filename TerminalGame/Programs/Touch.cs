using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalGame.Programs
{
    class Touch : Program
    {
        private static Touch _instance;

        public static Touch GetInstance()
        {
            if (_instance == null)
                _instance = new Touch();
            return _instance;
        }

        private Touch()
        {

        }

        protected override void Run()
        {
            if(_args.Length < 1)
            {
                Game.Terminal.WriteLine("touch: missing operand");
                Kill();
                return;
            }
            if(_args.Length > 1)
            {
                Game.Terminal.WriteLine("touch: too many operands");
                Kill();
                return;
            }

            World.World.GetInstance().Player.ConnectedComp.FileSystem.CurrentDir.AddFile(new Files.File(_args[0], "", Files.FileType.Text));
            Kill();
            return;
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {

        }
    }
}
