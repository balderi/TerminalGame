using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Files;

namespace TerminalGame.Programs
{
    class Cd : Program
    {
        private static Cd _instance;

        public static Cd GetInstance()
        {
            if (_instance == null)
                _instance = new Cd();
            return _instance;
        }

        private Cd()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: cd");
                Kill();
                return;
            }
            if(_args.Length < 1)
            {
                //TODO: Usage cd
                Game.Terminal.WriteLine("Too few arguments: cd");
                Kill();
                return;
            }
            if (_args[0] == "-")
            {
                Player.GetInstance().ConnectedComp.FileSystem.ChangeCurrentDir(Player.GetInstance().ConnectedComp.FileSystem.LastDir);
                Kill();
                return;
            }
            if (_args[0].Contains('/'))
            {
                try
                {
                    Player.GetInstance().ConnectedComp.FileSystem.ChangeCurrentDirFromPath(_args[0]);
                    Kill();
                    return;
                }
                catch(Exception e)
                {
                    Game.Terminal.WriteLine(e.Message);
                }
            }
            if(Player.GetInstance().ConnectedComp.FileSystem.TryFindFile(_args[0], out File f))
            {
                if (f.FileType == FileType.Directory)
                    Player.GetInstance().ConnectedComp.FileSystem.ChangeCurrentDir(f);
                else
                    Game.Terminal.WriteLine($"cd: {_args[0]}: Not a directory");
            }
            else
            {
                Game.Terminal.WriteLine($"cd: {_args[0]}: no such file or directory");
            }
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
