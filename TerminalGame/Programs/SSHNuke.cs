﻿using System;
using System.Threading;

namespace TerminalGame.Programs
{
    class SSHNuke
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;
        static Timer timer;
        static int count;
        static string[] textToWrite;

        public static void Execute()
        {
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
                " successful.\nAttempting to exploit SSHv1 CRC32 ",
                ".",
                ".",
                ".",
                " successful.",
                "\nResetting root password to \"password\".",
                "\nSystem open: Access level <9>"
                };
                timer = new Timer(SSHNukeWriter, autoEvent, 1000, 333);
            }
            else
            {
                terminal.Write("\nThe program \'sshnuke\' is currently not installed");
            }
            player.PlayersComputer.FileSystem.ChangeDir("/");
        }

        private static void SSHNukeWriter(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            terminal.Write(textToWrite[count++]);
            if (count == textToWrite.Length)
            {
                Player.GetInstance().ConnectedComputer.GetRoot();
                Player.GetInstance().ConnectedComputer.Connect();
                count = 0;
                autoEvent.Set();
                timer.Dispose();
            }
        }
    }
}