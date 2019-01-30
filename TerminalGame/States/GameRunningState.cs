using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.Screens;
using TerminalGame.Utils;

namespace TerminalGame.States
{
    class GameRunningState : State
    {
        private static GameRunningState _instance;
        public static GameRunningState GetInstance()
        {
            if (_instance == null)
                _instance = new GameRunningState();
            return _instance;
        }

        private GameRunningState()
        {
            _name = "gameRunning";
        }

        public override void Initialize(GraphicsDeviceManager graphics, Screen screen, Game game)
        {
            base.Initialize(graphics, screen, game);
            AddState("mainMenu", MainMenuState.GetInstance());
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void OnStateChange(object sender, StateChangeEventArgs e)
        {
            base.OnStateChange(this, e);
            CurrentScreen.SwitchOn();
        }
    }
}
