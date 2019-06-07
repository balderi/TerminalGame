using Microsoft.Xna.Framework;

namespace TerminalGame.States
{
    public interface IState
    {
        bool TryGetNextState(string state, out State outState);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
