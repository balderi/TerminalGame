using System;

namespace TerminalGame.Programs
{
    class Cd
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;

        public static void Execute(string folder = null)
        {
            if (_player.ConnectedComputer.PlayerHasRoot)
            {
                if (!String.IsNullOrEmpty(folder))
                {
                    if (_player.ConnectedComputer.FileSystem.TryFindFile(folder, true))
                    {
                        _player.ConnectedComputer.FileSystem.ChangeDir(folder);
                        return;
                    }
                    _terminal.Write("\ncd: " + folder + ": no such file or directory");
                    return;
                }
                _terminal.Write("\nUsage: cd [DIRECTORY]");
                return;
            }
            else
                _terminal.Write("\ncd: Permission denied");
        }
    }
}
