using System;
using System.Threading;
using TerminalGame.Computers;
using TerminalGame.Utilities;

namespace TerminalGame.Programs
{
    class Connect
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static Computer _playerComp = Player.GetInstance().PlayersComputer;
        private static UI.Modules.Terminal _terminal = _os.Terminal;
        private static string[] _textToWrite;
        private static string _ip;

        public static void Execute(string ip = null)
        {
            if (!String.IsNullOrEmpty(ip))
            {
                _terminal.BlockInput();
                _ip = ip;

                if (ip == _player.ConnectedComputer.IP || ip == _player.ConnectedComputer.Name)
                {
                    _terminal.Write("\nYou are already connected to " + _player.ConnectedComputer.Name + "@" + _player.ConnectedComputer.IP);
                    _terminal.UnblockInput();
                    return;
                }

                _textToWrite = new string[]
                {
                    "\nEstablishing connection to " + ip,
                    ".",
                    ".",
                    "."
                };

                _os.NetworkMap.IsActive = false;
                bool conn = false;

                for (int i = 0; i < _textToWrite.Length; i++)
                {
                    if (_textToWrite[i].Contains("\n"))
                        _terminal.Write(_textToWrite[i]);
                    else
                        _terminal.WritePartialLine(_textToWrite[i]);
                    Thread.Sleep((int)(500 * _playerComp.Speed));
                }

                foreach (Computer c in Computers.Computers.GetInstance().ComputerList)
                {
                    if (_ip == c.IP || _ip == c.Name)
                    {
                        conn = true;
                        c.Connect(false);
                        _terminal.Write("\nConnection established");

                        GameManager.GetInstance().SetIntensity(1);
                    }
                }

                if (!conn)
                {
                    _terminal.Write("\nCould not connect to " + _ip);

                    GameManager.GetInstance().ResetIntensity();
                }

                _os.NetworkMap.IsActive = true;
                _terminal.UnblockInput();
            }
            else
            {
                _terminal.Write("\nUsage: connect [IP]");
                return;
            }
        }
    }
}
