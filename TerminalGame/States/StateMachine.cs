using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.States
{
    public enum GameState
    {
        MainMenu,
        SettingsMenu,
        LoadMenu,
        SaveMenu,
        GameLoading,
        NewGame,
        GameRunning,
        GameOver,
    };

    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State startState)
        {
            CurrentState = startState;
        }

        public StateMachine Transition(GameState state)
        {
            CurrentState = CurrentState.Next(state);
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
    }
}
