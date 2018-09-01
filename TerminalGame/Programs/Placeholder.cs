using System;
using System.Threading;

namespace TerminalGame.Programs
{
    class Placeholder
    {
        private static UI.Modules.Terminal _terminal = OS.OS.GetInstance().Terminal;

        public static void Execute()
        {
            _terminal.Write("\nI'm sorry Dave, I'm afraid I can't do that.");
            return;
        }
    }
}
