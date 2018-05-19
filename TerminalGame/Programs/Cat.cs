using System;

namespace TerminalGame.Programs
{
    class Cat
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;
        public static void Execute(string filename = null)
        {
            if (player.ConnectedComputer.PlayerHasRoot)
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    if (player.ConnectedComputer.FileSystem.TryFindFile(filename, false))
                    {
                        player.ConnectedComputer.GenerateLog(player.PlayersComputer, "accessed file", player.ConnectedComputer.FileSystem.FindFile(filename, false));
                        terminal.Write("\n" + player.ConnectedComputer.FileSystem.FindFile(filename, false).Execute());
                        return;
                    }
                    terminal.Write("\ncat: no such file or directory");
                    return;
                }
                terminal.Write("\nUsage: cat [FILE]");
                return;
            }
            else
                terminal.Write("\ncat: Permission denied");
        }
    }
}
