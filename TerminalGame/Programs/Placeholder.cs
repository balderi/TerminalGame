using System;
using System.Threading;

namespace TerminalGame.Programs
{
    class Placeholder
    {
        static UI.Modules.Terminal terminal = OS.OS.GetInstance().Terminal;

        public static void Execute()
        {
            terminal.Write("\nI'm sorry Dave, I'm afraid I can't do that.");
            return;
        }
    }
}
