using TerminalGame.Utilities;

namespace TerminalGame.Programs
{
    static class Disconnect
    {
        private static Player _player = Player.GetInstance();
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;

        public static void Execute()
        {
            if (!_player.PlayersComputer.IsPlayerConnected)
            {
                GameManager.GetInstance().ResetIntensity();
                _player.ConnectedComputer.GenerateLog(_player.PlayersComputer, "disconnected");
                _player.ConnectedComputer.Disconnect(false);
                _terminal.Write("\nDisconnected");
                return;
            }
            _terminal.Write("\nCannot disconnect from gateway");
        }
    }
}
