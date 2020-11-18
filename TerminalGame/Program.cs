using System;

namespace TerminalGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var game = new TerminalGame())
#pragma warning restore IDE0063 // Use simple 'using' statement
                game.Run();
        }
    }
}
