using System;
using System.Threading;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class SSHNuke
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static Computer playerComp = Player.GetInstance().PlayersComputer;
        static UI.Modules.Terminal terminal = os.Terminal;
        static Timer timer;
        static int count;
        static string[] textToWrite;

        public static int Execute()
        {
            terminal.BlockInput();
            player.PlayersComputer.FileSystem.ChangeDir("/");
            player.PlayersComputer.FileSystem.ChangeDir("bin");
            if (player.PlayersComputer.FileSystem.TryFindFile("sshnuke", false))
            {
                var autoEvent = new AutoResetEvent(false);
                terminal.Write("\nConnecting to " + Player.GetInstance().ConnectedComputer.IP + ":ssh ");
                count = 0;
                textToWrite = new string[]
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

                for(int i = 0; i<textToWrite.Length; i++)
                {
                    if (textToWrite[i].Contains("\n"))
                        terminal.Write(textToWrite[i]);
                    else
                        terminal.WritePartialLine(textToWrite[i]);
                    Thread.Sleep((int)(1000 * playerComp.Speed));
                }

                terminal.Write("\nResetting root password to \"password\".");
                Thread.Sleep(500);
                player.ConnectedComputer.ChangePassword("password");
                player.ConnectedComputer.GetRoot();
                player.ConnectedComputer.Connect();
                terminal.Write("\nSystem open: Access level <9>");
                terminal.UnblockInput();
                player.PlayersComputer.FileSystem.ChangeDir("/");
                return 0;
            }
            else
            {
                terminal.Write("\nThe program \'sshnuke\' is currently not installed");
                terminal.UnblockInput();
                player.PlayersComputer.FileSystem.ChangeDir("/");
                return 1;
            }
        }
    }
}
