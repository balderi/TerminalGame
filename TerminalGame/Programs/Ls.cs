namespace TerminalGame.Programs
{
    class Ls
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;

        public static void Execute(string variant)
        {
            if (player.ConnectedComputer.PlayerHasRoot)
            {
                terminal.Write(player.ConnectedComputer.FileSystem.ListFiles());
            }
            else
                terminal.Write("\n" + variant + ": Permission denied");
        }
    }
}
