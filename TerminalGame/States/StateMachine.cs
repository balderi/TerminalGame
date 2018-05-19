using Microsoft.Xna.Framework;
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

        public void DrawState(GameTime gameTime)
        {
            CurrentState.Draw(gameTime);
        }

        public void DrawTransition(GameTime gameTime, Keys key)
        {
            Transition(key);
            DrawState(gameTime);
        }
    }
}
