using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.States.Screens;
using System.Timers;

namespace TerminalGame.States
{
    class SplashState : State
    {
        private Timer _timer;

        public static SplashState GetInstance()
        {
            if (_instance == null)
                _instance = new SplashState();
            return _instance;
        }

        private static SplashState _instance;

        private SplashState()
        {
            _timer = new Timer(2000);
            _timer.Elapsed += Timer_Tick;
            _name = "splash";
        }

        public override void Initialize(GraphicsDeviceManager graphics, Screen screen, Game game)
        {
            base.Initialize(graphics, screen, game);
            AddState("gameRunning", GameRunningState.GetInstance());
            _timer.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void OnStateChange(StateChangeEventArgs e)
        {
            base.OnStateChange(e);
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            StateMachine.GetInstance().ChangeState("gameRunning", new GameRunningScreen(_game));
        }
    }
}
