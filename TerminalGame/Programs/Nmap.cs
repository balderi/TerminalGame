using System;
using System.Threading;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Nmap
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static Computer _playerComp = Player.GetInstance().PlayersComputer;
        private static Computer _remoteComp;
        private static UI.Modules.Terminal _terminal = _os.Terminal;
        private static string[] _textToWrite;
        private static Random _rnd;

        public static int Execute(string ip = null)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            // TODO: Make latency dependent on distance from player computer. Maybe make this a property of computers.
            int latency = _rnd.Next(10, 99);
            _terminal.BlockInput();
            //_player.PlayersComputer.FileSystem.ChangeDir("/");
            //_player.PlayersComputer.FileSystem.ChangeDir("bin");
            if (_player.PlayersComputer.FileSystem.TryFindFile("nmap", false))
            {
                //_player.PlayersComputer.FileSystem.ChangeDir("/");
                if (String.IsNullOrEmpty(ip))
                {
                    _remoteComp = _player.ConnectedComputer;
                    ip = _remoteComp.Name;
                }

                _terminal.Write("\nStarting Nmap scan for " + ip);
                Thread.Sleep(20 * latency);
                if (HostExists(ip))
                {
                    _textToWrite = new string[]
                    {
                        "\nNmap scan report for " + _remoteComp.Name + " (" + _remoteComp.IP + ")",
                        "\nHost is up (0." + latency + "s latency).",
                        "\nNot shown: " + (1000 - _remoteComp.OpenPorts.Count) + " filtered ports",
                        "\n",
                        "\nPORT   STATE  SERVICE",
                        "\n",
                        "\n",
                        "\nDevice type: " + _remoteComp.ComputerType,
                        "\nNo exact OS matches for host (test conditions non-ideal).",
                        "\n",
                        "\nNmap done: 1 IP address (1 host up) scanned",
                        "\n",
                    };

                    for (int i = 0; i < _textToWrite.Length; i++)
                    {
                        if (i == 5)
                        {
                            foreach (var port in _remoteComp.OpenPorts)
                            {
                                _terminal.Write("\n" + PrettyPrintPorts(port.Key, port.Value));
                                Thread.Sleep(5 * latency);
                            }
                        }
                        else
                        {
                            if (_textToWrite[i].Contains("\n"))
                                _terminal.Write(_textToWrite[i]);
                            else
                                _terminal.WritePartialLine(_textToWrite[i]);
                            Thread.Sleep((int)(latency * _playerComp.Speed));
                        }
                    }
                }
                else
                {
                    Thread.Sleep(20 * latency);
                    _terminal.Write("\nNo response from host " + ip + ".");
                }

                _terminal.UnblockInput();
                return 0;
            }
            else
            {
                _terminal.Write("\nThe program \'nmap\' is currently not installed");
                _terminal.UnblockInput();
                //_player.PlayersComputer.FileSystem.ChangeDir("/");
                return 1;
            }
        }

        static bool HostExists(string ip)
        {
            foreach(var comp in Computers.Computers.GetInstance().ComputerList)
            {
                if (comp.IP == ip || comp.Name == ip)
                {
                    _remoteComp = comp;
                    return true;
                }
            }
            return false;
        }

        static string PrettyPrintPorts(int port, string service)
        {
            string retval = "";
            retval += port + Spaces(port.ToString(), 7);
            retval += "open" + Spaces("open", 7);
            retval += service.ToLower();
            return retval;
        }

        static string Spaces(string text, int length)
        {
            string retval = "";
            for(int i = 0; i < (length - text.Length); i++)
            {
                retval += " ";
            }
            return retval;
        }
    }
}
