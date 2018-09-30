using System;
using System.Threading;
using TerminalGame.Computers;
using TerminalGame.Utilities;

namespace TerminalGame.Programs
{
    class SSHNuke
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static Computer _playerComp = Player.GetInstance().PlayersComputer;
        private static UI.Modules.Terminal _terminal = _os.Terminal;
        private static string[] _textToWrite;

        public static int Execute()
        {
            _terminal.BlockInput();
            _player.PlayersComputer.FileSystem.ChangeDir("/");
            _player.PlayersComputer.FileSystem.ChangeDir("bin");
            if (_player.PlayersComputer.FileSystem.TryFindFile("sshnuke", false))
            {
                if(_player.ConnectedComputer != _player.PlayersComputer)
                    GameManager.GetInstance().UpIntensity();

                _terminal.Write("\nConnecting to " + _player.ConnectedComputer.IP + ":ssh ");
                _textToWrite = new string[]
                {
                ".",
                ".",
                ".",
                " successful.",
                "\nAttempting to exploit SSHv1 CRC32 ",
                ".",
                ".",
                ".",
                " successful."
                };

                for(int i = 0; i<_textToWrite.Length; i++)
                {
                    if(i == 3)
                    {
                        if (!_player.ConnectedComputer.CheckPortOpen(22))
                        {
                            Thread.Sleep((int)(500 * _playerComp.Speed));
                            _terminal.WritePartialLine(" failed.");
                            _terminal.Write("\nsshnuke: error: port 22 not open");
                            _terminal.UnblockInput();
                            return 1;
                        }
                    }
                    if (_textToWrite[i].Contains("\n"))
                        _terminal.Write(_textToWrite[i]);
                    else
                        _terminal.WritePartialLine(_textToWrite[i]);
                    Thread.Sleep((int)(1000 * _playerComp.Speed));
                }

                _terminal.Write("\nResetting root password to \"password\".");
                Thread.Sleep(500);
                _player.ConnectedComputer.ChangePassword("password");
                _player.ConnectedComputer.GetRoot();
                _player.ConnectedComputer.Connect();
                _terminal.Write("\nSystem open: Access level <9>");
                _terminal.UnblockInput();
                _player.PlayersComputer.FileSystem.ChangeDir("/");
                return 0;
            }
            else
            {
                _terminal.Write("\nThe program \'sshnuke\' is currently not installed");
                _terminal.UnblockInput();
                _player.PlayersComputer.FileSystem.ChangeDir("/");
                return 1;
            }
        }
    }
}
