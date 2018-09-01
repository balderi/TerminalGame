using System;
using System.Threading;

namespace TerminalGame.Programs
{
    class Connect
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;
        private static Timer _timer;
        private static int _count;
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

                var autoEvent = new AutoResetEvent(false);
                _count = 0;

                _textToWrite = new string[]
                {
                    "\nEstablishing connection to " + ip,
                    ".",
                    ".",
                    ".§"
                };

                _os.NetworkMap.IsActive = false;

                _timer = new Timer(ConnectionWriter, autoEvent, 100, 500);
            }
            else
            {
                _terminal.Write("\nUsage: connect [IP]");
                return;
            }
        }

        private static void ConnectionWriter(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            _terminal.Write(_textToWrite[_count++]);
            if (_count == _textToWrite.Length)
            {
                bool conn = false;
                foreach (Computers.Computer c in Computers.Computers.computerList)
                {
                    if (_ip == c.IP || _ip == c.Name)
                    {
                        conn = true;
                        c.Connect(false);
                        _terminal.Write("\nConnection established");
                        _os.NetworkMap.IsActive = true;
                    }
                }
                if(!conn)
                    _terminal.Write("\nCould not connect to " + _ip);
                _terminal.UnblockInput();
                _count = 0;
                autoEvent.Set();
                _timer.Dispose();
            }
        }
    }
}
