namespace TerminalGame.Programs
{
    static class Disconnect
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;

        public static void Execute()
        {
            if (!player.PlayersComputer.IsPlayerConnected)
            {
                player.ConnectedComputer.GenerateLog(player.PlayersComputer, "disconnected");
                player.ConnectedComputer.Disconnect(false);
                terminal.Write("\nDisconnected");
                return;
            }
            terminal.Write("\nCannot disconnect from gateway");
        }
    }
}
