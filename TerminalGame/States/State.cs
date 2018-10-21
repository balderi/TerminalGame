using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.States
{
    public abstract class State
    {
        /// <summary>
        /// Go to next state via a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Next state</returns>
        public virtual State Next(GameState state)
        {
            return Next();
        }

        /// <summary>
        /// Go to next state
        /// </summary>
        /// <returns>Next state</returns>
        public virtual State Next()
        {
            return this;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
