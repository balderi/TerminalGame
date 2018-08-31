using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.Scenes
{
    /// <summary>
    /// THis may or may not be used ever
    /// </summary>
    public interface IScene
    {
        // TODO: Is an interface the best way to do this?

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        void Update(GameTime gameTime);
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
