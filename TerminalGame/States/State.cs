using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.States
{
    /// <summary>
    /// Yeah... this all needs to be redone
    /// </summary>
    public abstract class State
    {
        // TODO: Create a State for each game state

        /// <summary>
        /// Go to next state via a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Next state</returns>
        public virtual State Next(Keys key)
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
