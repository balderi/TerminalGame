using Microsoft.Xna.Framework;
using TerminalGame.Screens;

namespace TerminalGame.States
{
    class GameLoadingState : State
    {
        TerminalGame Game;

        private static GameLoadingState _instance;
        public static GameLoadingState GetInstance()
        {
            if (_instance == null)
                _instance = new GameLoadingState();
            return _instance;
        }

        private GameLoadingState()
        {
            _name = "gameLoading";
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize(GraphicsDeviceManager graphics, Screen initialScreen, Game game)
        {
            Game = game as TerminalGame;
            base.Initialize(graphics, initialScreen, Game);
            AddState("gameRunning", GameRunningState.GetInstance());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void OnStateChange(object sender, StateChangeEventArgs e)
        {
            base.OnStateChange(sender, e);
            CurrentScreen.SwitchOn();
        }
    }
}
