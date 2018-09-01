using System;

namespace TerminalGame.Programs
{
    class Cat
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;

        public static void Execute(string filename = null)
        {
            if (_player.ConnectedComputer.PlayerHasRoot)
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    if (_player.ConnectedComputer.FileSystem.TryFindFile(filename, false))
                    {
                        _player.ConnectedComputer.GenerateLog(_player.PlayersComputer, "accessed file", _player.ConnectedComputer.FileSystem.FindFile(filename, false));
                        _terminal.Write("\n" + _player.ConnectedComputer.FileSystem.FindFile(filename, false).Execute());
                        return;
                    }
                    _terminal.Write("\ncat: no such file or directory");
                    return;
                }
                _terminal.Write("\nUsage: cat [FILE]");
                return;
            }
            else
                _terminal.Write("\ncat: Permission denied");
        }
    }
}
