using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.States.Screens;

namespace TerminalGame.States
{
    public class StateMachine
    {
        #region fields
        protected GraphicsDeviceManager _graphics;
        protected Game _game;
        #endregion

        #region properties
        public State CurrentState { get; private set; }
        #endregion

        private static StateMachine _instance;

        public static StateMachine GetInstance()
        {
            if (_instance == null)
                _instance = new StateMachine();
            return _instance;
        }

        private StateMachine()
        {

        }

        public void Initialize(State initialState, GraphicsDeviceManager graphics, Screen screen, Game game)
        {
            CurrentState = initialState;
            _graphics = graphics;
            _game = game;
            CurrentState.Initialize(_graphics, screen, _game);
        }

        public void ChangeState(string state, Screen screen)
        {
            if (CurrentState.TryGetNextState(state, out State retval))
            {
                Console.WriteLine("Changing state to: " + state);
                CurrentState = retval;
                CurrentState.Initialize(_graphics, screen, _game);
                return;
            }
            Console.WriteLine("Could not change state to: " + state);
        }

        public void Update(GameTime gameTime)
        {
            CurrentState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            CurrentState.Draw(gameTime);
        }
    }
}
