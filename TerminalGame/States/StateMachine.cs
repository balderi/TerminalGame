using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.States
{
    class StateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State startState)
        {
            CurrentState = startState;
        }

        public StateMachine Transition(Keys key)
        {
            CurrentState = CurrentState.Next(key);
            return this;
        }

        public void DrawState(SpriteBatch spriteBatch)
        {
            CurrentState.Draw(spriteBatch);
        }

        public void UpdateState(GameTime gameTime)
        {
            CurrentState.Update(gameTime);
        }

        public void DrawTransition(SpriteBatch spriteBatch, Keys key)
        {
            Transition(key);
            DrawState(spriteBatch);
        }
    }
}
