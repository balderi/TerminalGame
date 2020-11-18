using System;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Nmap : Program
    {
        private Random _rnd;
        private Computer _target;
        private string[] _textToWrite;
        private int _latency, _counter;

        private static Nmap _instance;

        public static Nmap GetInstance()
        {
            if (_instance == null)
                _instance = new Nmap();
            return _instance;
        }

        private Nmap()
        {
            _rnd = new Random();
        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length < 1)
            {
                _target = World.World.GetInstance().Player.ConnectedComp;
            }
            else
            {
                if (World.World.GetInstance().TryGetComputerByIp(_args[0], out var computer))
                {
                    _target = computer;
                }
                else
                {
                    Game.Terminal.WriteLine("Error: Host does not exist");
                    Kill();
                    return;
                }
            }

            Game.Terminal.WriteLine($"Starting Nmap scan for {_target.IP}");

            _counter = 0;
            _latency = _rnd.Next(10, 99);

            _textToWrite = new string[]
                    {
                        $"Nmap scan report for {_target.Name.Replace("§¤§", " ")} ({_target.IP})",
                        $"Host is up (0.{_latency}s latency).",
                        $"Not shown: {1000 - _target.OpenPorts.Count} filtered ports",
                        "",
                        "PORT   STATE  SERVICE",
                        "",
                        "",
                        $"Device type: " + _target.ComputerType,
                        "No exact OS matches for host (test conditions non-ideal).",
                        "",
                        "Nmap done: 1 IP address (1 host up) scanned",
                        "",
                    };

            _timer.AutoReset = true;
            _timer.Interval = 20 * _latency;
            _timer.Start();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            if (_isKill)
            {
                Console.WriteLine("command: Execution halted!");
                Game.Terminal.WriteLine("^C");
                _timer.Stop();
                _isKill = false;
                return;
            }

            if (_counter == _textToWrite.Length)
            {
                Kill();
                _timer.Stop();
                return;
            }

            if (_counter == 5)
            {
                foreach (var port in _target.OpenPorts)
                {
                    Game.Terminal.WriteLine(PrettyPrintPorts(port, ((Computers.Utils.KnownPorts)port).ToString()));
                }
            }

            Game.Terminal.WriteLine(_textToWrite[_counter++]);
            _timer.Interval = _latency;
        }

        private string PrettyPrintPorts(int port, string service)
        {
            string retval = "";
            retval += port + Spaces(port.ToString(), 7);
            retval += "open" + Spaces("open", 7);
            retval += service.ToLower();
            return retval;
        }

        private string Spaces(string text, int length)
        {
            string retval = "";
            for (int i = 0; i < (length - text.Length); i++)
            {
                retval += " ";
            }
            return retval;
        }
    }
}
