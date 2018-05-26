using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.States
{
    public abstract class State
    {
        // TODO: Create a State for each game state
        public virtual State Next(Keys key)
        {
            return Next();
        }

        public virtual State Next()
        {
            return this;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
