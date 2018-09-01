namespace TerminalGame.Programs
{
    class Ls
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;

        public static void Execute(string variant)
        {
            if (_player.ConnectedComputer.PlayerHasRoot)
            {
                _terminal.Write(_player.ConnectedComputer.FileSystem.ListFiles());
            }
            else
                _terminal.Write("\n" + variant + ": Permission denied");
        }
    }
}
