using System;

namespace TerminalGame.Programs
{
    class Cd
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;

        public static void Execute(string folder = null)
        {
            if (player.ConnectedComputer.PlayerHasRoot)
            {
                if (!String.IsNullOrEmpty(folder))
                {
                    if (player.ConnectedComputer.FileSystem.TryFindFile(folder, true))
                    {
                        player.ConnectedComputer.FileSystem.ChangeDir(folder);
                        return;
                    }
                    terminal.Write("\ncd: " + folder + ": no such file or directory");
                    return;
                }
                terminal.Write("\nUsage: cd [DIRECTORY]");
                return;
            }
            else
                terminal.Write("\ncd: Permission denied");
        }
    }
}
