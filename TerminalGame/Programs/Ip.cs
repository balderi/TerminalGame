using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Programs
{
    class Ip : Program
    {

        private static Ip _instance;

        public static Ip GetInstance()
        {
            if (_instance == null)
                _instance = new Ip();
            return _instance;
        }

        private Ip()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length < 1)
            {
                Game.Terminal.WriteLine("Usage: ip [ OBJECT ] { OPTIONS }");
                Kill();
                return;
            }

            // for ease of adding more ip objects later, if needed
            switch(_args[0])
            {
                case "addr":
                case "address":
                    {
                        if(_args[1] == "show")
                        {
                            Game.Terminal.WriteLine(Player.GetInstance().ConnectedComp.IP);
                            Kill();
                            return;
                        }
                        if(_args[1] == "help")
                        {
                            Game.Terminal.WriteLine("Usage: ip address [ show ]");
                            Kill();
                            return;
                        }
                        Game.Terminal.WriteLine($"Command \"{_args[1]}\" is unknown, try \"ip address help\".");
                        Kill();
                        return;
                    }
                case "help":
                    {
                        Game.Terminal.WriteLine("Usage: ip [ OPTIONS ] OBJECT { COMMAND | help }\nwhere  OBJECT := { address }\n       OPTIONS := { show }");
                        Kill();
                        return;
                    }
                default:
                    {
                        Game.Terminal.WriteLine($"Object \"{_args[0]}\" is unknown, try \" ip help\".");
                        Kill();
                        return;
                    }
            }
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
