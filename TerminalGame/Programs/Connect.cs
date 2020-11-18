using System;
using System.Text.RegularExpressions;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.Programs
{
    class Connect : Program
    {
        private string[] _connection;
        private int _counter;
        private bool _success;

        private static Connect _instance;

        public static Connect GetInstance()
        {
            if (_instance == null)
                _instance = new Connect();
            return _instance;
        }

        private Connect()
        {
            
        }

        public override void Kill()
        {
            if (_success)
            {
                if (!MusicManager.GetInstance().IsSongPlaying("hackLoop"))
                   MusicManager.GetInstance().ChangeSong("hackLoop", 0.1f);
            }
            else
                MusicManager.GetInstance().FadeIn(0.01f);

            base.Kill();
        }

        protected override void Run()
        {
            _success = false;
            _isKill = false;
            if (_args.Length < 1)
            {
                Game.Terminal.WriteLine("Usage: connect [ IP ]");
                Kill();
                return;
            }
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: connect");
                Kill();
                return;
            }
            if(!MusicManager.GetInstance().IsSongPlaying("hackLoop"))
                MusicManager.GetInstance().FadeOut(0.005f);
            Console.WriteLine("Attempting connection to host with IP {0}", _args[0]);
            if(_args[0] == World.World.GetInstance().Player.ConnectedComp.IP)
            {
                Game.Terminal.WriteLine("You are already connected to this host");
                Kill();
                return;
            }
            if (!Regex.Match(_args[0], @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+").Success)
            {
                Game.Terminal.WriteLine("connect: Invalid IP");
                Kill();
                return;
            }
            _connection = new string[] { "Connecting to " + _args[0], ".", ".", ".",
                "Connected to " + _args[0], "Error: no response from host", "Error: Host does not exist",
                "You are already connected to this host" };
            _timer.AutoReset = true;
            _timer.Interval = 500;
            _timer.Enabled = true;
            _counter = 1;
            Game.Terminal.WriteLine(_connection[0]);
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            if(_isKill)
            {
                Console.WriteLine("command: Execution halted!");
                Game.Terminal.WriteLine("^C");
                _timer.Stop();
                _isKill = false;
                return;
            }

            if(_counter < 4)
                Game.Terminal.Write(_connection[_counter]);
            else
            {
                foreach(Computer c in World.World.GetInstance().Computers)
                {
                    if (c.IP == _args[0])
                    {
                        if(c.IsPlayerConnected)
                        {
                            Game.Terminal.WriteLine(_connection[7]);
                            Kill();
                            _timer.Stop();
                            return;
                        }
                        if(c.Connect())
                        {
                            Console.WriteLine("Connection established to {1}@{0}", c.IP, c.GetPublicName());
                            _success = true;
                            Game.Terminal.WriteLine(_connection[4]);
                            Kill();
                            _timer.Stop();
                            return;
                        }
                        Game.Terminal.WriteLine(_connection[5]);
                        Kill();
                        _timer.Stop();
                        return;
                    }
                }
                Game.Terminal.WriteLine(_connection[6]);
                Kill();
                _timer.Stop();
                return;
            }
            _counter++;
        }
    }
}
