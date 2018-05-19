using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI
{
    /// <summary>
    /// Base UI component class
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Update component
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Draw component
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
